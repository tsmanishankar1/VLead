using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Renci.SshNet.Sftp;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

using Vis.VleadProcessV3.Models;

using Vis.VleadProcessV3.Repositories;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class BulkUploadMailController : ControllerBase
    {
        private readonly UnitWork _unitWork;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        public BulkUploadMailController(IConfiguration configuration,UnitWork unitWork, ApplicationDbContext _context)
        {
            _configuration = configuration;
            _unitWork = unitWork;
            this._context = _context;
        }
        [HttpPost]
        [Route("BulkUpload")]
        public async Task<IActionResult> SendMail(int JobId)
        {
            var host = _configuration.GetSection("Smtp").GetValue<string>("host");
            var port = _configuration.GetSection("Smtp").GetValue<int>("port");
            var userName = _configuration.GetSection("Smtp").GetValue<string>("userName");
            var password = _configuration.GetSection("Smtp").GetValue<string>("password");
            var Ssl = _configuration.GetSection("Smtp").GetValue<bool>("SSL");
            var defaultCredential = _configuration.GetSection("Smtp").GetValue<bool>("defaultCredential");
            var clientid = _unitWork.JobOrder.Where(x => x.Id == JobId).FirstOrDefault();
            if (clientid != null)
            {
                var checkcustomerforbunchmail = _unitWork.Customer.Where(x => x.IsDeleted == false && x.Id == clientid.ClientId).FirstOrDefault();
                if (checkcustomerforbunchmail.BunchMail == false)
                {
                    try
                    {
                        var jobOrderDetails = new JobOrder();
                        var pwtDetails = new ProcessWorkFlowTran();
                        var queryDetails = "";
                        var body = "";
                        var body1 = "";
                        var body2 = "";
                        string ftpUrl;
                        ftpUrl = _configuration.GetSection("AllocationService").GetValue<string>("downloadURL");//ConfigurationManager.AppSettings["downloadURL"];
                        if (JobId != 0)
                        {
                            jobOrderDetails = _unitWork.JobOrder.FirstOrDefault(x => x.Id == JobId);
                            pwtDetails = null;
                        }
                        //if (jobOrderDetails == null || jobOrderDetails.ClientOrderId == null)
                        if (jobOrderDetails == null || (jobOrderDetails.ClientSalesPerson == null && jobOrderDetails.ClientOrderId == null))
                        {
                            Log savelog = new Log();
                            savelog.Module = "Completed Job";
                            savelog.JobId = JobId;
                            savelog.Description = "Mail Not Sent because job order details or client order id is null";
                            savelog.Type = "Error";
                            savelog.CreatedUtc = DateTime.UtcNow;
                            _unitWork.Log.Add(savelog);
                            _unitWork.SaveChanges();
                            return Ok();
                        }
                        var emailIdAndName = new CustomerContact();
                        var getclientorderdetailsJO = new JobOrder();
                        var getclientorderdetails = new ClientOrder();
                        var CusOutputMailInfo = new Customer();
                        var jobStatusDescription = new JobStatus();
                        var mailNotify = new List<MailNotificationDetail>();
                        string jobstatusdes = "";

                        if (jobOrderDetails.CcemailId != null && jobOrderDetails.ClientOrderId == null)
                        {
                            emailIdAndName = _unitWork.CustomerContact.FirstOrDefault(x => x.CustomerId == jobOrderDetails.ClientId && x.Email == jobOrderDetails.CcemailId && x.IsDeleted == false);

                            getclientorderdetails = _unitWork.ClientOrder.FirstOrDefault(x => x.OrderId == jobOrderDetails.ClientId);

                            getclientorderdetailsJO = _unitWork.JobOrder.FirstOrDefault(x => x.ClientId == jobOrderDetails.ClientId);

                            CusOutputMailInfo = _unitWork.Customer.FirstOrDefault(x => x.Id == jobOrderDetails.ClientId);

                            jobStatusDescription = _unitWork.JobStatus.Where(x => x.Id == jobOrderDetails.JobStatusId).FirstOrDefault();
                            mailNotify = await _unitWork.MailNotificationDetail.WhereAsync(x => x.CompareMailId == emailIdAndName.Id && x.IsDeleted == false && (x.Task == "All" || x.Task == "Completed") && (x.ClientId == jobOrderDetails.ClientId));
                        }
                        else
                        {
                            getclientorderdetails = _unitWork.ClientOrder.FirstOrDefault(x => x.OrderId == jobOrderDetails.ClientOrderId);

                            getclientorderdetailsJO = _unitWork.JobOrder.FirstOrDefault(x => x.ClientId == jobOrderDetails.ClientId);

                            emailIdAndName = _unitWork.CustomerContact.FirstOrDefault(x => x.CustomerId == getclientorderdetails.ClientId && x.Email == getclientorderdetails.CcemailId && x.IsDeleted == false);

                            CusOutputMailInfo = _unitWork.Customer.FirstOrDefault(x => x.Id == getclientorderdetails.ClientId);

                            jobStatusDescription = _unitWork.JobStatus.Where(x => x.Id == getclientorderdetails.JobStatus).FirstOrDefault();

                            mailNotify = await _unitWork.MailNotificationDetail.WhereAsync(x => x.CompareMailId == emailIdAndName.Id && x.IsDeleted == false && (x.Task == "All" || x.Task == "Completed") && (x.ClientId == getclientorderdetails.ClientId));
                        }

                        //client output type check for mail with attachment


                        //var jobStatusDescription = _unitWork.JobStatus.Where(x => x.Id == getclientorderdetails.JobStatus).FirstOrDefault().JobStatusDescription;
                        var pwmRecord = _unitWork.ProcessWorkFlowMaster.Where(x => x.JobId == JobId).FirstOrDefault();
                        var pwtRecord = _unitWork.ProcessWorkFlowTran.Where(x => x.Wfmid == pwmRecord.Id).OrderByDescending(x => x.Id).FirstOrDefault();
                        var message = new MailMessage();
                        message.To.Add(new MailAddress(emailIdAndName.Email));



                        if (mailNotify.Count() >= 1)
                        {
                            foreach (var item in mailNotify)
                            {
                                var customerContactDetails = _unitWork.CustomerContact.FirstOrDefault(x => x.Id == item.CcmailId && x.IsDeleted == false);
                                if (customerContactDetails != null)
                                {
                                    message.CC.Add(new MailAddress(customerContactDetails.Email));
                                }
                            }
                        }
                        message.From = new MailAddress("visalert@vleadservices.com");
                        if (jobStatusDescription.JobStatusDescription == "Fresh")
                        {
                            //jobStatusDescription.JobStatusDescription = "New";
                            jobstatusdes = "New";
                        }
                        else if (jobStatusDescription.JobStatusDescription == "Fresh-Rush")
                        {
                            jobstatusdes = "New-Rush";
                        }
                        if (CusOutputMailInfo != null)
                        {
                            if (CusOutputMailInfo.OutputType == "Email")
                            {
                                if (jobOrderDetails.FileName != null && pwtRecord.FileUploadPath != null)
                                {
                                    var FilePath = _configuration.GetSection("AllocationService").GetValue<string>("jobFilesPathAutoUpload");//ConfigurationManager.AppSettings["jobFilesPathAutoUpload"];
                                    string route = FilePath + pwtRecord.FileUploadPath;//Web config path
                                    string[] fileEntries = Directory.GetFiles(route);
                                    Attachment attachment;
                                    //ContentDisposition disposition;
                                    long SizeallAttach = 0;
                                    foreach (string fileName in fileEntries)
                                    {
                                        var count = fileEntries.Count();
                                        string name;
                                        new FileExtensionContentTypeProvider().TryGetContentType(fileName, out name);
                                       
                                        attachment = new Attachment(fileName, mediaType: name??MediaTypeNames.Application.Octet);
                                        ContentDisposition disposition = attachment.ContentDisposition;
                                        disposition.DispositionType = DispositionTypeNames.Attachment;
                                        disposition.Size = new FileInfo(fileName).Length;
                                        SizeallAttach += disposition.Size;
                                        if (disposition.Size < 25000000)
                                        {
                                            message.Attachments.Add(attachment);
                                        }
                                    }
                                    if (SizeallAttach > 25000000)
                                    {
                                        message.Attachments.Dispose();
                                    }
                                }
                            }
                        }

                        Log addlog = new Log();
                        if ((jobstatusdes == "New" || jobstatusdes == "New-Rush") && (pwtDetails == null))
                        {
                            message.Subject = "Order Completed By VLead - (" + jobOrderDetails.FileName + ")";
                            message.IsBodyHtml = true;
                            var AttachCount = message.Attachments.Count;

                            body = "Hi " + emailIdAndName.ContactName + ",<br/><br/>" +
                                    "This e-mail is to keep you informed that the order  <b>" + jobOrderDetails.FileName + "</b> is completed and uploaded through VIS. Please download it from the portal or from this link <a href='" + ftpUrl + "api/Allocation/DownloadZipFile?path=" + pwtRecord.FileUploadPath + "'>Click here to Download.</a> Review and let us know your feedback." +
                                    "If the order needs to be revised, you may send us revisions through VIS itself.<br/><br/>" +
                                    "If you have any queries regarding this order, please send an e-mail to  support@vleadservices.com  or Call Toll-free - 18009571698 and Skype - (214)3479935.<br/><br/>";

                            body1 = "<b>Note:</b> Completed file size is more than 20 MB.  So, we unable to proceed with the attachments.  Please click the hyperlink to download the files.";

                            body2 = "<b>Note:</b> Please DO NOT respond to this e-mail or use this e-mail address since it is not monitored.<br/><br/>" +
                                    "Thank you for your interest in VLead Services.<br/><br/>" +
                                    "Many thanks for your business.<br/>" +
                                    "Support Team,<br/>" +
                                    "VLEAD DESIGN SERVICES PVT. LTD";
                            if (AttachCount > 0)
                            {
                                body = string.Concat(body, body2);
                                message.Body = string.Format(body, "Venu", "Test");
                            }
                            else
                            {
                                body = string.Concat(body, body1, body2);
                                message.Body = string.Format(body, "Venu", "Test");
                            }

                        
                            using (var smtp = new SmtpClient(host, port))
                            {
                                var checkmailtriger = _context.JobOrders.FirstOrDefault(x => x.Id == JobId);
                                if (checkmailtriger.IsMailTriggered == false || checkmailtriger.IsMailTriggered == null)
                                {
                                    smtp.UseDefaultCredentials = defaultCredential;
                                    smtp.Credentials = new NetworkCredential(userName, password);
                                    smtp.EnableSsl = Ssl;
                                    await smtp.SendMailAsync(message);
                                    checkmailtriger.IsMailTriggered = true;
                                    _context.Entry(checkmailtriger).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }
                            }
                            addlog.Module = "Completed Job";
                        }
                        else if ((jobStatusDescription.JobStatusDescription == "Revision") && (pwtDetails == null))
                        {
                            var AttachCount = message.Attachments.Count;

                            body = "Hi " + emailIdAndName.ContactName + ",<br/><br/>" +
                                    "This e-mail is to keep you informed that the order <b>" + jobOrderDetails.FileName + "</b> is revised and uploaded through VIS. Please download it from the portal or from this link <a href='" + ftpUrl + "api/Allocation/DownloadZipFile?path=" + pwtRecord.FileUploadPath + "'>Click here to Download.</a> Review and let us know your feedback.<br/><br/>" +
                                    "If you have any queries regarding this order, please send an e-mail to  support@vleadservices.com  or Call Toll-free - 18009571698 and Skype - (214)3479935.<br/><br/>";

                            body1 = "<b>Note:</b> Completed file size is more than 20 MB.  So, we unable to proceed with the attachments.  Please click the hyperlink to download the files.";

                            body2 = "<b>Note:</b> Please DO NOT respond to this e-mail or use this e-mail address since it is not monitored.<br/><br/>" +
                                    "Thank you for your interest in VLead Services.<br/><br/>" +
                                    "Many thanks for your business.<br/>" +
                                    "Support Team,<br/>" +
                                    "VLEAD DESIGN SERVICES PVT. LTD";
                            if (AttachCount > 0)
                            {
                                body = string.Concat(body, body2);
                                message.Body = string.Format(body, "Venu", "Test");
                            }
                            else
                            {
                                body = string.Concat(body, body1, body2);
                                message.Body = string.Format(body, "Venu", "Test");
                            }
                            message.Subject = "Order Revised By VLead  - (" + jobOrderDetails.FileName + ")";
                            // message.Body = string.Format(body, "Venu", "Test");
                            message.IsBodyHtml = true;
                            using (var smtp = new SmtpClient(host, port))
                            {
                                //await smtp.SendMailAsync(message);////
                                // ApplicationDbContext _context = new ApplicationDbContext(_configuration);
                                var checkmailtriger = _context.JobOrders.FirstOrDefault(x => x.Id == JobId);
                                if (checkmailtriger.IsMailTriggered == false || checkmailtriger.IsMailTriggered == null)
                                {
                                    smtp.UseDefaultCredentials = defaultCredential;
                                    smtp.Credentials = new NetworkCredential(userName, password);
                                    smtp.EnableSsl = Ssl;
                                    await smtp.SendMailAsync(message);////
                                    checkmailtriger.IsMailTriggered = true;
                                    _context.Entry(checkmailtriger).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }
                            }
                         
                        }
                        else if ((jobStatusDescription.JobStatusDescription == "Quotation") && (pwtDetails == null))
                        {
                            var AttachCount = message.Attachments.Count;
                            body = "Hi " + emailIdAndName.ContactName + ",<br/><br/>" +
                                //"We have completed your quote request for <b>" + getclientorderdetails.FileName + "</b> and the details are sent to you through your VIS account." +
                                "We have completed your quote request for <b>" + getclientorderdetailsJO.FileName + "</b> and the details are sent to you through your VIS account." +//

                                    "If you agree to the pricing, please approve the order by logging into your VIS account.<br/><br/>" +
                                    "If you have any queries regarding this order, please send an e-mail to  support@vleadservices.com  or Call Toll-free - 18009571698 and Skype - (214)3479935.<br/><br/>";

                            body1 = "<b>Note:</b> Completed file size is more than 20 MB.  So, we unable to proceed with the attachments.  Please click the hyperlink to download the files.";

                            body2 = "<b>Note:</b> Please DO NOT respond to this e-mail or use this e-mail address since it is not monitored.<br/><br/>" +
                                    "Thank you for your interest in VLead Services.<br/><br/>" +
                                    "Many thanks for your business.<br/>" +
                                    "Support Team,<br/>" +
                                    "VLEAD DESIGN SERVICES PVT. LTD";
                            if (AttachCount > 0)
                            {
                                body = string.Concat(body, body2);
                                message.Body = string.Format(body, "Venu", "Test");
                            }
                            else
                            {
                                body = string.Concat(body, body1, body2);
                                message.Body = string.Format(body, "Venu", "Test");
                            }
                            //message.Subject = "Quote Request Completed - (" + getclientorderdetails.FileName + ")";
                            message.Subject = "Quote Request Completed - (" + getclientorderdetailsJO.FileName + ")";//

                            // message.Body = string.Format(body, "Venu", "Test");
                            message.IsBodyHtml = true;
                            using (var smtp = new SmtpClient(host,port))
                            {
                                //await smtp.SendMailAsync(message);////
                             
                                var checkmailtriger = _context.JobOrders.FirstOrDefault(x => x.Id == JobId);
                                if (checkmailtriger.IsMailTriggered == false || checkmailtriger.IsMailTriggered == null)
                                {
                                    smtp.UseDefaultCredentials = defaultCredential;
                                    smtp.Credentials = new NetworkCredential(userName, password);
                                    smtp.EnableSsl = Ssl;
                                    await smtp.SendMailAsync(message);////
                                    checkmailtriger.IsMailTriggered = true;
                                    _context.Entry(checkmailtriger).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }
                            }

                            addlog.Module = "Quote Job";
                        }
                        addlog.Description = "Email sent successfully. To: " + message.To + ", CC : " + message.CC;

                        if (jobOrderDetails.ClientId != null || jobOrderDetails.ClientId != 0)
                        {
                            addlog.ClientId = jobOrderDetails.ClientId;
                        }
                        else
                        {
                            //addlog.ClientId = getclientorderdetails.ClientId;
                            addlog.ClientId = getclientorderdetailsJO.ClientId;//
                        }
                        addlog.JobId = JobId;
                        addlog.Type = "Success";
                        addlog.CreatedUtc = DateTime.UtcNow;
                        _unitWork.Log.Add(addlog);
                        _unitWork.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        Log savelog = new Log();
                        savelog.Module = "Completed Job";
                        savelog.JobId = JobId;
                        savelog.Description = "Mail Not Sent" + "-" + ex.Message.ToString() + "-" + "Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source + "string";
                        savelog.Type = "Error";
                        savelog.CreatedUtc = DateTime.UtcNow;
                        _unitWork.Log.Add(savelog);
                        _unitWork.SaveChanges();
                    }
                }
            }
            var updateisautoupload = _unitWork.JobOrder.Where(x => x.Id == JobId).FirstOrDefault();
            if (updateisautoupload != null)
            {
                if (updateisautoupload.JobStatusId == 10)
                {
                    updateisautoupload.IsAutoUploadCount = 5;
                    _unitWork.JobOrder.Update(updateisautoupload);
                    _unitWork.SaveChanges();
                }
            }
            return Ok();
        }
    }
}

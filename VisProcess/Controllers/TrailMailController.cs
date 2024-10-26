using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Net.Mail;

using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class TrailMailController : ControllerBase
    {
        private readonly UnitWork _unitWork;
        private readonly ProcedureWork _procedureWork;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        public TrailMailController(IConfiguration configuration,UnitWork unitWork, ProcedureWork procedureWork, ApplicationDbContext db)
        {
            _configuration = configuration;
            _unitWork = unitWork;
            _procedureWork = procedureWork;
            _db = db;
        }
        [HttpGet]
        [Route("SendMail")]
        public async Task<IActionResult> SendSummaryMailForTrailCompletedJobs()
        {
            try
            {
                var host = _configuration.GetSection("Smtp").GetValue<string>("host");
                var port = _configuration.GetSection("Smtp").GetValue<int>("port");
                var userName = _configuration.GetSection("Smtp").GetValue<string>("userName");
                var password = _configuration.GetSection("Smtp").GetValue<string>("password");
                var Ssl = _configuration.GetSection("Smtp").GetValue<bool>("SSL");
                var defaultCredential = _configuration.GetSection("Smtp").GetValue<bool>("defaultCredential");
               
                var TrailScopeCustomer = _unitWork.CustomerVsScope.Where(x => x.CustomerJobType == "Trial" && x.IsActive == true).Select(x => x.CustomerId).Distinct().ToList();
                    string joinval = string.Join(",", TrailScopeCustomer);
                    var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@customerId", joinval)
                    };
                    var TrailMailSumamryDetails = _procedureWork.ExecStoredProcedure<TrailJobsCompletedCountbyClientwise_Result>("TrailJobsCompletedCountbyClientwise @customerId", sqlpara);

                    //
                    if (TrailMailSumamryDetails.Any())
                    {
                        foreach (var val in TrailMailSumamryDetails)
                        {
                            var salesPerson = _db.CustomerVsSalesEmployees.Where(x => x.CustomerId == val.ClientId && x.IsActive == true).Select(x => new { EmpId = x.EmployeeId, CusName = x.CustomerName }).FirstOrDefault();
                            var empMail = _db.Employees.Where(x => x.EmployeeId == salesPerson.EmpId).Select(x => new { x.Email, x.EmployeeName }).FirstOrDefault();
                            var message = new MailMessage();
                           message.To.Add(new MailAddress(empMail.Email));
                     
                        message.From = new MailAddress("visalert@vleadservices.com");
                            var body = "Hi " + empMail.EmployeeName + ",<br/><br/>" +
                                       "This e-mail is to keep you informed that <b>" + val.Filescount + "</b> order(s) are completed through VIS for the customer “<b>" + salesPerson.CusName + "</b>”. Please review and upload the completed orders to the Customer, through VIS application." +
                                       "If you have any queries regarding this order, please send an e-mail to support@vleadservices.com <br/><br/>" +
                                       "<b>Note:</b> Please DO NOT respond to this e-mail or use this e-mail address since it is not monitored.<br/><br/>" +
                                       "Thank you for your interest with VLead Services.<br/><br/>" +
                                       "Many thanks for your business.<br/>" +
                                       "Support Team,<br/>" +
                                       "VLead Design Services.";
                            message.Subject = "Trial Order Completed By VLead - (" + val.Filescount + ")";
                            message.Body = string.Format(body, "Venu", "Test");
                            message.IsBodyHtml = true;
                        using (var smtp = new SmtpClient(host, port))
                        {

                            smtp.UseDefaultCredentials = defaultCredential;
                            smtp.Credentials = new NetworkCredential(userName, password);
                            smtp.EnableSsl = Ssl;
                            await smtp.SendMailAsync(message);
                        }
                    }
                    }
               // }
            }
            catch (Exception ex)
            {
                Log savelog = new Log();
                savelog.Module = "Completed Job for approval";
                savelog.Description = "Mail Not Sent" + "-" + ex.Message.ToString() + "-" + "Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source + "string";
                savelog.Type = "Error";
                savelog.CreatedUtc = DateTime.UtcNow;
                _unitWork.Log.Add(savelog);
                _unitWork.SaveChanges();
            }
            return Ok();
        }
    }
}

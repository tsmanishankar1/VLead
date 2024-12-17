using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class FtpFilesService
    {
        private readonly ApplicationDbContext _context;
        private readonly UnitWork _unitWork;
        private readonly TableWork _tableWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public FtpFilesService(ApplicationDbContext context, UnitWork unitWork, TableWork tableWork, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _context = context;
            _unitWork = unitWork;
            _tableWork = tableWork;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public FileMovement FtpFileAttachments(FtpFileServiceModel ftpFileService)
        {
            var fileMovement = new FileMovement();
            fileMovement.Message = "Converted Successfully";
            var jobFileName = "";
            var folder = ftpFileService.filePath;
            var filesToCopy = Directory.GetFiles(folder).Select(Path.GetFileNameWithoutExtension).ToList();
            try
            {
                foreach (var fileName in filesToCopy)
                {
                    var insertJobOrdersingle = false;

                    if (ftpFileService.jobStatusId == 7 || ftpFileService.jobStatusId == 8 || ftpFileService.jobStatusId == 9 || ftpFileService.jobStatusId == 10)
                    {
                        //var CheckFileNameExist = _unitWork.JobOrder.Where(x => x.FileName == item.FileName && x.ClientId == item.ClientId && x.DepartmentId == item.DepartmentId && x.JobStatusId != 10 && x.IsDeleted == false).ToList();    //------------------old code changed for division id Job Order---------
                        var CheckFileNameExist = _unitWork.JobOrder.Where(x => x.FileName == fileName && x.ClientId == ftpFileService.clientId && x.DepartmentId == ftpFileService.departmentId && x.DivisionId == ftpFileService.divisionId && x.JobStatusId != 10 && x.IsDeleted == false).ToList();//----------------division id Job Order--------------------------------
                        if (ftpFileService.jobStatusId == 10)
                        {
                            //var CheckFileNameQuoteExist = _unitWork.JobOrder.Where(x => x.FileName == item.FileName && x.ClientId == item.ClientId && x.DepartmentId == item.DepartmentId && x.JobStatusId == 10 && x.IsDeleted == false).ToList(); //------------------old code changed for division id Job Order---------
                            var CheckFileNameQuoteExist = _unitWork.JobOrder.Where(x => x.FileName == fileName && x.ClientId == ftpFileService.clientId && x.DepartmentId == ftpFileService.departmentId && x.DivisionId == ftpFileService.divisionId && x.JobStatusId == 10 && x.IsDeleted == false).ToList();//----------------division id Job Order--------------------------------
                            if (CheckFileNameQuoteExist.Count != 0)
                            {
                                jobFileName = jobFileName + "," + CheckFileNameQuoteExist.FirstOrDefault().FileName;
                                continue;

                            }
                        }
                        if (CheckFileNameExist.Count != 0)
                        {
                            jobFileName = jobFileName + "," + CheckFileNameExist.FirstOrDefault().FileName;
                            continue;

                        }
                    }

                    int jobstatusidsingle = ftpFileService.jobStatusId;
                    var JobstatusDetailsingle = _unitWork.JobStatus.Where(x => x.Id == jobstatusidsingle).FirstOrDefault();
                    int jobstatusgroupidsingle = JobstatusDetailsingle.JobStatusGroupId;
                    DateTime jobdatesingle = DateTime.UtcNow;
                    var transactiontypeDetailsingle = _unitWork.TransactionType.Where(x => x.JobStatusGroupId == jobstatusgroupidsingle).FirstOrDefault();
                    int transactionidsingle = transactiontypeDetailsingle.Id;
                    var freshJobExistssingle = _unitWork.JobOrder.Where(x => x.ClientId == ftpFileService.clientId && x.FileName.Equals(fileName)).OrderBy(x => x.Id).ToList();
                    var freshJobExistscountsingle = freshJobExistssingle.Count > 0;

                    string monthsingle = jobdatesingle.ToString("MM");
                    string daysingle = jobdatesingle.ToString("dd");
                    string Jobnosingle = "";
                    JobOrder jobOrder = new JobOrder();

                gotostmt_voucher:

                    var VoucherControlDetailsingle = _unitWork.VoucherControl.OrderByDescending(v => v.Id).FirstOrDefault(x => x.TransactionId == transactionidsingle && (x.EffectiveFrom <= jobdatesingle && x.EffectiveTo >= jobdatesingle) && x.DepartmentId == ftpFileService.departmentId && x.IsDeleted == false && x.IsActive == true);
                    if (VoucherControlDetailsingle == null)
                    {
                        fileMovement.Message = "VoucherControl is Missing";
                    }
                    string Prefixsingle = VoucherControlDetailsingle.Prefix.ToString();
                    long VoucherNosingle = VoucherControlDetailsingle.Voucherno + 1;
                    Jobnosingle = Prefixsingle + monthsingle + daysingle + "/" + VoucherNosingle;
                    var filesName = _unitWork.JobOrder.Where(x => x.FileName == fileName && x.ClientId == ftpFileService.clientId && x.DepartmentId == ftpFileService.departmentId && x.JobStatusId == jobstatusidsingle && x.IsDeleted == false).ToList();
                    if (jobstatusidsingle == 1 || jobstatusidsingle == 2 || jobstatusidsingle == 5)
                    {
                        Jobnosingle = Prefixsingle + monthsingle + daysingle + "/" + VoucherNosingle + "/R" + (filesName.Count == 0 ? 1 : (filesName.Count + 1));
                    }
                    var voucherControl = _unitWork.VoucherControl.Find(VoucherControlDetailsingle.Id);
                    voucherControl.Voucherno = VoucherNosingle;
                    voucherControl.UpdatedBy = jobOrder.CreatedBy;
                    voucherControl.UpdatedUtc = DateTime.UtcNow;
                    _unitWork.VoucherControl.Update(voucherControl);
                    _unitWork.SaveChanges();

                    int checkjobid = _unitWork.JobOrder.Count(x => x.JobId == Jobnosingle);
                    if (checkjobid == 1)
                    {
                        goto gotostmt_voucher;
                    }
                    else
                    {
                        jobOrder.TransactionId = transactionidsingle;
                        jobOrder.JobId = Jobnosingle;
                        jobOrder.JobDate = DateTime.Now;
                        jobOrder.JobDescription = "Convert From Client Order";
                        jobOrder.JobStatusId = ftpFileService.jobStatusId;
                        jobOrder.DepartmentId = ftpFileService.departmentId;
                        jobOrder.DivisionId = ftpFileService.divisionId;//----------------division id Job Order---------------------------
                        jobOrder.EmployeeId = ftpFileService.employeeId;
                        jobOrder.ClientId = ftpFileService.clientId;
                        jobOrder.FileReceivedDate = DateTime.UtcNow;
                        jobOrder.FileName = fileName;
                        jobOrder.FileInwardTypeId = 1;
                        jobOrder.DivisionId = ftpFileService.divisionId;//----------------division id Job Order---------------------------
                        jobOrder.IsDeleted = false;
                        jobOrder.CreatedBy = ftpFileService.employeeId;
                        jobOrder.CustomerJobType = "Live";
                        var freshJobExistsfordatesingle = _unitWork.JobOrder.Where(x => x.ClientId == ftpFileService.clientId && x.FileName == fileName && x.DepartmentId == ftpFileService.departmentId && x.IsDeleted == false).FirstOrDefault();
                        if (freshJobExistsfordatesingle != null)
                        {
                            if (freshJobExistsfordatesingle.IsCancelled == true || freshJobExistsfordatesingle.StatusId == 3 || freshJobExistsfordatesingle.StatusId == 5 || freshJobExistsfordatesingle.StatusId == 13)
                            {
                                jobFileName = jobFileName + "," + freshJobExistsfordatesingle.FileName;
                                continue;
                            }
                            else if (freshJobExistsfordatesingle.DateofUpload != null && freshJobExistsfordatesingle.DateofUpload < jobOrder.JobDate)
                            {
                                if (freshJobExistscountsingle)
                                {
                                    var parentjobidsingle = freshJobExistssingle.First();
                                    var referencejobidsingle = freshJobExistssingle.Last();
                                    jobOrder.ParentJobId = parentjobidsingle.JobId;
                                    jobOrder.JobReferenceId = referencejobidsingle.JobId;
                                }
                                insertJobOrdersingle = true;
                            }
                            else
                            {
                                continue;

                            }
                        }
                        else
                        {
                            //----------------division id Job Order---------------------------
                            if (ftpFileService.divisionId != 0)
                            {
                                insertJobOrdersingle = true;
                            }
                            //----------------division id Job Order---------------------------
                        }
                    }
                    if (insertJobOrdersingle)
                    {
                        _unitWork.JobOrder.Add(jobOrder);
                        //_unitWork.SaveChanges();
                        ProcessWorkFlowMaster processWorkFlowMaster = new ProcessWorkFlowMaster();
                        processWorkFlowMaster.TransactionId = jobOrder.TransactionId;
                        processWorkFlowMaster.Job = jobOrder;
                        // processWorkFlowMaster.JobId = _tableWork.JobOrderRepository.GetLastRecord(s => s.Id).Id;
                        processWorkFlowMaster.CustomerId = jobOrder.ClientId.Value;
                        processWorkFlowMaster.DepartmentId = jobOrder.DepartmentId;
                        processWorkFlowMaster.IsDeleted = jobOrder.IsDeleted;
                        processWorkFlowMaster.CreatedBy = jobOrder.CreatedBy;
                        processWorkFlowMaster.CreatedUtc = DateTime.UtcNow;
                        _unitWork.ProcessWorkFlowMaster.Add(processWorkFlowMaster);
                        // _unitWork.SaveChanges();
                        ProcessWorkFlowTran processWorkFlowTran = new ProcessWorkFlowTran();
                        processWorkFlowTran.Wfm = processWorkFlowMaster;
                        //  processWorkFlowTran.Wfmid = _tableWork.ProcessWorkFlowMasterRepository.GetLastRecord(s => s.Id).Id;
                        processWorkFlowTran.JidNavigation = jobOrder;
                        // processWorkFlowTran.Jid = _tableWork.JobOrderRepository.GetLastRecord(s => s.Id).Id;
                        processWorkFlowTran.StatusId = 1;
                        processWorkFlowTran.DepartmentId = jobOrder.DepartmentId;
                        processWorkFlowTran.PreviousProcessId = null;
                        processWorkFlowTran.ProcessId = 2;
                        processWorkFlowTran.JobStatusId = jobOrder.JobStatusId;
                        processWorkFlowTran.IsDeleted = processWorkFlowMaster.IsDeleted;
                        processWorkFlowTran.CreatedBy = jobOrder.CreatedBy;
                        processWorkFlowTran.CreatedUtc = DateTime.UtcNow;
                        processWorkFlowTran.IsActive = true;
                        //processWorkFlowTran.FileUploadPath = clientOrder.FileUploadPath;
                        _unitWork.ProcessWorkFlowTran.Add(processWorkFlowTran);
                        _unitWork.SaveChanges();
                        if (jobstatusidsingle == 1 || jobstatusidsingle == 2 || jobstatusidsingle == 5)
                        {
                            if ((filesName.Count + 1) >= 3)
                            {
                                RevisionEmailNotification(ftpFileService.clientId, fileName, (filesName.Count + 1));
                            }
                        }
                        JobOrderExtension joe = new JobOrderExtension();
                        joe.JidNavigation = jobOrder;
                        // joe.Jid = _tableWork.JobOrderRepository.GetLastRecord(s => s.Id).Id;
                        joe.Date = DateTime.UtcNow;
                        joe.Wft = processWorkFlowTran;
                        // joe.Wftid = _tableWork.ProcessWorkFlowTranRepository.GetLastRecord(s => s.Id).Id;
                        _unitWork.JobOrderExtension.Add(joe);
                        //Flaten Table inserted...
                        var customerShortName = _unitWork.Customer.Where(x => x.Id == ftpFileService.clientId).FirstOrDefault();
                        var dept = _unitWork.Department.Where(x => x.Id == ftpFileService.departmentId).FirstOrDefault();
                        var jobStatus = _unitWork.JobStatus.Where(x => x.Id == ftpFileService.jobStatusId).FirstOrDefault();
                        var ccIntake = _unitWork.Employee.Where(x => x.EmployeeId == jobOrder.CreatedBy).FirstOrDefault();
                        var empname = _unitWork.Employee.Where(x => x.EmployeeId == ftpFileService.employeeId).FirstOrDefault();
                        var classiid = _unitWork.Customer.FirstOrDefault(x => x.Id == ftpFileService.clientId).CustomerClassificationId;
                        var classification = _unitWork.CustomerClassification.FirstOrDefault(x => x.Id == classiid).Description;
                        TrayDetailMovement trayDetailMovement = new TrayDetailMovement();
                        trayDetailMovement.JobDate = DateTime.UtcNow;
                        trayDetailMovement.FileName = fileName;
                        trayDetailMovement.ClientId = ftpFileService.clientId;
                        trayDetailMovement.ClientShortName = customerShortName.ShortName;
                        trayDetailMovement.Department = dept.Description;
                        trayDetailMovement.JobStatus = jobStatus.JobStatusDescription;
                        trayDetailMovement.CustomerJobType = jobOrder.CustomerJobType;
                        trayDetailMovement.CurrentTray = "Production Allocation";
                        trayDetailMovement.CurrentStatus = "Pending";
                        trayDetailMovement.CcintakenBy = ccIntake.EmployeeCode + "-" + ccIntake.EmployeeName;
                        trayDetailMovement.IsDeleted = false;
                        trayDetailMovement.CreatedBy = ftpFileService.createdBy;
                        trayDetailMovement.CreatedUtc = DateTime.UtcNow;
                        trayDetailMovement.Jid = _tableWork.JobOrderRepository.GetLastRecord(s => s.Id).Id;
                        trayDetailMovement.JobId = jobOrder.JobId;
                        trayDetailMovement.DepartmentId = ftpFileService.departmentId;
                        trayDetailMovement.EmployeeId = ftpFileService.employeeId;
                        trayDetailMovement.EmployeeName = empname.EmployeeCode + "-" + empname.EmployeeName;
                        trayDetailMovement.StatusId = 1;
                        trayDetailMovement.ClassificationId = classiid;
                        trayDetailMovement.Classification = classification;
                        trayDetailMovement.CcintakenDate = DateTime.UtcNow;
                        _unitWork.TrayDetailMovement.Add(trayDetailMovement);
                        _unitWork.SaveChanges();

                        int year = jobOrder.FileReceivedDate.Year;
                        var Getjobsummary = _unitWork.JobSummary.Where(x => x.ClientId == ftpFileService.clientId && x.MonthYear == monthsingle + "-" + year).ToList();
                        var GetLastRecord = Getjobsummary.LastOrDefault();
                        if (Getjobsummary.Count == 0)
                        {
                            JobSummary savejobsummary = new JobSummary();
                            savejobsummary.ClientId = ftpFileService.clientId;
                            savejobsummary.MonthYear = monthsingle + "-" + year;
                            if (GetLastRecord == null)
                            {
                                savejobsummary.Pending = 1;
                                savejobsummary.TotalFiles = 1;
                            }
                            _unitWork.JobSummary.Add(savejobsummary);
                            _unitWork.SaveChanges();
                        }
                        else
                        {
                            var Updatejobsummary = _unitWork.JobSummary.Where(x => x.Id == GetLastRecord.Id).FirstOrDefault();
                            Updatejobsummary.Pending = GetLastRecord.Pending + 1;
                            Updatejobsummary.TotalFiles = GetLastRecord.TotalFiles + 1;
                            _unitWork.JobSummary.Update(Updatejobsummary);
                            _unitWork.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Log addlog = new Log();
                addlog.Module = "Converted job";
                addlog.Description = "Error Msg: " + ex.Message + ", Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source;
                addlog.Type = "Error Occ";
                addlog.CreatedUtc = DateTime.UtcNow;
                _context.Logs.Add(addlog);
                _context.SaveChanges();

            }
            finally
            {
                if (jobFileName != "")
                {
                    fileMovement.JobId = "File Name Already Exist!" + jobFileName;
                }
            }

            return fileMovement;
        }

        public Object RevisionEmailNotification(int customerId, string fileName, int revisionCount)
        {
            var message = new MailMessage();
            string alert = "Mail sent successfully";

            try
            {
                var customerDetails = _context.Customers
                   .Where(c => c.Id == customerId && !c.IsDeleted)
                   .Select(c => new
                   {
                       CustomerName = c.Name
                   })
                   .FirstOrDefault();

                var managerDetails = _context.CustomerVsManagers
                     .Where(cm => cm.CustomerId == customerId && cm.IsDeleted == false)
                     .Join(
                         _context.Employees,
                         cm => cm.ManagerId,
                         e => e.EmployeeId,
                         (cm, e) => new
                         {
                             ManagerId = cm.ManagerId,
                             ManagerEmail = e.Email,
                             ManagerName = e.EmployeeName
                         }
                     )
                     .FirstOrDefault();

                message.IsBodyHtml = true;
                var host = _configuration.GetSection("Smtp").GetValue<string>("host");
                var port = _configuration.GetSection("Smtp").GetValue<int>("port");
                var userName = _configuration.GetSection("Smtp").GetValue<string>("userName");
                var password = _configuration.GetSection("Smtp").GetValue<string>("password");
                var Ssl = _configuration.GetSection("Smtp").GetValue<bool>("SSL");
                var defaultCredential = _configuration.GetSection("Smtp").GetValue<bool>("defaultCredential");

                message.From = new MailAddress("visalert@vleadservices.com");
                message.To.Add(new MailAddress(managerDetails.ManagerEmail));

                if (revisionCount == 3)
                {
                    message.Subject = $"Alert: Third Revision for {fileName} for Client {customerDetails.CustomerName}";
                }
                else
                {
                    message.Subject = $"Alert: {revisionCount}th Revision for {fileName} for Client {customerDetails.CustomerName}";
                }

                if (revisionCount == 3)
                {
                    message.Body = $"Hi {managerDetails.ManagerName},<br/><br/>" +
                                   $"This is to inform you that the file {fileName} for Client {customerDetails.CustomerName} has undergone its third revision.<br/><br/>" +
                                   $"Please review the revisions and take necessary action.<br/><br/>" +
                                   $"Best regards,<br/>" +
                                   $"VIS Team";
                }
                else
                {
                    message.Body = $"Hi {managerDetails.ManagerName},<br/><br/>" +
                                   $"This is to inform you that the file {fileName} for Client {customerDetails.CustomerName} has undergone its {(revisionCount)}th revision.<br/><br/>" +
                                   $"Please review the revisions and take necessary action.<br/><br/>" +
                                   $"Best regards,<br/>" +
                                   $"VIS Team";
                }

                using (var smtp = new SmtpClient(host, port))
                {
                    smtp.UseDefaultCredentials = defaultCredential;
                    smtp.Credentials = new NetworkCredential(userName, password);
                    smtp.EnableSsl = Ssl;
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                alert = "There is a problems while sending mail! Try Again";
            }

            return alert;
        }

    }
}

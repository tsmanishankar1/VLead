using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var jobFileName = "";
            var folder = GetFolderPathForCustomer(ftpFileService.clientId);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var filesToCopy = Directory.GetFiles(folder).Select(Path.GetFileNameWithoutExtension).ToList();
            try
            {
                foreach (var fileName in filesToCopy)
                {
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
                    var voucherControl = _unitWork.VoucherControl.Find(VoucherControlDetailsingle.Id);
                    voucherControl.Voucherno = VoucherNosingle;
                    voucherControl.UpdatedBy = jobOrder.CreatedBy;
                    voucherControl.UpdatedUtc = DateTime.UtcNow;
                    _unitWork.VoucherControl.Update(voucherControl);
                    _unitWork.SaveChanges();

                    jobOrder.DepartmentId = ftpFileService.departmentId;
                    jobOrder.ClientId = ftpFileService.clientId;
                    jobOrder.EmployeeId = ftpFileService.employeeId;
                    jobOrder.JobStatusId = ftpFileService.jobStatusId;
                    jobOrder.JobId = Jobnosingle;
                    jobOrder.FileName = fileName;
                    jobOrder.TransactionId = transactionidsingle;
                    jobOrder.FileReceivedDate = DateTime.UtcNow;
                    jobOrder.JobDate = DateTime.UtcNow;
                    jobOrder.IsDeleted = false;
                    jobOrder.CustomerJobType = "Live";
                    jobOrder.CreatedBy = ftpFileService.createdBy;
                    jobOrder.DivisionId = ftpFileService.divisionId;
                    _unitWork.JobOrder.Add(jobOrder);
                    _unitWork.SaveChanges();

                    ProcessWorkFlowMaster processWorkFlowMaster = new ProcessWorkFlowMaster();
                    processWorkFlowMaster.TransactionId = jobOrder.TransactionId;
                    processWorkFlowMaster.JobId = _tableWork.JobOrderRepository.GetLastRecord(s => s.Id).Id;
                    processWorkFlowMaster.CustomerId = jobOrder.ClientId.Value;
                    processWorkFlowMaster.DepartmentId = jobOrder.DepartmentId;
                    processWorkFlowMaster.IsDeleted = jobOrder.IsDeleted;
                    processWorkFlowMaster.CreatedBy = jobOrder.CreatedBy;
                    processWorkFlowMaster.CreatedUtc = DateTime.UtcNow;
                    _unitWork.ProcessWorkFlowMaster.Add(processWorkFlowMaster);
                    _unitWork.SaveChanges();
                    ProcessWorkFlowTran processWorkFlowTran = new ProcessWorkFlowTran();
                    processWorkFlowTran.Wfmid = _tableWork.ProcessWorkFlowMasterRepository.GetLastRecord(s => s.Id).Id;
                    processWorkFlowTran.Jid = _tableWork.JobOrderRepository.GetLastRecord(s => s.Id).Id;
                    processWorkFlowTran.StatusId = _tableWork.ProcessWorkFlowTranRepository.GetLastRecord(s => s.Jid).StatusId;
                    processWorkFlowTran.DepartmentId = jobOrder.DepartmentId;
                    processWorkFlowTran.PreviousProcessId = _tableWork.ProcessWorkFlowTranRepository.GetLastRecord(s => s.Jid).PreviousProcessId;
                    processWorkFlowTran.ProcessId = _tableWork.ProcessWorkFlowTranRepository.GetLastRecord(s => s.Jid).ProcessId;
                    processWorkFlowTran.JobStatusId = jobOrder.JobStatusId;
                    processWorkFlowTran.IsDeleted = processWorkFlowMaster.IsDeleted;
                    processWorkFlowTran.CreatedBy = jobOrder.CreatedBy;
                    processWorkFlowTran.CreatedUtc = DateTime.UtcNow;
                    processWorkFlowTran.IsActive = true;
                    _unitWork.ProcessWorkFlowTran.Add(processWorkFlowTran);
                    _unitWork.SaveChanges();

                    JobOrderExtension joe = new JobOrderExtension();
                    joe.Jid = _tableWork.JobOrderRepository.GetLastRecord(s => s.Id).Id;
                    joe.Date = DateTime.UtcNow;
                    joe.Wftid = _tableWork.ProcessWorkFlowTranRepository.GetLastRecord(s => s.Id).Id;
                    _unitWork.JobOrderExtension.Add(joe);
                    _unitWork.SaveChanges();

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

            fileMovement.Message = "Converted Successfully";
            return fileMovement;
        }

        private string GetFolderPathForCustomer(int customerId)
        {
            var folderKey = $"FolderPaths:Customer{customerId}";
            var folderPath = _configuration.GetValue<string>(folderKey);
            return folderPath;
        }
    }
}

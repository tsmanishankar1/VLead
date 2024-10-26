using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class WorkflowService
    {
        private readonly ViewWork _viewWork;
        private readonly TableWork _tableWork;
        private readonly ApplicationDbContext _db;
        private readonly ApplicationDbContext _context;
        private readonly ApplicationDbContext Db;
        private readonly UnitWork _unitWork;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public WorkflowService(IConfiguration configuration, ViewWork viewWork, TableWork tableWork,
             UnitWork unitWork, ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _viewWork = viewWork;
            _tableWork = tableWork;
            _context = db;
            Db = db;
            _unitWork = unitWork;
            _configuration = configuration;
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public bool BulkClosureNextTraymovement()
        {
            var result = false;
            var statusDetails = _unitWork.Status.Where(x => x.Name == "Bulk Order Complete" || x.Name == "Attachment Missing").Select(x => x.Id);
            var pwt = _unitWork.ProcessWorkFlowTran.Where(x => statusDetails.Contains(x.StatusId) && x.IsActive == true).ToList();

            if (pwt.Count() >= 1)
            {
                foreach (var item in pwt)
                {
                    var str = "";
                    try
                    {
                        var jobOrder = _unitWork.JobOrder.Find(item.Jid); //check isdelete
                        var jobFilesLoc = _unitWork.JobFilesLocationMaster.FirstOrDefault(x => x.ProcessId == item.ProcessId && x.IsActive == true && x.ClientId == jobOrder.ClientId && x.DepartmentId == item.DepartmentId);
                        var path = jobFilesLoc.FtpfilePath + "\\" + jobOrder.FileName + "\\";
                        if (Directory.Exists(path))
                        { //checking folder
                            str = "step-1";
                            string[] file = Directory.GetFiles(path, "*.*");

                            if (file.Length >= 1 && file.Contains("thumbs.db") == false && file.Contains(".DS_Store") == false)
                            { //checking file
                                str += "step-3";
                                int? nxtPid = null;
                                int? currentPid = null;
                                var statusId = 0;
                                var processes = new ProcessWorkFlow();
                                var pwtRecords = _unitWork.ProcessWorkFlowTran.Where(x => x.Wfmid == item.Wfmid).ToList();
                                var employeasign = _unitWork.EmployeeAssign.Where(x => x.Jid == item.Jid && (x.Status == "Bulk Order Complete" || x.Status == "Attachment Missing")).FirstOrDefault();
                                if (employeasign != null)
                                {
                                    statusId = 2;
                                }

                                var process = _unitWork.ProcessWorkFlow.FirstOrDefault(x => x.DepartmentId == item.DepartmentId && x.CustomerId == jobOrder.ClientId && x.CurrentProcessId == item.ProcessId && x.StatusId == statusId && x.IsActive == true);
                                str += "step-4";
                                if (process == null)
                                {
                                    var process1 = _unitWork.ProcessWorkFlow.FirstOrDefault(x => x.DepartmentId == item.DepartmentId && x.CustomerId == null && x.CurrentProcessId == item.ProcessId && x.StatusId == statusId && x.IsActive == true);
                                    processes = process1;
                                    str += "step-5";
                                }
                                else
                                {
                                    processes = process;
                                    str += "step-6";
                                }
                                currentPid = processes.CurrentProcessId;
                                nxtPid = processes.NextProcessId;
                                statusId = processes.StatusId;
                                str += " StatusId: " + statusId;
                                str += "step-7";
                                var procTrans = new ProcessTransaction();
                                procTrans.OrderId = jobOrder.Id;
                                procTrans.IsClientOrder = 0;
                                procTrans.StatusId = 1;
                                procTrans.IsProcessWorkFlowTranInserted = 0;
                                procTrans.ProcessId = Convert.ToInt32(nxtPid);
                                procTrans.FakeProcessId = nxtPid;
                                procTrans.FakeStatusId = 1;
                                result = UploadFilesTest(path, procTrans);
                                str += "step-8";
                                item.IsActive = false;
                                item.StatusId = 1;
                                item.UpdatedUtc = DateTime.UtcNow;
                                _unitWork.ProcessWorkFlowTran.Update(item);

                                //06-13-2017 Check-n
                                if (employeasign != null)
                                {
                                    var processName = _unitWork.Process.FirstOrDefault(x => x.Id == currentPid).Name;
                                    var trayDetailMovementTran = _unitWork.TrayDetailMovementTran.Where(x => x.Wfmid == employeasign.Wfmid && x.EmployeeId == employeasign.EmployeeId && x.Process == processName && x.IsDeleted == false).OrderByDescending(x => x.Id).FirstOrDefault();

                                    employeasign.Status = "Completed";
                                    employeasign.IsActive = false;
                                    employeasign.UpdatedUtc = DateTime.UtcNow;
                                    _unitWork.EmployeeAssign.Update(employeasign);

                                    if (trayDetailMovementTran != null)
                                    {
                                        trayDetailMovementTran.Status = "Completed";
                                        trayDetailMovementTran.UpdatedUtc = DateTime.UtcNow;
                                        _unitWork.TrayDetailMovementTran.Update(trayDetailMovementTran);
                                    }
                                    var dept = _unitWork.Department.Where(x => x.Id == jobOrder.DepartmentId).FirstOrDefault(); //  
                                    var jobStatus = _unitWork.JobStatus.FirstOrDefault(x => x.Id == jobOrder.JobStatusId);
                                    var trayDetailMovement = _unitWork.TrayDetailMovement.Where(x => x.FileName == jobOrder.FileName && x.Department == dept.Description && x.ClientId == jobOrder.ClientId && x.CustomerJobType == jobOrder.CustomerJobType && x.JobStatus == jobStatus.JobStatusDescription && x.IsDeleted == false).OrderByDescending(x => x.Id).FirstOrDefault();//
                                    if (trayDetailMovement != null)
                                    {
                                        var currentTrayProcess = _unitWork.Process.FirstOrDefault(x => x.Id == currentPid).Name;
                                        var currentTryStatus = _unitWork.Process.FirstOrDefault(x => x.Id == nxtPid).Name;
                                        if (trayDetailMovement.CurrentTray == currentTrayProcess)
                                        {
                                            trayDetailMovement.CurrentTray = currentTryStatus;
                                            trayDetailMovement.CurrentStatus = "Pending";
                                            _unitWork.TrayDetailMovement.Update(trayDetailMovement);
                                        }
                                        if (trayDetailMovement.CurrentTray == currentTrayProcess)
                                        {

                                            trayDetailMovement.CurrentTray = currentTryStatus;
                                            trayDetailMovement.CurrentStatus = "Pending";
                                            _unitWork.TrayDetailMovement.Update(trayDetailMovement);
                                        }
                                    }
                                }

                                str += "step-9";

                                ProcessWorkFlowTran insertPWT = new ProcessWorkFlowTran();
                                insertPWT.Wfmid = item.Wfmid;
                                insertPWT.Jid = item.Jid;
                                insertPWT.DepartmentId = item.DepartmentId;
                                insertPWT.PreviousProcessId = currentPid;
                                insertPWT.ProcessId = nxtPid;
                                insertPWT.FileUploadPath = item.FileUploadPath;
                                var qualityStatusName = new ProcessWorkFlowTran();
                                var proofStatusName = new ProcessWorkFlowTran();

                                if (currentPid == 3)
                                {
                                    qualityStatusName = pwtRecords.Where(x => x.Wfmid == item.Wfmid && x.PreviousProcessId == 5 && x.ProcessId == 2 && (x.StatusId == 11 || x.StatusId == 26) && x.IsActive == false).OrderByDescending(x => x.Id).FirstOrDefault();
                                }
                                else if (currentPid == 5)
                                {
                                    proofStatusName = pwtRecords.Where(x => x.Wfmid == item.Wfmid && x.PreviousProcessId == 7 && x.ProcessId == 4 && (x.StatusId == 11 || x.StatusId == 26) && x.IsActive == false).OrderByDescending(x => x.Id).FirstOrDefault();
                                }
                                if (qualityStatusName != null)
                                {
                                    if (qualityStatusName.Id != 0)
                                    {
                                        insertPWT.ErrorStatus = "Corrected Job";
                                    }
                                }
                                if (proofStatusName != null)
                                {
                                    if (proofStatusName.Id != 0)
                                    {
                                        insertPWT.ErrorStatus = "Corrected Job";
                                    }
                                }

                                var employeeAssignDetails = _unitWork.EmployeeAssign.Where(x => x.Wfmid == item.Wfmid).OrderBy(x => x.Id).ToList();
                                var empAssignMinValue = employeeAssignDetails.GroupBy(x => new { x.Wftid, x.EmployeeId }).Select(grp => new
                                {
                                    Min = grp.Min(t => t.Id)
                                });
                                var empAssignMinValuesList = empAssignMinValue.ToList().Select(x => x.Min);

                                var dbemployees1 = new List<Employee>();
                                dbemployees1 = _unitWork.Employee.Where(x => x.IsDeleted == false).ToList();

                                var ViewGetEmployeesForPendingJobs = (from ea in employeeAssignDetails
                                                                      join emp in dbemployees1 on ea.EmployeeId equals emp.EmployeeId
                                                                      where empAssignMinValuesList.Contains(ea.Id)
                                                                      select new
                                                                      {
                                                                          ea.Id,
                                                                          ea.Wftid,
                                                                          ea.EmployeeId,
                                                                          ea.EstimatedTime,
                                                                          ea.Status,
                                                                          ea.DeviationTime,
                                                                          emp.EmployeeCode,
                                                                          emp.EmployeeName,
                                                                          ea.Wfmid,
                                                                          ea.ProcessId,
                                                                          ea.IsActive,
                                                                          ea.Remarks
                                                                      }).ToList();
                                var getEmployeesForPendingJobs = _viewWork.ViewGetEmployeesForPendingJobsRepository.Get(x => x.Wfmid == item.Wfmid && x.ProcessId == item.ProcessId && (x.Status == "Bulk Order Complete" || x.Status == "Attachment Missing" || x.Status == "Completed")).OrderByDescending(x => x.Id).ToList();
                                var ArtistEmployeeName = getEmployeesForPendingJobs.Where(x => x.ProcessId == 3).ToList();
                                var QualityEmployeeName = getEmployeesForPendingJobs.Where(x => x.ProcessId == 5).ToList();

                                if (ArtistEmployeeName.Count() == 1)
                                {
                                    if (nxtPid == 4 || nxtPid == 6)
                                    {
                                        insertPWT.ArtistName = ArtistEmployeeName.First().EmployeeName;
                                    }
                                }
                                else if (ArtistEmployeeName.Count() == 2)
                                {
                                    if (nxtPid == 4 || nxtPid == 6)
                                    {
                                        insertPWT.ArtistName = ArtistEmployeeName.First().EmployeeName + "," + ArtistEmployeeName.Last().EmployeeName;
                                    }
                                }
                                if (QualityEmployeeName.Count() == 1)
                                {
                                    if (nxtPid == 4 || nxtPid == 6)
                                    {
                                        insertPWT.ArtistName = QualityEmployeeName.First().EmployeeName;
                                    }
                                }

                                insertPWT.JobStatusId = item.JobStatusId;
                                insertPWT.AllocattedById = item.UpdatedBy;

                                insertPWT.StatusId = 1;

                                insertPWT.IsActive = true;
                                insertPWT.CreatedBy = item.UpdatedBy;
                                insertPWT.CreatedUtc = DateTime.UtcNow;
                                _unitWork.ProcessWorkFlowTran.Add(insertPWT);

                                if ((nxtPid == 1 && statusId == 2) || (nxtPid == 1 && statusId == 18))
                                {
                                    jobOrder.JobClosedUtc = DateTime.UtcNow;
                                    jobOrder.UpdatedBy = item.UpdatedBy;
                                    jobOrder.UpdatedUtc = DateTime.UtcNow;
                                    _unitWork.JobOrder.Update(jobOrder);

                                    string month = jobOrder.FileReceivedDate.ToString("MM");
                                    int day = jobOrder.FileReceivedDate.Day;
                                    int year = jobOrder.FileReceivedDate.Year;
                                    var jobSummaryLastRecord = _unitWork.JobSummary.Where(x => x.ClientId == jobOrder.ClientId && x.MonthYear == month + "-" + year).OrderByDescending(x => x.Id).FirstOrDefault();
                                    var jobStatus = _unitWork.JobStatus.Where(x => x.Id == jobOrder.JobStatusId).FirstOrDefault();
                                    int jobStatusCount = 1;
                                    if (jobStatus.JobStatusDescription.Contains("Fresh"))
                                    {
                                        if (jobSummaryLastRecord.FreshCompleted == null)
                                        {
                                            jobSummaryLastRecord.FreshCompleted = 0;
                                        }
                                        jobSummaryLastRecord.FreshCompleted = jobSummaryLastRecord.FreshCompleted + jobStatusCount;
                                    }
                                    else if (jobStatus.JobStatusDescription.Contains("Revision"))
                                    {
                                        if (jobSummaryLastRecord.RevisionCompleted == null)
                                        {
                                            jobSummaryLastRecord.RevisionCompleted = 0;
                                        }
                                        jobSummaryLastRecord.RevisionCompleted = jobSummaryLastRecord.RevisionCompleted + jobStatusCount;
                                    }
                                    else if (jobStatus.JobStatusDescription.Contains("Rework"))
                                    {
                                        if (jobSummaryLastRecord.ReworkCompleted == null)
                                        {
                                            jobSummaryLastRecord.ReworkCompleted = 0;
                                        }
                                        jobSummaryLastRecord.ReworkCompleted = jobSummaryLastRecord.ReworkCompleted + jobStatusCount;
                                    }
                                    jobSummaryLastRecord.Pending = jobSummaryLastRecord.Pending - 1;
                                    _unitWork.JobSummary.Update(jobSummaryLastRecord);
                                }
                                _unitWork.SaveChanges();
                            }
                            else
                            { //else part of checking file
                                //inside folder no attachments
                                //  AttachmentMissingCall(jobOrder,item);

                                //var pWftid = _unitWork.ProcessWorkFlowTran.OrderByDescending(x => x.Id).FirstOrDefault(x => x.Jid == item.Jid);// -
                                var pWftid = _unitWork.ProcessWorkFlowTran.Where(x => x.Jid == item.Jid).OrderByDescending(x => x.Id).FirstOrDefault();//-

                                if (pWftid.StatusId != 24)
                                {
                                    var statusIdofAttachMissing = _unitWork.Status.Where(x => x.Name == "Attachment Missing").FirstOrDefault().Id;
                                    item.StatusId = statusIdofAttachMissing;
                                    item.UpdatedBy = 1;
                                    item.UpdatedUtc = DateTime.UtcNow;
                                    _unitWork.ProcessWorkFlowTran.Update(item);

                                    var ea = _unitWork.EmployeeAssign.Where(x => x.Jid == item.Jid && x.Status == "Bulk Order Complete").FirstOrDefault();
                                    if (ea != null)
                                    {
                                        ea.Status = "Attachment Missing";
                                        ea.UpdatedUtc = DateTime.UtcNow;
                                        _unitWork.EmployeeAssign.Update(ea);

                                        var jobStatus = _unitWork.JobStatus.FirstOrDefault(x => x.Id == jobOrder.JobStatusId);
                                        var dept = _unitWork.Department.Where(x => x.Id == jobOrder.DepartmentId).FirstOrDefault(); //  
                                        var trayDetailMovement = _unitWork.TrayDetailMovement.Where(x => x.FileName == jobOrder.FileName && x.CustomerJobType == jobOrder.CustomerJobType && x.Department == dept.Description && x.ClientId == jobOrder.ClientId && x.JobStatus == jobStatus.JobStatusDescription && x.IsDeleted == false).OrderByDescending(x => x.Id).FirstOrDefault();//
                                        if (trayDetailMovement != null)
                                        {
                                            trayDetailMovement.CurrentStatus = "Attachment Missing";
                                            _unitWork.TrayDetailMovement.Update(trayDetailMovement);
                                        }

                                        var processName = _unitWork.Process.FirstOrDefault(x => x.Name == trayDetailMovement.CurrentTray).Name;//for current tray record need to update

                                        var trayDetailMovementTran = _unitWork.TrayDetailMovementTran.Where(x => x.Wfmid == ea.Wfmid && x.EmployeeId == ea.EmployeeId && x.Process == processName && x.IsDeleted == false).OrderByDescending(x => x.Id).FirstOrDefault();
                                        if (trayDetailMovementTran != null)
                                        {
                                            trayDetailMovementTran.Status = "Attachment Missing";
                                            trayDetailMovementTran.UpdatedUtc = DateTime.UtcNow;
                                            _unitWork.TrayDetailMovementTran.Update(trayDetailMovementTran);
                                        }
                                    }
                                    _unitWork.SaveChanges();
                                }

                            }
                        }
                        else
                        {// else part of checking folder
                            str += "step-10";
                            //var pWftid = _unitWork.ProcessWorkFlowTran.OrderByDescending(x => x.Id).FirstOrDefault(x => x.Jid == item.Jid);//-

                            var pWftid = _unitWork.ProcessWorkFlowTran.Where(x => x.Jid == item.Jid).OrderByDescending(x => x.Id).FirstOrDefault();//-


                            if (pWftid.StatusId != 24)
                            {
                                var statusIdofAttachMissing = _unitWork.Status.Where(x => x.Name == "Attachment Missing").FirstOrDefault().Id;
                                item.StatusId = statusIdofAttachMissing;
                                item.UpdatedBy = 1;
                                item.UpdatedUtc = DateTime.UtcNow;
                                _unitWork.ProcessWorkFlowTran.Update(item);

                                var ea = _unitWork.EmployeeAssign.Where(x => x.Jid == item.Jid && x.Status == "Bulk Order Complete").FirstOrDefault();
                                if (ea != null)
                                {
                                    ea.Status = "Attachment Missing";
                                    ea.UpdatedUtc = DateTime.UtcNow;
                                    _unitWork.EmployeeAssign.Update(ea);

                                    //13-06-2017 check-in
                                    var jobStatus = _unitWork.JobStatus.FirstOrDefault(x => x.Id == jobOrder.JobStatusId);
                                    var dept = _unitWork.Department.Where(x => x.Id == jobOrder.DepartmentId).FirstOrDefault(); //  
                                    var trayDetailMovement = _unitWork.TrayDetailMovement.Where(x => x.FileName == jobOrder.FileName && x.Department == dept.Description && x.ClientId == jobOrder.ClientId && x.CustomerJobType == jobOrder.CustomerJobType && x.JobStatus == jobStatus.JobStatusDescription && x.IsDeleted == false).OrderByDescending(x => x.Id).FirstOrDefault();//
                                    if (trayDetailMovement != null)
                                    {
                                        trayDetailMovement.CurrentStatus = "Attachment Missing";
                                        _unitWork.TrayDetailMovement.Update(trayDetailMovement);
                                    }

                                    var processName = _unitWork.Process.FirstOrDefault(x => x.Name == trayDetailMovement.CurrentTray).Name;//for current tray record need to update

                                    var trayDetailMovementTran = _unitWork.TrayDetailMovementTran.Where(x => x.Wfmid == ea.Wfmid && x.EmployeeId == ea.EmployeeId && x.Process == processName && x.IsDeleted == false).OrderByDescending(x => x.Id).FirstOrDefault();
                                    if (trayDetailMovementTran != null)
                                    {
                                        trayDetailMovementTran.Status = "Attachment Missing";
                                        trayDetailMovementTran.UpdatedUtc = DateTime.UtcNow;
                                        _unitWork.TrayDetailMovementTran.Update(trayDetailMovementTran);
                                    }
                                }
                                _unitWork.SaveChanges();
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        var jobOrder = _context.JobOrders.Find(item.Jid);
                        Log addlog = new Log();
                        addlog.Module = "bulk close Rep";
                        addlog.Description = "Error Msg in catch:  " + " - " + ex.Message.ToString() + "-" + "Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source + "string" + str;
                        addlog.JobId = jobOrder.Id;
                        addlog.ClientId = jobOrder.ClientId;
                        addlog.Type = "Error Occ";
                        addlog.CreatedUtc = DateTime.UtcNow;
                        _context.Logs.Add(addlog);
                        _context.SaveChanges();

                    }
                    finally
                    {

                    }
                }
            }

            return result;
        }

        public bool UploadFilesTest(string src, ProcessTransaction fileMovement)
        {
            try
            {
                var fileDependents = GetFileDependents(fileMovement);

                var path = _configuration.GetSection("Workflow").GetValue<string>("jobAttachPath") /*ConfigurationManager.AppSettings["jobAttchPath"]*/ + fileDependents.FolderPath.Replace("~/", "");
                var orignalPath = _configuration.GetSection("Workflow").GetValue<string>("jobAttachPath") /*ConfigurationManager.AppSettings["jobAttchPath"] */+ fileDependents.OrignalPath.Replace("~/", "");
                System.IO.Directory.CreateDirectory(path);
                var check = _unitWork.JobOrderFile.FirstOrDefault(x => x.Wfmid == fileDependents.Wfmid);
                var jobOrder = _unitWork.JobOrder.FirstOrDefault(x => x.Id == fileDependents.OrderId);

                var ProcessWorkFlowTran = _unitWork.ProcessWorkFlowTran.Where(x => x.Jid == jobOrder.Id).OrderByDescending(x => x.Id).FirstOrDefault();

                String[] Files;

                if (path[path.Length - 1] != Path.DirectorySeparatorChar)
                    path += Path.DirectorySeparatorChar;
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                Files = Directory.GetFileSystemEntries(src);
                foreach (string Element in Files)
                {
                    fileDependents.FileCount++;
                    var checkfile = Path.GetFileName(Element);
                    if (checkfile != "_Thumbs.db" && checkfile != "thumbs.db" && checkfile != "_thumbs.db" && checkfile != "Thumbs.db")
                    {

                        // Sub directories
                        if (Directory.Exists(Element))
                        {
                            copyDirectory(Element, path + Path.GetFileName(Element));
                        }
                        // Files in directory
                        else
                        {
                            File.Copy(Element, path + Path.GetFileName(Element), true);
                        }

                        if (fileDependents.IsClientOrder == 0)
                        {
                            var jobOrderFile = new JobOrderFile();
                            jobOrderFile.Wfmid = fileDependents.Wfmid;
                            jobOrderFile.Wftid = fileDependents.Wftid;
                            jobOrderFile.JobId = fileDependents.OrderId;
                            jobOrderFile.ProcessId = fileDependents.FakeProcessId;
                            jobOrderFile.IsActive = true;
                            jobOrderFile.FileName = Element;
                            jobOrderFile.CreatedUtc = DateTime.UtcNow;
                            _unitWork.JobOrderFile.Add(jobOrderFile);
                            //_db.JobOrderFiles.Add(jobOrderFile);
                        }
                        _unitWork.SaveChanges();
                        //_db.SaveChanges();

                    }

                }
                var processWorkFlowTran = _unitWork.ProcessWorkFlowTran.FirstOrDefault(x => x.Id == fileDependents.Wftid);
                //var processWorkFlowTran = pwtRecords.FirstOrDefault(x => x.Id == fileDependents.Wftid);
                if (fileMovement.StatusId != fileMovement.FakeStatusId)
                {
                    processWorkFlowTran.FileUploadPath = fileDependents.FakeDynamicFolderPath;

                }
                else
                {
                    processWorkFlowTran.FileUploadPath = fileDependents.DynamicFolderPath;
                }
                processWorkFlowTran.UpdatedUtc = DateTime.UtcNow;
                processWorkFlowTran.UpdatedBy = processWorkFlowTran.CreatedBy;
                _unitWork.ProcessWorkFlowTran.Update(processWorkFlowTran);
                _unitWork.SaveChanges();
                //_db.Entry(processWorkFlowTran).State = EntityState.Modified;
                //_db.SaveChanges();
            }
            catch (Exception ex)
            {

                Log addlog = new Log();
                addlog.Module = "bulk close Rep";
                addlog.Description = "Error Msg in upload folders: " + " - " + ex.Message.ToString() + "-" + "Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source + "string";
                addlog.JobId = 123;
                addlog.Type = "Error Occ";
                addlog.CreatedUtc = DateTime.UtcNow;
                _context.Logs.Add(addlog);
                _context.SaveChanges();

            }
            finally
            {

            }
            return true;
        }

        public ProcessTransaction GetFileDependents(ProcessTransaction fileMovement)
        {
            var folderPathPrefix = @"JobFiles";
            var sourcePath = "";
            var dynamicFolderPath = "";
            var fakeDynamicFolderPath = "";
            var folderPath = "";
            var fileName = "";
            int fileCount = 0;
            var Wfmid = 0;
            var Wftid = 0;
            int? pid = 0;
            var orignalFilePath = "";
            var jobFileName = "";
            if (fileMovement.IsClientOrder == 1)
            {
                dynamicFolderPath = Convert.ToString(fileMovement.OrderId);
                folderPath = folderPathPrefix + '\\' + dynamicFolderPath;
                fileName = Convert.ToString(fileMovement.OrderId) + '_';
                fileCount = _unitWork.ClientOrderExt.Count(x => x.ClientOrderId == fileMovement.OrderId);
                sourcePath = _unitWork.ClientOrder.Where(x => x.OrderId == fileMovement.OrderId).Select(x => x.FileUploadPath).FirstOrDefault();
                jobFileName = _unitWork.ClientOrder.Where(x => x.OrderId == fileMovement.OrderId).Select(x => x.FileName).FirstOrDefault();
            }
            else if (fileMovement.IsClientOrder == 0)
            {
                var jobOrder = _unitWork.JobOrder.Include("Customer").FirstOrDefault(x => x.Id == fileMovement.OrderId);
                var jobId = jobOrder.JobId;
                jobFileName = Regex.Replace(jobOrder.FileName, "[^0-9a-zA-Z]+", " ");
                jobId = jobId.Replace('/', '-');
                string fileReceivedDate = jobOrder.FileReceivedDate.ToString("MM-dd-yyyy");
                dynamicFolderPath = jobOrder.Customer.ShortName + '\\' + fileReceivedDate + '\\' + Regex.Replace(jobOrder.FileName, "[^0-9a-zA-Z]+", " ") + '-' + jobId;
                orignalFilePath = dynamicFolderPath;
                fileName = jobId + '_';
                Wfmid = _unitWork.ProcessWorkFlowMaster.Where(x => x.JobId == fileMovement.OrderId).Select(x => x.Id).FirstOrDefault();
                Wftid = _unitWork.ProcessWorkFlowTran.Where(x => x.Wfmid == Wfmid).OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();


                if (fileMovement.StatusId == 0)
                {
                    dynamicFolderPath = dynamicFolderPath + '\\' + "Production Allocation\\Pending-1";
                    sourcePath = _unitWork.ClientOrder.Where(x => x.OrderId == jobOrder.ClientOrderId).Select(x => x.FileUploadPath).FirstOrDefault();
                }
                else
                {
                    var process = _unitWork.Process.Where(x => x.Id == fileMovement.ProcessId).Select(x => x.Name).FirstOrDefault();
                    var status = _unitWork.Status.Where(x => x.Id == fileMovement.StatusId).Select(x => x.Name).FirstOrDefault();
                    var processStatusCount = _unitWork.ProcessWorkFlowTran.Count(x => x.ProcessId == fileMovement.ProcessId && x.StatusId == fileMovement.StatusId && x.Wfmid == Wfmid);


                    if ((fileMovement.StatusId != fileMovement.FakeStatusId) && fileMovement.IsCopyFiles != 1)
                    {
                        var fakeStatus = _unitWork.Status.Where(x => x.Id == fileMovement.FakeStatusId).Select(x => x.Name).FirstOrDefault();
                        var fakeProcessStatusCount = _unitWork.ProcessWorkFlowTran.Count(x => x.ProcessId == fileMovement.ProcessId && x.StatusId == fileMovement.FakeStatusId && x.Wfmid == Wfmid);

                        fakeDynamicFolderPath = dynamicFolderPath + '\\' + process + '\\' + fakeStatus + '-' + fakeProcessStatusCount;
                        process = _unitWork.Process.Where(x => x.Id == fileMovement.FakeProcessId).Select(x => x.Name).FirstOrDefault();
                    }
                    if (fileMovement.IsCopyFiles == 1)
                    {
                        if (fileMovement.IsProcessWorkFlowTranInserted == 0)
                        {
                            processStatusCount++;
                            var processWorkFlowTran = _unitWork.ProcessWorkFlowTran.Where(x => x.Wfmid == Wfmid).OrderByDescending(x => x.Id).Select(x => new { x.FileUploadPath, x.ProcessId }).FirstOrDefault();

                            sourcePath = processWorkFlowTran.FileUploadPath;
                            pid = processWorkFlowTran.ProcessId;
                        }
                        else
                        {
                            var processWorkFlowTran = _unitWork.ProcessWorkFlowTran.Where(x => x.Wfmid == Wfmid).OrderByDescending(x => x.Id).ToList().Skip(1).Select(x => new { x.FileUploadPath, x.ProcessId }).FirstOrDefault();

                            sourcePath = processWorkFlowTran.FileUploadPath;
                            pid = processWorkFlowTran.ProcessId;
                        }
                    }
                    else
                    {
                        if (fileMovement.IsProcessWorkFlowTranInserted == 0)
                        {
                            processStatusCount++;
                        }
                    }
                    dynamicFolderPath = dynamicFolderPath + '\\' + process + '\\' + status + '-' + processStatusCount;
                    fileCount = _unitWork.JobOrderFile.Count(x => x.JobId == fileMovement.OrderId);
                }
            }
            folderPath = folderPathPrefix + '\\' + dynamicFolderPath;
            fileMovement.SourcePath = folderPathPrefix + '\\' + sourcePath;
            fileMovement.DynamicFolderPath = dynamicFolderPath;
            fileMovement.FolderPath = folderPath;
            fileMovement.FileName = fileName;
            fileMovement.FileCount = fileCount;
            fileMovement.Wfmid = Wfmid;
            fileMovement.Wftid = Wftid;
            fileMovement.OrignalPath = folderPathPrefix + '\\' + orignalFilePath + '\\' + "Orignal File";
            fileMovement.OrignalDynamicPath = orignalFilePath + '\\' + "Orignal File";
            fileMovement.Pid = pid;
            fileMovement.FakeDynamicFolderPath = fakeDynamicFolderPath;
            fileMovement.JobFileName = jobFileName;
            return fileMovement;
        }

        public static void copyDirectory(string Src, string Dst)
        {
            String[] Files;

            if (Dst[Dst.Length - 1] != Path.DirectorySeparatorChar)
                Dst += Path.DirectorySeparatorChar;
            if (!Directory.Exists(Dst)) Directory.CreateDirectory(Dst);
            Files = Directory.GetFileSystemEntries(Src);
            foreach (string Element in Files)
            {
                // Sub directories
                if (Directory.Exists(Element))
                    copyDirectory(Element, Dst + Path.GetFileName(Element));
                // Files in directory
                else
                    File.Copy(Element, Dst + Path.GetFileName(Element), true);
            }
        }

        public Object GetProcessTransactionByWftid(int Wftid, int EmployeeId)
        {
            try
            {
                var getWorkflowDetails = _viewWork.ViewGetWorkflowListforLoginRepository.Get(x => x.Wftid == Wftid && x.EmployeeId == EmployeeId && x.IsActive == true).ToList().LastOrDefault();
                //var estTimeOfArtist = _db.ProcessWorkFlowTrans.OrderByDescending(x => x.Id).Where(x => x.Wfmid == getWorkflowDetails.Wfmid && x.ProcessId == 3).Select(x => new { x.EstimatedTime }).FirstOrDefault();
                var estTimeOfArtist = _tableWork.ProcessWorkFlowTranRepository.Get(x => x.Jid == getWorkflowDetails.Jid && x.ProcessId == 3).OrderByDescending(x => x.Id).Select(x => new { x.EstimatedTime }).FirstOrDefault();
                //var chkRevise = _db.ProcessWorkFlowTrans.OrderByDescending(x => x.Id).Where(x => x.Wfmid == getWorkflowDetails.Wfmid && x.ProcessId == 4 && x.StatusId == 11).ToList();
                var chkRevise = _tableWork.ProcessWorkFlowTranRepository.Get(x => x.Jid == getWorkflowDetails.Jid && x.ProcessId == 4 && (x.StatusId == 11 || x.StatusId == 26)).OrderByDescending(x => x.Id).ToList();
                var checkbench = _tableWork.EmployeeAssignRepository.Get(x => x.JobOrder.IsDeleted == false && x.JobOrder.JobStatusId == 11 && x.IsActive == true && x.IsBench == true && x.EmployeeId == EmployeeId).OrderByDescending(x => x.Id).FirstOrDefault();
                if (getWorkflowDetails.ParentJobId != "" && getWorkflowDetails.ParentJobId != null)
                {
                    var parentJob = _tableWork.JobOrderRepository.Get(x => x.JobId == getWorkflowDetails.ParentJobId).FirstOrDefault();
                    var parentTran = _tableWork.ProcessWorkFlowTranRepository.GetAllVal(x => x.Wfm).Where(x => x.Wfm.JobId == parentJob.Id).OrderByDescending(x => x.Id).FirstOrDefault();
                    getWorkflowDetails.ParentJobtranId = parentTran.Id;
                    getWorkflowDetails.ParentJobFilePath = parentTran.FileUploadPath;
                    var referenceJob = _tableWork.JobOrderRepository.Get(x => x.JobId == getWorkflowDetails.ReferenceJobId).FirstOrDefault();
                    var referenceTran = _tableWork.ProcessWorkFlowTranRepository.GetAllVal(x => x.Wfm).Where(x => x.Wfm.JobId == referenceJob.Id).OrderByDescending(x => x.Id).FirstOrDefault();
                    getWorkflowDetails.ReferenceJobTranId = referenceTran.Id;
                    getWorkflowDetails.ReferenceJobFilePath = referenceTran.FileUploadPath;
                    getWorkflowDetails.ReferenceJobStatusId = parentJob.JobStatusId;
                }
                var getWorkStatus = _viewWork.ViewGetWorkflowListforLoginRepository.Get(x => x.EmployeeId == EmployeeId && x.ProcessId == getWorkflowDetails.ProcessId && x.IsActive == true).OrderByDescending(x => x.Id).ToList();
                var getWorkStatus1 = _viewWork.ViewGetWorkflowListforLoginRepository.Get(x => x.EmployeeId == EmployeeId && x.IsActive == true).OrderByDescending(x => x.Id).ToList();
                var isHold = false;
                var SameEmp = false;
                foreach (var job in getWorkStatus)
                {
                    if (job.Status == "In Process")
                    {
                        job.WorkStatus = _tableWork.EmployeeAssignRepository.Get(x => x.Wftid == job.Wftid).OrderByDescending(x => x.Id).Select(x => x.Status).FirstOrDefault();
                        getWorkflowDetails.WorkStatus = job.WorkStatus;
                        if ((job.WorkStatus == "Working" || job.WorkStatus == "Break" || job.WorkStatus == "Training / Meeting" || job.WorkStatus == "Others") && job.Wftid != Wftid)
                        {
                            isHold = true;
                        }
                        else if ((job.WorkStatus == "Working" || job.WorkStatus == "Break" || job.WorkStatus == "Training / Meeting" || job.WorkStatus == "Others") && job.Wftid == Wftid)
                        {
                            isHold = false;
                            break;
                        }
                    }
                }
                foreach (var job in getWorkStatus1)
                {
                    if (job.Status == "In Process")
                    {

                        job.WorkStatus = _tableWork.EmployeeAssignRepository.Get(x => x.Wftid == job.Wftid).OrderByDescending(x => x.Id).Select(x => x.Status).FirstOrDefault();
                        getWorkflowDetails.WorkStatus = job.WorkStatus;
                        if ((job.WorkStatus == "Working" || job.WorkStatus == "Break" || job.WorkStatus == "Training / Meeting" || job.WorkStatus == "Others") && job.Wftid != Wftid)
                        {
                            SameEmp = true;
                        }
                        else if ((job.WorkStatus == "Working" || job.WorkStatus == "Break" || job.WorkStatus == "Training / Meeting" || job.WorkStatus == "Others") && job.Wftid == Wftid)
                        {
                            SameEmp = false;
                            break;
                        }
                    }
                }
                if (estTimeOfArtist != null)
                {
                    if (estTimeOfArtist.EstimatedTime != null)
                    {
                        getWorkflowDetails.EstimatedTime = Convert.ToInt32(estTimeOfArtist.EstimatedTime);
                    }
                }
                var result = new
                {
                    Checkbench = checkbench,
                    GetWorkflowDetails = getWorkflowDetails,
                    ChkRevise = chkRevise,
                    IsHold = isHold,
                    sameEmp = SameEmp
                };
                return result;
            }
            catch (Exception ex)
            {

                return ex;
            }
        }

        public IEnumerable<EmployeeAssign> GetProductionWorkList(ProcessTransaction processTransaction)
        {
            // the rest
            var employeeAssigns = _tableWork.EmployeeAssignRepository.Get(x => x.Wftid == processTransaction.Wftid && x.EmployeeId == processTransaction.EmployeeId && x.Status != "In Process" && x.Status != "Assigned" && x.IsActive == true).ToList();
            return employeeAssigns;
        }
        public class employeeSummary
        {
            public string Status { get; set; }
            public string Remarks { get; set; }
            public int? TotalTimeTaken { get; set; }
        }
        public class employeesSummary
        {
            public IEnumerable<employeeSummary> employeesSummaryList { get; set; }
        }
        public Object GetProductionWorkSummaryList(ProcessTransaction processTransaction)
        {
            var employeeSummaryHistory = new List<employeesSummary>();
            int Alltotaltime = 0, Workedtimetaken = 0, Breaktimetaken = 0, TrMtimetaken = 0, Holdtimetaken = 0, Otherstimetaken = 0, DeviationTime = 0;
            var Wfmid = _tableWork.EmployeeAssignRepository.Get(x => x.Wftid == processTransaction.Wftid).ToList().Select(x => x.Wfmid).FirstOrDefault();
            var summaryHistory = _tableWork.EmployeeAssignRepository
                        .Get(x => x.Wftid == processTransaction.Wftid && x.EmployeeId == processTransaction.EmployeeId && x.IsActive == true)
                        .GroupBy(x => new { x.Status, x.EstimatedTime }).ToList()
                        .Select(x => x.Select(y => new { Status = y.Status, TotalTimeTaken = x.Sum(z => z.TotalTimeTaken), y.EstimatedTime }).FirstOrDefault()).ToList();
            var status = "";
            var totalTimeTaken = "";
            var Findisbench = _tableWork.EmployeeAssignRepository.Get(x => x.IsBench == true && x.IsActive == true && x.Wftid == processTransaction.Wftid && x.EmployeeId == processTransaction.EmployeeId).OrderByDescending(x => x.Id).FirstOrDefault();
            foreach (var item in summaryHistory)
            {
                status = status + ", " + item.Status;
                totalTimeTaken = totalTimeTaken + ", " + item.TotalTimeTaken;
            }


            var EstimatedTime = 0;
            if (summaryHistory.Count() > 0)
            {
                Workedtimetaken = Convert.ToInt32(summaryHistory.Where(x => x.Status == "Worked").Select(x => x.TotalTimeTaken).FirstOrDefault());
                Breaktimetaken = Convert.ToInt32(summaryHistory.Where(x => x.Status == "Break").Select(x => x.TotalTimeTaken).FirstOrDefault());
                TrMtimetaken = Convert.ToInt32(summaryHistory.Where(x => x.Status == "Training / Meeting").Select(x => x.TotalTimeTaken).FirstOrDefault());
                Holdtimetaken = Convert.ToInt32(summaryHistory.Where(x => x.Status == "Hold").Select(x => x.TotalTimeTaken).FirstOrDefault());
                Otherstimetaken = Convert.ToInt32(summaryHistory.Where(x => x.Status == "Others").Select(x => x.TotalTimeTaken).FirstOrDefault());

                EstimatedTime = summaryHistory[0].EstimatedTime;
            }
            var GetWorkList = new List<GetWorkflowList>();
            GetWorkList.Add(new GetWorkflowList
            {
                Worked = Workedtimetaken,
                Break = Breaktimetaken,
                TrainingorMeeting = TrMtimetaken,
                Hold = Holdtimetaken,
                Others = Otherstimetaken,
                TotalTime = Findisbench == null ? Workedtimetaken + Breaktimetaken + Otherstimetaken : Workedtimetaken,
                EstTime = Convert.ToInt32(EstimatedTime),
                Deviation = Convert.ToInt32((EstimatedTime) - (Workedtimetaken + Breaktimetaken + Otherstimetaken)),
            });
            if (processTransaction.ProcessId == 5)
            {
                var employees = _tableWork.EmployeeAssignRepository.Get(x => x.Wfmid == Wfmid && x.ProcessId == 3 && x.Status != "Assigned").Select(x => new { empId = x.EmployeeId }).GroupBy(x => x.empId).AsQueryable();
                foreach (var employeeId in employees)
                {
                    var employeeSummaryList1 = new List<employeeSummary>();
                    var empId = Convert.ToInt32(employeeId.Key);
                    var employeeSummary = _tableWork.EmployeeAssignRepository
                        .Get(x => x.Wfmid == Wfmid && x.ProcessId == 3 && x.Status != "Assigned" && x.EmployeeId == empId)
                        .GroupBy(x => new { x.Status, x.EstimatedTime }).ToList()
                        .Select(x => x.Select(y => new { y.Status, TotalTimeTaken = x.Sum(z => z.TotalTimeTaken), y.EstimatedTime, y.Remarks }).FirstOrDefault());
                    foreach (var item in employeeSummary)
                    {
                        var temp = new employeeSummary
                        {
                            Status = item.Status,
                            Remarks = item.Remarks,
                            TotalTimeTaken = item.TotalTimeTaken
                        };
                        employeeSummaryList1.Add(temp);
                    }
                    var es1 = new employeesSummary
                    {
                        employeesSummaryList = employeeSummaryList1
                    };
                    employeeSummaryHistory.Add(es1);
                    Workedtimetaken = Convert.ToInt32(employeeSummary.Where(x => x.Status == "Worked").Select(x => x.TotalTimeTaken).FirstOrDefault());
                    Breaktimetaken = Convert.ToInt32(employeeSummary.Where(x => x.Status == "Break").Select(x => x.TotalTimeTaken).FirstOrDefault());
                    TrMtimetaken = Convert.ToInt32(employeeSummary.Where(x => x.Status == "Training / Meeting").Select(x => x.TotalTimeTaken).FirstOrDefault());
                    Holdtimetaken = Convert.ToInt32(employeeSummary.Where(x => x.Status == "Hold").Select(x => x.TotalTimeTaken).FirstOrDefault());
                    Otherstimetaken = Convert.ToInt32(employeeSummary.Where(x => x.Status == "Others").Select(x => x.TotalTimeTaken).FirstOrDefault());
                    //var estimatedTime = employeeSummary.Where(x => x.Status == "Worked").Select(x => x.EstimatedTime).FirstOrDefault();
                    var estimatedTime = employeeSummary.Where(x => x.Remarks != null).Select(x => x.EstimatedTime).FirstOrDefault();
                    GetWorkList.Add(new GetWorkflowList
                    {
                        Worked = Workedtimetaken,
                        Break = Breaktimetaken,
                        TrainingorMeeting = TrMtimetaken,
                        Hold = Holdtimetaken,
                        Others = Otherstimetaken,
                        TotalTime = Findisbench == null ? Workedtimetaken + Breaktimetaken + Otherstimetaken : Workedtimetaken,
                        EstTime = Convert.ToInt32(estimatedTime),
                        Deviation = Convert.ToInt32((estimatedTime) - (Workedtimetaken + Breaktimetaken + Otherstimetaken)),
                        EmployeeName = _tableWork.EmployeeRepository.Get(x => x.EmployeeId == empId).Select(x => x.EmployeeName).FirstOrDefault()
                    });
                }
            }
            var result = new
            {
                SummaryHistory = GetWorkList
            };
            return result;
        }
        //=====================================final CL======================================================
        public Object ChecklistPopup(int Wfmid)
        {
            //  using (ApplicationDbContext Db = new ApplicationDbContext(_configuration))
            // {
            var pwfDetails = Db.ProcessWorkFlowMasters.Where(x => x.Id == Wfmid).FirstOrDefault();
            var cus = Db.Customers.Where(x => x.Id == pwfDetails.CustomerId).FirstOrDefault();
            //var checklist = _tableWork.ChecklistRepository.Get(x => x.CustomerId == cus.Id && x.DeptId == pwfDetails.DepartmentId).Select(x => x.Description).ToList();
            var checklist = _tableWork.ChecklistRepository.Get(x => x.CustomerId == cus.Id && x.DeptId == pwfDetails.DepartmentId && x.IsDeleted == false).Select(x => x.Description).ToList();
            //return checklist;

            return checklist;
            // }

        }
        //=====================================final CL======================================================
        public async Task<Object> ChangeWorkflow(IFormFileCollection filesToUpload, ProcessTransaction processTransaction)
        {
            var result = new Object();

            _db.Database.SetCommandTimeout(0);

            var employeeAssignLastRecord = _db.EmployeeAssigns.Where(x => x.Wftid == processTransaction.Wftid && x.ProcessId == processTransaction.ProcessId && x.EmployeeId == processTransaction.EmployeeId && x.IsActive == true).OrderBy(x => x.Id).ToList().Last();
            EmployeeAssign insertEmployeeAssign = new EmployeeAssign();
            try
            {
                if (processTransaction.WorkType != "End")
                {
                    if (processTransaction.WorkType == "Start")
                    {
                        if (employeeAssignLastRecord.StartDate == null)
                        {
                            var processWorkFlowTranLastRecord = _db.ProcessWorkFlowTrans.Where(x => x.Wfmid == processTransaction.Wfmid && x.IsActive == true).OrderByDescending(x => x.Id).ToList().Skip(1).FirstOrDefault();
                            processWorkFlowTranLastRecord.IsActive = false;
                            processWorkFlowTranLastRecord.UpdatedBy = processTransaction.EmployeeId;
                            processWorkFlowTranLastRecord.UpdatedUtc = DateTime.UtcNow;
                            _db.Entry(processWorkFlowTranLastRecord).State = EntityState.Modified;
                            employeeAssignLastRecord.StartDate = DateTime.UtcNow;
                            employeeAssignLastRecord.Status = "In Process";
                            employeeAssignLastRecord.ErrorCategoryId = processTransaction.ErrorCategoryId;
                            employeeAssignLastRecord.UpdatedBy = processTransaction.EmployeeId;
                            employeeAssignLastRecord.UpdatedUtc = DateTime.UtcNow;
                            _db.Entry(employeeAssignLastRecord).State = EntityState.Modified;
                        }
                        else
                        {
                            employeeAssignLastRecord.EndDate = DateTime.UtcNow;
                            DateTime starttimedate = employeeAssignLastRecord.StartDate.Value;
                            DateTime endtimedate = DateTime.UtcNow;
                            TimeSpan gettime = endtimedate - starttimedate;
                            double totalmins = Math.Floor(gettime.TotalMinutes);
                            int timetaken = (int)totalmins;
                            employeeAssignLastRecord.TotalTimeTaken = timetaken;
                            employeeAssignLastRecord.UpdatedBy = processTransaction.EmployeeId;
                            employeeAssignLastRecord.UpdatedUtc = DateTime.UtcNow;
                            _db.Entry(employeeAssignLastRecord).State = EntityState.Modified;
                        }
                        insertEmployeeAssign.Status = "Working";
                    }
                    else
                    {
                        employeeAssignLastRecord.EndDate = DateTime.UtcNow;
                        DateTime starttimedate = employeeAssignLastRecord.StartDate.Value;
                        DateTime endtimedate = DateTime.UtcNow;
                        TimeSpan gettime = endtimedate - starttimedate;
                        double totalmins = Math.Floor(gettime.TotalMinutes);
                        int timetaken = (int)totalmins;
                        employeeAssignLastRecord.Status = "Worked";
                        employeeAssignLastRecord.TotalTimeTaken = timetaken;
                        employeeAssignLastRecord.UpdatedBy = processTransaction.EmployeeId;
                        employeeAssignLastRecord.UpdatedUtc = DateTime.UtcNow;
                        _db.Entry(employeeAssignLastRecord).State = EntityState.Modified;
                        insertEmployeeAssign.Status = processTransaction.WorkType;
                    }
                    insertEmployeeAssign.Wftid = employeeAssignLastRecord.Wftid;
                    insertEmployeeAssign.Wfmid = processTransaction.Wfmid;
                    insertEmployeeAssign.Jid = employeeAssignLastRecord.Jid;
                    insertEmployeeAssign.EmployeeId = employeeAssignLastRecord.EmployeeId;
                    insertEmployeeAssign.AllocattedDate = employeeAssignLastRecord.AllocattedDate;
                    insertEmployeeAssign.EstimatedTime = employeeAssignLastRecord.EstimatedTime;
                    insertEmployeeAssign.IsBench = employeeAssignLastRecord.IsBench;
                    insertEmployeeAssign.StartDate = DateTime.UtcNow;
                    insertEmployeeAssign.IsActive = true;
                    insertEmployeeAssign.ProcessId = employeeAssignLastRecord.ProcessId;
                    insertEmployeeAssign.CreatedBy = processTransaction.EmployeeId;
                    insertEmployeeAssign.CreatedUtc = DateTime.UtcNow;
                    _db.EmployeeAssigns.Add(insertEmployeeAssign);

                    updateTrayDetailMovement(processTransaction.Wfmid, employeeAssignLastRecord.Wftid, 0, processTransaction.ProcessId, 0, 0, insertEmployeeAssign.Status, 0, processTransaction.EmployeeId, 0, "", _db);
                    _db.SaveChanges();
                }
                else
                {
                    bool isFirstStartMethodExecuted = employeeAssignLastRecord.StartDate == null;
                    if (isFirstStartMethodExecuted)
                    {
                        if (employeeAssignLastRecord.StartDate == null)
                        {
                            var processWorkFlowTranLastRecord = _db.ProcessWorkFlowTrans.Where(x => x.Wfmid == processTransaction.Wfmid && x.IsActive == true).OrderByDescending(x => x.Id).ToList().Skip(1).FirstOrDefault();
                            processWorkFlowTranLastRecord.IsActive = false;
                            processWorkFlowTranLastRecord.UpdatedBy = processTransaction.EmployeeId;
                            processWorkFlowTranLastRecord.UpdatedUtc = DateTime.UtcNow;
                            _db.Entry(processWorkFlowTranLastRecord).State = EntityState.Modified;
                            employeeAssignLastRecord.StartDate = DateTime.UtcNow;
                            employeeAssignLastRecord.Status = "In Process";
                            employeeAssignLastRecord.ErrorCategoryId = processTransaction.ErrorCategoryId;
                            employeeAssignLastRecord.UpdatedBy = processTransaction.EmployeeId;
                            employeeAssignLastRecord.UpdatedUtc = DateTime.UtcNow;
                            _db.Entry(employeeAssignLastRecord).State = EntityState.Modified;
                        }
                        else
                        {
                            employeeAssignLastRecord.EndDate = DateTime.UtcNow;
                            DateTime starttimedate1 = employeeAssignLastRecord.StartDate.Value;
                            DateTime endtimedate1 = DateTime.UtcNow;
                            TimeSpan gettime1 = endtimedate1 - starttimedate1;
                            double totalmins1 = Math.Floor(gettime1.TotalMinutes);
                            int timetaken1 = (int)totalmins1;
                            employeeAssignLastRecord.TotalTimeTaken = timetaken1;
                            employeeAssignLastRecord.UpdatedBy = processTransaction.EmployeeId;
                            employeeAssignLastRecord.UpdatedUtc = DateTime.UtcNow;
                            _db.Entry(employeeAssignLastRecord).State = EntityState.Modified;
                        }
                        insertEmployeeAssign.Status = "Working";

                        insertEmployeeAssign.Wftid = employeeAssignLastRecord.Wftid;
                        insertEmployeeAssign.Wfmid = processTransaction.Wfmid;
                        insertEmployeeAssign.Jid = employeeAssignLastRecord.Jid;
                        insertEmployeeAssign.EmployeeId = employeeAssignLastRecord.EmployeeId;
                        insertEmployeeAssign.AllocattedDate = employeeAssignLastRecord.AllocattedDate;
                        insertEmployeeAssign.EstimatedTime = employeeAssignLastRecord.EstimatedTime;
                        insertEmployeeAssign.IsBench = employeeAssignLastRecord.IsBench;
                        insertEmployeeAssign.StartDate = DateTime.UtcNow;
                        insertEmployeeAssign.IsActive = true;
                        insertEmployeeAssign.ProcessId = employeeAssignLastRecord.ProcessId;
                        insertEmployeeAssign.CreatedBy = processTransaction.EmployeeId;
                        insertEmployeeAssign.CreatedUtc = DateTime.UtcNow;
                        _db.EmployeeAssigns.Add(insertEmployeeAssign);

                        updateTrayDetailMovement(processTransaction.Wfmid, employeeAssignLastRecord.Wftid, 0, processTransaction.ProcessId, 0, 0, insertEmployeeAssign.Status, 0, processTransaction.EmployeeId, 0, "", _db);
                        _db.SaveChanges();
                    }
                    var employeeAssignLastRecord1 = _db.EmployeeAssigns.Where(x => x.Wftid == processTransaction.Wftid && x.ProcessId == processTransaction.ProcessId && x.EmployeeId == processTransaction.EmployeeId && x.IsActive == true).OrderBy(x => x.Id).ToList().Last();
                    var processWorkFlowTranStatusId = 0;
                    int? nextProcessId = null;

                    processWorkFlowTranStatusId = _db.Statuses.Where(x => x.Name == processTransaction.Status).Select(x => x.Id).FirstOrDefault();

                    var pwmDetails = _db.ProcessWorkFlowMasters.FirstOrDefault(x => x.Id == processTransaction.Wfmid);
                    var employee = _db.Employees.Where(x => x.EmployeeId == processTransaction.EmployeeId).FirstOrDefault();

                    var updateJobOrderForScope = _db.JobOrders.Find(pwmDetails.JobId);
                    var processes = new ProcessWorkFlow();
                    var process = _db.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == employee.DepartmentId && x.CustomerId == pwmDetails.CustomerId && x.CurrentProcessId == processTransaction.ProcessId && x.StatusId == processWorkFlowTranStatusId && x.IsActive == true && x.ScopeId == processTransaction.ScopeId && x.JobStatusId == updateJobOrderForScope.JobStatusId);
                    if (process == null)
                    {
                        var process1 = _db.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == employee.DepartmentId && x.CustomerId == pwmDetails.CustomerId && x.CurrentProcessId == processTransaction.ProcessId && x.StatusId == processWorkFlowTranStatusId && x.IsActive == true && x.ScopeId == null && x.JobStatusId == updateJobOrderForScope.JobStatusId);
                        if (process1 == null)
                        {
                            var process2 = _db.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == employee.DepartmentId && x.CustomerId == pwmDetails.CustomerId && x.CurrentProcessId == processTransaction.ProcessId && x.StatusId == processWorkFlowTranStatusId && x.IsActive == true && x.ScopeId == processTransaction.ScopeId && x.JobStatusId == null);
                            if (process2 == null)
                            {
                                var process3 = _db.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == employee.DepartmentId && x.CustomerId == pwmDetails.CustomerId && x.CurrentProcessId == processTransaction.ProcessId && x.StatusId == processWorkFlowTranStatusId && x.IsActive == true && x.ScopeId == null && x.JobStatusId == null);
                                if (process3 == null)
                                {
                                    var process4 = _db.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == employee.DepartmentId && x.CustomerId == null && x.CurrentProcessId == processTransaction.ProcessId && x.StatusId == processWorkFlowTranStatusId && x.IsActive == true && x.ScopeId == processTransaction.ScopeId && x.JobStatusId == updateJobOrderForScope.JobStatusId);
                                    if (process4 == null)
                                    {
                                        var process5 = _db.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == employee.DepartmentId && x.CustomerId == null && x.CurrentProcessId == processTransaction.ProcessId && x.StatusId == processWorkFlowTranStatusId && x.IsActive == true && x.ScopeId == null && x.JobStatusId == updateJobOrderForScope.JobStatusId);
                                        if (process5 == null)
                                        {
                                            var process6 = _db.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == employee.DepartmentId && x.CustomerId == null && x.CurrentProcessId == processTransaction.ProcessId && x.StatusId == processWorkFlowTranStatusId && x.IsActive == true && x.ScopeId == processTransaction.ScopeId && x.JobStatusId == null);
                                            if (process6 == null)
                                            {
                                                var process7 = _db.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == employee.DepartmentId && x.CustomerId == null && x.CurrentProcessId == processTransaction.ProcessId && x.StatusId == processWorkFlowTranStatusId && x.IsActive == true && x.ScopeId == null && x.JobStatusId == null);
                                                if (process7 == null)
                                                {
                                                    result = new
                                                    {
                                                        Success = false,
                                                        Message = "Next Process is not defined from this tray....!",
                                                        IsProcessWorkFlowTranInserted = 0
                                                    };
                                                    return result;
                                                }
                                                else
                                                {
                                                    processes = process7;
                                                }
                                            }
                                            else
                                            {
                                                processes = process6;
                                            }
                                        }
                                        else
                                        {
                                            processes = process5;
                                        }
                                    }
                                    else
                                    {
                                        processes = process4;
                                    }
                                }
                                else
                                {
                                    processes = process3;
                                }
                            }
                            else
                            {
                                processes = process2;
                            }
                        }
                        else
                        {
                            processes = process1;
                        }
                    }
                    else
                    {
                        processes = process;
                    }
                    nextProcessId = processes.NextProcessId;

                    employeeAssignLastRecord1.EndDate = DateTime.UtcNow;
                    DateTime starttimedate = employeeAssignLastRecord1.StartDate.Value;
                    DateTime endtimedate = DateTime.UtcNow;
                    TimeSpan gettime = endtimedate - starttimedate;
                    double totalmins = Math.Floor(gettime.TotalMinutes);
                    int timetaken = (int)totalmins;
                    employeeAssignLastRecord1.TotalTimeTaken = timetaken;
                    employeeAssignLastRecord1.Status = "Worked";
                    employeeAssignLastRecord1.UpdatedBy = processTransaction.EmployeeId;
                    employeeAssignLastRecord1.UpdatedUtc = DateTime.UtcNow;
                    _db.Entry(employeeAssignLastRecord1).State = EntityState.Modified;
                    var employeeAssignRecords = _db.EmployeeAssigns.Where(x => x.Wfmid == processTransaction.Wfmid).OrderBy(x => x.Id).ToList();
                    var employeeAssignFirstRecord = employeeAssignRecords.Where(x => x.Wftid == processTransaction.Wftid && x.EmployeeId == processTransaction.EmployeeId && x.IsActive == true).FirstOrDefault();
                    var totalTime = employeeAssignRecords.Where(x => x.Wftid == processTransaction.Wftid && x.EmployeeId == processTransaction.EmployeeId && x.IsActive == true && (x.Status == "Worked" || x.Status == "Break" || x.Status == "Others")).Sum(x => x.TotalTimeTaken);
                    if (employeeAssignFirstRecord.IsBench == true)
                    {
                        totalTime = employeeAssignRecords.Where(x => x.Wftid == processTransaction.Wftid && x.EmployeeId == processTransaction.EmployeeId && x.IsActive == true && (x.Status == "Worked")).Sum(x => x.TotalTimeTaken);
                    }
                    employeeAssignFirstRecord.TotalTimeTaken = totalTime;
                    employeeAssignFirstRecord.DeviationTime = employeeAssignFirstRecord.EstimatedTime - totalTime;
                    employeeAssignFirstRecord.EndDate = DateTime.UtcNow;
                    employeeAssignFirstRecord.Status = processTransaction.Status;
                    employeeAssignFirstRecord.CommentsToClient = processTransaction.CommentsToClient;
                    employeeAssignFirstRecord.ErrorCategoryId = processTransaction.ErrorCategoryId;
                    employeeAssignFirstRecord.Remarks = processTransaction.Remarks;
                    employeeAssignFirstRecord.UpdatedBy = processTransaction.EmployeeId;
                    employeeAssignFirstRecord.UpdatedUtc = DateTime.UtcNow;
                    _db.Entry(employeeAssignFirstRecord).State = EntityState.Modified;
                    var saveProcessWorkFLowMaster = _db.ProcessWorkFlowMasters.Find(processTransaction.Wfmid);
                    if (processTransaction.ScopeId == null && employee.DepartmentId == 2)
                    {
                        pwmDetails.ScopeId = 21;
                    }
                    else
                    {
                        pwmDetails.ScopeId = processTransaction.ScopeId;
                    }
                    pwmDetails.StitchCount = processTransaction.StitchCount;
                    pwmDetails.UpdatedBy = processTransaction.EmployeeId;
                    pwmDetails.UpdatedUtc = DateTime.UtcNow;
                    _db.Entry(pwmDetails).State = EntityState.Modified;

                    if (processTransaction.ScopeId == null && employee.DepartmentId == 2)
                    {
                        updateJobOrderForScope.ScopeId = 21;
                    }
                    else
                    {
                        updateJobOrderForScope.ScopeId = processTransaction.ScopeId;
                    }
                    updateJobOrderForScope.CommentsToClient = processTransaction.CommentsToClient;
                    updateJobOrderForScope.StitchCount = processTransaction.StitchCount;
                    updateJobOrderForScope.UpdatedBy = processTransaction.EmployeeId;
                    updateJobOrderForScope.UpdatedUtc = DateTime.UtcNow;
                    _db.Entry(updateJobOrderForScope).State = EntityState.Modified;
                    //if (processWorkFlowTranStatusId == 2)
                    var selQCStatusId = _db.Statuses.Where(x => x.Name == "SelfQC").Select(x => x.Id).FirstOrDefault();
                    if (processWorkFlowTranStatusId == 2 || processWorkFlowTranStatusId == 26 || processWorkFlowTranStatusId == selQCStatusId)
                    {
                        processWorkFlowTranStatusId = 1;
                    }

                    var empAssignMinValue = employeeAssignRecords.GroupBy(x => new { x.Wftid, x.EmployeeId }).Select(grp => new
                    {
                        Min = grp.Min(t => t.Id)
                    });
                    var empAssignMinValuesList = empAssignMinValue.ToList().Select(x => x.Min);
                    var getEmployeesForPendingJobs = (from ea in employeeAssignRecords
                                                      join emp in _db.Employees on ea.EmployeeId equals emp.EmployeeId
                                                      where empAssignMinValuesList.Contains(ea.Id)
                                                      select new
                                                      {
                                                          ea.Id,
                                                          ea.Wftid,
                                                          ea.EmployeeId,
                                                          ea.EstimatedTime,
                                                          ea.Status,
                                                          ea.DeviationTime,
                                                          emp.EmployeeCode,
                                                          emp.EmployeeName,
                                                          ea.Wfmid,
                                                          ea.ProcessId,
                                                          ea.IsActive,
                                                          ea.Remarks
                                                      }).ToList();
                    var employeeAssign = getEmployeesForPendingJobs.Where(x => x.Wfmid == employeeAssignFirstRecord.Wfmid && x.ProcessId == employeeAssignFirstRecord.ProcessId && x.IsActive == true).ToList();
                    if (employeeAssign.Count() == 0)
                    {
                        employeeAssign = getEmployeesForPendingJobs.Where(x => x.Wfmid == employeeAssignFirstRecord.Wfmid && x.ProcessId == employeeAssignFirstRecord.ProcessId).ToList();

                    }
                    var inProcess = employeeAssign.Where(x => x.Status == "In Process").FirstOrDefault();
                    if (inProcess == null)
                    {
                        //var querySP = employeeAssign.Where(x => x.Status == "Query for Special Pricing").FirstOrDefault();
                        //var query = employeeAssign.Where(x => x.Status == "Query").FirstOrDefault();
                        //var error = employeeAssign.Where(x => x.Status == "Error").FirstOrDefault();
                        //var workIncomplete = employeeAssign.Where(x => x.Status == "Work Incomplete").FirstOrDefault();
                        //var completed = employeeAssign.Where(x => x.Status == "Completed").FirstOrDefault();
                        //assigned is need for checking
                        var assigned = employeeAssign.Where(x => x.Status == "Assigned").ToList();

                        //var cancelled = employeeAssign.Where(x => x.Status == "Cancelled").FirstOrDefault();
                        //var noNeedRevise = employeeAssign.Where(x => x.Status == "No Need Revise").FirstOrDefault();
                        //var sewOut = employeeAssign.Where(x => x.Status == "Sew Out").FirstOrDefault();
                        //var buddyProof = employeeAssign.Where(x => x.Status == "Buddy Proof").FirstOrDefault();
                        //var bulkCompleted = employeeAssign.Where(x => x.Status == "Bulk Order Complete").FirstOrDefault();

                        var myCheck = employeeAssign.Where(x => x.Status == processTransaction.Status).FirstOrDefault();

                        if (assigned.Count() != 0)
                        {
                            //if (querySP != null || query != null || error != null || cancelled != null)
                            if (myCheck.Status == "Query for Special Pricing" || myCheck.Status == "Query" || myCheck.Status == "Error" || myCheck.Status == "Completed With Error" || myCheck.Status == "Cancelled")
                            {
                                foreach (var item in assigned)
                                {
                                    var ea = _db.EmployeeAssigns.FirstOrDefault(x => x.Id == item.Id);
                                    ea.IsActive = false;
                                    ea.UpdatedBy = processTransaction.EmployeeId;
                                    ea.UpdatedUtc = DateTime.UtcNow;
                                    _db.Entry(ea).State = EntityState.Modified;
                                }
                                var assignedInTDMT = new List<TrayDetailMovementTran>();
                                if (employeeAssignFirstRecord.ProcessId == 3)
                                {
                                    assignedInTDMT = _db.TrayDetailMovementTrans.Where(x => x.Wfmid == employeeAssignFirstRecord.Wfmid && x.Process == "Production" && x.Status == "Pending" && x.IsDeleted == false).ToList();
                                }
                                if (assignedInTDMT.Count() > 0)
                                {
                                    foreach (var item in assignedInTDMT)
                                    {
                                        var trayDetailMovementTran = _db.TrayDetailMovementTrans.FirstOrDefault(x => x.Id == item.Id);
                                        trayDetailMovementTran.IsDeleted = true;
                                        trayDetailMovementTran.UpdatedBy = processTransaction.EmployeeId;
                                        trayDetailMovementTran.UpdatedUtc = DateTime.UtcNow;
                                        _db.Entry(trayDetailMovementTran).State = EntityState.Modified;
                                    }
                                }
                            }
                            //else if (workIncomplete != null || completed != null)
                            else if (myCheck.Status == "Work Incomplete" || myCheck.Status == "Completed")
                            {
                                /* --start Existing Mul alloc
                                var processWorkFlowTran = _db.ProcessWorkFlowTrans.Where(x => x.Wfmid == employeeAssignFirstRecord.Wfmid && (x.ProcessId == processTransaction.ProcessId - 1)).OrderByDescending(x => x.Id).FirstOrDefault();
                                processWorkFlowTran.IsActive = false;

                                processWorkFlowTran.UpdatedBy = processTransaction.EmployeeId;
                                processWorkFlowTran.UpdatedUtc = DateTime.UtcNow;
                                _db.Entry(processWorkFlowTran).State = EntityState.Modified;

                                 --End Existing Mul alloc*/

                                //multiple allocation

                                try
                                {
                                    var processWorkFlowTrans = _db.ProcessWorkFlowTrans.Where(x => x.Jid == employeeAssignFirstRecord.Jid);

                                    foreach (var processWorkFlowTran in processWorkFlowTrans)
                                    {
                                        processWorkFlowTran.UpdatedBy = processTransaction.EmployeeId;
                                        processWorkFlowTran.IsActive = false;

                                        processWorkFlowTran.UpdatedUtc = DateTime.UtcNow;
                                        _db.Entry(processWorkFlowTran).State = EntityState.Modified;
                                    }

                                    var emp = _db.Employees.Where(x => x.EmployeeId == processTransaction.EmployeeId).FirstOrDefault();
                                    var processWorkFlowTran1 = new ProcessWorkFlowTran();
                                    var data = _db.ProcessWorkFlowTrans.Where(x => x.Jid == employeeAssignFirstRecord.Jid).OrderByDescending(x => x.Id).FirstOrDefault();
                                    data.IsActive = true;
                                    //Adding new Issue
                                    //start
                                    processWorkFlowTran1.AllocatedEstimatedTime = data.AllocatedEstimatedTime;
                                    processWorkFlowTran1.AllocattedById = data.AllocattedById;
                                    processWorkFlowTran1.ArtistName = data.ArtistName;
                                    processWorkFlowTran1.DepartmentId = data.DepartmentId;
                                    processWorkFlowTran1.DeviationTime = data.DeviationTime;
                                    processWorkFlowTran1.EmployeeCount = data.EmployeeCount;
                                    processWorkFlowTran1.EmployeeName = data.EmployeeName;
                                    processWorkFlowTran1.ErrorCategoryId = data.ErrorCategoryId;
                                    processWorkFlowTran1.ErrorStatus = data.ErrorStatus;
                                    processWorkFlowTran1.EstimatedTime = data.EstimatedTime;
                                    processWorkFlowTran1.FileUploadPath = data.FileUploadPath;
                                    processWorkFlowTran1.IsFileCopied = data.IsFileCopied;
                                    processWorkFlowTran1.IsDeleted = data.IsDeleted;
                                    processWorkFlowTran1.Jid = data.Jid;
                                    processWorkFlowTran1.JobStatusId = data.JobStatusId;
                                    processWorkFlowTran1.ProcessId = data.ProcessId;
                                    processWorkFlowTran1.StatusId = data.StatusId;
                                    processWorkFlowTran1.TimeStamp = data.TimeStamp;
                                    processWorkFlowTran1.Wfmid = data.Wfmid;
                                    processWorkFlowTran1.WorkflowTypeDescription = data.WorkflowTypeDescription;
                                    //end
                                    processWorkFlowTran1.IsActive = true;
                                    processWorkFlowTran1.Remarks = "Completed By " + emp.EmployeeName;
                                    processWorkFlowTran1.PreviousProcessId = data.ProcessId;
                                    processWorkFlowTran1.CreatedBy = processTransaction.EmployeeId;
                                    processWorkFlowTran1.CreatedUtc = DateTime.UtcNow;
                                    processWorkFlowTran1.UpdatedBy = processTransaction.EmployeeId;
                                    processWorkFlowTran1.UpdatedUtc = DateTime.UtcNow;
                                    _db.Update(data);
                                    _db.ProcessWorkFlowTrans.Add(processWorkFlowTran1);


                                    updateTrayDetailMovement(processTransaction.Wfmid, employeeAssignFirstRecord.Wftid, processTransaction.ScopeId, employeeAssignFirstRecord.ProcessId, nextProcessId, processWorkFlowTranStatusId, employeeAssignFirstRecord.Status, (long)processTransaction.StitchCount, processTransaction.EmployeeId, 0, "", _db);
                                    _db.SaveChanges();

                                }
                                catch (Exception ex)
                                {
                                    if (ex.Message.Contains("referential integrity"))
                                    {

                                    }
                                    else
                                    {
                                        throw ex;
                                    }

                                }
                                if (processTransaction.CopyFiles == true)
                                {
                                    doFileMovement(0, employeeAssignFirstRecord.Wfmid, nextProcessId, processWorkFlowTranStatusId, nextProcessId, processWorkFlowTranStatusId, _db);
                                }
                                if (processTransaction.CopyFiles == false && filesToUpload.Count >= 1)
                                {
                                    var processWorkFlowMaster = _db.ProcessWorkFlowMasters.Where(x => x.Id == processTransaction.Wfmid).OrderByDescending(x => x.Id).ToList().FirstOrDefault();
                                    var jobOrder = _db.JobOrders.Find(processWorkFlowMaster.JobId);
                                    var procTrans = new ProcessTransaction();
                                    procTrans.OrderId = jobOrder.Id;
                                    procTrans.IsClientOrder = 0;
                                    procTrans.StatusId = processWorkFlowTranStatusId;
                                    procTrans.IsProcessWorkFlowTranInserted = 0;
                                    procTrans.ProcessId = Convert.ToInt32(nextProcessId);
                                    procTrans.FakeProcessId = nextProcessId;
                                    procTrans.FakeStatusId = processWorkFlowTranStatusId;
                                    try
                                    {
                                        procTrans.IsProcessWorkFlowTranInserted = 1;
                                        await UploadFiles(filesToUpload, procTrans, _db);
                                    }
                                    catch (Exception)
                                    {

                                        // throw;
                                    }
                                }
                                if (processTransaction.files != null)
                                {
                                    var fileMovementList = new List<ProcessTransaction>();
                                    var processWorkFlowMaster = _db.ProcessWorkFlowMasters.Where(x => x.Id == processTransaction.Wfmid).OrderByDescending(x => x.Id).ToList().FirstOrDefault();
                                    var jobOrder = _db.JobOrders.Find(processWorkFlowMaster.JobId);
                                    var procTrans = new ProcessTransaction();
                                    procTrans.OrderId = jobOrder.Id;
                                    //procTrans.IsClientOrder = 0;
                                    procTrans.StatusId = processWorkFlowTranStatusId;
                                    procTrans.IsProcessWorkFlowTranInserted = 0;
                                    procTrans.IsCopyFiles = 1;
                                    procTrans.ProcessId = Convert.ToInt32(nextProcessId);
                                    procTrans.FakeProcessId = nextProcessId;
                                    procTrans.FakeStatusId = processWorkFlowTranStatusId;
                                    procTrans.files = processTransaction.files;
                                    fileMovementList.Add(procTrans);
                                    CopyFiles(fileMovementList, _db);
                                }
                                result = new
                                {
                                    Success = true,
                                    Message = "Job Moved!",
                                    IsProcessWorkFlowTranInserted = 0,
                                    ProcessId = nextProcessId,
                                    FakeProcessId = nextProcessId,
                                    FakeStatusId = processWorkFlowTranStatusId
                                };

                                return result;
                            }
                        }

                        if (myCheck != null)
                        {
                            result = doNext(myCheck.Status, processWorkFlowTranStatusId, employeeAssignFirstRecord.Wfmid, employeeAssignFirstRecord.ProcessId, nextProcessId, processTransaction, filesToUpload, _db);
                        }

                        _db.SaveChanges();

                        if (processTransaction.CopyFiles == true)
                        {
                            doFileMovement(1, processTransaction.Wfmid, nextProcessId, processWorkFlowTranStatusId, nextProcessId, processWorkFlowTranStatusId, _db);
                        }
                        if (processTransaction.CopyFiles == false && filesToUpload.Count >= 1)
                        {
                            var processWorkFlowMaster = _db.ProcessWorkFlowMasters.Where(x => x.Id == processTransaction.Wfmid).OrderByDescending(x => x.Id).ToList().FirstOrDefault();
                            var jobOrder = _db.JobOrders.Find(processWorkFlowMaster.JobId);
                            var procTrans = new ProcessTransaction();
                            procTrans.OrderId = jobOrder.Id;
                            procTrans.IsClientOrder = 0;
                            procTrans.StatusId = processWorkFlowTranStatusId;
                            procTrans.IsProcessWorkFlowTranInserted = 1;
                            procTrans.ProcessId = Convert.ToInt32(nextProcessId);
                            procTrans.FakeProcessId = nextProcessId;
                            procTrans.FakeStatusId = processWorkFlowTranStatusId;
                            await UploadFiles(filesToUpload, procTrans, _db);
                        }
                        if (processTransaction.files != null)
                        {
                            var fileMovementList = new List<ProcessTransaction>();
                            var processWorkFlowMaster = _db.ProcessWorkFlowMasters.Where(x => x.Id == processTransaction.Wfmid).OrderByDescending(x => x.Id).ToList().FirstOrDefault();
                            var jobOrder = _db.JobOrders.Find(processWorkFlowMaster.JobId);
                            var procTrans = new ProcessTransaction();
                            procTrans.OrderId = jobOrder.Id;
                            procTrans.StatusId = processWorkFlowTranStatusId;
                            procTrans.IsProcessWorkFlowTranInserted = 1;
                            procTrans.IsCopyFiles = 1;
                            procTrans.ProcessId = Convert.ToInt32(nextProcessId);
                            procTrans.FakeProcessId = nextProcessId;
                            procTrans.FakeStatusId = processWorkFlowTranStatusId;
                            procTrans.files = processTransaction.files;
                            fileMovementList.Add(procTrans);
                            CopyFiles(fileMovementList, _db);
                        }
                    }
                    else
                    {
                        updateTrayDetailMovement(processTransaction.Wfmid, employeeAssignFirstRecord.Id, processTransaction.ScopeId, employeeAssignFirstRecord.ProcessId, nextProcessId, processWorkFlowTranStatusId, employeeAssignFirstRecord.Status, (long)processTransaction.StitchCount, processTransaction.EmployeeId, 0, "", _db);
                        _db.SaveChanges();
                        if (processTransaction.CopyFiles == true)
                        {
                            doFileMovement(0, employeeAssignFirstRecord.Wfmid, nextProcessId, processWorkFlowTranStatusId, nextProcessId, processWorkFlowTranStatusId, _db);
                        }


                    }
                }

                // transaction.Commit();
            }
            catch (Exception ex)
            {
                result = new
                {
                    Success = false,
                    Message = "Error Occured!",
                    IsProcessWorkFlowTranInserted = 0,
                    ProcessId = 0,
                    FakeProcessId = 0,
                    FakeStatusId = 0
                };
                //transaction.Rollback();

            }

            //}
            // }
            return result;
        }
        //SingleTableDPR start
        public bool updateTrayDetailMovement(int Wfmid, int Wftid, int? ScopeId, int? currentPid, int? nextProcessId, int statusId, string status, long stitchCount, int employeeId, int DepartmentId, string ErrorStatus, ApplicationDbContext _db)
        {
            var jobId = _db.ProcessWorkFlowMasters.Where(x => x.Id == Wfmid).FirstOrDefault();
            var jobOrder = _db.JobOrders.FirstOrDefault(x => x.Id == jobId.JobId);
            var jobStatus = _db.JobStatuses.FirstOrDefault(x => x.Id == jobOrder.JobStatusId);
            var empAssign = _db.EmployeeAssigns.Where(x => x.Wfmid == Wfmid).ToList();
            var dept = _db.Departments.Where(x => x.Id == jobOrder.DepartmentId).FirstOrDefault(); //

            var workedTime = empAssign.Where(x => x.Wftid == Wftid && x.Status == "Worked" && x.EmployeeId == employeeId).ToList().Sum(x => x.TotalTimeTaken).Value;
            var breakTime = empAssign.Where(x => x.Wftid == Wftid && x.Status == "Break" && x.EmployeeId == employeeId).ToList().Sum(x => x.TotalTimeTaken).Value;
            var pwtDetails = _db.ProcessWorkFlowTrans.Where(x => x.Wfmid == Wfmid && x.ProcessId == 2).OrderByDescending(x => x.Id).FirstOrDefault();
            var trayDetailMovement = new TrayDetailMovement();
            var trayDetailMovementTran = new TrayDetailMovementTran();
            var processName = _db.Processes.FirstOrDefault(x => x.Id == currentPid).Name;

            if (pwtDetails != null && currentPid == 3)
            {
                if (pwtDetails.EmployeeCount == 1)
                {
                    trayDetailMovement = _db.TrayDetailMovements.Where(x => x.FileName == jobOrder.FileName && x.Department == dept.Description && x.ClientId == jobOrder.ClientId && x.CustomerJobType == jobOrder.CustomerJobType && x.JobStatus == jobStatus.JobStatusDescription && x.IsDeleted == false).OrderByDescending(x => x.Id).FirstOrDefault();//
                    trayDetailMovementTran = _db.TrayDetailMovementTrans.Where(x => x.Wfmid == Wfmid && x.EmployeeId == employeeId && x.Process == processName && x.IsDeleted == false).OrderByDescending(x => x.Id).FirstOrDefault();
                }
                else
                {
                    trayDetailMovement = _db.TrayDetailMovements.Where(x => x.FileName == jobOrder.FileName && x.Department == dept.Description && x.ClientId == jobOrder.ClientId && x.CustomerJobType == jobOrder.CustomerJobType && x.JobStatus == jobStatus.JobStatusDescription && x.ProductionAllocatedToEmpId == employeeId && x.IsDeleted == false).OrderByDescending(x => x.Id).FirstOrDefault();//
                    trayDetailMovementTran = _db.TrayDetailMovementTrans.Where(x => x.Wfmid == Wfmid && x.EmployeeId == employeeId && x.Process == processName && x.IsDeleted == false).OrderByDescending(x => x.Id).FirstOrDefault();
                }
            }
            else
            {
                trayDetailMovement = _db.TrayDetailMovements.Where(x => x.FileName == jobOrder.FileName && x.Department == dept.Description && x.ClientId == jobOrder.ClientId && x.CustomerJobType == jobOrder.CustomerJobType && x.JobStatus == jobStatus.JobStatusDescription && x.IsDeleted == false).OrderByDescending(x => x.Id).FirstOrDefault();//
                trayDetailMovementTran = _db.TrayDetailMovementTrans.Where(x => x.Wfmid == Wfmid && x.EmployeeId == employeeId && x.Process == processName && x.IsDeleted == false).OrderByDescending(x => x.Id).FirstOrDefault();
            }

            if (trayDetailMovement != null)
            {

                if (nextProcessId == 0 && ScopeId == 0)
                {
                    trayDetailMovement.CurrentStatus = status;
                }
                else
                {
                    if (!ScopeId.Equals(null))
                    {
                        var scopeDesc = _db.Scopes.FirstOrDefault(x => x.Id == ScopeId);
                        trayDetailMovement.Scope = scopeDesc.Description;
                    }
                    var checkStatus = _db.Statuses.FirstOrDefault(x => x.Id == statusId).Name;
                    var nextStatus = "";
                    if (checkStatus == "Buddy Proof" || checkStatus == "Sew Out" || checkStatus == "SelfQC" || checkStatus == "No Need Revise" || checkStatus == "Completed" || checkStatus == "Completed With Error" || checkStatus == "Pending" || checkStatus == "Bulk Order Complete")
                    {
                        nextStatus = "Pending";
                    }
                    if (nextStatus == "Pending")
                    {
                        trayDetailMovement.CurrentTray = _db.Processes.FirstOrDefault(x => x.Id == nextProcessId).Name;
                        trayDetailMovement.CurrentStatus = _db.Statuses.FirstOrDefault(x => x.Id == statusId).Name;
                    }
                    else
                    {
                        trayDetailMovement.CurrentTray = _db.Processes.FirstOrDefault(x => x.Id == nextProcessId).Name;//2018-05-26 edited by JesusRaja
                        trayDetailMovement.CurrentStatus = _db.Statuses.FirstOrDefault(x => x.Id == statusId).Name;
                    }
                }
                if (currentPid == 3)
                {
                    trayDetailMovement.ProductionWorkedTime = workedTime;
                    trayDetailMovement.ProductionBreakTime = breakTime;
                    //
                    if (nextProcessId != 0 && ScopeId != 0)
                    {
                        trayDetailMovement.ProductionCompletedDateTime = DateTime.UtcNow;
                    }
                    if (DepartmentId == 2 && nextProcessId != 0 && ScopeId != 0)
                    {
                        trayDetailMovement.StitchCount = Convert.ToInt32(stitchCount);
                    }

                }
                else if (currentPid == 5)
                {
                    trayDetailMovement.QualityWorkedTime = workedTime;
                    trayDetailMovement.QualityBreakTime = breakTime;
                    if (nextProcessId != 0 && ScopeId != 0)
                    {
                        trayDetailMovement.QualityCompletedDateTime = DateTime.UtcNow;
                    }
                    if (DepartmentId == 2 && nextProcessId != 0 && ScopeId != 0)
                    {
                        trayDetailMovement.StitchCount = Convert.ToInt32(stitchCount);
                    }
                }
                else if (currentPid == 7)
                {
                    trayDetailMovement.ProofreadingWorkedTime = workedTime;
                    if (nextProcessId != 0 && ScopeId != 0)
                    {
                        trayDetailMovement.ProofReadingCompletedDateTime = DateTime.UtcNow;
                    }
                }
                else if (currentPid == 9)
                {
                    trayDetailMovement.BpworkedTime = workedTime;
                    if (nextProcessId != 0 && ScopeId != 0)
                    {
                        trayDetailMovement.BpcompletedDateTime = DateTime.UtcNow;
                    }
                    if (DepartmentId == 2 && nextProcessId != 0 && ScopeId != 0)
                    {
                        trayDetailMovement.StitchCount = Convert.ToInt32(stitchCount);
                    }
                }
                else if (currentPid == 11)
                {
                    trayDetailMovement.SoworkedTime = workedTime;
                    if (nextProcessId != 0 && ScopeId != 0)
                    {
                        trayDetailMovement.SocompletedDateTime = DateTime.UtcNow;
                    }
                    if (DepartmentId == 2 && nextProcessId != 0 && ScopeId != 0)
                    {
                        trayDetailMovement.StitchCount = Convert.ToInt32(stitchCount);
                    }
                }
                trayDetailMovement.UpdatedBy = employeeId;
                trayDetailMovement.UpdatedUtc = DateTime.UtcNow;
                _db.Entry(trayDetailMovement).State = EntityState.Modified;
            }
            if (trayDetailMovementTran != null)
            {
                if (nextProcessId == 0 && ScopeId == 0)
                {
                    if (trayDetailMovementTran != null)
                    {
                        if (status == "Hold")
                        {
                            trayDetailMovementTran.Status = "Hold";
                        }
                        else
                        {
                            trayDetailMovementTran.Status = "In Process";
                        }
                    }
                }
                else
                {
                    if (trayDetailMovementTran != null)
                    {
                        if (status == "Buddy Proof" || status == "Sew Out" || status == "SelfQC" || status == "No Need Revise")
                        {
                            trayDetailMovementTran.Status = "Completed";
                        }
                        else
                        {
                            trayDetailMovementTran.Status = status;
                        }
                        if (ErrorStatus == "Corrected Job")
                        {
                            trayDetailMovementTran.Status = "Error Correction";
                        }
                    }
                }
                if (ScopeId != 0)
                {
                    trayDetailMovementTran.ScopeId = ScopeId;
                }
                trayDetailMovementTran.StitchCount = stitchCount;
                trayDetailMovementTran.WorkedTime = workedTime;
                trayDetailMovementTran.BreakTime = breakTime;
                trayDetailMovementTran.UpdatedBy = employeeId;
                trayDetailMovementTran.UpdatedUtc = DateTime.UtcNow;
                _db.Entry(trayDetailMovementTran).State = EntityState.Modified;
            }
            return true;
        }
        //SingleTableDPR end
        public bool doFileMovement(int isProcessWorkFlowTranInserted, int? Wfmid, int? processId, int statusId, int? fakeProcessId, int fakeStatusId, ApplicationDbContext _db)
        {
            var jobId = _db.ProcessWorkFlowMasters.Where(x => x.Id == Wfmid).Select(x => x.JobId).FirstOrDefault();
            var fileMovementList = new List<ProcessTransaction>();
            var fileMovement = new ProcessTransaction();
            fileMovement.OrderId = jobId;
            fileMovement.ProcessId = Convert.ToInt32(processId);
            fileMovement.StatusId = statusId;
            fileMovement.IsProcessWorkFlowTranInserted = isProcessWorkFlowTranInserted;
            fileMovement.IsCopyFiles = 1;
            fileMovement.FakeProcessId = fakeProcessId;
            fileMovement.FakeStatusId = fakeStatusId;
            fileMovementList.Add(fileMovement);
            var fileUpload = CopyFiles(fileMovementList, _db);
            return true;
        }
        public async Task<bool> UploadFiles(IFormFileCollection filesToUpload, ProcessTransaction fileMovement, ApplicationDbContext db)
        {
            try
            {

                // using (ApplicationDbContext _db = new ApplicationDbContext(_configuration))
                //{



                var fileDependents = GetFileDependents(fileMovement);
                var path = Path.Combine(_webHostEnvironment.ContentRootPath, fileDependents.FolderPath);//HttpContext.Current.Server.MapPath(fileDependents.FolderPath);
                var orignalPath = Path.Combine(_webHostEnvironment.ContentRootPath, fileDependents.OrignalPath);// Path.GetFullPath(fileDependents.OrignalPath); //HttpContext.Current.Server.MapPath(fileDependents.OrignalPath);
                System.IO.Directory.CreateDirectory(path);
                var check = _db.JobOrderFiles.FirstOrDefault(x => x.Wfmid == fileDependents.Wfmid);
                var jobOrder = _db.JobOrders.FirstOrDefault(x => x.Id == fileDependents.OrderId);
                var Wfmid = _db.ProcessWorkFlowMasters.Where(x => x.JobId == fileMovement.OrderId).Select(x => x.Id).FirstOrDefault();
                var ProcessWorkFlowTran = _db.ProcessWorkFlowTrans.Where(x => x.Wfmid == Wfmid).OrderByDescending(x => x.Id).FirstOrDefault();

                for (var i = 0; i < filesToUpload.Count; i++)
                {
                    var fileName = "";
                    fileDependents.FileCount++;
                    fileName = Regex.Replace(Path.GetFileNameWithoutExtension(filesToUpload[i].FileName), "[^0-9a-zA-Z_-]+", " ");
                    if (fileName == " ")
                    {
                        fileName = fileMovement.JobFileName;
                    }
                    fileName += Path.GetExtension(filesToUpload[i].FileName);

                    if (filesToUpload[i] == null || filesToUpload[i].Length == 0)
                    {
                        return false;
                    }
                    try
                    {
                        var checkExist = System.IO.File.Exists(path + "\\" + fileName);
                        var fileNameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(fileName);
                        var fileNameExt = System.IO.Path.GetExtension(fileName);
                        for (int j = 1; checkExist == true; j++)
                        {
                            fileName = fileNameWithoutExt + " (" + j + ")" + fileNameExt;
                            checkExist = System.IO.File.Exists(path + "\\" + fileName);
                        }
                        using (var stream = new MemoryStream())
                        {
                            await filesToUpload[i].CopyToAsync(stream);
                            FileStream file = new FileStream(Path.Combine(path, fileName), FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                            file.WriteAsync(stream.ToArray(), 0, stream.ToArray().Length);
                        }
                        //filesToUpload[i].CopyTo(Path.Combine(path, fileName));
                        if (fileDependents.IsClientOrder == 0)
                        {
                            if (check == null && jobOrder.ClientOrderId == null)
                            {
                                System.IO.Directory.CreateDirectory(orignalPath);
                                using (var stream = new MemoryStream())
                                {
                                    await filesToUpload[i].CopyToAsync(stream);
                                    FileStream file = new FileStream(Path.Combine(path, fileName), FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                                    file.WriteAsync(stream.ToArray(), 0, stream.ToArray().Length);
                                }
                                // filesToUpload[i].CopyTo(Path.Combine(orignalPath, fileName));
                                jobOrder.FileUploadPath = fileDependents.OrignalDynamicPath;
                                _db.Entry(jobOrder).State = EntityState.Modified;
                                _db.SaveChanges();

                            }
                            if (fileDependents.IsProcessWorkFlowTranInserted == 1)
                            {
                                var processWorkFlowTran = _db.ProcessWorkFlowTrans.FirstOrDefault(x => x.Id == fileDependents.Wftid);
                                //var processWorkFlowTran = pwtRecords.FirstOrDefault(x => x.Id == fileDependents.Wftid);
                                if (fileMovement.StatusId != fileMovement.FakeStatusId)
                                {
                                    processWorkFlowTran.FileUploadPath = fileDependents.FakeDynamicFolderPath;

                                }
                                else
                                {
                                    processWorkFlowTran.FileUploadPath = fileDependents.DynamicFolderPath;
                                }
                                processWorkFlowTran.UpdatedUtc = DateTime.UtcNow;
                                processWorkFlowTran.UpdatedBy = processWorkFlowTran.CreatedBy;
                                _db.Entry(processWorkFlowTran).State = EntityState.Modified;
                                _db.SaveChanges();
                                //pwt.UpdatedUtc = DateTime.UtcNow;
                                //pwt.UpdatedBy = pwt.CreatedBy;
                                //_db.Entry(pwt).State = EntityState.Modified;
                            }
                            //var jobOrderFile = new JobOrderFile();
                            var jobOrderFile = new JobOrderFile(); //_db.JobOrderFiles.Create();

                            jobOrderFile.Wfmid = fileDependents.Wfmid;
                            jobOrderFile.Wftid = fileDependents.Wftid;
                            //jobOrderFile.Wftid = pwt.Id;
                            jobOrderFile.JobId = fileDependents.OrderId;
                            jobOrderFile.ProcessId = fileDependents.FakeProcessId;
                            jobOrderFile.IsActive = true;
                            jobOrderFile.FileName = fileName;
                            jobOrderFile.CreatedUtc = DateTime.UtcNow;
                            jobOrderFile.UpdatedUtc = DateTime.UtcNow;


                            //jobOrderFile.CreatedBy = processWorkFlowTran.CreatedBy;
                            //jobOrderFile.UpdatedBy = processWorkFlowTran.CreatedBy;


                            _db.JobOrderFiles.Add(jobOrderFile);
                            _db.SaveChanges();
                            //_db.Entry(jobOrderFile).State = EntityState.Added;

                        }
                        else if (fileDependents.IsClientOrder == 1)
                        {
                            var clientOrderExt = new ClientOrderExt();
                            clientOrderExt.ClientOrderId = fileDependents.OrderId;
                            clientOrderExt.AssociateFileName = fileName;
                            clientOrderExt.IsDeleted = false;
                            _db.ClientOrderExts.Add(clientOrderExt);
                            var clientOrder = _db.ClientOrders.FirstOrDefault(x => x.OrderId == fileDependents.OrderId);
                            clientOrder.FileUploadPath = fileDependents.DynamicFolderPath;
                            _db.Entry(clientOrder).State = EntityState.Modified;
                            _db.SaveChanges();
                        }


                        if (fileMovement.IsClientOrder == 0 && fileMovement.IsProcessWorkFlowTranInserted == 1)
                        {
                            if (ProcessWorkFlowTran.ProcessId == null && ProcessWorkFlowTran.FileUploadPath != null)
                            {
                                FTPFileUpload(true, fileName, fileDependents.DynamicFolderPath);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
        public bool CopyFiles(IEnumerable<ProcessTransaction> fileMovement, ApplicationDbContext _db)
        {
            foreach (var item in fileMovement)
            {
                var filesToCopy = new List<string>();
                var fileDependents = GetFileDependents(item);
                var sourcePath = Path.Combine(_webHostEnvironment.ContentRootPath, fileDependents.SourcePath);// Path.GetFullPath(); //HttpContext.Current.Server.MapPath(fileDependents.SourcePath);
                var orignalPath = Path.Combine(_webHostEnvironment.ContentRootPath, fileDependents.OrignalPath);// Path.GetFullPath(fileDependents.OrignalPath);//HttpContext.Current.Server.MapPath(fileDependents.OrignalPath);
                var destinationPath = Path.Combine(_webHostEnvironment.ContentRootPath, fileDependents.FolderPath);// Path.GetFullPath(fileDependents.FolderPath); //HttpContext.Current.Server.MapPath(fileDependents.FolderPath);
                System.IO.Directory.CreateDirectory(destinationPath);
                if (item.files == null)
                {
                    filesToCopy = Directory.GetFiles(sourcePath).Select(Path.GetFileName).ToList();
                }
                else
                {
                    filesToCopy = item.files;
                }
                foreach (var sourceFileName in filesToCopy)
                {
                    var fileName = "";
                    if (fileDependents.IsClientOrder == 1)
                    {
                        fileDependents.FileCount++;
                        fileName = sourceFileName;
                    }
                    else if (fileDependents.IsClientOrder == 0)
                    {
                        fileName = sourceFileName;
                        var checkExist = System.IO.File.Exists(destinationPath + "\\" + fileName);
                        var fileNameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(fileName);
                        var fileNameExt = System.IO.Path.GetExtension(fileName);
                        for (int i = 1; checkExist == true; i++)
                        {
                            fileName = fileNameWithoutExt + "(" + i + ")" + fileNameExt;
                            checkExist = System.IO.File.Exists(destinationPath + "\\" + fileName);
                        }
                    }
                    string sourceFile = System.IO.Path.Combine(sourcePath, sourceFileName);
                    string destinationFile = System.IO.Path.Combine(destinationPath, fileName);
                    System.IO.File.Copy(sourceFile, destinationFile, true);
                    if (fileDependents.IsProcessWorkFlowTranInserted == 1)
                    {
                        var processWorkFlowTran = _db.ProcessWorkFlowTrans.FirstOrDefault(x => x.Id == fileDependents.Wftid);
                        if (item.StatusId != item.FakeStatusId)
                        {
                            processWorkFlowTran.FileUploadPath = fileDependents.FakeDynamicFolderPath;
                        }
                        else
                        {
                            processWorkFlowTran.FileUploadPath = fileDependents.DynamicFolderPath;
                        }
                        _db.Entry(processWorkFlowTran).State = EntityState.Modified;
                    }
                    var jobOrderFile = new JobOrderFile();
                    jobOrderFile.Wfmid = item.Wfmid;
                    jobOrderFile.Wftid = fileDependents.Wftid;
                    //jobOrderFile.Wftid = pwt.Id;
                    jobOrderFile.JobId = item.OrderId;
                    jobOrderFile.ProcessId = item.ProcessId;
                    jobOrderFile.IsActive = true;
                    jobOrderFile.FileName = fileName;
                    jobOrderFile.CreatedUtc = DateTime.UtcNow;
                    _db.JobOrderFiles.Add(jobOrderFile);
                }
            }
            _db.SaveChanges();
            return true;
        }
        public Object doNext(string status, int fakeStatusId, int? Wfmid, int? processId, int? nextProcessId, ProcessTransaction processTransaction, IFormFileCollection filesToUpload, ApplicationDbContext _db)
        {
            var employeeAssignDetails = _db.EmployeeAssigns.Where(x => x.Wfmid == Wfmid).OrderBy(x => x.Id).ToList();
            var empAssignMinValue = employeeAssignDetails.GroupBy(x => new { x.Wftid, x.EmployeeId }).Select(grp => new
            {
                Min = grp.Min(t => t.Id)
            });
            var empAssignMinValuesList = empAssignMinValue.ToList().Select(x => x.Min);
            var ViewGetEmployeesForPendingJobs = (from ea in employeeAssignDetails
                                                  join emp in _db.Employees on ea.EmployeeId equals emp.EmployeeId
                                                  where empAssignMinValuesList.Contains(ea.Id)
                                                  select new
                                                  {
                                                      ea.Id,
                                                      ea.Wftid,
                                                      ea.EmployeeId,
                                                      ea.EstimatedTime,
                                                      ea.Status,
                                                      ea.DeviationTime,
                                                      emp.EmployeeCode,
                                                      emp.EmployeeName,
                                                      ea.Wfmid,
                                                      ea.ProcessId,
                                                      ea.IsActive,
                                                      ea.Remarks
                                                  }).ToList();
            var pwmRecords = _db.ProcessWorkFlowMasters.Where(x => x.Id == Wfmid).Select(x => new { x.CustomerId, x.JobId }).FirstOrDefault();
            var getEmployeesForPendingJobs = ViewGetEmployeesForPendingJobs.Where(x => x.Wfmid == Wfmid && x.ProcessId == processId && x.Status == status && x.IsActive == true).OrderByDescending(x => x.Id).ToList();
            var nxtPid = nextProcessId;
            int? currentPid = null;
            var statusId = 0;
            var result = new
            {
                Success = false,
                Message = "",
                IsProcessWorkFlowTranInserted = 0,
                ProcessId = nxtPid,
                FakeProcessId = nextProcessId,
                FakeStatusId = 0
            };
            var remarks = "";
            if (getEmployeesForPendingJobs.Count() > 1)
            {
                foreach (var item in getEmployeesForPendingJobs)
                {
                    remarks = remarks + "\n" + "Created By " + item.EmployeeName + " : " + item.Remarks + "\n";
                }
            }
            else
            {
                foreach (var item in getEmployeesForPendingJobs)
                {
                    remarks = item.Remarks;
                }
            }

            var processWorkFlowTranStatusId = 0;
            var jobQuery = new JobQuery();
            ProcessWorkFlowTran insertPWT = new ProcessWorkFlowTran();
            if (status == "Query for Special Pricing")
            {
                jobQuery.Comments = remarks;
                processWorkFlowTranStatusId = 8;
            }
            else if (status == "Query")
            {
                jobQuery.Comments = remarks;
                processWorkFlowTranStatusId = 6;
            }
            else
            {
                insertPWT.Remarks = remarks;
                processWorkFlowTranStatusId = _db.Statuses.Where(x => x.Name == status).Select(x => x.Id).FirstOrDefault();
            }

            var employee = _db.Employees.Where(x => x.EmployeeId == processTransaction.EmployeeId).FirstOrDefault();
            var pwtRecords = _db.ProcessWorkFlowTrans.Where(x => x.Wfmid == Wfmid).ToList();
            var pwtLastRecord = pwtRecords.Where(x => x.Wfmid == Wfmid).OrderByDescending(x => x.Id).FirstOrDefault();

            var processes = new ProcessWorkFlow();
            var process = _db.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == employee.DepartmentId && x.CustomerId == pwmRecords.CustomerId && x.CurrentProcessId == processId && x.StatusId == processWorkFlowTranStatusId && x.IsActive == true && x.ScopeId == processTransaction.ScopeId && x.JobStatusId == pwtLastRecord.JobStatusId);
            if (process == null)
            {
                var process1 = _db.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == employee.DepartmentId && x.CustomerId == pwmRecords.CustomerId && x.CurrentProcessId == processId && x.StatusId == processWorkFlowTranStatusId && x.IsActive == true && x.ScopeId == null && x.JobStatusId == pwtLastRecord.JobStatusId);
                if (process1 == null)
                {
                    var process2 = _db.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == employee.DepartmentId && x.CustomerId == pwmRecords.CustomerId && x.CurrentProcessId == processId && x.StatusId == processWorkFlowTranStatusId && x.IsActive == true && x.ScopeId == processTransaction.ScopeId && x.JobStatusId == null);
                    if (process2 == null)
                    {
                        var process3 = _db.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == employee.DepartmentId && x.CustomerId == pwmRecords.CustomerId && x.CurrentProcessId == processId && x.StatusId == processWorkFlowTranStatusId && x.IsActive == true && x.ScopeId == null && x.JobStatusId == null);
                        if (process3 == null)
                        {
                            var process4 = _db.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == employee.DepartmentId && x.CustomerId == null && x.CurrentProcessId == processId && x.StatusId == processWorkFlowTranStatusId && x.IsActive == true && x.ScopeId == processTransaction.ScopeId && x.JobStatusId == pwtLastRecord.JobStatusId);
                            if (process4 == null)
                            {
                                var process5 = _db.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == employee.DepartmentId && x.CustomerId == null && x.CurrentProcessId == processId && x.StatusId == processWorkFlowTranStatusId && x.IsActive == true && x.ScopeId == null && x.JobStatusId == pwtLastRecord.JobStatusId);
                                if (process5 == null)
                                {
                                    var process6 = _db.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == employee.DepartmentId && x.CustomerId == null && x.CurrentProcessId == processId && x.StatusId == processWorkFlowTranStatusId && x.IsActive == true && x.ScopeId == processTransaction.ScopeId && x.JobStatusId == null);
                                    if (process6 == null)
                                    {
                                        var process7 = _db.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == employee.DepartmentId && x.CustomerId == null && x.CurrentProcessId == processId && x.StatusId == processWorkFlowTranStatusId && x.IsActive == true && x.ScopeId == null && x.JobStatusId == null);
                                        if (process7 == null)
                                        {
                                        }
                                        else
                                        {
                                            processes = process7;
                                        }
                                    }
                                    else
                                    {
                                        processes = process6;
                                    }
                                }
                                else
                                {
                                    processes = process5;
                                }
                            }
                            else
                            {
                                processes = process4;
                            }
                        }
                        else
                        {
                            processes = process3;
                        }
                    }
                    else
                    {
                        processes = process2;
                    }
                }
                else
                {
                    processes = process1;
                }
            }
            else
            {
                processes = process;
            }
            currentPid = processes.CurrentProcessId;
            nxtPid = processes.NextProcessId;
            statusId = processes.StatusId;
            var selQCStatusId = _db.Statuses.Where(x => x.Name == "SelfQC").Select(x => x.Id).FirstOrDefault();
            if (selQCStatusId == statusId)
            {
                statusId = 2;
            }

            if (currentPid != 0)
            {
                var deviationTime = getEmployeesForPendingJobs.Sum(x => x.DeviationTime);
                pwtLastRecord.IsActive = false;
                pwtLastRecord.UpdatedBy = processTransaction.EmployeeId;
                pwtLastRecord.UpdatedUtc = DateTime.UtcNow;
                pwtLastRecord.DeviationTime = deviationTime;
                _db.Entry(pwtLastRecord).State = EntityState.Modified;
                insertPWT.Wfmid = pwtLastRecord.Wfmid;
                insertPWT.Jid = pwtLastRecord.Jid;
                insertPWT.DepartmentId = pwtLastRecord.DepartmentId;
                insertPWT.PreviousProcessId = currentPid;
                insertPWT.ProcessId = nxtPid;
                var qualityStatusName = new ProcessWorkFlowTran();
                var proofStatusName = new ProcessWorkFlowTran();

                if (currentPid == 3)
                {
                    qualityStatusName = pwtRecords.Where(x => x.Wfmid == pwtLastRecord.Wfmid && x.PreviousProcessId == 5 && x.ProcessId == 2 && (x.StatusId == 11 || x.StatusId == 26) && x.IsActive == false).OrderByDescending(x => x.Id).FirstOrDefault();
                }
                else if (currentPid == 5)
                {
                    proofStatusName = pwtRecords.Where(x => x.Wfmid == pwtLastRecord.Wfmid && x.PreviousProcessId == 7 && x.ProcessId == 4 && (x.StatusId == 11 || x.StatusId == 26) && x.IsActive == false).OrderByDescending(x => x.Id).FirstOrDefault();
                }
                if (qualityStatusName != null)
                {
                    if (qualityStatusName.Id != 0)
                    {
                        insertPWT.ErrorStatus = "Corrected Job";
                    }
                }
                if (proofStatusName != null)
                {
                    if (proofStatusName.Id != 0)
                    {
                        insertPWT.ErrorStatus = "Corrected Job";
                    }
                }

                var ArtistEmployeeName = getEmployeesForPendingJobs.Where(x => x.ProcessId == 3).ToList();
                var QualityEmployeeName = getEmployeesForPendingJobs.Where(x => x.ProcessId == 5).ToList();

                if (ArtistEmployeeName.Count() == 1)
                {
                    if (nxtPid == 4 || nxtPid == 6)
                    {
                        insertPWT.ArtistName = ArtistEmployeeName.First().EmployeeName;
                    }
                }
                else if (ArtistEmployeeName.Count() == 2)
                {
                    if (nxtPid == 4 || nxtPid == 6)
                    {
                        insertPWT.ArtistName = ArtistEmployeeName.First().EmployeeName + "," + ArtistEmployeeName.Last().EmployeeName;
                    }
                }
                if (QualityEmployeeName.Count() == 1)
                {
                    if (nxtPid == 4 || nxtPid == 6)
                    {
                        insertPWT.ArtistName = QualityEmployeeName.First().EmployeeName;
                    }
                }
                insertPWT.JobStatusId = pwtLastRecord.JobStatusId;
                insertPWT.AllocattedById = processTransaction.EmployeeId;
                if (processWorkFlowTranStatusId == 2 || processWorkFlowTranStatusId == selQCStatusId)
                {
                    processWorkFlowTranStatusId = 1;
                }
                insertPWT.StatusId = processWorkFlowTranStatusId;
                insertPWT.IsActive = true;
                insertPWT.CreatedBy = processTransaction.EmployeeId;
                insertPWT.CreatedUtc = DateTime.UtcNow;
                _db.ProcessWorkFlowTrans.Add(insertPWT);

                updateTrayDetailMovement(pwtLastRecord.Wfmid, pwtLastRecord.Id, processTransaction.ScopeId, currentPid, nxtPid, processWorkFlowTranStatusId, processTransaction.Status, (long)processTransaction.StitchCount, processTransaction.EmployeeId, employee.DepartmentId, insertPWT.ErrorStatus, _db);


                result = new
                {
                    Success = true,
                    Message = "Job Moved!",
                    IsProcessWorkFlowTranInserted = 1,
                    ProcessId = nxtPid,
                    FakeProcessId = nextProcessId,
                    FakeStatusId = processWorkFlowTranStatusId
                };
                if (processWorkFlowTranStatusId == 6 || processWorkFlowTranStatusId == 8)
                {
                    var jobOrder = _db.JobOrders.Find(pwmRecords.JobId);
                    var statusDesc = _db.Statuses.FirstOrDefault(x => x.Id == processWorkFlowTranStatusId && x.IsActive == true);
                    var jobQueryCheck = _db.JobOrderExtensions.Where(x => x.Jid == jobOrder.Id && (x.Date.Value.Year == DateTime.UtcNow.Year && x.Date.Value.Month == DateTime.UtcNow.Month && x.Date.Value.Day == DateTime.UtcNow.Day) && x.StatusId == processWorkFlowTranStatusId && x.Flag == true).OrderByDescending(x => x.Id).FirstOrDefault();

                    if (jobQueryCheck == null)
                    {
                        jobQuery.QueryDate = DateTime.UtcNow;
                        JobOrderExtension joe = new JobOrderExtension();
                        joe.Jid = jobOrder.Id;
                        joe.Date = DateTime.UtcNow;
                        joe.Wft = insertPWT;
                        //     joe.Wftid = insertPWT.Id;
                        joe.Flag = true;
                        joe.StatusId = processWorkFlowTranStatusId;
                        if (statusDesc != null)
                        {
                            joe.Type = statusDesc.Name;
                        }
                        _db.JobOrderExtensions.Add(joe);
                    }

                    if ((processWorkFlowTranStatusId == 6 || processWorkFlowTranStatusId == 8) && jobQueryCheck != null)
                    {
                        var jobQueryCheckForResp = _db.JobOrderExtensions.Where(x => x.Jid == jobOrder.Id && (x.Date.Value.Year == DateTime.UtcNow.Year && x.Date.Value.Month == DateTime.UtcNow.Month && x.Date.Value.Day == DateTime.UtcNow.Day) && x.StatusId == (processWorkFlowTranStatusId + 1) && x.Flag == true).OrderByDescending(x => x.Id).FirstOrDefault();
                        if (jobQueryCheckForResp != null)
                        {
                            var exten = _db.JobOrderExtensions.Where(x => x.Jid == jobOrder.Id && x.Flag == true).ToList();
                            foreach (var item in exten)
                            {
                                item.Flag = false;
                                _db.Entry(item).State = EntityState.Modified;
                            }
                            JobOrderExtension joe = new JobOrderExtension();
                            joe.Jid = jobOrder.Id;
                            joe.Date = DateTime.UtcNow;
                            joe.Wft = insertPWT;
                            //     joe.Wftid = insertPWT.Id;
                            joe.Flag = true;
                            joe.StatusId = processWorkFlowTranStatusId;
                            if (statusDesc != null)
                            {
                                joe.Type = statusDesc.Name;
                            }
                            _db.JobOrderExtensions.Add(joe);
                        }
                    }

                    jobQuery.QueryRaisedBy = processTransaction.EmployeeId;
                    jobQuery.Wft = insertPWT;
                    jobQuery.QueryRaisedOn = DateTime.UtcNow;
                    //       jobQuery.Wftid = insertPWT.Id;
                    jobQuery.Wfmid = insertPWT.Wfmid;
                    jobQuery.JobId = jobOrder.JobId;
                    jobQuery.Jid = insertPWT.Jid;
                    jobQuery.ProcessId = pwtLastRecord.ProcessId;
                    jobQuery.StatusId = processWorkFlowTranStatusId;
                    if (processTransaction.ScopeId == null && employee.DepartmentId == 2)
                    {
                        jobQuery.ScopeId = 21;
                    }
                    else
                    {
                        jobQuery.ScopeId = processTransaction.ScopeId;
                    }
                    if (statusDesc != null)
                    {
                        jobQuery.Status = statusDesc.Name;
                    }
                    jobQuery.EstimatedTime = processTransaction.Value;
                    jobQuery.StitchCount = processTransaction.StitchCount;
                    jobQuery.CreatedBy = processTransaction.EmployeeId;
                    jobQuery.CreatedUtc = DateTime.UtcNow;
                    _db.JobQueries.Add(jobQuery);
                    result = new
                    {
                        Success = true,
                        Message = "Job sent as query",
                        IsProcessWorkFlowTranInserted = 1,
                        ProcessId = nxtPid,
                        FakeProcessId = nextProcessId,
                        FakeStatusId = processWorkFlowTranStatusId
                    };
                }
            }
            if ((nxtPid == 1 && statusId == 2) || (nxtPid == 1 && statusId == 18) || (nxtPid == 1 && statusId == 26))
            {
                var jobOrder = _db.JobOrders.Find(pwmRecords.JobId);
                jobOrder.JobClosedUtc = DateTime.UtcNow;
                jobOrder.UpdatedBy = processTransaction.EmployeeId;
                jobOrder.UpdatedUtc = DateTime.UtcNow;
                _db.Entry(jobOrder).State = EntityState.Modified;



                string month = jobOrder.FileReceivedDate.ToString("MM");
                int day = jobOrder.FileReceivedDate.Day;
                int year = jobOrder.FileReceivedDate.Year;
                var jobSummaryLastRecord = _db.JobSummaries.Where(x => x.ClientId == jobOrder.ClientId && x.MonthYear == month + "-" + year).OrderByDescending(x => x.Id).FirstOrDefault();
                var jobStatus = _db.JobStatuses.Where(x => x.Id == jobOrder.JobStatusId).FirstOrDefault();
                int jobStatusCount = 1;
                if (jobStatus.JobStatusDescription.Contains("Fresh"))
                {
                    if (jobSummaryLastRecord.FreshCompleted == null)
                    {
                        jobSummaryLastRecord.FreshCompleted = 0;
                    }
                    jobSummaryLastRecord.FreshCompleted = jobSummaryLastRecord.FreshCompleted + jobStatusCount;
                }
                else if (jobStatus.JobStatusDescription.Contains("Revision"))
                {
                    if (jobSummaryLastRecord.RevisionCompleted == null)
                    {
                        jobSummaryLastRecord.RevisionCompleted = 0;
                    }
                    jobSummaryLastRecord.RevisionCompleted = jobSummaryLastRecord.RevisionCompleted + jobStatusCount;
                }
                else if (jobStatus.JobStatusDescription.Contains("Rework"))
                {
                    if (jobSummaryLastRecord.ReworkCompleted == null)
                    {
                        jobSummaryLastRecord.ReworkCompleted = 0;
                    }
                    jobSummaryLastRecord.ReworkCompleted = jobSummaryLastRecord.ReworkCompleted + jobStatusCount;
                }
                jobSummaryLastRecord.Pending = jobSummaryLastRecord.Pending - 1;
                _db.Entry(jobSummaryLastRecord).State = EntityState.Modified;
            }
            if (status != "Bulk Order Complete")
            {
                foreach (var item in getEmployeesForPendingJobs)
                {
                    //var employeeAssign = _db.EmployeeAssigns.FirstOrDefault(x => x.Id == item.id);
                    var employeeAssign = _db.EmployeeAssigns.FirstOrDefault(x => x.Id == item.Id);
                    employeeAssign.IsActive = false;
                    employeeAssign.UpdatedBy = processTransaction.EmployeeId;
                    employeeAssign.UpdatedUtc = DateTime.UtcNow;
                    _db.Entry(employeeAssign).State = EntityState.Modified;
                }
            }
            //_db.SaveChanges();
            return result;
        }
        public void FTPFileUpload(bool IsFilesInLocalServer, string FileName, string FileUploadPath)
        {
            string ftpUrl, ftpUsername, ftpPassword, fileToUploadPath;
            if (!IsFilesInLocalServer)
            {
                ftpUrl = _configuration.GetSection("AllocationService").GetValue<string>("ftpLocalServerUrl");//ConfigurationManager.AppSettings["ftpLocalServerUrl"];
                ftpUsername = _configuration.GetSection("AllocationService").GetValue<string>("ftpLocalServerUsername");// ConfigurationManager.AppSettings["ftpLocalServerUsername"];
                ftpPassword = _configuration.GetSection("AllocationService").GetValue<string>("ftpLocalServerPassword");//ConfigurationManager.AppSettings["ftpLocalServerPassword"];
            }
            else
            {
                ftpUrl = _configuration.GetSection("AllocationService").GetValue<string>("ftpClientServerUrl");//ConfigurationManager.AppSettings["ftpClientServerUrl"];
                ftpUsername = _configuration.GetSection("AllocationService").GetValue<string>("ftpClientServerUsername");// ConfigurationManager.AppSettings["ftpClientServerUsername"];
                ftpPassword = _configuration.GetSection("AllocationService").GetValue<string>("ftpClientServerPassword");//ConfigurationManager.AppSettings["ftpClientServerPassword"];
            }
            var credentials = new NetworkCredential(ftpUsername, ftpPassword);

            bool isFolderAvailable = false;
            bool isFileAvailable = false;
            //int fileCount = 0;
            string fileName = FileName, dynamicFolder = null;
            fileToUploadPath = FileUploadPath.Replace("\\", "/");
            var folders = fileToUploadPath.Split('/');
            foreach (var folder in folders)
            {
                try
                {
                    dynamicFolder = dynamicFolder + folder + "/";
                    WebRequest webRequest = WebRequest.Create(ftpUrl + dynamicFolder);
                    webRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                    webRequest.Credentials = credentials;
                    webRequest.GetResponse();
                    isFolderAvailable = true;
                }
                catch (WebException ex)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        response.Close();
                        isFolderAvailable = true;
                    }
                    else
                    {
                        response.Close();
                        isFolderAvailable = false;
                    }
                }
            }

            if (isFolderAvailable)
            {
                try
                {
                    WebRequest webRequest = WebRequest.Create(ftpUrl + fileToUploadPath + "/" + fileName);
                    webRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                    webRequest.Credentials = credentials;
                    webRequest.GetResponse();
                    isFileAvailable = true;
                }
                catch (WebException ex)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        response.Close();
                        isFileAvailable = false;
                    }
                    else
                    {
                        response.Close();
                        isFileAvailable = false;
                    }
                }
            }
            if (!isFileAvailable)
            {
                WebClient client = new WebClient();
                client.Credentials = credentials;
                string address = ftpUrl + FileUploadPath + "\\" + fileName;
                address = address.Replace("\\", "/");
                var folderPath = Path.Combine(_webHostEnvironment.ContentRootPath, "JobFiles", FileUploadPath);// Path.GetFullPath("JobFiles") + FileUploadPath;// HttpContext.Current.Server.MapPath(@"~/JobFiles/" + FileUploadPath);
                string filename = folderPath + "\\" + fileName;
                client.UploadFile(address, filename);
            }
        }
        public async Task<Object> ChangeWorkflowBulk(IFormFileCollection filesToUpload, ProcessTransaction processTransaction)
        {
            var result = new Object();
            var myColl = processTransaction.SelectedRows;

            foreach (var item in myColl)
            {
                item.WorkType = processTransaction.WorkType;
                item.ScopeId = processTransaction.ScopeId;
                if (item.WorkType == "End")
                {
                    item.Status = "Completed";
                }
                result = await ChangeWorkflow(filesToUpload, item);
            }
            return result;
        }
        public Object GetErrorCategories(int Wftid, int EmployeeId)
        {
            var deptId = _unitWork.ProcessWorkFlowTran.FirstOrDefault(x => x.Id == Wftid).DepartmentId;
            return _unitWork.ErrorCategory.Where(x => x.DepartmentId == deptId && x.IsDeleted == false).ToList();
        }

    }
}

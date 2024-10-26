using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Office.Interop.Excel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Policy;
using System.Text.RegularExpressions;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class JobOrderService
    {

        public JobOrderService(IConfiguration configuration, IMapper mapper,
            TableWork tableWork, UnitWork unitWork,
            ViewWork viewWork, ProcedureWork procedureWork, FileUpload fileUpload
            , ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _tableWork = tableWork;
            _unitWork = unitWork;
            _viewWork = viewWork;
            _procedureWork = procedureWork;
            _fileUpload = fileUpload;
            db = dbContext;
            _db = dbContext;
            _context = dbContext;
        }
        private readonly IMapper _mapper;
        private readonly FileUpload _fileUpload;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly TableWork _tableWork;
        private readonly UnitWork _unitWork;
        private readonly ViewWork _viewWork;
        private readonly ProcedureWork _procedureWork;
        private readonly ApplicationDbContext db;
        private readonly ApplicationDbContext _db;
        private readonly ApplicationDbContext _context;
        //int existingRevision = 0;
        //bool isFirstExecution = true;
        public Object GetJobHistory(int JId)
        {
            var jobHistoryList = new List<JobHistory>();
            var sqlPara = new SqlParameter[]
            {
                new SqlParameter("@JId",JId)
            };
            var jobQueryHistory = _procedureWork.ExecStoredProcedure<JobQueryHistory_Result>("JobQueryHistory @JId", sqlPara);

            var sqlPara1 = new SqlParameter[]
            {
                new SqlParameter("@JId",JId)
            };
            var jobProdHistory = _procedureWork.ExecStoredProcedure<JobProdHistory_Result>("JobProdHistory @JId", sqlPara1);

            //Modified from _context by uk
            var JobProdHistory = jobProdHistory.OrderByDescending(x => x.TranId).FirstOrDefault();
            if (JobProdHistory.PreviousProcessId == null && (JobProdHistory.StatusId == 7 || JobProdHistory.StatusId == 9) && JobProdHistory.IsFileCopied == false)
            {
                var orginalPath = _tableWork.JobOrderRepository.Get(x => x.Id == JId).Select(x => x.FileUploadPath).FirstOrDefault();
                if (orginalPath != null)
                {
                    //var fileUpload = new FileUpload(_configuration);
                    _fileUpload.CopyFilesFromServer(JobProdHistory.TranId, orginalPath, false);
                }
            }
            //Modified from _context by uk
            var jobCommonDetails = _viewWork.ViewJobQueryInCCRepository.Get(x => x.Jid == JId).OrderByDescending(x => x.TranId).FirstOrDefault();
            if (jobCommonDetails != null)
            {
                if (jobCommonDetails.ParentJobId != "" && jobCommonDetails.ParentJobId != null)
                {
                    var parentJob = _tableWork.JobOrderRepository.Get(x => x.JobId == jobCommonDetails.ParentJobId).FirstOrDefault();
                    var parentTran = _tableWork.ProcessWorkFlowTranRepository.Get(x => x.Jid == parentJob.Id).OrderByDescending(x => x.Id).FirstOrDefault();
                    jobCommonDetails.ParentJobtranId = parentTran.Id;
                    jobCommonDetails.ParentJobFilePath = parentTran.FileUploadPath;

                    var referenceJob = _tableWork.JobOrderRepository.Get(x => x.JobId == jobCommonDetails.ReferenceJobId).FirstOrDefault();
                    var referenceTran = _tableWork.ProcessWorkFlowTranRepository.Get(x => x.Jid == referenceJob.Id).OrderByDescending(x => x.Id).FirstOrDefault();
                    jobCommonDetails.ReferenceJobTranId = referenceTran.Id;
                    jobCommonDetails.ReferenceJobFilePath = referenceTran.FileUploadPath;
                    jobCommonDetails.ReferenceJobStatusId = parentJob.JobStatusId;
                }
            }

            var result = new
            {
                JobHistory = jobProdHistory,//jobHistoryList,
                JobCommonDetails = jobCommonDetails, // jobCommonDetails
                JobQueryHistory = jobQueryHistory
            };
            return result;
        }
        //
        ////Direct convert client order        
        public FileMovement DirectOrder(InwardExcel inwardExcel)
        {

            var fileMovement = new FileMovement();
            var GetDetails = inwardExcel.GetAllValues;
            fileMovement.JobId = "";
            var jobFileName = "";
            int glpcount = 0;
            var GetCustomerCollection = new List<GetCustomerCollectionVM>();
            string value = "";
            if (GetDetails != null) //Direct ClientOrder Conversion
            {
                var order = GetDetails.FirstOrDefault();
                int getdetailcount = GetDetails.Count();
                foreach (var item in GetDetails)
                {
                    var boolresult = GetBlockedResult(item.ClientId);
                    if (boolresult == true)
                    {
                        //return outstanding customer name & mail - block
                        glpcount++;
                        var cus = db.Customers.FirstOrDefault(x => x.Id == item.ClientId);
                        GetCustomerCollection.Add(new GetCustomerCollectionVM
                        {
                            CustomerId = cus.Id,
                            CustomerName = cus.ShortName
                        });
                        continue;
                    }
                    else
                    {
                        //allow to place an order - unblock
                        try
                        {
                            var insertJobOrdersingle = false;
                            if (item.JobStatusId == 7 || item.JobStatusId == 8 || item.JobStatusId == 9 || item.JobStatusId == 10)
                            {
                                //var CheckFileNameExist = _unitWork.JobOrder.Where(x => x.FileName == item.FileName && x.ClientId == item.ClientId && x.DepartmentId == item.DepartmentId && x.JobStatusId != 10 && x.IsDeleted == false).ToList();    //------------------old code changed for division id Job Order---------
                                var CheckFileNameExist = _unitWork.JobOrder.Where(x => x.FileName == item.FileName && x.ClientId == item.ClientId && x.DepartmentId == item.DepartmentId && x.DivisionId == item.DivisionId && x.JobStatusId != 10 && x.IsDeleted == false).ToList();//----------------division id Job Order--------------------------------
                                if (item.JobStatusId == 10)
                                {
                                    //var CheckFileNameQuoteExist = _unitWork.JobOrder.Where(x => x.FileName == item.FileName && x.ClientId == item.ClientId && x.DepartmentId == item.DepartmentId && x.JobStatusId == 10 && x.IsDeleted == false).ToList(); //------------------old code changed for division id Job Order---------
                                    var CheckFileNameQuoteExist = _unitWork.JobOrder.Where(x => x.FileName == item.FileName && x.ClientId == item.ClientId && x.DepartmentId == item.DepartmentId && x.DivisionId == item.DivisionId && x.JobStatusId == 10 && x.IsDeleted == false).ToList();//----------------division id Job Order--------------------------------
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
                            //if (item.JobStatusId == 1 || item.JobStatusId == 2 || item.JobStatusId == 5)
                            //{
                            //    var filesName = _unitWork.JobOrder.Where(x => x.FileName == item.FileName && x.ClientId == item.ClientId && x.DepartmentId == item.DepartmentId && x.DivisionId == item.DivisionId && x.IsDeleted == false).ToList();//----------------division id Job Order--------------------------------
                            //    if (filesName.Count != 0)
                            //    {
                            //        jobFileName = jobFileName + "," + filesName.FirstOrDefault().FileName;
                            //        continue;
                            //    }
                            //}
                            var clientOrder = _unitWork.ClientOrder.Where(x => x.OrderId == item.ClientOrderId).FirstOrDefault();

                            int jobstatusidsingle = item.JobStatusId;
                            var JobstatusDetailsingle = _unitWork.JobStatus.Where(x => x.Id == jobstatusidsingle).FirstOrDefault();
                            int jobstatusgroupidsingle = JobstatusDetailsingle.JobStatusGroupId;
                            DateTime jobdatesingle = DateTime.UtcNow;
                            var transactiontypeDetailsingle = _unitWork.TransactionType.Where(x => x.JobStatusGroupId == jobstatusgroupidsingle).FirstOrDefault();
                            int transactionidsingle = transactiontypeDetailsingle.Id;
                            var freshJobExistssingle = _unitWork.JobOrder.Where(x => x.ClientId == item.ClientId && x.FileName.Equals(item.FileName)).OrderBy(x => x.Id).ToList();
                            var freshJobExistscountsingle = freshJobExistssingle.Count > 0;

                            string monthsingle = jobdatesingle.ToString("MM");
                            string daysingle = jobdatesingle.ToString("dd");
                            string Jobnosingle = "";
                            JobOrder jobOrder = new JobOrder();

                        gotostmt_voucher:

                            var VoucherControlDetailsingle = _unitWork.VoucherControl.FirstOrDefault(x => x.TransactionId == transactionidsingle && (x.EffectiveFrom <= jobdatesingle && x.EffectiveTo >= jobdatesingle) && x.DepartmentId == item.DepartmentId && x.IsDeleted == false && x.IsActive == true);
                            if (VoucherControlDetailsingle == null)
                            {
                                fileMovement.Message = "VoucherControl is Missing";
                                if (getdetailcount == 1)
                                {
                                    return fileMovement;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            string Prefixsingle = VoucherControlDetailsingle.Prefix.ToString();
                            long VoucherNosingle = VoucherControlDetailsingle.Voucherno + 1;
                            Jobnosingle = Prefixsingle + monthsingle + daysingle + "/" + VoucherNosingle;
                            var filesName = _unitWork.JobOrder.Where(x => x.FileName == item.FileName && x.ClientId == item.ClientId && x.DepartmentId == item.DepartmentId && x.JobStatusId == jobstatusidsingle && x.IsDeleted == false).ToList();
                            if (jobstatusidsingle == 1 || jobstatusidsingle == 2 || jobstatusidsingle == 5)
                            {
                                Jobnosingle = Prefixsingle + monthsingle + daysingle + "/" + VoucherNosingle + "/R" + (filesName.Count == 0 ? 1 : (filesName.Count + 1));
                            }
                            //if (jobstatusidsingle == 1 || jobstatusidsingle == 2 || jobstatusidsingle == 5)
                            //{
                            //    int revisionCount = GetRevisionCount(item.FileName);
                            //    string revisionTag = GetRevisionTag(revisionCount);
                            //    if (revisionCount >= 2)
                            //    {
                            //        RevisionEmailNotification(item.ClientId, item.FileName, revisionCount);
                            //    }

                            //    Jobnosingle = Prefixsingle + monthsingle + daysingle + "/" + VoucherNosingle + revisionTag;
                            //}
                            var voucherControl = _unitWork.VoucherControl.Find(VoucherControlDetailsingle.Id);
                            voucherControl.Voucherno = VoucherNosingle;
                            voucherControl.UpdatedBy = jobOrder.CreatedBy;
                            voucherControl.UpdatedUtc = DateTime.UtcNow;
                            _unitWork.VoucherControl.Update(voucherControl);

                            int checkjobid = _unitWork.JobOrder.Count(x => x.JobId == Jobnosingle);
                            if (checkjobid == 1)
                            {
                                goto gotostmt_voucher;
                            }
                            else
                            {
                                jobOrder.TransactionId = transactionidsingle;
                                jobOrder.JobId = Jobnosingle;
                                jobOrder.JobDate = DateTime.UtcNow;
                                jobOrder.JobDescription = "Convert From Client Order";
                                jobOrder.JobStatusId = item.JobStatusId;
                                jobOrder.DepartmentId = item.DepartmentId;
                                jobOrder.DivisionId = item.DivisionId;//----------------division id Job Order---------------------------
                                jobOrder.EmployeeId = item.EmployeeId;
                                jobOrder.ClientId = item.ClientId;
                                jobOrder.ClientOrderId = item.ClientOrderId;
                                jobOrder.Remarks = item.Remarks;
                                jobOrder.PoNo = item.PoNo;
                                jobOrder.FileReceivedDate = item.FileReceivedDate;
                                jobOrder.FileName = item.FileName;
                                //
                                if (clientOrder.FileInwardTypeId == 9)
                                {
                                    jobOrder.FileInwardTypeId = 9;
                                }
                                else
                                {
                                    jobOrder.FileInwardTypeId = 3;
                                }
                                //
                                jobOrder.Username = item.Username;
                                jobOrder.SalesPersonName = item.SalesPersonName;
                                if (item.CCId != null)
                                {
                                    jobOrder.Ccid = item.CCId;
                                }
                                else
                                {
                                    var temp = _unitWork.ClientOrder.FirstOrDefault(c => c.FileName == item.FileName).Ccid;
                                    jobOrder.Ccid = temp;
                                }
                                if (item.CCEmailId != null)
                                {
                                    jobOrder.CcemailId = item.CCEmailId;
                                }
                                else
                                {
                                    string temp = _unitWork.ClientOrder.FirstOrDefault(c => c.FileName == item.FileName).CcemailId;
                                    jobOrder.CcemailId = temp;
                                }

                                jobOrder.CustomerName = item.CustomerName;
                                jobOrder.DivisionId = item.DivisionId;//----------------division id Job Order---------------------------
                                jobOrder.Temp = item.Temp;
                                jobOrder.Style = item.Style;
                                jobOrder.ProjectCode = item.ProjectCode;
                                jobOrder.TeamCode = item.TeamCode;
                                jobOrder.SchoolName = item.SchoolName;
                                jobOrder.Color = item.Color;
                                jobOrder.Gender = item.Gender;
                                jobOrder.LogoDimensionWidth = item.LogoDimensionWidth;
                                jobOrder.ApparelLogoLocation = item.ApparelLogoLocation;
                                jobOrder.ImprintColors1 = item.ImprintColors1;
                                jobOrder.ImprintColors2 = item.ImprintColors2;
                                jobOrder.ImprintColors3 = item.ImprintColors3;
                                jobOrder.VirtualProof = item.VirtualProof;
                                jobOrder.IsDeleted = false;
                                jobOrder.CreatedBy = item.EmployeeId;
                                jobOrder.CustomerJobType = "Live";
                                jobOrder.Podate = item.PODate;
                                if (clientOrder.ParentJobId != null && clientOrder.JobStatus == 9)
                                {
                                    var GetspecialPrice = _unitWork.JobQuery.Where(x => x.JobId == clientOrder.ParentJobId && x.ProcessId == 1).FirstOrDefault();
                                    if (GetspecialPrice != null)
                                    {
                                        if (GetspecialPrice.SpecialPrice != null)
                                        {
                                            jobOrder.SpecialPrice = GetspecialPrice.SpecialPrice;
                                            jobOrder.IsSpecialPrice = true;
                                            jobOrder.IsQuatation = false;
                                            jobOrder.PricingTypeId = 2;
                                        }
                                    }
                                }
                                jobOrder.FileUploadPath = clientOrder.FileUploadPath;
                                var freshJobExistsfordatesingle = _unitWork.JobOrder.Where(x => x.ClientId == item.ClientId && x.FileName == item.FileName && x.DepartmentId == item.DepartmentId && x.IsDeleted == false).FirstOrDefault();
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
                                    if (item.DivisionId != 0)
                                    {
                                        insertJobOrdersingle = true;
                                    }
                                    //----------------division id Job Order---------------------------
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
                                    processWorkFlowTran.FileUploadPath = clientOrder.FileUploadPath;
                                    _unitWork.ProcessWorkFlowTran.Add(processWorkFlowTran);
                                    _unitWork.SaveChanges();
                                    if (jobstatusidsingle == 1 || jobstatusidsingle == 2 || jobstatusidsingle == 5)
                                    {
                                        if ((filesName.Count) >= 2)
                                        {
                                            RevisionEmailNotification(item.ClientId, item.DepartmentId, item.DivisionId, item.FileName, (filesName.Count + 1), item.FileInwardTypeId);
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
                                    var customerShortName = _unitWork.Customer.Where(x => x.Id == item.ClientId).FirstOrDefault();
                                    var dept = _unitWork.Department.Where(x => x.Id == item.DepartmentId).FirstOrDefault();
                                    var jobStatus = _unitWork.JobStatus.Where(x => x.Id == item.JobStatusId).FirstOrDefault();
                                    var ccIntake = _unitWork.Employee.Where(x => x.EmployeeId == jobOrder.CreatedBy).FirstOrDefault();
                                    var empname = _unitWork.Employee.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();
                                    var classiid = _unitWork.Customer.FirstOrDefault(x => x.Id == item.ClientId).CustomerClassificationId;
                                    var classification = _unitWork.CustomerClassification.FirstOrDefault(x => x.Id == classiid).Description;
                                    TrayDetailMovement trayDetailMovement = new TrayDetailMovement();
                                    trayDetailMovement.JobDate = DateTime.UtcNow;
                                    trayDetailMovement.FileName = item.FileName;
                                    trayDetailMovement.ClientId = item.ClientId;
                                    trayDetailMovement.ClientShortName = customerShortName.ShortName;
                                    trayDetailMovement.Department = dept.Description;
                                    trayDetailMovement.JobStatus = jobStatus.JobStatusDescription;
                                    trayDetailMovement.CustomerJobType = jobOrder.CustomerJobType;
                                    trayDetailMovement.CurrentTray = "Production Allocation";
                                    trayDetailMovement.CurrentStatus = "Pending";
                                    trayDetailMovement.CcintakenBy = ccIntake.EmployeeCode + "-" + ccIntake.EmployeeName;
                                    trayDetailMovement.IsDeleted = false;
                                    trayDetailMovement.CreatedBy = item.EmployeeId;
                                    trayDetailMovement.CreatedUtc = DateTime.UtcNow;
                                    trayDetailMovement.Jid = _tableWork.JobOrderRepository.GetLastRecord(s => s.Id).Id;
                                    trayDetailMovement.JobId = jobOrder.JobId;
                                    trayDetailMovement.DepartmentId = item.DepartmentId;
                                    trayDetailMovement.EmployeeId = item.EmployeeId;
                                    trayDetailMovement.EmployeeName = empname.EmployeeCode + "-" + empname.EmployeeName;
                                    trayDetailMovement.StatusId = 1;
                                    trayDetailMovement.ClassificationId = classiid;
                                    trayDetailMovement.Classification = classification;
                                    trayDetailMovement.CcintakenDate = DateTime.UtcNow;
                                    _unitWork.TrayDetailMovement.Add(trayDetailMovement);

                                    var updateclientOrders = _unitWork.ClientOrder.Where(x => x.OrderId == item.ClientOrderId).FirstOrDefault();
                                    updateclientOrders.IsConverted = true;
                                    updateclientOrders.UpdatedBy = jobOrder.CreatedBy;
                                    updateclientOrders.UpdatedUtc = DateTime.UtcNow;
                                    _unitWork.ClientOrder.Update(updateclientOrders);

                                    var clientOrderExts = _unitWork.ClientOrderExt.Where(x => x.ClientOrderId == item.ClientOrderId && x.IsDeleted == false && x.FileDownloadDate == null).ToList();
                                    if (clientOrderExts == null)
                                    {
                                        fileMovement.Message = "Upload is Being Processed";
                                        return fileMovement;
                                    }
                                    foreach (var item1 in clientOrderExts)
                                    {
                                        item1.IsDeleted = true;
                                        item1.FileDownloadDate = DateTime.UtcNow;
                                        _unitWork.ClientOrderExt.Update(item1);

                                    }
                                    int year = jobOrder.FileReceivedDate.Year;
                                    var Getjobsummary = _unitWork.JobSummary.Where(x => x.ClientId == item.ClientId && x.MonthYear == monthsingle + "-" + year).ToList();
                                    var GetLastRecord = Getjobsummary.LastOrDefault();
                                    if (Getjobsummary.Count == 0)
                                    {
                                        JobSummary savejobsummary = new JobSummary();
                                        savejobsummary.ClientId = item.ClientId;
                                        savejobsummary.MonthYear = monthsingle + "-" + year;
                                        if (GetLastRecord == null)
                                        {
                                            savejobsummary.Pending = 1;
                                            savejobsummary.TotalFiles = 1;
                                        }
                                        _unitWork.JobSummary.Add(savejobsummary);

                                    }
                                    else
                                    {
                                        var Updatejobsummary = _unitWork.JobSummary.Where(x => x.Id == GetLastRecord.Id).FirstOrDefault();
                                        Updatejobsummary.Pending = GetLastRecord.Pending + 1;
                                        Updatejobsummary.TotalFiles = GetLastRecord.TotalFiles + 1;
                                        _unitWork.JobSummary.Update(Updatejobsummary);

                                    }
                                    var pwtLast = _unitWork.ProcessWorkFlowTran.Local().OrderByDescending(x => x.Id).FirstOrDefault();

                                    var path = Path.Combine(_webHostEnvironment.ContentRootPath, "JobFiles", pwtLast.FileUploadPath);//HttpContext.Current.Server.MapPath("~/JobFiles//" + pwtLast.FileUploadPath);
                                    var filesToCopy = Directory.GetFiles(path).Select(Path.GetFileName).ToList();
                                    foreach (var fileName in filesToCopy)
                                    {
                                        var jobOrderFile = new JobOrderFile();
                                        jobOrderFile.Wfmid = pwtLast.Wfmid;
                                        jobOrderFile.Wftid = pwtLast.Id;
                                        jobOrderFile.JobId = Convert.ToInt32(pwtLast.Jid);
                                        jobOrderFile.ProcessId = pwtLast.ProcessId;
                                        jobOrderFile.IsActive = true;
                                        jobOrderFile.FileName = fileName;
                                        jobOrderFile.CreatedUtc = DateTime.UtcNow;
                                        _unitWork.JobOrderFile.Add(jobOrderFile);
                                    }
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
                    }
                }
                if (glpcount >= 1)
                {
                    var cnamelocal = GetCustomerCollection.ToList();
                    value = string.Join(", ", from item in cnamelocal select item.CustomerName);
                    fileMovement.Message = "Converted Successfully,Some Order has been blocked temporarily due to Invoice Outstanding for customer- " + value + ",Kindly contact respective sales person/service desk.";
                    return fileMovement;
                }
            }
            fileMovement.Message = "Converted Successfully";
            return fileMovement;
        }
        ////Convert excel
        public FileMovement ExcelOrder(InwardExcel inwardExcel)
        {

            var fileMovement = new FileMovement();
            var CCPersonInfo = new CustomerContact();
            IEnumerable<FileInwardExcel> oInwardExcel = inwardExcel.ViewDatas;
            int glbcount = 0;
            var GetCustomerCollection = new List<GetCustomerCollectionVM>();
            string value = "";
            if (oInwardExcel != null)
            {
                int oInwardExcelcount = oInwardExcel.Count();
                foreach (var item in oInwardExcel) //Excel Orders
                {
                    string clientSN = item.ClientName.ToString(); // starts
                    var ClientInfo = _unitWork.Customer.Where(x => x.ShortName == clientSN).FirstOrDefault();
                    int clientId = ClientInfo.Id;
                    var boolresult = GetBlockedResult(clientId);
                    if (boolresult == true)
                    {
                        //return outstanding customer name & mail - block
                        glbcount++;
                        var cus = db.Customers.FirstOrDefault(x => x.Id == clientId);
                        GetCustomerCollection.Add(new GetCustomerCollectionVM
                        {
                            CustomerId = cus.Id,
                            CustomerName = cus.ShortName
                        });
                        continue;
                    }
                    else
                    {
                        //allow to place an order - unblock
                        if (item.ClientSalesPerson != null)
                        {
                            CCPersonInfo = _unitWork.CustomerContact.Where(x => x.CustomerId == clientId && x.IsDeleted == false && x.ContactName == item.ClientSalesPerson).FirstOrDefault();//SalesPersonName
                        }

                        if (CCPersonInfo != null || item.ClientSalesPerson == null)
                        { // ends


                            try
                            {
                                string FileName = item.FileName;
                                string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

                                foreach (char c in invalid)
                                {
                                    FileName = FileName.Replace(c.ToString(), "");
                                }
                                FileName = Regex.Replace(FileName, "[^0-9a-zA-Z_-]+", " ");
                                string Jobstatusdescription = "";
                                string jobdesc = item.JobStatusDescription.ToString();
                                string departmentdesc = item.Department.ToString();
                                string filetype = item.FileInwardMode.ToString();
                                string clientname = item.ClientName.ToString();
                                string divisionName = item.Division.ToString(); //-----------------------------------division id Job Order----------------------------------
                                if (item.Priority != null)
                                {
                                    string priorityfilestatus = item.Priority.ToString();
                                    Jobstatusdescription = jobdesc + "-" + priorityfilestatus;
                                }
                                else
                                {
                                    Jobstatusdescription = jobdesc;
                                }
                                var JobstatusDetail = _unitWork.JobStatus.Where(x => x.JobStatusDescription == Jobstatusdescription).FirstOrDefault();
                                var DepartmentDetail = _unitWork.Department.Where(x => x.Description.Contains(departmentdesc)).FirstOrDefault();
                                var FileinwardtypeDetail = _unitWork.FileInwardType.Where(x => x.Description.Contains(filetype)).FirstOrDefault();
                                var ClientDetail = _unitWork.Customer.Where(x => x.ShortName == clientname).FirstOrDefault();
                                var DivisionDetail = _unitWork.Division.Where(x => x.DivisionName == divisionName).FirstOrDefault();//------------------division id Job Order---------------------------------
                                int jobstatusid = JobstatusDetail.Id;
                                int jobstatusgroupid = JobstatusDetail.JobStatusGroupId;
                                int departmentid = DepartmentDetail.Id;
                                int fileinwardid = FileinwardtypeDetail.Id;
                                DateTime jobdate = item.DateofReceived;
                                int clientid = ClientDetail.Id;
                                int divisionid = DivisionDetail.Id;   //-------------------division id Job Order---------------------
                                var transactiontypeDetail = _unitWork.TransactionType.Where(x => x.JobStatusGroupId == jobstatusgroupid).FirstOrDefault();
                                int transactionid = transactiontypeDetail.Id;
                                var insertJobOrder = false;
                                if (jobstatusid == 7 || jobstatusid == 8 || jobstatusid == 9)
                                {
                                    var CheckFileNameExist = _unitWork.JobOrder.Where(x => x.FileName == FileName && x.ClientId == clientid && x.DepartmentId == departmentid && (x.JobStatusId != 10 || x.JobStatusId == jobstatusid) && x.IsDeleted == false).ToList();
                                    if (CheckFileNameExist.Count != 0)
                                    {
                                        insertJobOrder = false;

                                    }
                                }
                                //if (jobstatusid == 1 || jobstatusid == 2 || jobstatusid == 5)
                                //{
                                //    var fileName = _unitWork.JobOrder.Where(j => j.FileName == FileName && j.ClientId == clientid && j.DepartmentId == departmentid && j.JobStatusId == jobstatusid && j.IsDeleted == false).ToList();
                                //    if (fileName.Count != 0)
                                //    {
                                //        insertJobOrder = false;
                                //    }
                                //}

                                var freshJobExists = _unitWork.JobOrder.Where(x => x.ClientId == clientid && x.FileName == FileName && x.DepartmentId == departmentid && x.IsDeleted == false).OrderBy(x => x.Id).ToList();
                                var freshJobExistscount = freshJobExists.Count > 0;

                                string month = jobdate.ToString("MM");
                                string day = jobdate.ToString("dd");
                                int year = jobdate.Year;
                                string Jobno = "";
                                JobOrder oJobOrder = new JobOrder();

                            gotostmt_exlvouch:

                                var VoucherControlDetail = _unitWork.VoucherControl.FirstOrDefault(x => x.TransactionId == transactionid && (x.EffectiveFrom <= jobdate && x.EffectiveTo >= jobdate) && x.DepartmentId == departmentid && x.IsDeleted == false && x.IsActive == true);
                                if (VoucherControlDetail == null)
                                {
                                    fileMovement.Message = "VoucherControl is Missing";
                                    if (oInwardExcelcount == 1)
                                    {
                                        return fileMovement;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                string Prefix = VoucherControlDetail.Prefix.ToString();
                                long VoucherNo = VoucherControlDetail.Voucherno + 1;
                                Jobno = Prefix + month + day + "/" + VoucherNo;
                                var fileName = _unitWork.JobOrder.Where(j => j.FileName == FileName && j.ClientId == clientid && j.DepartmentId == departmentid && j.JobStatusId == jobstatusid && j.IsDeleted == false).ToList();
                                if (jobstatusid == 1 || jobstatusid == 2 || jobstatusid == 5)
                                {
                                    Jobno = Prefix + month + day + "/" + VoucherNo + "/R" + (fileName.Count == 0 ? 1 : (fileName.Count + 1));
                                }
                                //if (jobstatusid == 1 || jobstatusid == 2 || jobstatusid == 5)
                                //{
                                //    int revisionCount = GetRevisionCount(FileName);
                                //    string revisionTag = GetRevisionTag(revisionCount);
                                //    if (revisionCount >= 2)
                                //    {
                                //        RevisionEmailNotification(ClientDetail.Id, FileName, revisionCount);
                                //    }

                                //    Jobno = Prefix + month + day + "/" + VoucherNo + revisionTag;
                                //}
                                var oVoucherControl = _unitWork.VoucherControl.Find(VoucherControlDetail.Id);
                                oVoucherControl.Voucherno = VoucherNo;
                                oVoucherControl.UpdatedBy = oJobOrder.CreatedBy;
                                oVoucherControl.UpdatedUtc = DateTime.UtcNow;
                                _unitWork.VoucherControl.Update(oVoucherControl);

                                int checkdupjidexl = _unitWork.JobOrder.Count(x => x.JobId == Jobno);
                                if (checkdupjidexl == 1)
                                {
                                    goto gotostmt_exlvouch;
                                }
                                else
                                {
                                    oJobOrder.TransactionId = transactionid;
                                    oJobOrder.JobId = Jobno;
                                    oJobOrder.JobDate = DateTime.UtcNow;
                                    oJobOrder.JobDescription = ".";
                                    oJobOrder.JobStatusId = jobstatusid;
                                    oJobOrder.DepartmentId = departmentid;
                                    oJobOrder.DivisionId = divisionid;//------------------------------------------division id Job Order---------------------------------------------
                                    oJobOrder.FileReceivedDate = item.DateofReceived.ToUniversalTime();
                                    oJobOrder.FileName = FileName;
                                    oJobOrder.FileInwardTypeId = fileinwardid;
                                    oJobOrder.Username = item.Username;
                                    oJobOrder.SalesPersonName = item.SalesPersonName;
                                    if (item.PoNo != null)
                                    {
                                        oJobOrder.PoNo = item.PoNo;
                                    }
                                    if (item.DateofDelivery != null)
                                    {
                                        oJobOrder.DateofDelivery = Convert.ToDateTime(item.DateofDelivery);
                                    }
                                    if (item.ClientSalesPerson != null)
                                    {
                                        oJobOrder.CcemailId = CCPersonInfo.Email;//
                                        oJobOrder.Ccid = CCPersonInfo.Id;       //
                                        oJobOrder.ClientSalesPerson = item.ClientSalesPerson;//
                                    }
                                    oJobOrder.CustomerName = item.CustomerName;
                                    oJobOrder.Temp = item.Temp;
                                    oJobOrder.Style = item.Style;
                                    oJobOrder.ProjectCode = item.ProjectCode;
                                    oJobOrder.TeamCode = item.TeamCode;
                                    oJobOrder.SchoolName = item.SchoolName;
                                    oJobOrder.IsDeleted = false;
                                    oJobOrder.ClientId = clientid;
                                    oJobOrder.Color = item.Ground;
                                    oJobOrder.CustomerJobType = item.ClientStatus;
                                    oJobOrder.CreatedBy = inwardExcel.CreatedBy;
                                    var freshJobExistsfordate = _unitWork.JobOrder.Where(x => x.ClientId == clientid && x.FileName == FileName && x.DepartmentId == departmentid && x.IsDeleted == false).ToList();
                                    var RevisionExistsfordate = _unitWork.JobOrder.Where(x => x.ClientId == clientid && x.FileName == FileName && x.DepartmentId == departmentid && x.JobStatusId == 5 && x.IsDeleted == false).ToList(); //
                                    if (freshJobExistsfordate.Count != 0)
                                    {
                                        if (freshJobExistsfordate.Last().DateofUpload != null && freshJobExistsfordate.Last().DateofUpload < oJobOrder.JobDate.ToUniversalTime())
                                        {
                                            if (freshJobExistscount)
                                            {
                                                var parentjobid = freshJobExists.First();
                                                var referencejobid = freshJobExists.Last();
                                                oJobOrder.ParentJobId = parentjobid.JobId;
                                                oJobOrder.JobReferenceId = referencejobid.JobId;
                                            }
                                            if (jobstatusid == 7 || jobstatusid == 8 || jobstatusid == 9)
                                            {
                                                var CheckFileNameExist = _unitWork.JobOrder.Where(x => x.FileName == FileName && x.ClientId == clientid && x.DepartmentId == departmentid && (x.JobStatusId != 10 || x.JobStatusId == jobstatusid) && x.IsDeleted == false).ToList();
                                                if (CheckFileNameExist.Count != 0)
                                                {
                                                    insertJobOrder = false;
                                                    var fileinward = _unitWork.FileInwardExcel.Where(x => x.Id == item.Id).SingleOrDefault();
                                                    fileinward.Status = false;
                                                    _unitWork.FileInwardExcel.Update(fileinward);
                                                    _unitWork.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                if (RevisionExistsfordate.Count != 0)//
                                                {
                                                    if (RevisionExistsfordate.Last().DateofUpload == null)
                                                    {
                                                        insertJobOrder = false;
                                                        var fileinward = _unitWork.FileInwardExcel.Where(x => x.Id == item.Id).SingleOrDefault();
                                                        fileinward.Status = false;
                                                        _unitWork.FileInwardExcel.Update(fileinward);
                                                        _unitWork.SaveChanges();
                                                    }
                                                    else
                                                    {
                                                        insertJobOrder = true;
                                                    }

                                                }//
                                                else//
                                                {//
                                                    insertJobOrder = true;

                                                }//                                           
                                            }
                                        }
                                        else
                                        {
                                            var fileinward = _unitWork.FileInwardExcel.Where(x => x.Id == item.Id).SingleOrDefault();
                                            fileinward.Status = false;
                                            _unitWork.FileInwardExcel.Update(fileinward);
                                            _unitWork.SaveChanges();

                                        }
                                    }
                                    else
                                    {
                                        insertJobOrder = true;
                                    }

                                    if (insertJobOrder)
                                    {
                                        _unitWork.JobOrder.Add(oJobOrder);

                                        _unitWork.SaveChanges();
                                        if (jobstatusid == 1 || jobstatusid == 2 || jobstatusid == 5)
                                        {
                                            if ((fileName.Count) >= 2)
                                            {
                                                RevisionEmailNotification(ClientDetail.Id, DepartmentDetail.Id, DivisionDetail.Id, FileName, (fileName.Count + 1), fileinwardid);
                                            }
                                        }
                                        ProcessWorkFlowMaster saveProcessWorkFlowMaster = new ProcessWorkFlowMaster();
                                        saveProcessWorkFlowMaster.TransactionId = oJobOrder.TransactionId;
                                        saveProcessWorkFlowMaster.Job = oJobOrder;
                                        // saveProcessWorkFlowMaster.JobId = _tableWork.JobOrderRepository.GetLastRecord(x=>x.Id).Id;
                                        saveProcessWorkFlowMaster.CustomerId = oJobOrder.ClientId.Value;
                                        saveProcessWorkFlowMaster.DepartmentId = oJobOrder.DepartmentId;
                                        saveProcessWorkFlowMaster.IsDeleted = oJobOrder.IsDeleted;
                                        saveProcessWorkFlowMaster.CreatedBy = oJobOrder.CreatedBy;
                                        saveProcessWorkFlowMaster.CreatedUtc = DateTime.UtcNow;
                                        _unitWork.ProcessWorkFlowMaster.Add(saveProcessWorkFlowMaster);
                                        _unitWork.SaveChanges();
                                        ProcessWorkFlowTran saveProcessWorkFlowTran = new ProcessWorkFlowTran();
                                        saveProcessWorkFlowTran.Wfm = saveProcessWorkFlowMaster;
                                        //  saveProcessWorkFlowTran.Wfmid= _tableWork.ProcessWorkFlowMasterRepository.GetLastRecord(x => x.Id).Id;
                                        saveProcessWorkFlowTran.JidNavigation = oJobOrder;
                                        //   saveProcessWorkFlowTran.Jid = _tableWork.JobOrderRepository.GetLastRecord(x => x.Id).Id;
                                        saveProcessWorkFlowTran.StatusId = 1;
                                        saveProcessWorkFlowTran.DepartmentId = oJobOrder.DepartmentId;
                                        if(oJobOrder.DepartmentId == 12)
                                        {
                                            saveProcessWorkFlowTran.ProcessId = 14;
                                        }
                                        else
                                        {
                                            saveProcessWorkFlowTran.ProcessId = 2;
                                        }
                                        saveProcessWorkFlowTran.JobStatusId = oJobOrder.JobStatusId;
                                        saveProcessWorkFlowTran.IsDeleted = saveProcessWorkFlowMaster.IsDeleted;
                                        saveProcessWorkFlowTran.CreatedBy = saveProcessWorkFlowMaster.CreatedBy;
                                        saveProcessWorkFlowTran.CreatedUtc = saveProcessWorkFlowMaster.CreatedUtc;
                                        saveProcessWorkFlowTran.IsActive = true;
                                        _unitWork.ProcessWorkFlowTran.Add(saveProcessWorkFlowTran);
                                        _unitWork.SaveChanges();

                                        JobOrderExtension joe = new JobOrderExtension();
                                        joe.JidNavigation = oJobOrder;
                                        //  joe.Jid = _tableWork.JobOrderRepository.GetLastRecord(x => x.Id).Id; 
                                        joe.Date = DateTime.UtcNow;
                                        joe.Wft = saveProcessWorkFlowTran;
                                        // joe.Wftid = _tableWork.ProcessWorkFlowTranRepository.GetLastRecord(s=>s.Id).Id;
                                        _unitWork.JobOrderExtension.Add(joe);

                                        //flaten table insertion
                                        var customerShortName = _unitWork.Customer.FirstOrDefault(x => x.Id == oJobOrder.ClientId);
                                        var dept = _unitWork.Department.FirstOrDefault(x => x.Id == oJobOrder.DepartmentId);
                                        var jobStatus = _unitWork.JobStatus.FirstOrDefault(x => x.Id == oJobOrder.JobStatusId);
                                        var ccIntake = _unitWork.Employee.FirstOrDefault(x => x.EmployeeId == oJobOrder.CreatedBy);
                                        TrayDetailMovement trayDetailMovement = new TrayDetailMovement();
                                        trayDetailMovement.JobDate = DateTime.UtcNow;
                                        trayDetailMovement.FileName = oJobOrder.FileName;
                                        trayDetailMovement.ClientId = oJobOrder.ClientId;
                                        trayDetailMovement.ClientShortName = customerShortName.ShortName;
                                        trayDetailMovement.Department = dept.Description;
                                        trayDetailMovement.JobStatus = jobStatus.JobStatusDescription;
                                        trayDetailMovement.CustomerJobType = oJobOrder.CustomerJobType;
                                        trayDetailMovement.CurrentTray = "Production Allocation";
                                        trayDetailMovement.CurrentStatus = "Pending";
                                        trayDetailMovement.CcintakenBy = ccIntake.EmployeeCode + "-" + ccIntake.EmployeeName;
                                        trayDetailMovement.IsDeleted = false;
                                        trayDetailMovement.CreatedBy = oJobOrder.CreatedBy;
                                        trayDetailMovement.CreatedUtc = DateTime.UtcNow;
                                        trayDetailMovement.JobId = oJobOrder.JobId;
                                        _unitWork.TrayDetailMovement.Add(trayDetailMovement);

                                        _unitWork.SaveChanges();

                                        var fileinward = _unitWork.FileInwardExcel.Where(x => x.Id == item.Id).SingleOrDefault();
                                        FileInwardExcel ofie = _unitWork.FileInwardExcel.Find(fileinward.Id);
                                        _unitWork.FileInwardExcel.Delete(ofie);
                                        _unitWork.SaveChanges();

                                        var Getjobsummary = _unitWork.JobSummary.Where(x => x.ClientId == clientid && x.MonthYear == month + "-" + year).ToList();
                                        var GetLastRecord = Getjobsummary.LastOrDefault();
                                        if (Getjobsummary.Count == 0)
                                        {
                                            JobSummary savejobsummary = new JobSummary();
                                            savejobsummary.ClientId = clientid;
                                            savejobsummary.MonthYear = month + "-" + year;
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

                                        var jobOrder = _unitWork.JobOrder.Local().FirstOrDefault();
                                        var pwmId = _unitWork.ProcessWorkFlowMaster.Where(x => x.JobId == jobOrder.Id).Select(x => x.Id).FirstOrDefault();
                                        var pwtLast = _unitWork.ProcessWorkFlowTran.Where(x => x.Wfmid == pwmId).FirstOrDefault();
                                        fileMovement.OrderId = jobOrder.Id;
                                        fileMovement.ProcessId = pwtLast.ProcessId;
                                        fileMovement.StatusId = pwtLast.StatusId;
                                        fileMovement.JobId = jobOrder.JobId;
                                        fileMovement.IsProcessWorkFlowTranInserted = 1;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                                Log addlog = new Log();
                                addlog.Module = "SaveImportExcel";
                                addlog.Description = "Error Msg: " + ex.Message + ", Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source;
                                addlog.Type = "Error Occ";
                                addlog.CreatedUtc = DateTime.UtcNow;
                                _context.Logs.Add(addlog);
                                _context.SaveChanges();

                            }
                        }// s
                        else
                        {
                            //fileMovement.Message = "Client Sales Person Name does not Exists";
                            fileMovement.Message = "Client Sales Person Name does not Exists / File Name Already Exist";
                            var fileinward = _unitWork.FileInwardExcel.Where(x => x.Id == item.Id).SingleOrDefault();
                            if (fileinward != null)
                            {
                                fileinward.Status = false;
                            }
                            _unitWork.FileInwardExcel.Update(fileinward);
                            _unitWork.SaveChanges();
                        }
                    }

                } //foreachend
                if (glbcount >= 1)
                {
                    var cnamelocal = GetCustomerCollection.ToList();
                    value = string.Join(", ", from item in cnamelocal select item.CustomerName);
                    fileMovement.Message = "File Inward Successful,Some Order has been blocked temporarily due to Invoice Outstanding for customer- " + value + ",Kindly contact respective sales person/service desk.";
                    return fileMovement;
                }
                fileMovement.Message = "File Inward Successful";
            }
            return fileMovement;
        }
        ////internal order
        public FileMovement InternalOrder(InwardExcel inwardExcel)
        {
            var fileMovement = new FileMovement();
            IEnumerable<FileInwardExcel> oInwardExcel = inwardExcel.ViewDatas;
            var boolresult = GetBlockedResult(inwardExcel.ClientId);
            if (boolresult == true)
            {
                //return outstanding customer name & mail - block
                fileMovement.JobId = "outstanding";
                return fileMovement;
            }
            else
            {
                //allow to place an order - unblock
                if (oInwardExcel == null || oInwardExcel.Count<FileInwardExcel>() == 0) //Internal JobOrder Creation
                {
                    if (inwardExcel.JobStatusId == 7 || inwardExcel.JobStatusId == 8 || inwardExcel.JobStatusId == 9)
                    {
                        // var CheckFileNameExist = _unitWork.JobOrder.Where(x => x.FileName == inwardExcel.FileName && x.ClientId == inwardExcel.ClientId && x.DepartmentId == inwardExcel.DepartmentId && x.JobStatusId != 10 && x.IsDeleted == false).ToList();//------------------old code changed for division id Job Order---------
                        var CheckFileNameExist = _unitWork.JobOrder.Where(x => x.FileName == inwardExcel.FileName && x.ClientId == inwardExcel.ClientId && x.DepartmentId == inwardExcel.DepartmentId && x.DivisionId == inwardExcel.DivisionId && x.JobStatusId != 10 && x.IsDeleted == false).ToList();//----------------------division id Job Order-------------------------------
                        if (CheckFileNameExist.Count != 0)
                        {
                            fileMovement.JobId = "File Name Already Exist!";
                            return fileMovement;
                        }
                    }
                    var FileinwardtypeDetail = _unitWork.FileInwardType.Where(x => x.Description.Contains(inwardExcel.FileInwardMode)).FirstOrDefault();
                    int jobStatusId = inwardExcel.JobStatusId;
                    var jobStatusGroupId = _unitWork.JobStatus.Where(x => x.Id == jobStatusId).Select(x => x.JobStatusGroupId).FirstOrDefault();
                    DateTime fileReceivedDate = inwardExcel.FileReceivedDate;
                    var transactionTypeId = _unitWork.TransactionType.Where(x => x.JobStatusGroupId == jobStatusGroupId).Select(x => x.Id).FirstOrDefault();

                    string month = fileReceivedDate.ToString("MM");
                    string day = fileReceivedDate.ToString("dd");
                    string dynamicJobId = "";


                    // Mapper.Initialize(x => x.CreateMap<InwardExcel, JobOrder>());
                    var jobOrder = _mapper.Map<JobOrder>(inwardExcel);//Mapper.Map<JobOrder>(inwardExcel);

                gotostmt_intvoucher:

                    var voucherControl = _unitWork.VoucherControl.FirstOrDefault(x => x.TransactionId == transactionTypeId && (x.EffectiveFrom <= fileReceivedDate && x.EffectiveTo >= fileReceivedDate) && x.DepartmentId == inwardExcel.DepartmentId && x.IsDeleted == false && x.IsActive == true);
                    if (voucherControl == null)
                    {
                        fileMovement.Message = "VoucherControl is Missing";
                        return fileMovement;
                    }
                    string voucherControlPrefix = voucherControl.Prefix.ToString();
                    long voucherNo = voucherControl.Voucherno + 1;
                    dynamicJobId = voucherControlPrefix + month + day + "/" + voucherNo;
                    var filesName = _unitWork.JobOrder.Where(x => x.FileName == inwardExcel.FileName && x.ClientId == inwardExcel.ClientId && x.DepartmentId == inwardExcel.DepartmentId && x.JobStatusId == jobStatusId && x.IsDeleted == false).ToList();
                    if (jobStatusId == 1 || jobStatusId == 2 || jobStatusId == 5)
                    {
                        dynamicJobId = voucherControlPrefix + month + day + "/" + voucherNo + "/R" + (filesName.Count == 0 ? 1 : (filesName.Count + 1));
                    }
                    voucherControl.Voucherno = voucherNo;
                    voucherControl.UpdatedBy = jobOrder.CreatedBy;
                    voucherControl.UpdatedUtc = DateTime.UtcNow;
                    _unitWork.VoucherControl.Update(voucherControl);

                    int checkdupjidint = _unitWork.JobOrder.Count(x => x.JobId == dynamicJobId);
                    if (checkdupjidint == 1)
                    {
                        goto gotostmt_intvoucher;
                    }
                    else
                    {
                        jobOrder.TransactionId = transactionTypeId;
                        jobOrder.JobId = dynamicJobId;
                        jobOrder.DivisionId = inwardExcel.DivisionId; //------------------------------------division id Job Order-------------------------------------------------
                        jobOrder.IsDeleted = false;
                        jobOrder.ClientOrderId = null;
                        jobOrder.FileReceivedDate = inwardExcel.FileReceivedDate.AddHours(5).ToUniversalTime();
                        jobOrder.JobDate = DateTime.UtcNow;
                        var checkFreshJobOrder = _unitWork.JobOrder.Where(x => x.ClientId == inwardExcel.ClientId && x.FileName == inwardExcel.FileName && x.DepartmentId == inwardExcel.DepartmentId && x.IsDeleted == false).ToList();
                        var checkFreshJobOrderList = _unitWork.JobOrder.Where(x => x.ClientId == inwardExcel.ClientId && x.FileName.Equals(inwardExcel.FileName) && x.DepartmentId == inwardExcel.DepartmentId && x.IsDeleted == false).OrderBy(x => x.Id).ToList();
                        var insertJobOrdersingle = false;
                        if (checkFreshJobOrder.Count != 0)
                        {
                            if (checkFreshJobOrder.Last().DateofUpload != null && checkFreshJobOrder.Last().DateofUpload < jobOrder.JobDate.ToUniversalTime())
                            {
                                if (checkFreshJobOrderList.Count > 0)
                                {
                                    var parentjobidsingle = checkFreshJobOrderList.First();
                                    var referencejobidsingle = checkFreshJobOrderList.Last();

                                    jobOrder.ParentJobId = parentjobidsingle.JobId;
                                    jobOrder.JobReferenceId = referencejobidsingle.JobId;
                                }
                                insertJobOrdersingle = true;
                            }
                            else
                            {
                                fileMovement.JobId = "Previous Job is not closed for the File Name and Client!";
                                return fileMovement;
                            }
                        }
                        else
                        {
                            insertJobOrdersingle = true;
                        }
                        if (insertJobOrdersingle)
                        {
                            _unitWork.JobOrder.Add(jobOrder);
                            _unitWork.SaveChanges();
                            if (jobStatusId == 1 || jobStatusId == 2 || jobStatusId == 5)
                            {
                                if ((filesName.Count) >= 2)
                                {
                                    RevisionEmailNotification(inwardExcel.ClientId, inwardExcel.DepartmentId, inwardExcel.DivisionId, inwardExcel.FileName, (filesName.Count + 1), FileinwardtypeDetail.Id);
                                }
                            }

                            ProcessWorkFlowMaster processWorkFlowMaster = new ProcessWorkFlowMaster();
                            processWorkFlowMaster.TransactionId = jobOrder.TransactionId;
                            processWorkFlowMaster.CustomerId = jobOrder.ClientId.Value;
                            processWorkFlowMaster.DepartmentId = jobOrder.DepartmentId;
                            processWorkFlowMaster.IsDeleted = jobOrder.IsDeleted;
                            processWorkFlowMaster.CreatedBy = jobOrder.CreatedBy;
                            processWorkFlowMaster.CreatedUtc = DateTime.UtcNow;
                            processWorkFlowMaster.JobId = _tableWork.JobOrderRepository.GetLastRecord(s => s.Id).Id;
                            _unitWork.ProcessWorkFlowMaster.Add(processWorkFlowMaster);
                            _unitWork.SaveChanges();
                            ProcessWorkFlowTran processWorkFlowTran = new ProcessWorkFlowTran();
                            processWorkFlowTran.Jid = _tableWork.JobOrderRepository.GetLastRecord(s => s.Id).Id;
                            processWorkFlowTran.StatusId = 1;
                            processWorkFlowTran.DepartmentId = jobOrder.DepartmentId;
                            processWorkFlowTran.PreviousProcessId = null;
                            // For training department
                            if (jobOrder.DepartmentId == 12)
                            {
                                processWorkFlowTran.ProcessId = 14;
                            }
                            else
                            {
                                processWorkFlowTran.ProcessId = 2;
                            }
                            processWorkFlowTran.JobStatusId = jobOrder.JobStatusId;
                            processWorkFlowTran.IsDeleted = processWorkFlowMaster.IsDeleted;
                            processWorkFlowTran.CreatedBy = jobOrder.CreatedBy;
                            processWorkFlowTran.CreatedUtc = DateTime.UtcNow;
                            processWorkFlowTran.IsActive = true;
                            processWorkFlowTran.Wfmid = _tableWork.ProcessWorkFlowMasterRepository.GetLastRecord(s => s.Id).Id;
                            _unitWork.ProcessWorkFlowTran.Add(processWorkFlowTran);
                            _unitWork.SaveChanges();
                            JobOrderExtension joe = new JobOrderExtension();
                            joe.Jid = _tableWork.JobOrderRepository.GetLastRecord(s => s.Id).Id;
                            joe.Date = DateTime.UtcNow;
                            joe.Wftid = _tableWork.ProcessWorkFlowTranRepository.GetLastRecord(s => s.Id).Id;//processWorkFlowTran.Id;
                            _unitWork.JobOrderExtension.Add(joe);



                            int year = jobOrder.FileReceivedDate.Year;
                            var Getjobsummary = _unitWork.JobSummary.Where(x => x.ClientId == inwardExcel.ClientId && x.MonthYear == month + "-" + year).ToList();
                            var GetLastRecord = Getjobsummary.LastOrDefault();
                            if (Getjobsummary.Count == 0)
                            {
                                JobSummary savejobsummary = new JobSummary();
                                savejobsummary.ClientId = inwardExcel.ClientId;
                                savejobsummary.MonthYear = month + "-" + year;
                                if (GetLastRecord == null)
                                {
                                    savejobsummary.Pending = 1;
                                    savejobsummary.TotalFiles = 1;
                                }
                                _unitWork.JobSummary.Add(savejobsummary);

                            }
                            else
                            {
                                var Updatejobsummary = _unitWork.JobSummary.Where(x => x.Id == GetLastRecord.Id).FirstOrDefault();
                                Updatejobsummary.Pending = GetLastRecord.Pending + 1;
                                Updatejobsummary.TotalFiles = GetLastRecord.TotalFiles + 1;
                                _unitWork.JobSummary.Update(Updatejobsummary);

                            }

                            //flaten table insertion
                            var customerShortName = _unitWork.Customer.Where(x => x.Id == jobOrder.ClientId).FirstOrDefault();
                            var dept = _unitWork.Department.Where(x => x.Id == jobOrder.DepartmentId).FirstOrDefault();
                            var jobStatus = _unitWork.JobStatus.Where(x => x.Id == jobOrder.JobStatusId).FirstOrDefault();
                            var ccIntake = _unitWork.Employee.Where(x => x.EmployeeId == jobOrder.CreatedBy).FirstOrDefault();
                            var empname = _unitWork.Employee.Where(x => x.EmployeeId == inwardExcel.EmployeeId).FirstOrDefault();
                            var classiid = _unitWork.Customer.FirstOrDefault(x => x.Id == inwardExcel.ClientId).CustomerClassificationId;
                            var classification = _unitWork.CustomerClassification.FirstOrDefault(x => x.Id == classiid).Description;

                            TrayDetailMovement trayDetailMovement = new TrayDetailMovement();
                            trayDetailMovement.JobDate = DateTime.UtcNow;
                            trayDetailMovement.FileName = inwardExcel.FileName;
                            trayDetailMovement.ClientId = inwardExcel.ClientId;
                            trayDetailMovement.ClientShortName = customerShortName.ShortName;
                            trayDetailMovement.Department = dept.Description;
                            trayDetailMovement.JobStatus = jobStatus.JobStatusDescription;
                            trayDetailMovement.CustomerJobType = jobOrder.CustomerJobType;
                            trayDetailMovement.CurrentTray = "Production Allocation";
                            trayDetailMovement.CurrentStatus = "Pending";
                            trayDetailMovement.CcintakenBy = ccIntake.EmployeeCode + "-" + ccIntake.EmployeeName;
                            trayDetailMovement.IsDeleted = false;
                            trayDetailMovement.CreatedBy = inwardExcel.EmployeeId;
                            trayDetailMovement.CreatedUtc = DateTime.UtcNow;
                            //trayDetailMovement.JId = jobOrder.Id;
                            trayDetailMovement.JobId = jobOrder.JobId;
                            trayDetailMovement.DepartmentId = inwardExcel.DepartmentId;
                            trayDetailMovement.EmployeeId = inwardExcel.EmployeeId;
                            trayDetailMovement.EmployeeName = empname.EmployeeCode + "-" + empname.EmployeeName;
                            trayDetailMovement.StatusId = 1;
                            trayDetailMovement.ClassificationId = classiid;
                            trayDetailMovement.Classification = classification;
                            trayDetailMovement.CcintakenDate = DateTime.UtcNow;
                            _unitWork.TrayDetailMovement.Add(trayDetailMovement);
                            //                        
                            _unitWork.SaveChanges();

                            var jobOrder1 = _unitWork.JobOrder.Local().FirstOrDefault();
                            var pwmId = _unitWork.ProcessWorkFlowMaster.Where(x => x.JobId == jobOrder1.Id).Select(x => x.Id).FirstOrDefault();
                            var pwtLast = _unitWork.ProcessWorkFlowTran.Where(x => x.Wfmid == pwmId).FirstOrDefault();
                            fileMovement.OrderId = jobOrder1.Id;
                            fileMovement.ProcessId = pwtLast.ProcessId;
                            fileMovement.StatusId = pwtLast.StatusId;
                            fileMovement.JobId = jobOrder1.JobId;
                            fileMovement.IsProcessWorkFlowTranInserted = 1;
                        }
                    }
                }
            }

            return fileMovement;
        }
        ////
        //genric method to block orders
        public bool GetBlockedResult(int ClientId)
        {
            bool result = false;

            var cus = _unitWork.Customer.FirstOrDefault(x => x.Id == ClientId);
            if (cus != null)
            {
                if (cus.CreditDays != null)
                {
                    var cancelledInvoices = (from A in _tableWork.InvoiceTranRepository.Get().Where(x => x.InvoiceNo != null)
                                             join B in _tableWork.InvoiceMasterRepository.Get() on A.Imid equals B.Id
                                             select B);


                    var cancelledInvoices2 = (from invMaster in _tableWork.InvoiceMasterRepository.Get()
                                              join tran in _tableWork.InvoiceTranRepository.Get().Where(x => x.InvoiceNo != null) on invMaster.InvoiceNo equals tran.InvoiceNo
                                              select invMaster);

                    var AllCancelledInvoices = cancelledInvoices.Union(cancelledInvoices2);

                    //------fixed invoice bugs
                    var outstandingDetailsBasedOnCustomer = (from i in _tableWork.InvoiceMasterRepository.Get().Except(AllCancelledInvoices)
                                                             where (i.IsOutstanding == true && i.IsDeleted == false && i.CustomerId == ClientId && i.InvoiceNo != null && i.InvoiceDate != null)
                                                             select new { InvoiceDate = i.InvoiceDate.Value.Date, i.Id, i.InvoiceNo, i.CustomerId }).ToList();
                    //------fixed invoice bugs


                    var GetCDVMList = new List<CreditDaysOutHighVM>();
                    int? CustomerLimit = 0;
                    if (outstandingDetailsBasedOnCustomer.Count() > 0)
                    {
                        foreach (var removetime in outstandingDetailsBasedOnCustomer)
                        {
                            DateTime current = DateTime.UtcNow.Date;
                            int TotalDaysCount = Convert.ToInt32((current - removetime.InvoiceDate).TotalDays);

                            GetCDVMList.Add(new CreditDaysOutHighVM
                            {
                                Id = removetime.Id,
                                InvoiceNo = removetime.InvoiceNo,
                                InvoiceDate = removetime.InvoiceDate,
                                CustomerID = removetime.CustomerId,
                                TotalDays = TotalDaysCount + 1
                            });
                        }
                        var highestOSDays = GetCDVMList.OrderByDescending(x => x.TotalDays).FirstOrDefault();
                        CustomerLimit = _unitWork.Customer.FirstOrDefault(x => x.Id == highestOSDays.CustomerID).CreditDays;
                        if (highestOSDays.TotalDays > CustomerLimit)
                        {
                            //true - check request
                            var checkJAE = _unitWork.JobApprovalExt.Where(x => x.IsApproved == true && x.IsPeriodElapsed != true && x.ClientId == ClientId).ToList();
                            if (checkJAE.Count() >= 1) // need to change == after client side modification
                            {
                                return false;
                            }
                            else
                            {
                                //block
                                return true;
                            }
                        }
                        else
                        {
                            //unblock
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            return result;
        }
        //
        public IEnumerable<GetEmployee> GetAssignedEmployees(ViewJobAllocation CustomerJobType)
        {
            var processWorkFlowTran = _tableWork.ProcessWorkFlowTranRepository.Get(x => x.Id == CustomerJobType.TranId).FirstOrDefault();
            var getEmployees = new List<ViewGetEmployeesForPendingJob>();
            if (CustomerJobType.ProcessId == null || CustomerJobType.ProcessId == 0)
            {
                getEmployees = _viewWork.ViewGetEmployeesForPendingJobsRepository.Get(x => x.Jid == processWorkFlowTran.Jid && x.IsActive == true).ToList();
            }
            else
            {
                getEmployees = _viewWork.ViewGetEmployeesForPendingJobsRepository.Get(x => x.Jid == processWorkFlowTran.Jid && x.ProcessId == CustomerJobType.ProcessId && x.IsActive == true).ToList();
            }
            var GetEmployee = new List<GetEmployee>();
            foreach (var item in getEmployees)
            {
                string EmpName = item.EmployeeName;
                string EmpCode = item.EmployeeCode;
                string EmployeenameWithCode = EmpCode + " - " + EmpName;
                GetEmployee.Add(new GetEmployee
                {
                    EmployeeId = item.EmployeeId,
                    EmployeeName = item.EmployeeName,
                    EmployeeCode = item.EmployeeCode,
                    EstTime = item.EstimatedTime,
                    Status = item.Status,
                    EmployeenameWithCode = EmployeenameWithCode,
                    WFMId = item.Wfmid,
                    JId = item.Jid
                });
            }
            return GetEmployee;
        }
        public IEnumerable<ViewJobAllocationandJobOrder> GetPendingJobsWithEmployeeId(ViewJobAllocation CustomerJobType)
        {
            var allocatedJobs = new List<ViewJobAllocationandJobOrder>();
            if (CustomerJobType.ProcessId == null)
            {
                allocatedJobs = _viewWork.ViewJobAllocationandJobOrderRepository.Get(x => (x.EmployeeId == CustomerJobType.EmployeeId) && (x.Status == "Assigned" || x.Status == "In Process") && (x.IsActive.Equals(true))).ToList();
            }
            else
            {
                allocatedJobs = _viewWork.ViewJobAllocationandJobOrderRepository.Get(x => (x.EmployeeId == CustomerJobType.EmployeeId) && (x.ProcessId == CustomerJobType.ProcessId) && (x.Status == "Assigned" || x.Status == "In Process") && (x.IsActive.Equals(true))).ToList();
            }
            foreach (var allocatedJob in allocatedJobs)
            {
                allocatedJob.Status = _tableWork.EmployeeAssignRepository.Get(x => x.Wftid == allocatedJob.Wftid).OrderByDescending(x => x.Id).Select(x => x.Status).FirstOrDefault();
            }

            return allocatedJobs;
        }
        public IEnumerable<ViewFileInwardExcel> GetImportExcel(int EmployeeId)
        {
            return _viewWork.ViewFileInwardExcelRepository.Get(x => x.UploadedBy == EmployeeId).OrderByDescending(x => x.Status == false).ToList();
        }
        public IEnumerable<FileInwardExcel> GetImportExcelTrue(int EmployeeId)
        {
            return _tableWork.FileInwardExcelRepository.Get(x => x.Status == true && x.UploadedBy == EmployeeId).OrderByDescending(x => x.DateofReceived).AsQueryable();
        }
        public IEnumerable<Scope> openFolder(FileMovement folderPath)
        {
            System.Diagnostics.Process.Start(@"\\172.16.36.97\JobFiles\" + folderPath.FolderPath);
            var scopeDetails = new List<Scope>();
            return scopeDetails;

        }
        public void CancelImportExcel()
        {
            // using (ApplicationDbContext _db = new ApplicationDbContext(_configuration))
            // {


            _db.Database.ExecuteSqlRaw("TRUNCATE TABLE [FileInwardExcel]");

            // }
        }
        public IEnumerable<sp_NonBillableJobs_Result> GetNonBillableData(NonBillablePara para)
        {
            // ApplicationDbContext db = new ApplicationDbContext(_configuration);
            var sqlpara = new SqlParameter[]
                        {
                            new SqlParameter("@FromDate",  para.FromDate.Date),
                            new SqlParameter("@ToDate",  para.ToDate.Date),
                            new SqlParameter("@ClientId", (object)para.ClientId ?? DBNull.Value),
                            new SqlParameter("@DepartmentId", (object)para.DepartmentId ?? DBNull.Value)
                        };

            var spresult = _procedureWork.ExecStoredProcedure<sp_NonBillableJobs_Result>("sp_NonBillableJobs @FromDate,@ToDate,@ClientId,@DepartmentId", sqlpara).ToList();
            return spresult;
        }
        public IEnumerable<NonBillable> GetNonBillable()
        {
            // ApplicationDbContext db = new ApplicationDbContext(_configuration);
            var result = db.NonBillables.Where(x => x.IsDeleted == false).ToList();
            return result;
        }
        public string UpdateNonBillable(NBJIdPara GetNBPara)
        {
            string message = "Updated Successfuly";
            var selectedId = GetNBPara.GetNBPara;
            int NBId = GetNBPara.NonBillableId;
            var jid = selectedId.Select(x => x.Id);
            var updateOrders = _unitWork.JobOrder.Where(x => jid.Contains(x.Id)).ToList();
            updateOrders.ForEach(x => x.NonBillableStatusId = NBId);
            _unitWork.SaveChanges();
            return message;
        }
        //2019-05-30
        //credit days approval service
        public Object GetServiceforCreditDays()
        {
            //  ApplicationDbContext db = new ApplicationDbContext(_configuration);

            var cancelledInvoices = (from A in _tableWork.InvoiceTranRepository.Get().Where(x => x.InvoiceNo != null)
                                     join B in _tableWork.InvoiceMasterRepository.Get() on A.Imid equals B.Id
                                     select B);


            var cancelledInvoices2 = (from invMaster in _tableWork.InvoiceMasterRepository.Get()
                                      join tran in _tableWork.InvoiceTranRepository.Get().Where(x => x.InvoiceNo != null) on invMaster.InvoiceNo equals tran.InvoiceNo
                                      select invMaster);

            var AllCancelledInvoices = cancelledInvoices.Union(cancelledInvoices2);


            var JAE = _tableWork.JobApprovalExtRepository.Get().Where(x => x.IsApproved == true && x.IsPeriodElapsed != true).OrderByDescending(x => x.Id).ToList();

            foreach (var item in JAE)
            {
                var GetCDVMList = new List<CreditDaysOutHighVM>();
                int? NewLimit = 0;

                if (item.ClientId != null)
                {
                    var outstandingDetailsBasedOnCustomer = (from i in _tableWork.InvoiceMasterRepository.Get().Except(AllCancelledInvoices)
                                                             where (i.IsOutstanding == true && i.IsDeleted == false && i.CustomerId == item.ClientId)
                                                             select new { InvoiceDate = i.InvoiceDate.Value.Date, i.Id, i.InvoiceNo, i.CustomerId }).ToList();
                    if (outstandingDetailsBasedOnCustomer.Count() > 0)
                    {
                        foreach (var removetime in outstandingDetailsBasedOnCustomer)
                        {
                            DateTime current = DateTime.UtcNow.Date;
                            int TotalDaysCount = Convert.ToInt32((current - removetime.InvoiceDate).TotalDays);

                            GetCDVMList.Add(new CreditDaysOutHighVM
                            {
                                Id = removetime.Id,
                                InvoiceNo = removetime.InvoiceNo,
                                InvoiceDate = removetime.InvoiceDate,
                                CustomerID = removetime.CustomerId,
                                TotalDays = TotalDaysCount + 1
                            });
                        }
                        var highestOSDays = GetCDVMList.OrderByDescending(x => x.TotalDays).FirstOrDefault();
                        NewLimit = db.Customers.FirstOrDefault(x => x.Id == highestOSDays.CustomerID).CreditDays + item.CreditDays;
                        if (highestOSDays.TotalDays > NewLimit)
                        {
                            //update IsPeriodElapsed
                            var updateJAE = _unitWork.JobApprovalExt.FirstOrDefault(x => x.Id == item.Id);
                            updateJAE.IsPeriodElapsed = true;
                            _unitWork.JobApprovalExt.Update(updateJAE);
                            _unitWork.SaveChanges();
                        }
                        else
                        {
                            var updateJAE = _unitWork.JobApprovalExt.FirstOrDefault(x => x.Id == item.Id);
                            updateJAE.IsPeriodElapsed = false;
                            _unitWork.JobApprovalExt.Update(updateJAE);
                            _unitWork.SaveChanges();
                        }
                    }
                }
            }

            return null;
        }

        public bool SendMailServiceCreditDays()
        {
            //int localco = db.ClientOrders.Local.FirstOrDefault().OrderId;
            var url = _configuration.GetSection("JobOrder").GetValue<string>("apiurl");//ConfigurationManager.AppSettings["apiurl"];
            var request = (HttpWebRequest)WebRequest.Create(url + "api/GenricMail/CreditDays");
            //var postData = "OrderId=" + localco;
            //var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentLength = data.Length;
            //using (var stream = request.GetRequestStream())
            //{
            //    stream.Write(data, 0, data.Length);
            //}
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return true;
        }
        //private int GetRevisionCount(string filesName)
        //{
        //    var CheckFileName = _unitWork.JobOrder
        //            .Where(x => x.FileName == filesName && (x.JobStatusId == 1 || x.JobStatusId == 2 || x.JobStatusId == 5) && x.IsDeleted == false);
        //    int existingRevisions = isFirstExecution ? 0 : 1;
        //    existingRevision += existingRevisions;
        //    isFirstExecution = false;
        //    return existingRevision;
        //}

        //private string GetRevisionTag(int revisionCount)
        //{
        //    if (revisionCount == 0)
        //    {
        //        return "/R";
        //    }
        //    else
        //    {
        //        return "/R" + revisionCount;
        //    }
        //}

        public object RevisionEmailNotification(int customerId, int departmentId, int divisionId, string fileName, int revisionCount, int fileInwardTypeId)
        {
            var message = new MailMessage();
            string alert = "Mail sent successfully";

            try
            {
                var customerDetails = _context.Customers
                   .Where(c => c.Id == customerId && !c.IsDeleted)
                   .Select(c => new
                   {
                       CustomerName = c.ShortName
                   })
                   .FirstOrDefault();

                var departmentDetails = _context.Departments
                   .Where(d => d.Id == departmentId && !d.IsDeleted)
                   .Select(d => new
                   {
                       DepartmentName = d.Description
                   })
                   .FirstOrDefault();

                var divisionDetails = _context.Divisions
                   .Where(div => div.Id == divisionId && div.IsDeleted == false)
                   .Select(div => new
                   {
                       DivisionName = div.DivisionName
                   })
                   .FirstOrDefault();

                var jobDetails = _context.JobOrders
                   .Where(j => j.FileName == fileName && !j.IsDeleted)
                   .Select(j => new
                   {
                       JId = j.Id,
                       JobId = j.JobId
                   })
                   .OrderByDescending(j => j.JId)
                   .FirstOrDefault();

                var fileInwardModeDetails = _context.FileInwardTypes
                   .Where(f => f.Id == fileInwardTypeId && !f.IsDeleted)
                   .Select(f => new
                   {
                       FileInwardMode = f.Description
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
                //message.To.Add(new MailAddress("prasanabalaji@vleadservices.com"));
                message.CC.Add(new MailAddress("vsowmya@vleadservices.com"));

                var currentDate = DateTime.Now;
                var day = currentDate.Day;
                var monthYear = currentDate.ToString("MMM yyyy");
                string daySuffix = day % 10 == 1 && day != 11 ? "st"
                                 : day % 10 == 2 && day != 12 ? "nd"
                                 : day % 10 == 3 && day != 13 ? "rd"
                                 : "th";
                var formattedDate = $"{day}{daySuffix} {monthYear}";

                message.Subject = $"More than 2 revision file detail for {formattedDate}";

                message.Body = $"Hi {managerDetails.ManagerName} and All,<br/><br/>" +
                               $"We have received below files more than 2 revisions. This for your notification and action.<br/><br/>" +
                               $"<table border=\"1\"><tr style=\"background-color:#e6e6e6; color: #595959\"><th>S#</th><th>Client Code</th><th>Department</th><th>Division</th><th>Job ID</th><th>File Name</th><th>Revision Count</th><th>File Inward Mode</th></tr>" +
                               $"<tr>" +
                               $"<td>{1}</td>" +
                               $"<td>{customerDetails.CustomerName}</td>" +
                               $"<td>{departmentDetails.DepartmentName}</td>" +
                               $"<td>{divisionDetails.DivisionName}</td>" +
                               $"<td>{jobDetails.JobId}</td>" +
                               $"<td>{fileName}</td>" +
                               $"<td>{revisionCount}</td>" +
                               $"<td>{fileInwardModeDetails.FileInwardMode}</td>" +
                               $"</tr>" +
                               $"</table><br/><br/>Thanks,<br/>" +
                               $"VIS Support";

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

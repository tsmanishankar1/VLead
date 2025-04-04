using static Vis.VleadProcessV3.Services.AccountService;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.ViewModels;
using Microsoft.EntityFrameworkCore;

using System.Configuration;
using Microsoft.Data.SqlClient;
using Vis.VleadProcessV3.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.Intrinsics.X86;
using System.Security.Policy;

namespace Vis.VleadProcessV3.Services
{

    public class AllocationService
    {
        private readonly ViewWork _viewWork;
        private readonly TableWork _tableWork;
        private readonly ProcedureWork _procedureWork;
        private readonly IConfiguration _configuration;
        private readonly FileUpload fileUpload;
        private readonly UnitWork _unitWork;
        private readonly UnitViewWork _unitviewWork;
        private readonly ApplicationDbContext _context;                   
        private readonly TableWork _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AllocationService(IConfiguration configuration, ViewWork viewWork,
            TableWork tableWork, ProcedureWork procedureWork,
            UnitWork unitWork, UnitViewWork unitViewWork, FileUpload fileUpload,
            ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this.fileUpload = fileUpload;
            _webHostEnvironment = webHostEnvironment;
            _context = dbContext;
            _configuration = configuration;
            _viewWork = viewWork;
            _db = tableWork;
            _tableWork = tableWork;
            _procedureWork = procedureWork;
            _unitWork = unitWork;
            _unitviewWork = unitViewWork;
            
        }
       
        public Object GetPendingJobs(int EmployeeId, int ProcessId)
        {
            ////var pendingJobs = _viewWork.ViewJobDetailsRepository.Get(x => x.ProcessId == ProcessId && x.StatusId == 1 && x.PreviousProcessId == null && x.IsDeleted == false && x.IsActive == true).OrderByDescending(x => x.TranId).AsQueryable();
            //var cancelledJobs = _viewWork.ViewJobDetailsRepository.Get(x => x.ProcessId == ProcessId && (x.StatusId == 5 || x.StatusId == 3 || x.StatusId == 13 || x.StatusId == 14 || x.StatusId == 15 || x.StatusId == 16) && x.IsDeleted == false && x.IsActive == true && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).AsQueryable();
            var cancelledJobs = _unitviewWork.ViewJobDetails.Where(x => x.ProcessId == ProcessId && (x.StatusId == 5 || x.StatusId == 3 || x.StatusId == 13 || x.StatusId == 14 || x.StatusId == 15 || x.StatusId == 16) && x.IsDeleted == false && x.IsActive == true && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).ToList();
            //var quotationJobs = _viewWork.ViewJobDetailsRepository.Get(x => x.ProcessId == ProcessId && (x.StatusId == 19) && x.IsDeleted == false && x.IsActive == true && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).AsQueryable();
            var quotationJobs = _unitviewWork.ViewJobDetails.Where(x => x.ProcessId == ProcessId && (x.StatusId == 19) && x.IsDeleted == false && x.IsActive == true && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).ToList();
            var result = new
            {
                //PendingJobs = pendingJobs,
                CancelledJobs = cancelledJobs,
                QuotationJobs = quotationJobs
            };
            return result;
        }

        public Object GetCountForCC(int EmployeeId, int ProcessId)
        {
            var result = new Object();
            try
            {
                //var jobdetail = _viewWork.ViewJobDetailsRepository.Get(x => x.ProcessId == ProcessId && x.IsActive == true && x.IsDeleted == false && x.DivEmpId == EmployeeId);
                var jobdetail = _unitviewWork.ViewJobDetails.Where(x => x.ProcessId == ProcessId && x.IsActive == true && x.IsDeleted == false && x.DivEmpId == EmployeeId).ToList();
                //var Queryandresponsedetail = _viewWork.ViewJobQueryInCCRepository.Get(x => x.ProcessId == ProcessId && x.IsActive == true && x.IsDeleted == false && x.DivEmpId == EmployeeId);
                var Queryandresponsedetail = _unitviewWork.ViewJobQueryInCC.Where(x => x.ProcessId == ProcessId && x.IsActive == true && x.IsDeleted == false && x.DivEmpId == EmployeeId).ToList();
                int quotationJobsCount = 0, queryResponseJobsCount = 0, queriesJobsCount = 0, cancelledJobsCount = 0;
                if (ProcessId == 1)
                {
                    cancelledJobsCount = jobdetail.Count(x => x.StatusId == 5 || x.StatusId == 3 || x.StatusId == 13 || x.StatusId == 14 || x.StatusId == 15 || x.StatusId == 16);
                    quotationJobsCount = jobdetail.Count(x => x.StatusId == 19);
                    queriesJobsCount = Queryandresponsedetail.Count(x => x.StatusId == 6 || x.StatusId == 8);
                    queryResponseJobsCount = Queryandresponsedetail.Count(x => x.StatusId == 7 || x.StatusId == 9);
                }
                result = new
                {
                    QueriesJobsCount = queriesJobsCount,
                    QueryResponseJobsCount = queryResponseJobsCount,
                    CancelledJobsCount = cancelledJobsCount,
                    QuotationJobCount = quotationJobsCount,
                };
            }
            catch (Exception ex)
            {
               using (var transaction =_context.Database.BeginTransaction())
               {
                    Log addlog = new Log();
                    addlog.Module = "GetCount Function in Allocation Rep.";
                    addlog.Description = "Error Msg GetCount Function: " + ProcessId + "-" + ex.Message.ToString() + "-" + "Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source + "string";
                    addlog.Type = "Error Occ";
                    addlog.CreatedUtc= DateTime.UtcNow;
                    _context.Logs.Add(addlog);
                    _context.SaveChanges();
                    transaction.Commit();
                }
            }
            return result;
        }


        public Object GetQueryPendingJobs(int EmployeeId, int ProcessId, int DepartmentId)
        {
            var employee = _tableWork.EmployeeRepository.Get(x => x.EmployeeId == EmployeeId && x.DateOfResignation == null).FirstOrDefault();
            var queryPendingJobs = new List<ViewJobQueryInCc>();
            if (ProcessId == 1 || (DepartmentId == 0 && ProcessId == 6))
            {
                //queryPendingJobs = _viewWork.ViewJobQueryInCCRepository.Get(x => x.ProcessId == ProcessId && (x.StatusId == 6 || x.StatusId == 8) && x.IsActive == true && x.IsDeleted == false && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).ToList();
                queryPendingJobs = _unitviewWork.ViewJobQueryInCC.Where(x => x.ProcessId == ProcessId && (x.StatusId == 6 || x.StatusId == 8) && x.IsActive == true && x.IsDeleted == false && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).ToList();
            }
            else if (ProcessId == 6)
            {
                //queryPendingJobs = _viewWork.ViewJobQueryInCCRepository.Get(x => x.ProcessId == ProcessId && (x.StatusId == 6 || x.StatusId == 8) && x.IsActive == true && x.IsDeleted == false && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).ToList(); // && x.DepartmentId == DepartmentId
                queryPendingJobs = _unitviewWork.ViewJobQueryInCC.Where(x => x.ProcessId == ProcessId && (x.StatusId == 6 || x.StatusId == 8) && x.IsActive == true && x.IsDeleted == false && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).ToList(); // && x.DepartmentId == DepartmentId
            }
            else
            {
                //queryPendingJobs = _viewWork.ViewJobQueryInCCRepository.Get(x => x.ProcessId == ProcessId && (x.StatusId == 6 || x.StatusId == 8) && x.IsActive == true && x.IsDeleted == false && x.EmployeeCount == null && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).ToList(); // && x.DepartmentId == employee.DepartmentId
                queryPendingJobs = _unitviewWork.ViewJobQueryInCC.Where(x => x.ProcessId == ProcessId && (x.StatusId == 6 || x.StatusId == 8) && x.IsActive == true && x.IsDeleted == false && x.EmployeeCount == null && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).ToList(); // && x.DepartmentId == employee.DepartmentId
            }
            var result = new
            {
                QueryPendingJobs = queryPendingJobs,
            };
            return result;
        }
        public Object GetQueryResponseJobs(int EmployeeId, int ProcessId)
        {
            //var queryResponseJobs = _viewWork.ViewJobQueryInCCRepository.Get(x => x.ProcessId == ProcessId && (x.StatusId == 7 || x.StatusId == 9) && x.IsActive == true && x.IsDeleted == false && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).ToList();
            var queryResponseJobs = _unitviewWork.ViewJobQueryInCC.Where(x => x.ProcessId == ProcessId && (x.StatusId == 7 || x.StatusId == 9) && x.IsActive == true && x.IsDeleted == false && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).ToList();
            var result = new
            {
                QueryResponseJobs = queryResponseJobs
            };
            return result;
        }
        public Object GetFileNames(string path)
        {
            var folderPath = Path.Combine(_webHostEnvironment.ContentRootPath, "JobFiles", path);//HttpContext.Current.Server.MapPath("~/JobFiles/" + path);
            var files = Directory.GetFiles(folderPath).Select(Path.GetFileName).ToList();
            var result = new
            {
                Files = files
            };
            return result;
        }

        public Object GetAmountForSpecialPrice(ProcessMovement processMovement)
        {
            var result = new
            {
                Success = false,
                Message = "",
                Amount = 0M
            };
            decimal productvalue = 0M;
            var joborder = _unitWork.JobOrder.FirstOrDefault(x => x.Id == processMovement.JId);
            if (joborder != null)
            {
                if (processMovement.DepartmentId == 1)
                {
                    if (processMovement.EstimatedTime != null && processMovement.EstimatedTime != 0)
                    {
                        int workedesttime = processMovement.EstimatedTime;
                        var Getpricingscope = _unitWork.PricingWithScope.FirstOrDefault(x => x.CustomerId == processMovement.ClientId && x.ScopeId == processMovement.SelectedScopeId
                            && x.PricingTypeId == 2 && x.IsDelete != true && x.IsApproved == true && x.JobStatusId == joborder.JobStatusId
                            && x.WefromDate <= processMovement.FileReceivedDate && (x.WetoDate >= processMovement.FileReceivedDate || x.WetoDate == null));
                        if (Getpricingscope != null)
                        {
                            decimal getrateperhour = Getpricingscope.RatePerHour.Value;
                            int estimationtimeinpricing = Convert.ToInt32(Getpricingscope.EstimationTime);
                            if (estimationtimeinpricing >= workedesttime)
                            {
                                productvalue = Getpricingscope.Price.Value;
                            }
                            else
                            {
                                decimal exceedtime = workedesttime - estimationtimeinpricing;
                                decimal perhour = exceedtime / 60;
                                decimal aditionalrate = perhour * getrateperhour;
                                productvalue = Getpricingscope.Price.Value + aditionalrate;
                            }
                            result = new
                            {
                                Success = true,
                                Message = "",
                                //Amount = productvalue
                                Amount = Math.Round(productvalue, 2)
                            };
                            return result;
                        }
                        else
                        {
                            result = new
                            {
                                Success = false,
                                Message = "No Pricing Type for this Client.",
                                Amount = 0M
                            };
                            return result;
                        }
                    }
                }
                else
                {
                    var choosepricingtype = _unitWork.PricingWithScope.FirstOrDefault(x => x.CustomerId == processMovement.ClientId
                           && x.PricingTypeId == 6 && x.IsDelete != true && x.IsApproved == true && x.JobStatusId == joborder.JobStatusId
                           && x.WefromDate <= processMovement.FileReceivedDate && (x.WetoDate >= processMovement.FileReceivedDate || x.WetoDate == null));
                    if (choosepricingtype != null)
                    {
                        float getworkedcount = Convert.ToInt64(processMovement.StitchCount);
                        if (getworkedcount <= 1000)
                        {
                            productvalue = choosepricingtype.Price.Value;
                        }
                        else
                        {
                            float GetTotalstritchcount = ((getworkedcount) / (1000));
                            decimal getrate = Convert.ToDecimal(GetTotalstritchcount) * choosepricingtype.Price.Value;
                            if (getrate >= choosepricingtype.RatePerHour)
                            {
                                productvalue = choosepricingtype.RatePerHour.Value;
                            }
                            else
                            {
                                productvalue = getrate;
                            }
                        }
                        result = new
                        {
                            Success = true,
                            Message = "",
                            //Amount = productvalue
                            Amount = Math.Round(productvalue, 2)
                        };
                        return result;
                    }
                    else
                    {
                        result = new
                        {
                            Success = true,
                            Message = "No Pricing Type for this Client.",
                            Amount = 0M
                        };
                        return result;
                    }
                }
            }
            return result;
        }



        //QA starts

        public Object QARestriction(ProcessMovement processMovement)
        {

            var selectedEid = processMovement.SelectedEmployees.First().EmployeeId; ;

            var JobIdsProductionAndQAEmnpIdsAreSame = new List<int>();
            foreach (var item in processMovement.SelectedRows)
            {
                var Exists = _tableWork.EmployeeAssignRepository.Exist(check => check.Jid == item.JId && check.EmployeeId == selectedEid && check.ProcessId == 3 && check.Status == "Worked" && check.IsActive == true);
                //var Exists = _tableWork.EmployeeAssignRepository.Exist(check => check.JId == item.JId && check.EmployeeId == selectedEid && check.ProcessId == 3);


                if (Exists)
                {


                    JobIdsProductionAndQAEmnpIdsAreSame.Add(item.JId);

                }


            }
            //var selectedJobs = processMovement.SelectedRows;
            //var employees = processMovement.SelectedEmployees;

            //foreach (var job in selectedJobs)
            //{
            //    try
            //    {
            //        var GetJobdonebyInProd = _tableWork.EmployeeAssignRepository.Exist(check => check.JId == job.JId && check.ProcessId == '3' && check.Status == "Worked" && check.IsActive == true);

            //        foreach (var item in employees)
            //        {
            //            var employeeDetails = _tableWork.EmployeeRepository.Exist(x => x.EmployeeId == item.EmployeeId);
            //        }
            //    }
            //    catch (Exception Ex)
            //    {

            //    }
            //}
            //return result;

            var result = new
            {
                Success = false,
                Message = "Next Process is not defined from this tray....!",
                Jids = JobIdsProductionAndQAEmnpIdsAreSame

            };
            return result;
        }
        // QA ends

        public Object ProcessMovement(ProcessMovement processMovement)
        {
            var fileMovementList = new List<FileMovement>();
            string EXmessage = "";
            var selectedJobs = processMovement.SelectedRows;
            var employees = processMovement.SelectedEmployees.Count() == 0 ? null : processMovement.SelectedEmployees;
            var jobFileName = "";
            var step = "test";
            var result = new
            {
                Success = false,
                Message = "",
                OrderId = 0,
                WFTId = 0,
                Wfmid = 0,
                ProcessId = 0
            };
            string ifmes = "";
            string elsemes = "";
            string elseifmes = "";
            string value = "";
            int wftid = 0;
            foreach (var job in selectedJobs)
            {
                try
                {
                    var estTime = (from cvs in _context.CustomerVsScopes
                                   join cust in _context.Customers on cvs.CustomerId equals cust.Id
                                   where cvs.CustomerId == job.CustomerId
                                            && cvs.ScopeId == processMovement.SelectedScopeId
                                            && cvs.IsDeleted == false
                                            && cust.IsEstimatedTime == true
                                            select cvs.EstimatedTime).FirstOrDefault();

                    var ScopeExists = _tableWork.CustomerVsScopeRepository.Exist(check => check.CustomerId == job.CustomerId && check.ScopeId == processMovement.SelectedScopeId && check.IsActive == true);
                    if ((ScopeExists == true)
                        || (processMovement.ProcessId == 4 && processMovement.StatusId == 19 && ScopeExists == true)
                        || (processMovement.ProcessId == 2 && processMovement.StatusId == 19 && ScopeExists == true)
                       || (processMovement.SelectedScopeId == 0)
                        )
                    {
                        //if (processMovement.StatusId != 19)
                        using (var _context = new ApplicationDbContext(_configuration))
                        {
                            var processes = new ProcessWorkFlow();
                            var pwtLastRecord = _context.ProcessWorkFlowTrans.Where(x => x.Wfmid == job.TranMasterId).OrderByDescending(x => x.Id).FirstOrDefault();
                            var pwtFirstRecord = _context.ProcessWorkFlowTrans.Where(x => x.Wfmid == job.TranMasterId && x.ProcessId == processMovement.ProcessId && x.IsActive == true).OrderByDescending(x => x.Id).FirstOrDefault();
                            var insertPWT = new ProcessWorkFlowTran();

                            var process = _context.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == job.DepartmentId && x.CustomerId == job.CustomerId && x.CurrentProcessId == processMovement.ProcessId && x.StatusId == processMovement.StatusId && x.IsActive == true && x.ScopeId == (processMovement.SelectedScopeId == 0 ? null : processMovement.SelectedScopeId) && x.JobStatusId == pwtLastRecord.JobStatusId);
                            if (process == null)
                            {
                                var process1 = _context.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == job.DepartmentId && x.CustomerId == job.CustomerId && x.CurrentProcessId == processMovement.ProcessId && x.StatusId == processMovement.StatusId && x.IsActive == true && x.ScopeId == null && x.JobStatusId == pwtLastRecord.JobStatusId);
                                if (process1 == null)
                                {
                                    var process2 = _context.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == job.DepartmentId && x.CustomerId == job.CustomerId && x.CurrentProcessId == processMovement.ProcessId && x.StatusId == processMovement.StatusId && x.IsActive == true && x.ScopeId == (processMovement.SelectedScopeId == 0 ? null : processMovement.SelectedScopeId) && x.JobStatusId == null);
                                    if (process2 == null)
                                    {
                                        var process3 = _context.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == job.DepartmentId && x.CustomerId == job.CustomerId && x.CurrentProcessId == processMovement.ProcessId && x.StatusId == processMovement.StatusId && x.IsActive == true && x.ScopeId == null && x.JobStatusId == null);
                                        if (process3 == null)
                                        {
                                            var process4 = _context.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == job.DepartmentId && x.CustomerId == null && x.CurrentProcessId == processMovement.ProcessId && x.StatusId == processMovement.StatusId && x.IsActive == true && x.ScopeId == (processMovement.SelectedScopeId == 0 ? null : processMovement.SelectedScopeId) && x.JobStatusId == pwtLastRecord.JobStatusId);
                                            if (process4 == null)
                                            {
                                                var process5 = _context.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == job.DepartmentId && x.CustomerId == null && x.CurrentProcessId == processMovement.ProcessId && x.StatusId == processMovement.StatusId && x.IsActive == true && x.ScopeId == null && x.JobStatusId == pwtLastRecord.JobStatusId);
                                                if (process5 == null)
                                                {
                                                    var process6 = _context.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == job.DepartmentId && x.CustomerId == null && x.CurrentProcessId == processMovement.ProcessId && x.StatusId == processMovement.StatusId && x.IsActive == true && x.ScopeId == (processMovement.SelectedScopeId == 0 ? null : processMovement.SelectedScopeId) && x.JobStatusId == null);
                                                    if (process6 == null)
                                                    {
                                                        var process7 = _context.ProcessWorkFlows.FirstOrDefault(x => x.DepartmentId == job.DepartmentId && x.CustomerId == null && x.CurrentProcessId == processMovement.ProcessId && x.StatusId == processMovement.StatusId && x.IsActive == true && x.ScopeId == null && x.JobStatusId == null);
                                                        if (process7 == null)
                                                        {
                                                            result = new
                                                            {
                                                                Success = false,
                                                                Message = "Next Process is not defined from this tray....!",
                                                                OrderId = 0,
                                                                WFTId = 0,
                                                                Wfmid = 0,
                                                                ProcessId = 0
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

                            if (processes != null)
                            {

                                if ((job.TranId) != 0)
                                {
                                    var chkTimeStamp = _context.ProcessWorkFlowTrans.AsNoTracking().FirstOrDefault(x => x.Id == job.TranId);

                                    if (!job.TimeStamp.SequenceEqual(chkTimeStamp.TimeStamp))
                                    {
                                        result = new
                                        {
                                            Success = false,
                                            Message = "Job(s) moved to Next Process, Try another Jobs",
                                            OrderId = 0,
                                            WFTId = 0,
                                            Wfmid = 0,
                                            ProcessId = 0
                                        };
                                        if (processMovement.AutoUploadJobs == true && result.Message == "Job(s) moved to Next Process, Try another Job")
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            return result;
                                        }
                                    }
                                }

                                if ((pwtLastRecord.ProcessId == 2 || pwtLastRecord.ProcessId == 4 || pwtLastRecord.ProcessId == 6 || pwtLastRecord.ProcessId == 8 || pwtLastRecord.ProcessId == 10 || pwtLastRecord.ProcessId == 14) && (processMovement.StatusId == 1 || processMovement.StatusId == 11 || processMovement.StatusId == 26))
                                {
                                    pwtLastRecord.IsActive = true;
                                }
                                else
                                {
                                    pwtLastRecord.IsActive = false;
                                }
                                if ((processMovement.StatusId == 11 || processMovement.StatusId == 26) && process.NextProcessId == 2)
                                {
                                    pwtLastRecord.IsActive = false;
                                }

                                if (processMovement.EmployeeId != 0)
                                {
                                    pwtLastRecord.UpdatedBy = processMovement.EmployeeId;
                                    pwtFirstRecord.UpdatedBy = processMovement.EmployeeId;
                                }
                                pwtLastRecord.UpdatedUtc = DateTime.UtcNow;
                                pwtFirstRecord.UpdatedUtc = DateTime.UtcNow;
                                _context.Entry(pwtLastRecord).State = EntityState.Modified;
                                _context.Entry(pwtFirstRecord).State = EntityState.Modified;
                                if (processMovement.StatusId != 27)
                                {
                                    insertPWT.Wfmid = pwtLastRecord.Wfmid;
                                    insertPWT.Jid = pwtLastRecord.Jid;
                                    insertPWT.DepartmentId = pwtLastRecord.DepartmentId;
                                    insertPWT.PreviousProcessId = processes.CurrentProcessId;
                                    insertPWT.ProcessId = processes.NextProcessId;
                                    insertPWT.JobStatusId = pwtLastRecord.JobStatusId;
                                    if (processMovement.EmployeeId != 0)
                                    {
                                        insertPWT.AllocattedById = processMovement.EmployeeId;
                                        insertPWT.CreatedBy = processMovement.EmployeeId;
                                    }
                                    insertPWT.StatusId = processMovement.StatusId;
                                    if (processMovement.AutoUploadJobs == true)
                                    {
                                        insertPWT.Remarks = job.CommentsToClient;
                                    }
                                    else
                                    {
                                        insertPWT.Remarks = processMovement.Remarks;
                                    }
                                    insertPWT.IsActive = true;
                                    insertPWT.FileUploadPath = pwtLastRecord.FileUploadPath;
                                    insertPWT.CreatedUtc = DateTime.UtcNow;

                                    _context.ProcessWorkFlowTrans.Add(insertPWT);
                                }
                                // _unitWork.SaveChanges();
                                //flaten table insertion started
                                var empName = new Employee();
                                if (processMovement.EmployeeId != 0)
                                {
                                    empName = _context.Employees.FirstOrDefault(x => x.EmployeeId == processMovement.EmployeeId);
                                }

                                var jobId1 = _context.ProcessWorkFlowMasters.Where(x => x.Id == job.TranMasterId).Select(x => x.JobId).FirstOrDefault();
                                var jobOrder1 = _context.JobOrders.FirstOrDefault(x => x.Id == jobId1);
                                var js = _context.JobStatuses.FirstOrDefault(x => x.Id == jobOrder1.JobStatusId);
                                var dept = _context.Departments.Where(x => x.Id == jobOrder1.DepartmentId).FirstOrDefault();
                                var trayDetailMovement = _context.TrayDetailMovements.Where(x => x.FileName == jobOrder1.FileName && x.Department == dept.Description && x.CustomerJobType == jobOrder1.CustomerJobType && x.ClientId == jobOrder1.ClientId && x.JobStatus == js.JobStatusDescription && x.IsDeleted == false).OrderByDescending(x => x.Id).FirstOrDefault();
                                //var trayDetailMovement = _context.TrayDetailMovements.Where(x => x.FileName == jobOrder1.FileName  && x.CustomerJobType == jobOrder1.CustomerJobType && x.ClientId == jobOrder1.ClientId && x.JobStatus == js.JobStatusDescription && x.IsDeleted == false).OrderByDescending(x => x.Id).FirstOrDefault();
                                //var checks = _context.TrayDetailMovements.Where(x => x.FileName == jobOrder1.FileName && x.CustomerJobType == jobOrder1.CustomerJobType && x.ClientId == jobOrder1.ClientId && x.JobStatus == js.JobStatusDescription && x.IsDeleted == false).OrderByDescending(x => x.Id).ToList();
                                var checks = _context.TrayDetailMovements.Where(x => x.FileName == jobOrder1.FileName && x.Department == dept.Description && x.CustomerJobType == jobOrder1.CustomerJobType && x.ClientId == jobOrder1.ClientId && x.JobStatus == js.JobStatusDescription && x.IsDeleted == false).OrderByDescending(x => x.Id).ToList();
                                var trayDetailInsertCheck = "";
                                if (trayDetailMovement != null)
                                {
                                    if ((processMovement.ProcessId == 2 || processMovement.ProcessId == 14) && trayDetailMovement.CurrentStatus != "Query Response" && trayDetailMovement.CurrentStatus != "Query for SP Response")
                                    {
                                        if (checks != null)
                                        {
                                            foreach (var item in checks)
                                            {
                                                var status = item.CurrentStatus;
                                                if ((status == "Query") || (status == "Query for Special Pricing") || (status == "Work Incomplete") || (status == "Error" || status == "Completed With Error"))
                                                {
                                                    trayDetailInsertCheck = status;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (checks.Count() != 0)
                                        {
                                            trayDetailInsertCheck = checks.FirstOrDefault().CurrentStatus;
                                        }
                                    }
                                }

                                if (trayDetailMovement != null)
                                {
                                    if ((trayDetailInsertCheck == "Query" && (processes.CurrentProcessId == 2 || processes.CurrentProcessId == 4 || processes.CurrentProcessId == null || processes.CurrentProcessId == 1)) || (trayDetailInsertCheck == "Query for Special Pricing" && (processes.CurrentProcessId == 2 || processes.CurrentProcessId == 4 || processes.CurrentProcessId == null)) || (trayDetailInsertCheck == "Work Incomplete") || (trayDetailInsertCheck == "Error" || trayDetailInsertCheck == "Completed With Error"))
                                    {
                                        TrayDetailMovement insertTrayDetailMovement = new TrayDetailMovement();
                                        if ((processMovement.ProcessId == null) || (processMovement.ProcessId == 1 && processMovement.StatusId == 7))
                                        {
                                            insertTrayDetailMovement.JobDate = DateTime.UtcNow;
                                        }
                                        else
                                        {
                                            insertTrayDetailMovement.JobDate = trayDetailMovement.JobDate;
                                        }

                                        insertTrayDetailMovement.FileName = trayDetailMovement.FileName;
                                        insertTrayDetailMovement.ClientId = trayDetailMovement.ClientId;
                                        insertTrayDetailMovement.ClientShortName = trayDetailMovement.ClientShortName;
                                        insertTrayDetailMovement.Department = trayDetailMovement.Department;
                                        insertTrayDetailMovement.JobStatus = trayDetailMovement.JobStatus;
                                        insertTrayDetailMovement.CustomerJobType = trayDetailMovement.CustomerJobType;
                                        if (processes.NextProcessId == null)
                                        {
                                            insertTrayDetailMovement.CurrentTray = "Client";
                                        }
                                        else
                                        {
                                            insertTrayDetailMovement.CurrentTray = _context.Processes.FirstOrDefault(x => x.Id == processes.NextProcessId).Name;
                                        }
                                        insertTrayDetailMovement.CurrentStatus = _context.Statuses.FirstOrDefault(x => x.Id == processMovement.StatusId).Name;
                                        insertTrayDetailMovement.IsDeleted = false;
                                        insertTrayDetailMovement.CreatedBy = processMovement.EmployeeId;
                                        insertTrayDetailMovement.CreatedUtc = DateTime.UtcNow;
                                        if ((trayDetailInsertCheck == "Query") || (trayDetailInsertCheck == "Query for Special Pricing"))
                                        {
                                            if (processes.CurrentProcessId == 1)
                                            {
                                                if (empName != null)
                                                {
                                                    insertTrayDetailMovement.CcintakenBy = empName.EmployeeCode + "-" + empName.EmployeeName;
                                                }
                                            }

                                            //harsha added  to check query flow in DPR ref

                                            if (processes.CurrentProcessId == 2)
                                            {
                                                insertTrayDetailMovement.ProductionAllocatedBy = empName.EmployeeCode + "-" + empName.EmployeeName;
                                            }
                                            else if (processes.CurrentProcessId == 4)
                                            {
                                                insertTrayDetailMovement.QualityAllocatedBy = empName.EmployeeCode + "-" + empName.EmployeeName;
                                            }

                                            //harsha ended to check query flow in DPR ref
                                        }
                                        else if ((trayDetailInsertCheck == "Work Incomplete") || (trayDetailInsertCheck == "Error" || trayDetailInsertCheck == "Completed With Error"))
                                        {
                                            if (processMovement.SelectedScopeId != 0)
                                            {
                                                var scopeDesc = _context.Scopes.FirstOrDefault(x => x.Id == processMovement.SelectedScopeId);
                                                insertTrayDetailMovement.Scope = scopeDesc.Description;
                                            }
                                            if (processes.NextProcessId == null)
                                            {
                                                insertTrayDetailMovement.CurrentTray = "Client";
                                            }
                                            else
                                            {
                                                insertTrayDetailMovement.CurrentTray = _context.Processes.FirstOrDefault(x => x.Id == processes.NextProcessId).Name;
                                            }
                                            insertTrayDetailMovement.CurrentStatus = _context.Statuses.FirstOrDefault(x => x.Id == processMovement.StatusId).Name;
                                            if (processes.CurrentProcessId == 2)
                                            {
                                                insertTrayDetailMovement.ProductionAllocatedBy = empName.EmployeeCode + "-" + empName.EmployeeName;
                                                if (employees != null)
                                                {
                                                    var empId = employees.First();
                                                    var employeeDetails = _context.Employees.FirstOrDefault(x => x.EmployeeId == empId.EmployeeId);
                                                    insertTrayDetailMovement.ProductionAllocatedToEmpId = empId.EmployeeId;
                                                    insertTrayDetailMovement.ProductionAllocatedTo = employeeDetails.EmployeeCode + "-" + employeeDetails.EmployeeName;
                                                    if (selectedJobs.Count() > 1)
                                                    {
                                                        if (job.AllocatedEstimatedTime > 0)
                                                        {
                                                            insertTrayDetailMovement.EstimationTime = job.AllocatedEstimatedTime;
                                                        }
                                                        else
                                                        {
                                                            if (estTime > 0)
                                                            {
                                                                insertTrayDetailMovement.EstimationTime = estTime;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (empId.EstimatedTime > 0)
                                                        {
                                                            insertTrayDetailMovement.EstimationTime = empId.EstimatedTime;
                                                        }
                                                        else
                                                        {
                                                            if (estTime > 0)
                                                            {
                                                                insertTrayDetailMovement.EstimationTime = estTime;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else if (processes.CurrentProcessId == 4)
                                            {
                                                insertTrayDetailMovement.QualityAllocatedBy = empName.EmployeeCode + "-" + empName.EmployeeName;
                                                if (employees != null)
                                                {
                                                    var empId = employees.First();
                                                    var employeeDetails = _context.Employees.FirstOrDefault(x => x.EmployeeId == empId.EmployeeId);
                                                    insertTrayDetailMovement.QualityAllocatedToEmpId = empId.EmployeeId;
                                                    insertTrayDetailMovement.QualityAllocatedTo = employeeDetails.EmployeeCode + "-" + employeeDetails.EmployeeName;
                                                }
                                            }
                                            else if (processes.CurrentProcessId == 6)
                                            {
                                                insertTrayDetailMovement.ProofReadingAllocatedBy = empName.EmployeeCode + "-" + empName.EmployeeName;
                                                if (employees != null)
                                                {
                                                    var empId = employees.First();
                                                    var employeeDetails = _context.Employees.FirstOrDefault(x => x.EmployeeId == empId.EmployeeId);
                                                    insertTrayDetailMovement.ProofReadingAllocatedToEmpId = empId.EmployeeId;
                                                    insertTrayDetailMovement.ProofReadingAllocatedTo = employeeDetails.EmployeeCode + "-" + employeeDetails.EmployeeName;
                                                }
                                            }
                                            else if (processes.CurrentProcessId == 8)
                                            {
                                                insertTrayDetailMovement.BpallocatedBy = empName.EmployeeCode + "-" + empName.EmployeeName;
                                                if (employees != null)
                                                {
                                                    var empId = employees.First();
                                                    var employeeDetails = _context.Employees.FirstOrDefault(x => x.EmployeeId == empId.EmployeeId);
                                                    insertTrayDetailMovement.BpallocatedToEmpId = empId.EmployeeId;
                                                    insertTrayDetailMovement.BpallocatedTo = employeeDetails.EmployeeCode + "-" + employeeDetails.EmployeeName;
                                                }
                                            }
                                            else if (processes.CurrentProcessId == 10)
                                            {
                                                insertTrayDetailMovement.SoallocatedBy = empName.EmployeeCode + "-" + empName.EmployeeName;
                                                if (employees != null)
                                                {
                                                    var empId = employees.First();
                                                    var employeeDetails = _context.Employees.FirstOrDefault(x => x.EmployeeId == empId.EmployeeId);
                                                    insertTrayDetailMovement.SoallocatedToEmpId = empId.EmployeeId;
                                                    insertTrayDetailMovement.SoallocatedTo = employeeDetails.EmployeeCode + "-" + employeeDetails.EmployeeName;
                                                }
                                            }
                                            else if (processes.CurrentProcessId == 1 && processMovement.StatusId == 12)
                                            {
                                                var employeeDetails = _context.Employees.FirstOrDefault(x => x.EmployeeId == processMovement.EmployeeId);
                                                insertTrayDetailMovement.UploadedBy = employeeDetails.EmployeeCode + "-" + employeeDetails.EmployeeName;
                                                insertTrayDetailMovement.UploadedUtc = DateTime.UtcNow;
                                            }
                                            insertTrayDetailMovement.IsDeleted = false;
                                            insertTrayDetailMovement.CreatedBy = processMovement.EmployeeId;
                                            insertTrayDetailMovement.CreatedUtc = DateTime.UtcNow;
                                        }
                                        _context.TrayDetailMovements.Add(insertTrayDetailMovement);
                                        //...........................Newly Added Blog........................................//                                        
                                        //....................................................................//
                                        //_context.SaveChanges();
                                        if (employees != null && processMovement.ProcessId == 2)
                                        {
                                            var checkForDeletingTDM = _context.TrayDetailMovements.Where(x => x.FileName == jobOrder1.FileName && x.CustomerJobType == jobOrder1.CustomerJobType && x.JobStatus == js.JobStatusDescription && x.IsDeleted == false && x.CurrentTray == "Production").OrderByDescending(x => x.Id).ToList();
                                            foreach (var item in checkForDeletingTDM.Skip(1))
                                            {
                                                item.IsDeleted = true;
                                                item.UpdatedBy = processMovement.EmployeeId;
                                                item.UpdatedUtc = DateTime.UtcNow;
                                                _context.Entry(item).State = EntityState.Modified;
                                            }
                                        }

                                        var processName = "";
                                        if (processes.NextProcessId != null)
                                        {
                                            processName = _context.Processes.FirstOrDefault(x => x.Id == processes.NextProcessId).Name;
                                        }

                                        if (insertTrayDetailMovement != null)
                                        {
                                            var trayDetailMovementTran = _context.TrayDetailMovementTrans.Where(x => x.Wfmid == job.TranMasterId && x.Process == processName && x.Status == "Pending" && x.IsDeleted == false).ToList();
                                            if (trayDetailMovementTran.Count() >= 1)
                                            {
                                                foreach (var item in trayDetailMovementTran)
                                                {
                                                    item.IsDeleted = true;
                                                    item.UpdatedBy = processMovement.EmployeeId;
                                                    item.UpdatedUtc = DateTime.UtcNow;
                                                    _context.Entry(item).State = EntityState.Modified;
                                                }
                                                //_context.SaveChanges();
                                            }
                                        }

                                        if (employees != null && processMovement.ProcessId == 2)
                                        {
                                            foreach (var item in employees)
                                            {
                                                var employeeDetails = _context.Employees.FirstOrDefault(x => x.EmployeeId == item.EmployeeId);
                                                TrayDetailMovementTran insertTrayDetailMovementTran = new TrayDetailMovementTran();
                                                insertTrayDetailMovementTran.Tdmid = insertTrayDetailMovement.Id;
                                                insertTrayDetailMovementTran.Tdm = insertTrayDetailMovement;
                                                insertTrayDetailMovementTran.JobDate = insertTrayDetailMovement.JobDate;
                                                insertTrayDetailMovementTran.Wfmid = pwtLastRecord.Wfmid;
                                                insertTrayDetailMovementTran.ClientId = insertTrayDetailMovement.ClientId;
                                                insertTrayDetailMovementTran.ClientShortName = insertTrayDetailMovement.ClientShortName;
                                                insertTrayDetailMovementTran.DepartmentId = job.DepartmentId;
                                                insertTrayDetailMovementTran.Department = insertTrayDetailMovement.Department;
                                                insertTrayDetailMovementTran.EmployeeId = item.EmployeeId;
                                                insertTrayDetailMovementTran.EmployeeName = employeeDetails.EmployeeCode + "-" + employeeDetails.EmployeeName;
                                                insertTrayDetailMovementTran.Status = insertTrayDetailMovement.CurrentStatus;
                                                insertTrayDetailMovementTran.Process = insertTrayDetailMovement.CurrentTray;
                                                if (selectedJobs.Count() > 1)

                                                {

                                                    if (job.AllocatedEstimatedTime > 0)

                                                    {

                                                        insertTrayDetailMovement.EstimationTime = job.AllocatedEstimatedTime;

                                                    }

                                                    else

                                                    {

                                                        if (estTime > 0)

                                                        {

                                                            insertTrayDetailMovement.EstimationTime = estTime;

                                                        }

                                                    }

                                                }

                                                else

                                                {

                                                    if (item.EstimatedTime > 0)

                                                    {

                                                        insertTrayDetailMovement.EstimationTime = item.EstimatedTime;

                                                    }

                                                    else

                                                    {

                                                        if (estTime > 0)

                                                        {

                                                            insertTrayDetailMovement.EstimationTime = estTime;

                                                        }

                                                    }

                                                }
                                                insertTrayDetailMovementTran.IsDeleted = false;
                                                insertTrayDetailMovementTran.ScopeId = processMovement.SelectedScopeId == 0 ? null : processMovement.SelectedScopeId;
                                                insertTrayDetailMovementTran.CreatedBy = processMovement.EmployeeId;
                                                insertTrayDetailMovementTran.CreatedUtc = DateTime.UtcNow;
                                                insertTrayDetailMovementTran.IsBench = processMovement.IsBench;
                                                _context.TrayDetailMovementTrans.Add(insertTrayDetailMovementTran);
                                            }
                                            //_context.SaveChanges();
                                        }
                                        else if (employees != null)
                                        {
                                            var emps = employees.FirstOrDefault();
                                            var employeeDetails = _context.Employees.FirstOrDefault(x => x.EmployeeId == emps.EmployeeId);
                                            TrayDetailMovementTran insertTrayDetailMovementTran = new TrayDetailMovementTran();
                                            insertTrayDetailMovementTran.Tdmid = insertTrayDetailMovement.Id;
                                            insertTrayDetailMovementTran.Tdm = insertTrayDetailMovement;
                                            insertTrayDetailMovementTran.JobDate = insertTrayDetailMovement.JobDate;
                                            insertTrayDetailMovementTran.Wfmid = pwtLastRecord.Wfmid;
                                            insertTrayDetailMovementTran.ClientId = insertTrayDetailMovement.ClientId;
                                            insertTrayDetailMovementTran.ClientShortName = insertTrayDetailMovement.ClientShortName;
                                            insertTrayDetailMovementTran.DepartmentId = job.DepartmentId;
                                            insertTrayDetailMovementTran.Department = insertTrayDetailMovement.Department;
                                            insertTrayDetailMovementTran.EmployeeId = employees.FirstOrDefault().EmployeeId;
                                            insertTrayDetailMovementTran.EmployeeName = employeeDetails.EmployeeCode + "-" + employeeDetails.EmployeeName;
                                            insertTrayDetailMovementTran.Status = insertTrayDetailMovement.CurrentStatus;
                                            insertTrayDetailMovementTran.Process = insertTrayDetailMovement.CurrentTray;
                                            if (selectedJobs.Count() > 1)
                                            {
                                                if (job.AllocatedEstimatedTime > 0)
                                                {
                                                    insertTrayDetailMovementTran.EstimatedTime = job.AllocatedEstimatedTime;
                                                }
                                                else
                                                {
                                                    if (estTime > 0)
                                                    {
                                                        insertTrayDetailMovementTran.EstimatedTime = (int)estTime;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (emps.EstimatedTime > 0)
                                                {
                                                    insertTrayDetailMovementTran.EstimatedTime = emps.EstimatedTime;
                                                }
                                                else
                                                {
                                                    if (estTime > 0)
                                                    {
                                                        insertTrayDetailMovementTran.EstimatedTime = (int)estTime;
                                                    }
                                                }
                                            }
                                            insertTrayDetailMovementTran.IsDeleted = false;
                                            insertTrayDetailMovementTran.CreatedBy = processMovement.EmployeeId;
                                            insertTrayDetailMovementTran.CreatedUtc = DateTime.UtcNow;
                                            insertTrayDetailMovementTran.IsBench = processMovement.IsBench;
                                            _context.TrayDetailMovementTrans.Add(insertTrayDetailMovementTran);
                                            //_context.SaveChanges();
                                        }
                                    }
                                    else if (processMovement.ProcessId == 2 && processMovement.StatusId == 27)
                                    {
                                        var fileMovement = new FileMovement();
                                        int filesCount = (int)processMovement.FileCount;
                                        var editedFileName = processMovement.EditedFileName;
                                        for (int count = 0; count < filesCount; count++)
                                        {
                                            int jobstatusidsingle = jobOrder1.JobStatusId;
                                            var JobstatusDetailsingle = _unitWork.JobStatus.Where(x => x.Id == jobstatusidsingle).FirstOrDefault();
                                            int jobstatusgroupidsingle = JobstatusDetailsingle.JobStatusGroupId;
                                            DateTime jobdatesingle = DateTime.UtcNow;
                                            var transactiontypeDetailsingle = _unitWork.TransactionType.Where(x => x.JobStatusGroupId == jobstatusgroupidsingle).FirstOrDefault();
                                            int transactionidsingle = transactiontypeDetailsingle.Id;
                                            var freshJobExistssingle = _unitWork.JobOrder.Where(x => x.ClientId == job.CustomerId && x.FileName.Equals(jobOrder1.FileName)).OrderBy(x => x.Id).ToList();
                                            var freshJobExistscountsingle = freshJobExistssingle.Count > 0;

                                            string monthsingle = jobdatesingle.ToString("MM");
                                            string daysingle = jobdatesingle.ToString("dd");
                                            string Jobnosingle = "";
                                            JobOrder jobOrder = new JobOrder();

                                        gotostmt_voucher:

                                            var VoucherControlDetailsingle = _unitWork.VoucherControl.OrderByDescending(v => v.Id).FirstOrDefault(x => x.TransactionId == transactionidsingle && (x.EffectiveFrom <= jobdatesingle && x.EffectiveTo >= jobdatesingle) && x.DepartmentId == job.DepartmentId && x.IsDeleted == false && x.IsActive == true);
                                            if (VoucherControlDetailsingle == null)
                                            {
                                                fileMovement.Message = "VoucherControl is Missing";
                                            }
                                            string Prefixsingle = VoucherControlDetailsingle.Prefix.ToString();
                                            long VoucherNosingle = VoucherControlDetailsingle.Voucherno + 1;
                                            Jobnosingle = Prefixsingle + monthsingle + daysingle + "/" + VoucherNosingle;
                                            var filesName = _unitWork.JobOrder.Where(x => x.FileName == editedFileName[count] && x.ClientId == job.CustomerId && x.DepartmentId == job.DepartmentId && x.JobStatusId == jobstatusidsingle && x.IsDeleted == false).ToList();
                                            if (jobstatusidsingle == 1 || jobstatusidsingle == 2 || jobstatusidsingle == 5)
                                            {
                                                Jobnosingle = Prefixsingle + monthsingle + daysingle + "/" + VoucherNosingle + "/R" + (filesName.Count == 0 ? 1 : (filesName.Count + 1));
                                            }
                                            var voucherControl = _unitWork.VoucherControl.Find(VoucherControlDetailsingle.Id);
                                            voucherControl.Voucherno = VoucherNosingle;
                                            voucherControl.UpdatedBy = processMovement.EmployeeId;
                                            voucherControl.UpdatedUtc = DateTime.UtcNow;
                                            _unitWork.VoucherControl.Update(voucherControl);
                                            _unitWork.SaveChanges();

                                            jobOrder.DepartmentId = job.DepartmentId;
                                            jobOrder.ClientId = job.CustomerId;
                                            jobOrder.EmployeeId = processMovement.EmployeeId;
                                            jobOrder.JobStatusId = jobOrder1.JobStatusId;
                                            jobOrder.JobId = Jobnosingle;
                                            jobOrder.FileName = editedFileName[count];
                                            jobOrder.TransactionId = transactionidsingle;
                                            jobOrder.FileReceivedDate = processMovement.FileReceivedDate;
                                            jobOrder.JobDate = DateTime.UtcNow;
                                            jobOrder.IsDeleted = false;
                                            jobOrder.CustomerJobType = "Live";
                                            jobOrder.FileInwardTypeId = 3;
                                            jobOrder.CreatedBy = processMovement.EmployeeId;
                                            jobOrder.DivisionId = jobOrder1.DivisionId;
                                            _unitWork.JobOrder.Add(jobOrder);
                                            _unitWork.SaveChanges();

                                            ProcessWorkFlowMaster processWorkFlowMaster = new ProcessWorkFlowMaster();
                                            processWorkFlowMaster.TransactionId = jobOrder.TransactionId;
                                            processWorkFlowMaster.Job = jobOrder;
                                            processWorkFlowMaster.CustomerId = jobOrder.ClientId.Value;
                                            processWorkFlowMaster.DepartmentId = jobOrder.DepartmentId;
                                            processWorkFlowMaster.IsDeleted = jobOrder.IsDeleted;
                                            processWorkFlowMaster.CreatedBy = jobOrder.CreatedBy;
                                            processWorkFlowMaster.CreatedUtc = DateTime.UtcNow;
                                            _unitWork.ProcessWorkFlowMaster.Add(processWorkFlowMaster);

                                            ProcessWorkFlowTran processWorkFlowTran = new ProcessWorkFlowTran();
                                            processWorkFlowTran.Wfm = processWorkFlowMaster;
                                            processWorkFlowTran.JidNavigation = jobOrder;
                                            processWorkFlowTran.StatusId = 1;
                                            processWorkFlowTran.DepartmentId = jobOrder.DepartmentId;
                                            processWorkFlowTran.PreviousProcessId = null;
                                            processWorkFlowTran.ProcessId = 2;
                                            processWorkFlowTran.JobStatusId = jobOrder.JobStatusId;
                                            processWorkFlowTran.IsDeleted = processWorkFlowMaster.IsDeleted;
                                            processWorkFlowTran.CreatedBy = jobOrder.CreatedBy;
                                            processWorkFlowTran.CreatedUtc = DateTime.UtcNow;
                                            processWorkFlowTran.IsActive = true;
                                            _unitWork.ProcessWorkFlowTran.Add(processWorkFlowTran);
                                            _unitWork.SaveChanges();

                                            JobOrderExtension joe = new JobOrderExtension();
                                            joe.JidNavigation = jobOrder;
                                            joe.Date = DateTime.UtcNow;
                                            joe.Wft = processWorkFlowTran;
                                            _unitWork.JobOrderExtension.Add(joe);

                                            //Flaten Table inserted...
                                            var customerShortName = _unitWork.Customer.Where(x => x.Id == job.CustomerId).FirstOrDefault();
                                            var dept1 = _unitWork.Department.Where(x => x.Id == jobOrder.DepartmentId).FirstOrDefault();
                                            var jobStatus = _unitWork.JobStatus.Where(x => x.Id == jobOrder.JobStatusId).FirstOrDefault();
                                            var ccIntake = _unitWork.Employee.Where(x => x.EmployeeId == jobOrder.CreatedBy).FirstOrDefault();
                                            var empname = _unitWork.Employee.Where(x => x.EmployeeId == processMovement.EmployeeId).FirstOrDefault();
                                            var classiid = _unitWork.Customer.FirstOrDefault(x => x.Id == job.CustomerId).CustomerClassificationId;
                                            var classification = _unitWork.CustomerClassification.FirstOrDefault(x => x.Id == classiid).Description;

                                            TrayDetailMovement trayDetailMovement1 = new TrayDetailMovement();
                                            trayDetailMovement1.JobDate = DateTime.UtcNow;
                                            trayDetailMovement1.FileName = jobOrder.FileName;
                                            trayDetailMovement1.ClientId = job.CustomerId;
                                            trayDetailMovement1.ClientShortName = customerShortName.ShortName;
                                            trayDetailMovement1.Department = dept.Description;
                                            trayDetailMovement1.JobStatus = jobStatus.JobStatusDescription;
                                            trayDetailMovement1.CustomerJobType = jobOrder.CustomerJobType;
                                            trayDetailMovement1.CurrentTray = "Production Allocation";
                                            trayDetailMovement1.CurrentStatus = "Pending";
                                            trayDetailMovement1.CcintakenBy = ccIntake.EmployeeCode + "-" + ccIntake.EmployeeName;
                                            trayDetailMovement1.IsDeleted = false;
                                            trayDetailMovement1.CreatedBy = processMovement.EmployeeId;
                                            trayDetailMovement1.CreatedUtc = DateTime.UtcNow;
                                            trayDetailMovement1.Jid = _tableWork.JobOrderRepository.GetLastRecord(s => s.Id).Id;
                                            trayDetailMovement1.JobId = jobOrder.JobId;
                                            trayDetailMovement1.DepartmentId = job.DepartmentId;
                                            trayDetailMovement1.EmployeeId = processMovement.EmployeeId;
                                            trayDetailMovement1.EmployeeName = empname.EmployeeCode + "-" + empname.EmployeeName;
                                            trayDetailMovement1.StatusId = 1;
                                            trayDetailMovement1.ClassificationId = classiid;
                                            trayDetailMovement1.Classification = classification;
                                            trayDetailMovement1.CcintakenDate = DateTime.UtcNow;
                                            _unitWork.TrayDetailMovement.Add(trayDetailMovement1);
                                            _unitWork.SaveChanges();

                                            int year = jobOrder.FileReceivedDate.Year;
                                            var Getjobsummary = _unitWork.JobSummary.Where(x => x.ClientId == job.ClientId && x.MonthYear == monthsingle + "-" + year).ToList();
                                            var GetLastRecord = Getjobsummary.LastOrDefault();
                                            if (Getjobsummary.Count == 0)
                                            {
                                                JobSummary savejobsummary = new JobSummary();
                                                savejobsummary.ClientId = job.ClientId;
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
                                    else
                                    {
                                        if (processMovement.SelectedScopeId != 0)
                                        {
                                            var scopeDesc = _context.Scopes.FirstOrDefault(x => x.Id == processMovement.SelectedScopeId);
                                            trayDetailMovement.Scope = scopeDesc.Description;
                                        }
                                        if (processes.NextProcessId == null)
                                        {
                                            trayDetailMovement.CurrentTray = "Client";
                                        }
                                        else
                                        {
                                            trayDetailMovement.CurrentTray = _context.Processes.FirstOrDefault(x => x.Id == processes.NextProcessId).Name;
                                        }
                                        trayDetailMovement.CurrentStatus = _context.Statuses.FirstOrDefault(x => x.Id == processMovement.StatusId).Name;
                                        if (processes.CurrentProcessId == 1 && (processMovement.StatusId == 6 || processMovement.StatusId == 7 || processMovement.StatusId == 8 || processMovement.StatusId == 9))
                                        {
                                            if (empName != null)
                                            {
                                                trayDetailMovement.CcintakenBy = empName.EmployeeCode + "-" + empName.EmployeeName;
                                            }
                                        }
                                        if (processes.CurrentProcessId == 2)
                                        {
                                            trayDetailMovement.ProductionAllocatedBy = empName.EmployeeCode + "-" + empName.EmployeeName;
                                            if (employees?.Count() > 0)
                                            {
                                                var empId = employees.First();
                                                var employeeDetails = _context.Employees.FirstOrDefault(x => x.EmployeeId == empId.EmployeeId);
                                                trayDetailMovement.ProductionAllocatedToEmpId = empId.EmployeeId;
                                                trayDetailMovement.ProductionAllocatedTo = employeeDetails.EmployeeCode + "-" + employeeDetails.EmployeeName;
                                                if (selectedJobs.Count() > 1)
                                                {
                                                    if (job.AllocatedEstimatedTime > 0)
                                                    {
                                                        trayDetailMovement.EstimationTime = job.AllocatedEstimatedTime;
                                                    }
                                                    else
                                                    {
                                                        if (estTime > 0)
                                                        {
                                                            trayDetailMovement.EstimationTime = estTime;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (empId.EstimatedTime > 0)
                                                    {
                                                        trayDetailMovement.EstimationTime = empId.EstimatedTime;
                                                    }
                                                    else
                                                    {
                                                        if (estTime > 0)
                                                        {
                                                            trayDetailMovement.EstimationTime = estTime;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else if (processes.CurrentProcessId == 4)
                                        {
                                            trayDetailMovement.QualityAllocatedBy = empName.EmployeeCode + "-" + empName.EmployeeName;
                                            if (employees != null)
                                            {
                                                var empId = employees.First();
                                                var employeeDetails = _context.Employees.FirstOrDefault(x => x.EmployeeId == empId.EmployeeId);
                                                trayDetailMovement.QualityAllocatedToEmpId = empId.EmployeeId;
                                                trayDetailMovement.QualityAllocatedTo = employeeDetails.EmployeeCode + "-" + employeeDetails.EmployeeName;
                                            }
                                        }
                                        else if (processes.CurrentProcessId == 6)
                                        {
                                            trayDetailMovement.ProofReadingAllocatedBy = empName.EmployeeCode + "-" + empName.EmployeeName;
                                            if (employees != null)
                                            {
                                                var empId = employees.First();
                                                var employeeDetails = _context.Employees.FirstOrDefault(x => x.EmployeeId == empId.EmployeeId);
                                                trayDetailMovement.ProofReadingAllocatedToEmpId = empId.EmployeeId;
                                                trayDetailMovement.ProofReadingAllocatedTo = employeeDetails.EmployeeCode + "-" + employeeDetails.EmployeeName;
                                            }
                                        }
                                        else if (processes.CurrentProcessId == 8)
                                        {
                                            trayDetailMovement.BpallocatedBy = empName.EmployeeCode + "-" + empName.EmployeeName;
                                            if (employees != null)
                                            {
                                                var empId = employees.First();
                                                var employeeDetails = _context.Employees.FirstOrDefault(x => x.EmployeeId == empId.EmployeeId);
                                                trayDetailMovement.BpallocatedToEmpId = empId.EmployeeId;
                                                trayDetailMovement.BpallocatedTo = employeeDetails.EmployeeCode + "-" + employeeDetails.EmployeeName;
                                            }
                                        }
                                        else if (processes.CurrentProcessId == 10)
                                        {
                                            trayDetailMovement.SoallocatedBy = empName.EmployeeCode + "-" + empName.EmployeeName;
                                            if (employees != null)
                                            {
                                                var empId = employees.First();
                                                var employeeDetails = _context.Employees.FirstOrDefault(x => x.EmployeeId == empId.EmployeeId);
                                                trayDetailMovement.SoallocatedToEmpId = empId.EmployeeId;
                                                trayDetailMovement.SoallocatedTo = employeeDetails.EmployeeCode + "-" + employeeDetails.EmployeeName;
                                            }
                                        }
                                        else if (processes.CurrentProcessId == 1 && processMovement.StatusId == 12)
                                        {
                                            var employeeDetails = _context.Employees.FirstOrDefault(x => x.EmployeeId == processMovement.EmployeeId);
                                            trayDetailMovement.UploadedBy = employeeDetails.EmployeeCode + "-" + employeeDetails.EmployeeName;
                                            trayDetailMovement.UploadedUtc = DateTime.UtcNow;
                                        }
                                        trayDetailMovement.UpdatedBy = processMovement.EmployeeId;
                                        trayDetailMovement.UpdatedUtc = DateTime.UtcNow;
                                        _context.Entry(trayDetailMovement).State = EntityState.Modified;
                                        //_context.SaveChanges();
                                        var processName = "";
                                        if (processes.NextProcessId != null)
                                        {
                                            processName = _context.Processes.FirstOrDefault(x => x.Id == processes.NextProcessId).Name;
                                        }
                                        var trayDetailMovementTran = _context.TrayDetailMovementTrans.Where(x => x.Wfmid == job.TranMasterId && x.Process == processName && x.Status == "Pending" && x.IsDeleted == false).ToList();
                                        if (trayDetailMovementTran.Count() >= 1)
                                        {
                                            foreach (var item in trayDetailMovementTran)
                                            {
                                                item.IsDeleted = true;
                                                item.UpdatedBy = processMovement.EmployeeId;
                                                item.UpdatedUtc = DateTime.UtcNow;
                                                _context.Entry(item).State = EntityState.Modified;
                                            }
                                            //_context.SaveChanges();
                                        }

                                        if (employees != null && processMovement.ProcessId == 2 || processMovement.ProcessId == 14)
                                        {
                                            foreach (var item in employees)
                                            {
                                                var employeeDetails = _context.Employees.FirstOrDefault(x => x.EmployeeId == item.EmployeeId);
                                                TrayDetailMovementTran insertTrayDetailMovementTran = new TrayDetailMovementTran();
                                                insertTrayDetailMovementTran.Tdmid = trayDetailMovement.Id;
                                                //     insertTrayDetailMovementTran.Tdm = trayDetailMovement;
                                                insertTrayDetailMovementTran.JobDate = trayDetailMovement.JobDate;
                                                insertTrayDetailMovementTran.Wfmid = pwtLastRecord.Wfmid;
                                                insertTrayDetailMovementTran.ClientId = trayDetailMovement.ClientId;
                                                insertTrayDetailMovementTran.ClientShortName = trayDetailMovement.ClientShortName;
                                                insertTrayDetailMovementTran.DepartmentId = job.DepartmentId;
                                                insertTrayDetailMovementTran.Department = trayDetailMovement.Department;
                                                insertTrayDetailMovementTran.EmployeeId = item.EmployeeId;
                                                insertTrayDetailMovementTran.EmployeeName = employeeDetails.EmployeeCode + "-" + employeeDetails.EmployeeName;
                                                insertTrayDetailMovementTran.Status = trayDetailMovement.CurrentStatus;
                                                insertTrayDetailMovementTran.Process = trayDetailMovement.CurrentTray;
                                                if (selectedJobs.Count() > 1)
                                                {
                                                    if (job.AllocatedEstimatedTime > 0)
                                                    {
                                                        insertTrayDetailMovementTran.EstimatedTime = job.AllocatedEstimatedTime;
                                                    }
                                                    else
                                                    {
                                                        if (estTime > 0)
                                                        {
                                                            insertTrayDetailMovementTran.EstimatedTime = estTime;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (item.EstimatedTime > 0)
                                                    {
                                                        insertTrayDetailMovementTran.EstimatedTime = item.EstimatedTime;
                                                    }
                                                    else
                                                    {
                                                        if (estTime > 0)
                                                        {
                                                            insertTrayDetailMovementTran.EstimatedTime = estTime;
                                                        }
                                                    }
                                                }
                                                insertTrayDetailMovementTran.IsDeleted = false;
                                                insertTrayDetailMovementTran.ScopeId = processMovement.SelectedScopeId == 0 ? null : processMovement.SelectedScopeId;
                                                insertTrayDetailMovementTran.CreatedBy = processMovement.EmployeeId;
                                                insertTrayDetailMovementTran.CreatedUtc = DateTime.UtcNow;
                                                insertTrayDetailMovementTran.IsBench = processMovement.IsBench;
                                                _context.TrayDetailMovementTrans.Add(insertTrayDetailMovementTran);
                                            }
                                            //_context.SaveChanges();
                                        }
                                        else if (employees != null)
                                        {
                                            var emps = employees.FirstOrDefault();
                                            var employeeDetails = _context.Employees.FirstOrDefault(x => x.EmployeeId == emps.EmployeeId);
                                            TrayDetailMovementTran insertTrayDetailMovementTran = new TrayDetailMovementTran();
                                            insertTrayDetailMovementTran.Tdmid = trayDetailMovement.Id;
                                            insertTrayDetailMovementTran.JobDate = trayDetailMovement.JobDate;
                                            insertTrayDetailMovementTran.Wfmid = pwtLastRecord.Wfmid;
                                            insertTrayDetailMovementTran.ClientId = trayDetailMovement.ClientId;
                                            insertTrayDetailMovementTran.ClientShortName = trayDetailMovement.ClientShortName;
                                            insertTrayDetailMovementTran.DepartmentId = job.DepartmentId;
                                            insertTrayDetailMovementTran.Department = trayDetailMovement.Department;
                                            insertTrayDetailMovementTran.EmployeeId = employees.FirstOrDefault().EmployeeId;
                                            insertTrayDetailMovementTran.EmployeeName = employeeDetails.EmployeeCode + "-" + employeeDetails.EmployeeName;
                                            insertTrayDetailMovementTran.Status = trayDetailMovement.CurrentStatus;
                                            insertTrayDetailMovementTran.Process = trayDetailMovement.CurrentTray;
                                            if (selectedJobs.Count() > 1)
                                            {
                                                if (job.AllocatedEstimatedTime > 0)
                                                {
                                                    insertTrayDetailMovementTran.EstimatedTime = job.AllocatedEstimatedTime;
                                                }
                                                else
                                                {
                                                    if (estTime > 0)
                                                    {
                                                        insertTrayDetailMovementTran.EstimatedTime = (int)estTime;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (emps.EstimatedTime > 0)
                                                {
                                                    insertTrayDetailMovementTran.EstimatedTime = emps.EstimatedTime;
                                                }
                                                else
                                                {
                                                    if (estTime > 0)
                                                    {
                                                        insertTrayDetailMovementTran.EstimatedTime = (int)estTime;
                                                    }
                                                }
                                            }
                                            insertTrayDetailMovementTran.IsDeleted = false;
                                            insertTrayDetailMovementTran.CreatedBy = processMovement.EmployeeId;
                                            insertTrayDetailMovementTran.CreatedUtc = DateTime.UtcNow;
                                            insertTrayDetailMovementTran.IsBench = processMovement.IsBench;
                                            _context.TrayDetailMovementTrans.Add(insertTrayDetailMovementTran);
                                            //_context.SaveChanges();
                                        }

                                        if (employees != null && processMovement.ProcessId == 2 || processMovement.ProcessId == 14)
                                        {
                                            var checkForDeletingTDM = _context.TrayDetailMovements.Where(x => x.FileName == jobOrder1.FileName && x.CustomerJobType == jobOrder1.CustomerJobType && x.JobStatus == js.JobStatusDescription && x.IsDeleted == false && x.CurrentTray == "Production").OrderByDescending(x => x.Id).ToList();
                                            foreach (var item in checkForDeletingTDM.Skip(1))
                                            {
                                                item.IsDeleted = true;
                                                item.UpdatedBy = processMovement.EmployeeId;
                                                item.UpdatedUtc = DateTime.UtcNow;
                                                _context.Entry(item).State = EntityState.Modified;
                                            }
                                            if (employees.Count() > 1 && processMovement.ProcessId == 2 || processMovement.ProcessId == 14)
                                            {
                                                foreach (var item in employees.Skip(1))
                                                {
                                                    TrayDetailMovement insertTrayDetailMovement = new TrayDetailMovement();
                                                    insertTrayDetailMovement.JobDate = trayDetailMovement.JobDate;
                                                    insertTrayDetailMovement.FileName = trayDetailMovement.FileName;
                                                    insertTrayDetailMovement.ClientId = trayDetailMovement.ClientId;
                                                    insertTrayDetailMovement.ClientShortName = trayDetailMovement.ClientShortName;
                                                    insertTrayDetailMovement.Department = trayDetailMovement.Department;
                                                    insertTrayDetailMovement.JobStatus = trayDetailMovement.JobStatus;
                                                    insertTrayDetailMovement.CustomerJobType = trayDetailMovement.CustomerJobType;
                                                    insertTrayDetailMovement.CcintakenBy = trayDetailMovement.CcintakenBy;
                                                    if (processes.NextProcessId == null)
                                                    {
                                                        insertTrayDetailMovement.CurrentTray = "Client";
                                                    }
                                                    else
                                                    {
                                                        insertTrayDetailMovement.CurrentTray = _context.Processes.FirstOrDefault(x => x.Id == processes.NextProcessId).Name;
                                                    }
                                                    insertTrayDetailMovement.CurrentStatus = _context.Statuses.FirstOrDefault(x => x.Id == processMovement.StatusId).Name;
                                                    if (processMovement.SelectedScopeId != 0)
                                                    {
                                                        var scopeDesc = _context.Scopes.FirstOrDefault(x => x.Id == processMovement.SelectedScopeId && x.IsDeleted == false);
                                                        insertTrayDetailMovement.Scope = scopeDesc.Description;
                                                    }
                                                    if (processes.NextProcessId == null)
                                                    {
                                                        insertTrayDetailMovement.CurrentTray = "Client";
                                                    }
                                                    else
                                                    {
                                                        insertTrayDetailMovement.CurrentTray = _context.Processes.FirstOrDefault(x => x.Id == processes.NextProcessId).Name;
                                                    }
                                                    insertTrayDetailMovement.CurrentStatus = _context.Statuses.FirstOrDefault(x => x.Id == processMovement.StatusId).Name;
                                                    if (empName != null)
                                                    {
                                                        insertTrayDetailMovement.ProductionAllocatedBy = empName.EmployeeCode + "-" + empName.EmployeeName;
                                                    }
                                                    var empDetails = _context.Employees.FirstOrDefault(x => x.EmployeeId == item.EmployeeId);
                                                    insertTrayDetailMovement.ProductionAllocatedToEmpId = empDetails.EmployeeId;
                                                    insertTrayDetailMovement.ProductionAllocatedTo = empDetails.EmployeeCode + "-" + empDetails.EmployeeName;
                                                    if (selectedJobs.Count() > 1)
                                                    {
                                                        if (job.AllocatedEstimatedTime > 0)
                                                        {
                                                            insertTrayDetailMovement.EstimationTime = job.AllocatedEstimatedTime;
                                                        }
                                                        else
                                                        {
                                                            if (estTime > 0)
                                                            {
                                                                insertTrayDetailMovement.EstimationTime = estTime;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (item.EstimatedTime > 0)
                                                        {
                                                            insertTrayDetailMovement.EstimationTime = item.EstimatedTime;
                                                        }
                                                        else
                                                        {
                                                            if (estTime > 0)
                                                            {
                                                                insertTrayDetailMovement.EstimationTime = estTime;
                                                            }
                                                        }
                                                    }
                                                    insertTrayDetailMovement.IsDeleted = false;
                                                    insertTrayDetailMovement.CreatedBy = processMovement.EmployeeId;
                                                    insertTrayDetailMovement.CreatedUtc = DateTime.UtcNow;
                                                    _context.TrayDetailMovements.Add(insertTrayDetailMovement);
                                                }
                                            }
                                            //_context.SaveChanges();
                                        }
                                    }
                                }
                                //flaten table insertion ended
                                if (processMovement.ProcessId == 2 && processMovement.StatusId == 27)
                                {
                                    result = new
                                    {
                                        Success = true,
                                        Message = "Job(s) Splitted Successfully",
                                        OrderId = 0,
                                        WFTId = wftid,
                                        Wfmid = job.TranMasterId,
                                        ProcessId = 2
                                    };
                                }
                                else
                                {
                                    result = new
                                    {
                                        Success = true,
                                        Message = "Job(s) moved!",
                                        OrderId = 0,
                                        WFTId = 0,
                                        Wfmid = 0,
                                        ProcessId = 0
                                    };
                                }
                                var employeeAssigns = _context.EmployeeAssigns.Where(x => x.Wftid == pwtLastRecord.Id && (x.Status == "Assigned" || x.Status == "Work Incomplete") && x.IsActive == true).OrderBy(x => x.Id).AsQueryable();
                                foreach (var item in employeeAssigns)
                                {
                                    item.IsActive = false;
                                    item.UpdatedBy = processMovement.EmployeeId;
                                    item.UpdatedUtc = DateTime.UtcNow;
                                    _context.Entry(item).State = EntityState.Modified;
                                }
                                if (employees != null && processMovement.StatusId != 27)
                                {
                                    foreach (var employee in employees)
                                    {
                                        EmployeeAssign empAssign = new EmployeeAssign();
                                        empAssign.AllocattedDate = DateTime.UtcNow;
                                        if (selectedJobs.Count() > 1)
                                        {
                                            if (job.AllocatedEstimatedTime > 0)
                                            {
                                                empAssign.EstimatedTime = job.AllocatedEstimatedTime;
                                            }
                                            else
                                            {
                                                if (estTime > 0)
                                                {
                                                    empAssign.EstimatedTime = (int)estTime;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (employee.EstimatedTime > 0)
                                            {
                                                empAssign.EstimatedTime = employee.EstimatedTime;
                                            }
                                            else
                                            {
                                                if (estTime > 0)
                                                {
                                                    empAssign.EstimatedTime = (int)estTime;
                                                }
                                            }
                                        }
                                        empAssign.EmployeeId = employee.EmployeeId;
                                        empAssign.Wftid = insertPWT.Id;//_tableWork.ProcessWorkFlowTranRepository.GetLastRecord(s=>s.Id).Id;
                                        empAssign.Wfmid = job.TranMasterId;
                                        empAssign.Wft = insertPWT;
                                        empAssign.Jid = job.JId;
                                        empAssign.Status = "Assigned";
                                        empAssign.IsActive = true;
                                        empAssign.IsBench = processMovement.IsBench;
                                        empAssign.ProcessId = processes.NextProcessId;
                                        empAssign.CreatedBy = processMovement.EmployeeId;
                                        empAssign.CreatedUtc = DateTime.UtcNow;
                                        _context.EmployeeAssigns.Add(empAssign);
                                    }
                                    //_context.SaveChanges();
                                    var employeeAssignLocalRecords = _context.EmployeeAssigns.Local.ToList();
                                    var empAssignList = _context.EmployeeAssigns.Where(x => x.Wfmid == job.TranMasterId).ToList();
                                    var empAssignTotalRecords = empAssignList.Concat(employeeAssignLocalRecords).ToList();
                                    var empAssignMinValue = empAssignTotalRecords.GroupBy(x => new { x.Wftid, x.EmployeeId }).Select(grp => new
                                    {
                                        Min = grp.Min(t => t.Id)
                                    });
                                    var empAssignMinValuesList = empAssignMinValue.ToList().Select(x => x.Min);
                                    var getEmployeesForPendingJobs = (from ea in empAssignTotalRecords
                                                                      join emp in _context.Employees on ea.EmployeeId equals emp.EmployeeId
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
                                                                      }).Where(x => x.Wfmid == job.TranMasterId && x.ProcessId == processes.NextProcessId && x.IsActive == true).ToList();
                                    var EstimatedTimeSum = getEmployeesForPendingJobs.Sum(x => x.EstimatedTime);
                                    insertPWT.EstimatedTime = EstimatedTimeSum;
                                    pwtLastRecord.AllocatedEstimatedTime = EstimatedTimeSum;
                                    pwtFirstRecord.AllocatedEstimatedTime = EstimatedTimeSum;
                                    if (getEmployeesForPendingJobs.Count() == 1)
                                    {
                                        pwtLastRecord.EmployeeCount = 1;
                                        pwtLastRecord.EmployeeName = getEmployeesForPendingJobs.FirstOrDefault().EmployeeName;
                                        pwtFirstRecord.EmployeeName = pwtLastRecord.EmployeeName;
                                    }
                                    else
                                    {
                                        pwtLastRecord.EmployeeCount = getEmployeesForPendingJobs.Count();
                                        pwtFirstRecord.EmployeeCount = getEmployeesForPendingJobs.Count();
                                        if (pwtFirstRecord.EmployeeName != null)
                                        {
                                            pwtFirstRecord.EmployeeName = null;
                                        }
                                    }
                                    //_context.SaveChanges();
                                    if (processMovement.ProcessId == 2 || processMovement.ProcessId == 14)
                                    {
                                        var Getjobcategory = _context.JobCategories.Where(x => x.EstimatedTime >= insertPWT.EstimatedTime).FirstOrDefault();
                                        var saveProcessWorkFLowMaster = _context.ProcessWorkFlowMasters.Find(job.TranMasterId);
                                        var updateJobOrderForScope = _context.JobOrders.FirstOrDefault(x => x.Id == job.JId);
                                        saveProcessWorkFLowMaster.ScopeId = processMovement.SelectedScopeId == 0 ? null : processMovement.SelectedScopeId;
                                        updateJobOrderForScope.ScopeId = processMovement.SelectedScopeId == 0 ? null : processMovement.SelectedScopeId;
                                        updateJobOrderForScope.EstimatedTime = insertPWT.EstimatedTime;
                                        if (processMovement.ProcessId == 2 || processMovement.ProcessId == 14)
                                        {
                                            if (Getjobcategory == null)
                                            {
                                                var Getlastjobcategory = _context.JobCategories.OrderByDescending(x => x.Id).FirstOrDefault();
                                                saveProcessWorkFLowMaster.JobCategoryId = Getlastjobcategory.Id;
                                                updateJobOrderForScope.JobCategoryId = Getlastjobcategory.Id;
                                            }
                                            else
                                            {
                                                saveProcessWorkFLowMaster.JobCategoryId = Getjobcategory.Id;
                                                updateJobOrderForScope.JobCategoryId = Getjobcategory.Id;
                                            }
                                        }
                                        saveProcessWorkFLowMaster.UpdatedBy = processMovement.EmployeeId;
                                        saveProcessWorkFLowMaster.UpdatedUtc = DateTime.UtcNow;
                                        updateJobOrderForScope.UpdatedBy = processMovement.EmployeeId;
                                        updateJobOrderForScope.UpdatedUtc = DateTime.UtcNow;
                                        _context.Entry(saveProcessWorkFLowMaster).State = EntityState.Modified;
                                        _context.Entry(updateJobOrderForScope).State = EntityState.Modified;
                                        //_context.SaveChanges();
                                    }
                                    result = new
                                    {
                                        Success = true,
                                        Message = "Job(s) Assigned",
                                        OrderId = 0,
                                        WFTId = insertPWT.Id,
                                        Wfmid = job.TranMasterId,
                                        ProcessId = 9
                                    };
                                }
                                if (processMovement.StatusId == 6 || processMovement.StatusId == 7 || processMovement.StatusId == 8 || processMovement.StatusId == 9 || processMovement.StatusId == 22)
                                {
                                    var jobQueryCheck = _context.JobOrderExtensions.Where(x => x.Jid == job.JId && (x.Date.Value.Year == DateTime.UtcNow.Year && x.Date.Value.Month == DateTime.UtcNow.Month && x.Date.Value.Day == DateTime.UtcNow.Day) && x.StatusId == processMovement.StatusId && x.Flag == true).OrderByDescending(x => x.Id).FirstOrDefault();
                                    var statusDesc = _context.Statuses.FirstOrDefault(x => x.Id == processMovement.StatusId && x.IsActive == true);
                                    JobQuery jobQuery = new JobQuery();
                                    DateTime targetTime = new DateTime();
                                    if (processMovement.EmployeeId != 0)
                                    {
                                        jobQuery.QueryRaisedBy = processMovement.EmployeeId;
                                        jobQuery.CreatedBy = processMovement.EmployeeId;
                                    }
                                    jobQuery.QueryRaisedOn = DateTime.UtcNow;
                                    jobQuery.Wftid = insertPWT.Id;
                                    jobQuery.Wft = insertPWT;
                                    jobQuery.Wfmid = job.TranMasterId;
                                    jobQuery.JobId = processMovement.JobId;
                                    jobQuery.Jid = job.JId;
                                    jobQuery.ProcessId = pwtLastRecord.ProcessId;
                                    jobQuery.StatusId = processMovement.StatusId;
                                    jobQuery.Comments = processMovement.Remarks;
                                    jobQuery.EstimatedTime = processMovement.Value;
                                    if (processMovement.SelectedScopeId != 0)  // Query 
                                    {
                                        jobQuery.ScopeId = processMovement.SelectedScopeId;
                                    }
                                    if (jobQueryCheck == null)
                                    {
                                        var jobQueryChecktemp = _context.JobOrderExtensions.Where(x => x.Jid == job.JId && x.StatusId == processMovement.StatusId && x.Flag == true).OrderByDescending(x => x.Id).FirstOrDefault();

                                        if (jobQueryChecktemp != null)
                                        {
                                            var est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                                            targetTime = TimeZoneInfo.ConvertTimeFromUtc(jobQueryChecktemp.Date.Value, est);
                                            if (targetTime.Day != DateTime.Now.Day)
                                            {
                                                jobQuery.QueryDate = DateTime.UtcNow;
                                                JobOrderExtension joe = new JobOrderExtension();
                                                joe.Jid = job.JId;

                                                joe.Date = DateTime.UtcNow;
                                                joe.Wftid = insertPWT.Id;
                                                joe.Wft = insertPWT;
                                                joe.Flag = true;
                                                joe.StatusId = processMovement.StatusId;
                                                if (statusDesc != null)
                                                {
                                                    joe.Type = statusDesc.Name;
                                                }
                                                _context.JobOrderExtensions.Add(joe);
                                            }
                                        }
                                        else
                                        {
                                            jobQuery.QueryDate = DateTime.UtcNow;
                                            JobOrderExtension joe = new JobOrderExtension();

                                            joe.Jid = job.JId;
                                            joe.Date = DateTime.UtcNow;
                                            joe.Wftid = insertPWT.Id;
                                            joe.Wft = insertPWT;
                                            joe.Flag = true;
                                            joe.StatusId = processMovement.StatusId;
                                            if (statusDesc != null)
                                            {
                                                joe.Type = statusDesc.Name;
                                            }
                                            _context.JobOrderExtensions.Add(joe);
                                        }
                                    }
                                    if ((processMovement.StatusId == 6 || processMovement.StatusId == 8) && jobQueryCheck != null)
                                    {
                                        var jobQueryCheckForResp = _context.JobOrderExtensions.Where(x => x.Jid == job.JId && (x.Date.Value.Year == DateTime.UtcNow.Year && x.Date.Value.Month == DateTime.UtcNow.Month && x.Date.Value.Day == DateTime.UtcNow.Day) && x.StatusId == (processMovement.StatusId + 1) && x.Flag == true).OrderByDescending(x => x.Id).FirstOrDefault();
                                        if (jobQueryCheckForResp != null)
                                        {
                                            var exten = _context.JobOrderExtensions.Where(x => x.Jid == job.JId && x.Flag == true).ToList();
                                            foreach (var item in exten)
                                            {
                                                item.Flag = false;
                                                _context.Entry(item).State = EntityState.Modified;
                                            }
                                            JobOrderExtension joe = new JobOrderExtension();

                                            joe.Jid = job.JId;
                                            joe.Date = DateTime.UtcNow;
                                            joe.Wftid = insertPWT.Id;
                                            joe.Wft = insertPWT;
                                            joe.Flag = true;
                                            joe.StatusId = processMovement.StatusId;
                                            if (statusDesc != null)
                                            {
                                                joe.Type = statusDesc.Name;
                                            }
                                            _context.JobOrderExtensions.Add(joe);
                                        }
                                    }
                                    if (statusDesc != null)
                                    {
                                        jobQuery.Status = statusDesc.Name;
                                    }
                                    if (processMovement.StatusId == 9)
                                    {
                                        var updatejoborder = _context.JobOrders.FirstOrDefault(x => x.Id == job.JId);
                                        updatejoborder.SpecialPrice = processMovement.Amount;
                                        updatejoborder.IsSpecialPrice = true;
                                        updatejoborder.IsQuatation = false;
                                        updatejoborder.PricingTypeId = 2;
                                        updatejoborder.UpdatedUtc = DateTime.UtcNow;
                                        _context.Entry(updatejoborder).State = EntityState.Modified;
                                    }
                                    if (processMovement.EstimatedTime != null && processMovement.EstimatedTime != 0)
                                    {
                                        jobQuery.EstimatedTime = processMovement.EstimatedTime;
                                    }
                                    jobQuery.SpecialPrice = processMovement.Amount;
                                    jobQuery.StitchCount = processMovement.StitchCount;
                                    jobQuery.CreatedUtc = DateTime.UtcNow;
                                    _context.JobQueries.Add(jobQuery);
                                    if ((processMovement.ProcessId == null) || (processMovement.ProcessId == 1 && processMovement.StatusId == 7) || (processMovement.ProcessId == 1 && processMovement.StatusId == 9))
                                    {
                                        var updatequeryjobdate = _context.JobOrders.FirstOrDefault(x => x.Id == job.JId);
                                        updatequeryjobdate.QueryJobDate = DateTime.UtcNow;
                                        updatequeryjobdate.UpdatedUtc = DateTime.UtcNow;
                                        _context.Entry(updatequeryjobdate).State = EntityState.Modified;
                                    }
                                    result = new
                                    {
                                        Success = true,
                                        Message = "Job sent as query",
                                        OrderId = 0,
                                        WFTId = 0,
                                        Wfmid = 0,
                                        ProcessId = 0
                                    };
                                }
                                else if (processMovement.StatusId == 19) // quotation
                                {
                                    var jobquerydetails = _context.JobQueries.Where(x => x.JobId == processMovement.JobId).OrderByDescending(x => x.Id).FirstOrDefault();
                                    var updateJobOrder = _context.JobOrders.FirstOrDefault(x => x.JobId == processMovement.JobId);
                                    var statusDesc = _context.Statuses.FirstOrDefault(x => x.Id == processMovement.StatusId && x.IsActive == true);
                                    if (jobquerydetails != null)
                                    {
                                        JobQuery saveJobQuery = new JobQuery();
                                        saveJobQuery.Wftid = insertPWT.Id;
                                        saveJobQuery.Wft = insertPWT;
                                        saveJobQuery.Wfmid = job.TranMasterId;
                                        saveJobQuery.JobId = processMovement.JobId;
                                        if (updateJobOrder != null)
                                        {
                                            saveJobQuery.Jid = updateJobOrder.Id;
                                        }
                                        saveJobQuery.ProcessId = pwtLastRecord.ProcessId;
                                        if (processMovement.EmployeeId != 0)
                                        {
                                            saveJobQuery.QueryRaisedBy = processMovement.EmployeeId;
                                            saveJobQuery.CreatedBy = processMovement.EmployeeId;
                                            updateJobOrder.UpdatedBy = processMovement.EmployeeId;
                                        }
                                        saveJobQuery.QueryRaisedOn = DateTime.UtcNow;
                                        saveJobQuery.StatusId = jobquerydetails.StatusId;
                                        saveJobQuery.EstimatedTime = jobquerydetails.EstimatedTime;
                                        if (jobquerydetails.ScopeId != 0)  //added
                                        {
                                            saveJobQuery.ScopeId = jobquerydetails.ScopeId;
                                        }
                                        if (statusDesc != null)
                                        {
                                            saveJobQuery.Status = statusDesc.Name;
                                        }
                                        saveJobQuery.DateofDelivery = jobquerydetails.DateofDelivery;
                                        saveJobQuery.StitchCount = jobquerydetails.StitchCount;
                                        saveJobQuery.SpecialPrice = processMovement.Amount;
                                        saveJobQuery.Comments = processMovement.Remarks;
                                        saveJobQuery.Validity = jobquerydetails.Validity;
                                        saveJobQuery.CreatedUtc = DateTime.UtcNow;
                                        _context.JobQueries.Add(saveJobQuery);

                                        updateJobOrder.DateofUpload = DateTime.UtcNow;
                                        updateJobOrder.UpdatedUtc = DateTime.UtcNow;
                                        _context.Entry(updateJobOrder).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        JobQuery saveJobQuery = new JobQuery();
                                        saveJobQuery.Wftid = insertPWT.Id;
                                        saveJobQuery.Wft = insertPWT;
                                        saveJobQuery.Wfmid = job.TranMasterId;
                                        saveJobQuery.JobId = processMovement.JobId;
                                        if (updateJobOrder != null)
                                        {
                                            saveJobQuery.Jid = updateJobOrder.Id;
                                        }
                                        saveJobQuery.ProcessId = pwtLastRecord.ProcessId;
                                        if (processMovement.EmployeeId != 0)
                                        {
                                            saveJobQuery.QueryRaisedBy = processMovement.EmployeeId;
                                            updateJobOrder.UpdatedBy = processMovement.EmployeeId;
                                        }
                                        saveJobQuery.QueryRaisedOn = DateTime.UtcNow;
                                        saveJobQuery.StatusId = processMovement.StatusId;
                                        saveJobQuery.EstimatedTime = processMovement.EstimationTime;
                                        if (processMovement.ScopeId != 0)
                                        {
                                            saveJobQuery.ScopeId = processMovement.ScopeId;
                                        }
                                        if (statusDesc != null)
                                        {
                                            saveJobQuery.Status = statusDesc.Name;
                                        }
                                        saveJobQuery.DateofDelivery = processMovement.DateofDelivery;
                                        saveJobQuery.StitchCount = processMovement.StitchCount;
                                        saveJobQuery.SpecialPrice = processMovement.Amount;
                                        saveJobQuery.Comments = processMovement.Comments;
                                        saveJobQuery.Validity = processMovement.Validity;
                                        _context.JobQueries.Add(saveJobQuery);

                                        updateJobOrder.JobClosedUtc = DateTime.UtcNow;
                                        updateJobOrder.UpdatedUtc = DateTime.UtcNow;
                                        _context.Entry(updateJobOrder).State = EntityState.Modified;

                                    }
                                    result = new
                                    {
                                        Success = true,
                                        Message = "Job sent as quotation",
                                        OrderId = updateJobOrder.Id,
                                        WFTId = 0,
                                        Wfmid = 0,
                                        ProcessId = 0
                                    };
                                }
                                else if (processMovement.StatusId == 3 || processMovement.StatusId == 5 || processMovement.StatusId == 13)
                                {
                                    var jobOrderforCancel = _context.JobOrders.FirstOrDefault(x => x.Id == job.JId);
                                    jobOrderforCancel.StatusId = processMovement.StatusId;
                                    jobOrderforCancel.IsCancelled = true;
                                    jobOrderforCancel.UpdatedBy = processMovement.EmployeeId;
                                    jobOrderforCancel.UpdatedUtc = DateTime.UtcNow;
                                    _context.Entry(jobOrderforCancel).State = EntityState.Modified;

                                    var jobOrderForRevisionDisable = _context.JobOrders.Where(x => x.ClientId == jobOrderforCancel.ClientId && x.FileName == jobOrderforCancel.FileName && x.DepartmentId == jobOrderforCancel.DepartmentId && x.IsDeleted == false && x.Id != job.JId).OrderByDescending(x => x.Id).FirstOrDefault();
                                    if (jobOrderForRevisionDisable != null)
                                    {
                                        jobOrderForRevisionDisable.ClientRevision = false;
                                        jobOrderForRevisionDisable.UpdatedBy = processMovement.EmployeeId;
                                        jobOrderForRevisionDisable.UpdatedUtc = DateTime.UtcNow;
                                        _context.Entry(jobOrderForRevisionDisable).State = EntityState.Modified;
                                    }
                                }
                                bool? isBulkCheck = false;
                                if (job.ClientId != 0)
                                {
                                    isBulkCheck = _context.Customers.FirstOrDefault(x => x.Id == job.ClientId && x.IsDeleted == false).IsBulk;
                                }

                                //if ((processMovement.StatusId == 12 && processMovement.CopyFiles == true && (job.ClientId != 1 && job.ClientId != 53) && isBulkCheck != true) || processMovement.StatusId != 12)////
                                //if ((processMovement.StatusId == 12 && processMovement.CopyFiles == true && (job.IsJobFilesNotTranfer!=true) && isBulkCheck != true) || processMovement.StatusId != 12)//
                                if ((processMovement.StatusId == 12 && processMovement.CopyFiles == true && job.IsJobFilesNotTransfer != true && isBulkCheck != true) || processMovement.StatusId != 12)
                                {
                                    var jobOrder = _context.JobOrders.Find(job.JId);
                                    var path = "";
                                    if (processMovement.AutoUploadJobs == true)
                                    {
                                        path = _configuration.GetSection("AllocationService").GetValue<string>("jobFilesPath") + insertPWT.FileUploadPath;
                                        // path = ConfigurationManager.AppSettings["jobFilesPath"] + insertPWT.FileUploadPath;
                                    }
                                    else
                                    {
                                        path = Path.Combine(_webHostEnvironment.ContentRootPath, "JobFiles", insertPWT.FileUploadPath == null ? "" : insertPWT.FileUploadPath);//HttpContext.Current.Server.MapPath("~/JobFiles//" + insertPWT.FileUploadPath);
                                    }
                                    if (path != "")
                                    {

                                        var filesToCopy = Directory.GetFiles(path).Select(Path.GetFileName).ToList();
                                        foreach (var fileName in filesToCopy)
                                        {
                                            var jobOrderFile = new JobOrderFile();
                                            jobOrderFile.Wfmid = job.TranMasterId;
                                            jobOrderFile.Wftid = insertPWT.Id;
                                            jobOrderFile.Wft = insertPWT;
                                            jobOrderFile.JobId = job.JId;
                                            jobOrderFile.ProcessId = insertPWT.ProcessId;
                                            jobOrderFile.IsActive = true;
                                            jobOrderFile.FileName = fileName;
                                            jobOrderFile.CreatedUtc = DateTime.UtcNow;
                                            _context.JobOrderFiles.Add(jobOrderFile);
                                        }
                                    }
                                    if (insertPWT.ProcessId == null)
                                    {
                                        if (insertPWT.FileUploadPath != null)
                                        {
                                            if (processMovement.AutoUploadJobs == true)
                                            {
                                                var folderPath = _configuration.GetSection("AllocationService").GetValue<string>("jobFilesPath") + insertPWT.FileUploadPath;
                                                var filesToCopy1 = Directory.GetFiles(folderPath).Select(Path.GetFileName).ToList();
                                                foreach (var file in filesToCopy1)
                                                {
                                                    FTPFileUpload(true, file, insertPWT.FileUploadPath);
                                                }
                                            }
                                            else
                                            //normal process and existing process
                                            {
                                                //  var fileUpload = new FileUpload(_configuration);
                                                fileUpload.FTPFileCopy(insertPWT.FileUploadPath);
                                            }

                                        }

                                        //code for query resp file not showing if attach there
                                        if (jobOrder.FileUploadPath != null && processMovement.StatusId != 12)
                                        {
                                            // var fileUpload = new FileUpload(_configuration);
                                            fileUpload.FTPFileCopy(jobOrder.FileUploadPath);
                                        }
                                    }
                                }
                            }
                            if (processes.NextProcessId == 1 && processMovement.StatusId == 18)
                            {
                                var jobOrder = _context.JobOrders.Find(job.JId);
                                jobOrder.JobClosedUtc = DateTime.UtcNow;
                                jobOrder.UpdatedBy = processMovement.EmployeeId;
                                jobOrder.UpdatedUtc = DateTime.UtcNow;
                                _context.Entry(jobOrder).State = EntityState.Modified;

                                var js = _context.JobStatuses.FirstOrDefault(x => x.Id == jobOrder.JobStatusId);
                                var checks = _context.TrayDetailMovements.Where(x => x.FileName == jobOrder.FileName && x.CustomerJobType == jobOrder.CustomerJobType && x.JobStatus == js.JobStatusDescription && x.IsDeleted == false).OrderByDescending(x => x.Id).ToList();
                                foreach (var item in checks)
                                {
                                    item.StitchCount = null;
                                    item.UpdatedBy = processMovement.EmployeeId;
                                    item.UpdatedUtc = DateTime.UtcNow;
                                    _context.Entry(item).State = EntityState.Modified;
                                }
                                var trayDetailMovement = _context.TrayDetailMovements.Local.FirstOrDefault();
                                trayDetailMovement.StitchCount = Convert.ToInt32(jobOrder.StitchCount);

                                string month = jobOrder.FileReceivedDate.ToString("MM");
                                int day = jobOrder.FileReceivedDate.Day;
                                int year = jobOrder.FileReceivedDate.Year;
                                var jobSummaryLastRecord = _context.JobSummaries.Where(x => x.ClientId == jobOrder.ClientId && x.MonthYear == month + "-" + year).OrderByDescending(x => x.Id).FirstOrDefault();
                                var jobStatus = _context.JobStatuses.Where(x => x.Id == jobOrder.JobStatusId).FirstOrDefault();
                                int jobStatusCount = 1;
                                if (jobStatus.JobStatusDescription.Contains("Fresh"))
                                {
                                    if (jobSummaryLastRecord.FreshCompleted == null)
                                    {
                                        jobSummaryLastRecord.FreshCompleted = 0;
                                    }
                                    jobSummaryLastRecord.FreshCompleted = jobSummaryLastRecord.FreshCompleted + jobStatusCount;
                                }
                                if (jobStatus.JobStatusDescription.Contains("Fresh-Rush")) //
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
                                _context.Entry(jobSummaryLastRecord).State = EntityState.Modified;
                                //_context.SaveChanges();
                            }

                            var checkCCEmailIdinJO = _unitWork.JobOrder.Where(x => x.IsDeleted == false && x.Id == job.JId).FirstOrDefault();//
                            if (processMovement.StatusId == 12)//1st if
                            {
                                var jobOrder = _context.JobOrders.FirstOrDefault(x => x.Id == job.JId);
                                jobOrder.DateofUpload = DateTime.UtcNow;
                                if (processMovement.EmployeeId != 0)
                                {
                                    jobOrder.UpdatedBy = processMovement.EmployeeId;
                                }
                                var checkcustomer = _unitWork.Customer.Where(x => x.IsDeleted == false && x.Id == job.ClientId).FirstOrDefault();
                                if (checkcustomer.BunchMail == true)
                                {
                                    var updatejoborderforscheduledmail = new JobOrder();
                                    updatejoborderforscheduledmail = _context.JobOrders.AsNoTracking().FirstOrDefault(x => x.Id == job.JId);
                                    if (updatejoborderforscheduledmail != null)
                                    {
                                        jobOrder.IsAutoUploadCount = 5;
                                    }
                                }
                                jobOrder.UpdatedUtc = DateTime.UtcNow;
                                _context.Entry(jobOrder).State = EntityState.Modified;

                                // var fileUpload = new FileUpload(_configuration);
                                var jobOrderLocalDetails = _context.JobOrders.Local.FirstOrDefault(x => x.Id == job.JId);
                                if (jobOrderLocalDetails != null)
                                {
                                    var ftpPath = _context.CustomerDepartments.FirstOrDefault(x => x.CustomerId == jobOrderLocalDetails.ClientId && x.DepartmentId == jobOrderLocalDetails.DepartmentId && x.IsActive == true);
                                    if (ftpPath != null)
                                    {
                                        if (ftpPath.InputType == "FTP")
                                        {
                                            fileUpload.FTPFileCopyWithBatch(insertPWT.FileUploadPath, jobOrderLocalDetails.DateofUpload, jobOrderLocalDetails.FileName, ftpPath.OutFtppath, ftpPath.FtpuserName, ftpPath.Ftppassword);
                                        }
                                        else if (ftpPath.InputType == "SFTP")
                                        {
                                            //fileUpload.SFTPFileCopyWithBatch(insertPWT.FileUploadPath, jobOrderLocalDetails.DateofUpload, jobOrderLocalDetails.FileName, ftpPath.OutFTPPath, ftpPath.FTPUserName, ftpPath.FTPPassword, ftpPath.Host, Convert.ToInt32(ftpPath.Port));
                                        }
                                    }
                                }
                                var js = _context.JobStatuses.FirstOrDefault(x => x.Id == jobOrder.JobStatusId);
                                var checks = _context.TrayDetailMovements.Where(x => x.FileName == jobOrder.FileName && x.CustomerJobType == jobOrder.CustomerJobType && x.JobStatus == js.JobStatusDescription && x.IsDeleted == false).OrderByDescending(x => x.Id).ToList();
                                foreach (var item in checks)
                                {
                                    item.StitchCount = null;
                                    item.Scope = "";
                                    item.UpdatedBy = processMovement.EmployeeId;
                                    item.UpdatedUtc = DateTime.UtcNow;
                                    _context.Entry(item).State = EntityState.Modified;
                                }
                                var trayDetailMovement = _context.TrayDetailMovements.Local.FirstOrDefault();
                                if(trayDetailMovement != null)
                                {
                                    trayDetailMovement.StitchCount = Convert.ToInt32(jobOrder.StitchCount);
                                }
                                if (jobOrder.ScopeId != 0 || jobOrder.ScopeId != null)
                                {
                                    var scopeDesc = _context.Scopes.FirstOrDefault(x => x.Id == jobOrder.ScopeId && x.IsDeleted == false);
                                    trayDetailMovement.Scope = scopeDesc.Description;
                                }

                                //_context.SaveChanges();
                                if (jobOrder.ClientOrderId != null)
                                {
                                    var clientOrder = _context.ClientOrders.FirstOrDefault(x => x.OrderId == jobOrder.ClientOrderId);
                                    clientOrder.IsCompleted = true;
                                    if (processMovement.EmployeeId != 0)
                                    {
                                        clientOrder.UpdatedBy = processMovement.EmployeeId;
                                    }
                                    clientOrder.UpdatedUtc = DateTime.UtcNow;
                                    _context.Entry(clientOrder).State = EntityState.Modified;
                                    //_context.SaveChanges();
                                }
                                result = new
                                {
                                    Success = true,
                                    Message = "Job Uploaded to Client!",
                                    //OrderId = jobId,
                                    OrderId = job.JId,
                                    WFTId = 0,
                                    Wfmid = 0,
                                    ProcessId = 0
                                };
                                step += "step - 0";
                                //
                                var checkcustomerforbunchmail = _unitWork.Customer.Where(x => x.IsDeleted == false && x.Id == job.ClientId).FirstOrDefault();
                                if (processMovement.AutoUploadJobs == true && checkcustomerforbunchmail.BunchMail == false)
                                {
                                    step += "step - 0+1";
                                    var errorDesc = "";
                                    var jobOrderDetails = new JobOrder();
                                    var pwtDetails = new ProcessWorkFlowTran();
                                    var body = "";
                                    var body1 = "";
                                    var body2 = "";
                                    string ftpUrl;
                                    ftpUrl = _configuration.GetSection("AllocationService").GetValue<string>("downloadUrl");


                                    if (job.JId != 0)
                                    {
                                        jobOrderDetails = _context.JobOrders.FirstOrDefault(x => x.Id == job.JId);
                                        pwtDetails = null;
                                    }
                                    //if (jobOrderDetails == null || jobOrderDetails.ClientOrderId == null)
                                    if (jobOrderDetails == null || (jobOrderDetails.ClientSalesPerson == null && jobOrderDetails.ClientOrderId == null))//
                                    {
                                        using (var transaction1 = _context.Database.BeginTransaction())
                                        {
                                            Log savelog = new Log();
                                            savelog.Module = "Completed Job";
                                            savelog.Description = "Mail Not Sent";
                                            savelog.JobId = job.JId;
                                            savelog.Type = "Error";
                                            savelog.CreatedUtc = DateTime.UtcNow;
                                            _context.Logs.Add(savelog);
                                            _context.SaveChanges();
                                            transaction1.Commit();
                                        }
                                        //return null;
                                    }
                                    else
                                    {
                                        step += "step - 0+2";

                                        var emailIdAndName = new CustomerContact();//
                                        var getclientorderdetailsJO = new JobOrder();//
                                        var getclientorderdetails = new ClientOrder();//
                                        var CusOutputMailInfo = new Customer();//
                                        var jobStatusDescription = new JobStatus();//
                                        var mailNotify = new List<MailNotificationDetail>();//
                                        string jobstatusdes = "";//

                                        if (jobOrderDetails.CcemailId != null && jobOrderDetails.ClientOrderId == null)
                                        {

                                            getclientorderdetails = _context.ClientOrders.FirstOrDefault(x => x.OrderId == jobOrderDetails.ClientOrderId);

                                            getclientorderdetailsJO = _unitWork.JobOrder.FirstOrDefault(x => x.Id == jobOrderDetails.Id); // changed

                                            emailIdAndName = _context.CustomerContacts.FirstOrDefault(x => x.CustomerId == jobOrderDetails.ClientId && x.Email == jobOrderDetails.CcemailId && x.IsDeleted == false);

                                            CusOutputMailInfo = _context.Customers.FirstOrDefault(x => x.Id == jobOrderDetails.ClientId);

                                            jobStatusDescription = _context.JobStatuses.Where(x => x.Id == jobOrderDetails.JobStatusId).FirstOrDefault();

                                        }
                                        else
                                        {
                                            getclientorderdetails = _context.ClientOrders.FirstOrDefault(x => x.OrderId == jobOrderDetails.ClientOrderId); // correct..

                                            getclientorderdetailsJO = _unitWork.JobOrder.FirstOrDefault(x => x.ClientOrderId == jobOrderDetails.ClientOrderId);

                                            emailIdAndName = _context.CustomerContacts.FirstOrDefault(x => x.CustomerId == getclientorderdetails.ClientId && x.Email == getclientorderdetails.CcemailId && x.IsDeleted == false);

                                            CusOutputMailInfo = _context.Customers.FirstOrDefault(x => x.Id == getclientorderdetails.ClientId);


                                            jobStatusDescription = _context.JobStatuses.Where(x => x.Id == getclientorderdetails.JobStatus).FirstOrDefault();

                                        }
                                        var pwmRecord = _context.ProcessWorkFlowMasters.Where(x => x.JobId == job.JId).FirstOrDefault();
                                        var pwtRecord = _context.ProcessWorkFlowTrans.Where(x => x.Wfmid == pwmRecord.Id).OrderByDescending(x => x.Id).FirstOrDefault();
                                        var message = new MailMessage();
                                        message.To.Add(new MailAddress(emailIdAndName.Email));
                                        mailNotify = _context.MailNotificationDetails.Where(x => x.CompareMailId == emailIdAndName.Id && x.IsDeleted == false && (x.Task == "All" || x.Task == "Completed") && x.ClientId == getclientorderdetailsJO.ClientId).ToList();//
                                        if (mailNotify.Count() >= 1)
                                        {
                                            foreach (var item in mailNotify)
                                            {
                                                var customerContactDetails = _context.CustomerContacts.FirstOrDefault(x => x.Id == item.CcmailId && x.IsDeleted == false);
                                                if (customerContactDetails != null)
                                                {
                                                    message.CC.Add(new MailAddress(customerContactDetails.Email));
                                                }
                                            }
                                        }
                                        message.From = new MailAddress("visalert@vleadservices.com");
                                        if (jobStatusDescription.JobStatusDescription == "Fresh")//.
                                        {
                                            //jobStatusDescription = "New";
                                            jobstatusdes = "New";//
                                        }
                                        else if (jobStatusDescription.JobStatusDescription == "Fresh-Rush")////.
                                        {
                                            jobstatusdes = "New-Rush";//
                                        }

                                        //Attachments
                                        if (CusOutputMailInfo != null)
                                        {
                                            step += "step - 0+2";
                                            if (CusOutputMailInfo.OutputType == "Email")//.
                                            {
                                                if (jobOrderDetails.FileName != null)
                                                {

                                                    step += "step - 1";
                                                    try
                                                    {
                                                        step += "step - 2";
                                                        var FilePath = _configuration.GetSection("AllocationService").GetValue<string>("jobFilesPathAutoUpload");
                                                        string route = FilePath + pwtRecord.FileUploadPath;
                                                        string[] fileEntries = Directory.GetFiles(route);
                                                        Attachment attachment;
                                                        //ContentDisposition disposition;
                                                        long SizeallAttach = 0;
                                                        foreach (string fileName in fileEntries)
                                                        {
                                                            step += "step - 3";
                                                            var count = fileEntries.Count();
                                                            // var name = System.Web.MimeMapping.GetMimeMapping(fileName);
                                                            var name = Path.GetExtension(fileName);//MimeKit.MimeTypes.GetMimeType(fileName);
                                                            attachment = new Attachment(fileName, mediaType: name);
                                                            ContentDisposition disposition = attachment.ContentDisposition;
                                                            disposition.DispositionType = DispositionTypeNames.Attachment;
                                                            disposition.Size = new FileInfo(fileName).Length;
                                                            SizeallAttach += disposition.Size;
                                                            if (disposition.Size < 25000000)
                                                            {
                                                                step += "step - 4";
                                                                message.Attachments.Add(attachment);
                                                            }
                                                        }
                                                        if (SizeallAttach > 25000000)
                                                        {
                                                            step += "step - 5";
                                                            message.Attachments.Dispose();
                                                        }
                                                    }
                                                    catch (Exception Ex)
                                                    {
                                                        using (var transaction1 = _context.Database.BeginTransaction())
                                                        {
                                                            step += "step - 6";
                                                            Log savelog = new Log();
                                                            savelog.Module = "Catch block Mail attachments";
                                                            savelog.Description = "Mail Not Sent";
                                                            savelog.JobId = job.JId;
                                                            savelog.Type = "Error" + step;
                                                            savelog.CreatedUtc = DateTime.UtcNow;
                                                            _context.Logs.Add(savelog);
                                                            _context.SaveChanges();
                                                            transaction1.Commit();
                                                        }
                                                    }
                                                    finally
                                                    {

                                                    }
                                                }
                                            }
                                        }

                                        // if ((jobStatusDescription == "New") && (pwtDetails == null))
                                        if ((jobstatusdes == "New" || jobstatusdes == "New-Rush") && (pwtDetails == null))////
                                        {
                                            var AttachCount = message.Attachments.Count;

                                            body = "Hi " + emailIdAndName.ContactName + ",<br/><br/>" +
                                            "This e-mail is to keep you informed that the order  <b>" + jobOrderDetails.FileName + "</b> is completed and uploaded through VIS. Please download it from the portal or from this link <a href='" + ftpUrl + "api/Allocation/DownloadZipFile?path=" + pwtRecord.FileUploadPath + "'>Click here to Download.</a> Review and let us know your feedback." +
                                            "If the order needs to be revised, you may send us revisions through VIS itself.<br/><br/>" +
                                            "If you have any queries regarding this order, please send an e-mail to support@vleadservices.com or call us @ (469) 910-0091 / (714) 257-5484.<br/><br/>";

                                            body1 = "<b>Note:</b> Completed file size is more than 20 MB.  So, we unable to proceed with the attachments.  Please click the hyperlink to download the files.";

                                            body2 = "<b>Note:</b> Please DO NOT respond to this e-mail or use this e-mail address since it is not monitored.<br/><br/>" +
                                                    "Thank you for your interest with VLead Services.<br/><br/>" +
                                                    "Many thanks for your business.<br/>" +
                                                    "Support Team,<br/>" +
                                                    "VLead Design Services.";

                                            message.Subject = "Order Completed By VLead - (" + jobOrderDetails.FileName + ")";


                                            errorDesc = errorDesc + "," + "Step-2";

                                            message.IsBodyHtml = true;
                                            errorDesc = errorDesc + "," + "Step-4";
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
                                            var host = _configuration.GetSection("Smtp").GetValue<string>("host");
                                            var port = _configuration.GetSection("Smtp").GetValue<int>("port");
                                            var userName = _configuration.GetSection("Smtp").GetValue<string>("userName");
                                            var password = _configuration.GetSection("Smtp").GetValue<string>("password");
                                            var Ssl = _configuration.GetSection("Smtp").GetValue<bool>("SSL");
                                            var defaultCredential = _configuration.GetSection("Smtp").GetValue<bool>("defaultCredential");
                                            using (var smtp = new SmtpClient(host, port))
                                            {
                                                errorDesc = errorDesc + "," + "Step-4";
                                                //smtp.Send(message);////
                                                ////
                                                using (var transaction1 = _context.Database.BeginTransaction())
                                                {
                                                    var checkmailtriger = _context.JobOrders.FirstOrDefault(x => x.Id == job.JId);
                                                    if (checkmailtriger.IsMailTriggered == false || checkmailtriger.IsMailTriggered == null)
                                                    {
                                                        smtp.UseDefaultCredentials = defaultCredential;
                                                        smtp.Credentials = new NetworkCredential(userName, password);
                                                        smtp.EnableSsl = Ssl;
                                                        smtp.Send(message);////
                                                        checkmailtriger.IsMailTriggered = true;
                                                        _context.Entry(checkmailtriger).State = EntityState.Modified;
                                                        ////
                                                        Log addlog = new Log();
                                                        addlog.Module = "Completed Job";
                                                        addlog.Description = "Email sent successfully. To: " + message.To + ", CC : " + message.CC + ",errorDesc :" + errorDesc + "temp : " + checkcustomerforbunchmail.BunchMail + ":" + job.ClientId;
                                                        addlog.ClientId = getclientorderdetailsJO.ClientId;//
                                                        addlog.JobId = job.JId;
                                                        //addlog.OrderId = getclientorderdetails.OrderId;
                                                        addlog.OrderId = getclientorderdetailsJO.ClientOrderId;//
                                                        addlog.Type = "Success";
                                                        addlog.CreatedUtc = DateTime.UtcNow;
                                                        _context.Logs.Add(addlog);
                                                        ////
                                                        _context.SaveChanges();
                                                        transaction1.Commit();
                                                    }
                                                }
                                                ////
                                            }
                                        }
                                        else if ((jobStatusDescription.JobStatusDescription == "Revision") && (pwtDetails == null))//.
                                        {
                                            var AttachCount = message.Attachments.Count;

                                            body = "Hi " + emailIdAndName.ContactName + ",<br/><br/>" +
                                            "This e-mail is to keep you informed that the order <b>" + jobOrderDetails.FileName + "</b> is revised and uploaded through VIS. Please download it from the portal or from this link <a href='" + ftpUrl + "api/Allocation/DownloadZipFile?path=" + pwtRecord.FileUploadPath + "'>Click here to Download.</a> Review and let us know your feedback.<br/><br/>" +
                                            "If you have any queries regarding this order, please send an e-mail to support@vleadservices.com or call us at (248) 525 6936/(214) 347-9935.<br/><br/>";

                                            body1 = "<b>Note:</b> Completed file size is more than 20 MB.  So, we unable to proceed with the attachments.  Please click the hyperlink to download the files.";

                                            body2 = "<b>Note:</b> Please DO NOT respond to this e-mail or use this e-mail address since it is not monitored.<br/><br/>" +
                                                    "Thank you for your interest with VLead Services.<br/><br/>" +
                                                    "Many thanks for your business.<br/>" +
                                                    "Support Team,<br/>" +
                                                    "VLead Design Services.";
                                            message.Subject = "Order Revised By VLead  - (" + jobOrderDetails.FileName + ")";

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

                                            message.IsBodyHtml = true;
                                            var host = _configuration.GetSection("Smtp").GetValue<string>("host");
                                            var port = _configuration.GetSection("Smtp").GetValue<int>("port");
                                            var userName = _configuration.GetSection("Smtp").GetValue<string>("userName");
                                            var password = _configuration.GetSection("Smtp").GetValue<string>("password");
                                            var Ssl = _configuration.GetSection("Smtp").GetValue<bool>("SSL");
                                            var defaultCredential = _configuration.GetSection("Smtp").GetValue<bool>("defaultCredential");
                                            using (var smtp = new SmtpClient(host, port))
                                            {
                                                //smtp.Send(message);////
                                                ////
                                                using (var mail = new ApplicationDbContext(_configuration))
                                                {
                                                    var checkmailtriger = _context.JobOrders.FirstOrDefault(x => x.Id == job.JId);
                                                    if (checkmailtriger.IsMailTriggered == false || checkmailtriger.IsMailTriggered == null)
                                                    {
                                                        smtp.UseDefaultCredentials = defaultCredential;
                                                        smtp.Credentials = new NetworkCredential(userName, password);
                                                        smtp.EnableSsl = Ssl;
                                                        smtp.Send(message);////
                                                        checkmailtriger.IsMailTriggered = true;
                                                        mail.Entry(checkmailtriger).State = EntityState.Modified;
                                                        ////
                                                        Log addlog = new Log();
                                                        addlog.Module = "Revised Job";
                                                        addlog.JobId = job.JId;
                                                        addlog.Description = "Email sent successfully" + ",errorDesc :" + errorDesc;
                                                        addlog.ClientId = getclientorderdetailsJO.ClientId;//JO
                                                        addlog.Type = "Success";
                                                        addlog.CreatedUtc = DateTime.UtcNow;
                                                        mail.Logs.Add(addlog);
                                                        ////
                                                        mail.SaveChanges();

                                                    }
                                                }
                                                ////
                                            }
                                        }
                                        else if ((jobStatusDescription.JobStatusDescription == "Quotation") && (pwtDetails == null))//.
                                        {
                                            var AttachCount = message.Attachments.Count;

                                            body = "Hi " + emailIdAndName.ContactName + ",<br/><br/>" +
                                            //"We have completed your quote request for <b>" + getclientorderdetails.FileName + "</b> and the details are sent to you through your VIS account." +
                                            "We have completed your quote request for <b>" + getclientorderdetailsJO.FileName + "</b> and the details are sent to you through your VIS account." +//
                                            "If you agree the pricing, please approve the order and send it to us as new order.<br/><br/>" +
                                            "If you have any queries regarding this order, please send an e-mail to support@vleadservices.com or call us at (248) 525 6936/(214) 347-9935.<br/><br/>";

                                            body1 = "<b>Note:</b> Completed file size is more than 20 MB.  So, we unable to proceed with the attachments.  Please click the hyperlink to download the files.";

                                            body2 = "<b>Note:</b> Please DO NOT respond to this e-mail or use this e-mail address since it is not monitored.<br/><br/>" +
                                                    "Thank you for your interest with VLead Services.<br/><br/>" +
                                                    "Many thanks for your business.<br/>" +
                                                    "Support Team,<br/>" +
                                                    "VLead Design Services.";
                                            //message.Subject = "Quote Request Completed - (" + getclientorderdetails.FileName + ")";
                                            message.Subject = "Quote Request Completed - (" + getclientorderdetailsJO.FileName + ")";//JO                                            
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

                                            message.IsBodyHtml = true;
                                            var host = _configuration.GetSection("Smtp").GetValue<string>("host");
                                            var port = _configuration.GetSection("Smtp").GetValue<int>("port");
                                            var userName = _configuration.GetSection("Smtp").GetValue<string>("userName");
                                            var password = _configuration.GetSection("Smtp").GetValue<string>("password");
                                            var Ssl = _configuration.GetSection("Smtp").GetValue<bool>("SSL");
                                            var defaultCredential = _configuration.GetSection("Smtp").GetValue<bool>("defaultCredential");
                                            using (var smtp = new SmtpClient(host, port))
                                            {
                                                //smtp.Send(message);////
                                                ////
                                                using (var transaction1 = _context.Database.BeginTransaction())
                                                {
                                                    var checkmailtriger = _context.JobOrders.FirstOrDefault(x => x.Id == job.JId);
                                                    if (checkmailtriger.IsMailTriggered == false || checkmailtriger.IsMailTriggered == null)
                                                    {
                                                        smtp.UseDefaultCredentials = defaultCredential;
                                                        smtp.Credentials = new NetworkCredential(userName, password);
                                                        smtp.EnableSsl = Ssl;
                                                        smtp.Send(message);////
                                                        checkmailtriger.IsMailTriggered = true;
                                                        _context.Entry(checkmailtriger).State = EntityState.Modified;
                                                        ////
                                                        Log addlog = new Log();
                                                        addlog.Module = "Quote Job";
                                                        addlog.JobId = job.JId;
                                                        addlog.Description = "Email sent successfully" + ",errorDesc :" + errorDesc;
                                                        addlog.ClientId = getclientorderdetailsJO.ClientId;//JO
                                                        addlog.Type = "Success";
                                                        addlog.CreatedUtc = DateTime.UtcNow;
                                                        _context.Logs.Add(addlog);
                                                        ////
                                                        _context.SaveChanges();
                                                        transaction1.Commit();
                                                    }
                                                }
                                                ////
                                            }
                                        }
                                        errorDesc = errorDesc + "," + "Step-7";
                                    } // else end
                                }//if ends

                                else if (processMovement.AutoUploadJobs == false && checkcustomerforbunchmail.BunchMail == false && (checkCCEmailIdinJO.ClientSalesPerson != null || checkCCEmailIdinJO.CcemailId != null))//else if start new added
                                {
                                    //calling other api from one api
                                    var url = _configuration.GetSection("AllocationService").GetValue<string>("BunchAPIMail");
                                    var request = (HttpWebRequest)WebRequest.Create(url + "api/BulkUploadMail/BulkUpload?JobId=" + job.JId);
                                    var postData = "JobId=" + job.JId;
                                    var data = Encoding.ASCII.GetBytes(postData);
                                    request.Method = "POST";
                                    request.ContentType = "application/x-www-form-urlencoded";
                                    //attaching file while calling api
                                    //request.Headers.Add("Content-Disposition", "attachment;filename=\"" + fileInfo.Name + "\"");
                                    //request.Headers.Add("Content-Length", fileInfo.Length.ToString());
                                    //request.ContentType = "application/octet-stream";
                                    request.ContentLength = data.Length;
                                    using (var stream = request.GetRequestStream())
                                    {
                                        stream.Write(data, 0, data.Length);
                                    }
                                    var response = (HttpWebResponse)request.GetResponse();
                                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                                    //
                                }
                            }
                            if ((job.TranId) != 0)
                            {
                                //  var chkTimeStamp = _context.ProcessWorkFlowTrans.AsNoTracking().FirstOrDefault(x => x.Id == job.TranId);
                                var chkTimeStamp = _context.ProcessWorkFlowTrans.AsNoTracking().FirstOrDefault(x => x.Id == job.TranId);
                                //var job1 = _tableWork.ProcessWorkFlowTranRepository.GetLastRecord(s => s.Id);
                                if (!job.TimeStamp.SequenceEqual(chkTimeStamp.TimeStamp))
                                {
                                    result = new
                                    {
                                        Success = false,
                                        Message = "Job(s) moved to Next Process, Try another Jobs",
                                        OrderId = 0,
                                        WFTId = 0,
                                        Wfmid = 0,
                                        ProcessId = 0
                                    };
                                    if (processMovement.AutoUploadJobs == true && result.Message == "Job(s) moved to Next Process, Try another Jobs")
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        return result;
                                    }
                                }
                            }
                            if (result.Message == "Job Uploaded to Client!")
                            {
                                var JobCheck = _context.JobOrders.Where(x => x.Id == job.JId).Select(x => new { JobId = x.Id, IsUpload = x.DateofUpload }).FirstOrDefault();
                                if (JobCheck.IsUpload == null)
                                {
                                    var UpdateIsActiveRecords = _context.ProcessWorkFlowTrans.Where(x => x.Jid == JobCheck.JobId && x.IsActive == true && x.StatusId != 12).ToList();
                                    UpdateIsActiveRecords.ForEach(a =>
                                    {
                                        a.IsActive = false;
                                        a.UpdatedBy = processMovement.EmployeeId;
                                        a.UpdatedUtc = DateTime.UtcNow;
                                    });
                                }
                            }
                            if (processMovement.StatusId != 27)
                            {
                                _context.SaveChanges();
                            }
                            //started
                            if (result.Message == "Job(s) Assigned")
                            {
                                ifmes = "Job(s) Assigned";
                                wftid = insertPWT.Id;
                                result = new
                                {
                                    Success = true,
                                    Message = "Job(s) Assigned",
                                    OrderId = 0,
                                    WFTId = insertPWT.Id,
                                    Wfmid = job.TranMasterId,
                                    ProcessId = 9
                                };
                            }
                            //transaction.Commit();
                        }
                    }
                    else if (ScopeExists == false)
                    {
                        var clientname = _unitWork.Customer.FirstOrDefault(x => x.Id == job.CustomerId).ShortName;
                        var cnamelocal = _unitWork.Customer.Local().ToList();
                        value = string.Join(", ", from item in cnamelocal select item.ShortName);
                        elsemes = "Job(s) NotAssigned";

                        //var insertPWT = new ProcessWorkFlowTran();
                        if (ifmes == "Job(s) Assigned" && elsemes == "Job(s) NotAssigned")
                        {
                            elseifmes = "Job(s) SomeJobs";
                            result = new
                            {
                                Success = true,
                                Message = "Job(s) not assigned for customer(s): " + "" + "'" + value + "' due to scope not mapped",
                                OrderId = 0,
                                WFTId = wftid,
                                Wfmid = job.TranMasterId,
                                ProcessId = 9
                            };
                        }
                        else
                        {
                            if (elseifmes == "Job(s) SomeJobs" && elsemes == "Job(s) NotAssigned")
                            {
                                result = new
                                {
                                    Success = true,
                                    Message = "Job(s) not assigned for customer(s): " + "" + "'" + value + "' due to scope not mapped",
                                    OrderId = 0,
                                    WFTId = wftid,
                                    Wfmid = job.TranMasterId,
                                    ProcessId = 9
                                };
                            }
                            else
                            {
                                if (processMovement.ProcessId == 2)
                                {
                                    result = new
                                    {
                                        Success = false,
                                        Message = "Job(s) not assigned for customer(s): " + "" + "'" + value + "' due to scope not mapped",
                                        OrderId = 0,
                                        WFTId = 0,
                                        Wfmid = 0,
                                        ProcessId = 0
                                    };
                                }
                                else
                                {
                                    result = new
                                    {
                                        Success = false,
                                        Message = "Job not sent due to scope not mapped for customer: " + "" + "'" + value + "'",
                                        OrderId = 0,
                                        WFTId = 0,
                                        Wfmid = 0,
                                        ProcessId = 0
                                    };
                                }
                            }

                        }
                        continue;

                    }
                    if (ifmes == "Job(s) Assigned")
                    {
                        //var insertPWT = new ProcessWorkFlowTran();
                        if (ifmes == "Job(s) Assigned" && elsemes == "Job(s) NotAssigned")
                        {
                            elseifmes = "Job(s) SomeJobs";
                            result = new
                            {
                                Success = true,
                                Message = "Job(s) not assigned for customer(s): " + "" + "'" + value + "' due to scope not mapped",
                                OrderId = 0,
                                WFTId = wftid,
                                Wfmid = job.TranMasterId,
                                ProcessId = 9
                            };
                        }
                        else
                        {
                            if (elseifmes == "Job(s) SomeJobs" && elsemes == "Job(s) NotAssigned")
                            {
                                result = new
                                {
                                    Success = true,
                                    Message = "Job(s) not assigned for customer(s): " + "" + "'" + value + "' due to scope not mapped",
                                    OrderId = 0,
                                    WFTId = wftid,
                                    Wfmid = job.TranMasterId,
                                    ProcessId = 9
                                };
                            }
                            else
                            {
                                result = new
                                {
                                    Success = true,
                                    Message = "Job(s) Assigned",
                                    OrderId = 0,
                                    WFTId = wftid,
                                    Wfmid = job.TranMasterId,
                                    ProcessId = 9
                                };
                            }
                        }


                    }
                }//
                catch (Exception ex)
                {
                    EXmessage = ex.Message + "****" + ex.StackTrace + ex.InnerException;

                    step += "step - 0+3";
                    var jobOrderInErr = _tableWork.JobOrderRepository.GetSingle(x => x.Id == job.JId);
                    var checkcustomer = _tableWork.CustomerRepository.GetSingle(x => x.Id == jobOrderInErr.ClientId && x.BunchMail == true);
                    //&& checkcustomer == null
                    if (processMovement.AutoUploadJobs == true && checkcustomer == null)
                    {
                        using (var transaction1 = new ApplicationDbContext(_configuration))
                        {
                            if (jobOrderInErr.IsAutoUploadCount == null)
                            {
                                jobOrderInErr.IsAutoUploadCount = 1;
                            }
                            else
                            {
                                jobOrderInErr.IsAutoUploadCount += 1;
                            }
                            jobOrderInErr.UpdatedUtc = DateTime.UtcNow;
                            transaction1.Entry(jobOrderInErr).State = EntityState.Modified;
                            transaction1.SaveChanges();
                        }
                    }

                    jobFileName = jobFileName + ", " + jobOrderInErr.FileName;
                    using (var transaction1 = new ApplicationDbContext(_configuration))
                    {
                        Log addlog = new Log();
                        addlog.Module = "Alloc Rep" + processMovement.ProcessId;
                        addlog.Description = "Error Msg: " + ex.Message + ", Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source + "sTEPS:" + step;
                        addlog.ClientId = job.CustomerId;
                        addlog.JobId = job.JId;
                        addlog.Type = "Error Occ";
                        addlog.CreatedUtc = DateTime.UtcNow;
                        _context.Logs.Add(addlog);
                        transaction1.SaveChanges();
                    }
                    result = new
                    {
                        Success = false,
                        Message = "Error Occured in :" + jobFileName + EXmessage,
                        OrderId = 0,
                        WFTId = 0,
                        Wfmid = 0,
                        ProcessId = 0
                    };
                }
                finally
                {
                    if (jobFileName != "")
                    {
                        result = new
                        {
                            Success = false,
                            Message = "Error Occured in File Name(s) :" + jobFileName + EXmessage,
                            OrderId = 0,
                            WFTId = 0,
                            Wfmid = 0,
                            ProcessId = 0
                        };
                    }
                }
            }
            //the above line is one foreach end
            return result;
        }


        public IEnumerable<JobOrder> ScheduledMailSend(int OrderId)
        {
            var CompressedMailForNewJob = new List<JobOrder>();
            var host = _configuration.GetSection("Smtp").GetValue<string>("host");
            var port = _configuration.GetSection("Smtp").GetValue<int>("port");
            var userName = _configuration.GetSection("Smtp").GetValue<string>("userName");
            var password = _configuration.GetSection("Smtp").GetValue<string>("password");
            var Ssl = _configuration.GetSection("Smtp").GetValue<bool>("SSL");
            var defaultCredential = _configuration.GetSection("Smtp").GetValue<bool>("defaultCredential");
            try
            {
                //Taken Bunch mail customer
                var Checkclientformail = _unitWork.Customer.Where(x => x.BunchMail == true && x.IsDeleted == false).Select(x => x.Id).ToList();
                if (Checkclientformail != null)
                {
                    //conforming mail for the client in clientorder table
                    CompressedMailForNewJob = _unitWork.JobOrder.Where(c => c.IsAutoUploadCount == 5 && c.DateofUpload != null && Checkclientformail.Contains(c.ClientId.Value) == true && c.IsDeleted == false).OrderByDescending(c => c.Ccid).ToList();
                    //shortlisted salesperson 
                    var shortlistedsalesperson = CompressedMailForNewJob.GroupBy(x => x.Ccid).ToList();
                    foreach (var item in shortlistedsalesperson)
                    {
                        var countoffile = _unitWork.JobOrder.Where(x => x.IsAutoUploadCount == 5 && x.DateofUpload != null && x.Ccid == item.Key && x.IsDeleted == false).ToList();
                        int mjid = item.FirstOrDefault().Id;////
                        var message = new MailMessage();
                        if (countoffile == null)
                        {
                            continue;
                        }
                        else
                        {
                            int revision = countoffile.Where(c => c.JobStatusId == 1 || c.JobStatusId == 2 || c.JobStatusId == 5).Count();
                            int fresh = countoffile.Where(c => c.JobStatusId == 7 || c.JobStatusId == 8 || c.JobStatusId == 9).Count();
                            int Quotation = countoffile.Where(c => c.JobStatusId == 10).Count();
                            var Firstmail = countoffile.FirstOrDefault();
                            var emailIdAndName = _unitWork.CustomerContact.Where(x => x.CustomerId == Firstmail.ClientId && x.ContactName == Firstmail.SalesPersonName).FirstOrDefault();
                            message.To.Add(new MailAddress(emailIdAndName.Email));
                            var mailNotify = _unitWork.MailNotificationDetail.Where(x => x.CompareMailId == emailIdAndName.Id && x.IsDeleted == false && (x.Task == "All" || x.Task == "New") && x.ClientId == Firstmail.ClientId).ToList();
                            if (mailNotify.Count() >= 1)
                            {
                                foreach (var item2 in mailNotify)
                                {
                                    var customerContactDetails = _unitWork.CustomerContact.Where(x => x.Id == item2.CcmailId && x.IsDeleted == false).FirstOrDefault();
                                    if (customerContactDetails != null)
                                    {
                                        message.CC.Add(new MailAddress(customerContactDetails.Email));
                                    }
                                }
                            }
                            message.From = new MailAddress("visalert@vleadservices.com");
                            if (fresh != 0)
                            {
                                var body = "Hi " + emailIdAndName.ContactName + ",<br/><br/>" +
                                "This e-mail is to keep you informed that the   <b>" + fresh + "</b> order(s) is completed and uploaded through VIS. Review and let us know your feedback." +
                                "If the order needs to be revised, you may send us revisions through VIS itself.<br/><br/>" +
                                "If you have any queries regarding this order, please send an e-mail to support@vleadservices.com or call us @ (469) 910-0091 / (714) 257-5484.<br/><br/>" +
                                "<b>Note:</b> Completed file size is more than 20 MB.  So, we unable to proceed with the attachments.  Please click the hyperlink to download the files." +
                                "<b>Note:</b> Please DO NOT respond to this e-mail or use this e-mail address since it is not monitored.<br/><br/>" +
                                "Thank you for your interest with VLead Services.<br/><br/>" +
                                "Many thanks for your business.<br/>" +
                                "Support Team,<br/>" +
                                "VLead Design Services.";

                                message.Subject = "Order Completed By VLead ";

                                //message.CC.Add(new MailAddress("support@vleadservices.com"));
                                message.Body = string.Format(body, "Venu", "Test");
                                message.IsBodyHtml = true;
                                using (var smtp = new SmtpClient(host, port))
                                {
                                    //smtp.Send(message);////
                                    ////
                                    using(var transaction1 = _context.Database.BeginTransaction())
                                    {

                                  // ApplicationDbContext _context = new ApplicationDbContext(_configuration);
                                    var checkmailtriger = _context.JobOrders.FirstOrDefault(x => x.Id == mjid);
                                    if (checkmailtriger.IsMailTriggered == false || checkmailtriger.IsMailTriggered == null)
                                    {
                                        smtp.UseDefaultCredentials = defaultCredential;
                                        smtp.Credentials = new NetworkCredential(userName, password);
                                        smtp.EnableSsl = Ssl;
                                        smtp.Send(message);////
                                        checkmailtriger.IsMailTriggered = true;
                                       // _context.Entry(checkmailtriger).State = EntityState.Modified;
                                        _context.SaveChanges();
                                           
                                    }
                                        transaction1.Commit();
                                    }
                                    ////
                                }
                            }
                            if (revision != 0)
                            {
                                var body = "Hi " + emailIdAndName.ContactName + ",<br/><br/>" +
                                "This e-mail is to keep you informed that the <b>" + revision + "</b> Order(s) is revised and uploaded through VIS. Please download it from the portal. Review and let us know your feedback.<br/><br/>" +
                                "If you have any queries regarding this order, please send an e-mail to support@vleadservices.com or call us at (248) 525 6936/(214) 347-9935.<br/><br/>" +
                                "<b>Note:</b> Please DO NOT respond to this e-mail or use this e-mail address since it is not monitored.<br/><br/>" +
                                "Thank you for your interest with VLead Services.<br/><br/>" +
                                "Many thanks for your business.<br/>" +
                                "Support Team,<br/>" +
                                "VLead Design Services.";
                                message.Subject = "Order Revised By VLead";
                                message.Body = string.Format(body, "Venu", "Test");
                                message.IsBodyHtml = true;
                                using (var smtp = new SmtpClient(host, port))
                                {
                                    //smtp.Send(message);////
                                    ////
                                    using(var transaction1 = _context.Database.BeginTransaction())
                                    {

                                  //  ApplicationDbContext _context = new ApplicationDbContext(_configuration);
                                    var checkmailtriger = _context.JobOrders.FirstOrDefault(x => x.Id == mjid);
                                    if (checkmailtriger.IsMailTriggered == false || checkmailtriger.IsMailTriggered == null)
                                    {
                                        smtp.UseDefaultCredentials = defaultCredential;
                                        smtp.Credentials = new NetworkCredential(userName, password);
                                        smtp.EnableSsl = Ssl;
                                        smtp.Send(message);////
                                        checkmailtriger.IsMailTriggered = true;
                                        //_context.Entry(checkmailtriger).State = EntityState.Modified;
                                        _context.SaveChanges();
                                    }
                                            transaction1.Commit();
                                    }
                                    ////
                                }
                            }
                            if (Quotation != 0)
                            {
                                var body = "Hi " + emailIdAndName.ContactName + ",<br/><br/>" +
                                "We have completed your quote request for <b>" + Quotation + "</b> Order(s) and the details are sent to you through your VIS account." +
                                "If you agree the pricing, please approve the order and send it to us as new order.<br/><br/>" +
                                "If you have any queries regarding this order, please send an e-mail to support@vleadservices.com or call us at (248) 525 6936/(214) 347-9935.<br/><br/>" +
                                "<b>Note:</b> Please DO NOT respond to this e-mail or use this e-mail address since it is not monitored.<br/><br/>" +
                                "Thank you for your interest with VLead Services.<br/><br/>" +
                                "Many thanks for your business.<br/>" +
                                "Support Team,<br/>" +
                                "VLead Design Services.";
                                message.Subject = "Quote Request Completed ";
                                message.Body = string.Format(body, "Venu", "Test");
                                message.IsBodyHtml = true;
                                using (var smtp = new SmtpClient(host, port))
                                {
                                    //smtp.Send(message);////
                                    ////
                                    using(var transaction1 = _context.Database.BeginTransaction())
                                    {

                                   // ApplicationDbContext _context = new ApplicationDbContext(_configuration);
                                    var checkmailtriger = _context.JobOrders.FirstOrDefault(x => x.Id == mjid);
                                    if (checkmailtriger.IsMailTriggered == false || checkmailtriger.IsMailTriggered == null)
                                    {
                                        smtp.UseDefaultCredentials = defaultCredential;
                                        smtp.Credentials = new NetworkCredential(userName, password);
                                        smtp.EnableSsl = Ssl;
                                        smtp.Send(message);////
                                        checkmailtriger.IsMailTriggered = true;
                                       // _context.Entry(checkmailtriger).State = EntityState.Modified;
                                        _context.SaveChanges();
                                    }
                                            transaction1.Commit();
                                    }
                                    ////
                                }
                            }
                            Log savelog = new Log();
                            savelog.Module = "Scheduled Mail Send For Multiple";
                            savelog.Description = "Email sent successfully. To: " + message.To + ", CC : " + message.CC;
                            savelog.ClientId = Firstmail.ClientId;
                            savelog.OrderId = Firstmail.Id;
                            savelog.JobId = mjid;
                            savelog.Type = "Success";
                            savelog.CreatedUtc = DateTime.UtcNow;
                            _unitWork.Log.Add(savelog);
                            _unitWork.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log savelog = new Log();
                savelog.Module = "Scheduled Mail Send";
                savelog.Description = "Exception Message:" + ex.Message + "Inner Exception" + ex.InnerException + "Data" + ex.Data;
                savelog.ClientId = null;
                savelog.OrderId = null;
                savelog.Type = "Error";
                savelog.CreatedUtc = DateTime.UtcNow;
                _unitWork.Log.Add(savelog);
                _unitWork.SaveChanges();
            }
            return CompressedMailForNewJob;
        }
        public void FTPFileUpload(bool IsFilesInLocalServer, string FileName, string FileUploadPath)
        {
            string ftpUrl, ftpUsername, ftpPassword, fileToUploadPath, jobFilesPath;
            if (!IsFilesInLocalServer)
            {
                ftpUrl = _configuration.GetSection("AllocationService").GetValue<string>("ftpLocalServerUrl");
                ftpUsername = _configuration.GetSection("AllocationService").GetValue<string>("ftpLocalServerUsername");
                ftpPassword = _configuration.GetSection("AllocationService").GetValue<string>("ftpLocalServerPassword");
                jobFilesPath = _configuration.GetSection("AllocationService").GetValue<string>("jobFilesClientPath");
            }
            else
            {
                ftpUrl = _configuration.GetSection("AllocationService").GetValue<string>("ftpClientServerUrl");
                ftpUsername = _configuration.GetSection("AllocationService").GetValue<string>("ftpClientServerUsername");
                ftpPassword = _configuration.GetSection("AllocationService").GetValue<string>("ftpClientServerPassword");
                jobFilesPath = _configuration.GetSection("AllocationService").GetValue<string>("jobFilesPath");
            }
            var credentials = new NetworkCredential(ftpUsername, ftpPassword);
            bool isFolderAvailable = false;
            bool isFileAvailable = false;
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
                var folderPath = jobFilesPath + FileUploadPath;
                string filename = folderPath + "\\" + fileName;
                client.UploadFile(address, filename);
            }
        }


        public Object GetPendingJobsAndEmployees(int EmployeeId, int ProcessId, int IsPendingJob, int DepartmentId)
        {
            var employees = new Object();
            var AllocationJobs = new List<ViewJobAllocation>();
            var allocation = new List<JobAllocation_Result>();
            int deptid = 0; //edited
            try
            {
                if (ProcessId != 6)
                {
                    var employee = _tableWork.EmployeeRepository.Get(x => x.EmployeeId == EmployeeId && x.DateOfResignation == null && x.IsDeleted == false).FirstOrDefault();
                    employees = getEmployees(ProcessId, employee.DepartmentId);
                    var sqlPara = new SqlParameter[]
                    {
                        new SqlParameter("@EmployeeId",EmployeeId), // added
                        new SqlParameter("@IsPendingJob",IsPendingJob),
                        new SqlParameter("@ProcessId",ProcessId),
                        new SqlParameter("@DepartmentId",deptid) //edited - employee.DepartmentId
                    };
                    allocation = _procedureWork.ExecStoredProcedure<JobAllocation_Result>("JobAllocation @EmployeeId,@IsPendingJob, @ProcessId, @DepartmentId", sqlPara).ToList();
                }
                else
                {
                    employees = getEmployees(ProcessId, 8);
                    var sqlPara1 = new SqlParameter[]
                    {
                        new SqlParameter("@EmployeeId",EmployeeId),// added
                        new SqlParameter("@IsPendingJob",IsPendingJob),
                        new SqlParameter("@ProcessId",ProcessId),
                        new SqlParameter("@DepartmentId",deptid) //edited - employee.DepartmentId
                    };
                    allocation = _procedureWork.ExecStoredProcedure<JobAllocation_Result>("JobAllocation @EmployeeId,@IsPendingJob, @ProcessId, @DepartmentId", sqlPara1).ToList();
                }
            }
            catch (Exception ex)
            {
               // using (ApplicationDbContext _context = new ApplicationDbContext(_configuration))
               // {
                    Log addlog = new Log();
                    addlog.Module = "Get Pending jobs and emps";
                    addlog.Description = "Error Msg in alloc trays :  " + ex.Message.ToString() + "-" + "Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source;
                    addlog.Type = "Error Occured in alloc trays";
                    addlog.CreatedUtc = DateTime.UtcNow;
                    _context.Logs.Add(addlog);
                    _context.SaveChanges();
               // }
            }
            var result = new
            {
                AllocationJobs = allocation,
                Employees = employees
            };
            return result;
        }
        public IEnumerable<GetEmployee> getEmployees(int ProcessId, int DepartmentId)
        //public Object getEmployees(int ProcessId, int DepartmentId)
        {
            var employeeProcesses = new List<EmployeeProcess>();
            if (ProcessId == 6)
            {
                employeeProcesses = _tableWork.EmployeeProcessRepository.GetAllVal(x => x.Employee).Where(x => (x.ProcessId == (ProcessId + 1) && x.IsDeleted == false) && (x.Employee.IsDeleted == false && x.Employee.DateOfResignation == null)).OrderBy(x => x.Employee.EmployeeName).ToList();
            }
            else
            {
                employeeProcesses = _tableWork.EmployeeProcessRepository.GetAllVal(x => x.Employee).Where(x => (x.ProcessId == (ProcessId + 1) && x.IsDeleted == false) && (x.Employee.IsDeleted == false && x.Employee.DateOfResignation == null)).OrderBy(x => x.Employee.EmployeeName).ToList(); //&& x.Employee.DepartmentId == DepartmentId
            }
            var employees = new List<GetEmployee>();
            foreach (var item in employeeProcesses)
            {
                string employeeNameWithCode = item.Employee.EmployeeCode + " - " + item.Employee.EmployeeName;
                var shiftDetails = _tableWork.ShiftVsEmployeeRepository.GetAllVal(x => x.Shift).FirstOrDefault(x => x.EmployeeId == item.Employee.EmployeeId && x.IsDeleted == false);
                var shift = "";
                if (shiftDetails != null)
                {
                    shift = shiftDetails.Shift.Description;
                }
                employees.Add(new GetEmployee
                {
                    EmployeeId = item.Employee.EmployeeId,
                    EmployeeName = item.Employee.EmployeeName,
                    EmployeeCode = item.Employee.EmployeeCode,
                    EmployeenameWithCode = employeeNameWithCode,
                    ShiftName = shift
                });
            }
            return employees;
        }
        public Object GetCount(int ProcessId, int EmployeeId, int DepartmentId)
        {
            var result = new Object();
            try
            {
                var AllocationJobs = new List<ViewJobAllocation>();
                var employee = _tableWork.EmployeeRepository.Get(x => x.EmployeeId == EmployeeId && x.DateOfResignation == null && x.IsDeleted == false).FirstOrDefault();
                var pendingJob = new List<ViewJobAllocation>();
                var allocatedJob = new List<ViewJobAllocation>();
                if (DepartmentId != 0)
                {
                    //var jobAllocationDetails = _viewWork.ViewJobAllocationRepository.Get(x => (x.ProcessId == ProcessId) && (x.DepartmentId == DepartmentId) &&
                    //                            (x.IsActive == true) && (x.StatusId == 1 || x.StatusId == 3 || x.StatusId == 5 || x.StatusId == 10 || x.StatusId == 11 || x.StatusId == 19
                    //                            || x.StatusId == 7 || x.StatusId == 9 || x.StatusId == 6 || x.StatusId == 8 || x.StatusId == 13 || x.StatusId == 14 || x.StatusId == 15 || x.StatusId == 16) && x.IsDeleted == false && x.DivEmpId == EmployeeId).ToList(); // added - EmployeeId
                    var jobAllocationDetails = _unitviewWork.ViewJobAllocation.Where(x => (x.ProcessId == ProcessId) && (x.DepartmentId == DepartmentId) &&
                                                (x.IsActive == true) && (x.StatusId == 1 || x.StatusId == 3 || x.StatusId == 5 || x.StatusId == 10 || x.StatusId == 11 || x.StatusId == 26 || x.StatusId == 19
                                                || x.StatusId == 7 || x.StatusId == 9 || x.StatusId == 6 || x.StatusId == 8 || x.StatusId == 13 || x.StatusId == 14 || x.StatusId == 15 || x.StatusId == 16) && x.IsDeleted == false && x.DivEmpId == EmployeeId).ToList(); // added - EmployeeId //added - StatusId == 26

                    pendingJob = jobAllocationDetails.Where(x => x.EmployeeCount == null).ToList();
                    allocatedJob = jobAllocationDetails.Where(x => x.EmployeeCount != null).ToList();
                }
                else
                {
                    //var jobAllocationDetails = _viewWork.ViewJobAllocationRepository.Get(x => (x.ProcessId == ProcessId) &&
                    //                            (x.IsActive == true) && (x.StatusId == 1 || x.StatusId == 3 || x.StatusId == 5 || x.StatusId == 10 || x.StatusId == 11 || x.StatusId == 19
                    //                            || x.StatusId == 7 || x.StatusId == 9 || x.StatusId == 6 || x.StatusId == 8 || x.StatusId == 13 || x.StatusId == 14 || x.StatusId == 15 || x.StatusId == 16) && x.IsDeleted == false && x.DivEmpId == EmployeeId).ToList(); // added - EmployeeId
                    var jobAllocationDetails = _unitviewWork.ViewJobAllocation.Where(x => (x.ProcessId == ProcessId) &&
                                                (x.IsActive == true) && (x.StatusId == 1 || x.StatusId == 3 || x.StatusId == 5 || x.StatusId == 10 || x.StatusId == 11 || x.StatusId == 26 || x.StatusId == 19
                                                || x.StatusId == 7 || x.StatusId == 9 || x.StatusId == 6 || x.StatusId == 8 || x.StatusId == 13 || x.StatusId == 14 || x.StatusId == 15 || x.StatusId == 16) && x.IsDeleted == false && x.DivEmpId == EmployeeId).ToList(); // added - EmployeeId // added - StatusId == 26

                    pendingJob = jobAllocationDetails.Where(x => x.EmployeeCount == null).ToList();
                    allocatedJob = jobAllocationDetails.Where(x => x.EmployeeCount != null).ToList();
                }
                //var queryandresponse = _viewWork.ViewJobQueryInCCRepository.Get(x => x.IsActive == true && x.IsDeleted == false && x.DivEmpId == EmployeeId);
                var queryandresponse = _unitviewWork.ViewJobQueryInCC.Where(x => x.IsActive == true && x.IsDeleted == false && x.DivEmpId == EmployeeId);
                var processWorkFlowTrans = new List<ProcessWorkFlowTran>();
                int pendingJobsCount = 0, freshJobsCount = 0, revisionJobsCount = 0, reworkJobsCount = 0, allocatedJobCount = 0, errorJobsCount = 0, quotationJobsCount = 0, queryResponseJobsCount = 0, queriesJobsCount = 0;

                if (DepartmentId == 0 && ProcessId == 6)
                {
                    processWorkFlowTrans = _tableWork.ProcessWorkFlowTranRepository.Get(x => x.ProcessId == ProcessId && x.IsActive == true).ToList();
                    queryResponseJobsCount = queryandresponse.Count(x => x.ProcessId == ProcessId && (x.StatusId == 7 || x.StatusId == 9));
                    queriesJobsCount = queryandresponse.Count(x => x.ProcessId == ProcessId && (x.StatusId == 6 || x.StatusId == 8));
                    allocatedJobCount = allocatedJob.Count();
                }
                else if (ProcessId == 6)
                {
                    processWorkFlowTrans = _tableWork.ProcessWorkFlowTranRepository.Get(x => x.ProcessId == ProcessId && x.IsActive == true).ToList();
                    queryResponseJobsCount = queryandresponse.Count(x => x.ProcessId == ProcessId && (x.StatusId == 7 || x.StatusId == 9)); // && x.DepartmentId == DepartmentId
                    queriesJobsCount = queryandresponse.Count(x => x.ProcessId == ProcessId && (x.StatusId == 6 || x.StatusId == 8)); // && x.DepartmentId == DepartmentId
                    allocatedJobCount = allocatedJob.Count();
                }
                else
                {
                    processWorkFlowTrans = _tableWork.ProcessWorkFlowTranRepository.Get(x => x.ProcessId == ProcessId && x.IsActive == true).ToList(); // && x.DepartmentId == employee.DepartmentId
                    allocatedJobCount = allocatedJob.Count(); // .Where(x => x.DepartmentId == employee.DepartmentId)
                    queryResponseJobsCount = queryandresponse.Count(x => x.ProcessId == ProcessId && x.EmployeeCount == null && (x.StatusId == 7 || x.StatusId == 9)); // && x.DepartmentId == employee.DepartmentId
                    queriesJobsCount = queryandresponse.Count(x => x.ProcessId == ProcessId && x.EmployeeCount == null && (x.StatusId == 6 || x.StatusId == 8)); // && x.DepartmentId == employee.DepartmentId
                    pendingJob = pendingJob.ToList(); // .Where(x => x.DepartmentId == employee.DepartmentId)
                }
                freshJobsCount = pendingJob.Count(x => (x.StatusId == 1 || x.StatusId == 10 || x.StatusId == 26) && (x.JobStatusId == 7 || x.JobStatusId == 8 || x.JobStatusId == 9));
                revisionJobsCount = pendingJob.Count(x => (x.StatusId == 1 || x.StatusId == 10) && (x.JobStatusId == 1 || x.JobStatusId == 2 || x.JobStatusId == 5));
                reworkJobsCount = pendingJob.Count(x => (x.StatusId == 1 || x.StatusId == 10) && (x.JobStatusId == 3 || x.JobStatusId == 4 || x.JobStatusId == 6));
                errorJobsCount = pendingJob.Count(x => x.StatusId == 11); //added StatusId == 11
                quotationJobsCount = pendingJob.Count(x => (x.StatusId == 1 || x.StatusId == 10) && (x.JobStatusId == 10));

                result = new
                {
                    PendingJobsCount = pendingJobsCount,
                    FreshJobsCount = freshJobsCount,
                    RevisionJobsCount = revisionJobsCount,
                    ReworkJobsCount = reworkJobsCount,
                    AllocatedJobCount = allocatedJobCount,
                    QueriesJobsCount = queriesJobsCount,
                    QueryResponseJobsCount = queryResponseJobsCount,
                    ErrorJobsCount = errorJobsCount,
                    QuotationJobCount = quotationJobsCount,
                };
            }
            catch (Exception ex)
            {

               // using (ApplicationDbContext _context = new ApplicationDbContext(_configuration))
              //  {
                    Log addlog = new Log();
                    addlog.Module = "GetCount Function in Allocation Rep.";
                    addlog.Description = "Error Msg GetCount Function: " + ProcessId + "-" + EmployeeId + "-" + ex.Message.ToString() + "-" + "Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source + "string";
                    addlog.Type = "Error Occ";
                    addlog.CreatedUtc = DateTime.UtcNow;
                    _context.Logs.Add(addlog);
                    _context.SaveChanges();
               // }
            }
            return result;
        }

        public Object GetQueryResponseJobsAndEmployees(int EmployeeId, int ProcessId, int DepartmentId)
        {
            var employee = _tableWork.EmployeeRepository.Get(x => x.EmployeeId == EmployeeId && x.DateOfResignation == null).FirstOrDefault();
            var queryResponseJobs = new List<ViewJobQueryInCc>();
            if (ProcessId == 1 || (DepartmentId == 0 && ProcessId == 6))
            {
                //queryResponseJobs = _viewWork.ViewJobQueryInCCRepository.Get(x => x.ProcessId == ProcessId && (x.StatusId == 7 || x.StatusId == 9) && x.IsActive == true && x.EmployeeCount == null && x.IsDeleted == false && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).ToList();
                queryResponseJobs = _unitviewWork.ViewJobQueryInCC.Where(x => x.ProcessId == ProcessId && (x.StatusId == 7 || x.StatusId == 9) && x.IsActive == true && x.EmployeeCount == null && x.IsDeleted == false && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).ToList();
            }
            else if (ProcessId == 6)
            {
                //queryResponseJobs = _viewWork.ViewJobQueryInCCRepository.Get(x => x.ProcessId == ProcessId && (x.StatusId == 7 || x.StatusId == 9) && x.IsActive == true && x.EmployeeCount == null && x.IsDeleted == false && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).ToList(); // && x.DepartmentId == DepartmentId
                queryResponseJobs = _unitviewWork.ViewJobQueryInCC.Where(x => x.ProcessId == ProcessId && (x.StatusId == 7 || x.StatusId == 9) && x.IsActive == true && x.EmployeeCount == null && x.IsDeleted == false && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).ToList(); // && x.DepartmentId == DepartmentId
            }
            else
            {
                //queryResponseJobs = _viewWork.ViewJobQueryInCCRepository.Get(x => x.ProcessId == ProcessId && (x.StatusId == 7 || x.StatusId == 9) && x.IsActive == true && x.EmployeeCount == null && x.IsDeleted == false && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).ToList(); // && x.DepartmentId == employee.DepartmentId
                queryResponseJobs = _unitviewWork.ViewJobQueryInCC.Where(x => x.ProcessId == ProcessId && (x.StatusId == 7 || x.StatusId == 9) && x.IsActive == true && x.EmployeeCount == null && x.IsDeleted == false && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).ToList(); // && x.DepartmentId == employee.DepartmentId
            }
            var employees = getEmployees(ProcessId, employee.DepartmentId);
            var result = new
            {
                QueryResponseJobs = queryResponseJobs,
                Employees = employees
            };
            return result;
        }

        public IEnumerable<JobCategory> GetJobCategory()
        {
            return _tableWork.JobCategoryRepository.Get().AsQueryable();
        }
        public Object GetScopeValues(int EmployeeId)
        {

            //var departmentId = _tableWork.EmployeeRepository.Get(x => x.EmployeeId.Equals(EmployeeId)).Select(x => x.DepartmentId).FirstOrDefault();
            //var scopeValues = _tableWork.ScopeRepository.Get(x => x.DepartmentId.Equals(departmentId) && x.IsDeleted == false).AsQueryable(); // deprt wise scope loading //
            var scopeValues = _tableWork.ScopeRepository.Get(x => x.IsDeleted == false).AsQueryable(); // all scope without dept wise
            var scopeDetails = new List<DropdownValues>();
            foreach (var item in scopeValues)
            {
                scopeDetails.Add(new DropdownValues
                {
                    Id = item.Id,
                    Name = item.Description
                });
            }
            var result = new
            {
                ScopeDetails = scopeDetails
            };
            return result;
        }
        public Object GetWorkFlowList(int EmployeeId, int ProcessId, int IsPendingJob, int DepartmentId)
        {
            var result = new Object();
            try
            {
                if (ProcessId == 9 || ProcessId == 11)
                {
                    if (ProcessId == 9)
                    {
                        ProcessId = 8;
                    }
                    if (ProcessId == 11)
                    {
                        ProcessId = 10;
                    }
                    var allocation = new List<ViewJobAllocation>();
                    var departmentId = _tableWork.EmployeeRepository.GetSingle(x => x.EmployeeId == EmployeeId).DepartmentId;
                    ////
                    //var jid = _viewWork.ViewJobAllocationRepository.Get(x => x.ProcessId == (ProcessId + 1) && x.StatusId == 1 && x.IsActive == true && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).FirstOrDefault();//get jid here
                    //var status = _tableWork.EmployeeAssignRepository.Get(x => x.JId == jid.JId && x.ProcessId == (ProcessId + 1) && x.EmployeeId == EmployeeId && x.IsActive == true).OrderByDescending(x => x.Id).FirstOrDefault();//get top status here
                    ////
                    if (IsPendingJob == 1)
                    {
                        //allocation = _db.ViewJobAllocation.Where(x => x.ProcessId == ProcessId && x.DepartmentId == departmentId && (x.StatusId == 21 || x.StatusId == 10 || x.StatusId == 20) && x.IsActive == true && (x.JobStatusId == 7 || x.JobStatusId == 8 || x.JobStatusId == 9) && x.EmployeeCount == null).OrderByDescending(x => x.TranId).ToList();
                        allocation = _viewWork.ViewJobAllocationRepository.Get(x => x.ProcessId == ProcessId && x.IsActive == true && (x.JobStatusId == 7 || x.JobStatusId == 8 || x.JobStatusId == 9) && x.EmployeeCount == null && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).ToList(); //added- EmployeeId and removed- && x.DepartmentId == departmentId 
                    }
                    else if (IsPendingJob == 2)
                    {
                        //allocation = _db.ViewJobAllocation.Where(x => x.ProcessId == ProcessId && x.DepartmentId == departmentId && (x.StatusId == 21 || x.StatusId == 10 || x.StatusId == 20) && x.IsActive == true && (x.JobStatusId == 1 || x.JobStatusId == 2 || x.JobStatusId == 5)).OrderByDescending(x => x.TranId).ToList();
                        allocation = _viewWork.ViewJobAllocationRepository.Get(x => x.ProcessId == ProcessId && x.IsActive == true && (x.JobStatusId == 1 || x.JobStatusId == 2 || x.JobStatusId == 5) && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).ToList();
                    }
                    else if (IsPendingJob == 3)
                    {
                        //allocation = _db.ViewJobAllocation.Where(x => x.ProcessId == ProcessId && x.DepartmentId == departmentId && (x.StatusId == 21 || x.StatusId == 10 || x.StatusId == 20) && x.IsActive == true && (x.JobStatusId == 3 || x.JobStatusId == 4 || x.JobStatusId == 6)).OrderByDescending(x => x.TranId).ToList();
                        allocation = _viewWork.ViewJobAllocationRepository.Get(x => x.ProcessId == ProcessId && x.IsActive == true && (x.JobStatusId == 3 || x.JobStatusId == 4 || x.JobStatusId == 6) && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).ToList();
                    }
                    else if (IsPendingJob == 4)
                    {
                        //allocation = _db.ViewJobAllocation.Where(x => x.ProcessId == ProcessId && x.DepartmentId == departmentId && (x.StatusId == 21 || x.StatusId == 10 || x.StatusId == 20) && x.IsActive == true && (x.JobStatusId == 10)).OrderByDescending(x => x.TranId).ToList();
                        allocation = _viewWork.ViewJobAllocationRepository.Get(x => x.ProcessId == ProcessId && x.IsActive == true && (x.JobStatusId == 10) && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).ToList();
                    }
                    else if (IsPendingJob == 5)
                    {
                        ////allocation = _db.ViewJobAllocation.Where(x => x.ProcessId == (ProcessId + 1) && x.StatusId == 1 && x.IsActive == true && x.EmployeeId == EmployeeId).OrderByDescending(x => x.TranId).OrderByDescending(x => x.TranId).ToList();
                        //                        
                        //allocation = _viewWork.ViewJobAllocationRepository.Get(x => x.ProcessId == (ProcessId + 1) && x.StatusId == 1 && x.IsActive == true && x.DivEmpId == EmployeeId && x.WorkStatus == status.Status).OrderByDescending(x => x.TranId).ToList();
                        //
                        allocation = _viewWork.ViewJobAllocationRepository.Get(x => x.ProcessId == (ProcessId + 1) && x.StatusId == 1 && x.IsActive == true && x.DivEmpId == EmployeeId).OrderByDescending(x => x.TranId).ToList(); ////&& x.EmployeeId == EmployeeId
                    }

                    //var freshJobsCount = _db.ViewJobAllocation.Where(x => x.ProcessId == ProcessId && x.DepartmentId == departmentId && (x.StatusId == 21 || x.StatusId == 10 || x.StatusId == 20) && x.IsActive == true && (x.JobStatusId == 7 || x.JobStatusId == 8 || x.JobStatusId == 9) && x.EmployeeCount == null).OrderByDescending(x => x.TranId).ToList().Count();
                    //var revisionJobsCount = _db.ViewJobAllocation.Where(x => x.ProcessId == ProcessId && x.DepartmentId == departmentId && (x.StatusId == 21 || x.StatusId == 10 || x.StatusId == 20) && x.IsActive == true && (x.JobStatusId == 1 || x.JobStatusId == 2 || x.JobStatusId == 5)).OrderByDescending(x => x.TranId).ToList().Count();
                    //var reworkJobsCount = _db.ViewJobAllocation.Where(x => x.ProcessId == ProcessId && x.DepartmentId == departmentId && (x.StatusId == 21 || x.StatusId == 10 || x.StatusId == 20) && x.IsActive == true && (x.JobStatusId == 3 || x.JobStatusId == 4 || x.JobStatusId == 6)).OrderByDescending(x => x.TranId).ToList().Count();
                    //var quoteJobsCount = _db.ViewJobAllocation.Where(x => x.ProcessId == ProcessId && x.DepartmentId == departmentId && (x.StatusId == 21 || x.StatusId == 10 || x.StatusId == 20) && x.IsActive == true && (x.JobStatusId == 10)).OrderByDescending(x => x.TranId).OrderByDescending(x => x.TranId).ToList().Count();
                    //var sewOutJobsCount = _db.ViewJobAllocation.Where(x => x.ProcessId == (ProcessId + 1) && x.StatusId == 1 && x.IsActive == true && x.EmployeeId == EmployeeId).OrderByDescending(x => x.TranId).OrderByDescending(x => x.TranId).ToList().Count();

                    //var jobAllocation = _db.ViewJobAllocation.Where(x => x.ProcessId == ProcessId && x.DepartmentId == departmentId && x.IsActive == true && (x.StatusId == 21 || x.StatusId == 10 || x.StatusId == 20)).ToList();
                    var jobAllocation = _viewWork.ViewJobAllocationRepository.Get(x => x.ProcessId == ProcessId && x.IsActive == true && x.DivEmpId == EmployeeId).ToList(); // && x.DepartmentId == departmentId

                    var freshJobsCount = jobAllocation.Count(x => (x.JobStatusId == 7 || x.JobStatusId == 8 || x.JobStatusId == 9) && x.EmployeeCount == null);
                    var revisionJobsCount = jobAllocation.Count(x => (x.JobStatusId == 1 || x.JobStatusId == 2 || x.JobStatusId == 5));
                    var reworkJobsCount = jobAllocation.Count(x => (x.JobStatusId == 3 || x.JobStatusId == 4 || x.JobStatusId == 6));
                    var quoteJobsCount = jobAllocation.Count(x => (x.JobStatusId == 10));
                    var sewOutJobsCount = _viewWork.ViewJobAllocationRepository.Count(x => x.ProcessId == (ProcessId + 1) && x.StatusId == 1 && x.IsActive == true && x.DivEmpId == EmployeeId); //&& x.EmployeeId == EmployeeId
                    //
                    //var sewOutJobsCount = _viewWork.ViewJobAllocationRepository.Count(x => x.ProcessId == (ProcessId + 1) && x.StatusId == 1 && x.IsActive == true && x.DivEmpId == EmployeeId && x.WorkStatus == status.Status); //&& x.EmployeeId == EmployeeId
                    //
                    if (ProcessId == 8)
                    {
                        foreach (var item in allocation)
                        {
                            var processWorkflowTran = new ProcessWorkFlowTran();
                            processWorkflowTran = _tableWork.ProcessWorkFlowTranRepository.Get(x => x.ProcessId == 2 && x.PreviousProcessId == 1 && x.Wfmid == item.TranMasterId).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (processWorkflowTran == null)
                            {
                                processWorkflowTran = _tableWork.ProcessWorkFlowTranRepository.Get(x => x.ProcessId == 2 && x.PreviousProcessId == null && x.Wfmid == item.TranMasterId).OrderByDescending(x => x.Id).FirstOrDefault();
                            }
                            if (processWorkflowTran != null)
                            {
                                item.EmployeeCount = processWorkflowTran.EmployeeCount;
                                item.EmployeeName = processWorkflowTran.EmployeeName;
                            }
                        }
                    }
                    result = new
                    {
                        GetWorkflowDetails = allocation,
                        FreshJobsCount = freshJobsCount,
                        RevisionJobsCount = revisionJobsCount,
                        ReworkJobsCount = reworkJobsCount,
                        QuoteJobsCount = quoteJobsCount,
                        SewOutJobsCount = sewOutJobsCount
                    };

                }
                else
                {
                    var getWorkflowDetails = new List<ViewGetWorkflowListforLogin>();
                    if (IsPendingJob == 1)
                    {
                        if (ProcessId == 7 && DepartmentId != 0)
                        {
                            getWorkflowDetails = _viewWork.ViewGetWorkflowListforLoginRepository.Get(x => (x.JobStatusId == 7 || x.JobStatusId == 8 || x.JobStatusId == 9) && x.EmployeeId == EmployeeId && x.DepartmentId == DepartmentId && x.ProcessId == ProcessId && x.IsActive == true && (x.IsBulk == false || x.IsBulk == null)).OrderByDescending(x => x.Id).ToList();
                        }
                        else
                        {
                            getWorkflowDetails = _viewWork.ViewGetWorkflowListforLoginRepository.Get(x => (x.JobStatusId == 7 || x.JobStatusId == 8 || x.JobStatusId == 9) && x.EmployeeId == EmployeeId && x.ProcessId == ProcessId && x.IsActive == true && (x.IsBulk == false || x.IsBulk == null)).OrderByDescending(x => x.Id).ToList();
                        }
                    }
                    else if (IsPendingJob == 2)
                    {
                        if (ProcessId == 7 && DepartmentId != 0)
                        {
                            getWorkflowDetails = _viewWork.ViewGetWorkflowListforLoginRepository.Get(x => (x.JobStatusId == 1 || x.JobStatusId == 2 || x.JobStatusId == 5) && x.EmployeeId == EmployeeId && x.DepartmentId == DepartmentId && x.ProcessId == ProcessId && x.IsActive == true && (x.IsBulk == false || x.IsBulk == null)).OrderByDescending(x => x.Id).ToList();
                        }
                        else
                        {
                            getWorkflowDetails = _viewWork.ViewGetWorkflowListforLoginRepository.Get(x => (x.JobStatusId == 1 || x.JobStatusId == 2 || x.JobStatusId == 5) && x.EmployeeId == EmployeeId && x.ProcessId == ProcessId && x.IsActive == true && (x.IsBulk == false || x.IsBulk == null)).OrderByDescending(x => x.Id).ToList();
                        }
                    }
                    else if (IsPendingJob == 3)
                    {
                        if (ProcessId == 7 && DepartmentId != 0)
                        {
                            getWorkflowDetails = _viewWork.ViewGetWorkflowListforLoginRepository.Get(x => (x.JobStatusId == 3 || x.JobStatusId == 4 || x.JobStatusId == 6) && x.EmployeeId == EmployeeId && x.DepartmentId == DepartmentId && x.ProcessId == ProcessId && x.IsActive == true && (x.IsBulk == false || x.IsBulk == null)).OrderByDescending(x => x.Id).ToList();
                        }
                        else
                        {
                            getWorkflowDetails = _viewWork.ViewGetWorkflowListforLoginRepository.Get(x => (x.JobStatusId == 3 || x.JobStatusId == 4 || x.JobStatusId == 6) && x.EmployeeId == EmployeeId && x.ProcessId == ProcessId && x.IsActive == true && (x.IsBulk == false || x.IsBulk == null)).OrderByDescending(x => x.Id).ToList();
                        }
                    }
                    else if (IsPendingJob == 4)
                    {
                        if (ProcessId == 7 && DepartmentId != 0)
                        {
                            getWorkflowDetails = _viewWork.ViewGetWorkflowListforLoginRepository.Get(x => x.JobStatusId == 10 && x.EmployeeId == EmployeeId && x.DepartmentId == DepartmentId && x.ProcessId == ProcessId && x.IsActive == true && (x.IsBulk == false || x.IsBulk == null)).OrderByDescending(x => x.Id).ToList();
                        }
                        else
                        {
                            getWorkflowDetails = _viewWork.ViewGetWorkflowListforLoginRepository.Get(x => x.JobStatusId == 10 && x.EmployeeId == EmployeeId && x.ProcessId == ProcessId && x.IsActive == true && (x.IsBulk == false || x.IsBulk == null)).OrderByDescending(x => x.Id).ToList();
                        }
                    }
                    //Bulk Closure purpose --05/05/2017 3:50:00 PM
                    else if (IsPendingJob == 6)
                    {
                        if (ProcessId == 7 && DepartmentId != 0)
                        {
                            //getWorkflowDetails = _db.ViewGetWorkflowListforLogin.Where(x => x.JobStatusId == 10 && x.EmployeeId == EmployeeId && x.DepartmentId == DepartmentId && x.ProcessId == ProcessId && x.IsActive == true).OrderByDescending(x => x.Id).ToList();
                            getWorkflowDetails = _viewWork.ViewGetWorkflowListforLoginRepository.Get(x => x.IsBulk == true && x.EmployeeId == EmployeeId && x.DepartmentId == DepartmentId && x.ProcessId == ProcessId && x.IsActive == true && (x.Status == "Assigned" || x.Status == "In Process")).OrderByDescending(x => x.Id).ToList();
                        }
                        else
                        {
                            getWorkflowDetails = _viewWork.ViewGetWorkflowListforLoginRepository.Get(x => x.IsBulk == true && x.EmployeeId == EmployeeId && x.ProcessId == ProcessId && x.IsActive == true && (x.Status == "Assigned" || x.Status == "In Process")).OrderByDescending(x => x.Id).ToList();
                        }
                    }
                    else if (IsPendingJob == 7)
                    {
                        if (ProcessId == 7 && DepartmentId != 0)
                        {
                            getWorkflowDetails = _viewWork.ViewGetWorkflowListforLoginRepository.Get(x => x.IsBulk == true && x.IsActive == true && x.EmployeeId == EmployeeId && x.DepartmentId == DepartmentId && x.ProcessId == ProcessId && (x.Status == "Bulk Order Complete" || x.Status == "Attachment Missing")).OrderByDescending(x => x.Id).ToList();
                        }
                        else
                        {
                            getWorkflowDetails = _viewWork.ViewGetWorkflowListforLoginRepository.Get(x => x.IsBulk == true && x.IsActive == true && x.EmployeeId == EmployeeId && x.ProcessId == ProcessId && (x.Status == "Bulk Order Complete" || x.Status == "Attachment Missing")).OrderByDescending(x => x.Id).ToList();
                        }
                    }
                    int freshJobsCount = 0, revisionJobsCount = 0, reworkJobsCount = 0, quoteJobsCount = 0, bulkJobsCount = 0, bulkUploadJobsCount = 0;
                    if (ProcessId == 7 && DepartmentId != 0)
                    {
                        var workflowListForLogin = _viewWork.ViewGetWorkflowListforLoginRepository
                            .Get(x => x.EmployeeId == EmployeeId && x.DepartmentId == DepartmentId && x.ProcessId == ProcessId && x.IsActive == true)
                            .Select(x => new { JobStatusId = x.JobStatusId, IsBulk = x.IsBulk, Statuses = x.Status }).ToList();
                        var normalWorkflowList = workflowListForLogin.Where(x => x.IsBulk == false || x.IsBulk == null).Select(x => new { JobStatusId = x.JobStatusId }).ToList();

                        freshJobsCount = normalWorkflowList.Count(x => (x.JobStatusId == 7 || x.JobStatusId == 8 || x.JobStatusId == 9));
                        revisionJobsCount = normalWorkflowList.Count(x => (x.JobStatusId == 1 || x.JobStatusId == 2 || x.JobStatusId == 5));
                        reworkJobsCount = normalWorkflowList.Count(x => (x.JobStatusId == 3 || x.JobStatusId == 4 || x.JobStatusId == 6));
                        quoteJobsCount = normalWorkflowList.Count(x => x.JobStatusId == 10);
                        //Bulk Closure purpose --05/05/2017 3:50:00 PM
                        bulkJobsCount = workflowListForLogin.Count(x => x.IsBulk == true && (x.Statuses == "Assigned" || x.Statuses == "In Process"));
                        bulkUploadJobsCount = workflowListForLogin.Count(x => x.IsBulk == true && (x.Statuses == "Bulk Order Complete" || x.Statuses == "Attachment Missing"));
                    }
                    else
                    {
                        var workflowListForLogin = _viewWork.ViewGetWorkflowListforLoginRepository
                            .Get(x => x.EmployeeId == EmployeeId && x.ProcessId == ProcessId && x.IsActive == true)
                            .Select(x => new { JobStatusId = x.JobStatusId, IsBulk = x.IsBulk, Statuses = x.Status }).ToList();

                        var normalWorkflowList = workflowListForLogin.Where(x => x.IsBulk == false || x.IsBulk == null).Select(x => new { JobStatusId = x.JobStatusId }).ToList();

                        freshJobsCount = normalWorkflowList.Count(x => (x.JobStatusId == 7 || x.JobStatusId == 8 || x.JobStatusId == 9));
                        revisionJobsCount = normalWorkflowList.Count(x => (x.JobStatusId == 1 || x.JobStatusId == 2 || x.JobStatusId == 5));
                        reworkJobsCount = normalWorkflowList.Count(x => (x.JobStatusId == 3 || x.JobStatusId == 4 || x.JobStatusId == 6));
                        quoteJobsCount = normalWorkflowList.Count(x => (x.JobStatusId == 10));
                        //Bulk Closure purpose --05/05/2017 3:50:00 PM
                        bulkJobsCount = workflowListForLogin.Count(x => x.IsBulk == true && (x.Statuses == "Assigned" || x.Statuses == "In Process"));
                        bulkUploadJobsCount = workflowListForLogin.Count(x => x.IsBulk == true && (x.Statuses == "Bulk Order Complete" || x.Statuses == "Attachment Missing"));
                    }
                    foreach (var job in getWorkflowDetails)
                    {
                        var estTimeOfArtist = _tableWork.ProcessWorkFlowTranRepository.Get(x => x.Wfmid == job.Wfmid && x.ProcessId == 3).OrderByDescending(x => x.Id).Select(x => new { x.EstimatedTime }).FirstOrDefault();
                        if (estTimeOfArtist != null)
                        {
                            if (estTimeOfArtist.EstimatedTime != null)
                            {
                                job.EstimatedTime = Convert.ToInt32(estTimeOfArtist.EstimatedTime);
                            }
                        }

                        if (job.Status == "In Process")
                        {
                            job.WorkStatus = _tableWork.EmployeeAssignRepository.Get(x => x.Wftid == job.Wftid).OrderByDescending(x => x.Id).Select(x => x.Status).FirstOrDefault();
                        }
                        else
                        {
                            job.WorkStatus = job.Status;
                        }
                    }
                    result = new
                    {
                        GetWorkflowDetails = getWorkflowDetails,
                        FreshJobsCount = freshJobsCount,
                        RevisionJobsCount = revisionJobsCount,
                        ReworkJobsCount = reworkJobsCount,
                        QuoteJobsCount = quoteJobsCount,

                        BulkJobsCount = bulkJobsCount,  //Bulk Closure purpose --05/05/2017 3:50:00 PM
                        BulkUploadJobsCount = bulkUploadJobsCount
                    };

                }
            }
            catch (Exception ex)
            {
             //   using (ApplicationDbContext _context = new ApplicationDbContext(_configuration))
              //  {
                    Log addlog = new Log();
                    addlog.Module = "GetWorkFlowList Function in Allocation Rep.";
                    addlog.Description = "Error Msg GetWorkFlowList Function: " + ProcessId + "-" + EmployeeId + "-" + ex.Message.ToString() + "-" + "Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source + "string";
                    addlog.Type = "Error Occ";
                    addlog.CreatedUtc = DateTime.UtcNow;
                    _context.Logs.Add(addlog);
                    _context.SaveChanges();
               // }
            }
            return result;
        }
        public Object CheckSelfQC(ViewJobAllocation WFTIdAndEmployeeId)
        {
            var result = new
            {
                SelfQC = false
            };
            var assignedEmployees = GetAssignedEmployees(WFTIdAndEmployeeId);
            if (assignedEmployees.Count() > 1)
            {
                return result;
            }
            else
            {
                var assignedEmployeeDetails = assignedEmployees.FirstOrDefault();
                //var customerId = _db.ProcessWorkFlowMasters.Where(x => x.Id == assignedEmployeeDetails.Wfmid).Select(x => x.CustomerId).FirstOrDefault();
                var clientId = _tableWork.JobOrderRepository.Get(x => x.Id == assignedEmployeeDetails.JId).Select(x => x.ClientId).FirstOrDefault();
                var selfQCProcess = _tableWork.CustomerVsEmployeeRepository.Get(x => x.EmployeeId == WFTIdAndEmployeeId.EmployeeId && x.CustomerId == clientId && x.IsDeleted == false && x.IsAgmapproved == true).FirstOrDefault();
                if (selfQCProcess != null)
                {
                    result = new
                    {
                        SelfQC = true
                    };
                    return result;
                }
                return result;
            }
        }
 

        public Object GetCompletedJobs(int EmpId)
        {
            var result = new Object();
            var JobDetails = new List<ViewJobForcompletedJob>();

            var CompletedJobsList = new List<ViewJobForcompletedJob>();
            var resultCompletedJobsList = new List<ViewJobForcompletedJob>();
            int ResultForCompletedList = 0, ResultForUnapprovedList = 0;
            var salesCheck = _tableWork.CustomerVsSalesEmployeeRepository.Get(x => x.EmployeeId == EmpId && x.IsActive == true).FirstOrDefault();
            if (salesCheck != null)
            {
                CompletedJobsList = _viewWork.ViewJobForcompletedJobsRepository.Get(x => x.ProcessId == 1 && (x.StatusId == 1 || x.StatusId == 18 || x.StatusId == 26) && x.PreviousProcessId != null && x.JobClosedUtc != null && x.DateofUpload == null && x.IsDeleted == false && x.IsActive == true && x.Scopetype == "Trial" && x.SalesEmpId == EmpId).OrderByDescending(x => x.TranId).ToList();

                resultCompletedJobsList = CompletedJobsList.ToList();
                return result = new
                {
                    ResultForCompletedList = CompletedJobsList.Count(),
                    resultCompletedJobsList = CompletedJobsList
                };
            }
            else
            {
                CompletedJobsList = _viewWork.ViewJobForcompletedJobsRepository.Get(x => x.ProcessId == 1 && (x.StatusId == 1 || x.StatusId == 18 || x.StatusId == 26) && x.PreviousProcessId != null && x.JobClosedUtc != null && x.DateofUpload == null && x.IsDeleted == false && x.IsActive == true && x.Scopetype != "Trial").OrderByDescending(x => x.TranId).ToList();

                resultCompletedJobsList = CompletedJobsList.ToList();
                return result = new
                {
                    ResultForCompletedList = CompletedJobsList.Count(),
                    resultCompletedJobsList = CompletedJobsList
                };
            }
        }

      

        public IEnumerable<ViewJobForcompletedJob> GetCompletedJobsforService(int EmpId)
        {
            var CompletedJobsList = new List<ViewJobForcompletedJob>();
            var resultCompletedJobsList = new List<ViewJobForcompletedJob>();

            var salesCheck = _tableWork.CustomerVsSalesEmployeeRepository.Get(x => x.EmployeeId == EmpId && x.IsActive == true).FirstOrDefault();
            if (salesCheck != null)
            {
                CompletedJobsList = _viewWork.ViewJobForcompletedJobsRepository.Get(x => x.ProcessId == 1 && (x.StatusId == 1 || x.StatusId == 18 || x.StatusId == 26) && x.PreviousProcessId != null && x.JobClosedUtc != null && x.DateofUpload == null && x.IsDeleted == false && x.IsActive == true && x.Scopetype == "Trial" && x.SalesEmpId == EmpId).OrderByDescending(x => x.TranId).ToList();

                return resultCompletedJobsList = CompletedJobsList.ToList();
            }
            else
            {
                CompletedJobsList = _viewWork.ViewJobForcompletedJobsRepository.Get(x => x.ProcessId == 1 && (x.StatusId == 1 || x.StatusId == 18 || x.StatusId == 26) && x.PreviousProcessId != null && x.JobClosedUtc != null && x.DateofUpload == null && x.IsDeleted == false && x.IsActive == true && x.Scopetype != "Trial").OrderByDescending(x => x.TranId).ToList();

                return resultCompletedJobsList = CompletedJobsList.ToList();
            }
        }

        public Object GetAssignedEmployeesToChangeEstTime(int JId)
        {
            var assignedEmployees = _tableWork.EmployeeAssignRepository.GetAllVal(x => x.Employee).Where(x => (x.Jid == JId) && (x.ProcessId == 3) && x.Remarks != null).Select(x => new { Id = x.EmployeeId, Name = x.Employee.EmployeeName, EstimatedTime = x.EstimatedTime }).ToList();
            var chkRevise = _tableWork.ProcessWorkFlowTranRepository.Get(x => x.Jid == JId && x.ProcessId == 4 && (x.StatusId == 11 || x.StatusId == 26)).OrderByDescending(x => x.Id).ToList();
            var result = new
            {
                AssignedEmployees = assignedEmployees,
                ChkRevise = chkRevise
            };
            return result;
        }
        public IEnumerable<GetEmployee> GetAssignedEmployees(ViewJobAllocation CustomerJobType)
        {
            var processWorkFlowTran = _tableWork.ProcessWorkFlowTranRepository.Get(x => x.Id == CustomerJobType.TranId).FirstOrDefault();
            var getEmployees = new List<ViewGetEmployeesForPendingJob>();
            if (CustomerJobType.ProcessId == null || CustomerJobType.ProcessId == 0)
            {
                //getEmployees = _db.ViewGetEmployeesForPendingJobs.Where(x => x.Wfmid == processWorkFlowTran.Wfmid && x.IsActive == true).ToList();
                getEmployees = _viewWork.ViewGetEmployeesForPendingJobsRepository.Get(x => x.Jid == processWorkFlowTran.Jid && x.IsActive == true).ToList();
            }
            else
            {
                //getEmployees = _db.ViewGetEmployeesForPendingJobs.Where(x => x.Wfmid == processWorkFlowTran.Wfmid && x.ProcessId == CustomerJobType.ProcessId && x.IsActive == true).ToList();
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
        public Object GetCustomerScopeValues(int DepartmentId, int ClientId) //,int CustomerId
        {
            var scopeValues = _tableWork.CustomerVsScopeRepository.Get(x => x.DeptId.Equals(DepartmentId) && x.CustomerId.Equals(ClientId) && x.IsDeleted == false && x.IsActive == true).AsQueryable();
            var scopeDetails = new List<DropdownValues>();
            foreach (var item in scopeValues)
            {
                scopeDetails.Add(new DropdownValues
                {
                    //Id = item.Id,
                    //Name = item.Description
                    Id = item.ScopeId,
                    Name = item.ScopeName
                });
            }
            var result = new
            {
                ScopeDetails = scopeDetails
            };
            return result;
        }
        public Object ChangeEstimatedTime(ProcessMovement processMovement)
        {
            var result = new
            {
                Success = false
            };
             using (var transaction = _context.Database.BeginTransaction())
              {
            var employeeAssign = _context.EmployeeAssigns.Where(x => x.ProcessId == processMovement.ProcessId && x.Jid == processMovement.JId && x.EmployeeId == processMovement.EmployeeId && x.Remarks != null).FirstOrDefault();
            employeeAssign.EstimatedTime = processMovement.EstimatedTime;
            employeeAssign.DeviationTime = (processMovement.EstimatedTime - employeeAssign.TotalTimeTaken);
            employeeAssign.UpdatedBy = processMovement.UpdatedBy;
            employeeAssign.UpdatedUtc = DateTime.UtcNow;
               _context.EmployeeAssigns.Update(employeeAssign);
               // _context.SaveChanges();

            var traydetailMovementTran = _context.TrayDetailMovementTrans.Where(x => x.Wfmid == processMovement.TranMasterId && x.Process == "Production" && x.EmployeeId == processMovement.EmployeeId && x.IsDeleted == false).OrderByDescending(x => x.Id).FirstOrDefault();
            traydetailMovementTran.EstimatedTime = processMovement.EstimatedTime;
            traydetailMovementTran.UpdatedBy = processMovement.UpdatedBy;
            traydetailMovementTran.UpdatedUtc = DateTime.UtcNow;
                _context.TrayDetailMovementTrans.Update(traydetailMovementTran);


            var jobOrder = _context.JobOrders.Where(x => x.Id == processMovement.JId).FirstOrDefault();
            var jobStatus = _context.JobStatuses.Where(x => x.Id == jobOrder.JobStatusId).FirstOrDefault();

            var traydetailMovement = _context.TrayDetailMovements.Where(x => x.FileName == jobOrder.FileName && x.CustomerJobType == jobOrder.CustomerJobType && x.JobStatus == jobStatus.JobStatusDescription && x.ProductionAllocatedToEmpId == processMovement.EmployeeId && x.IsDeleted == false).OrderByDescending(x => x.Id).FirstOrDefault();
            traydetailMovement.EstimationTime = processMovement.EstimatedTime;
            traydetailMovement.UpdatedBy = processMovement.UpdatedBy;
            traydetailMovement.UpdatedUtc = DateTime.UtcNow;
               _context.TrayDetailMovements.Update(traydetailMovement);

            var estTimeAndDevTime = _context.EmployeeAssigns.Where(x => (x.Wftid == employeeAssign.Wftid
            ) && (x.ProcessId == processMovement.ProcessId) && x.Remarks != null).Select(x => new { x.EstimatedTime, x.DeviationTime }).ToList();
            var estimatedTime = estTimeAndDevTime.Sum(x => x.EstimatedTime);
            var deviationTime = estTimeAndDevTime.Sum(x => x.DeviationTime);

            var processWorkFlowTran = _context.ProcessWorkFlowTrans.Where(x => x.Jid == processMovement.JId && x.ProcessId == processMovement.ProcessId).OrderByDescending(x => x.Id).FirstOrDefault();
            processWorkFlowTran.EstimatedTime = estimatedTime;
            processWorkFlowTran.DeviationTime = deviationTime;
            processWorkFlowTran.UpdatedBy = processMovement.UpdatedBy;
            processWorkFlowTran.UpdatedUtc = DateTime.UtcNow;
            _context.ProcessWorkFlowTrans.Update(processWorkFlowTran);

            jobOrder.EstimatedTime = estimatedTime;
            jobOrder.UpdatedBy = processMovement.UpdatedBy;
            jobOrder.UpdatedUtc = DateTime.UtcNow;
            _context.JobOrders.Update(jobOrder);

            long dbstatus = _db.SaveChanges();
             transaction.Commit();
            bool status = dbstatus > 0;
            result = new
            {
                Success = status
            };
            }
            return result;

        }
        public Object GetQuerySPDetailsForQA(int JId)
        {
            return (from jo in _tableWork.JobOrderRepository.Get()
                    join sc in _tableWork.ScopeRepository.Get() on jo.ScopeId equals sc.Id into scope
                    from sc1 in scope.DefaultIfEmpty()
                    select new
                    {
                        EstimatedTime = jo.EstimatedTime,
                        Scope = sc1.Description,
                        Id = jo.Id,
                        ScopeId = sc1.Id, //added
                        StitchCount = jo.StitchCount
                    })
                        .Where(x => x.Id == JId).FirstOrDefault();
            //using (ApplicationDbContext _db = new ApplicationDbContext(_configuration))
            //{
            //    return (from jo in _db.JobOrders
            //            join sc in _db.Scopes on jo.ScopeId equals sc.Id into scope
            //            from sc1 in scope.DefaultIfEmpty()
            //            select new
            //            {
            //                EstimatedTime = jo.EstimatedTime,
            //                Scope = sc1.Description,
            //                Id = jo.Id,
            //                StitchCount = jo.StitchCount
            //            })
            //            .Where(x => x.Id == JId).FirstOrDefault();
            //}
        }
        public Object GetDepartmentProcess()
        {
            var result = (from cus in _tableWork.CustomerRepository.Get()
                          join cusdept in _tableWork.CustomerDepartmentRepository.Get() on cus.Id equals cusdept.CustomerId
                          select new
                          {
                              cus.Id,
                              cus.ShortName,
                              cusdept.DepartmentId,
                              cus.IsDeleted
                          }).ToList().Where(x => x.DepartmentId == 2 && x.IsDeleted == false).OrderBy(x => x.ShortName).ToList();

            return result;

        }
    }
}

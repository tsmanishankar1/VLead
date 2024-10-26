using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class ReportService
    {
        private readonly TableWork _tableWork;
        private readonly ViewWork _viewWork;
        private readonly ProcedureWork _procedureWork;
        private readonly UnitWork _unitWork;
        private readonly UnitProcedure _unitprocedure;
      
        private readonly ApplicationDbContext db;
        private readonly ApplicationDbContext adb;
        private readonly ApplicationDbContext dbo;
        public ReportService(TableWork tableWork,ViewWork viewWork,
            ProcedureWork procedureWork,UnitWork unitWork,UnitProcedure unitProcedure,ApplicationDbContext dbContext)
        {
            db = dbContext;
            adb = dbContext;
            dbo = dbContext;
          
            _tableWork = tableWork;
            _viewWork = viewWork;
            _procedureWork = procedureWork;
            _unitWork = unitWork;
            _unitprocedure = unitProcedure;
        }
        public Object GetProcess()
        { 
            var GetProcess = _tableWork.ProcessRepository.Get(x => x.IsActive == true).OrderBy(x => x.Id).ToList();
            var GetDepartment = _tableWork.DepartmentRepository.Get(x => (x.IsDeleted == false) && (x.Description.Contains("Artwork") || x.Description.Contains("Digi"))).OrderBy(x => x.Id).ToList();
            var GetCustomerClassification = _tableWork.CustomerClassificationRepository.Get(x => x.IsDeleted == false).OrderBy(x => x.Id).ToList();
            var DBDivList = _tableWork.DivisionRepository.Get(x => x.IsDeleted == false).ToList();
            var DBCusList = _tableWork.CustomerRepository.Get(x => x.IsDeleted == false).ToList();
            var GetList = new
            {
                ProcessList = GetProcess,
                DepartmentList = GetDepartment,
                CustomerClassification = GetCustomerClassification,
                DivisionList = DBDivList,
                CustomerList = DBCusList,
            };
            return GetList;
        }
        public Object GetLocationForJobOut()
        {
            var res = _tableWork.JobFilesLocationMasterRepository.GetAllVal(x => x.Department, x => x.Customer).Where(x => x.IsActive == true).ToList();
            var GetList = new
            {
                jobslist = res
            };
            return GetList;
        }

        public Object GetJobOrderDetailsList(JobOrderDetailsViewModel oTrayDetailMovement)
        {

            var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@FromDate",  oTrayDetailMovement.FromDate),
                        new SqlParameter("@ToDate", oTrayDetailMovement.ToDate),
                        new SqlParameter("@ClientId", oTrayDetailMovement.ClientId),
                        new SqlParameter("@DepartmentId", oTrayDetailMovement.DepartmentId)
                    };

            return _procedureWork.ExecStoredProcedure<GetJobOrderReportDetails_Result>("GetJobOrderReportDetails @FromDate,@ToDate,@ClientId,@DepartmentId", sqlpara);
        }
        public Object GetCustomerWithShortNameAndFullName()
        {
            var GetCustomer = _tableWork.CustomerRepository.Get(x => x.IsBlacklisted == false && x.IsDeleted == false).OrderBy(x => x.Id).Select(x => new { x.Id, ShortName = x.ShortName + " - " + x.Name }).ToList();

            var GetList = new
            {
                ClientList = GetCustomer

            };
            return GetList;
        }
        public Object GetScope()
        {

            var GetScope = _tableWork.ScopeRepository.Get(x => x.IsDeleted == false).OrderBy(x => x.Id).ToList();

            var GetList = new
            {
                ScopeList = GetScope
            };
            return GetList;
        }

        public Object GetInvoiceListingReport(DashboardReportViewModel dashboardReportViewModel)
        {
            var sqlPara = new SqlParameter[]
            {
                new  SqlParameter("@comparisonType",dashboardReportViewModel.ComparisonType),
                new SqlParameter("@fromComparison",(object)dashboardReportViewModel.FromComparison ?? DBNull.Value),
                new SqlParameter("@toComparison",(object)dashboardReportViewModel.ToComparison ?? DBNull.Value),
                new SqlParameter("@fromDate",dashboardReportViewModel.FromDate),
                new SqlParameter("@toDate",dashboardReportViewModel.ToDate),
                new SqlParameter("@department",dashboardReportViewModel.Department),
                new SqlParameter("@customerIds",string.Join(",", dashboardReportViewModel.CustomerId)),
                new SqlParameter("@dateFormat",dashboardReportViewModel.DateFormat)
            };
            var detailResult = _procedureWork.ExecStoredProcedure<InvoiceReport_Result>("InvoiceReport @comparisonType,@fromComparison,@toComparison,@fromDate,@toDate,@department,@customerIds,@dateFormat", sqlPara).ToList();
            return detailResult;
        }

        public Object GetMonthWiseRevenueReport(DashboardReportViewModel dashboardReportViewModel)
        {
            var sqlPara = new SqlParameter[]
           {
               new  SqlParameter("@comparisonType",dashboardReportViewModel.ComparisonType),
               new SqlParameter("@fromComparison",(object)dashboardReportViewModel.FromComparison ?? DBNull.Value),
               new SqlParameter("@toComparison",(object)dashboardReportViewModel.ToComparison ?? DBNull.Value),
               new SqlParameter("@fromDate",dashboardReportViewModel.FromDate),
               new SqlParameter("@toDate",dashboardReportViewModel.ToDate),
               new SqlParameter("@customerIds",string.Join(",", dashboardReportViewModel.CustomerId)),
               new SqlParameter("@dateFormat",dashboardReportViewModel.DateFormat)
           };
            var detailResult = _procedureWork.ExecStoredProcedure<MonthWiseRevenueReport_Result>("MonthWiseRevenueReport @comparisonType,@fromComparison,@toComparison,@fromDate,@toDate,@customerIds,@dateFormat", sqlPara).ToList();
            if (dashboardReportViewModel.Department != "All")
            {
                int departmentId = 0;
                if (dashboardReportViewModel.Department == "Artwork")
                {
                    departmentId = 1;
                }
                else if (dashboardReportViewModel.Department == "Digitizing")
                {
                    departmentId = 2;
                }
                detailResult = detailResult.Where(x => x.departmentId == departmentId).ToList();
            }
            return detailResult;
        }

        public Object GetRevenueAnalysisReport(DashboardReportViewModel dashboardReportViewModel)
        {
            var scopeIds = dashboardReportViewModel.ScopeId;
            if (dashboardReportViewModel.FromComparison == null)
            {
                dashboardReportViewModel.FromComparison = "";
            }
            if (dashboardReportViewModel.ToComparison == null)
            {
                dashboardReportViewModel.ToComparison = "";
            }
            var sqlPara = new SqlParameter[]
            {
                new  SqlParameter("@comparisonType",dashboardReportViewModel.ComparisonType),
                new SqlParameter("@fromComparison",dashboardReportViewModel.FromComparison),
                new SqlParameter("@toComparison",dashboardReportViewModel.ToComparison),
                new SqlParameter("@fromDate",dashboardReportViewModel.FromDate),
                new SqlParameter("@toDate",dashboardReportViewModel.ToDate),
                new SqlParameter("@customerIds",string.Join(",", dashboardReportViewModel.CustomerId)),
                new SqlParameter("@dateFormat",dashboardReportViewModel.DateFormat)
            };
            var detailResult = _procedureWork.ExecStoredProcedure<RevenueAnalysisReport_Result>("RevenueAnalysisReport @comparisonType,@fromComparison,@toComparison,@fromDate,@toDate,@customerIds,@dateFormat", sqlPara).Where(x => scopeIds.Contains(x.scopeId)).ToList();

            return detailResult;
        }

        public Object GetListOfTrendReport(TrendReportViewModel trendReportViewModel)
        {
            DateTime startDate;
            DateTime endDate;
            int count;
            object detailResult;
            WeekendDaysCount(trendReportViewModel.toDate, out startDate, out endDate, out count);
            if (trendReportViewModel.CustomerClassificationId == 0)
            {
                var sqlPara = new SqlParameter[]
            {
                new  SqlParameter("@startDate",startDate),
                new SqlParameter("@endDate",endDate),
                new SqlParameter("@dateFormat",trendReportViewModel.DateFormat)
            };

                detailResult = _procedureWork.ExecStoredProcedure<TrendReport_Result>("TrendReport @startDate,@endDate,@dateFormat", sqlPara).ToList();
            }
            else
            {
                var sqlPara = new SqlParameter[]
            {
                new  SqlParameter("@startDate",startDate),
                new SqlParameter("@endDate",endDate),
                new SqlParameter("@dateFormat",trendReportViewModel.DateFormat),
                new SqlParameter("@departmentId",trendReportViewModel.DepartmentId),
                new SqlParameter("@customerClassificationId",trendReportViewModel.CustomerClassificationId)
            };
                detailResult = _procedureWork.ExecStoredProcedure<TrendDetail_Result>("TrendReport @startDate,@endDate,@dateFormat,@departmentId,@customerClassificationId", sqlPara).ToList();
            }
            var res = new
            {
                weekendPlusFederalHolidaysCount = count,
                trendAnalysisReport = detailResult
            };
            return res;

        }
        private void WeekendDaysCount(DateTime toDate, out DateTime startDate, out DateTime endDate, out int count)
        {
            var getYearOfToDate = toDate.Year.ToString();
            var getMonthOfToDate = toDate.Month.ToString();
            startDate = Convert.ToDateTime("01-" + getMonthOfToDate + "-" + getYearOfToDate);
            endDate = toDate;

            var startDt = startDate;
            var endDt = endDate;
            var workingDays = (endDate - startDate).Days + 1;

            var weekEndDays = (from d in Enumerable.Range(0, (endDate - startDate).Days + 1)
                               let currentDate = startDt.AddDays(d)
                               where currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday
                               select currentDate).ToList();

            count = workingDays - weekEndDays.Count();
        }

        public IEnumerable<FileStatusReportViewModel> GetTATFileStatusReports(InitialLoad GetFileStatusReport)
        {
            var totalFileStatuses = new List<FileStatusReportViewModel>();

            var sqlPara = new SqlParameter[]
            {
                new  SqlParameter("@FileReceivedDate",GetFileStatusReport.FileReceivedDate),
                new SqlParameter("@DepartmentId",(object)GetFileStatusReport.DepartmentId ?? DBNull.Value)
            };

            var GetTotalReceivedCollection = _procedureWork.ExecStoredProcedure<TATReport_Vlead_Result>("TATReport_Vlead @FileReceivedDate,@DepartmentId", sqlPara).ToList();
            foreach (var item in GetTotalReceivedCollection)
            {
                totalFileStatuses.Add(new FileStatusReportViewModel
                {
                    FileReceivedDate = GetFileStatusReport.FileReceivedDate,
                    ClientId = item.ClientId.Value,
                    DepartmentId = item.DepartmentId,
                    CustomerJobType = item.CustomerJobType,
                    Description = item.Description,
                    ShortName = item.ClientName,
                    TotalFileReceived = item.Total_Files_Recd.Value,
                    ArtCompleted = item.Artist_Completed.Value,
                    QCCompleted = item.Quality_Completed.Value,
                    FileUploaded = item.Uploaded_Files.Value,
                    ArtPending = item.Artist_Pending.Value,
                    QCPending = item.Quality_Pending.Value,
                    BuddyProof = item.Buddy_Proof.Value,
                    Proofpending = item.Proof_Reading_Pending.Value,
                    ProofCorrection = item.Proof_Correction.Value,
                    Supportpending = item.Support_Pending.Value,
                    Hold = item.Hold.Value,
                });
            }
            return totalFileStatuses;
        }
        
        public IEnumerable<FileStatusReportRefModify_Result> GetFileStatusReportsForRef(FileStatusReportModifyViewModel GetFileStatusReport)
        {
            DateTime utcdate = DateTime.ParseExact(GetFileStatusReport.FileReceivedDate.ToString("MM-dd-yyyy"), "MM-dd-yyyy", CultureInfo.InvariantCulture);
            var sqlPara = new SqlParameter[]
           {
                new  SqlParameter("@FileReceivedDate",utcdate),
                new SqlParameter("@DepartmentId",GetFileStatusReport.DepartmentId),
                new SqlParameter("@ClientId",GetFileStatusReport.ClientId),
                new SqlParameter("@jobstatusId",GetFileStatusReport.jobstatusId),
                new SqlParameter("@ClassificationId",GetFileStatusReport.customerClassficationId)
           };

            var totalFileStatuses = _procedureWork.ExecStoredProcedure<FileStatusReportRefModify_Result>("FileStatusReportRefModify @FileReceivedDate,@DepartmentId,@ClientId,@jobstatusId,@ClassificationId", sqlPara).ToList();

            return totalFileStatuses;
        }
        public IEnumerable<CurrentTrayFileStatusReport_Result> GetCurrentTrayProductionFileStatusReport(FileStatusReportViewModel GetFileStatusReport)
        {
            //var osFileStatuses = new List<FileStatusReportViewModel>();
            DateTime utcdate = DateTime.ParseExact(GetFileStatusReport.FileReceivedDate.ToString("MM-dd-yyyy"), "MM-dd-yyyy", CultureInfo.InvariantCulture);
            TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

            var sqlPara = new SqlParameter[]
            {
                new  SqlParameter("@FileReceivedDate",utcdate),
                new SqlParameter("@DepartmentId",GetFileStatusReport.DepartmentId),
                new SqlParameter("@ClientId",GetFileStatusReport.ClientId),
                new SqlParameter("@jobstatusId",GetFileStatusReport.jobstatusId),
                new SqlParameter("@CustomerJobType",GetFileStatusReport.CustomerJobType),
                new SqlParameter("@ProcessId",GetFileStatusReport.ProcessId)
            };

            var GetTotalReceivedCollection = _procedureWork.ExecStoredProcedure<CurrentTrayFileStatusReport_Result>("CurrentTrayFileStatusReport @ClientId,@DepartmentId,@CustomerJobType,@FileReceivedDate,@ProcessId,@jobstatusId", sqlPara).OrderByDescending(x => x.FileReceivedDate).ToList();

            return GetTotalReceivedCollection;
        }

        public Object GetQuotationTrackerReport(DailyReportViewModel dpr)
        {
            var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@FromDate", dpr.FromDate),
                        new SqlParameter("@ToDate", dpr.ToDate),
                        new SqlParameter("@ClientId",  dpr.ClientId),
                        new SqlParameter("@DepartmentId", dpr.DepartmentId)
                    };
            var jobs = _procedureWork.ExecStoredProcedure<GetQuoteTrackJobs_Result>("GetQuoteTrackJobs @FromDate,@ToDate,@ClientId,@DepartmentId", sqlpara).ToList();
            var QuotationTracker = new List<QuotationTrackerViewModel>();
            foreach (var item in jobs)
            {
                var tranDetails = _unitWork.ProcessWorkFlowTran.Where(x => x.Jid == item.JId && x.StatusId == 19).OrderBy(x => x.Id).FirstOrDefault();
                if (tranDetails != null)
                {
                    var ccTranRecord = _unitWork.ProcessWorkFlowTran.Where(x => x.Id > tranDetails.Id && x.Jid == item.JId && x.StatusId == 19).OrderBy(x => x.Id).FirstOrDefault();
                    decimal? ccJobQueryData = 0.0M;
                    var ccName = "";
                    if (ccTranRecord != null)
                    {
                        if (ccTranRecord.CreatedBy != null)
                            ccName = _unitWork.Employee.Where(x => x.EmployeeId == ccTranRecord.CreatedBy).FirstOrDefault().EmployeeName;

                        var spValue = _unitWork.JobQuery.Where(x => x.Wftid == ccTranRecord.Id && x.Jid == item.JId).Select(x => x.SpecialPrice).FirstOrDefault();
                        if (spValue != null)
                        {
                            ccJobQueryData = spValue.Value;
                        }
                    }

                    QuotationTracker.Add(new QuotationTrackerViewModel
                    {
                        Department = item.Department,
                        FileName = item.FileName,
                        FileReceivedDate = item.ESTFileRecdDate,
                        JobDate = item.ESTJobDate,
                        CompletedDate = item.ESTDateOfUpload,
                        Client = item.ShortName,
                        EstimatedTimeinMM = item.EstimatedTime,
                        StitchCount = item.StitchCount,
                        AmountinDollar = ccJobQueryData,
                        RemarksFromVLead = item.Remarks,
                        QuotSentbyAllocProductionQuality = item.artistName,
                        QuotationSentbyCC = ccName,
                        ApprovedStatus = item.ApprovedStatus,
                        ApprovedDate = item.ESTApprovedDate
                    });

                }

            }

            return QuotationTracker;

        }

        public Object GetClientStatusReport(ClientStatusReportViewModel clientStatusReportViewModel)
        {
            var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@fromDate", clientStatusReportViewModel.fromDate),
                        new SqlParameter("@toDate", clientStatusReportViewModel.toDate),
                        new SqlParameter("@CustomerId",  string.Join(",", clientStatusReportViewModel.CustomerId)),
                        new SqlParameter("@department", clientStatusReportViewModel.department)
                    };
            var detailResult = _procedureWork.ExecStoredProcedure<GetClientStatus_Result>("GetClientStatus @fromDate,@toDate,@CustomerId,@department", sqlpara).ToList();

            var res = new
            {
                clientStatusReport = detailResult
            };
            return res;

        }

        public Object GetNewClientReport(NewClientReportViewModel newClientReportViewModel)
        {
            var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@fromDate", newClientReportViewModel.fromDate),
                        new SqlParameter("@toDate", newClientReportViewModel.toDate),
                        new SqlParameter("@department",  newClientReportViewModel.department)
                    };
            var detailResult = _procedureWork.ExecStoredProcedure<GetNewClientReport_Result>("GetNewClientReport @fromDate,@toDate,@department", sqlpara).ToList();

            var res = new
            {
                newCLientReport = detailResult
            };
            return res;

        }

        public Object GetTrialClientReport(TrialClientReportViewModel trialClientReportViewModel)
        {
            var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@fromDate", trialClientReportViewModel.fromDate),
                        new SqlParameter("@toDate", trialClientReportViewModel.toDate),
                        new SqlParameter("@CustomerId",  string.Join(",", trialClientReportViewModel.CustomerId)),
                        new SqlParameter("@Department", trialClientReportViewModel.Department)
                    };
            var trialClientMisReport = _procedureWork.ExecStoredProcedure<GetTrialClientReport_Result>("GetTrialClientReport @fromDate,@toDate,@CustomerId,@Department", sqlpara).ToList();

            var res = new
            {
                trialClientMisReport = trialClientMisReport
            };

            return res;

        }

        public IList<GetTrialClientSummaryReport_Result> GetTrialClientSummaryReport(DateTime fromDate, DateTime toDate, int[] customerId, string department)
        {
            var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@fromDate", fromDate),
                        new SqlParameter("@toDate", toDate),
                        new SqlParameter("@CustomerId",  string.Join(",", customerId)),
                        new SqlParameter("@Department", department)
                    };
            var trialClientMisSummaryResult = _procedureWork.ExecStoredProcedure<GetTrialClientSummaryReport_Result>("GetTrialClientSummaryReport @fromDate,@toDate,@CustomerId,@Department", sqlpara).ToList();
            return trialClientMisSummaryResult;
        }

        public Object GetRevisionCountReport(RevisionCountReportViewModel revisionCountReportViewModel)
        {
            var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@fromDate", revisionCountReportViewModel.fromDate),
                        new SqlParameter("@toDate", revisionCountReportViewModel.toDate),
                        new SqlParameter("@CustomerId",  string.Join(",", revisionCountReportViewModel.CustomerId)),
                        new SqlParameter("@department", revisionCountReportViewModel.department)
                    };
            var detailResult = _procedureWork.ExecStoredProcedure<GetRevisionCount_Result>("GetRevisionCount @fromDate,@toDate,@CustomerId,@department", sqlpara).ToList();

            var res = new
            {
                revisionCountReport = detailResult
            };
            return res;

        }

        public Object GetEmployeeErrorstatus(EmployeeErrorReport oerrorstatus)
        {
            var totalFileerror = new List<EmployeeErrorReport>();

            var totalFileerrorlist = new List<EmployeeErrorReport>();

            var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@FromDate", oerrorstatus.FromDate),
                        new SqlParameter("@ToDate", oerrorstatus.ToDate)
                    };
            var Totalerrorfile = _procedureWork.ExecStoredProcedure<ErrorReport_Result>("ErrorReport @FromDate,@ToDate", sqlpara).ToList();

            var totalError = Totalerrorfile.Select(x => new { JID = x.JId }).GroupBy(x => x.JID).ToList();

            foreach (var item in totalError)
            {
                var status = _unitWork.ProcessWorkFlowTran.Where(x => x.Jid == item.Key && x.StatusId == 11 && x.PreviousProcessId == 7).ToList();

                var JobId = "";
                var FileName = "";
                var Shortname = "";
                var Department = "";
                var EmployeeName = "";
                var Remarks = "";
                var proofreadername = "";
                var QCName = "";
                var ProName = "";
                var NeedRevise = "";
                var sewout = "";
                var buttyproof = "";
                if (status.Count() == 1)
                {

                    var TranId = status.FirstOrDefault();

                    var aa = _unitWork.ProcessWorkFlowTran.Where(x => x.Jid == item.Key && x.Id <= TranId.Id).OrderByDescending(x => x.Id).ToList();

                    var jobId = _unitWork.JobOrder.Where(x => x.Id == item.Key).FirstOrDefault();
                    if (jobId != null)
                    {
                        JobId = jobId.JobId;
                        FileName = jobId.FileName;
                        var shortname = _unitWork.Customer.Where(x => x.Id == jobId.ClientId).Select(x => x.ShortName).FirstOrDefault();
                        Shortname = shortname;
                        var department = _unitWork.Department.Where(x => x.Id == jobId.DepartmentId).Select(x => x.Description).FirstOrDefault();
                        Department = department;
                    }
                    var empId = _unitWork.ProcessWorkFlowTran.Where(x => x.Jid == item.Key && x.StatusId == 11 && x.PreviousProcessId == 7).FirstOrDefault();
                    var empname = _unitWork.Employee.Where(x => x.EmployeeId == empId.UpdatedBy).Select(x => x.EmployeeName).FirstOrDefault();  //
                    if (empname != null)
                    {
                        EmployeeName = empname;
                        NeedRevise = "Revised";
                    }
                    else
                    {
                        EmployeeName = "";
                        NeedRevise = "";
                    }
                    var Pro = aa.Where(x => x.Jid == item.Key && x.ProcessId == 2 && x.StatusId == 1).OrderBy(x => x.Id).FirstOrDefault();
                    if (Pro != null)
                    {
                        ProName = Pro.EmployeeName;
                    }

                    var QC = aa.Where(x => x.Jid == item.Key && x.ProcessId == 4 && x.StatusId == 1).OrderBy(x => x.Id).FirstOrDefault();
                    if (QC != null)
                    {
                        QCName = QC.EmployeeName;
                    }


                    var PR = aa.Where(x => x.Jid == item.Key && x.ProcessId == 6 && x.StatusId == 1).OrderBy(x => x.Id).FirstOrDefault();
                    if (PR != null)
                    {
                        proofreadername = PR.EmployeeName;
                    }

                    var BPA = aa.Where(x => x.Jid == item.Key && x.ProcessId == 8 && x.StatusId == 20).OrderBy(x => x.Id).FirstOrDefault();
                    if (BPA != null)
                    {
                        buttyproof = BPA.EmployeeName;
                    }


                    var SOA = aa.Where(x => x.Jid == item.Key && x.ProcessId == 10 && x.StatusId == 21).OrderBy(x => x.Id).FirstOrDefault();
                    if (SOA != null)
                    {
                        sewout = SOA.EmployeeName;
                    }

                    var Revise = (from pwt in _tableWork.ProcessWorkFlowTranRepository.Get()
                                  join jo in _tableWork.JobOrderRepository.Get() on pwt.Jid equals jo.Id
                                  select new
                                  {
                                      pwt.StatusId,
                                      pwt.Jid
                                  }).Where(x => x.Jid == item.Key && x.StatusId == 18).FirstOrDefault();
                    if (Revise != null)
                    {
                        NeedRevise = "No Need Revise";
                    }

                    totalFileerrorlist.Add(new EmployeeErrorReport
                    {
                        // Id = item.Key,
                        JobId = JobId,
                        FileName = FileName,
                        ClientShortName = Shortname,
                        Department = Department,
                        ProName = ProName,
                        QCName = QCName,
                        PRName = proofreadername,
                        BuddyName = buttyproof,
                        SewoutName = sewout,
                        Remarks = status.FirstOrDefault().Remarks,
                        NeedRevise = NeedRevise,
                        EmployeeName = EmployeeName

                    });
                }
                if (status.Count() > 1)
                {
                    var trail = status.OrderByDescending(x => x.Id).Select(x => x.Id).ToList();
                    var test2 = 0;
                    foreach (var item1 in status)
                    {
                        var revisedfirst = trail.FirstOrDefault();
                        var test = trail.LastOrDefault();
                        var aa = new List<ProcessWorkFlowTran>();
                        if (item1.Id == test)
                        {
                            aa = _unitWork.ProcessWorkFlowTran.Where(x => x.Jid == item.Key && x.Id <= test).OrderByDescending(x => x.Id).ToList();
                            NeedRevise = "";
                        }
                        else
                        {

                            aa = _unitWork.ProcessWorkFlowTran.Where(x => x.Jid == item.Key && x.Id >= test2 && x.Id <= item1.Id).OrderByDescending(x => x.Id).ToList();
                        }

                        var jobId = _unitWork.JobOrder.Where(x => x.Id == item.Key).FirstOrDefault();
                        if (jobId != null)
                        {
                            JobId = jobId.JobId;
                            FileName = jobId.FileName;
                            var shortname = _unitWork.Customer.Where(x => x.Id == jobId.ClientId).Select(x => x.ShortName).FirstOrDefault();
                            Shortname = shortname;
                            var department = _unitWork.Department.Where(x => x.Id == jobId.DepartmentId).Select(x => x.Description).FirstOrDefault();
                            Department = department;

                        }



                        var revised = aa.Where(x => x.Jid == item.Key && x.Id == item1.Id && x.ProcessId == 4).OrderByDescending(x => x.Id).FirstOrDefault();
                        if (revised != null)
                        {
                            var revisedname = _unitWork.Employee.Where(x => x.EmployeeId == revised.UpdatedBy).Select(x => x.EmployeeName).FirstOrDefault();
                            if (revisedname != null)
                            {
                                NeedRevise = "Revised";
                                EmployeeName = revisedname;
                            }
                            else
                            {
                                NeedRevise = "";
                                EmployeeName = "";
                            }
                        }
                        else
                        {
                            NeedRevise = "";
                            EmployeeName = "";
                        }
                        if (item1.Id == revisedfirst)
                        {
                            var Revise = _unitWork.ProcessWorkFlowTran.Where(x => x.Jid == item.Key && x.StatusId == 18 && x.Id > revisedfirst && x.ProcessId == 1).FirstOrDefault();
                            if (Revise != null)
                            {
                                var reviseempname = _unitWork.Employee.Where(x => x.EmployeeId == Revise.AllocattedById).Select(x => x.EmployeeName).FirstOrDefault();
                                NeedRevise = "No Need Revise";
                                EmployeeName = reviseempname;
                            }
                        }

                        if (item1.Id == test)
                        {
                            var Pro = aa.Where(x => x.Jid == item.Key && x.ProcessId == 2 && x.StatusId == 1).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (Pro != null)
                            {
                                ProName = Pro.EmployeeName;
                            }
                            else
                            {
                                ProName = "";
                            }
                            var QC = aa.Where(x => x.Jid == item.Key && x.ProcessId == 4 && x.StatusId == 1).OrderBy(x => x.Id).FirstOrDefault();
                            if (QC != null)
                            {
                                QCName = QC.EmployeeName;
                            }
                            else
                            {
                                QCName = "";
                            }
                        }
                        if (item1.Id != test)
                        {
                            var Pro = aa.Where(x => x.Jid == item.Key && x.ProcessId == 2 && x.StatusId == 11).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (Pro != null)
                            {
                                ProName = Pro.EmployeeName;
                            }
                            else
                            {
                                ProName = "";
                            }
                            var QC = aa.Where(x => x.Jid == item.Key && x.ProcessId == 4 && x.StatusId == 11).OrderBy(x => x.Id).FirstOrDefault();
                            if (QC != null)
                            {
                                QCName = QC.EmployeeName;
                            }
                            else
                            {
                                QCName = "";
                            }
                        }

                        var PR = aa.Where(x => x.Jid == item.Key && x.ProcessId == 6 && x.StatusId == 1).OrderBy(x => x.Id).FirstOrDefault();
                        if (PR != null)
                        {
                            proofreadername = PR.EmployeeName;
                        }
                        else
                        {
                            proofreadername = "";
                        }

                        var BPA = aa.Where(x => x.Jid == item.Key && x.ProcessId == 8 && x.StatusId == 20).OrderBy(x => x.Id).FirstOrDefault();
                        if (BPA != null)
                        {
                            buttyproof = BPA.EmployeeName;
                        }
                        else
                        {
                            buttyproof = "";
                        }


                        var SOA = aa.Where(x => x.Jid == item.Key && x.ProcessId == 10 && x.StatusId == 21).OrderBy(x => x.Id).FirstOrDefault();
                        if (SOA != null)
                        {
                            sewout = SOA.EmployeeName;
                        }
                        else
                        {
                            sewout = "";
                        }

                        var demo = _unitWork.ProcessWorkFlowTran.Where(x => x.Id == item1.Id).FirstOrDefault();

                        var remarks = demo.Remarks;
                        if (remarks != null)
                        {
                            Remarks = remarks;
                        }

                        totalFileerrorlist.Add(new EmployeeErrorReport
                        {
                            //Id = item.Key,
                            JobId = JobId,
                            FileName = FileName,
                            ClientShortName = Shortname,
                            Department = Department,
                            ProName = ProName,
                            QCName = QCName,
                            PRName = proofreadername,
                            BuddyName = buttyproof,
                            SewoutName = sewout,
                            Remarks = Remarks,
                            NeedRevise = NeedRevise,
                            EmployeeName = EmployeeName

                        });
                        test2 = item1.Id;
                    }
                }
            }
            return totalFileerrorlist;
        }

        public Object GetQCEmployeeErrorstatus(EmployeeErrorReport oerrorstatus)
        {
            var totalfileerror = new List<EmployeeErrorReport>();

            var totalfileerrorlist = new List<EmployeeErrorReport>();

            var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@FromDate", oerrorstatus.FromDate),
                        new SqlParameter("@ToDate", oerrorstatus.ToDate)
                    };
            var totalerrorfile = _procedureWork.ExecStoredProcedure<QCErrorReport_Result>("QCErrorReport @FromDate,@ToDate", sqlpara).ToList();

            var totalError = totalerrorfile.Select(x => new { JID = x.JId }).GroupBy(x => x.JID).ToList();
            foreach (var item in totalError)
            {

                //var QCERROR = _unitWork.EmployeeAssign.Where(x => x.Jid == item.Key && x.ErrorCategoryId != null && x.ProcessId == 5).ToList();
                var QCERROR = _unitWork.EmployeeAssign.Where(x => x.Jid == item.Key && x.Status == "Error" && x.ProcessId == 5).ToList();

                var proname = "";
                var JobId = "";
                var FileName = "";
                var Shortname = "";
                var Department = "";
                var Remarks = "";
                var QCName = "";
                var sewoutname = "";
                var buddyname = "";
                var QCRemarks = "";// 

                if (QCERROR.Count() == 1)
                {
                    var jobId = _unitWork.JobOrder.Where(x => x.Id == item.Key).FirstOrDefault();
                    if (jobId != null)
                    {
                        JobId = jobId.JobId;
                        FileName = jobId.FileName;
                        var shortname = _unitWork.Customer.Where(x => x.Id == jobId.ClientId).Select(x => x.ShortName).FirstOrDefault();
                        Shortname = shortname;
                        var department = _unitWork.Department.Where(x => x.Id == jobId.DepartmentId).Select(x => x.Description).FirstOrDefault();
                        Department = department;

                    }
                    var demo = (from ea in _tableWork.EmployeeAssignRepository.Get()
                                join ec in _tableWork.ErrorCategoryRepository.Get() on ea.ErrorCategoryId equals ec.Id into joinedT
                                from ec in joinedT.DefaultIfEmpty()
                                select new
                                {
                                    ea.Status,
                                    ea.Jid,
                                    ea.Remarks,//
                                    ec.Description
                                }).Where(x => x.Jid == item.Key && x.Status == "Error").FirstOrDefault();
                    if (demo != null)
                    {
                        var remarks = demo.Description;
                        var qcremarks = demo.Remarks;//
                        Remarks = remarks;
                        QCRemarks = qcremarks;
                    }

                    //if (remarks != null)
                    //{

                    //}
                    //var QCname = _unitWork.EmployeeAssign.Where(x => x.Jid == item.Key && x.ProcessId == 5 && x.ErrorCategoryId != null).FirstOrDefault();
                    var QCname = _unitWork.EmployeeAssign.Where(x => x.Jid == item.Key && x.ProcessId == 5 && x.Status == "Error").FirstOrDefault();
                    if (QCname != null)
                    {
                        var empname = _unitWork.Employee.Where(x => x.EmployeeId == QCname.EmployeeId).FirstOrDefault();
                        QCName = empname.EmployeeCode + '-' + empname.EmployeeName; //
                    }
                    else
                    {
                        QCName = "";
                    }
                    var emp = _unitWork.EmployeeAssign.Where(x => x.Jid == item.Key && x.ProcessId == 3 && (x.Status == "Completed" || x.Status == "Sew Out" || x.Status == "Buddy Proof" || x.Status == "SelfQC")).FirstOrDefault();
                    if (emp != null)
                    {
                        var empname1 = _unitWork.Employee.Where(x => x.EmployeeId == emp.EmployeeId).FirstOrDefault();
                        proname = empname1.EmployeeCode + '-' + empname1.EmployeeName; //
                    }
                    else
                    {
                        proname = "";
                    }

                    totalfileerrorlist.Add(new EmployeeErrorReport
                    {
                        // Id = (Convert.ToInt32(item.WFMId)),
                        JobId = JobId,
                        FileName = FileName,
                        ClientShortName = Shortname,
                        Department = Department,
                        ProName = proname,
                        QCName = QCName,
                        Remarks = Remarks,
                        SewoutName = sewoutname,
                        BuddyName = buddyname,
                        QCRemarks = QCRemarks//
                    });

                }

                // if (QCERROR.Count() > 1)
                else
                {
                    var trail = QCERROR.OrderByDescending(x => x.Id).Select(x => x.Id).ToList();

                    var test2 = 0;
                    foreach (var item1 in QCERROR)
                    {

                        // var test = trail.FirstOrDefault();
                        var test = trail.LastOrDefault();
                        var aa = new List<EmployeeAssign>();
                        if (item1.Id == test)
                        {
                            aa = _unitWork.EmployeeAssign.Where(x => x.Jid == item1.Jid && x.Id <= test).OrderByDescending(x => x.Id).ToList();

                        }
                        else
                        {
                            aa = _unitWork.EmployeeAssign.Where(x => x.Jid == item1.Jid && x.Id >= test2 && x.Id <= item1.Id).OrderByDescending(x => x.Id).ToList();
                        }

                        var jobId = _unitWork.JobOrder.Where(x => x.Id == item.Key).FirstOrDefault();
                        if (jobId != null)
                        {
                            JobId = jobId.JobId;
                            FileName = jobId.FileName;
                            var shortname = _unitWork.Customer.Where(x => x.Id == jobId.ClientId).Select(x => x.ShortName).FirstOrDefault();
                            Shortname = shortname;
                            var department = _unitWork.Department.Where(x => x.Id == jobId.DepartmentId).Select(x => x.Description).FirstOrDefault();
                            Department = department;

                        }



                        var demo = (from ea in _tableWork.EmployeeAssignRepository.Get()
                                    join ec in _tableWork.ErrorCategoryRepository.Get() on ea.ErrorCategoryId equals ec.Id into joinedT
                                    from ec in joinedT.DefaultIfEmpty()
                                    select new
                                    {
                                        ea.Status,
                                        ea.Id,
                                        ec.Description,
                                        ea.ErrorCategoryId,
                                        ea.Remarks
                                    }).Where(x => x.Id == item1.Id && x.Status == "Error").FirstOrDefault();
                        //.Select(x => x.Description).FirstOrDefault();//

                        //var remarks = demo;
                        //var qcremarks = demo.Remarks;//
                        //var remarks = demo.Description;
                        //if (remarks != null)
                        //{
                        //    Remarks = remarks;
                        //}
                        if (demo != null)
                        {

                            var remarks = demo.Description;
                            var qcremarks = demo.Remarks;//
                            Remarks = remarks;
                            QCRemarks = qcremarks;
                        }
                        var QCname = _unitWork.EmployeeAssign.Where(x => x.Jid == item.Key && x.ProcessId == 5 && x.Id == item1.Id).FirstOrDefault();

                        if (QCname != null)
                        {
                            var empname = _unitWork.Employee.Where(x => x.EmployeeId == QCname.EmployeeId).FirstOrDefault();
                            QCName = empname.EmployeeCode + '-' + empname.EmployeeName;
                        }
                        else
                        {
                            QCName = "";
                        }
                        var emp = aa.Where(x => x.Jid == item1.Jid && x.ProcessId == 3 && x.Id <= item1.Id).FirstOrDefault();

                        if (emp != null)
                        {
                            var empname1 = _unitWork.Employee.Where(x => x.EmployeeId == emp.EmployeeId).FirstOrDefault();
                            proname = empname1.EmployeeCode + '-' + empname1.EmployeeName;
                        }
                        else
                        {
                            proname = "";
                        }


                        totalfileerrorlist.Add(new EmployeeErrorReport
                        {
                            //Id = (Convert.ToInt32(item.Key)),
                            JobId = JobId,
                            FileName = FileName,
                            ClientShortName = Shortname,
                            Department = Department,
                            ProName = proname,
                            QCName = QCName,
                            Remarks = Remarks,
                            SewoutName = sewoutname,
                            BuddyName = buddyname,
                            //QCRemarks = qcremarks//
                            QCRemarks = QCRemarks//
                        });
                        test2 = item1.Id;
                    }
                }

            }
            return totalfileerrorlist;
        }

        public Object GetFTEUtilizationReport(FTEUtilizationReportViewModel fteUtilizationReportViewModel)
        {
            var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@fromDate", fteUtilizationReportViewModel.fromDate),
                        new SqlParameter("@toDate", fteUtilizationReportViewModel.toDate),
                        new SqlParameter("@department", fteUtilizationReportViewModel.department)
                    };
            var detailResult = _procedureWork.ExecStoredProcedure<GetFTEUtilizationReport_Result>("GetFTEUtilizationReport @fromDate,@toDate,@department", sqlpara).ToList();
            var tempResult = new List<FteUtilizationFields>();
            var result = new List<FteUtilizationFields>();

            var artistCode = "";
            foreach (var item in detailResult)
            {
                if (artistCode == "")
                    artistCode = item.EmployeeCode; ;
                if (artistCode == item.EmployeeCode)
                    CalculateTotalArtistEstimationTime(ref result, item);
                else
                {
                    CalculateArtistWiseTotal(ref result, artistCode);
                    artistCode = item.EmployeeCode;
                    CalculateTotalArtistEstimationTime(ref result, item);
                }
            }
            CalculateArtistWiseTotal(ref result, artistCode);
            var res = new
            {
                fteUtilizationReport = result
            };
            return res;


        }

        private void CalculateArtistWiseTotal(ref List<FteUtilizationFields> result, string artistCode)
        {
            var temp = new FteUtilizationFields();
            temp.ArtistName = "Total";
            temp.ProductionWorkedTime = result.Where(x => x.EmployeeCode == artistCode).Sum(x => x.ProductionWorkedTime);
            temp.QualityWorkedTime = result.Where(x => x.EmployeeCode == artistCode).Sum(x => x.QualityWorkedTime);
            temp.BPWorkedTime = result.Where(x => x.EmployeeCode == artistCode).Sum(x => x.BPWorkedTime);
            temp.SOWorkedTime = result.Where(x => x.EmployeeCode == artistCode).Sum(x => x.SOWorkedTime);
            temp.EstimationTime = result.Where(x => x.EmployeeCode == artistCode).Sum(x => x.EstimationTime);
            temp.TotalEstimationTime = result.Where(x => x.EmployeeCode == artistCode).Sum(x => x.TotalEstimationTime);
            result.Where(x => x.EmployeeCode == artistCode).All(x => { x.FtePercentage = (Convert.ToDouble(x.TotalEstimationTime) / Convert.ToDouble(temp.TotalEstimationTime)); return true; });
            temp.FtePercentage = result.Where(x => x.EmployeeCode == artistCode).Sum(x => x.FtePercentage);
            result.Add(temp);
        }

        private void CalculateTotalArtistEstimationTime(ref List<FteUtilizationFields> result, GetFTEUtilizationReport_Result item)
        {
            var temp = new FteUtilizationFields();
            temp.ClientCode = item.ClientCode;
            temp.department = item.department;
            temp.EmployeeCode = item.EmployeeCode;
            temp.ArtistName = item.ArtistName;
            temp.ProductionWorkedTime = item.ProductionWorkedTime;
            temp.QualityWorkedTime = item.QualityWorkedTime;
            temp.BPWorkedTime = item.BPWorkedTime;
            temp.SOWorkedTime = item.SOWorkedTime;
            temp.EstimationTime = item.EstimationTime;
            temp.TotalEstimationTime = item.EstimationTime.Value + item.QualityWorkedTime.Value + item.BPWorkedTime.Value + item.SOWorkedTime.Value;
            result.Add(temp);
        }

        public Object GetFTEUtilizationwithClientwiseReport(FTEUtilizationwithClientWiseReportViewModel fteUtilizationwithClientWiseReportViewModel)
        {
            var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@fromDate", fteUtilizationwithClientWiseReportViewModel.fromDate),
                        new SqlParameter("@toDate", fteUtilizationwithClientWiseReportViewModel.toDate),
                        new SqlParameter("@CustomerId", string.Join(",", fteUtilizationwithClientWiseReportViewModel.CustomerId)),
                        new SqlParameter("@department", fteUtilizationwithClientWiseReportViewModel.department)
                    };
            var detailResult = _procedureWork.ExecStoredProcedure<GetFTEUtilizationWithClientWiseReport_Result>("GetFTEUtilizationWithClientWiseReport @fromDate,@toDate,@CustomerId,@department", sqlpara).ToList();
            var tempResult = new List<FteUtilizationFields>();
            var result = new List<FteUtilizationFields>();

            var clientCode = "";
            foreach (var item in detailResult)
            {
                if (clientCode == "")
                    clientCode = item.ClientCode; ;
                if (clientCode == item.ClientCode)
                    CalculateTotalArtistEstimationTimeClientWise(ref result, item);
                else
                {
                    CalculateArtistWiseTotalforClientwise(ref result, clientCode);
                    clientCode = item.ClientCode;
                    CalculateTotalArtistEstimationTimeClientWise(ref result, item);
                }
            }
            CalculateArtistWiseTotalforClientwise(ref result, clientCode);
            var res = new
            {
                fteUtilizationReport = result
            };
            return res;
        }

        private void CalculateArtistWiseTotalforClientwise(ref List<FteUtilizationFields> result, string clientCode)
        {
            var temp = new FteUtilizationFields();
            temp.ArtistName = "Total";
            temp.ProductionWorkedTime = result.Where(x => x.ClientCode == clientCode).Sum(x => x.ProductionWorkedTime);
            temp.QualityWorkedTime = result.Where(x => x.ClientCode == clientCode).Sum(x => x.QualityWorkedTime);
            temp.BPWorkedTime = result.Where(x => x.ClientCode == clientCode).Sum(x => x.BPWorkedTime);
            temp.SOWorkedTime = result.Where(x => x.ClientCode == clientCode).Sum(x => x.SOWorkedTime);
            temp.EstimationTime = result.Where(x => x.ClientCode == clientCode).Sum(x => x.EstimationTime);
            temp.TotalEstimationTime = result.Where(x => x.ClientCode == clientCode).Sum(x => x.TotalEstimationTime);
            result.Where(x => x.ClientCode == clientCode).All(x => { x.FtePercentage = (Convert.ToDouble(x.TotalEstimationTime) / Convert.ToDouble(temp.TotalEstimationTime)); return true; });
            temp.FtePercentage = result.Where(x => x.ClientCode == clientCode).Sum(x => x.FtePercentage);
            result.Add(temp);
        }

        private void CalculateTotalArtistEstimationTimeClientWise(ref List<FteUtilizationFields> result, GetFTEUtilizationWithClientWiseReport_Result item)
        {
            var temp = new FteUtilizationFields();
            temp.ClientCode = item.ClientCode;
            temp.department = item.department;
            temp.EmployeeCode = item.EmployeeCode;
            temp.ArtistName = item.ArtistName;
            temp.ProductionWorkedTime = item.ProductionWorkedTime;
            temp.QualityWorkedTime = item.QualityWorkedTime;
            temp.BPWorkedTime = item.BPWorkedTime;
            temp.SOWorkedTime = item.SOWorkedTime;
            temp.EstimationTime = item.EstimationTime;
            temp.TotalEstimationTime = item.EstimationTime.Value + item.QualityWorkedTime.Value + item.BPWorkedTime.Value + item.SOWorkedTime.Value;
            result.Add(temp);
        }
        public Object GetIntakeAndUploadedReport(IntakeAndUploadedReportViewModel intakeAndUploadedReportViewModel)
        {

            var sqlPara = new SqlParameter[]
            {
                new  SqlParameter("@fromDate",intakeAndUploadedReportViewModel.fromDate),
                new SqlParameter("@toDate",intakeAndUploadedReportViewModel.toDate),
                new SqlParameter("@dateFormat",intakeAndUploadedReportViewModel.dateFormat)
            };
            var detailResult = _procedureWork.ExecStoredProcedure<GetCCIntakeAndUploadedReport_Result>("GetCCIntakeAndUploadedReport @fromDate,@toDate,@dateFormat", sqlPara);

            var res = new
            {
                intakeAndUploadedReport = detailResult
            };
            return res;

        }
        public Object GetCancelledJobsReport(DailyReportViewModel GetCancelledJobStatusReport)
        {
            var sqlPara = new SqlParameter[]
            {
                new  SqlParameter("@ClientId",GetCancelledJobStatusReport.ClientId),
                new SqlParameter("@DepartmentId",GetCancelledJobStatusReport.DepartmentId),
                new SqlParameter("@FromDate",GetCancelledJobStatusReport.FromDate),
                new SqlParameter("@ToDate",GetCancelledJobStatusReport.ToDate)
            };
            var totaljobs = _procedureWork.ExecStoredProcedure<CancelledJobReport_Result>("CancelledJobReport @ClientId,@DepartmentId,@FromDate,@ToDate", sqlPara);

            //  CancelledJobReport(GetCancelledJobStatusReport.ClientId, GetCancelledJobStatusReport.DepartmentId, GetCancelledJobStatusReport.FromDate, GetCancelledJobStatusReport.ToDate).ToList();

            foreach (var job in totaljobs)
            {
                var checkRespRecord = _tableWork.ProcessWorkFlowTranRepository.Get(x => x.Jid == job.JId && (x.StatusId == 7 || x.StatusId == 9)).OrderByDescending(x => x.Id).FirstOrDefault();
                var actionInitiatedTray = new ProcessWorkFlowTran();
                if (checkRespRecord != null)
                {
                    actionInitiatedTray = _tableWork.ProcessWorkFlowTranRepository.Get(x => x.Jid == job.JId && x.Id >= checkRespRecord.Id && (x.StatusId == 6 || x.StatusId == 8 || x.StatusId == 19)).OrderByDescending(x => x.Id).LastOrDefault();
                    if (actionInitiatedTray != null)
                    {
                        job.ActionInitiatedTray = _tableWork.ProcessRepository.GetSingle(x => x.Id == actionInitiatedTray.PreviousProcessId).Name;
                        job.ActionInitiatedBy = _tableWork.EmployeeRepository.GetSingle(x => x.EmployeeId == actionInitiatedTray.CreatedBy).EmployeeName;
                    }
                }
                else
                {
                    actionInitiatedTray = _tableWork.ProcessWorkFlowTranRepository.Get(x => x.Jid == job.JId && (x.StatusId == 6 || x.StatusId == 8 || x.StatusId == 19)).OrderByDescending(x => x.Id).LastOrDefault();
                    if (actionInitiatedTray != null)
                    {
                        job.ActionInitiatedTray = _tableWork.ProcessRepository.GetSingle(x => x.Id == actionInitiatedTray.PreviousProcessId).Name;
                        job.ActionInitiatedBy = _tableWork.EmployeeRepository.GetSingle(x => x.EmployeeId == actionInitiatedTray.CreatedBy).EmployeeName;
                    }
                }

                var empAssignRecords = new List<EmployeeAssign>();
                if (checkRespRecord != null)
                {
                    empAssignRecords = _tableWork.EmployeeAssignRepository.Get(x => x.Jid == job.JId && x.Wftid >= checkRespRecord.Id && x.Status == "Worked").ToList();
                }
                else
                {
                    empAssignRecords = _tableWork.EmployeeAssignRepository.Get(x => x.Jid == job.JId && x.Status == "Worked").ToList();
                }

                int WorkedTimeProd = empAssignRecords.Where(x => x.ProcessId == 3).Sum(x => x.TotalTimeTaken).Value;
                int WorkedTimeQC = empAssignRecords.Where(x => x.ProcessId == 5).Sum(x => x.TotalTimeTaken).Value;
                int WorkedTimeProof = empAssignRecords.Where(x => x.ProcessId == 7).Sum(x => x.TotalTimeTaken).Value;
                int WorkedTimeSewout = empAssignRecords.Where(x => x.ProcessId == 11).Sum(x => x.TotalTimeTaken).Value;
                int WorkedTimeBuddy = empAssignRecords.Where(x => x.ProcessId == 9).Sum(x => x.TotalTimeTaken).Value;

                job.WorkedTimeProd = WorkedTimeProd == 0 ? 0 : WorkedTimeProd;
                job.WorkedTimeQC = WorkedTimeQC == 0 ? 0 : WorkedTimeQC;
                job.WorkedTimeProof = WorkedTimeProof == 0 ? 0 : WorkedTimeProof;
                job.WorkedTimeSewout = WorkedTimeSewout == 0 ? 0 : WorkedTimeSewout;
                job.WorkedTimeBuddy = WorkedTimeBuddy == 0 ? 0 : WorkedTimeBuddy;

            }

            return totaljobs;

        }
        
        public IEnumerable<OSFileStatusReportModify_Result> GetOSFileStatusReport(FileStatusReportViewModel GetFileStatusReport)
        {
            DateTime utcdate = DateTime.ParseExact(GetFileStatusReport.FileReceivedDate.ToString("MM-dd-yyyy"), "MM-dd-yyyy", CultureInfo.InvariantCulture);
            var sqlPara = new SqlParameter[]
            {
                new  SqlParameter("@ClientId",GetFileStatusReport.ClientId),
                new SqlParameter("@DepartmentId",GetFileStatusReport.DepartmentId),
                new SqlParameter("@CustomerJobType",GetFileStatusReport.CustomerJobType),
                new SqlParameter("@FileReceivedDate",utcdate)
            };
            var GetTotalReceivedCollection = _procedureWork.ExecStoredProcedure<OSFileStatusReportModify_Result>("OSFileStatusReportModify @ClientId,@DepartmentId,@CustomerJobType,@FileReceivedDate", sqlPara);

            return GetTotalReceivedCollection;
        }
        public IEnumerable<QueryFileStatusReport_Result> GetQuerySentReport(FileStatusReportViewModel GetFileStatusReport)
        {
            DateTime utcdate = DateTime.ParseExact(GetFileStatusReport.FileReceivedDate.ToString("MM-dd-yyyy"), "MM-dd-yyyy", CultureInfo.InvariantCulture);
            var sqlPara = new SqlParameter[]
            {
                new SqlParameter("@FileReceivedDate",utcdate),
                new SqlParameter("@ClientId",GetFileStatusReport.ClientId),
                new SqlParameter("@DepartmentId",GetFileStatusReport.DepartmentId),
                new SqlParameter("@jobstatusId",GetFileStatusReport.jobstatusId),
                new SqlParameter("@ClassificationId",GetFileStatusReport.customerClassficationId)
            };
            var GetTotalQuery = _procedureWork.ExecStoredProcedure<QueryFileStatusReport_Result>("QueryFileStatusReport @FileReceivedDate,@DepartmentId,@ClientId,@jobstatusId,@ClassificationId", sqlPara);
            return GetTotalQuery;
        }
        public Object UnbilledRevenueReport(UnbilledRevenueReportViewModel unbilledRevenueReportViewModel)
        {
            var scopeIds = unbilledRevenueReportViewModel.ScopeId;
            var sqlPara = new SqlParameter[]
            {
                new  SqlParameter("@fromDate",unbilledRevenueReportViewModel.fromDate),
                new SqlParameter("@toDate",unbilledRevenueReportViewModel.toDate),
                new SqlParameter("@customerIds",string.Join(",", unbilledRevenueReportViewModel.CustomerId)),
                new SqlParameter("@department",unbilledRevenueReportViewModel.department)

            };
            var detailResult = _procedureWork.ExecStoredProcedure<UnbilledRevenueReport_Result>("UnbilledRevenueReport @fromDate, @toDate, @customerIds, @department", sqlPara).Where(x => scopeIds.Contains(x.ScopeId)).ToList();
            //var detailResult = _db.RevenueAnalysisReport(dashboardReportViewModel.ComparisonType, dashboardReportViewModel.FromComparison, dashboardReportViewModel.ToComparison, dashboardReportViewModel.FromDate, dashboardReportViewModel.ToDate, string.Join(",", dashboardReportViewModel.CustomerId), dashboardReportViewModel.DateFormat).Where(x => scopeIds.Contains(x.scopeId)).ToList();
            var res = new
            {
                UnbilledRevenueRecords = detailResult
            };
            return res;

        }

        // SewOut Report
        public Object GetSewOutDetailsList(JobOrderDetailsViewModel oTrayDetailMovement)
        {
            var sqlpara = new SqlParameter[]
                {
                        new SqlParameter("@FileReceivedDateFrom",  oTrayDetailMovement.FromDate),
                        new SqlParameter("@FileReceivedDateTo", oTrayDetailMovement.ToDate)
                };
            var totalFileStatus = new List<SewOutReportModel>();

            var totalStatus = new List<SewOutReportModel>();
            int SumofTotalfiles = 0;
            int SumofTotalsewoutfiles = 0;
            decimal SumofTotalsewoutpercentage = 0;

            if (oTrayDetailMovement.ClientId != 0)
            {
                var param = new SqlParameter[]
                {
                        new SqlParameter("@FileReceivedDateFrom",  oTrayDetailMovement.FromDate),
                        new SqlParameter("@FileReceivedDateTo", oTrayDetailMovement.ToDate)
                };
                var totalfilesList = _procedureWork.ExecStoredProcedure<TotalNofreshfiles_Result>("TotalNofreshfiles @FileReceivedDateFrom,@FileReceivedDateTo", param).Where(x => x.ClientId == oTrayDetailMovement.ClientId).Count();
                var customerlist = _unitWork.Customer.Where(x => x.Id == oTrayDetailMovement.ClientId).Select(x => x.ShortName).FirstOrDefault();
                var paras = new SqlParameter[]
                {
                        new SqlParameter("@FileReceivedDateFrom",  oTrayDetailMovement.FromDate),
                        new SqlParameter("@FileReceivedDateTo", oTrayDetailMovement.ToDate)
                };
                var totalsewoutlist = _procedureWork.ExecStoredProcedure<TotalNoSewOutfiles_Result>("TotalNoSewOutfiles @FileReceivedDateFrom,@FileReceivedDateTo", paras).Where(x => x.ClientId == oTrayDetailMovement.ClientId && x.StatusId == 21).Count();
                var percentage = ((decimal)totalfilesList > 0) ? (((decimal)totalsewoutlist / (decimal)totalfilesList) * 100) : 0;
                var percentageround = Math.Round(percentage, 2);

                totalFileStatus.Add(new SewOutReportModel
                {
                    TotalList = totalfilesList,
                    customer = customerlist,
                    Total = totalsewoutlist,
                    SewOutpercentage = percentageround,
                    SumTotalfiles = totalfilesList,
                    SumTotalsewoutfiles = totalsewoutlist,
                    SumTotalsewoutpercentage = percentageround,
                });
            }

            else
            {
                var customerDetails = _procedureWork.ExecStoredProcedure<TotalNofreshfiles_Result>("TotalNofreshfiles @FileReceivedDateFrom,@FileReceivedDateTo", sqlpara).GroupBy(x => x.ClientId).ToList();
                foreach (var item in customerDetails)
                {
                    var param = new SqlParameter[]
                    {
                        new SqlParameter("@FileReceivedDateFrom",  oTrayDetailMovement.FromDate),
                        new SqlParameter("@FileReceivedDateTo", oTrayDetailMovement.ToDate)
                    };
                    var customerlist = _unitWork.Customer.Where(x => x.Id == item.Key).Select(x => x.ShortName).FirstOrDefault();
                    var totalfilesList = _procedureWork.ExecStoredProcedure<TotalNofreshfiles_Result>("TotalNofreshfiles @FileReceivedDateFrom,@FileReceivedDateTo", param).Where(x => x.ClientId == item.Key && x.DepartmentId == 2).Count();

                    var paras = new SqlParameter[]
                    {
                        new SqlParameter("@FileReceivedDateFrom",  oTrayDetailMovement.FromDate),
                        new SqlParameter("@FileReceivedDateTo", oTrayDetailMovement.ToDate)
                    };
                    var totalsewoutlist = _procedureWork.ExecStoredProcedure<TotalNoSewOutfiles_Result>("TotalNoSewOutfiles @FileReceivedDateFrom,@FileReceivedDateTo", paras).Where(x => x.ClientId == item.Key).Count();
                    var percentage = ((decimal)totalfilesList > 0) ? (((decimal)totalsewoutlist / (decimal)totalfilesList) * 100) : 0;
                    var percentageround = Math.Round(percentage, 2);

                    totalFileStatus.Add(new SewOutReportModel
                    {
                        TotalList = totalfilesList,
                        customer = customerlist,
                        Total = totalsewoutlist,
                        SewOutpercentage = percentageround,
                    });
                    SumofTotalfiles += totalfilesList;
                    SumofTotalsewoutfiles += totalsewoutlist;
                }

                var percentage1 = ((decimal)SumofTotalfiles > 0) ? (((decimal)SumofTotalsewoutfiles / (decimal)SumofTotalfiles) * 100) : 0;
                SumofTotalsewoutpercentage = Math.Round(percentage1, 2);
                totalFileStatus.Add(new SewOutReportModel
                {
                    SumTotalfiles = SumofTotalfiles,
                    SumTotalsewoutfiles = SumofTotalsewoutfiles,
                    SumTotalsewoutpercentage = SumofTotalsewoutpercentage,
                });

            }
            return totalFileStatus;
        }

        // Collection  Report
        public IEnumerable<CollectionsReport_Result> GetCollectionsReport(JobOrderDetailsViewModel GetCollectionseport)
        {

            var sqlpara = new SqlParameter[]
                {
                        new SqlParameter("@FromDate",  GetCollectionseport.FromDate),
                        new SqlParameter("@ToDate", GetCollectionseport.ToDate),
                };
            var totalcollections = new List<CollectionsReport_Result>();
            var GetTotalReceivedCollection = _procedureWork.ExecStoredProcedure<CollectionsReport_Result>("CollectionsReport @FromDate,@ToDate", sqlpara).ToList();
            foreach (var item in GetTotalReceivedCollection)
            {
                if (item.InvoiceDate != null)
                {
                    totalcollections.Add(new CollectionsReport_Result
                    {
                        ShortName = item.ShortName,
                        CollectionDate = item.CollectionDate,
                        //ReceiptValue = item.CollectionAmount,
                        AdjustmentAmount = (decimal)item.AdjustmentAmount,
                        InvoiceNo = item.InvoiceNo,
                        InvoiceDate = item.InvoiceDate,
                    });
                }
                else
                {
                    totalcollections.Add(new CollectionsReport_Result
                    {
                        ShortName = item.ShortName,
                        CollectionDate = item.CollectionDate,
                        //ReceiptValue = item.CollectionAmount,
                        AdjustmentAmount = (decimal)item.AdjustmentAmount,
                        InvoiceNo = "Advance",
                        InvoiceDate = item.InvoiceDate,
                    });

                }
            }
            return totalcollections;
        }

        //Query tracker
        public Object GetQueryTrackerReport(JobOrderDetailsViewModel GetQueryTrackerReport)
        {

            var queryTrackerViewModel = new List<QueryTrackerViewModel>();

            var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@FromDate",  GetQueryTrackerReport.FromDate),
                        new SqlParameter("@ToDate", GetQueryTrackerReport.ToDate),
                        new SqlParameter("@CustomerId", GetQueryTrackerReport.ClientId),
                        new SqlParameter("@DepartmentId", GetQueryTrackerReport.DepartmentId)
                    };
            //var jq = _db.GetQueryTrackJobs(GetQueryTrackerReport.DepartmentId, GetQueryTrackerReport.ClientId, GetQueryTrackerReport.FromDate, GetQueryTrackerReport.ToDate).ToList();
            var jq = _procedureWork.ExecStoredProcedure<GetQueryTrackJobs_Result>("GetQueryTrackJobs @DepartmentId,@CustomerId,@FromDate,@ToDate", sqlpara).ToList();
            foreach (var item in jq.ToList())
            {
                var myCheck1 = _unitWork.JobQuery.Where(x => x.Jid == item.JId && x.Id < item.Id && (x.StatusId == 7 || x.StatusId == 9)).OrderByDescending(x => x.Id).FirstOrDefault();
                var jqCheckDate = new List<JobQuery>();
                if (myCheck1 != null)
                {
                    jqCheckDate = _unitWork.JobQuery.Where(x => x.Jid == item.JId && x.Id < item.Id && x.Id > myCheck1.Id).ToList();
                }
                else
                {
                    jqCheckDate = _unitWork.JobQuery.Where(x => x.Jid == item.JId && x.Id < item.Id).ToList();
                }

                if (jqCheckDate.Count() > 0)
                {
                    jq.RemoveAll(x => x.JId == item.JId);
                    continue;
                }
                else
                {
                    var param = new SqlParameter[]
                    {
                        new SqlParameter("@Id",  item.Id)
                    };

                    //var basicDetails = _db.GetQueryTrackJobDetails(item.Id).FirstOrDefault();
                    var basicDetails = _procedureWork.ExecStoredProcedure<GetQueryTrackJobDetails_Result>("GetQueryTrackJobDetails @Id", param).FirstOrDefault();
                    var paras = new SqlParameter[]
                    {
                        new SqlParameter("@Id",  item.Id),
                        new SqlParameter("@JId",  item.JId),
                    };
                    var artists = _procedureWork.ExecStoredProcedure<GetQueryArtistDetails_Result>("GetQueryArtistDetails @Id, @JId", paras).ToList();

                    var prod = artists.FirstOrDefault(x => (x.ProcessId == 3 || x.ProcessId == 5 || x.ProcessId == 7 || x.ProcessId == 9 || x.ProcessId == 11));
                    var prodName = "";
                    var prodcomment = "";
                    if (prod != null)
                    {
                        prodcomment = prod.Comments;
                        prodName = prod.EmployeeName;
                    }

                    var prodAlloc = artists.FirstOrDefault(x => (x.ProcessId == 2 || x.ProcessId == 4 || x.ProcessId == 6));
                    var prodAllocName = "";
                    var prodallocationcomment = "";
                    if (prodAlloc != null)
                    {
                        prodallocationcomment = prodAlloc.Comments;
                        prodAllocName = prodAlloc.EmployeeName;
                    }

                    var ccTeam = artists.FirstOrDefault(x => x.ProcessId == 1);
                    var ccTeamName = "";
                    if (ccTeam != null)
                    {
                        ccTeamName = ccTeam.EmployeeName;
                    }

                    var respDateComments = artists.FirstOrDefault(x => (x.ProcessId == null || x.ProcessId == 1) && (x.StatusId == 7 || x.StatusId == 9));
                    var respDetailsComments = "";
                    var respDetailsQueryRaisedOn = "";
                    decimal? specialPrice = 0.0M;
                    if (respDateComments != null)
                    {
                        respDetailsComments = respDateComments.Comments;
                        respDetailsQueryRaisedOn = respDateComments.ESTQueryRaisedOn.ToString();
                        specialPrice = respDateComments.SpecialPrice;
                    }

                    queryTrackerViewModel.Add(new QueryTrackerViewModel
                    {
                        FileName = basicDetails.FileName,
                        FileReceivedDate = item.ESTJobDate,
                        Department = basicDetails.Description,
                        Client = item.ShortName,
                        ClientStatus = basicDetails.CustomerJobType,
                        JobType = basicDetails.JobStatusDescription,
                        //QuerySentDate = jqDetails.QueryRaisedOn,
                        QuerySentDate = basicDetails.ESTQueryRaisedOn,
                        Status = basicDetails.Name,
                        //QueryComments = jqDetails.Comments,
                        //QueryComments = basicDetails.Comments,
                        QueryAllocationTeamComments = prodallocationcomment,
                        QueryProductionTeamComments = prodcomment,
                        QueryResolvedComments = respDetailsComments,
                        QueryRaisedfromProductionTeam = prodName,
                        queryRaisedAllocationTeam = prodAllocName,
                        QuerySentbyinCCTeam = ccTeamName,
                        QueryResolvedDate = respDetailsQueryRaisedOn,
                        SpecialPricinginDollar = specialPrice
                    });

                    jq.RemoveAll(x => x.JId== item.JId);
                    continue;
                }
            }
            return queryTrackerViewModel;
        }

        public bool GetUserLoginDepartment(int EmployeeId)
        {

            bool status = false;
            try
            {
                bool IsMISDept = _unitWork.Employee.Include("Department").Where(x => x.EmployeeId == EmployeeId && x.IsDeleted == false && x.DepartmentId == 5 && x.Department.IsDeleted == false).Any();

                if (IsMISDept == true)
                {

                    status = true;
                }
                else
                {
                    status = false;
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return status;
        }


        public Object GetEmployeewisetotalList1(EmployeeWiseReport oTrayDetailMovement)
        {
            var totalFileStatus = new List<EmployeeDetailsReport>();
            var sqlPara = new SqlParameter[]
            {
                new  SqlParameter("@FromDate",oTrayDetailMovement.FromDate),
                new SqlParameter("@ToDate",oTrayDetailMovement.ToDate),
                new SqlParameter("@DepartmentId",oTrayDetailMovement.DepartmentId),
                new SqlParameter("@EmployeeId",oTrayDetailMovement.EmployeeId),
                new SqlParameter("@ClientId",oTrayDetailMovement.ClientId),
                new SqlParameter("@Status", (object)oTrayDetailMovement.Status ?? DBNull.Value)

            };
            var totalList = _procedureWork.ExecStoredProcedure<GetEmployeeDetailsReport_Result>("GetEmployeeDetailsReport @FromDate,@ToDate,@DepartmentId,@EmployeeId,@ClientId,@Status", sqlPara).ToList();
            var sqlPara1 = new SqlParameter[]
            {
                new  SqlParameter("@FromDate",oTrayDetailMovement.FromDate),
                new SqlParameter("@ToDate",oTrayDetailMovement.ToDate),
                new SqlParameter("@DepartmentId",oTrayDetailMovement.DepartmentId),
                new SqlParameter("@EmployeeId",oTrayDetailMovement.EmployeeId),
                new SqlParameter("@ClientId",oTrayDetailMovement.ClientId),

            };
            var total = _procedureWork.ExecStoredProcedure<GetEmployeeDetailsTotalReport_Result>("GetEmployeeDetailsTotalReport @FromDate,@ToDate,@DepartmentId,@EmployeeId,@ClientId", sqlPara1).ToList();
            int Id = 0;

            if (oTrayDetailMovement.Status != null)
            {
                foreach (var item in totalList)
                {
                    totalFileStatus.Add(new EmployeeDetailsReport
                    {
                        Id = ++Id,
                        EmployeeName = item.EmployeeName,
                        ClientShortName = item.ClientShortName,
                        Status = item.Status,
                        FilesAllocatedInProd = item.FilesAllocatedInProd,
                        EstimatedTime = item.EstimatedTime,
                        ProdWorkedTime = item.ProdWorkedTime,
                        ProdBreakTime = item.ProdBreakTime,
                        FilesCompInProd = item.FilesCompInProd,
                        FilesAllocatedInQuality = item.FilesAllocatedInQuality,
                        QualityWorkedTime = item.QualityWorkedTime,
                        FilesCompInQuality = item.FilesCompInQuality,
                        //
                        FilesAllocatedInProof = item.FilesAllocatedInPR,
                        ProofWorkedTime = item.PRWorkedTime,
                        FilesCompInProof = item.FilesCompInPR,
                        //
                        FilesAllocatedInSO = item.FilesAllocatedInSO,
                        SOWorkedTime = item.SOWorkedTime,
                        FilesCompInSO = item.FilesCompInSO,
                        FilesAllocatedInBP = item.FilesAllocatedInBP,
                        BPWorkedTime = item.BPWorkedTime

                    });
                }
            }



            else if (oTrayDetailMovement.EmployeeId == 0 && oTrayDetailMovement.ClientId == 0)
            {
                foreach (var item in total)
                {
                    totalFileStatus.Add(new EmployeeDetailsReport
                    {
                        EmployeeName = item.EmployeeName,
                        ClientShortName = item.ClientShortName,
                        Status = item.Status,
                        FilesAllocatedInProd = item.TotalFilesAllocatedInProd,
                        EstimatedTime = item.TotalEstimatedTime,
                        ProdWorkedTime = item.TotalProdWorkedTime,
                        ProdBreakTime = item.TotalProdBreakTime,
                        FilesCompInProd = item.TotalFilesCompInProd,
                        FilesAllocatedInQuality = item.TotalFilesAllocatedInQuality,
                        QualityWorkedTime = item.TotalQualityWorkedTime,
                        FilesCompInQuality = item.TotalFilesCompInQuality,
                        //
                        FilesAllocatedInProof = item.TotalFilesAllocatedInPR,
                        ProofWorkedTime = item.TotalPRWorkedTime,
                        FilesCompInProof = item.TotalFilesCompInPR,
                        //
                        FilesAllocatedInSO = item.TotalFilesAllocatedInSO,
                        SOWorkedTime = item.TotalSOWorkedTime,
                        FilesCompInSO = item.TotalFilesCompInSO,
                        FilesAllocatedInBP = item.TotalFilesAllocatedInBP,
                        BPWorkedTime = item.TotalBPWorkedTime

                    });

                    foreach (var item1 in totalList)
                    {
                        if (item1.ClientId == item.ClientId)
                        {

                            totalFileStatus.Add(new EmployeeDetailsReport
                            {

                                Id = ++Id,
                                EmployeeName = item1.EmployeeName,
                                ClientShortName = item1.ClientShortName,
                                Status = item1.Status,
                                FilesAllocatedInProd = item1.FilesAllocatedInProd,
                                EstimatedTime = item1.EstimatedTime,
                                ProdWorkedTime = item1.ProdWorkedTime,
                                ProdBreakTime = item1.ProdBreakTime,
                                FilesCompInProd = item1.FilesCompInProd,
                                FilesAllocatedInQuality = item1.FilesAllocatedInQuality,
                                QualityWorkedTime = item1.QualityWorkedTime,
                                FilesCompInQuality = item1.FilesCompInQuality,
                                //
                                FilesAllocatedInProof = item1.FilesAllocatedInPR,
                                ProofWorkedTime = item1.PRWorkedTime,
                                FilesCompInProof = item1.FilesCompInPR,
                                //
                                FilesAllocatedInSO = item1.FilesAllocatedInSO,
                                SOWorkedTime = item1.SOWorkedTime,
                                FilesCompInSO = item1.FilesCompInSO,
                                FilesAllocatedInBP = item1.FilesAllocatedInBP,
                                BPWorkedTime = item1.BPWorkedTime
                            });
                        }
                    }

                }
            }
            else
            {
                foreach (var item in total)
                {
                    totalFileStatus.Add(new EmployeeDetailsReport
                    {
                        EmployeeName = item.EmployeeName,
                        ClientShortName = item.ClientShortName,
                        Status = item.Status,
                        FilesAllocatedInProd = item.TotalFilesAllocatedInProd,
                        EstimatedTime = item.TotalEstimatedTime,
                        ProdWorkedTime = item.TotalProdWorkedTime,
                        ProdBreakTime = item.TotalProdBreakTime,
                        FilesCompInProd = item.TotalFilesCompInProd,
                        FilesAllocatedInQuality = item.TotalFilesAllocatedInQuality,
                        QualityWorkedTime = item.TotalQualityWorkedTime,
                        FilesCompInQuality = item.TotalFilesCompInQuality,
                        //
                        FilesAllocatedInProof = item.TotalFilesAllocatedInPR,
                        ProofWorkedTime = item.TotalPRWorkedTime,
                        FilesCompInProof = item.TotalFilesCompInPR,
                        //
                        FilesAllocatedInSO = item.TotalFilesAllocatedInSO,
                        SOWorkedTime = item.TotalSOWorkedTime,
                        FilesCompInSO = item.TotalFilesCompInSO,
                        FilesAllocatedInBP = item.TotalFilesAllocatedInBP,
                        BPWorkedTime = item.TotalBPWorkedTime

                    });
                    if (oTrayDetailMovement.EmployeeId == 0)
                    {
                        foreach (var item1 in totalList)
                        {
                            if (item1.EmployeeId == item.EmployeeId)
                            {
                                totalFileStatus.Add(new EmployeeDetailsReport
                                {
                                    Id = ++Id,
                                    EmployeeName = item1.EmployeeName,
                                    ClientShortName = item1.ClientShortName,
                                    Status = item1.Status,
                                    FilesAllocatedInProd = item1.FilesAllocatedInProd,
                                    EstimatedTime = item1.EstimatedTime,
                                    ProdWorkedTime = item1.ProdWorkedTime,
                                    ProdBreakTime = item1.ProdBreakTime,
                                    FilesCompInProd = item1.FilesCompInProd,
                                    FilesAllocatedInQuality = item1.FilesAllocatedInQuality,
                                    QualityWorkedTime = item1.QualityWorkedTime,
                                    FilesCompInQuality = item1.FilesCompInQuality,
                                    //
                                    FilesAllocatedInProof = item1.FilesAllocatedInPR,
                                    ProofWorkedTime = item1.PRWorkedTime,
                                    FilesCompInProof = item1.FilesCompInPR,
                                    //
                                    FilesAllocatedInSO = item1.FilesAllocatedInSO,
                                    SOWorkedTime = item1.SOWorkedTime,
                                    FilesCompInSO = item1.FilesCompInSO,
                                    FilesAllocatedInBP = item1.FilesAllocatedInBP,
                                    BPWorkedTime = item1.BPWorkedTime
                                });
                            }
                        }
                    }
                    else
                    {
                        foreach (var item1 in totalList)
                        {
                            if (item1.ClientId == item.ClientId)
                            {
                                totalFileStatus.Add(new EmployeeDetailsReport
                                {
                                    Id = ++Id,
                                    EmployeeName = item1.EmployeeName,
                                    ClientShortName = item1.ClientShortName,
                                    Status = item1.Status,
                                    FilesAllocatedInProd = item1.FilesAllocatedInProd,
                                    EstimatedTime = item1.EstimatedTime,
                                    ProdWorkedTime = item1.ProdWorkedTime,
                                    ProdBreakTime = item1.ProdBreakTime,
                                    //FilesCompInProd = item1.FilesCompInProd,
                                    FilesAllocatedInQuality = item1.FilesAllocatedInQuality,
                                    QualityWorkedTime = item1.QualityWorkedTime,
                                    FilesCompInQuality = item1.FilesCompInQuality,
                                    //
                                    FilesAllocatedInProof = item1.FilesAllocatedInPR,
                                    ProofWorkedTime = item1.PRWorkedTime,
                                    FilesCompInProof = item1.FilesCompInPR,
                                    //
                                    FilesAllocatedInSO = item1.FilesAllocatedInSO,
                                    SOWorkedTime = item1.SOWorkedTime,
                                    FilesCompInSO = item1.FilesCompInSO,
                                    FilesAllocatedInBP = item1.FilesAllocatedInBP,
                                    BPWorkedTime = item1.BPWorkedTime
                                });

                            }

                        }

                    }

                }
            }
            return totalFileStatus;
        }

        public Object GetEmployeeWiseGrandTotalList(EmployeeWiseReport oTrayDetailMovement)
        {
            var totalFileStatus = new List<EmployeeDetailsReport>();
            var sqlPara = new SqlParameter[]
            {
                new  SqlParameter("@FromDate",oTrayDetailMovement.FromDate),
                new SqlParameter("@ToDate",oTrayDetailMovement.ToDate),
                new SqlParameter("@DepartmentId",oTrayDetailMovement.DepartmentId),
                new SqlParameter("@EmployeeId",oTrayDetailMovement.EmployeeId),
                new SqlParameter("@ClientId",oTrayDetailMovement.ClientId),
                new SqlParameter("@Status", (object)oTrayDetailMovement.Status ?? DBNull.Value)

            };
            var totalList = _procedureWork.ExecStoredProcedure<GetEmployeeGrandDetailsReport_Result>("GetEmployeeGrandDetailsReport @FromDate,@ToDate,@DepartmentId,@EmployeeId,@ClientId,@Status", sqlPara).ToList();
            var sqlPara1 = new SqlParameter[]
            {
                new  SqlParameter("@FromDate",oTrayDetailMovement.FromDate),
                new SqlParameter("@ToDate",oTrayDetailMovement.ToDate),
                new SqlParameter("@DepartmentId",oTrayDetailMovement.DepartmentId),
                new SqlParameter("@EmployeeId",oTrayDetailMovement.EmployeeId),
                new SqlParameter("@ClientId",oTrayDetailMovement.ClientId),

            };
            var total = _procedureWork.ExecStoredProcedure<GetEmployeeGrandDetailsTotalReport_Result>("GetEmployeeGrandDetailsTotalReport @FromDate,@ToDate,@DepartmentId,@EmployeeId,@ClientId", sqlPara1).ToList();

            if (oTrayDetailMovement.Status != null)
            {
                foreach (var item in total)
                {
                    totalFileStatus.Add(new EmployeeDetailsReport
                    {

                        Status = item.Status,
                        FilesAllocatedInProd = item.TotalFilesAllocatedInProd,
                        EstimatedTime = item.TotalEstimatedTime,
                        ProdWorkedTime = item.TotalProdWorkedTime,
                        ProdBreakTime = item.TotalProdBreakTime,
                        FilesCompInProd = item.TotalFilesCompInProd,
                        FilesAllocatedInQuality = item.TotalFilesAllocatedInQuality,
                        //
                        FilesAllocatedInProof = item.TotalFilesAllocatedInPR,
                        ProofWorkedTime = item.TotalPRWorkedTime,
                        FilesCompInProof = item.TotalFilesCompInPR,
                        //
                        QualityWorkedTime = item.TotalQualityWorkedTime,
                        FilesCompInQuality = item.TotalFilesCompInQuality,
                        FilesAllocatedInSO = item.TotalFilesAllocatedInSO,
                        SOWorkedTime = item.TotalSOWorkedTime,
                        FilesCompInSO = item.TotalFilesCompInSO,
                        FilesAllocatedInBP = item.TotalFilesAllocatedInBP,
                        BPWorkedTime = item.TotalBPWorkedTime

                    });

                    foreach (var item1 in totalList)
                    {

                        totalFileStatus.Add(new EmployeeDetailsReport
                        {

                            Status = item1.Status,
                            FilesAllocatedInProd = item1.TotalFilesAllocatedInProd,
                            EstimatedTime = item1.TotalEstimatedTime,
                            ProdWorkedTime = item1.TotalProdWorkedTime,
                            ProdBreakTime = item1.TotalProdBreakTime,
                            FilesCompInProd = item1.TotalFilesCompInProd,
                            FilesAllocatedInQuality = item1.TotalFilesAllocatedInQuality,
                            QualityWorkedTime = item1.TotalQualityWorkedTime,
                            FilesCompInQuality = item1.TotalFilesCompInQuality,
                            //
                            FilesAllocatedInProof = item1.TotalFilesAllocatedInPR,
                            ProofWorkedTime = item1.TotalPRWorkedTime,
                            FilesCompInProof = item1.TotalFilesCompInPR,
                            //
                            FilesAllocatedInSO = item1.TotalFilesAllocatedInSO,
                            SOWorkedTime = item1.TotalSOWorkedTime,
                            FilesCompInSO = item1.TotalFilesCompInSO,
                            FilesAllocatedInBP = item1.TotalFilesAllocatedInBP,
                            BPWorkedTime = item1.TotalBPWorkedTime
                        });
                    }

                }
            }


            else if (oTrayDetailMovement.ClientId == 0 && oTrayDetailMovement.EmployeeId == 0)
            {

                foreach (var item in total)
                {
                    totalFileStatus.Add(new EmployeeDetailsReport
                    {

                        Status = item.Status,
                        FilesAllocatedInProd = item.TotalFilesAllocatedInProd,
                        EstimatedTime = item.TotalEstimatedTime,
                        ProdWorkedTime = item.TotalProdWorkedTime,
                        ProdBreakTime = item.TotalProdBreakTime,
                        FilesCompInProd = item.TotalFilesCompInProd,
                        FilesAllocatedInQuality = item.TotalFilesAllocatedInQuality,
                        QualityWorkedTime = item.TotalQualityWorkedTime,
                        FilesCompInQuality = item.TotalFilesCompInQuality,
                        //
                        FilesAllocatedInProof = item.TotalFilesAllocatedInPR,
                        ProofWorkedTime = item.TotalPRWorkedTime,
                        FilesCompInProof = item.TotalFilesCompInPR,
                        //
                        FilesAllocatedInSO = item.TotalFilesAllocatedInSO,
                        SOWorkedTime = item.TotalSOWorkedTime,
                        FilesCompInSO = item.TotalFilesCompInSO,
                        FilesAllocatedInBP = item.TotalFilesAllocatedInBP,
                        BPWorkedTime = item.TotalBPWorkedTime

                    });



                    foreach (var item1 in totalList)
                    {

                        totalFileStatus.Add(new EmployeeDetailsReport
                        {

                            Status = item1.Status,
                            FilesAllocatedInProd = item1.TotalFilesAllocatedInProd,
                            EstimatedTime = item1.TotalEstimatedTime,
                            ProdWorkedTime = item1.TotalProdWorkedTime,
                            ProdBreakTime = item1.TotalProdBreakTime,
                            FilesCompInProd = item1.TotalFilesCompInProd,
                            FilesAllocatedInQuality = item1.TotalFilesAllocatedInQuality,
                            QualityWorkedTime = item1.TotalQualityWorkedTime,
                            FilesCompInQuality = item1.TotalFilesCompInQuality,
                            //
                            FilesAllocatedInProof = item1.TotalFilesAllocatedInPR,
                            ProofWorkedTime = item1.TotalPRWorkedTime,
                            FilesCompInProof = item1.TotalFilesCompInPR,
                            //
                            FilesAllocatedInSO = item1.TotalFilesAllocatedInSO,
                            SOWorkedTime = item1.TotalSOWorkedTime,
                            FilesCompInSO = item1.TotalFilesCompInSO,
                            FilesAllocatedInBP = item1.TotalFilesAllocatedInBP,
                            BPWorkedTime = item1.TotalBPWorkedTime
                        });
                    }

                }
            }
            else
            {

                foreach (var item in total)
                {
                    totalFileStatus.Add(new EmployeeDetailsReport
                    {
                        //EmployeeName = item.EmployeeName,
                        //ClientShortName = item.ClientShortName,
                        Status = item.Status,
                        FilesAllocatedInProd = item.TotalFilesAllocatedInProd,
                        EstimatedTime = item.TotalEstimatedTime,
                        ProdWorkedTime = item.TotalProdWorkedTime,
                        ProdBreakTime = item.TotalProdBreakTime,
                        FilesCompInProd = item.TotalFilesCompInProd,
                        FilesAllocatedInQuality = item.TotalFilesAllocatedInQuality,
                        QualityWorkedTime = item.TotalQualityWorkedTime,
                        FilesCompInQuality = item.TotalFilesCompInQuality,
                        //
                        FilesAllocatedInProof = item.TotalFilesAllocatedInPR,
                        ProofWorkedTime = item.TotalPRWorkedTime,
                        FilesCompInProof = item.TotalFilesCompInPR,
                        //
                        FilesAllocatedInSO = item.TotalFilesAllocatedInSO,
                        SOWorkedTime = item.TotalSOWorkedTime,
                        FilesCompInSO = item.TotalFilesCompInSO,
                        FilesAllocatedInBP = item.TotalFilesAllocatedInBP,
                        BPWorkedTime = item.TotalBPWorkedTime

                    });
                    if (oTrayDetailMovement.ClientId == 0)
                    {
                        foreach (var item1 in totalList)
                        {
                            totalFileStatus.Add(new EmployeeDetailsReport
                            {
                                //EmployeeName = item1.EmployeeName,
                                //ClientShortName = item1.ClientShortName,
                                Status = item1.Status,
                                FilesAllocatedInProd = item1.TotalFilesAllocatedInProd,
                                EstimatedTime = item1.TotalEstimatedTime,
                                ProdWorkedTime = item1.TotalProdWorkedTime,
                                ProdBreakTime = item1.TotalProdBreakTime,
                                FilesCompInProd = item1.TotalFilesCompInProd,
                                FilesAllocatedInQuality = item1.TotalFilesAllocatedInQuality,
                                QualityWorkedTime = item1.TotalQualityWorkedTime,
                                FilesCompInQuality = item1.TotalFilesCompInQuality,
                                //
                                FilesAllocatedInProof = item1.TotalFilesAllocatedInPR,
                                ProofWorkedTime = item1.TotalPRWorkedTime,
                                FilesCompInProof = item1.TotalFilesCompInPR,
                                //
                                FilesAllocatedInSO = item1.TotalFilesAllocatedInSO,
                                SOWorkedTime = item1.TotalSOWorkedTime,
                                FilesCompInSO = item1.TotalFilesCompInSO,
                                FilesAllocatedInBP = item1.TotalFilesAllocatedInBP,
                                BPWorkedTime = item1.TotalBPWorkedTime
                            });
                        }
                    }
                    else
                    {
                        foreach (var item1 in totalList)
                        {
                            totalFileStatus.Add(new EmployeeDetailsReport
                            {
                                //EmployeeName = item1.EmployeeName,
                                //ClientShortName = item1.ClientShortName,
                                Status = item1.Status,
                                FilesAllocatedInProd = item1.TotalFilesAllocatedInProd,
                                EstimatedTime = item1.TotalEstimatedTime,
                                ProdWorkedTime = item1.TotalProdWorkedTime,
                                ProdBreakTime = item1.TotalProdBreakTime,
                                FilesCompInProd = item1.TotalFilesCompInProd,
                                FilesAllocatedInQuality = item1.TotalFilesAllocatedInQuality,
                                QualityWorkedTime = item1.TotalQualityWorkedTime,
                                FilesCompInQuality = item1.TotalFilesCompInQuality,
                                //
                                FilesAllocatedInProof = item1.TotalFilesAllocatedInPR,
                                ProofWorkedTime = item1.TotalPRWorkedTime,
                                FilesCompInProof = item1.TotalFilesCompInPR,
                                //
                                FilesAllocatedInSO = item1.TotalFilesAllocatedInSO,
                                SOWorkedTime = item1.TotalSOWorkedTime,
                                FilesCompInSO = item1.TotalFilesCompInSO,
                                FilesAllocatedInBP = item1.TotalFilesAllocatedInBP,
                                BPWorkedTime = item1.TotalBPWorkedTime
                            });
                        }
                    }
                }
            }
            return totalFileStatus;
        }

        //GetHoldStatusDetailsReport
        public Object GetHoldStatusDetailsReport(EmployeeWiseReport oTrayDetailMovement)
        {

            // return _db.GetEmployeeGrandDetailsReport_Hold(oTrayDetailMovement.FromDate, oTrayDetailMovement.ToDate, oTrayDetailMovement.DepartmentId, oTrayDetailMovement.EmployeeId, oTrayDetailMovement.ClientId, oTrayDetailMovement.Status).ToList();
            var sqlPara = new SqlParameter[]
            {
                new SqlParameter("@FromDate",oTrayDetailMovement.FromDate),
                new SqlParameter("@ToDate", oTrayDetailMovement.ToDate),
                new SqlParameter("@DepartmentId",oTrayDetailMovement.DepartmentId),
                new SqlParameter("@EmployeeId",oTrayDetailMovement.EmployeeId),
                new SqlParameter("@ClientId",oTrayDetailMovement.ClientId),
                new SqlParameter("@Status", oTrayDetailMovement.Status)
            };
            return _procedureWork.ExecStoredProcedure<GetEmployeeGrandDetailsReport_Hold_Result>("GetEmployeeGrandDetailsReport_Hold @FromDate,@ToDate,@DepartmentId,@EmployeeId,@ClientId,@Status", sqlPara).ToList();

        }

        public Object GetAllDropDownList()
        {
            var customerclass = _tableWork.CustomerClassificationRepository.Get(x => x.IsDeleted == false).ToArray();
            var getemployee = _tableWork.EmployeeRepository.Get(x => x.IsDeleted == false && (x.DateOfResignation == null || x.ResignReasons == null)).OrderBy(x => x.EmployeeName).AsQueryable();
            var customers = _tableWork.CustomerRepository.Get(x => x.IsDeleted == false && x.IsBlacklisted == false).Select(x => new { Id = x.Id, ShortName = x.ShortName, Name = x.Name }).ToArray();

            var DetailList = new
            {
                ClassificationList = customerclass,
                EmployeeList = getemployee,
                CustomerList = customers,
            };
            return DetailList;
        }

        public Object GetEWRList(EmployeeWiseReport oTrayDetailMovement)
        {
            var clienttraylist = new List<EmployeeReport>();
            var clientidList = oTrayDetailMovement.SelectedClientId;
            var employeeidList = oTrayDetailMovement.SelectedEmployeeId;
            var customercollection = from TrayDetailMovements in _unitWork.TrayDetailMovement.Include("Employee")
                                     where clientidList.Contains(TrayDetailMovements.ClientId.Value)
                                     where TrayDetailMovements.JobDate >= oTrayDetailMovement.FileReceivedDateFrom &&
                                     TrayDetailMovements.JobDate <= oTrayDetailMovement.FileReceivedDateTo
                                     select TrayDetailMovements;
            return customercollection;
        }
        //DPR Report
        public Object GetDailyProductionStatusReport(DailyReportViewModel GetFileStatusReport)
        {
            var sqlPara = new SqlParameter[]
            {
                new  SqlParameter("@ClientId",GetFileStatusReport.ClientId),
                new SqlParameter("@Department", (object)GetFileStatusReport.Department ?? DBNull.Value),
                new SqlParameter("@FromDate",GetFileStatusReport.FromDate),
                new SqlParameter("@ToDate",GetFileStatusReport.ToDate)

            };
            return _procedureWork.ExecStoredProcedure<DailyProductionReportRef_Result>("DailyProductionReportRef @ClientId,@Department,@FromDate,@ToDate", sqlPara).ToList();

        }
        //SingleTableDPR start        
        public IEnumerable<SP_SingleTableDPR_Result> SP_SingleTableDPR(DPRViewModel dprvm)
        {
            var result = _unitprocedure.SP_SingleTableDPR(dprvm.FromDate, dprvm.ToDate, dprvm.TrayDetailName, dprvm.CustomerType, dprvm.Classification, dprvm.DepartmentId, dprvm.CustomerId, dprvm.Status, dprvm.EmployeeId).AsQueryable();
            return result;
        }
        //SingleTableDPR end

        public IEnumerable<CustomerClassification> GetCustomerClass()
        {
          
                var result = adb.CustomerClassifications.Where(x => x.IsDeleted == false).ToList();
                return result;
       
        }

        public Object GetQueryTrackerSummary(DailyReportViewModel GetQueryTrackerSummary)
        {
            var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@FromDate",  GetQueryTrackerSummary.FromDate),
                        new SqlParameter("@ToDate", GetQueryTrackerSummary.ToDate),
                        new SqlParameter("@ClientId", GetQueryTrackerSummary.ClientId),
                        new SqlParameter("@DepartmentId", GetQueryTrackerSummary.DepartmentId)
                    };

            var resultFromSProc = _procedureWork.ExecStoredProcedure<QueryTrackerSummaryDetails_Result>("QueryTrackerSummaryDetails @FromDate,@ToDate,@ClientId,@DepartmentId", sqlpara).ToList();
            return resultFromSProc;

        }

        public Object DailyProductionReport(ViewDailyProductionReport param)
        {
            var dailyProductionReports = new List<ViewDailyOperationsReport>();
            if (param.DepartmentId == 0)
            {
                dailyProductionReports = _viewWork.ViewDailyOperationsReportRepository.Get(x => (x.FileReceivedDate >= param.FileReceivedDate && x.FileReceivedDate <= param.JobDate)).ToList();
            }
            else
            {
                dailyProductionReports = _viewWork.ViewDailyOperationsReportRepository.Get(x => (x.FileReceivedDate >= param.FileReceivedDate && x.FileReceivedDate <= param.JobDate) && x.DepartmentId == param.DepartmentId).ToList();
            }
            foreach (var item in dailyProductionReports)
            {
                var ccteamupload = _tableWork.JobOrderRepository.GetAllVal(x => x.Employee).Where(x => x.JobId == item.JobId && x.DateofUpload != null).OrderByDescending(x => x.Id).FirstOrDefault();
                var ccteamIntake = _tableWork.ProcessWorkFlowTranRepository.GetAllVal(x => x.CreatedByNavigation).Where(x => x.Wfmid == item.Wfmid).OrderBy(x => x.Id).FirstOrDefault();
                item.Ccintake = ccteamIntake.CreatedByNavigation.EmployeeName;
                if (ccteamupload == null)
                {
                    item.Ccupload = null;
                    item.FinalStatus = "Completed";
                }
                else
                {
                    item.Ccupload = ccteamupload.Employee.EmployeeName;
                    item.FinalStatus = "Uploaded";
                }

                var qcError = _tableWork.ProcessWorkFlowTranRepository.Get(x => x.Wfmid == item.Wfmid && x.StatusId == 11 && x.PreviousProcessId == 5).OrderByDescending(x => x.Id).FirstOrDefault();
                if (qcError != null)
                {
                    item.QcerrorStatus = "Error";
                }
                else
                {
                    item.QcerrorStatus = "No-Error";
                }

                var proofError = _tableWork.ProcessWorkFlowTranRepository.Get(x => x.Wfmid == item.Wfmid && x.StatusId == 11 && x.PreviousProcessId == 7).OrderByDescending(x => x.Id).FirstOrDefault();
                if (proofError != null)
                {
                    item.PrerrorStatus = "Error";
                }
                else
                {
                    item.PrerrorStatus = "No-Error";
                }
                var acceptedProof = _tableWork.ProcessWorkFlowTranRepository.Get(x => x.Wfmid == item.Wfmid && x.PreviousProcessId == 5 && (x.StatusId == 17 || x.StatusId == 18)).OrderByDescending(x => x.Id).FirstOrDefault();
                if (acceptedProof != null)
                {
                    if (acceptedProof.StatusId == 18)
                    {
                        item.ProofAccepted = "No";
                    }
                    else
                    {
                        item.ProofAccepted = "Yes";
                    }
                }
                else
                {
                    item.ProofAccepted = null;
                }

            }
            var result = new
            {
                ScopeDetails = dailyProductionReports
            };
            return result;
        }
        public IEnumerable<GetEmployee> GetAssignedEmployees(ViewJobAllocation CustomerJobType)
        {
            var processWorkFlowTran = _tableWork.ProcessWorkFlowTranRepository.GetSingle(x => x.Id == CustomerJobType.TranId);
            var getEmployees = new List<ViewGetEmployeesForPendingJob>();
            if (CustomerJobType.ProcessId == null)
            {
                //getEmployees = _db.View_GetEmployeesForPendingJobs.Where(x => x.WFMId == processWorkFlowTran.WFMId && x.IsActive == true).ToList();
                getEmployees = _viewWork.ViewGetEmployeesForPendingJobsRepository.Get(x => x.Jid == processWorkFlowTran.Jid && x.IsActive == true).ToList();
            }
            else
            {
                //getEmployees = _db.View_GetEmployeesForPendingJobs.Where(x => x.WFMId == processWorkFlowTran.WFMId && x.ProcessId == CustomerJobType.ProcessId && x.IsActive == true).ToList();
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
        //Dashboard Sample
        //.............................
        //CurrentDashboardSummary By Division
        public IEnumerable<GetCurrentDashSummaryViewModel> GetCurrentDashboardSummary()
        { 
          
            DateTime systemdate = DateTime.Now;
            //
            DateTime getcurrentdt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(systemdate, TimeZoneInfo.Local.Id, "India Standard Time");
            //
            DateTime Scurrentdate = getcurrentdt.Date;
            //string Scurrentdate = getcurrentdt.ToString("yyyy-MM-dd");
            string Scurrenttime = getcurrentdt.ToString("HH:mm:ss");
            string onlyDfromtime = "06:00:00";
            string onlyDtotime = "22:30:00";
            string onlyNfromtime = "22:31:00";
            string onlyNtotime = "05:59:00";
            DateTime fromtime = new DateTime();
            DateTime totime = new DateTime();
            if ((DateTime.ParseExact(onlyDfromtime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Scurrenttime, "HH:mm:ss", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(onlyDtotime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Scurrenttime, "HH:mm:ss", CultureInfo.InvariantCulture)))
            {
                //day
                fromtime = Scurrentdate.AddHours(00).AddMinutes(30).AddSeconds(00);
                totime = getcurrentdt.ToUniversalTime();
            }
            else
            {
                //night
                string checktime = "00:00:00";
                if (DateTime.ParseExact(checktime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact(Scurrenttime, "HH:mm:ss", CultureInfo.InvariantCulture))
                {
                    fromtime = Scurrentdate.AddDays(-1).AddHours(17).AddMinutes(01).AddSeconds(00);
                }
                else
                {
                    fromtime = Scurrentdate.AddHours(17).AddMinutes(01).AddSeconds(00);
                }
                totime = getcurrentdt.ToUniversalTime();
            }
            //fromtime = new DateTime(2018,11,23,00,00,00);
            //totime = new DateTime(2018,11,23,23,59,00);
            int? glbempid = 0;
            int glbdeptid = 0;
            var GetDashSummaryList = new List<GetCurrentDashSummaryViewModel>();
            var GetDashSummarySingle = new GetCurrentDashSummarySingleViewModel();
            var tdmttble = (from tdmt in db.TrayDetailMovementTrans
                            join pwfm in db.ProcessWorkFlowMasters on tdmt.Wfmid equals pwfm.Id
                            join jo in db.JobOrders on pwfm.JobId equals jo.Id
                            join cvd in db.CustomerVsDivisions on tdmt.ClientId equals cvd.CustomerId
                            where ((tdmt.UpdatedUtc >= fromtime && tdmt.UpdatedUtc <= totime) && (tdmt.Process == "Production" && tdmt.Status == "Completed") && (jo.JobStatusId == 9 || jo.JobStatusId == 5) && tdmt.IsDeleted == false)
                            select new { tdmt.EmployeeId, tdmt.Wfmid, jo.JobStatusId, jo.SpecialPrice, cvd.DivisionId }).ToList();
            int tdmttblcount = tdmttble.Count();
            if (tdmttblcount > 0)
            {
                var grpempid = tdmttble.GroupBy(x => x.EmployeeId).ToList();
                foreach (var itememp in grpempid)
                {
                    glbempid = itememp.FirstOrDefault().EmployeeId;
                    var emptbl = db.Employees.Where(x => x.EmployeeId == glbempid && (x.DepartmentId == 1 || x.DepartmentId == 2) && x.IsDeleted == false).ToList();//empid and deptid
                    int emptblcount = emptbl.Count();
                    if (emptblcount > 0)
                    {
                        if (emptblcount == 1)
                        {
                            glbdeptid = emptbl.FirstOrDefault().DepartmentId;
                            var empdivtbl = db.EmployeeVsDivisions.Where(x => x.EmployeeId == glbempid && x.DeptId == glbdeptid && x.IsDeleted == false).ToList();
                            int empdivtblcount = empdivtbl.Count();
                            if (empdivtblcount > 0)
                            {
                                foreach (var itemevd in empdivtbl)
                                {
                                    var holddivid = itemevd.DivisionId;
                                    if (holddivid != null)
                                    {
                                        var divtbl = db.Divisions.FirstOrDefault(x => x.Id == holddivid);
                                        if (holddivid == 1)
                                        {
                                            GetDashSummarySingle.TotalCount1 += 1;

                                            GetDashSummarySingle.TotalFresh1 += tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 9 && x.DivisionId == 1).Count();

                                            GetDashSummarySingle.TotalRevision1 += tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 5 && x.DivisionId == 1).Count();

                                            GetDashSummarySingle.TotalCompletedCount1 += tdmttble.Where(x => x.EmployeeId == glbempid && x.DivisionId == 1).Count();

                                            GetDashSummarySingle.TotalRevenue1 = Convert.ToDecimal(tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 9 && x.DivisionId == 1 && x.SpecialPrice != null).Sum(x => x.SpecialPrice.Value)) + Convert.ToDecimal(GetDashSummarySingle.TotalRevenue1);
                                        }
                                        else if (holddivid == 2)
                                        {
                                            GetDashSummarySingle.TotalCount2 += 1;

                                            GetDashSummarySingle.TotalFresh2 += tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 9 && x.DivisionId == 2).Count();

                                            GetDashSummarySingle.TotalRevision2 += tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 5 && x.DivisionId == 2).Count();

                                            GetDashSummarySingle.TotalCompletedCount2 += tdmttble.Where(x => x.EmployeeId == glbempid && x.DivisionId == 2).Count();

                                            GetDashSummarySingle.TotalRevenue2 = Convert.ToDecimal(tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 9 && x.DivisionId == 2 && x.SpecialPrice != null).Sum(x => x.SpecialPrice.Value)) + Convert.ToDecimal(GetDashSummarySingle.TotalRevenue2);
                                        }
                                        else if (holddivid == 3)
                                        {
                                            GetDashSummarySingle.TotalCount3 += 1;

                                            GetDashSummarySingle.TotalFresh3 += tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 9 && x.DivisionId == 3).Count();

                                            GetDashSummarySingle.TotalRevision3 += tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 5 && x.DivisionId == 3).Count();

                                            GetDashSummarySingle.TotalCompletedCount3 += tdmttble.Where(x => x.EmployeeId == glbempid && x.DivisionId == 3).Count();

                                            GetDashSummarySingle.TotalRevenue3 = Convert.ToDecimal(tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 9 && x.DivisionId == 3 && x.SpecialPrice != null).Sum(x => x.SpecialPrice.Value)) + Convert.ToDecimal(GetDashSummarySingle.TotalRevenue3);
                                        }
                                        else if (holddivid == 4)
                                        {
                                            GetDashSummarySingle.TotalCount4 += 1;

                                            GetDashSummarySingle.TotalFresh4 += tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 9 && x.DivisionId == 4).Count();

                                            GetDashSummarySingle.TotalRevision4 += tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 5 && x.DivisionId == 4).Count();

                                            GetDashSummarySingle.TotalCompletedCount4 += tdmttble.Where(x => x.EmployeeId == glbempid && x.DivisionId == 4).Count();

                                            GetDashSummarySingle.TotalRevenue4 = Convert.ToDecimal(tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 9 && x.DivisionId == 4 && x.SpecialPrice != null).Sum(x => x.SpecialPrice.Value)) + Convert.ToDecimal(GetDashSummarySingle.TotalRevenue4);
                                        }
                                        else if (holddivid == 5)
                                        {
                                            GetDashSummarySingle.TotalCount5 += 1;

                                            GetDashSummarySingle.TotalFresh5 += tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 9 && x.DivisionId == 5).Count();

                                            GetDashSummarySingle.TotalRevision5 += tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 5 && x.DivisionId == 5).Count();

                                            GetDashSummarySingle.TotalCompletedCount5 += tdmttble.Where(x => x.EmployeeId == glbempid && x.DivisionId == 5).Count();

                                            GetDashSummarySingle.TotalRevenue5 = Convert.ToDecimal(tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 9 && x.DivisionId == 5 && x.SpecialPrice != null).Sum(x => x.SpecialPrice.Value)) + Convert.ToDecimal(GetDashSummarySingle.TotalRevenue5);
                                        }
                                        else if (holddivid == 6)
                                        {
                                            GetDashSummarySingle.TotalCount6 += 1;

                                            GetDashSummarySingle.TotalFresh6 += tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 9 && x.DivisionId == 6).Count();

                                            GetDashSummarySingle.TotalRevision6 += tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 5 && x.DivisionId == 6).Count();

                                            GetDashSummarySingle.TotalCompletedCount6 += tdmttble.Where(x => x.EmployeeId == glbempid && x.DivisionId == 6).Count();

                                            GetDashSummarySingle.TotalRevenue6 = Convert.ToDecimal(tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 9 && x.DivisionId == 6 && x.SpecialPrice != null).Sum(x => x.SpecialPrice.Value)) + Convert.ToDecimal(GetDashSummarySingle.TotalRevenue6);
                                        }
                                        else if (holddivid == 7)
                                        {
                                            GetDashSummarySingle.TotalCount7 += 1;

                                            GetDashSummarySingle.TotalFresh7 += tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 9 && x.DivisionId == 7).Count();

                                            GetDashSummarySingle.TotalRevision7 += tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 5 && x.DivisionId == 7).Count();

                                            GetDashSummarySingle.TotalCompletedCount7 += tdmttble.Where(x => x.EmployeeId == glbempid && x.DivisionId == 7).Count();

                                            GetDashSummarySingle.TotalRevenue7 = Convert.ToDecimal(tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 9 && x.DivisionId == 7 && x.SpecialPrice != null).Sum(x => x.SpecialPrice.Value)) + Convert.ToDecimal(GetDashSummarySingle.TotalRevenue7);
                                        }
                                        else if (holddivid == 8)
                                        {
                                            GetDashSummarySingle.TotalCount8 += 1;

                                            GetDashSummarySingle.TotalFresh8 += tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 9 && x.DivisionId == 8).Count();

                                            GetDashSummarySingle.TotalRevision8 += tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 5 && x.DivisionId == 8).Count();

                                            GetDashSummarySingle.TotalCompletedCount8 += tdmttble.Where(x => x.EmployeeId == glbempid && x.DivisionId == 8).Count();

                                            GetDashSummarySingle.TotalRevenue8 = Convert.ToDecimal(tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 9 && x.DivisionId == 8 && x.SpecialPrice != null).Sum(x => x.SpecialPrice.Value)) + Convert.ToDecimal(GetDashSummarySingle.TotalRevenue8);
                                        }
                                        else if (holddivid == 9)
                                        {
                                            GetDashSummarySingle.TotalCount9 += 1;

                                            GetDashSummarySingle.TotalFresh9 += tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 9 && x.DivisionId == 9).Count();

                                            GetDashSummarySingle.TotalRevision9 += tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 5 && x.DivisionId == 9).Count();

                                            GetDashSummarySingle.TotalCompletedCount9 += tdmttble.Where(x => x.EmployeeId == glbempid && x.DivisionId == 9).Count();

                                            GetDashSummarySingle.TotalRevenue9 = Convert.ToDecimal(tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 9 && x.DivisionId == 9 && x.SpecialPrice != null).Sum(x => x.SpecialPrice.Value)) + Convert.ToDecimal(GetDashSummarySingle.TotalRevenue9);
                                        }
                                        else if (holddivid == 10)
                                        {
                                            GetDashSummarySingle.TotalCount10 += 1;

                                            GetDashSummarySingle.TotalFresh10 += tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 9 && x.DivisionId == 10).Count();

                                            GetDashSummarySingle.TotalRevision10 += tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 5 && x.DivisionId == 10).Count();

                                            GetDashSummarySingle.TotalCompletedCount10 += tdmttble.Where(x => x.EmployeeId == glbempid && x.DivisionId == 10).Count();

                                            GetDashSummarySingle.TotalRevenue10 = Convert.ToDecimal(tdmttble.Where(x => x.EmployeeId == glbempid && x.JobStatusId == 9 && x.DivisionId == 10 && x.SpecialPrice != null).Sum(x => x.SpecialPrice.Value)) + Convert.ToDecimal(GetDashSummarySingle.TotalRevenue10);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                var comptdivtbl = db.Divisions.ToList();
                foreach (var itemdiv in comptdivtbl)
                {
                    if (itemdiv.Id == 1)
                    {
                        GetDashSummaryList.Add(new GetCurrentDashSummaryViewModel
                        {
                            DivisionId = 1,
                            Division = itemdiv.DivisionName,
                            TLName = db.Employees.FirstOrDefault(x => x.EmployeeId == itemdiv.TeamLeadEmpId).EmployeeName,
                            Norms = itemdiv.Capacity * GetDashSummarySingle.TotalCount1,
                            TotalTeamStrength = GetDashSummarySingle.TotalCount1,
                            //TotalCapacity = itemdiv.Capacity * GetDashSummarySingle.TotalPresent1,
                            FreshCount = GetDashSummarySingle.TotalFresh1,
                            RevisionCount = GetDashSummarySingle.TotalRevision1,
                            TotalCompletedCount = GetDashSummarySingle.TotalCompletedCount1,
                            TotalDeficitExceed = ((GetDashSummarySingle.TotalFresh1) - (itemdiv.Capacity * GetDashSummarySingle.TotalCount1)),
                            TotalRevenueUSD = GetDashSummarySingle.TotalRevenue1,
                            TotalRevenueINR = 70 * GetDashSummarySingle.TotalRevenue1,
                            CurrentDatetime = getcurrentdt
                        });
                    }
                    else if (itemdiv.Id == 2)
                    {
                        GetDashSummaryList.Add(new GetCurrentDashSummaryViewModel
                        {
                            DivisionId = 2,
                            Division = itemdiv.DivisionName,
                            TLName = db.Employees.FirstOrDefault(x => x.EmployeeId == itemdiv.TeamLeadEmpId).EmployeeName,
                            Norms = itemdiv.Capacity * GetDashSummarySingle.TotalCount2,
                            TotalTeamStrength = GetDashSummarySingle.TotalCount2,
                            //TotalCapacity = itemdiv.Capacity * GetDashSummarySingle.TotalPresent2,
                            FreshCount = GetDashSummarySingle.TotalFresh2,
                            RevisionCount = GetDashSummarySingle.TotalRevision2,
                            TotalCompletedCount = GetDashSummarySingle.TotalCompletedCount2,
                            TotalDeficitExceed = ((GetDashSummarySingle.TotalFresh2) - (itemdiv.Capacity * GetDashSummarySingle.TotalCount2)),
                            TotalRevenueUSD = GetDashSummarySingle.TotalRevenue2,
                            TotalRevenueINR = 70 * GetDashSummarySingle.TotalRevenue2,
                            CurrentDatetime = getcurrentdt
                        });
                    }
                    else if (itemdiv.Id == 3)
                    {
                        GetDashSummaryList.Add(new GetCurrentDashSummaryViewModel
                        {
                            DivisionId = 3,
                            Division = itemdiv.DivisionName,
                            TLName = db.Employees.FirstOrDefault(x => x.EmployeeId == itemdiv.TeamLeadEmpId).EmployeeName,
                            Norms = itemdiv.Capacity * GetDashSummarySingle.TotalCount3,
                            TotalTeamStrength = GetDashSummarySingle.TotalCount3,
                            //TotalCapacity = itemdiv.Capacity * GetDashSummarySingle.TotalPresent3,
                            FreshCount = GetDashSummarySingle.TotalFresh3,
                            RevisionCount = GetDashSummarySingle.TotalRevision3,
                            TotalCompletedCount = GetDashSummarySingle.TotalCompletedCount3,
                            TotalDeficitExceed = ((GetDashSummarySingle.TotalFresh3) - (itemdiv.Capacity * GetDashSummarySingle.TotalCount3)),
                            TotalRevenueUSD = GetDashSummarySingle.TotalRevenue3,
                            TotalRevenueINR = 70 * GetDashSummarySingle.TotalRevenue3,
                            CurrentDatetime = getcurrentdt
                        });
                    }
                    else if (itemdiv.Id == 4)
                    {
                        GetDashSummaryList.Add(new GetCurrentDashSummaryViewModel
                        {
                            DivisionId = 4,
                            Division = itemdiv.DivisionName,
                            TLName = db.Employees.FirstOrDefault(x => x.EmployeeId == itemdiv.TeamLeadEmpId).EmployeeName,
                            Norms = itemdiv.Capacity * GetDashSummarySingle.TotalCount4,
                            TotalTeamStrength = GetDashSummarySingle.TotalCount4,
                            //TotalCapacity = itemdiv.Capacity * GetDashSummarySingle.TotalPresent4,
                            FreshCount = GetDashSummarySingle.TotalFresh4,
                            RevisionCount = GetDashSummarySingle.TotalRevision4,
                            TotalCompletedCount = GetDashSummarySingle.TotalCompletedCount4,
                            TotalDeficitExceed = ((GetDashSummarySingle.TotalFresh4) - (itemdiv.Capacity * GetDashSummarySingle.TotalCount4)),
                            TotalRevenueUSD = GetDashSummarySingle.TotalRevenue4,
                            TotalRevenueINR = 70 * GetDashSummarySingle.TotalRevenue4,
                            CurrentDatetime = getcurrentdt
                        });
                    }
                    else if (itemdiv.Id == 5)
                    {
                        GetDashSummaryList.Add(new GetCurrentDashSummaryViewModel
                        {
                            DivisionId = 5,
                            Division = itemdiv.DivisionName,
                            TLName = db.Employees.FirstOrDefault(x => x.EmployeeId == itemdiv.TeamLeadEmpId).EmployeeName,
                            Norms = itemdiv.Capacity * GetDashSummarySingle.TotalCount5,
                            TotalTeamStrength = GetDashSummarySingle.TotalCount5,
                            //TotalCapacity = itemdiv.Capacity * GetDashSummarySingle.TotalPresent5,
                            FreshCount = GetDashSummarySingle.TotalFresh5,
                            RevisionCount = GetDashSummarySingle.TotalRevision5,
                            TotalCompletedCount = GetDashSummarySingle.TotalCompletedCount5,
                            TotalDeficitExceed = ((GetDashSummarySingle.TotalFresh5) - (itemdiv.Capacity * GetDashSummarySingle.TotalCount5)),
                            TotalRevenueUSD = GetDashSummarySingle.TotalRevenue5,
                            TotalRevenueINR = 70 * GetDashSummarySingle.TotalRevenue5,
                            CurrentDatetime = getcurrentdt
                        });
                    }
                    else if (itemdiv.Id == 6)
                    {
                        GetDashSummaryList.Add(new GetCurrentDashSummaryViewModel
                        {
                            DivisionId = 6,
                            Division = itemdiv.DivisionName,
                            TLName = db.Employees.FirstOrDefault(x => x.EmployeeId == itemdiv.TeamLeadEmpId).EmployeeName,
                            Norms = itemdiv.Capacity * GetDashSummarySingle.TotalCount6,
                            TotalTeamStrength = GetDashSummarySingle.TotalCount6,
                            //TotalCapacity = itemdiv.Capacity * GetDashSummarySingle.TotalPresent6,
                            FreshCount = GetDashSummarySingle.TotalFresh6,
                            RevisionCount = GetDashSummarySingle.TotalRevision6,
                            TotalCompletedCount = GetDashSummarySingle.TotalCompletedCount6,
                            TotalDeficitExceed = ((GetDashSummarySingle.TotalFresh6) - (itemdiv.Capacity * GetDashSummarySingle.TotalCount6)),
                            TotalRevenueUSD = GetDashSummarySingle.TotalRevenue6,
                            TotalRevenueINR = 70 * GetDashSummarySingle.TotalRevenue6,
                            CurrentDatetime = getcurrentdt
                        });
                    }
                    else if (itemdiv.Id == 7)
                    {
                        GetDashSummaryList.Add(new GetCurrentDashSummaryViewModel
                        {
                            DivisionId = 7,
                            Division = itemdiv.DivisionName,
                            TLName = db.Employees.FirstOrDefault(x => x.EmployeeId == itemdiv.TeamLeadEmpId).EmployeeName,
                            Norms = itemdiv.Capacity * GetDashSummarySingle.TotalCount7,
                            TotalTeamStrength = GetDashSummarySingle.TotalCount7,
                            //TotalCapacity = itemdiv.Capacity * GetDashSummarySingle.TotalPresent7,
                            FreshCount = GetDashSummarySingle.TotalFresh7,
                            RevisionCount = GetDashSummarySingle.TotalRevision7,
                            TotalCompletedCount = GetDashSummarySingle.TotalCompletedCount7,
                            TotalDeficitExceed = ((GetDashSummarySingle.TotalFresh7) - (itemdiv.Capacity * GetDashSummarySingle.TotalCount7)),
                            TotalRevenueUSD = GetDashSummarySingle.TotalRevenue7,
                            TotalRevenueINR = 70 * GetDashSummarySingle.TotalRevenue7,
                            CurrentDatetime = getcurrentdt
                        });
                    }
                    else if (itemdiv.Id == 8)
                    {
                        GetDashSummaryList.Add(new GetCurrentDashSummaryViewModel
                        {
                            DivisionId = 8,
                            Division = itemdiv.DivisionName,
                            TLName = db.Employees.FirstOrDefault(x => x.EmployeeId == itemdiv.TeamLeadEmpId).EmployeeName,
                            Norms = itemdiv.Capacity * GetDashSummarySingle.TotalCount8,
                            TotalTeamStrength = GetDashSummarySingle.TotalCount8,
                            //TotalCapacity = itemdiv.Capacity * GetDashSummarySingle.TotalPresent8,
                            FreshCount = GetDashSummarySingle.TotalFresh8,
                            RevisionCount = GetDashSummarySingle.TotalRevision8,
                            TotalCompletedCount = GetDashSummarySingle.TotalCompletedCount8,
                            TotalDeficitExceed = ((GetDashSummarySingle.TotalFresh8) - (itemdiv.Capacity * GetDashSummarySingle.TotalCount8)),
                            TotalRevenueUSD = GetDashSummarySingle.TotalRevenue8,
                            TotalRevenueINR = 70 * GetDashSummarySingle.TotalRevenue8,
                            CurrentDatetime = getcurrentdt
                        });
                    }
                    else if (itemdiv.Id == 9)
                    {
                        GetDashSummaryList.Add(new GetCurrentDashSummaryViewModel
                        {
                            DivisionId = 9,
                            Division = itemdiv.DivisionName,
                            TLName = db.Employees.FirstOrDefault(x => x.EmployeeId == itemdiv.TeamLeadEmpId).EmployeeName,
                            Norms = itemdiv.Capacity * GetDashSummarySingle.TotalCount9,
                            TotalTeamStrength = GetDashSummarySingle.TotalCount9,
                            //TotalCapacity = itemdiv.Capacity * GetDashSummarySingle.TotalPresent9,
                            FreshCount = GetDashSummarySingle.TotalFresh9,
                            RevisionCount = GetDashSummarySingle.TotalRevision9,
                            TotalCompletedCount = GetDashSummarySingle.TotalCompletedCount9,
                            TotalDeficitExceed = ((GetDashSummarySingle.TotalFresh9) - (itemdiv.Capacity * GetDashSummarySingle.TotalCount9)),
                            TotalRevenueUSD = GetDashSummarySingle.TotalRevenue9,
                            TotalRevenueINR = 70 * GetDashSummarySingle.TotalRevenue9,
                            CurrentDatetime = getcurrentdt
                        });
                    }
                    else if (itemdiv.Id == 10)
                    {
                        GetDashSummaryList.Add(new GetCurrentDashSummaryViewModel
                        {
                            DivisionId = 10,
                            Division = itemdiv.DivisionName,
                            TLName = db.Employees.FirstOrDefault(x => x.EmployeeId == itemdiv.TeamLeadEmpId).EmployeeName,
                            Norms = itemdiv.Capacity * GetDashSummarySingle.TotalCount10,
                            TotalTeamStrength = GetDashSummarySingle.TotalCount10,
                            //TotalCapacity = itemdiv.Capacity * GetDashSummarySingle.TotalPresent10,
                            FreshCount = GetDashSummarySingle.TotalFresh10,
                            RevisionCount = GetDashSummarySingle.TotalRevision10,
                            TotalCompletedCount = GetDashSummarySingle.TotalCompletedCount10,
                            TotalDeficitExceed = ((GetDashSummarySingle.TotalFresh10) - (itemdiv.Capacity * GetDashSummarySingle.TotalCount10)),
                            TotalRevenueUSD = GetDashSummarySingle.TotalRevenue10,
                            TotalRevenueINR = 70 * GetDashSummarySingle.TotalRevenue10,
                            CurrentDatetime = getcurrentdt
                        });
                    }
                }
            }
            return GetDashSummaryList;
        }
        //CurrentDashboardSummary EmployeeList By Division
        public IEnumerable<GetCurrentDashSummaryListViewModel> GetCurrentDashboardSummaryList(GetCurrentDashSummaryListPara gdsp)
        {
           // ApplicationDbContext db = new ApplicationDbContext(_configuration);
            DateTime getcurrentdt = gdsp.CurrentDatetime;
            DateTime Scurrentdate = getcurrentdt.Date;
            //string Scurrentdate = getcurrentdt.ToString("yyyy-MM-dd");
            string Scurrenttime = getcurrentdt.ToString("HH:mm:ss");
            string onlyDfromtime = "06:00:00";
            string onlyDtotime = "22:30:00";
            string onlyNfromtime = "22:31:00";
            string onlyNtotime = "05:59:00";
            DateTime fromtime = new DateTime();
            DateTime totime = new DateTime();
            if ((DateTime.ParseExact(onlyDfromtime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Scurrenttime, "HH:mm:ss", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(onlyDtotime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Scurrenttime, "HH:mm:ss", CultureInfo.InvariantCulture)))
            {
                //day
                fromtime = Scurrentdate.AddHours(00).AddMinutes(30).AddSeconds(00);
                totime = getcurrentdt.ToUniversalTime();
            }
            else
            {
                //night
                string checktime = "00:00:00";
                if (DateTime.ParseExact(checktime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact(Scurrenttime, "HH:mm:ss", CultureInfo.InvariantCulture))
                {
                    fromtime = Scurrentdate.AddDays(-1).AddHours(17).AddMinutes(01).AddSeconds(00);
                }
                else
                {
                    fromtime = Scurrentdate.AddHours(17).AddMinutes(01).AddSeconds(00);
                }
                totime = getcurrentdt.ToUniversalTime();
            }
            //fromtime = new DateTime(2018,11,07,00,00,00);
            //totime = new DateTime(2018,11,07,23,59,00);
            int? glbempid = 0;
            int glbdeptid = 0;
            //
            int freshcount = 0;
            int revisioncount = 0;
            decimal? revenueusd = 0;
            int totalarchieved = 0;
            //
            var GetDashSummaryList = new List<GetCurrentDashSummaryListViewModel>(); //&& cvd.DivisionId == gdsp.DivisionId
            var tdmttble = (from tdmt in db.TrayDetailMovementTrans
                            join pwfm in db.ProcessWorkFlowMasters on tdmt.Wfmid equals pwfm.Id
                            join jo in db.JobOrders on pwfm.JobId equals jo.Id
                            join cvd in db.CustomerVsDivisions on tdmt.ClientId equals cvd.CustomerId
                            where ((tdmt.UpdatedUtc >= fromtime && tdmt.UpdatedUtc <= totime) && (tdmt.Process == "Production" && tdmt.Status == "Completed") && (jo.JobStatusId == 9 || jo.JobStatusId == 5) && tdmt.IsDeleted == false)
                            select new { tdmt.EmployeeId, tdmt.Wfmid, jo.JobStatusId, jo.SpecialPrice, cvd.DivisionId }).ToList();
            int tdmttblcount = tdmttble.Count();
            if (tdmttblcount > 0)
            {
                var grpempid = tdmttble.GroupBy(x => x.EmployeeId).ToList();
                foreach (var itememp in grpempid)
                {
                    glbempid = itememp.FirstOrDefault().EmployeeId;
                    var emptbl = db.Employees.Where(x => x.EmployeeId == glbempid && (x.DepartmentId == 1 || x.DepartmentId == 2) && x.IsDeleted == false).ToList();//empid and deptid
                    int emptblcount = emptbl.Count();
                    if (emptblcount > 0)
                    {
                        if (emptblcount == 1)
                        {
                            glbdeptid = emptbl.FirstOrDefault().DepartmentId;
                            var empdivtbl = db.EmployeeVsDivisions.Where(x => x.EmployeeId == glbempid && x.DivisionId == gdsp.DivisionId && x.DeptId == glbdeptid && x.IsDeleted == false).ToList();
                            int empdivtblcount = empdivtbl.Count();
                            if (empdivtblcount > 0)
                            {
                                //
                                var divtbl = db.Divisions.FirstOrDefault(x => x.Id == gdsp.DivisionId);
                                var checkoutemp = emptbl.FirstOrDefault();
                                freshcount = tdmttble.Where(x => x.JobStatusId == 9 && x.EmployeeId == glbempid && x.DivisionId == gdsp.DivisionId).Count();
                                revisioncount = tdmttble.Where(x => x.JobStatusId == 5 && x.EmployeeId == glbempid && x.DivisionId == gdsp.DivisionId).Count();
                                totalarchieved = tdmttble.Where(x => x.EmployeeId == glbempid && x.DivisionId == gdsp.DivisionId).Count();
                                revenueusd = Convert.ToDecimal(tdmttble.Where(x => x.JobStatusId == 9 && x.EmployeeId == glbempid && x.DivisionId == gdsp.DivisionId && x.SpecialPrice != null).Sum(x => x.SpecialPrice.Value));
                                if (checkoutemp.IsOutsource == true)
                                {
                                    GetDashSummaryList.Add(new GetCurrentDashSummaryListViewModel
                                    {
                                        EmployeeCode = emptbl.FirstOrDefault().EmployeeCode,
                                        EmployeeName = emptbl.FirstOrDefault().EmployeeName,
                                        Norms = divtbl.Capacity,
                                        FreshCount = freshcount,
                                        DeficitExceed = freshcount - divtbl.Capacity,
                                        RevisionCount = revisioncount,
                                        OverAllArchieved = totalarchieved,
                                        RevenueUSD = revenueusd,
                                        RevenueINR = 70 * revenueusd,
                                        OutSource = 1
                                    });
                                    freshcount = 0;
                                    revisioncount = 0;
                                    revenueusd = 0;
                                    totalarchieved = 0;
                                }
                                else
                                {
                                    GetDashSummaryList.Add(new GetCurrentDashSummaryListViewModel
                                    {
                                        EmployeeCode = emptbl.FirstOrDefault().EmployeeCode,
                                        EmployeeName = emptbl.FirstOrDefault().EmployeeName,
                                        Norms = divtbl.Capacity,
                                        FreshCount = freshcount,
                                        DeficitExceed = freshcount - divtbl.Capacity,
                                        RevisionCount = revisioncount,
                                        OverAllArchieved = totalarchieved,
                                        RevenueUSD = revenueusd,
                                        RevenueINR = 70 * revenueusd
                                    });
                                    freshcount = 0;
                                    revisioncount = 0;
                                    revenueusd = 0;
                                    totalarchieved = 0;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return GetDashSummaryList;
        }

        //DashboardSummary By Division
        public IEnumerable<GetDashSummaryViewModel> GetDashboardSummary(GetDashSummaryPara gdsp)
        {
            //ApplicationDbContext db = new ApplicationDbContext(_configuration);
            string holdempcode = "";
            string holdproductionempcode = "";
            int holdproductionempid = 0;
            var GetDashSummaryList = new List<GetDashSummaryViewModel>();
            var GetDashSummarySingle = new GetDashSummarySingleViewModel();

            var sqlpara = new SqlParameter[]
                        {
                            new SqlParameter("@fromdate",  gdsp.FromDate),
                            new SqlParameter("@todate", gdsp.ToDate.AddDays(1)),
                            new SqlParameter("@shifttype", gdsp.Shift)
                        };

            var resultFromSProc = _procedureWork.ExecStoredProcedure<spAttendanceData_Result>("spAttendanceData @fromdate,@todate,@shifttype", sqlpara).ToList();
            var gpempcode = resultFromSProc.GroupBy(x => x.StaffId).ToList();
            foreach (var empcode in gpempcode)
            {
                holdempcode = empcode.FirstOrDefault().StaffId;
                var checkempdept = db.Employees.Where(x => x.EmployeeCode == holdempcode && (x.DepartmentId == 1 || x.DepartmentId == 2) && x.IsDeleted == false).ToList();
                int countcheckempdept = checkempdept.Count();
                if (countcheckempdept > 0)
                {
                    //logic
                    if (countcheckempdept == 1)
                    {
                        var holdempsingle = checkempdept.FirstOrDefault();//empid and deptid
                        holdproductionempcode = holdempsingle.EmployeeCode;
                        holdproductionempid = holdempsingle.EmployeeId;
                        if (holdempsingle != null)
                        {
                            var holddivid = db.EmployeeVsDivisions.Where(x => x.EmployeeId == holdempsingle.EmployeeId && x.DeptId == holdempsingle.DepartmentId && x.IsDeleted == false).ToList();
                            int countholddivid = holddivid.Count();
                            if (countholddivid > 0)
                            {
                                foreach (var itemdivid in holddivid)
                                {
                                    //
                                    var holdsingledivid = itemdivid.DivisionId;
                                    if (holdsingledivid != null)
                                    {
                                        var divisionid = db.Divisions.FirstOrDefault(x => x.Id == holdsingledivid);
                                        var checkattstatus = resultFromSProc.FirstOrDefault(x => x.StaffId == holdproductionempcode);
                                        if (holdsingledivid == 1)
                                        {
                                            GetDashSummarySingle.TotalCount1 += 1;
                                            if (checkattstatus != null)
                                            {
                                                if ((empcode.FirstOrDefault().DayAccount == 1 || empcode.FirstOrDefault().DayAccount == 0.5) && (empcode.FirstOrDefault().AttendanceStatus == "PR" || empcode.FirstOrDefault().AttendanceStatus == "hd"))
                                                {
                                                    GetDashSummarySingle.TotalPresent1 += empcode.FirstOrDefault().DayAccount;
                                                }
                                                var result1 = (from tdmt in db.TrayDetailMovementTrans
                                                               join pwfm in db.ProcessWorkFlowMasters on tdmt.Wfmid equals pwfm.Id
                                                               join jo in db.JobOrders on pwfm.JobId equals jo.Id
                                                               join cvd in db.CustomerVsDivisions on tdmt.ClientId equals cvd.CustomerId
                                                               where ((tdmt.UpdatedUtc >= checkattstatus.ActualInDateTime && tdmt.UpdatedUtc <= checkattstatus.ActualOutDateTime) && tdmt.EmployeeId == holdproductionempid && (tdmt.Process == "Production" && tdmt.Status == "Completed") && jo.JobStatusId == 9 && cvd.DivisionId == 1 && tdmt.IsDeleted == false)
                                                               select new { jo.SpecialPrice }).ToList();
                                                GetDashSummarySingle.TotalCompletedCount1 += result1.Count();
                                                GetDashSummarySingle.TotalRevenue1 = Convert.ToDecimal(result1.Where(x => x.SpecialPrice != null).Sum(x => x.SpecialPrice).Value) + Convert.ToDecimal(GetDashSummarySingle.TotalRevenue1);
                                            }
                                        }
                                        else if (holdsingledivid == 2)
                                        {
                                            GetDashSummarySingle.TotalCount2 += 1;
                                            if (checkattstatus != null)
                                            {
                                                if ((empcode.FirstOrDefault().DayAccount == 1 || empcode.FirstOrDefault().DayAccount == 0.5) && (empcode.FirstOrDefault().AttendanceStatus == "PR" || empcode.FirstOrDefault().AttendanceStatus == "hd"))
                                                {
                                                    GetDashSummarySingle.TotalPresent2 += empcode.FirstOrDefault().DayAccount;
                                                }
                                                var result2 = (from tdmt in db.TrayDetailMovementTrans
                                                               join pwfm in db.ProcessWorkFlowMasters on tdmt.Wfmid equals pwfm.Id
                                                               join jo in db.JobOrders on pwfm.JobId equals jo.Id
                                                               join cvd in db.CustomerVsDivisions on tdmt.ClientId equals cvd.CustomerId
                                                               where ((tdmt.UpdatedUtc >= checkattstatus.ActualInDateTime && tdmt.UpdatedUtc <= checkattstatus.ActualOutDateTime) && tdmt.EmployeeId == holdproductionempid && (tdmt.Process == "Production" && tdmt.Status == "Completed") && jo.JobStatusId == 9 && cvd.DivisionId == 2 && tdmt.IsDeleted == false)
                                                               select new { jo.SpecialPrice }).ToList();
                                                GetDashSummarySingle.TotalCompletedCount2 += result2.Count();
                                                GetDashSummarySingle.TotalRevenue2 = Convert.ToDecimal(result2.Where(x => x.SpecialPrice != null).Sum(x => x.SpecialPrice).Value) + Convert.ToDecimal(GetDashSummarySingle.TotalRevenue2);
                                            }
                                        }
                                        else if (holdsingledivid == 3)
                                        {
                                            GetDashSummarySingle.TotalCount3 += 1;
                                            if (checkattstatus != null)
                                            {
                                                if ((empcode.FirstOrDefault().DayAccount == 1 || empcode.FirstOrDefault().DayAccount == 0.5) && (empcode.FirstOrDefault().AttendanceStatus == "PR" || empcode.FirstOrDefault().AttendanceStatus == "hd"))
                                                {
                                                    GetDashSummarySingle.TotalPresent3 += empcode.FirstOrDefault().DayAccount;
                                                }
                                                var result3 = (from tdmt in db.TrayDetailMovementTrans
                                                               join pwfm in db.ProcessWorkFlowMasters on tdmt.Wfmid equals pwfm.Id
                                                               join jo in db.JobOrders on pwfm.JobId equals jo.Id
                                                               join cvd in db.CustomerVsDivisions on tdmt.ClientId equals cvd.CustomerId
                                                               where ((tdmt.UpdatedUtc >= checkattstatus.ActualInDateTime && tdmt.UpdatedUtc <= checkattstatus.ActualOutDateTime) && tdmt.EmployeeId == holdproductionempid && (tdmt.Process == "Production" && tdmt.Status == "Completed") && jo.JobStatusId == 9 && cvd.DivisionId == 3 && tdmt.IsDeleted == false)
                                                               select new { jo.SpecialPrice }).ToList();
                                                GetDashSummarySingle.TotalCompletedCount3 += result3.Count();
                                                GetDashSummarySingle.TotalRevenue3 = Convert.ToDecimal(result3.Where(x => x.SpecialPrice != null).Sum(x => x.SpecialPrice).Value) + Convert.ToDecimal(GetDashSummarySingle.TotalRevenue3);
                                            }
                                        }
                                        else if (holdsingledivid == 4)
                                        {
                                            GetDashSummarySingle.TotalCount4 += 1;
                                            if (checkattstatus != null)
                                            {
                                                if ((empcode.FirstOrDefault().DayAccount == 1 || empcode.FirstOrDefault().DayAccount == 0.5) && (empcode.FirstOrDefault().AttendanceStatus == "PR" || empcode.FirstOrDefault().AttendanceStatus == "hd"))
                                                {
                                                    GetDashSummarySingle.TotalPresent4 += empcode.FirstOrDefault().DayAccount;
                                                }
                                                var result4 = (from tdmt in db.TrayDetailMovementTrans
                                                               join pwfm in db.ProcessWorkFlowMasters on tdmt.Wfmid equals pwfm.Id
                                                               join jo in db.JobOrders on pwfm.JobId equals jo.Id
                                                               join cvd in db.CustomerVsDivisions on tdmt.ClientId equals cvd.CustomerId
                                                               where ((tdmt.UpdatedUtc >= checkattstatus.ActualInDateTime && tdmt.UpdatedUtc <= checkattstatus.ActualOutDateTime) && tdmt.EmployeeId == holdproductionempid && (tdmt.Process == "Production" && tdmt.Status == "Completed") && jo.JobStatusId == 9 && cvd.DivisionId == 4 && tdmt.IsDeleted == false)
                                                               select new { jo.SpecialPrice }).ToList();
                                                GetDashSummarySingle.TotalCompletedCount4 += result4.Count();
                                                GetDashSummarySingle.TotalRevenue4 = Convert.ToDecimal(result4.Where(x => x.SpecialPrice != null).Sum(x => x.SpecialPrice).Value) + Convert.ToDecimal(GetDashSummarySingle.TotalRevenue4);
                                            }
                                        }
                                        else if (holdsingledivid == 5)
                                        {
                                            GetDashSummarySingle.TotalCount5 += 1;
                                            if (checkattstatus != null)
                                            {
                                                if ((empcode.FirstOrDefault().DayAccount == 1 || empcode.FirstOrDefault().DayAccount == 0.5) && (empcode.FirstOrDefault().AttendanceStatus == "PR" || empcode.FirstOrDefault().AttendanceStatus == "hd"))
                                                {
                                                    GetDashSummarySingle.TotalPresent5 += empcode.FirstOrDefault().DayAccount;
                                                }
                                                var result5 = (from tdmt in db.TrayDetailMovementTrans
                                                               join pwfm in db.ProcessWorkFlowMasters on tdmt.Wfmid equals pwfm.Id
                                                               join jo in db.JobOrders on pwfm.JobId equals jo.Id
                                                               join cvd in db.CustomerVsDivisions on tdmt.ClientId equals cvd.CustomerId
                                                               where ((tdmt.UpdatedUtc >= checkattstatus.ActualInDateTime && tdmt.UpdatedUtc <= checkattstatus.ActualOutDateTime) && tdmt.EmployeeId == holdproductionempid && (tdmt.Process == "Production" && tdmt.Status == "Completed") && jo.JobStatusId == 9 && cvd.DivisionId == 5 && tdmt.IsDeleted == false)
                                                               select new { jo.SpecialPrice }).ToList();
                                                GetDashSummarySingle.TotalCompletedCount5 += result5.Count();
                                                GetDashSummarySingle.TotalRevenue5 = Convert.ToDecimal(result5.Where(x => x.SpecialPrice != null).Sum(x => x.SpecialPrice).Value) + Convert.ToDecimal(GetDashSummarySingle.TotalRevenue5);
                                            }
                                        }
                                        else if (holdsingledivid == 6)
                                        {
                                            GetDashSummarySingle.TotalCount6 += 1;
                                            if (checkattstatus != null)
                                            {
                                                if ((empcode.FirstOrDefault().DayAccount == 1 || empcode.FirstOrDefault().DayAccount == 0.5) && (empcode.FirstOrDefault().AttendanceStatus == "PR" || empcode.FirstOrDefault().AttendanceStatus == "hd"))
                                                {
                                                    GetDashSummarySingle.TotalPresent6 += empcode.FirstOrDefault().DayAccount;
                                                }
                                                var result6 = (from tdmt in db.TrayDetailMovementTrans
                                                               join pwfm in db.ProcessWorkFlowMasters on tdmt.Wfmid equals pwfm.Id
                                                               join jo in db.JobOrders on pwfm.JobId equals jo.Id
                                                               join cvd in db.CustomerVsDivisions on tdmt.ClientId equals cvd.CustomerId
                                                               where ((tdmt.UpdatedUtc >= checkattstatus.ActualInDateTime && tdmt.UpdatedUtc <= checkattstatus.ActualOutDateTime) && tdmt.EmployeeId == holdproductionempid && (tdmt.Process == "Production" && tdmt.Status == "Completed") && jo.JobStatusId == 9 && cvd.DivisionId == 6 && tdmt.IsDeleted == false)
                                                               select new { jo.SpecialPrice }).ToList();
                                                GetDashSummarySingle.TotalCompletedCount6 += result6.Count();
                                                GetDashSummarySingle.TotalRevenue6 = Convert.ToDecimal(result6.Where(x => x.SpecialPrice != null).Sum(x => x.SpecialPrice).Value) + Convert.ToDecimal(GetDashSummarySingle.TotalRevenue6);
                                            }
                                        }
                                        else if (holdsingledivid == 7)
                                        {
                                            GetDashSummarySingle.TotalCount7 += 1;
                                            if (checkattstatus != null)
                                            {
                                                if ((empcode.FirstOrDefault().DayAccount == 1 || empcode.FirstOrDefault().DayAccount == 0.5) && (empcode.FirstOrDefault().AttendanceStatus == "PR" || empcode.FirstOrDefault().AttendanceStatus == "hd"))
                                                {
                                                    GetDashSummarySingle.TotalPresent7 += empcode.FirstOrDefault().DayAccount;
                                                }
                                                var result7 = (from tdmt in db.TrayDetailMovementTrans
                                                               join pwfm in db.ProcessWorkFlowMasters on tdmt.Wfmid equals pwfm.Id
                                                               join jo in db.JobOrders on pwfm.JobId equals jo.Id
                                                               join cvd in db.CustomerVsDivisions on tdmt.ClientId equals cvd.CustomerId
                                                               where ((tdmt.UpdatedUtc >= checkattstatus.ActualInDateTime && tdmt.UpdatedUtc <= checkattstatus.ActualOutDateTime) && tdmt.EmployeeId == holdproductionempid && (tdmt.Process == "Production" && tdmt.Status == "Completed") && jo.JobStatusId == 9 && cvd.DivisionId == 7 && tdmt.IsDeleted == false)
                                                               select new { jo.SpecialPrice }).ToList();
                                                GetDashSummarySingle.TotalCompletedCount7 += result7.Count();
                                                GetDashSummarySingle.TotalRevenue7 = Convert.ToDecimal(result7.Where(x => x.SpecialPrice != null).Sum(x => x.SpecialPrice).Value) + Convert.ToDecimal(GetDashSummarySingle.TotalRevenue7);
                                            }
                                        }
                                        else if (holdsingledivid == 8)
                                        {
                                            GetDashSummarySingle.TotalCount8 += 1;
                                            if (checkattstatus != null)
                                            {
                                                if ((empcode.FirstOrDefault().DayAccount == 1 || empcode.FirstOrDefault().DayAccount == 0.5) && (empcode.FirstOrDefault().AttendanceStatus == "PR" || empcode.FirstOrDefault().AttendanceStatus == "hd"))
                                                {
                                                    GetDashSummarySingle.TotalPresent8 += empcode.FirstOrDefault().DayAccount;
                                                }
                                                var result8 = (from tdmt in db.TrayDetailMovementTrans
                                                               join pwfm in db.ProcessWorkFlowMasters on tdmt.Wfmid equals pwfm.Id
                                                               join jo in db.JobOrders on pwfm.JobId equals jo.Id
                                                               join cvd in db.CustomerVsDivisions on tdmt.ClientId equals cvd.CustomerId
                                                               where ((tdmt.UpdatedUtc >= checkattstatus.ActualInDateTime && tdmt.UpdatedUtc <= checkattstatus.ActualOutDateTime) && tdmt.EmployeeId == holdproductionempid && (tdmt.Process == "Production" && tdmt.Status == "Completed") && jo.JobStatusId == 9 && cvd.DivisionId == 8 && tdmt.IsDeleted == false)
                                                               select new { jo.SpecialPrice }).ToList();
                                                GetDashSummarySingle.TotalCompletedCount8 += result8.Count();
                                                GetDashSummarySingle.TotalRevenue8 = Convert.ToDecimal(result8.Where(x => x.SpecialPrice != null).Sum(x => x.SpecialPrice).Value) + Convert.ToDecimal(GetDashSummarySingle.TotalRevenue8);
                                            }
                                        }
                                        else if (holdsingledivid == 9)
                                        {
                                            GetDashSummarySingle.TotalCount9 += 1;
                                            if (checkattstatus != null)
                                            {
                                                if ((empcode.FirstOrDefault().DayAccount == 1 || empcode.FirstOrDefault().DayAccount == 0.5) && (empcode.FirstOrDefault().AttendanceStatus == "PR" || empcode.FirstOrDefault().AttendanceStatus == "hd"))
                                                {
                                                    GetDashSummarySingle.TotalPresent9 += empcode.FirstOrDefault().DayAccount;
                                                }
                                                var result9 = (from tdmt in db.TrayDetailMovementTrans
                                                               join pwfm in db.ProcessWorkFlowMasters on tdmt.Wfmid equals pwfm.Id
                                                               join jo in db.JobOrders on pwfm.JobId equals jo.Id
                                                               join cvd in db.CustomerVsDivisions on tdmt.ClientId equals cvd.CustomerId
                                                               where ((tdmt.UpdatedUtc >= checkattstatus.ActualInDateTime && tdmt.UpdatedUtc <= checkattstatus.ActualOutDateTime) && tdmt.EmployeeId == holdproductionempid && (tdmt.Process == "Production" && tdmt.Status == "Completed") && jo.JobStatusId == 9 && cvd.DivisionId == 9 && tdmt.IsDeleted == false)
                                                               select new { jo.SpecialPrice }).ToList();
                                                GetDashSummarySingle.TotalCompletedCount9 += result9.Count();
                                                GetDashSummarySingle.TotalRevenue9 = Convert.ToDecimal(result9.Where(x => x.SpecialPrice != null).Sum(x => x.SpecialPrice).Value) + Convert.ToDecimal(GetDashSummarySingle.TotalRevenue9);
                                            }
                                        }
                                        else if (holdsingledivid == 10)
                                        {
                                            GetDashSummarySingle.TotalCount10 += 1;
                                            if (checkattstatus != null)
                                            {
                                                if ((empcode.FirstOrDefault().DayAccount == 1 || empcode.FirstOrDefault().DayAccount == 0.5) && (empcode.FirstOrDefault().AttendanceStatus == "PR" || empcode.FirstOrDefault().AttendanceStatus == "hd"))
                                                {
                                                    GetDashSummarySingle.TotalPresent10 += empcode.FirstOrDefault().DayAccount;
                                                }
                                                var result10 = (from tdmt in db.TrayDetailMovementTrans
                                                                join pwfm in db.ProcessWorkFlowMasters on tdmt.Wfmid equals pwfm.Id
                                                                join jo in db.JobOrders on pwfm.JobId equals jo.Id
                                                                join cvd in db.CustomerVsDivisions on tdmt.ClientId equals cvd.CustomerId
                                                                where ((tdmt.UpdatedUtc >= checkattstatus.ActualInDateTime && tdmt.UpdatedUtc <= checkattstatus.ActualOutDateTime) && tdmt.EmployeeId == holdproductionempid && (tdmt.Process == "Production" && tdmt.Status == "Completed") && jo.JobStatusId == 9 && cvd.DivisionId == 10 && tdmt.IsDeleted == false)
                                                                select new { jo.SpecialPrice }).ToList();
                                                GetDashSummarySingle.TotalCompletedCount10 += result10.Count();
                                                GetDashSummarySingle.TotalRevenue10 = Convert.ToDecimal(result10.Where(x => x.SpecialPrice != null).Sum(x => x.SpecialPrice).Value) + Convert.ToDecimal(GetDashSummarySingle.TotalRevenue10);
                                            }
                                        }
                                        //}                                        
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //temp logic
                        continue;
                    }
                }
            }
            var comptdivtbl = db.Divisions.ToList();
            foreach (var itemdiv in comptdivtbl)
            {
                if (itemdiv.Id == 1)
                {
                    GetDashSummaryList.Add(new GetDashSummaryViewModel
                    {
                        DivisionId = 1,
                        Division = itemdiv.DivisionName,
                        TLName = db.Employees.FirstOrDefault(x => x.EmployeeId == itemdiv.TeamLeadEmpId).EmployeeName,
                        TotalTeamStrength = GetDashSummarySingle.TotalCount1,
                        TotalPresent = GetDashSummarySingle.TotalPresent1,
                        TotalAbsent = GetDashSummarySingle.TotalCount1 - GetDashSummarySingle.TotalPresent1,
                        TotalCapacity = itemdiv.Capacity * GetDashSummarySingle.TotalPresent1,
                        TotalCompletedCount = GetDashSummarySingle.TotalCompletedCount1,
                        TotalDeficitExceed = ((GetDashSummarySingle.TotalCompletedCount1) - (itemdiv.Capacity * GetDashSummarySingle.TotalPresent1)),
                        TotalRevenueUSD = GetDashSummarySingle.TotalRevenue1,
                        TotalRevenueINR = 70 * GetDashSummarySingle.TotalRevenue1
                    });
                }
                else if (itemdiv.Id == 2)
                {
                    GetDashSummaryList.Add(new GetDashSummaryViewModel
                    {
                        DivisionId = 2,
                        Division = itemdiv.DivisionName,
                        TLName = db.Employees.FirstOrDefault(x => x.EmployeeId == itemdiv.TeamLeadEmpId).EmployeeName,
                        TotalTeamStrength = GetDashSummarySingle.TotalCount2,
                        TotalPresent = GetDashSummarySingle.TotalPresent2,
                        TotalAbsent = GetDashSummarySingle.TotalCount2 - GetDashSummarySingle.TotalPresent2,
                        TotalCapacity = itemdiv.Capacity * GetDashSummarySingle.TotalPresent2,
                        TotalCompletedCount = GetDashSummarySingle.TotalCompletedCount2,
                        TotalDeficitExceed = ((GetDashSummarySingle.TotalCompletedCount2) - (itemdiv.Capacity * GetDashSummarySingle.TotalPresent2)),
                        TotalRevenueUSD = GetDashSummarySingle.TotalRevenue2,
                        TotalRevenueINR = 70 * GetDashSummarySingle.TotalRevenue2
                    });
                }
                else if (itemdiv.Id == 3)
                {
                    GetDashSummaryList.Add(new GetDashSummaryViewModel
                    {
                        DivisionId = 3,
                        Division = itemdiv.DivisionName,
                        TLName = db.Employees.FirstOrDefault(x => x.EmployeeId == itemdiv.TeamLeadEmpId).EmployeeName,
                        TotalTeamStrength = GetDashSummarySingle.TotalCount3,
                        TotalPresent = GetDashSummarySingle.TotalPresent3,
                        TotalAbsent = GetDashSummarySingle.TotalCount3 - GetDashSummarySingle.TotalPresent3,
                        TotalCapacity = itemdiv.Capacity * GetDashSummarySingle.TotalPresent3,
                        TotalCompletedCount = GetDashSummarySingle.TotalCompletedCount3,
                        TotalDeficitExceed = ((GetDashSummarySingle.TotalCompletedCount3) - (itemdiv.Capacity * GetDashSummarySingle.TotalPresent3)),
                        TotalRevenueUSD = GetDashSummarySingle.TotalRevenue3,
                        TotalRevenueINR = 70 * GetDashSummarySingle.TotalRevenue3
                    });
                }
                else if (itemdiv.Id == 4)
                {
                    GetDashSummaryList.Add(new GetDashSummaryViewModel
                    {
                        DivisionId = 4,
                        Division = itemdiv.DivisionName,
                        TLName = db.Employees.FirstOrDefault(x => x.EmployeeId == itemdiv.TeamLeadEmpId).EmployeeName,
                        TotalTeamStrength = GetDashSummarySingle.TotalCount4,
                        TotalPresent = GetDashSummarySingle.TotalPresent4,
                        TotalAbsent = GetDashSummarySingle.TotalCount4 - GetDashSummarySingle.TotalPresent4,
                        TotalCapacity = itemdiv.Capacity * GetDashSummarySingle.TotalPresent4,
                        TotalCompletedCount = GetDashSummarySingle.TotalCompletedCount4,
                        TotalDeficitExceed = ((GetDashSummarySingle.TotalCompletedCount4) - (itemdiv.Capacity * GetDashSummarySingle.TotalPresent4)),
                        TotalRevenueUSD = GetDashSummarySingle.TotalRevenue4,
                        TotalRevenueINR = 70 * GetDashSummarySingle.TotalRevenue4
                    });
                }
                else if (itemdiv.Id == 5)
                {
                    GetDashSummaryList.Add(new GetDashSummaryViewModel
                    {
                        DivisionId = 5,
                        Division = itemdiv.DivisionName,
                        TLName = db.Employees.FirstOrDefault(x => x.EmployeeId == itemdiv.TeamLeadEmpId).EmployeeName,
                        TotalTeamStrength = GetDashSummarySingle.TotalCount5,
                        TotalPresent = GetDashSummarySingle.TotalPresent5,
                        TotalAbsent = GetDashSummarySingle.TotalCount5 - GetDashSummarySingle.TotalPresent5,
                        TotalCapacity = itemdiv.Capacity * GetDashSummarySingle.TotalPresent5,
                        TotalCompletedCount = GetDashSummarySingle.TotalCompletedCount5,
                        TotalDeficitExceed = ((GetDashSummarySingle.TotalCompletedCount5) - (itemdiv.Capacity * GetDashSummarySingle.TotalPresent5)),
                        TotalRevenueUSD = GetDashSummarySingle.TotalRevenue5,
                        TotalRevenueINR = 70 * GetDashSummarySingle.TotalRevenue5
                    });
                }
                else if (itemdiv.Id == 6)
                {
                    GetDashSummaryList.Add(new GetDashSummaryViewModel
                    {
                        DivisionId = 6,
                        Division = itemdiv.DivisionName,
                        TLName = db.Employees.FirstOrDefault(x => x.EmployeeId == itemdiv.TeamLeadEmpId).EmployeeName,
                        TotalTeamStrength = GetDashSummarySingle.TotalCount6,
                        TotalPresent = GetDashSummarySingle.TotalPresent6,
                        TotalAbsent = GetDashSummarySingle.TotalCount6 - GetDashSummarySingle.TotalPresent6,
                        TotalCapacity = itemdiv.Capacity * GetDashSummarySingle.TotalPresent6,
                        TotalCompletedCount = GetDashSummarySingle.TotalCompletedCount6,
                        TotalDeficitExceed = ((GetDashSummarySingle.TotalCompletedCount6) - (itemdiv.Capacity * GetDashSummarySingle.TotalPresent6)),
                        TotalRevenueUSD = GetDashSummarySingle.TotalRevenue6,
                        TotalRevenueINR = 70 * GetDashSummarySingle.TotalRevenue6
                    });
                }
                else if (itemdiv.Id == 7)
                {
                    GetDashSummaryList.Add(new GetDashSummaryViewModel
                    {
                        DivisionId = 7,
                        Division = itemdiv.DivisionName,
                        TLName = db.Employees.FirstOrDefault(x => x.EmployeeId == itemdiv.TeamLeadEmpId).EmployeeName,
                        TotalTeamStrength = GetDashSummarySingle.TotalCount7,
                        TotalPresent = GetDashSummarySingle.TotalPresent7,
                        TotalAbsent = GetDashSummarySingle.TotalCount7 - GetDashSummarySingle.TotalPresent7,
                        TotalCapacity = itemdiv.Capacity * GetDashSummarySingle.TotalPresent7,
                        TotalCompletedCount = GetDashSummarySingle.TotalCompletedCount7,
                        TotalDeficitExceed = ((GetDashSummarySingle.TotalCompletedCount7) - (itemdiv.Capacity * GetDashSummarySingle.TotalPresent7)),
                        TotalRevenueUSD = GetDashSummarySingle.TotalRevenue7,
                        TotalRevenueINR = 70 * GetDashSummarySingle.TotalRevenue7
                    });
                }
                else if (itemdiv.Id == 8)
                {
                    GetDashSummaryList.Add(new GetDashSummaryViewModel
                    {
                        DivisionId = 8,
                        Division = itemdiv.DivisionName,
                        TLName = db.Employees.FirstOrDefault(x => x.EmployeeId == itemdiv.TeamLeadEmpId).EmployeeName,
                        TotalTeamStrength = GetDashSummarySingle.TotalCount8,
                        TotalPresent = GetDashSummarySingle.TotalPresent8,
                        TotalAbsent = GetDashSummarySingle.TotalCount8 - GetDashSummarySingle.TotalPresent8,
                        TotalCapacity = itemdiv.Capacity * GetDashSummarySingle.TotalPresent8,
                        TotalCompletedCount = GetDashSummarySingle.TotalCompletedCount8,
                        TotalDeficitExceed = ((GetDashSummarySingle.TotalCompletedCount8) - (itemdiv.Capacity * GetDashSummarySingle.TotalPresent8)),
                        TotalRevenueUSD = GetDashSummarySingle.TotalRevenue8,
                        TotalRevenueINR = 70 * GetDashSummarySingle.TotalRevenue8
                    });
                }
                else if (itemdiv.Id == 9)
                {
                    GetDashSummaryList.Add(new GetDashSummaryViewModel
                    {
                        DivisionId = 9,
                        Division = itemdiv.DivisionName,
                        TLName = db.Employees.FirstOrDefault(x => x.EmployeeId == itemdiv.TeamLeadEmpId).EmployeeName,
                        TotalTeamStrength = GetDashSummarySingle.TotalCount9,
                        TotalPresent = GetDashSummarySingle.TotalPresent9,
                        TotalAbsent = GetDashSummarySingle.TotalCount9 - GetDashSummarySingle.TotalPresent9,
                        TotalCapacity = itemdiv.Capacity * GetDashSummarySingle.TotalPresent9,
                        TotalCompletedCount = GetDashSummarySingle.TotalCompletedCount9,
                        TotalDeficitExceed = ((GetDashSummarySingle.TotalCompletedCount9) - (itemdiv.Capacity * GetDashSummarySingle.TotalPresent9)),
                        TotalRevenueUSD = GetDashSummarySingle.TotalRevenue9,
                        TotalRevenueINR = 70 * GetDashSummarySingle.TotalRevenue9
                    });
                }
                else if (itemdiv.Id == 10)
                {
                    GetDashSummaryList.Add(new GetDashSummaryViewModel
                    {
                        DivisionId = 10,
                        Division = itemdiv.DivisionName,
                        TLName = db.Employees.FirstOrDefault(x => x.EmployeeId == itemdiv.TeamLeadEmpId).EmployeeName,
                        TotalTeamStrength = GetDashSummarySingle.TotalCount10,
                        TotalPresent = GetDashSummarySingle.TotalPresent10,
                        TotalAbsent = GetDashSummarySingle.TotalCount10 - GetDashSummarySingle.TotalPresent10,
                        TotalCapacity = itemdiv.Capacity * GetDashSummarySingle.TotalPresent10,
                        TotalCompletedCount = GetDashSummarySingle.TotalCompletedCount10,
                        TotalDeficitExceed = ((GetDashSummarySingle.TotalCompletedCount10) - (itemdiv.Capacity * GetDashSummarySingle.TotalPresent10)),
                        TotalRevenueUSD = GetDashSummarySingle.TotalRevenue10,
                        TotalRevenueINR = 70 * GetDashSummarySingle.TotalRevenue10
                    });
                }
            }
            return GetDashSummaryList;
        }
        //DashboardSummary EmployeeList By Division
        public IEnumerable<GetDashSummaryListViewModel> GetDashBoardSummaryList(GetDashSummaryListPara gdsp)
        {
           
            var GetDashSummaryList = new List<GetDashSummaryListViewModel>();
            string glbempcode = "";
            int glbempid = 0;
            int glbdeptid = 0;
            int freshcount = 0;
            int revisioncount = 0;
            int benchcount = 0;
            int qccompletedcount = 0;
            decimal? revenueusd = 0;
            int totalarchieved = 0;
            var sqlpara = new SqlParameter[]
                        {
                            new SqlParameter("@fromdate",  gdsp.FromDate),
                            new SqlParameter("@todate", gdsp.ToDate.AddDays(1)),
                            new SqlParameter("@shifttype", gdsp.Shift)
                        };

            var resultFromSProc = _procedureWork.ExecStoredProcedure<spAttendanceData_Result>("spAttendanceData @fromdate,@todate,@shifttype", sqlpara).ToList();
            var gpempcode = resultFromSProc.Where(x => (x.DayAccount == 1 || x.DayAccount == 0.5) && (x.AttendanceStatus == "PR" || x.AttendanceStatus == "hd")).GroupBy(x => x.StaffId).ToList();
            foreach (var empcode in gpempcode)
            {
                glbempcode = empcode.FirstOrDefault().StaffId;
                var empidfromcode = db.Employees.FirstOrDefault(x => x.EmployeeCode == glbempcode && (x.DepartmentId == 1 || x.DepartmentId == 2) && x.IsDeleted == false);
                if (empidfromcode != null)
                {
                    glbempid = empidfromcode.EmployeeId;
                    glbdeptid = empidfromcode.DepartmentId;
                    var finddivemp = db.EmployeeVsDivisions.Where(x => x.DivisionId == gdsp.DivisionId && x.EmployeeId == glbempid && x.DeptId == glbdeptid && x.IsDeleted == false).ToList();
                    int checkdivempcount = finddivemp.Count();
                    if (checkdivempcount > 0)
                    {
                        var divtbl = db.Divisions.FirstOrDefault(x => x.Id == gdsp.DivisionId);
                        var checkattstatus = resultFromSProc.FirstOrDefault(x => x.StaffId == glbempcode);
                        if (checkattstatus != null)
                        {
                            var result1 = (from tdmt in db.TrayDetailMovementTrans
                                           join pwfm in db.ProcessWorkFlowMasters on tdmt.Wfmid equals pwfm.Id
                                           join jo in db.JobOrders on pwfm.JobId equals jo.Id
                                           join cvd in db.CustomerVsDivisions on tdmt.ClientId equals cvd.CustomerId
                                           where ((tdmt.UpdatedUtc >= checkattstatus.ActualInDateTime && tdmt.UpdatedUtc <= checkattstatus.ActualOutDateTime) && tdmt.EmployeeId == glbempid && ((tdmt.Process == "Production" && tdmt.Status == "Completed") || (tdmt.Process == "Quality" && tdmt.Status == "Completed")) && (jo.JobStatusId == 9 || jo.JobStatusId == 5) && cvd.DivisionId == gdsp.DivisionId && tdmt.IsDeleted == false)
                                           select new { tdmt.Process, tdmt.Status, tdmt.IsBench, jo.JobStatusId, jo.SpecialPrice }).ToList();

                            freshcount = result1.Where(x => x.Process == "Production" && x.Status == "Completed" && x.JobStatusId == 9 && x.IsBench != true).Count();

                            revisioncount = result1.Where(x => x.Process == "Production" && x.Status == "Completed" && x.JobStatusId == 5 && x.IsBench != true).Count();

                            revenueusd = Convert.ToDecimal(result1.Where(x => x.Process == "Production" && x.Status == "Completed" && x.JobStatusId == 9 && x.IsBench != true && x.SpecialPrice != null).Sum(x => x.SpecialPrice));

                            benchcount = result1.Where(x => x.Process == "Production" && x.Status == "Completed" && x.IsBench == true).Count();

                            totalarchieved = result1.Where(x => x.Process == "Production" && x.Status == "Completed").Count();

                            qccompletedcount = result1.Where(x => x.Process == "Quality" && x.Status == "Completed").Count();

                            GetDashSummaryList.Add(new GetDashSummaryListViewModel
                            {
                                EmployeeCode = glbempcode,
                                EmployeeName = empidfromcode.EmployeeName,
                                Norms = divtbl.Capacity,
                                FreshCount = freshcount,
                                DeficitExceed = freshcount - divtbl.Capacity,
                                BenchCount = benchcount,
                                QCCompletedCount = qccompletedcount,
                                RevisionCount = revisioncount,
                                OverAllArchieved = totalarchieved,
                                RevenueUSD = revenueusd,
                                RevenueINR = 70 * revenueusd
                            });
                            freshcount = 0;
                            revisioncount = 0;
                            revenueusd = 0;
                            benchcount = 0;
                            qccompletedcount = 0;
                            totalarchieved = 0;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }
            if (gdsp.Shift == "D")
            {
                var getoutsourceemp = (from evd in db.EmployeeVsDivisions
                                       join emp in db.Employees on evd.EmployeeId equals emp.EmployeeId
                                       where (emp.IsOutsource == true && evd.DivisionId == gdsp.DivisionId && emp.IsDeleted == false)
                                       select new { emp.EmployeeId, emp.EmployeeCode, emp.EmployeeName, emp.IsOutsource }).ToList();
                foreach (var itemempid in getoutsourceemp)
                {
                    DateTime fromtime = gdsp.FromDate.AddDays(-1).AddHours(18).AddMinutes(30).AddSeconds(00);
                    DateTime totime = gdsp.ToDate.AddHours(18).AddMinutes(29).AddSeconds(00);
                    var tdmttbl = (from tdmt in db.TrayDetailMovementTrans
                                   join pwfm in db.ProcessWorkFlowMasters on tdmt.Wfmid equals pwfm.Id
                                   join jo in db.JobOrders on pwfm.JobId equals jo.Id
                                   join cvd in db.CustomerVsDivisions on tdmt.ClientId equals cvd.CustomerId
                                   where ((tdmt.UpdatedUtc >= fromtime && tdmt.UpdatedUtc <= totime) && tdmt.EmployeeId == itemempid.EmployeeId && (tdmt.Process == "Production" && tdmt.Status == "Completed") && (jo.JobStatusId == 9 || jo.JobStatusId == 5) && cvd.DivisionId == gdsp.DivisionId && tdmt.IsDeleted == false)
                                   select new { tdmt.Process, tdmt.Status, tdmt.IsBench, jo.JobStatusId, jo.SpecialPrice }).ToList();
                    var divtbl = db.Divisions.FirstOrDefault(x => x.Id == gdsp.DivisionId);
                    freshcount = tdmttbl.Where(x => x.JobStatusId == 9).Count();
                    revisioncount = tdmttbl.Where(x => x.JobStatusId == 5).Count();
                    totalarchieved = tdmttbl.Count();
                    revenueusd = Convert.ToDecimal(tdmttbl.Where(x => x.JobStatusId == 9 && x.SpecialPrice != null).Sum(x => x.SpecialPrice).Value);
                    GetDashSummaryList.Add(new GetDashSummaryListViewModel
                    {
                        EmployeeCode = itemempid.EmployeeCode,
                        EmployeeName = itemempid.EmployeeName,
                        Norms = divtbl.Capacity,
                        FreshCount = freshcount,
                        DeficitExceed = freshcount - divtbl.Capacity,
                        RevisionCount = revisioncount,
                        OverAllArchieved = totalarchieved,
                        RevenueUSD = revenueusd,
                        RevenueINR = 70 * revenueusd,
                        OutSource = 1
                    });
                    freshcount = 0;
                    revisioncount = 0;
                    totalarchieved = 0;
                    revenueusd = 0;
                }
            }
            return GetDashSummaryList;
        }
        //.......................
        public Object GetDashboardData()
        {
           
            DateTime fromdate = Convert.ToDateTime("2018-11-05 00:00:00");
            DateTime todate = Convert.ToDateTime("2018-11-09 23:59:59");

            var emplist = (from emp in dbo.EmployeeVsDivisions
                           join div in dbo.Divisions on emp.DivisionId equals div.Id
                           join empl in dbo.Employees on emp.EmployeeId equals empl.EmployeeId
                           where (div.Id == 6 && emp.IsDeleted == false)
                           select new
                           {
                               emp.EmployeeId,
                               empl.EmployeeCode,
                               empl.EmployeeName,
                               empl.IsOutsource,
                               div.Id,
                               div.DivisionName,
                               div.TeamLeadEmpId,
                               div.Capacity
                           }).ToList();

            var GetDBData = new List<GetDBData>();
            var GetBenchData = new List<GetBenchData>();
            int? BenchGivenDivId = 0;
            int? BenchTakenDivId = 0;
            int holdcount = 0;

            var OSemplist = (from emp in dbo.EmployeeVsDivisions
                             join div in dbo.Divisions on emp.DivisionId equals div.Id
                             join empl in dbo.Employees on emp.EmployeeId equals empl.EmployeeId
                             where (div.Id == 6 && emp.IsDeleted == false)
                             select new
                             {
                                 empl.EmployeeId,
                                 empl.EmployeeCode,
                                 empl.EmployeeName,
                                 div.Id,
                                 div.DivisionName,
                                 div.TeamLeadEmpId,
                                 div.Capacity
                             }).ToList();
            //var OutSourceEmpList = dbo.Employees .Where(x => x.IsOutsource ==  true).ToList();

            foreach (var singleemp in emplist)
            {
                var result = dbo.TrayDetailMovementTrans.Where(x => (x.UpdatedUtc >= fromdate && x.UpdatedUtc <= todate) && x.EmployeeId == singleemp.EmployeeId).ToList();

                var OSProductionresult = dbo.TrayDetailMovementTrans.Where(x => (x.JobDate >= fromdate && x.JobDate <= todate) && (x.UpdatedUtc >= fromdate && x.UpdatedUtc <= todate) && x.EmployeeId == singleemp.EmployeeId && singleemp.IsOutsource == true).ToList();

                int OutsourceProductionCount = OSProductionresult.Where(x => x.Process == "Production" && x.Status == "Completed" && x.JobStatus == "Fresh").Count();

                //foreach (var OSsingleemp in OSemplist)
                //{
                //    var OutsourceProductionResult = dbo.TrayDetailMovementTrans.Where(x => (x.JobDate >= fromdate && x.JobDate <= todate) && (x.UpdatedUTC >= fromdate && x.UpdatedUTC <= todate) && x.EmployeeId == OSsingleemp.EmployeeId).ToList();
                //}
                int ProductionFreshCount = result.Where(x => x.Process == "Production" && x.Status == "Completed" && x.JobStatus == "Fresh" && x.IsBench != true).Count();
                int ProductionRevisionCount = result.Where(x => x.Process == "Production" && x.Status == "Completed" && x.JobStatus == "Revision" && x.IsBench != true).Count();
                int QualityFreshCount = result.Where(x => x.Process == "Quality" && x.Status == "Completed" && x.JobStatus == "Fresh" && x.IsBench != true).Count();
                int QualityRevisionCount = result.Where(x => x.Process == "Quality" && x.Status == "Completed" && x.JobStatus == "Revision" && x.IsBench != true).Count();

                int BenchProductionFreshCount = result.Where(x => x.Process == "Production" && x.Status == "Completed" && x.JobStatus == "Fresh" && x.IsBench == true).Count();
                int BenchProductionRevisionCount = result.Where(x => x.Process == "Production" && x.Status == "Completed" && x.JobStatus == "Revision" && x.IsBench == true).Count();
                int BenchQualityFreshCount = result.Where(x => x.Process == "Quality" && x.Status == "Completed" && x.JobStatus == "Fresh" && x.IsBench == true).Count();
                int BenchQualityRevisionCount = result.Where(x => x.Process == "Quality" && x.Status == "Completed" && x.JobStatus == "Revision" && x.IsBench == true).Count();




                var BenchTakenGiven = dbo.TrayDetailMovementTrans.Where(x => (x.JobDate >= fromdate && x.JobDate <= todate) && (x.UpdatedUtc >= fromdate && x.UpdatedUtc<= todate) && x.EmployeeId == singleemp.EmployeeId && x.IsBench == true).ToList();
                holdcount = BenchTakenGiven.Count();
                if (holdcount != 0)
                {
                    foreach (var item in BenchTakenGiven)
                    {
                        var isbenchgivencheck = dbo.CustomerVsDivisions.Where(x => x.CustomerId == item.ClientId && x.DeptId == item.DepartmentId).FirstOrDefault();
                        if (isbenchgivencheck != null)
                        {
                            if (isbenchgivencheck.DivisionId != singleemp.Id)
                            {
                                BenchGivenDivId = singleemp.Id;
                                BenchTakenDivId = isbenchgivencheck.DivisionId;
                            }

                            else
                            {
                                BenchGivenDivId = 0;
                                BenchTakenDivId = 0;
                            }
                        }
                        GetDBData.Add(new GetDBData
                        {
                            EmployeeName = singleemp.EmployeeName,
                            EmployeeId = singleemp.EmployeeId,
                            ProductionFreshCount = ProductionFreshCount,
                            ProductionRevisionCount = ProductionRevisionCount,
                            QualityFreshCount = QualityFreshCount,
                            QualityRevisionCount = QualityRevisionCount,
                            OutsourceProductionCount = OutsourceProductionCount,

                            BenchProductionFreshCount = BenchProductionFreshCount,
                            BenchProductionRevisionCount = BenchProductionRevisionCount,
                            BenchQualityFreshCount = BenchQualityFreshCount,
                            BenchQualityRevisionCount = BenchQualityRevisionCount,
                            GetBenchGivenDivision = BenchGivenDivId,
                            GetBenchTakenDivision = BenchTakenDivId
                        });
                    }
                }
                else
                {
                    GetDBData.Add(new GetDBData
                    {
                        EmployeeName = singleemp.EmployeeName,
                        EmployeeId = singleemp.EmployeeId,
                        ProductionFreshCount = ProductionFreshCount,
                        ProductionRevisionCount = ProductionRevisionCount,
                        QualityFreshCount = QualityFreshCount,
                        QualityRevisionCount = QualityRevisionCount,
                        OutsourceProductionCount = OutsourceProductionCount,

                        BenchProductionFreshCount = BenchProductionFreshCount,
                        BenchProductionRevisionCount = BenchProductionRevisionCount,
                        BenchQualityFreshCount = BenchQualityFreshCount,
                        BenchQualityRevisionCount = BenchQualityRevisionCount,
                        GetBenchGivenDivision = 0,
                        GetBenchTakenDivision = 0
                    });
                }
            }
            return GetDBData;
        }
        //.......................
        public Object GetDashboardFSR()
        {
           
            DateTime fromdate = Convert.ToDateTime("30-Oct-2018");
            int deprt = 0;
            int clint = 0;
            int div = 7;
            int? filecount = 0;
            int? totalfilereceived = 0;
            var GetFSRList = new List<GetDashboardFSRPara>();
            var cvsdlist = (from cvsd in db.CustomerVsDivisions where (cvsd.DivisionId == div && cvsd.IsDeleted == false) select new { cvsd.CustomerId, cvsd.DivisionId, cvsd.DeptId }).ToList();
            var grpcuscvsd = cvsdlist.GroupBy(x => x.CustomerId).ToList();
            int checkcount = cvsdlist.Count();
            if (checkcount > 0)
            {
                foreach (var item in grpcuscvsd)
                {
                    var sqlpara = new SqlParameter[]
                        {
                            new SqlParameter("@FileReceivedDate",  fromdate),
                            new SqlParameter("@DepartmentId", (object)deprt ?? DBNull.Value),
                            new SqlParameter("@ClientId", (object)clint ?? DBNull.Value)
                        };

                    var resultFromSProc = _procedureWork.ExecStoredProcedure<DashboardReportFSR_Result>("DashboardReportFSR @FileReceivedDate,@DepartmentId,@ClientId", sqlpara).ToList();

                    var getcounts = resultFromSProc.Where(x => x.ClientId == item.FirstOrDefault().CustomerId && x.DepartmentId == item.FirstOrDefault().DeptId).ToList();
                    int checkgetcounts = getcounts.Count();
                    if (checkgetcounts != 0)
                    {
                        if (checkgetcounts > 1)
                        {
                            filecount = filecount + getcounts.Sum(x => x.Fileasonpereviousday).Value;
                            totalfilereceived = totalfilereceived + getcounts.Sum(x => x.TodayFilesReceived).Value;
                        }
                        else
                        {
                            filecount = filecount + getcounts.FirstOrDefault().Fileasonpereviousday;
                            totalfilereceived = totalfilereceived + getcounts.FirstOrDefault().TodayFilesReceived;
                        }
                    }
                }
                var division = db.Divisions.FirstOrDefault(x => x.Id == div);
                GetFSRList.Add(new GetDashboardFSRPara
                {
                    DivisionId = div,
                    Division = division.DivisionName,
                    Inventory = filecount,
                    TodayFileReceived = totalfilereceived
                });
            }
            return GetFSRList;
        }
        //...........................................
        //...........Hourly Report...................
        public IEnumerable<sp_GetEmployeebyDivision_Result> GetEmployeebyDivision(HourlyPara para)
        {
            var sqlparaCC = new SqlParameter[]
                        {
                            new SqlParameter("@division", para.DivisionId)
                        };
            var resultFromCCSProc = _procedureWork.ExecStoredProcedure<sp_GetEmployeebyDivision_Result>("sp_GetEmployeebyDivision @division", sqlparaCC).ToList();

            return resultFromCCSProc;
        }
        public Object GetHourlyReport(HourlyPara para)
        {
           

            var sqlparaCC = new SqlParameter[]
                        {
                            new SqlParameter("@date",  para.Date.Date),
                            new SqlParameter("@division", (object)para.DivisionId ?? DBNull.Value),
                            new SqlParameter("@employee", (object)para.EmployeeId ?? DBNull.Value),
                            new SqlParameter("@process", (object)para.Process ?? DBNull.Value)
                        };
            var resultFromCCSProc = _procedureWork.ExecStoredProcedure<sp_HourlyCCReport_Result>("sp_HourlyCCReport @date,@division,@employee,@process", sqlparaCC).ToList();

            HourlyReportVM hrvm = new HourlyReportVM();
            var SetData = new List<HourlyReportVM>();
            var CCPASetData = new List<HourlyReportVM>();

            foreach (var itemCC in resultFromCCSProc)
            {
                SetData.Add(new HourlyReportVM
                {
                    Date = itemCC.Date,
                    DivisionName = itemCC.DivisionName,
                    EmployeeCode = itemCC.EmployeeCode,
                    EmployeeName = itemCC.EmployeeName,
                    ClientShortName = itemCC.ClientShortName,
                    Process = itemCC.Process,
                    CC0 = itemCC.C0,
                    CC1 = itemCC.C1,
                    CC2 = itemCC.C2,
                    CC3 = itemCC.C3,
                    CC4 = itemCC.C4,
                    CC5 = itemCC.C5,
                    CC6 = itemCC.C6,
                    CC7 = itemCC.C7,
                    CC8 = itemCC.C8,
                    CC9 = itemCC.C9,
                    CC10 = itemCC.C10,
                    CC11 = itemCC.C11,
                    CC12 = itemCC.C12,
                    CC13 = itemCC.C13,
                    CC14 = itemCC.C14,
                    CC15 = itemCC.C15,
                    CC16 = itemCC.C16,
                    CC17 = itemCC.C17,
                    CC18 = itemCC.C18,
                    CC19 = itemCC.C19,
                    CC20 = itemCC.C20,
                    CC21 = itemCC.C21,
                    CC22 = itemCC.C22,
                    CC23 = itemCC.C23,
                    Type = itemCC.Type
                });
            }

            var sqlparaAC = new SqlParameter[]
                        {
                            new SqlParameter("@date",  para.Date.Date),
                            new SqlParameter("@division", (object)para.DivisionId ?? DBNull.Value),
                            new SqlParameter("@employee", (object)para.EmployeeId ?? DBNull.Value),
                            new SqlParameter("@process", (object)para.Process ?? DBNull.Value)
                        };
            var resultFromACSProc = _procedureWork.ExecStoredProcedure<sp_HourlyACReport_Result>("sp_HourlyACReport @date,@division,@employee,@process", sqlparaAC).ToList();

            foreach (var itemAC in resultFromACSProc)
            {
                SetData.Add(new HourlyReportVM
                {
                    DivisionName = itemAC.DivisionName,
                    EmployeeCode = itemAC.EmployeeCode,
                    EmployeeName = itemAC.EmployeeName,
                    ClientShortName = itemAC.ClientShortName,
                    Process = itemAC.Process,
                    AC0 = itemAC.P0,
                    AC1 = itemAC.P1,
                    AC2 = itemAC.P2,
                    AC3 = itemAC.P3,
                    AC4 = itemAC.P4,
                    AC5 = itemAC.P5,
                    AC6 = itemAC.P6,
                    AC7 = itemAC.P7,
                    AC8 = itemAC.P8,
                    AC9 = itemAC.P9,
                    AC10 = itemAC.P10,
                    AC11 = itemAC.P11,
                    AC12 = itemAC.P12,
                    AC13 = itemAC.P13,
                    AC14 = itemAC.P14,
                    AC15 = itemAC.P15,
                    AC16 = itemAC.P16,
                    AC17 = itemAC.P17,
                    AC18 = itemAC.P18,
                    AC19 = itemAC.P19,
                    AC20 = itemAC.P20,
                    AC21 = itemAC.P21,
                    AC22 = itemAC.P22,
                    AC23 = itemAC.P23,
                    Type = itemAC.Type
                });
            }

            var result = SetData.ToList();
            foreach (var item in result)
            {
                var ccCompare = resultFromCCSProc.Where(x => x.DivisionName == item.DivisionName && x.EmployeeCode == item.EmployeeCode && x.EmployeeName == item.EmployeeName && x.ClientShortName == item.ClientShortName && x.Process == item.Process).ToList();

                var pcCompare = resultFromACSProc.Where(x => x.DivisionName == item.DivisionName && x.EmployeeCode == item.EmployeeCode && x.EmployeeName == item.EmployeeName && x.ClientShortName == item.ClientShortName && x.Process == item.Process).ToList();

                if (ccCompare.Count() == 1 && pcCompare.Count() == 1)
                {
                    var check = CCPASetData.Where(x => x.DivisionName == item.DivisionName && x.EmployeeCode == item.EmployeeCode && x.EmployeeName == item.EmployeeName && x.ClientShortName == item.ClientShortName && x.Process == item.Process).ToList();
                    if (check.Count() == 0)
                    {
                        var ccsingle = ccCompare.FirstOrDefault();
                        var pcsingle = pcCompare.FirstOrDefault();
                        hrvm.DivisionName = ccsingle.DivisionName;
                        hrvm.EmployeeCode = ccsingle.EmployeeCode;
                        hrvm.EmployeeName = ccsingle.EmployeeName;
                        hrvm.ClientShortName = ccsingle.ClientShortName;
                        hrvm.Process = ccsingle.Process;
                        hrvm.AC0 = pcsingle.P0;
                        hrvm.AC1 = pcsingle.P1;
                        hrvm.AC2 = pcsingle.P2;
                        hrvm.AC3 = pcsingle.P3;
                        hrvm.AC4 = pcsingle.P4;
                        hrvm.AC5 = pcsingle.P5;
                        hrvm.AC6 = pcsingle.P6;
                        hrvm.AC7 = pcsingle.P7;
                        hrvm.AC8 = pcsingle.P8;
                        hrvm.AC9 = pcsingle.P9;
                        hrvm.AC10 = pcsingle.P10;
                        hrvm.AC11 = pcsingle.P11;
                        hrvm.AC12 = pcsingle.P12;
                        hrvm.AC13 = pcsingle.P13;
                        hrvm.AC14 = pcsingle.P14;
                        hrvm.AC15 = pcsingle.P15;
                        hrvm.AC16 = pcsingle.P16;
                        hrvm.AC17 = pcsingle.P17;
                        hrvm.AC18 = pcsingle.P18;
                        hrvm.AC19 = pcsingle.P19;
                        hrvm.AC20 = pcsingle.P20;
                        hrvm.AC21 = pcsingle.P21;
                        hrvm.AC22 = pcsingle.P22;
                        hrvm.AC23 = pcsingle.P23;
                        hrvm.CC0 = ccsingle.C0;
                        hrvm.CC1 = ccsingle.C1;
                        hrvm.CC2 = ccsingle.C2;
                        hrvm.CC3 = ccsingle.C3;
                        hrvm.CC4 = ccsingle.C4;
                        hrvm.CC5 = ccsingle.C5;
                        hrvm.CC6 = ccsingle.C6;
                        hrvm.CC7 = ccsingle.C7;
                        hrvm.CC8 = ccsingle.C8;
                        hrvm.CC9 = ccsingle.C9;
                        hrvm.CC10 = ccsingle.C10;
                        hrvm.CC11 = ccsingle.C11;
                        hrvm.CC12 = ccsingle.C12;
                        hrvm.CC13 = ccsingle.C13;
                        hrvm.CC14 = ccsingle.C14;
                        hrvm.CC15 = ccsingle.C15;
                        hrvm.CC16 = ccsingle.C16;
                        hrvm.CC17 = ccsingle.C17;
                        hrvm.CC18 = ccsingle.C18;
                        hrvm.CC19 = ccsingle.C19;
                        hrvm.CC20 = ccsingle.C20;
                        hrvm.CC21 = ccsingle.C21;
                        hrvm.CC22 = ccsingle.C22;
                        hrvm.CC23 = ccsingle.C23;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (ccCompare.Count() == 1 && pcCompare.Count() == 0)
                {
                    var ccsingle = ccCompare.FirstOrDefault();
                    hrvm.DivisionName = ccsingle.DivisionName;
                    hrvm.EmployeeCode = ccsingle.EmployeeCode;
                    hrvm.EmployeeName = ccsingle.EmployeeName;
                    hrvm.ClientShortName = ccsingle.ClientShortName;
                    hrvm.Process = ccsingle.Process;
                    hrvm.AC0 = 0;
                    hrvm.AC1 = 0;
                    hrvm.AC2 = 0;
                    hrvm.AC3 = 0;
                    hrvm.AC4 = 0;
                    hrvm.AC5 = 0;
                    hrvm.AC6 = 0;
                    hrvm.AC7 = 0;
                    hrvm.AC8 = 0;
                    hrvm.AC9 = 0;
                    hrvm.AC10 = 0;
                    hrvm.AC11 = 0;
                    hrvm.AC12 = 0;
                    hrvm.AC13 = 0;
                    hrvm.AC14 = 0;
                    hrvm.AC15 = 0;
                    hrvm.AC16 = 0;
                    hrvm.AC17 = 0;
                    hrvm.AC18 = 0;
                    hrvm.AC19 = 0;
                    hrvm.AC20 = 0;
                    hrvm.AC21 = 0;
                    hrvm.AC22 = 0;
                    hrvm.AC23 = 0;
                    hrvm.CC0 = ccsingle.C0;
                    hrvm.CC1 = ccsingle.C1;
                    hrvm.CC2 = ccsingle.C2;
                    hrvm.CC3 = ccsingle.C3;
                    hrvm.CC4 = ccsingle.C4;
                    hrvm.CC5 = ccsingle.C5;
                    hrvm.CC6 = ccsingle.C6;
                    hrvm.CC7 = ccsingle.C7;
                    hrvm.CC8 = ccsingle.C8;
                    hrvm.CC9 = ccsingle.C9;
                    hrvm.CC10 = ccsingle.C10;
                    hrvm.CC11 = ccsingle.C11;
                    hrvm.CC12 = ccsingle.C12;
                    hrvm.CC13 = ccsingle.C13;
                    hrvm.CC14 = ccsingle.C14;
                    hrvm.CC15 = ccsingle.C15;
                    hrvm.CC16 = ccsingle.C16;
                    hrvm.CC17 = ccsingle.C17;
                    hrvm.CC18 = ccsingle.C18;
                    hrvm.CC19 = ccsingle.C19;
                    hrvm.CC20 = ccsingle.C20;
                    hrvm.CC21 = ccsingle.C21;
                    hrvm.CC22 = ccsingle.C22;
                    hrvm.CC23 = ccsingle.C23;
                }
                if (ccCompare.Count() == 0 && pcCompare.Count() == 1)
                {
                    var pcsingle = pcCompare.FirstOrDefault();
                    hrvm.DivisionName = pcsingle.DivisionName;
                    hrvm.EmployeeCode = pcsingle.EmployeeCode;
                    hrvm.EmployeeName = pcsingle.EmployeeName;
                    hrvm.ClientShortName = pcsingle.ClientShortName;
                    hrvm.Process = pcsingle.Process;
                    hrvm.AC0 = pcsingle.P0;
                    hrvm.AC1 = pcsingle.P1;
                    hrvm.AC2 = pcsingle.P2;
                    hrvm.AC3 = pcsingle.P3;
                    hrvm.AC4 = pcsingle.P4;
                    hrvm.AC5 = pcsingle.P5;
                    hrvm.AC6 = pcsingle.P6;
                    hrvm.AC7 = pcsingle.P7;
                    hrvm.AC8 = pcsingle.P8;
                    hrvm.AC9 = pcsingle.P9;
                    hrvm.AC10 = pcsingle.P10;
                    hrvm.AC11 = pcsingle.P11;
                    hrvm.AC12 = pcsingle.P12;
                    hrvm.AC13 = pcsingle.P13;
                    hrvm.AC14 = pcsingle.P14;
                    hrvm.AC15 = pcsingle.P15;
                    hrvm.AC16 = pcsingle.P16;
                    hrvm.AC17 = pcsingle.P17;
                    hrvm.AC18 = pcsingle.P18;
                    hrvm.AC19 = pcsingle.P19;
                    hrvm.AC20 = pcsingle.P20;
                    hrvm.AC21 = pcsingle.P21;
                    hrvm.AC22 = pcsingle.P22;
                    hrvm.AC23 = pcsingle.P23;
                    hrvm.CC0 = 0;
                    hrvm.CC1 = 0;
                    hrvm.CC2 = 0;
                    hrvm.CC3 = 0;
                    hrvm.CC4 = 0;
                    hrvm.CC5 = 0;
                    hrvm.CC6 = 0;
                    hrvm.CC7 = 0;
                    hrvm.CC8 = 0;
                    hrvm.CC9 = 0;
                    hrvm.CC10 = 0;
                    hrvm.CC11 = 0;
                    hrvm.CC12 = 0;
                    hrvm.CC13 = 0;
                    hrvm.CC14 = 0;
                    hrvm.CC15 = 0;
                    hrvm.CC16 = 0;
                    hrvm.CC17 = 0;
                    hrvm.CC18 = 0;
                    hrvm.CC19 = 0;
                    hrvm.CC20 = 0;
                    hrvm.CC21 = 0;
                    hrvm.CC22 = 0;
                    hrvm.CC23 = 0;
                }
                CCPASetData.Add(new HourlyReportVM
                {
                    DivisionName = hrvm.DivisionName,
                    EmployeeCode = hrvm.EmployeeCode,
                    EmployeeName = hrvm.EmployeeName,
                    ClientShortName = hrvm.ClientShortName,
                    Process = hrvm.Process,
                    AC0 = hrvm.AC0,
                    AC1 = hrvm.AC1,
                    AC2 = hrvm.AC2,
                    AC3 = hrvm.AC3,
                    AC4 = hrvm.AC4,
                    AC5 = hrvm.AC5,
                    AC6 = hrvm.AC6,
                    AC7 = hrvm.AC7,
                    AC8 = hrvm.AC8,
                    AC9 = hrvm.AC9,
                    AC10 = hrvm.AC10,
                    AC11 = hrvm.AC11,
                    AC12 = hrvm.AC12,
                    AC13 = hrvm.AC13,
                    AC14 = hrvm.AC14,
                    AC15 = hrvm.AC15,
                    AC16 = hrvm.AC16,
                    AC17 = hrvm.AC17,
                    AC18 = hrvm.AC18,
                    AC19 = hrvm.AC19,
                    AC20 = hrvm.AC20,
                    AC21 = hrvm.AC21,
                    AC22 = hrvm.AC22,
                    AC23 = hrvm.AC23,
                    CC0 = hrvm.CC0,
                    CC1 = hrvm.CC1,
                    CC2 = hrvm.CC2,
                    CC3 = hrvm.CC3,
                    CC4 = hrvm.CC4,
                    CC5 = hrvm.CC5,
                    CC6 = hrvm.CC6,
                    CC7 = hrvm.CC7,
                    CC8 = hrvm.CC8,
                    CC9 = hrvm.CC9,
                    CC10 = hrvm.CC10,
                    CC11 = hrvm.CC11,
                    CC12 = hrvm.CC12,
                    CC13 = hrvm.CC13,
                    CC14 = hrvm.CC14,
                    CC15 = hrvm.CC15,
                    CC16 = hrvm.CC16,
                    CC17 = hrvm.CC17,
                    CC18 = hrvm.CC18,
                    CC19 = hrvm.CC19,
                    CC20 = hrvm.CC20,
                    CC21 = hrvm.CC21,
                    CC22 = hrvm.CC22,
                    CC23 = hrvm.CC23,
                });
            }
            return CCPASetData;
        }

        public Object GetTATReport(TATPara para)
        {
          
            TATViewModel tat = new TATViewModel();
            var SetData = new List<TATViewModel>();
            string Process = "";
            string Status = "";
            var sqlpara = new SqlParameter[]
                        {
                            new SqlParameter("@date",  para.Date.Date),
                            new SqlParameter("@department", (object)para.DepartmentId ?? DBNull.Value),
                            new SqlParameter("@client", (object)para.ClientId ?? DBNull.Value)
                        };

            var spresult = _procedureWork.ExecStoredProcedure<sp_TATReport_Result>("sp_TATReport @date,@department,@client", sqlpara).ToList();

            foreach (var item in spresult)
            {
                var pwtrow = db.ProcessWorkFlowTrans.Where(x => x.Jid == item.JId && x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.Id).FirstOrDefault();

                if (pwtrow != null)
                {
                    var processrow = db.Processes.FirstOrDefault(x => x.Id == pwtrow.ProcessId);
                    if (processrow != null)
                    {
                        Process = processrow.Name;
                    }
                    else
                    {
                        Process = "Client";
                    }
                    var statusrow = db.Statuses.FirstOrDefault(x => x.Id == pwtrow.StatusId && x.IsActive == true);
                    if (statusrow != null)
                    {
                        Status = statusrow.Name;
                    }
                }
                SetData.Add(new TATViewModel
                {
                    JId = item.JId,
                    JobSubmittedDateEST = item.JobSubmittedDateEST,
                    DateofUploadEST = item.DateofUploadEST,
                    TATinHHMM = item.TATinHHMM,
                    JobId = item.JobId,
                    FileName = item.FileName,
                    Scope = item.Scope,
                    StitchCount = item.StitchCount,
                    TATExceed = item.TATExceed,
                    Process = Process,
                    Status = Status
                });
            }
            return SetData;
        }
    }
}

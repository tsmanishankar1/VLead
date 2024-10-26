using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase 
    {
        private readonly ReportService _reportService;
        private readonly Service.ReportService _reportService1;
        private readonly DashboardReportService _dashboardReportService;
        private readonly Service.ReportTrialCilentSummaryService _reportTrialCilentSummaryService;
        public ReportController(DashboardReportService dashboardReportService,ReportService reportService, Service.ReportTrialCilentSummaryService reportTrialCilentSummaryService, Service.ReportService reportService1)
        {
            _reportService = reportService;
            _dashboardReportService = dashboardReportService;
            _reportTrialCilentSummaryService = reportTrialCilentSummaryService;
            _reportService1 = reportService1;
        }
        [HttpGet]
        [Route("GetProcess")]
        public Object GetProcessHeader()
        {
            return _reportService.GetProcess();
        }
        [HttpGet]
        [Route("GetLocationForJobOut")]
        public Object GetLocationForJobOut()
        {
            return _reportService.GetLocationForJobOut();
        }
        [HttpPost]
        [Route("GetJobOrderDetailsList")]
        public Object GetJobOrderDetailsList(JobOrderDetailsViewModel oTrayDetailMovement)
        {
            var Getsummary = _reportService.GetJobOrderDetailsList(oTrayDetailMovement);
            var GetJobSummary = new
            {
                JobOrderDetailsReport = Getsummary,
            };
            return GetJobSummary;
        }
        [HttpGet]
        [Route("GetCustomerWithShortNameAndFullName")]
        public Object GetCustomerWithShortNameAndFullName()
        {
            return _reportService.GetCustomerWithShortNameAndFullName();
        }
        [HttpGet]
        [Route("GetScope")]
        public Object GetScope()
        {
            return _reportService.GetScope();
        }
        [HttpPost]
        [Route("GetDashboardReport")]
        public Object GetInvoiceListingReport(DashboardReportViewModel dashboardReportViewModel)
        {
            var res = new object();
            UpdateDashboardReportViewModelDatesBasedOnComparisonType(dashboardReportViewModel);
            switch (dashboardReportViewModel.ReportName)
            {
                case "Monthly Invoice Completed":

                    res = _reportService.GetInvoiceListingReport(dashboardReportViewModel);
                    break;
                case "Month Wise Revenue":
                    res = _reportService.GetMonthWiseRevenueReport(dashboardReportViewModel);
                    break;
                case "Revenue Analysis":
                    res = _reportService.GetRevenueAnalysisReport(dashboardReportViewModel);
                    break;
            }
            var result = new
            {
                Records = res,
            };
            return result;
        }
        [HttpPost]
        [Route("GetTrendReport")]
        public Object GetListOfTrendReport(TrendReportViewModel trendReportViewModel)
        {
            var res = _reportService.GetListOfTrendReport(trendReportViewModel);
            return res;
        }
        private void UpdateDashboardReportViewModelDatesBasedOnComparisonType(DashboardReportViewModel dashboardReportViewModel)
        {
            var fromDate = dashboardReportViewModel.FromDate;
            var toDate = dashboardReportViewModel.ToDate;

            switch (dashboardReportViewModel.ComparisonType)
            {
                case "period":

                    dashboardReportViewModel.FromDate = fromDate.Date;
                    dashboardReportViewModel.ToDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                    break;

                case "monthComparison":

                    dashboardReportViewModel.FromDate = new DateTime(fromDate.Year, fromDate.Month, 1);
                    dashboardReportViewModel.ToDate = new DateTime(toDate.Year, toDate.Month, DateTime.DaysInMonth(toDate.Year, toDate.Month), 23, 59, 59);
                    break;
                case "quarterComparison":
                    switch (dashboardReportViewModel.FromComparison)
                    {
                        case "Q1":
                            dashboardReportViewModel.FromDate = new DateTime(fromDate.Year, 4, 1);
                            dashboardReportViewModel.ToDate = new DateTime(toDate.Year, 4, 1);
                            break;
                        case "Q2":
                            dashboardReportViewModel.FromDate = new DateTime(fromDate.Year, 7, 1);
                            dashboardReportViewModel.ToDate = new DateTime(toDate.Year, 7, 1);
                            break;
                        case "Q3":
                            dashboardReportViewModel.FromDate = new DateTime(fromDate.Year, 10, 1);
                            dashboardReportViewModel.ToDate = new DateTime(toDate.Year, 10, 1);
                            break;
                        case "Q4":
                            dashboardReportViewModel.FromDate = new DateTime(fromDate.Year, 1, 1);
                            dashboardReportViewModel.ToDate = new DateTime(toDate.Year, 1, 1);
                            break;
                        default:
                            break;
                    }
                    break;
                case "halfYearComparison":
                    switch (dashboardReportViewModel.FromComparison)
                    {
                        case "H1":
                            dashboardReportViewModel.FromDate = new DateTime(fromDate.Year, 4, 1);
                            dashboardReportViewModel.ToDate = new DateTime(toDate.Year, 4, 1);
                            //dashboardReportViewModel.ToDate = new DateTime(toDate.Year,10, 1);
                            break;
                        case "H2":
                            dashboardReportViewModel.FromDate = new DateTime(fromDate.Year, 10, 1);
                            dashboardReportViewModel.ToDate = new DateTime(toDate.Year, 10, 1);
                            //dashboardReportViewModel.ToDate = new DateTime(toDate.Year, 03, 1);
                            break;
                        default:
                            break;
                    }
                    break;
                case "annualComparison":
                    dashboardReportViewModel.FromDate = new DateTime(fromDate.Year, 4, 1);
                    //dashboardReportViewModel.ToDate = new DateTime(toDate.Year, 12, DateTime.DaysInMonth(toDate.Year, toDate.Month), 23, 59, 59);
                    dashboardReportViewModel.ToDate = new DateTime(toDate.Year, 4, 1);
                    break;
                default:
                    break;
            }
        }
        [HttpPost]
        [Route("GetRevenuePerHeadReport")]
        public Object GetRevenuePerHeadReport(GetRevenuePerHeadReportViewModel getRevenuePerHeadReportViewModel)
        {
            var res = _dashboardReportService.GetRevenuePerHeadReport(getRevenuePerHeadReportViewModel);
            return res;
        }
        [HttpPost]
        [Route("GetUnbilledClientReport")]
        public Object GetUnbilledClientReport(GetUnbilledClientReportViewModel getUnbilledClientReportViewModel)
        {
            var res = _dashboardReportService.GetUnbilledClientReport(getUnbilledClientReportViewModel);
            return res;
        }
        [HttpPost]
        [Route("GetTATFileStatus")]
        public Object GetTATFileStatusReport(InitialLoad GetFileStatusReport)
        {
            var Getsummary = _reportService.GetTATFileStatusReports(GetFileStatusReport);
            var GetJobSummary = new
            {
                GatFileStatusList = Getsummary,
            };
            return GetJobSummary;
        }
        [HttpPost]
        [Route("GetFileStatusReportsForRef")]
        public Object GetFileStatusReportsForRef(FileStatusReportModifyViewModel GetFileStatusReport)
        {
            var Getsummary = _reportService.GetFileStatusReportsForRef(GetFileStatusReport);

            var osFiles = Getsummary.Select(x => x.Fileasonpereviousday).Sum();
            var osPD = Getsummary.Select(x => x.FileasReceivedonpereviousday).Sum();
            var TodayFilesReceivedFiles = Getsummary.Select(x => x.TodayFilesReceived).Sum();
            var TotalFileReceivedFiles = Getsummary.Select(x => x.TotalFileReceived).Sum();
            var QuerySentFiles = Getsummary.Select(x => x.QuerySent).Sum();
            var QueryResponseFiles = Getsummary.Select(x => x.QueryResponse).Sum();
            var TotalFilesCount = TotalFileReceivedFiles + QueryResponseFiles;
            var ClientCoordinationFiles = Getsummary.Select(x => x.ClientCoordination).Sum();
            var ProdnAllocationFiles = Getsummary.Select(x => x.ProdnAllocation).Sum();
            var ProductionFiles = Getsummary.Select(x => x.Production).Sum();
            var QualityFiles = Getsummary.Select(x => x.Quality).Sum();
            var QualityAllocationFiles = Getsummary.Select(x => x.QualityAllocation).Sum();
            var ProofReadingAllocationFiles = Getsummary.Select(x => x.ProofReadingAllocation).Sum();
            var ProofReadingFiles = Getsummary.Select(x => x.ProofReading).Sum();
            var BuddyProofAllocationFiles = Getsummary.Select(x => x.BuddyProofAllocation).Sum();
            var BuddyProofFiles = Getsummary.Select(x => x.BuddyProof).Sum();
            var sewOutallocationFiles = Getsummary.Select(x => x.sewOutallocation).Sum();
            var SewOutFiles = Getsummary.Select(x => x.SewOut).Sum();
            var CanDupFiles = Getsummary.Select(x => x.CancelledJobs).Sum();
            var FileUploaded = Getsummary.Select(x => x.FileUploaded).Sum();
            var TotalPending = (osFiles + TotalFilesCount) - FileUploaded;

            var GetJobSummary = new
            {
                GatFileStatusList = Getsummary,
                OSFiles = osFiles,
                OSPD = osPD,
                TR = TodayFilesReceivedFiles,
                RF = TotalFileReceivedFiles,
                QS = QuerySentFiles,
                QR = QueryResponseFiles,
                TFC = TotalFilesCount,
                CC = ClientCoordinationFiles,
                PAF = ProdnAllocationFiles,
                PF = ProductionFiles,
                QF = QualityFiles,
                QAF = QualityAllocationFiles,
                PRAF = ProofReadingAllocationFiles,
                PRF = ProofReadingFiles,
                BPAF = BuddyProofAllocationFiles,
                BPF = BuddyProofFiles,
                SOAF = sewOutallocationFiles,
                SOF = SewOutFiles,
                CanF = CanDupFiles,
                FUF = FileUploaded,
                TOF = TotalPending,
            };
            return GetJobSummary;
        }
        [HttpPost]
        [Route("UnbilledRevenueReport")]
        public Object UnbilledRevenueReport(UnbilledRevenueReportViewModel unbilledRevenueReportViewModel)
        {
            var res = _reportService.UnbilledRevenueReport(unbilledRevenueReportViewModel);
            return res;
        }
       
        [HttpPost]
        [Route("ClientwiseThresholdFCReport")]
        public IActionResult ClientwiseThresholdFCReport(InputForClientwiseThresholdFCReport input)
        {
            var result =_reportService1.GetClientwiseThresholdFCReport(input.period, input.CustomerId, input.department);
            return Ok(result);
        }

        public class InputForClientwiseThresholdFCReport
        {
            public int period { get; set; }
            public int[] CustomerId { get; set; }

            public int department { get; set; }

        }
        [HttpPost]
        [Route("GetQuotationTrackerReport")]
        public Object GetQuotationTrackerReport(DailyReportViewModel GetQuotationTrackerReport)
        {
            var Getsummary = _reportService.GetQuotationTrackerReport(GetQuotationTrackerReport);
            var GetQuotationTrackerSummary = new
            {
                QuotationTrackerList = Getsummary,
            };
            return GetQuotationTrackerSummary;
        }
        [HttpPost]
        [Route("GetClientStatusReport")]
        public Object GetClientStatusReport(ClientStatusReportViewModel clientStatusReportViewModel)
        {
            var res = _reportService.GetClientStatusReport(clientStatusReportViewModel);
            return res;
        }
        [HttpPost]
        [Route("GetNewClientReport")]
        public Object GetNewClientReport(NewClientReportViewModel newClientReportViewModel)
        {
            var res = _reportService.GetNewClientReport(newClientReportViewModel);
            return res;

        }
        [HttpPost]
        [Route("getTrialClientReport")]
        public Object GetTrialClientReport(TrialClientReportViewModel trialClientReportViewModel)
        {
            var res = _reportService.GetTrialClientReport(trialClientReportViewModel);
            return res;
        }
        [HttpPost]
        [Route("getTrialClientSummaryReport")]
        public IActionResult GetTrialClientSummaryReport(InputForTrialClientSummaryReport input)
        {
            var result = _reportTrialCilentSummaryService.GetTrialClientSummaryReport(input.fromDate, input.toDate, input.CustomerId, input.department);
            return Ok(result);
        }
        [HttpPost]
        [Route("GetRevisionCountReport")]
        public Object GetRevisionCountReport(RevisionCountReportViewModel revisionCountReportViewModel)
        {
            var res = _reportService.GetRevisionCountReport(revisionCountReportViewModel);
            return res;

        }
        [HttpPost]
        [Route("GetEmployeeErrorstatus")]
        public Object GetEmployeeErrorstatus(EmployeeErrorReport errorparams)
        {
            var Getsummary = _reportService.GetEmployeeErrorstatus(errorparams);
            var GetJobSummary = new
            {
                totalFileerror = Getsummary,
            };
            return GetJobSummary;
        }
        [HttpPost]
        [Route("GetIntakeAndUploadedReport")]
        public Object GetIntakeAndUploadedReport(IntakeAndUploadedReportViewModel intakeAndUploadedReportViewModel)
        {
            var res = _reportService.GetIntakeAndUploadedReport(intakeAndUploadedReportViewModel);
            return res;
        }
        [HttpPost]
        [Route("GetCancelledJobReport")]
        public Object GetCancelledJobsReport(DailyReportViewModel GetCancelledJobStatusReport)
        {
            var Getsummary = _reportService.GetCancelledJobsReport(GetCancelledJobStatusReport);
            var GetCancelledJobSummary = new
            {
                CancelledJobStatusList = Getsummary,
            };
            return GetCancelledJobSummary;
        }
        [HttpPost]
        [Route("GetOSFileStatusReport")]
        public Object GetOSFileStatusReport(FileStatusReportViewModel GetFileStatusReport)
        {
            var Getsummary = _reportService.GetOSFileStatusReport(GetFileStatusReport);
            var GetJobSummary = new
            {
                GatFileStatusList = Getsummary,
            };
            return GetJobSummary;
        }
        [HttpPost]
        [Route("GetQuerySentReport")]
        public Object GetQuerySentReport(FileStatusReportViewModel GetFileStatusReport)
        {
            var Getsummary = _reportService.GetQuerySentReport(GetFileStatusReport);
            var GetJobSummary = new
            {
                GatFileStatusList = Getsummary,
            };
            return GetJobSummary;
        }
        [HttpPost]
        [Route("GetEmployeeQCErrorstatus")]
        public Object GetEmployeeQCErrorstatus(EmployeeErrorReport errorparams)
        {
            var Getsummary = _reportService.GetQCEmployeeErrorstatus(errorparams);
            var GetJobSummary = new
            {
                totalFileerror = Getsummary,
            };
            return GetJobSummary;
        }
        [HttpPost]
        [Route("GetFTEUtilizationReport")]
        public Object GetFTEUtilizationReport(FTEUtilizationReportViewModel fteUtilizationReportViewModel)
        {
            var res = _reportService.GetFTEUtilizationReport(fteUtilizationReportViewModel);
            return res;
        }
        [HttpPost]
        [Route("GetFTEUtilizationwithClientwiseReport")]
        public Object GetFTEUtilizationwithClientwiseReport(FTEUtilizationwithClientWiseReportViewModel fteUtilizationwithClientWiseReportViewModel)
        {
            var res = _reportService.GetFTEUtilizationwithClientwiseReport(fteUtilizationwithClientWiseReportViewModel);
            return res;
        }

        //sew out
        [HttpPost]
        [Route("GetSewOutDetails")]
        public Object GetSewOutDetailsList(JobOrderDetailsViewModel oTrayDetailMovement)
        {
            var Getsummary = _reportService.GetSewOutDetailsList(oTrayDetailMovement);
            var GetJobSummary = new
            {
                TotalGetEWRList = Getsummary,
            };
            return GetJobSummary;
        }

        // collection report
        [HttpPost]
        [Route("GetCollectionsReport")]
        public Object GetCollectionsReportDetails(JobOrderDetailsViewModel getinputs)
        {
            var GetCollections = _reportService.GetCollectionsReport(getinputs);
            var GetCollectionList = new
            {
                GetCollectionList = GetCollections,
            };
            return GetCollectionList;
        }
        //query tracker
        [HttpPost]
        [Route("GetQueryTrackerReport")]
        public Object GetQueryTrackerReport(JobOrderDetailsViewModel GetQueryTrackerReport)
        {
            var Getlist = _reportService.GetQueryTrackerReport(GetQueryTrackerReport);
            var GetQueryTrackerList = new
            {
                QueryTrackerList = Getlist,
            };
            return GetQueryTrackerList;
        }

        // QueryTracker Dept
        [HttpGet]
        [Route("GetUserDepartment")]
        public Object GetUserLogin(int userid)
        {
            var result = _reportService.GetUserLoginDepartment(userid);
            return new
            {
                result = result
            };
        }
        //hold status
        [HttpPost]
        [Route("GetHoldStatusReport")]
        public Object GetHoldStatusReportDetails(EmployeeWiseReport ewrparams)
        {
            var Getsummary = _reportService.GetHoldStatusDetailsReport(ewrparams);
            var GetJobSummary = new
            {
                HoldStatusList = Getsummary,
            };
            return GetJobSummary;
        }

        [HttpPost]
        [Route("GetQueryTrackerSummary")]
        public Object GetQueryTrackerSummary(DailyReportViewModel GetQueryTrackerReport)
        {
            var Getsummary = _reportService.GetQueryTrackerSummary(GetQueryTrackerReport);
            var GetQueryTrackerSummary = new
            {
                QueryTrackerSummaryList = Getsummary,
            };
            return GetQueryTrackerSummary;
        }

        [HttpPost]
        [Route("GetAssignedEmployees")]
        public Object GetAssignedEmployees(ViewJobAllocation CustomerJobType)
        {
            var getAssignedEmployees = _reportService.GetAssignedEmployees(CustomerJobType);
            var result = new
            {
                AssignedEmployees = getAssignedEmployees
            };
            return result;
        }
        [HttpPost]
        [Route("dailyProductionReport")]
        public Object DailyProductionReport(ViewDailyProductionReport test)
        {
            return _reportService.DailyProductionReport(test);
        }
        [HttpPost]
        [Route("GetEmployeewisetotalList1")]
        public Object GetEmployeewisetotalList1(EmployeeWiseReport oTrayDetailMovement)
        {
            var Getsummary = _reportService.GetEmployeewisetotalList1(oTrayDetailMovement);
            var GetJobSummary = new
            {
                TotalGetEWRList = Getsummary,
            };
            return GetJobSummary;
        }
        [HttpPost]
        [Route("GetEmployeeWiseGrandTotalList")]
        public Object GetEmployeeWiseGrandTotalList(EmployeeWiseReport ewrparams)
        {
            var Getsummary = _reportService.GetEmployeeWiseGrandTotalList(ewrparams);
            var GetJobSummary = new
            {
                GrandTotalGetEWRList = Getsummary,
            };
            return GetJobSummary;
        }
        [HttpGet]
        [Route("GetAllddlList")]
        public Object ddlDetailList()
        {
            return _reportService.GetAllDropDownList();
        }
        [HttpPost]
        [Route("GetEWRList")]
        public Object GetEWRList(EmployeeWiseReport oTrayDetailMovement)
        {
            var Getsummary = _reportService.GetEWRList(oTrayDetailMovement);
            var GetJobSummary = new
            {
                GetEWRList = Getsummary,
            };
            return GetJobSummary;
        }
        //DPR Report
        [HttpPost]
        [Route("GetDailyProductionStatusReport")]
        public Object GetDailyProductionStatusReport(DailyReportViewModel GetFileStatusReport)
        {
            var Getsummary = _reportService.GetDailyProductionStatusReport(GetFileStatusReport);
            var GetJobSummary = new
            {
                DailyStatusList = Getsummary,
            };
            return GetJobSummary;
        }
        //SingleTableDPR start
        [HttpPost]
        [Route("nGetDPRReport")]
        public IActionResult Post(DPRViewModel dprvm)
        {
            var Getsummary = _reportService.SP_SingleTableDPR(dprvm);
            var GetJobSummary = new
            {
                DPRList = Getsummary,
            };
            return Ok(GetJobSummary);
        }
        //SingleTableDPR end
        [HttpGet]
        [Route("nGetCCList")]
        public IActionResult Get()
        {
            var Getsummary = _reportService.GetCustomerClass();
            var GetJobSummary = new
            {
                CCList = Getsummary,
            };
            return Ok(GetJobSummary);
        }
        [HttpPost]
        [Route("GetCurrentTrayFileStatusReport")]
        public Object GetCurrentTrayProductionFileStatusReport(FileStatusReportViewModel GetFileStatusReport)
        {
            var Getsummary = _reportService.GetCurrentTrayProductionFileStatusReport(GetFileStatusReport);
            var GetJobSummary = new
            {
                GatFileStatusList = Getsummary,
            };
            return GetJobSummary;
        }
        //DashBoardData Sample
        //....................................
        [HttpPost]
        [Route("nGetDBSummaryList")]
        public IActionResult GetDashSummary(GetDashSummaryPara gdsp)
        {
            var Getsummary = _reportService.GetDashboardSummary(gdsp);
            var GetJobSummary = new
            {
                DBList = Getsummary,
            };
            return Ok(GetJobSummary);
        }
        [HttpGet]
        [Route("nGetCurrentDBSummary")]
        public IActionResult GetCurrentDashSummary()
        {
            var Getsummary = _reportService.GetCurrentDashboardSummary();
            var GetJobSummary = new
            {
                DBList = Getsummary,
            };
            return Ok(GetJobSummary);
        }
        [HttpPost]
        [Route("nGetDBSumList")]
        public IActionResult GetDashSummaryList(GetDashSummaryListPara gdsp)
        {
            var Getsummary = _reportService.GetDashBoardSummaryList(gdsp);
            var GetJobSummary = new
            {
                DBList = Getsummary,
            };
            return Ok(GetJobSummary);
        }
        [HttpPost]
        [Route("nGetCurrentDBSumList")]
        public IActionResult GetCurrentDashSummaryList(GetCurrentDashSummaryListPara gdsp)
        {
            var Getsummary = _reportService.GetCurrentDashboardSummaryList(gdsp);
            var GetJobSummary = new
            {
                DBList = Getsummary,
            };
            return Ok(GetJobSummary);
        }

        [HttpGet]
        [Route("nGetDBList")]
        public IActionResult GetDash()
        {
            var Getsummary = _reportService.GetDashboardData();
            var GetJobSummary = new
            {
                DBList = Getsummary,
            };
            return Ok(GetJobSummary);
        }

        [HttpGet]
        [Route("nGetDBFSRList")]
        public IActionResult GetDashFSR()
        {
            var Getsummary = _reportService.GetDashboardFSR();
            var GetJobSummary = new
            {
                DBList = Getsummary,
            };
            return Ok(GetJobSummary);
        }
        //....................................
        [HttpPost]
        [Route("nGetEmployeebyDivision")]
        public IActionResult GetEmployeebyDivision(HourlyPara para)
        {
            var GetEmpDivsummary = _reportService.GetEmployeebyDivision(para);
            var GetEmpDivSummary = new
            {
                DBEmpDivList = GetEmpDivsummary,
            };
            return Ok(GetEmpDivSummary);
        }
        [HttpPost]
        [Route("nGetHourlyReport")]
        public IActionResult GetHourlyReport(HourlyPara para)
        {
            var GetHourlysummary = _reportService.GetHourlyReport(para);
            var GetHourlySummary = new
            {
                DBHourlyList = GetHourlysummary,
            };
            return Ok(GetHourlySummary);
        }
        [HttpPost]
        [Route("nGetTATReport")]
        public IActionResult GetTATReport(TATPara para)
        {
            var GetTATsummary = _reportService.GetTATReport(para);
            var GetTATSummary = new
            {
                DBTATList = GetTATsummary,
            };
            return Ok(GetTATSummary);
        }

    }

}

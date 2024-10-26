using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Service
{
    public class ReportTrialCilentSummaryService
    {
        public ReportTrialCilentSummaryService(Vis.VleadProcessV3.Services.ReportService reportService,
            PricingService pricingService)
        {
            _reportService = reportService;
            _pricingService = pricingService;
        }
        private readonly Vis.VleadProcessV3.Services.ReportService _reportService;//= new Vis.VleadProcessV3.Services.ReportService();
        private readonly PricingService _pricingService;//= new PricingService();

        public IList<TrialClientMisSummaryReportViewModel> GetTrialClientSummaryReport(DateTime fromDate, DateTime toDate, int[] customerId, string department)
        {
            var trialClientMisSummaryResult = _reportService.GetTrialClientSummaryReport(fromDate, toDate, customerId, department);

            var result = new List<TrialClientMisSummaryReportViewModel>();

            var groupedResult = trialClientMisSummaryResult
                .GroupBy(x => new
                {
                    x.DepartmentId,
                    x.ClientId,
                    x.Client,
                    x.ScopeId,
                    x.Scope,
                })
                .Select(y => new
                {
                    DepartmentId = y.Key.DepartmentId.Value,
                    CustomerId = y.Key.ClientId,
                    CustomerName = y.Key.Client,
                    ScopeId = y.Key.ScopeId,
                    ScopeName = y.Key.Scope,

                });

            foreach (var item in groupedResult)
            {
                var viewModel = new TrialClientMisSummaryReportViewModel();
                viewModel.LatestDate = trialClientMisSummaryResult.First(x => x.DepartmentId == item.DepartmentId &&
                x.ClientId == item.CustomerId && x.ScopeId == item.ScopeId).FileReceivedDate;
                viewModel.FirstDate = trialClientMisSummaryResult.Last(x => x.DepartmentId == item.DepartmentId &&
                x.ClientId == item.CustomerId && x.ScopeId == item.ScopeId).FileReceivedDate;
                viewModel.DepartmentId = item.DepartmentId;
                viewModel.Client = item.CustomerName;
                viewModel.Scope = item.ScopeName;

                var price = _pricingService.GetPriceBy(item.DepartmentId, item.CustomerId.Value, item.ScopeId.Value);
                if (price != null && price > 0)
                {
                    viewModel.PricingGiven = "Yes";
                }
                else
                {
                    viewModel.PricingGiven = "No";
                }

                var filteredSummaryResult = trialClientMisSummaryResult.Where(x =>
                    x.DepartmentId == item.DepartmentId &&
                    x.ClientId == item.CustomerId
                    && x.ScopeId == item.ScopeId);
                var days = Convert.ToInt32((viewModel.LatestDate.Value - viewModel.FirstDate.Value).TotalDays);
                viewModel.NumberOfDaysInTransition = (days > 0) ? days : 1;
                viewModel.TotalFileCountReceivedTillDate = filteredSummaryResult.Count();
                viewModel.FreshFileCount = filteredSummaryResult.Sum(x => x.freshFileCount.Value);
                viewModel.FreshAvgStitchCount = (viewModel.FreshFileCount > 0) ? filteredSummaryResult.Sum(x => x.freshStitchCount.Value) / viewModel.FreshFileCount : 0;
                viewModel.FreshAvgTimeStamp = (viewModel.FreshFileCount > 0) ? filteredSummaryResult.Where(x => x.freshFileTimeTaken.HasValue).Sum(x => x.freshFileTimeTaken.Value) / viewModel.FreshFileCount : 0;
                viewModel.RevisionFileCount = filteredSummaryResult.Sum(x => x.revisionFileCount.Value);
                viewModel.RevisionAvgStitchCount = (viewModel.RevisionFileCount > 0) ? filteredSummaryResult.Sum(x => x.revisionStitchCount.Value) / viewModel.RevisionFileCount : 0;
                viewModel.RevisionAvgTimeStamp = (viewModel.RevisionFileCount > 0) ? filteredSummaryResult.Where(x => x.revisionFileTimeTaken.HasValue).Sum(x => x.revisionFileTimeTaken.Value) / viewModel.RevisionFileCount : 0;

                result.Add(viewModel);
            }
            return result;
        }
    }
}

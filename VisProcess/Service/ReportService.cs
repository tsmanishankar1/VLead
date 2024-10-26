using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Service
{
    public class ReportService
    {
        private readonly IConfiguration _configuration;
        private readonly Service.CustomerService _customerService;
        private readonly Service.DepartmentService _departmentService;
        private readonly Service.ScopeService _scopeService;
        public ReportService(IConfiguration configuration,DashboardReportService dashboardReportService, 
            Service.CustomerService customerService, Service.DepartmentService departmentService,Service.ScopeService scopeService)
        {
            _configuration = configuration;
            _dashboardReportService = dashboardReportService;// new DashboardReportService(configuration);
            _customerService = customerService;
            _departmentService = departmentService;
            _scopeService= scopeService;
        }
        private readonly DashboardReportService _dashboardReportService;//= new DashboardReportService();

        private IEnumerable<GetJobOrdersByPeriod_Result> GetJobOrdersByMonth(IList<GetJobOrdersByPeriod_Result> jobOrders, DateTime date, int clientId, int deptId, string fileType, int? scopeId)
        {
            return jobOrders.Where(x => (x.JobDate.Month == date.Month && x.JobDate.Year == date.Year) &&
            (x.ClientId == clientId && x.DepartmentId == deptId && x.CustomerJobType == fileType && x.ScopeId == scopeId)).OrderByDescending(x => x.JobDate);
        }

        public IList<ClientwiseFileCountThresholdReportViewModel> GetClientwiseThresholdFCReport(int totalMonths, int[] customerId, int department)
        {
            var jobOrders = _dashboardReportService.GetClientwiseThresholdFCReport(totalMonths, customerId, department);
            var jobOrdersViewModel = new List<ClientwiseFileCountThresholdReportViewModel>();
            var customerService = _customerService;
            var listingjoborders = jobOrders.Select(x =>
            new
            {
                ClientId = x.ClientId,
                DepartmentId = x.DepartmentId,
                FileType = x.CustomerJobType,
                ScopeId = x.ScopeId
            }
            ).Distinct().OrderBy(x => x.ClientId)
            .ThenBy(x => x.DepartmentId)
            .ThenBy(x => x.FileType)
            .ThenBy(x => x.ScopeId);

            var currentDate = DateTime.UtcNow.AddMinutes(-300).Date;
            foreach (var client in listingjoborders)
            {
                var customer = customerService.GetCustomer(client.ClientId.Value, true);
                var jobOrderVM = new ClientwiseFileCountThresholdReportViewModel
                {
                    ShortName = customer.ShortName,
                    Name = customer.Name,
                    Department = _departmentService.GetDepartment(client.DepartmentId).Description,
                    FileType = client.FileType,
                    ClientType = customer.CustomerClassification.Description,
                    Scope = (client.ScopeId.HasValue) ? _scopeService.GetScope(client.ScopeId.Value).Description : "-",
                    LastReceivedDate = null,
                    Months = new List<int>()
                };
                for (var i = 1; i <= totalMonths; i++)
                {
                    var jobOrdersByMonth = GetJobOrdersByMonth(jobOrders, currentDate.AddMonths(-i), client.ClientId.Value, client.DepartmentId, client.FileType, client.ScopeId);
                    var count = jobOrdersByMonth.Count();
                    if (count > 0 && jobOrderVM.LastReceivedDate == null)
                        jobOrderVM.LastReceivedDate = jobOrdersByMonth.FirstOrDefault().JobDate;
                    jobOrderVM.Months.Add(count);
                }

                var reversedMonths = (List<int>)jobOrderVM.Months;
                reversedMonths.Reverse();
                jobOrdersViewModel.Add(jobOrderVM);
            }
            return jobOrdersViewModel;
        }
    }
}

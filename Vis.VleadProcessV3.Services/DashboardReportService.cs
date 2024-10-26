using Microsoft.Data.SqlClient;
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
    public class DashboardReportService
    {
       
        private readonly ProcedureWork _procedureWork;
        public DashboardReportService(ProcedureWork procedureWork)
        {

            _procedureWork = procedureWork;
        }
        public Object GetRevenuePerHeadReport(GetRevenuePerHeadReportViewModel getRevenuePerHeadReportViewModel)
        {
            var fromDate = getRevenuePerHeadReportViewModel.fromDate;
            var lastToDate = new DateTime(fromDate.Year, fromDate.Month, DateTime.DaysInMonth(fromDate.Year, fromDate.Month));
            var sqlPara = new SqlParameter[]
            {
                new  SqlParameter("@fromDate",lastToDate),
            };
            var detailResult = _procedureWork.ExecStoredProcedure<GetMonthwiseRevenuePerHeadReport_Result>("GetMonthwiseRevenuePerHeadReport @fromDate", sqlPara).ToList();

            var res = new
            {
                allRevenuePerHeadResults = detailResult
            };
            return res;

        }
        public Object GetUnbilledClientReport(GetUnbilledClientReportViewModel getUnbilledClientReportViewModel)
        {
            var test = getUnbilledClientReportViewModel.fromDate;
            var fromDate = getUnbilledClientReportViewModel.fromDate;
            var finalFromDate = new DateTime(fromDate.Year, fromDate.Month, 1);
            var toDate = getUnbilledClientReportViewModel.toDate;
            var lastToDate = new DateTime(toDate.Year, toDate.Month, DateTime.DaysInMonth(toDate.Year, toDate.Month));
            var sqlPara = new SqlParameter[]
            {
                new  SqlParameter("@fromDate",finalFromDate),
                new  SqlParameter("@toDate",lastToDate),
                new  SqlParameter("@customerIds",string.Join(",", getUnbilledClientReportViewModel.CustomerId)),
                new  SqlParameter("@category",getUnbilledClientReportViewModel.category),
            };
            var detailResult = _procedureWork.ExecStoredProcedure<GetUnbilledClient_Result>("GetUnbilledClient @fromDate, @toDate, @customerIds, @category ", sqlPara).ToList();

            if (getUnbilledClientReportViewModel.Department != "All")
            {
                int departmentId = 0;
                if (getUnbilledClientReportViewModel.Department == "Artwork")
                {
                    departmentId = 1;
                }
                else if (getUnbilledClientReportViewModel.Department == "Digitizing")
                {
                    departmentId = 2;
                }
                detailResult = detailResult.Where(x => x.departmentId == departmentId).ToList();
            }

            var res = new
            {
                unbilledClientReport = detailResult
            };
            return res;

        }
        public IList<GetJobOrdersByPeriod_Result> GetClientwiseThresholdFCReport(int totalMonths, int[] customerId, int department)
        {
            var currentMonth = DateTime.UtcNow.Date.AddMonths(-1).Month;
            var currentYear = DateTime.UtcNow.Date.AddMonths(-1).Year;
            var firstday = new DateTime(currentYear, currentMonth, 1);
            var lastDay = new DateTime(currentYear, currentMonth, DateTime.DaysInMonth(currentYear, currentMonth));

            var jobOrders = new List<GetJobOrdersByPeriod_Result>();
            switch (totalMonths)
            {
                case 1:
                    var sqlPara1 = new SqlParameter[]
                    {
                        new  SqlParameter("@from",firstday),
                        new  SqlParameter("@to",lastDay),
                        new  SqlParameter("@customerId",string.Join(",", customerId)),
                        new  SqlParameter("@department",department)
                    };
                    jobOrders = _procedureWork.ExecStoredProcedure<GetJobOrdersByPeriod_Result>("GetJobOrdersByPeriod @from, @to, @customerId, @department", sqlPara1).ToList();
                    break;
                case 2:
                    var sqlPara2 = new SqlParameter[]
                    {
                        new  SqlParameter("@from",firstday.AddMonths(-1)),
                        new  SqlParameter("@to",lastDay),
                        new  SqlParameter("@customerId",string.Join(",", customerId)),
                        new  SqlParameter("@department",department)
                    };
                    jobOrders = _procedureWork.ExecStoredProcedure<GetJobOrdersByPeriod_Result>("GetJobOrdersByPeriod @from, @to, @customerId, @department", sqlPara2).ToList();
                    break;
                case 3:
                    var sqlPara3 = new SqlParameter[]
                    {
                        new  SqlParameter("@from",firstday.AddMonths(-2)),
                        new  SqlParameter("@to",lastDay),
                        new  SqlParameter("@customerId",string.Join(",", customerId)),
                        new  SqlParameter("@department",department)
                    };
                    jobOrders = _procedureWork.ExecStoredProcedure<GetJobOrdersByPeriod_Result>("GetJobOrdersByPeriod @from, @to, @customerId, @department", sqlPara3).ToList();
                    break;
                case 4:
                    var sqlPara4 = new SqlParameter[]
                    {
                        new  SqlParameter("@from",firstday.AddMonths(-3)),
                        new  SqlParameter("@to",lastDay),
                        new  SqlParameter("@customerId",string.Join(",", customerId)),
                        new  SqlParameter("@department",department)
                    };
                    jobOrders = _procedureWork.ExecStoredProcedure<GetJobOrdersByPeriod_Result>("GetJobOrdersByPeriod @from, @to, @customerId, @department", sqlPara4).ToList();
                    break;
                case 5:
                    var sqlPara5 = new SqlParameter[]
                    {
                        new  SqlParameter("@from",firstday.AddMonths(-4)),
                        new  SqlParameter("@to",lastDay),
                        new  SqlParameter("@customerId",string.Join(",", customerId)),
                        new  SqlParameter("@department",department)
                    };
                    jobOrders = _procedureWork.ExecStoredProcedure<GetJobOrdersByPeriod_Result>("GetJobOrdersByPeriod @from, @to, @customerId, @department", sqlPara5).ToList();
                    break;
                case 6:
                    var sqlPara6 = new SqlParameter[]
                    {
                        new  SqlParameter("@from",firstday.AddMonths(-5)),
                        new  SqlParameter("@to",lastDay),
                        new  SqlParameter("@customerId",string.Join(",", customerId)),
                        new  SqlParameter("@department",department)
                    };
                    jobOrders = _procedureWork.ExecStoredProcedure<GetJobOrdersByPeriod_Result>("GetJobOrdersByPeriod @from, @to, @customerId, @department", sqlPara6).ToList();
                    break;
                default:
                    break;
            }
            return jobOrders;
        }
    }
}

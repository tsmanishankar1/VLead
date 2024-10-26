using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.Models;
using Microsoft.Data.SqlClient;

namespace Vis.VleadProcessV3.Services
{
    public class PricingTriggerService
    {
        private readonly ApplicationDbContext db;
        private readonly TableWork tableWork;
        private readonly IConfiguration _configuration;
        private readonly ProcedureWork _procedureWork;

        public PricingTriggerService(ApplicationDbContext db, TableWork tableWork, IConfiguration configuration, ProcedureWork procedureWork)
        {
            this.db = db;
            this.tableWork = tableWork;
            this._configuration = configuration;
            this._procedureWork = procedureWork;
        }
        public List<GetPendingPricingDetailForSales_Result> GetPendingPricingForSales(int empId)
        {
            var sqlPara = new SqlParameter[]
            {
            new SqlParameter("@EmployeeId", empId),
            };
            var pendingPriceJobs = _procedureWork.ExecStoredProcedure<GetPendingPricingDetailForSales_Result>("GetPendingPricingDetailForSales @EmployeeId", sqlPara);
            if (pendingPriceJobs != null)
            {
                return pendingPriceJobs.ToList();
            }
            else
            {
                return new List<GetPendingPricingDetailForSales_Result>();
            }
        }

        public object GetCustomersWithNullPriceForScope(DateTime startDate, DateTime endDate, string customerIds)
        {
            var sqlPara = new SqlParameter[]
                {
            new SqlParameter("@fromdate", startDate),
            new SqlParameter("@todate", endDate),
            new SqlParameter("@customerIds", customerIds),
                };
            var pendingPriceDetail = _procedureWork.ExecStoredProcedure<GetCustomersWithNullPriceForScope_Result>("GetCustomersWithNullPriceForScope @fromdate, @todate, @customerIds", sqlPara);
            if (pendingPriceDetail != null)
            {
                return pendingPriceDetail.ToList();
            }
            else
            {
                return new List<GetCustomersWithNullPriceForScope_Result>();
            }
        }
        public List<CustomerVsSalesEmployee> GetCustomerBySalesEmployeeId(int employeeId)
        {
            var result = db.CustomerVsSalesEmployees.Where(x => x.EmployeeId == employeeId && x.IsDeleted == false && x.IsActive == true).ToList();
            if (result != null)
            {
                return result;
            }
            else
            {
                return new List<CustomerVsSalesEmployee>();
            }
        }

    }
}
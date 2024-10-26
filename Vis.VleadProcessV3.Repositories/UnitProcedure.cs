using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.Entity.Infrastructure;
using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.Repositories
{
    public class UnitProcedure
    {
        private readonly ApplicationDbContext context;
        public UnitProcedure(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }
        public virtual IEnumerable<GetGenerateforInvoice_Result> GetGenerateforInvoice(Nullable<int> clientid, Nullable<System.DateTime> fromdate, Nullable<System.DateTime> todate)
        {

            SqlParameter clientidParameter = new SqlParameter("@ClientId", clientid);
            SqlParameter fromdateParameter = new SqlParameter("@FromDate", fromdate);
            SqlParameter todateParameter = new SqlParameter("@ToDate", todate);
            var result = context.GetGenerateforInvoice_Results.FromSqlRaw("GetGenerateforInvoice @ClientId,@FromDate,@ToDate", clientidParameter, fromdateParameter, todateParameter).ToList();
           
            return result;
         
        }
        public virtual IEnumerable<GeneratedInvoice_Result> GeneratedInvoice(Nullable<int> invno)
        {
          
            SqlParameter invnoParameter = new SqlParameter("@ClientId", invno);
            var result = context.GeneratedInvoice_Results.FromSqlRaw("GeneratedInvoice @ClientId", invnoParameter).ToList();
   
            return result;
 
        }
        public virtual IEnumerable<GetStaffingAmount_Result> GetStaffingAmount(Nullable<int> cusid)
        {
           
            SqlParameter cusidParameter = new SqlParameter("@ClientId", cusid);
            var result = context.GetStaffingAmount_Results.FromSqlRaw("GetStaffingAmount @ClientId", cusidParameter).ToList();
          
            return result;
         
        }
        public virtual IEnumerable<GetCheckStaffing_Result> GetCheckStaffing(Nullable<int> cusid, Nullable<int> dept, Nullable<System.DateTime> efd)
        {
        
            SqlParameter cusidParameter = new SqlParameter("@ClientId", cusid);
            SqlParameter deptParameter = new SqlParameter("@DepartmentId", dept);
            SqlParameter efdParameter = new SqlParameter("@FileReceivedDate", efd);
            var result = context.GetCheckStaffing_Results.FromSqlRaw("GetCheckStaffing @ClientId,@DepartmentId,@FileReceivedDate", cusidParameter, deptParameter, efdParameter).ToList();
            
            return result;
           
        }
        public virtual IEnumerable<GetPricingforTandM_Result> GetPricingforTandM(Nullable<int> clientid, Nullable<int> deptid, Nullable<int> scopeid, Nullable<int> pricetype, Nullable<int> jstatusid, Nullable<int> between)
        {
            
            SqlParameter clientidParameter = new SqlParameter("@ClientId", clientid);
            SqlParameter deptidParameter = new SqlParameter("@DepartmentId", deptid);
            SqlParameter scopeidParameter = new SqlParameter("@ScopeId", scopeid);
            SqlParameter pricetypeParameter = new SqlParameter("@PricingTypeId", pricetype);
            SqlParameter jstatusidParameter = new SqlParameter("@JobStatusId", jstatusid);
            SqlParameter betweenParameter = new SqlParameter("@EstimatedTime", between);
            var result = context.GetPricingforTandM_Results.FromSqlRaw("GetPricingforTandM @ClientId,@DepartmentId,@ScopeId,@PricingTypeId,@JobStatusId,@EstimatedTime", clientidParameter, deptidParameter, scopeidParameter, pricetypeParameter, jstatusidParameter, betweenParameter).ToList();
          
            return result;
           
        }
        public virtual IEnumerable<GetRateforTandM_Result> GetRateforTandM(Nullable<int> clientid, Nullable<int> deptid, Nullable<int> scopeid, Nullable<int> pricetype, Nullable<int> jstatusid)
        {
         
            SqlParameter clientidParameter = new SqlParameter("@ClientId", clientid);
            SqlParameter deptidParameter = new SqlParameter("@DepartmentId", deptid);
            SqlParameter scopeidParameter = new SqlParameter("@ScopeId", scopeid);
            SqlParameter pricetypeParameter = new SqlParameter("@PricingTypeId", pricetype);
            SqlParameter jstatusidParameter = new SqlParameter("@JobStatusId", jstatusid);
            var result = context.GetRateforTandM_Results.FromSqlRaw("GetRateforTandM @ClientId,@DepartmentId,@ScopeId,@PricingTypeId,@JobStatusId", clientidParameter, deptidParameter, scopeidParameter, pricetypeParameter, jstatusidParameter).ToList();
           
            return result;
          
        }
        public virtual IEnumerable<GetDropInvoiceCancel_Result> GetDropInvoiceCancel(Nullable<int> cusid)
        {
          
            SqlParameter cusidParameter = new SqlParameter("@Id", cusid);
            var result = context.GetDropInvoiceCancel_Results.FromSqlRaw("GetDropInvoiceCancel @Id", cusidParameter).ToList();
            return result;
            
        }
        public virtual IEnumerable<GetTranMergeInvoiceCancel_Result> GetTranMergeInvoiceCancel(Nullable<int> imid)
        {
           
            SqlParameter imidParameter = new SqlParameter("@Id", imid);
            var result = context.GetTranMergeInvoiceCancel_Results.FromSqlRaw("GetTranMergeInvoiceCancel @Id", imidParameter).ToList();
           
            return result;
           
        }
        public virtual IEnumerable<GetIntegrationMaster_Result> GetIntegrationMaster(Nullable<int> clientid, Nullable<System.DateTime> fromdate, Nullable<System.DateTime> todate)
        {
         
            SqlParameter clientidParameter = new SqlParameter("@CustomerID", clientid);
            SqlParameter fromdateParameter = new SqlParameter("@FromDate", fromdate);
            SqlParameter todateParameter = new SqlParameter("@ToDate", todate);
            var result = context.GetIntegrationMaster_Results.FromSqlRaw("GetIntegrationMaster @CustomerID,@FromDate,@ToDate", clientidParameter, fromdateParameter, todateParameter).ToList();
           
            return result;
            
        }
        public virtual IEnumerable<GetIntegrationTran_Result> GetIntegrationTran(Nullable<int> imid)
        {
          
            SqlParameter imidParameter = new SqlParameter("@Id", imid);
            var result = context.GetIntegrationTran_Results.FromSqlRaw("GetIntegrationTran @Id", imidParameter).ToList();
           
            return result;
          
        }
  
      
        public virtual IEnumerable<GetReceiptsIntegrationTran_Result> GetReceiptsIntegrationTran(Nullable<int> RMID)
        {
         
            SqlParameter RMIDParameter = new SqlParameter("@Id", RMID);
            var result = context.GetReceiptsIntegrationTran_Results.FromSqlRaw("GetReceiptsIntegrationTran @Id", RMIDParameter).ToList();
         
            return result;
            
        }

        public virtual IEnumerable<WaiverJobforInvoice_Result> WaiverJobforInvoice(Nullable<int> departmentId, Nullable<int> customerId, string fileName)
        {
           
            SqlParameter departmentIdParameter = new SqlParameter("@DepartmentId", departmentId);
            SqlParameter customerIdParameter = new SqlParameter("@ClientId", customerId);
            SqlParameter fileNameParameter = new SqlParameter("@FileName", fileName);
            var result = context.WaiverJobforInvoice_Results.FromSqlRaw("WaiverJobforInvoice @DepartmentId,@ClientId,@FileName", departmentIdParameter, customerIdParameter, fileNameParameter).ToList();
          
            return result;
          
        }

 

        public virtual IEnumerable<SP_SingleTableDPR_Result> SP_SingleTableDPR(Nullable<System.DateTime> FromDate, Nullable<System.DateTime> ToDate, string TrayDetailName, string CustomerType, Nullable<int> Classification, Nullable<int> DepartmentId, Nullable<int> CustomerId, string Status, Nullable<int> EmployeeId)
        {
           
            SqlParameter FromDateParameter = new SqlParameter("@FromDate", FromDate);
            SqlParameter ToDateParameter = new SqlParameter("@ToDate", ToDate);
            SqlParameter TrayDetailNameParameter = new SqlParameter("@TrayDetailName", TrayDetailName);
            SqlParameter CustomerTypeParameter = new SqlParameter("@CustomerType", (object)CustomerType ?? DBNull.Value);
            SqlParameter ClassificationParameter = new SqlParameter("@Classification", (object)Classification ?? DBNull.Value);
            SqlParameter DepartmentIdParameter = new SqlParameter("@DepartmentId", (object)DepartmentId ?? DBNull.Value);
            SqlParameter CustomerIdParameter = new SqlParameter("@CustomerId", (object)CustomerId ?? DBNull.Value);
            SqlParameter StatusParameter = new SqlParameter("@Status", (object)Status ?? DBNull.Value);
            SqlParameter EmployeeIdParameter = new SqlParameter("@EmployeeId", (object)EmployeeId ?? DBNull.Value);
            var result = context.Database.SqlQueryRaw<SP_SingleTableDPR_Result>("SP_SingleTableDPR @FromDate,@ToDate,@TrayDetailName,@CustomerType,@Classification,@DepartmentId,@CustomerId,@Status,@EmployeeId", FromDateParameter, ToDateParameter, TrayDetailNameParameter, CustomerTypeParameter, ClassificationParameter, DepartmentIdParameter, CustomerIdParameter, StatusParameter, EmployeeIdParameter).ToList();

            
            return result;
         
        }
    }
}

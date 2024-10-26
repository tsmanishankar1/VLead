using Microsoft.Data.SqlClient;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class ClientOrderService
    {
        private readonly ViewWork _viewWork;
        private readonly TableWork _tableWork;
        private readonly ProcedureWork _procedureWork;
        private readonly TableWork _tow;
        public ClientOrderService(ViewWork viewWork,TableWork tableWork,ProcedureWork procedureWork)
        {

            _viewWork = viewWork;
            _tableWork = tableWork;
            _tow = tableWork;
            _procedureWork = procedureWork;
            
        }
        public ViewQuotationDetailsinJobOrder GetJobOrderOrder(string JobId)
        {
            return _viewWork.ViewQuotationDetailsinJobOrderRepository.Get(x => x.ParentJobId == JobId && x.SpecialPrice != 0).FirstOrDefault();
        }
        public JobQuery QueryDetails(int WFTId, int WFMId)
        {
            return _tableWork.JobQueryRepository.GetAllVal(x => x.Scope).Where(x => x.Wftid == WFTId && x.Wfmid == WFMId).FirstOrDefault();
        }
        public JobQuery QuotationDetails(string JobId)
        {
            return _tableWork.JobQueryRepository.GetAllVal(x => x.Scope).Where(x => x.JobId == JobId && x.SpecialPrice == 0).FirstOrDefault();
        }
        public IEnumerable<Department> GetDepartmentsForJO()
        {
            return _tableWork.DepartmentRepository.Get(x => (x.IsDeleted == false) && (x.Description.Contains("Art") || x.Description.Contains("Digi") || x.Description.Contains("Train"))).AsQueryable();
        }
        public IEnumerable<Division> GetDivisionForJO()
        {
            return _tableWork.DivisionRepository.Get(x => (x.IsDeleted == false)).AsQueryable();
        }
        public IEnumerable<FileInwardType> GetFileInwardTypeListForJO()
        {
            return _tableWork.FileInwardTypeRepository.Get(x => x.IsDeleted == false).AsQueryable();
        }
        public IEnumerable<JobStatus> GetJobStatusForJO()
        {
            return _tableWork.JobStatusRepository.Get(x => x.IsActive == true).AsQueryable();
        }
        public IEnumerable<Customer> GetCustomersForJO()
        {
            return _tableWork.CustomerRepository.Get(x => x.IsDeleted == false && x.IsBlacklisted == false && x.IsApproved == true).AsQueryable();
        }

        public IEnumerable<CustomerContact> GetCustomerContactsForJO(int custId)
        {
            return _tableWork.CustomerContactRepository.Get(x => x.IsDeleted == false && x.CustomerId == custId).AsQueryable();
        }

        public IEnumerable<ClientOrderCCJobs_Result> GetAllClientOrderExt(int actionType)
        {
            var sqlPara = new SqlParameter[]
            {
                new SqlParameter("@ActionType",actionType)
            };
            return _procedureWork.ExecStoredProcedure<ClientOrderCCJobs_Result>("ClientOrderCCJobs @ActionType", sqlPara);
        }
        public int GetAllClientOrderCounts(int actionType)
        {
            var sqlPara = new SqlParameter[]
            {
                new SqlParameter("@ActionType",actionType)
            };
            return _procedureWork.ExecStoredProcedure<ClientOrderCCJobs_Result>("ClientOrderCCJobs @ActionType", sqlPara).Count();
        }
        public ClientOrder GetClientOrder(int orderId, int needFileCopy)
        {
            var clientOrder = _tableWork.ClientOrderRepository.GetAllVal(x => x.JobStatusNavigation, x => x.ClientOrderExts).FirstOrDefault(x => x.OrderId == orderId);
            //if (clientOrder.FileUploadPath != null && needFileCopy == 1)
            //{
            //    var fileUpload = new FileUpload();
            //    fileUpload.CopyFilesFromServer(orderId, clientOrder.FileUploadPath, true);
            //} commented for downloadusfilesservice
            return clientOrder;
        }
        public JobQuery QueryDetailspost(Jobqueryfortesting gjobquery)
        {
            if (gjobquery.Wftid == 0)
            {
                return _tableWork.JobQueryRepository.GetAllVal(x => x.Scope).Where(x => x.Jid == gjobquery.Jid).OrderByDescending(x => x.Id).FirstOrDefault();
            }
            else
            {
                return _tableWork.JobQueryRepository.GetAllVal(x => x.Scope).Where(x => x.Wftid == gjobquery.Wftid && x.Jid == gjobquery.Jid && x.EstimatedTime != 0).FirstOrDefault();
            }
        }
        public Object AllApprovalListforSalesTrack(ApprovalViewModel Approval)
        {
         

                var SalesApprovedList = new List<JobApprovalExt>();
                try
                {
                    SalesApprovedList.AddRange(_tow.JobApprovalExtRepository.GetAllVal(x => x.Client, x => x.Employee1).ToList());

                }
                catch (Exception ex)
                {

                    Log addlog = new Log();
                    addlog.Module = "Client Order";
                    addlog.Description = "Error Msg Sales approval Track: " + ex.Message.ToString() + "-" + "Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source + "string";
                    addlog.Type = "Error Occ";
                    addlog.CreatedUtc = DateTime.UtcNow;
                    _tow.LogRepository.Insert(addlog);
                    _tow.SaveChanges();
                }
                return SalesApprovedList.OrderByDescending(x => x.Id);
           
        }

 
        public Object GetCustomerFeedbackDetails(CustomerFeedbackViewModel CusFeedbackRep)
        {
            var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@FromDate",  CusFeedbackRep.FromDate),
                        new SqlParameter("@ToDate", CusFeedbackRep.ToDate),
                        new SqlParameter("@ClientId", (object)CusFeedbackRep.CustomerId ?? DBNull.Value),
                        new SqlParameter("@CCId", (object)CusFeedbackRep.CustomerContactId ?? DBNull.Value),
                    };

            var result = _procedureWork.ExecStoredProcedure<GetCustomerFeedbackReportDetails_Result>("GetCustomerFeedbackReportDetails @FromDate,@ToDate,@ClientId,@CCId", sqlpara);
            return result;
        }

        public Object GetCreditApprovalBySalesProcess(int JobApprovalExtnId, int IsApprove, int EmployeeId)
        {
            bool IsApproved;
            string message;
            string ApprovalDescription;
            var result = _tableWork.JobApprovalExtRepository.Get(x => x.Id == JobApprovalExtnId).FirstOrDefault();

            if (IsApprove == 1)
            {
                IsApproved = true;
                message = "Approved Successfully";
                ApprovalDescription = "Approved";

            }
            else
            {
                IsApproved = false;
                message = "Rejected Successfully";
                ApprovalDescription = "Rejected";
            }
          
            result.IsApproved = IsApproved;
            result.ApprovedBy = EmployeeId;
            result.ApprovalType = ApprovalDescription;
            result.ApprovedDate = DateTime.UtcNow;
            _tableWork.JobApprovalExtRepository.Update(result);
            _tableWork.SaveChanges();
            return message;

        }
    }
}

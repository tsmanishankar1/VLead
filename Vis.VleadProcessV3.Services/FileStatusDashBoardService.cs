using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;

namespace Vis.VleadProcessV3.Services
{
    public class FileStatusDashBoardService
    {
        private readonly TableWork _tableWork;
        private readonly UnitWork _unitWork;
       
        private readonly ProcedureWork _procedureWork;

        public FileStatusDashBoardService( TableWork tableWork,UnitWork unitWork,ProcedureWork procedureWork)
        {
          
            _tableWork = tableWork;
            _unitWork = unitWork;

            _procedureWork = procedureWork;
        }

        public IQueryable<Department> GetAllDepartments()
        {
            return _unitWork.Department.OrderBy(x => x.Description).Where(x => x.IsDeleted == false).AsQueryable();
        }
        public IQueryable<Customer> GetAllCustomers()
        {
            return _unitWork.Customer.OrderBy(x => x.Name).Where(x => x.IsDeleted == false).AsQueryable();
        }
        public IEnumerable<DashBoardStatus_Result> GetStatus(int clientId, int departmentId)
        {
            DateTime utcdate = DateTime.ParseExact(DateTime.UtcNow.ToString("MM-dd-yyyy"), "MM-dd-yyyy", CultureInfo.InvariantCulture);
            var sqlPara = new SqlParameter[]
            {
                new  SqlParameter("@DepartmentId",departmentId),
                new  SqlParameter("@ClientId",clientId),
                new  SqlParameter ("@Utcdate", utcdate)
            };
            return _procedureWork.ExecStoredProcedure<DashBoardStatus_Result>("DashBoardStatus @Utcdate,@DepartmentId, @ClientId", sqlPara).ToList();
            // DashBoardStatus(utcdate, departmentId, clientId).ToList();
        }

        // Average Revenue
        public IEnumerable<AverageRevenue_Result> GetAverageRevenue()
        {
            return _procedureWork.ExecProcedureOrView<AverageRevenue_Result>("AverageRevenue").ToList();

        }
        //
        public IEnumerable<TrayWisePendingStatus_Result> GetTrayWiseStatusDashboard(int departmentId, Nullable<DateTime> fileDate)
        {
            //DateTime utcdate = DateTime.ParseExact(DateTime.UtcNow.ToString("MM-dd-yyyy"), "MM-dd-yyyy", CultureInfo.InvariantCulture);
  
            var sqlPara = new SqlParameter[]
           {
                new  SqlParameter("@DepartmentId",departmentId),
                new  SqlParameter ("@Filedate", fileDate)
           };

            /* failed to pass null value in filedate */
            if (sqlPara[1].Value == null)
            {
                sqlPara[1].Value = DBNull.Value;
            }
            /***************************************** Balaji *****************************************/
            return _procedureWork.ExecStoredProcedure<TrayWisePendingStatus_Result>("TrayWisePendingStatus @fileDate, @departmentId", sqlPara).ToList();
        }
        public IEnumerable<GetDetailedFileCount_Result> GetDetailedFileCount(DetailedFileCountReport DetailedFilecount)
        {

            if (DetailedFilecount.LiveTrial == null || DetailedFilecount.LiveTrial == string.Empty)
            {

                var sqlPara = new SqlParameter[]
               {
                new  SqlParameter("@ClientId",DetailedFilecount.ClientId),
                new  SqlParameter("@FileReceivedDate", DetailedFilecount.FileReceivedDate),
                new  SqlParameter("@CustomerClassficationId",DetailedFilecount.customerClassficationId),
                new  SqlParameter("@JobstatusId", DetailedFilecount.jobstatusId) ,
                new  SqlParameter("@DepartmentId", DetailedFilecount.DepartmentId),
                new  SqlParameter("@LiveTrial", string.Empty)
                };
                return _procedureWork.ExecStoredProcedure<GetDetailedFileCount_Result>("GetDetailedFileCount @ClientId, @FileReceivedDate, @CustomerClassficationId,@JobstatusId, @DepartmentId, @LiveTrial", sqlPara).ToList();
            }
            else
            {
                var sqlPara = new SqlParameter[]
              {
                new  SqlParameter("@ClientId",DetailedFilecount.ClientId),
                new  SqlParameter("@FileReceivedDate", DetailedFilecount.FileReceivedDate),
                new  SqlParameter("@CustomerClassficationId",DetailedFilecount.customerClassficationId),
                new  SqlParameter("@JobstatusId", DetailedFilecount.jobstatusId) ,
                new  SqlParameter("@DepartmentId", DetailedFilecount.DepartmentId),
                new  SqlParameter("@LiveTrial", DetailedFilecount.LiveTrial)
              };
                return _procedureWork.ExecStoredProcedure<GetDetailedFileCount_Result>("GetDetailedFileCount @ClientId, @FileReceivedDate, @CustomerClassficationId,@JobstatusId, @DepartmentId, @LiveTrial", sqlPara).ToList();
            }
        }
        public Object GetProcess()
        {
            var GetProcess = _tableWork.ProcessRepository.Get(x => x.IsActive == true).OrderBy(x => x.Id).ToList();
            var GetDepartment = _tableWork.DepartmentRepository.Get(x => (x.IsDeleted == false) && (x.Description.Contains("Artwork") || x.Description.Contains("Digi"))).OrderBy(x => x.Id).ToList();
            var GetJobStatus = _tableWork.JobStatusRepository.Get(x => x.IsActive == true && (x.Id == 5 || x.Id == 6 || x.Id == 9 || x.Id == 10)).OrderBy(x => x.Id).ToList();
            var GetCustomerClassification = _tableWork.CustomerClassificationRepository.Get(x => x.IsDeleted == false).OrderBy(x => x.Id).ToList();
            var GetList = new
            {
                ProcessList = GetProcess,
                DepartmentList = GetDepartment,
                JobStatus = GetJobStatus,
                CustomerClassification = GetCustomerClassification,
            };
            return GetList;
        }

    }
}


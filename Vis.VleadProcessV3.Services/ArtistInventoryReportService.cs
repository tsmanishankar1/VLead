using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class ArtistInventoryReportService
    {
        private readonly ProcedureWork _procedureWork;// = new ProcedureWork();
        private readonly ApplicationDbContext _context;
        private readonly TableWork _tableWork;
        private readonly TableWork _tbWork;

        public ArtistInventoryReportService(ProcedureWork procedureWork, ApplicationDbContext dbContext, TableWork tableWork, TableWork tbWork)
        {
            //_configuration= configuration;
            //_unitWork = unitWork;// new UnitWork(configuration);
            _procedureWork = procedureWork;// new ProcedureWork(configuration);
            _context = dbContext;
            _tableWork = tableWork;
            _tbWork=tbWork;
        }

        public Object GetArtistInventoryDeptReport(ArtistInventoryReportDeptViewModel jobArtistInventoryReportDept)
        {

            var jobArtistInventoryReportDeptWise = new List<ArtistInventoryReportDept_Result>();

            var sqlPara = new SqlParameter[]
            {
                new SqlParameter("@FileReceivedDate",jobArtistInventoryReportDept.FileReceivedDate),
                new SqlParameter("@DepartmentId",jobArtistInventoryReportDept.DepartmentId),
                new SqlParameter("@ClientId",jobArtistInventoryReportDept.ClientId),
                new SqlParameter("@jobstatusId",jobArtistInventoryReportDept.JobStatusId),
                new SqlParameter("@ClassificationId",jobArtistInventoryReportDept.ClassificationId)
            };

            jobArtistInventoryReportDeptWise = _procedureWork.ExecStoredProcedure<ArtistInventoryReportDept_Result>("ArtistInventoryReportTotal @FileReceivedDate,@DepartmentId, @ClientId, @jobstatusId, @ClassificationId", sqlPara).OrderBy(x => x.Department).ToList();

            var result = new
            {
                JobArtistInventoryReportDeptWise = jobArtistInventoryReportDeptWise
            };
            return result;
        }

        public Object GetArtistInventoryDivReport(ArtistInventoryReportDivViewModel jobArtistInventoryReportDiv)
        {

            var jobArtistInventoryReportDivWise = new List<ArtistInventoryReportDiv_Result>();

            var sqlPara = new SqlParameter[]
            {
                new SqlParameter("@FileReceivedDate",jobArtistInventoryReportDiv.FileReceivedDate),
                new SqlParameter("@DivisionId",jobArtistInventoryReportDiv.DivisionId),
                new SqlParameter("@ClientId",jobArtistInventoryReportDiv.ClientId),
                new SqlParameter("@jobstatusId",jobArtistInventoryReportDiv.JobStatusId),
                new SqlParameter("@ClassificationId",jobArtistInventoryReportDiv.ClassificationId)
            };

            jobArtistInventoryReportDivWise = _procedureWork.ExecStoredProcedure<ArtistInventoryReportDiv_Result>("ArtistInventoryRepDivTotal @FileReceivedDate,@DivisionId, @ClientId, @jobstatusId, @ClassificationId", sqlPara).OrderBy(x => x.Division).ToList();

            var result = new
            {
                JobArtistInventoryReportDivWise = jobArtistInventoryReportDivWise
            };
            return result;
        }


        public Object GetArtDigiOrderEntryInventory(ArtistInventoryReportDeptViewModel jobArtDigiOrderEntryInventory)
        {

            var jobArtDigiOrderEntryInventoryDetails = new List<ArtDigiOrderEntryInventory_Result>();

            var sqlPara = new SqlParameter[]
            {
                new SqlParameter("@FileReceivedDate",jobArtDigiOrderEntryInventory.FileReceivedDate),
                new SqlParameter("@DepartmentId",jobArtDigiOrderEntryInventory.DepartmentId),
                new SqlParameter("@ClientId",jobArtDigiOrderEntryInventory.ClientId),
                new SqlParameter("@jobstatusId",jobArtDigiOrderEntryInventory.JobStatusId),
                new SqlParameter("@ClassificationId",jobArtDigiOrderEntryInventory.ClassificationId)
            };

            jobArtDigiOrderEntryInventoryDetails = _procedureWork.ExecStoredProcedure<ArtDigiOrderEntryInventory_Result>("ArtDigiOrderEntryTotal @FileReceivedDate,@DepartmentId, @ClientId, @jobstatusId, @ClassificationId", sqlPara).OrderBy(x => x.Division).ToList();

            var result = new
            {
                JobArtworkDigitizingOrderEntryInventory = jobArtDigiOrderEntryInventoryDetails
            };
            return result;
        }


        public Object GetArtDigiFileCount(ArtistInventoryReportDeptViewModel jobArtDigiOrderFileCount)
        {

            var jobArtDigiFileCountDetails = new List<ArtDigiFileCount_Result>();

            var sqlPara = new SqlParameter[]
            {
                new SqlParameter("@FileReceivedDate",jobArtDigiOrderFileCount.FileReceivedDate),
                new SqlParameter("@DepartmentId",jobArtDigiOrderFileCount.DepartmentId),
                new SqlParameter("@ClientId",jobArtDigiOrderFileCount.ClientId),
                new SqlParameter("@jobstatusId",jobArtDigiOrderFileCount.JobStatusId),
                new SqlParameter("@ClassificationId",jobArtDigiOrderFileCount.ClassificationId)
            };

            jobArtDigiFileCountDetails = _procedureWork.ExecStoredProcedure<ArtDigiFileCount_Result>("ArtDigiFileCountTotal @FileReceivedDate,@DepartmentId, @ClientId, @jobstatusId, @ClassificationId", sqlPara).OrderBy(x => x.Division).ToList();

            var result = new
            {
                JobArtworkDigitizingFileCount = jobArtDigiFileCountDetails
            };
            return result;
        }
    }
}


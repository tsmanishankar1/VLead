using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Vis.VleadProcessV3.Models;

using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class FileStatusDashBoardController : ControllerBase
    {
        private readonly FileStatusDashBoardService _FileStatusDashBoardService;
        public FileStatusDashBoardController(FileStatusDashBoardService fileStatusDashBoardService)
        {
            _FileStatusDashBoardService = fileStatusDashBoardService;
        }

        [HttpGet]
        [Route("GetDepartmentforFilestatusDashboard")]
        public IActionResult GetAllDepartments()
        {
            var Departments = _FileStatusDashBoardService.GetAllDepartments();
            return Ok(Departments);
        }
        [HttpGet]
        [Route("GetClientforFilestatusDashboard")]
        public IActionResult GetAllCustomers()
        {
            var Customers = _FileStatusDashBoardService.GetAllCustomers();
            return Ok(Customers);
        }
        [HttpGet]
        [Route("GetFilestatusDashboard")]
        public IActionResult GetStatus(int clientId, int departmentId)
        {
            var Customers = _FileStatusDashBoardService.GetStatus(clientId, departmentId);
            var result = new
            {
                AllStatusCounts = Customers
            };
            return Ok(result);
        }

        [HttpGet]
        [Route("GetDashboardAverageRevenue")]
        public IActionResult GetAvgRevenue()
        {
            var AvgRevenue = _FileStatusDashBoardService.GetAverageRevenue();
            var result = new
            {
                AllAvgRevenue = AvgRevenue
            };
            return Ok(result);
        }


        [HttpGet]
        [Route("GetTrayWiseStatusDashboard")]
        public IActionResult GetTrayWiseStatusDashboard(int departmentId, Nullable<DateTime> fileDate)
        {
            var Customers = _FileStatusDashBoardService.GetTrayWiseStatusDashboard(departmentId, fileDate);
            var result = new
            {
                AllStatusCounts = Customers
            };
            return Ok(result);
        }
        [HttpPost]
        [Route("GetDetailedFileCount")]
        public IActionResult GetDetailedFileCount(DetailedFileCountReport DetailedFilecount)
        {
            var GetDetailedCustomers = _FileStatusDashBoardService.GetDetailedFileCount(DetailedFilecount);

            var completedfilestoday = GetDetailedCustomers.Select(x => x.Completed_Files_Today).Sum();
            var outstandingpendingfiles = GetDetailedCustomers.Select(x => x.Outstanding_Pending_Files).Sum();
            var pendingfilestoday = GetDetailedCustomers.Select(x => x.Pending_Files_Today).Sum();
            var querysent = GetDetailedCustomers.Select(x => x.Query_Sent).Sum();
            var querysenttilldate = GetDetailedCustomers.Select(x => x.Query_Sent_Till_Date).Sum();
            //var TotalFilesCount = TotalFileReceivedFiles + QueryResponseFiles;
            var receivedfilestilldate = GetDetailedCustomers.Select(x => x.Received_Files_Till_Date).Sum();
            var receivedfilestoday = GetDetailedCustomers.Select(x => x.Received_Files_Today).Sum();
            //var ProductionFiles = GetDetailedCustomers.Select(x => x.Production).Sum();
            //var QualityFiles = GetDetailedCustomers.Select(x => x.Quality).Sum();
            //var QualityAllocationFiles = GetDetailedCustomers.Select(x => x.QualityAllocation).Sum();
            //var ProofReadingAllocationFiles = GetDetailedCustomers.Select(x => x.ProofReadingAllocation).Sum();
            //var ProofReadingFiles = GetDetailedCustomers.Select(x => x.ProofReading).Sum();
            //var BuddyProofAllocationFiles = GetDetailedCustomers.Select(x => x.BuddyProofAllocation).Sum();
            //var BuddyProofFiles = GetDetailedCustomers.Select(x => x.BuddyProof).Sum();
            //var sewOutallocationFiles = GetDetailedCustomers.Select(x => x.sewOutallocation).Sum();
            //var SewOutFiles = GetDetailedCustomers.Select(x => x.SewOut).Sum();
            //var CanDupFiles = GetDetailedCustomers.Select(x => x.CancelledJobs).Sum();
            //var FileUploaded = GetDetailedCustomers.Select(x => x.FileUploaded).Sum();
            //var TotalPending = (osFiles + TotalFilesCount) - FileUploaded;

            var result = new
            {
                AllStatusCounts = GetDetailedCustomers,
                completedtoday = completedfilestoday,
                outstandingtoday = outstandingpendingfiles,
                pendingtoday = pendingfilestoday,
                querytoday = querysent,
                querytilldate = querysenttilldate,
                receivedtilldate = receivedfilestilldate,
                recedivedtoday = receivedfilestoday
            };
            return Ok(result);
        }
        [HttpGet]
        [Route("GetProcess")]
        public IActionResult GetProcessHeader()
        {
            var ddldata = _FileStatusDashBoardService.GetProcess();
            var resultdata = new
            {
                AllddlList = ddldata
            };
            return Ok(resultdata);
        }
    }
}

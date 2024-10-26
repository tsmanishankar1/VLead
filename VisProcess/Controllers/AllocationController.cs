using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.IO.Compression;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Vis.VleadProcessV3.Models;

using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;


namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/Allocation")]
    [ApiController]
    public class AllocationController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AllocationService _allocationService;
        private readonly ClientCoOrdinationService _ClientCoOrdinationService;
        public AllocationController(AllocationService allocationService,
            ClientCoOrdinationService clientCoOrdinationService,
            IWebHostEnvironment webHostEnvironment)
        {
            _allocationService = allocationService;
            _ClientCoOrdinationService = clientCoOrdinationService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("getPendingJobs/{EmployeeId}/{ProcessId}")]//hited
        public Object GetPendingJobs(int EmployeeId, int ProcessId)
        {
      
           

            return _allocationService.GetPendingJobs(EmployeeId, ProcessId);
        }
        [HttpGet]
      
        [Route("getcountforcc/{EmployeeId}/{ProcessId}")]
        public Object GetCountForCC(int EmployeeId, int ProcessId)
        {
            return _allocationService.GetCountForCC(EmployeeId, ProcessId);
        }
        [HttpGet]
        [Route("getQueryPendingJobs/{EmployeeId}/{ProcessId}/{DepartmentId}")]//hited
        public Object GetQueryPendingJobs(int EmployeeId, int ProcessId, int DepartmentId)
        {
            return _allocationService.GetQueryPendingJobs(EmployeeId, ProcessId, DepartmentId);
        }
        [HttpGet]
        [Route("getQueryResponseJobs/{EmployeeId}/{ProcessId}")]//hited
        public Object GetQueryResponseJobs(int EmployeeId, int ProcessId)
        {
            return _allocationService.GetQueryResponseJobs(EmployeeId, ProcessId);
        }
        [HttpGet]
        [Route("getPendingAllocationJobsAndEmployees/{EmployeeId}/{ProcessId}/{IsPendingJob}/{DepartmentId}")]//hited
        public Object GetPendingJobsAndEmployees(int EmployeeId, int ProcessId, int IsPendingJob, int DepartmentId)
        {
            return _allocationService.GetPendingJobsAndEmployees(EmployeeId, ProcessId, IsPendingJob, DepartmentId);
        }
        [HttpGet]
        [Route("getCount/{EmployeeId}/{ProcessId}/{DepartmentId}")]
        public Object GetCount(int EmployeeId, int ProcessId, int DepartmentId)
        {
            return _allocationService.GetCount(ProcessId, EmployeeId, DepartmentId);
        }
        [HttpPost]
        [Route("processMovement")]
        public Object ProcessMovement(ProcessMovement processMovement)
        {
            return _allocationService.ProcessMovement(processMovement);
        }

        //QA restriction
        [HttpPost]
        [Route("QARestriction")]
        public Object QARestriction(ProcessMovement processMovement)
        {
            return _allocationService.QARestriction(processMovement);
        }
        //QA restriction end


        [HttpGet]
        [Route("getCompletedJobs")]
        public Object GetCompletedJobs(int EmpId)
        {
            var ClientInfo = _allocationService.GetCompletedJobs(EmpId);
            var result = new
            {
                ClientDetails = ClientInfo
            };
            return result;
        }
       
        [HttpGet]
        [Route("getAssignedEmployeesToChangeEstTime/{JId}")]
        public Object GetAssignedEmployeesToChangeEstTime(int JId)
        {
            return _allocationService.GetAssignedEmployeesToChangeEstTime(JId);
        }
        [HttpGet]
        [Route("getQueryResponseJobsAndEmployees/{EmployeeId}/{ProcessId}/{DepartmentId}")]
        public Object GetQueryResponseJobsAndEmployees(int EmployeeId, int ProcessId, int DepartmentId)
        {
            return _allocationService.GetQueryResponseJobsAndEmployees(EmployeeId, ProcessId, DepartmentId);
        }
        [HttpGet]
        [Route("getJobCategory")]
        public IEnumerable<JobCategory> GetJobCategory()//hited
        {
            return _allocationService.GetJobCategory();
        }
        [HttpGet]
        [Route("getScopeValues/{EmployeeId}")]//hited
        public Object GetScopeValues(int EmployeeId)
        {
            return _allocationService.GetScopeValues(EmployeeId);
        }
        [HttpGet]
        [Route("getWorkflowJobList/{EmployeeId}/{ProcessId}/{IsPendingJob}/{DepartmentId}")]
        public Object GetWorkFlowList(int EmployeeId, int ProcessId, int IsPendingJob, int DepartmentId)
        {
            return _allocationService.GetWorkFlowList(EmployeeId, ProcessId, IsPendingJob, DepartmentId);
        }
        [HttpPost]
        [Route("checkSelfQC")]
        public Object CheckSelfQC(ViewJobAllocation WFTIdAndEmployeeId)
        {
            return _allocationService.CheckSelfQC(WFTIdAndEmployeeId);
        }
        [HttpPost]
        [Route("getJobMovementJobsWithclientIdfileName")]//hited
        public Object GetJobMovementJobsWithclientIdfileName([FromBody] JobMovementViewModel jobOrder)
        {
            return _ClientCoOrdinationService.GetJobMovementJobsWithclientIdfileName(jobOrder);
        }

        [HttpPost]
        [Route("getAmountForSpecialPrice")]
        public Object GetAmountForSpecialPrice(ProcessMovement processMovement)
        {
            return _allocationService.GetAmountForSpecialPrice(processMovement);
        }

        [HttpGet]
        [Route("getCustomerScopeValues/{DepartmentId}/{ClientId}")]//hited
        public Object GetCustomerScopeValues(int DepartmentId, int ClientId)
        {
            return _allocationService.GetCustomerScopeValues(DepartmentId, ClientId);
        }
        [HttpGet]
        [Route("getFileNames/{path}")]
        public Object GetFileNames(string path)
        {
            path = path.Replace('_', '\\');
            return _allocationService.GetFileNames(path);
        }
        [HttpGet]
        [Route("downloadFilesTest/{path}/{filename}")]
        public IActionResult DownloadFilesTest(string path, string filename)
        {
            try
            {
              /// throw new System.Web.Http.HttpResponseException(HttpStatusCode.NotFound);
                path = path.Replace('_', '\\');
                var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "JobFiles",
                    HttpUtility.UrlDecode(path),
                    HttpUtility.UrlDecode(filename));
                if (!System.IO.File.Exists(filePath))
                    throw new System.Web.Http.HttpResponseException(HttpStatusCode.NotFound);
                HttpResponseMessage Response = new HttpResponseMessage(HttpStatusCode.OK);
                byte[] fileData;
                using (var bufferedStream = new FileStream(filePath, FileMode.Open))
                {
                    var memoryStream = new MemoryStream();
                    bufferedStream.CopyTo(memoryStream);
                    fileData = memoryStream.ToArray();
                }

               // System.IO.File.ReadAllBytes(filePath);

                //var memory = new MemoryStream();
                //using (var stream = new FileStream(path, FileMode.Open))
                //{
                //    stream.CopyTo(memory);
                //}

                ////if (fileData == null)
                ////    throw new System.Web.Http.HttpResponseException(HttpStatusCode.NotFound);
                Response.Content = new ByteArrayContent(fileData);
                Response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                return File(fileData, "application/octet-stream", filename);

            }
            catch(Exception ex)
            { 
                return Problem(ex.Message);

            }
         
           
        }
        [HttpPost]
        [Route("changeEstimatedTime")]
        public Object ChangeEstimatedTime(ProcessMovement processMovement)
        {
            return _allocationService.ChangeEstimatedTime(processMovement);
        }
        [HttpGet]
        [Route("GetQuerySPDetailsForQA/{JId}")]//hited
        public Object GetQuerySPDetailsForQA(int JId)
        {
            return _allocationService.GetQuerySPDetailsForQA(JId);
        }
        [HttpGet]
        [Route("GetDepartmentProcess")]
        public Object GetDepartmentProcess()//hited
        {
            return _allocationService.GetDepartmentProcess();
        }
        [HttpGet("DownloadZipFile")]
        public IActionResult DownloadZipFile(string path)
        {
            path = path.Replace('_', '\\');

            var folderPath = Path.Combine(_webHostEnvironment.ContentRootPath, "JobFiles", path+'\\');//Server.MapPath("~/JobFiles/" + path + "/");
            var selectedfiles = Directory.GetFiles(folderPath).Select(Path.GetFileName).ToList();
            var timeStamp = DateTime.Now.Ticks;
            var tempPath= Path.Combine(_webHostEnvironment.ContentRootPath, "JobFiles\\Zip", timeStamp + ".zip");
            if (System.IO.File.Exists(tempPath))
            {
                System.IO.File.Delete(tempPath);
            }
            ZipArchive zip = ZipFile.Open(tempPath, ZipArchiveMode.Create);

            foreach (string file in selectedfiles)
            {
                zip.CreateEntryFromFile(folderPath + file, file);
            }
            zip.Dispose();
            IFileProvider provider = new PhysicalFileProvider(Path.Combine(_webHostEnvironment.ContentRootPath, "JobFiles\\Zip"));
            IFileInfo fileInfo = provider.GetFileInfo(timeStamp + ".zip");
            var readStream = fileInfo.CreateReadStream();
            return File(readStream,"application/zip", timeStamp + ".zip");
        }
    }
}

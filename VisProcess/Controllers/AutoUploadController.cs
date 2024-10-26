using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class AutoUploadController : ControllerBase
    {
       private readonly AllocationService _allocationService;
    
        public AutoUploadController(AllocationService allocationService)
        {
            _allocationService = allocationService;
        }
        [HttpGet]
        [Route("Upload")]
        public IActionResult Get()
        {
            var completedJobs = _allocationService.GetCompletedJobsforService(0);

            var clientJobs = completedJobs.Where(x => (x.IsAutoUploadCount == null || x.IsAutoUploadCount <= 2) && (x.IsManualUpload == false || x.IsManualUpload == null)).OrderBy(x => x.TranId).ToList();

            var pm = new ProcessMovement();
            var processMovement = new List<ProcessMovement>();
            foreach (var item in clientJobs)
            {
                processMovement.Add(new ProcessMovement
                {
                    TranMasterId = item.TranMasterId,
                    DepartmentId = item.DepartmentId,
                    ClientId = item.ClientId,
                    TranId = item.TranId,
                    TimeStamp = item.TimeStamp,
                    JId = item.Jid,
                    JobId = item.JobId,
                    CommentsToClient = item.CommentsToClient
                });
            }
            pm.SelectedRows = processMovement;
            pm.EmployeeId = 2218;//staging-2218//live-274//
            pm.StatusId = 12;
            pm.ProcessId = 1;
            pm.CopyFiles = true;
            pm.AutoUploadJobs = true;

            var allocRepActionCall = _allocationService.ProcessMovement(pm);
            return Ok();
        }
    }
}

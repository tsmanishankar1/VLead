using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.Services.WindowsService;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackgroundController : ControllerBase
    {
        private readonly ClientCoOrdinationService _ClientCoOrdinationService;
        private readonly WorkflowService _WorkflowService;
        private readonly AllocationService _allocationService;
        private readonly UnitWork _unitofwork;
        private readonly AsyncWindowsService _windowsService;
        private readonly IConfiguration _configuration;
        public BackgroundController(ClientCoOrdinationService clientCoOrdination,
            WorkflowService workflowService,AllocationService allocationService,
            UnitWork unitWork, AsyncWindowsService windowsService, IConfiguration configuration)
        {
            _ClientCoOrdinationService = clientCoOrdination;
            _WorkflowService = workflowService;
            _allocationService = allocationService;
            _unitofwork = unitWork;
            _windowsService = windowsService;
            _configuration = configuration;
        }
        [HttpGet]
        [Route("ExportExcel")]//hited
        public void ExportExcel()
        {
            _ClientCoOrdinationService.ExportExcel();
        }
        [HttpGet]
        [Route("BulkTrayMovement")]
        public Object BulkTrayMovementForBackgroundService()
        {
            return _WorkflowService.BulkClosureNextTraymovement();
        }
        [HttpGet]
        [Route("ScheduledMail")]
        public IActionResult ScheduledMail()
        {
            try
            {

                List<JobOrder> completedJobs = new List<JobOrder>();
                completedJobs = _allocationService.ScheduledMailSend(0).ToList();
               
                int[] items=new int[completedJobs.Count];
              
                foreach (var item in completedJobs)
                {
                    items.Append(item.Id);
                 
                    var check = _unitofwork.JobOrder.Where(x => x.Id == item.Id && x.IsDeleted == false).FirstOrDefault();
                    if (check != null)
                    {
                        check.IsAutoUploadCount = 6;
                        _unitofwork.JobOrder.Update(check);
                        _unitofwork.SaveChanges();
                    }
                }
                return Ok(new
                {
                    count=completedJobs.Count(),
                     items
                });
            }
            catch (Exception ex)
            {
                Vis.VleadProcessV3.Models.Log savelog = new Vis.VleadProcessV3.Models.Log();
                savelog.Module = "Scheduled Mail Send Service";
                savelog.Description = "Email is Not send because of " + ex.InnerException + ": Message =" + ex.Message + "Data :" + ex.Data + "Source : " + ex.Source;
                savelog.Type = "Error";
                savelog.CreatedUtc = DateTime.UtcNow;
                _unitofwork.Log.Add(savelog);
                _unitofwork.SaveChanges();
             
                return StatusCode(500,ex.ToString()); 

            }

        }

        [HttpGet]
        [Route("AutoUploadNonVis")]
        public async Task<IActionResult> AutoUploadNonVis()
        {
            int holdjid = 0;
            try
            {
                var empid = _configuration.GetSection("VisAutoUpload").GetValue<int>("employeeId");
      

                var completedJobs = await _windowsService.GetCompletedJobsforService(0);
                var clientJobs = completedJobs.Where(x => x.ClientOrderId != null && (x.IsAutoUploadCount == null || x.IsAutoUploadCount <= 2) && (x.IsManualUpload == false || x.IsManualUpload == null)).OrderBy(x => x.TranId).ToList();
                var pm = new ProcessMovement();
                var processMovement = new List<ProcessMovement>();
                foreach (var item in clientJobs)
                {
                    holdjid = item.Jid;
                    processMovement.Add(new ProcessMovement
                    {
                        TranMasterId = item.TranMasterId,
                        DepartmentId = item.DepartmentId,
                        ClientId = item.ClientId,
                        TranId = item.TranId,
                        TimeStamp = item.TimeStamp,
                        JId = item.Jid,
                        JobId = item.JobId,
                        CommentsToClient = item.CommentsToClient,
                        IsJobFilesNotTransfer = item.IsJobFilesNotTransfer//
                    });
                }
                pm.SelectedEmployees = new List<ProcessMovement>();
                pm.SelectedRows = processMovement;
                pm.EmployeeId = Convert.ToInt16(empid);//staging-2218//live-274//
                pm.StatusId = 12;
                pm.ProcessId = 1;
                pm.CopyFiles = true;
                pm.AutoUploadJobs = true;
                pm.SelectedScopeId = 0;
       
                var allocRepActionCall = _allocationService.ProcessMovement(pm);
                int totalcount = clientJobs.Count;
               
                return Ok(totalcount);
                

            }
            catch (Exception ex)
            {
              
                return StatusCode(500, holdjid);

            }
            
        }
        [HttpGet]
        [Route("AutoUploadVis")]
        public async Task<IActionResult> AutoUploadVis()
        {
            int holdjid = 0;
            try
            {
                var empid = _configuration.GetSection("VisAutoUpload").GetValue<int>("employeeId");

                var completedJobs =await _windowsService.GetCompletedJobsforService(0);

               
                var clientJobs = completedJobs.Where(x => x.ClientOrderId != null && (x.IsAutoUploadCount == null || x.IsAutoUploadCount <= 2) && (x.IsManualUpload == false || x.IsManualUpload == null)).OrderBy(x => x.TranId).ToList();
                var pm = new ProcessMovement();
                var processMovement = new List<ProcessMovement>();
                foreach (var item in clientJobs)
                {
                    holdjid = item.Jid;
                    processMovement.Add(new ProcessMovement
                    {
                        TranMasterId = item.TranMasterId,
                        DepartmentId = item.DepartmentId,
                        ClientId = item.ClientId,
                        TranId = item.TranId,
                        TimeStamp = item.TimeStamp,
                        JId = item.Jid,
                        JobId = item.JobId,
                        CommentsToClient = item.CommentsToClient,
                        IsJobFilesNotTransfer = item.IsJobFilesNotTransfer//
                    });
                }
                pm.SelectedEmployees = new List<ProcessMovement>();
                pm.SelectedRows = processMovement;
                pm.EmployeeId = Convert.ToInt16(empid);//staging-2218//live-274//
                pm.StatusId = 12;
                pm.ProcessId = 1;
                pm.CopyFiles = true;
                pm.AutoUploadJobs = true;
                pm.SelectedScopeId = 0;

                var allocRepActionCall = _allocationService.ProcessMovement(pm);
                int totalcount = clientJobs.Count();
              
                return Ok(totalcount);
            

            }
            catch (Exception ex)
            {
              
                return StatusCode(500, holdjid);

            }
          

        }
        [HttpGet]
        [Route("DelelteZip")]
        public IActionResult DelelteZip()//need to configure
        {
            try
            {
                DirectoryInfo info =  new DirectoryInfo(_configuration.GetSection("DeleteZip").GetValue<string>("zipFolderPath"));
                var delayTime = Convert.ToInt32(_configuration.GetSection("DeleteZip").GetValue<int>("deleteZipInterval"));
                FileInfo[] files = info.GetFiles().Where(x => x.CreationTime < DateTime.Now.Subtract(new TimeSpan(0, delayTime, 0))).OrderBy(p => p.CreationTime).ToArray();
                foreach (FileInfo file in files)
                {
                    if ((System.IO.File.Exists(file.FullName)))
                    {
                        System.IO.File.Delete(file.FullName);
                    }
                }
              
                return Ok();
                
            }
            catch (Exception ex)
            {
                
                return Problem(ex.ToString());

            }
        }
    }
}

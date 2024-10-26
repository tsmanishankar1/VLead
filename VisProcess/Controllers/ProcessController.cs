using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessController : ControllerBase
    {
        private readonly ProcessService _processService;
        public ProcessController(ProcessService processService)
        {
            _processService =processService;
        }

        [HttpGet]
        [Route("ListProcess")]
        public IActionResult GetAllProcessList()
        {
            var ProcessList = _processService.GetAllProcess();
            if(ProcessList == null)
            {
                return NotFound("Process is Not Available");

            }
            else
            {
                return Ok(ProcessList);
            }
         
        }
        [HttpPost]
        [Route("CreateProcessFlow")]
        public IActionResult CreateProcess( [FromBody] Process process)
        {
            var Createprocess = _processService.CreateProcess(process);
            if (!Createprocess)
            {
                return StatusCode(500, "Unable to get Process Fields");

            }
            else
            {
                return Ok(Createprocess);
            }
          
        }
        [HttpGet]
        [Route("ProcessDetails")]
        public IActionResult GetProcessDetails(int id)
        {
            var getProcessDetails = _processService.GetProcessDetails(id);
            if (getProcessDetails == null)
            {
                return StatusCode(500, "Unable to get Process Fields");

            }
            else
            {
                return Ok(getProcessDetails);
            }
           
        }
        [HttpPost]
        [Route("Update-Process")]
        public IActionResult UpdateProcess([FromBody] Process process)
        {
            var UpdateProcess = _processService.UpdateProcess(process);
            if (!UpdateProcess)
            {
                return StatusCode(500, "Unable to get Process Fields");

            }
            else
            {
                return Ok(UpdateProcess);
            }
          
        }
        [HttpGet]
        [Route("Delete-Process")]
        public IActionResult RemoveProcess( int id)
        {
            var DeleteProcess = _processService.RemoveProcess(id);
            if (!DeleteProcess)
            {
                return NotFound("Process is Not Available");

            }
            else
            {
                return Ok(DeleteProcess);
            }
            
        }
    }
}

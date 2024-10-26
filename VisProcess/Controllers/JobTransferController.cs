using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class JobTransferController : ControllerBase
    {
        private readonly JobTransferService _jobTransferService;
        public JobTransferController(JobTransferService jobTransferService)
        {
            _jobTransferService = jobTransferService;
        }
        [HttpPost]
        [Route("JobOrderDetails")]
        public Object JobOrderList([FromBody] JobOrder1 joborder)
        {
            var getjobs = _jobTransferService.JobOrderDetails(joborder);
            var jobdetails = new
            {
                Jobs = getjobs,
            };
            return jobdetails;
        }
        [HttpGet]
        [Route("Clients")]
        public IActionResult GetCustomerList()
        {
            var getCustomer = _jobTransferService.AllCustomerlist();
            if (getCustomer!=null)
            {
                return Ok(getCustomer);
            }
            else
            {
                return StatusCode(500, "Unable to get customer");
            }
            
        }
        [HttpPost]
        [Route("ConvertDepartment")]
        public FileMovement ConvertDepartment([FromBody] JobTransferViewModel joborder)
        {
            return _jobTransferService.ConvertDepartment(joborder);
        }
    }
}

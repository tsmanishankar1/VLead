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
    public class CustomerQueryController : ControllerBase
    {
        private readonly CustomerQueryService _customerQueryService;
        public CustomerQueryController(CustomerQueryService customerQueryService)
        {
            _customerQueryService = customerQueryService;
        }
        [HttpGet]
        [Route("GetNotApprovedQueryForSPJobsToCCCount")]
        public IActionResult GetNotApprovedQueryForSPJobsToCCCount()
        {
            var details = _customerQueryService.GetNotApprovedQueryForSPJobsToCCCount();
            var result = new
            {
                queryJobs = details
            };
            //return result;
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("Data Not Available");
            }
           
        }
        [HttpGet]
        [Route("GetNotApprovedQueryForSPJobsToCC")]
        public IActionResult GetNotApprovedQueryForSPJobsToCC()
        {
            var details = _customerQueryService.GetNotApprovedQueryForSPJobsToCC();
            var result = new
            {
                queryJobs = details
            };
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("Data Not Available");
            }
          
        }
    }
}

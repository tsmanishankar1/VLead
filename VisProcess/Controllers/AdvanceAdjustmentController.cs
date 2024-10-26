using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdvanceAdjustmentController : ControllerBase
    {
        private readonly AdvanceAdjustmentService _advanceAdjustmentService;
        public AdvanceAdjustmentController(AdvanceAdjustmentService advanceAdjustmentService)
        {
            _advanceAdjustmentService = advanceAdjustmentService;
        }

        [HttpGet]
        [Route("GetAllCustomerAdvance")]
        public IActionResult GetCustomerAdvance( int CustomerId)
        {
            var advanceDetails = _advanceAdjustmentService.GetCustomerAdvance(CustomerId);
            if( advanceDetails == null)
            {
                return NotFound("Advance Details is Not Available");
            }
            else
            {
                return Ok(advanceDetails);
            }

        }
        [HttpPost]
        [Route("CreateAdvanceAdjustment")]
        public IActionResult CreateAdvanceAdjustment( [FromBody] AdvanceAdjustmentModel1 advanceAdjustment)
        {
            var advanceStatus = _advanceAdjustmentService.CreateAdvanceAdjustment(advanceAdjustment);
            if (advanceStatus == null)
            {
                return NotFound("Advance Details is Not Available");
            }
            else
            {
                return Ok(advanceStatus);
            }
            
        }
    }
}

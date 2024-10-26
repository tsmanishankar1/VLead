using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class TimeZoneController : ControllerBase
    
    {
        private readonly TimeZoneService _timeZoneService;
        public TimeZoneController(TimeZoneService timeZoneService)
        {
            _timeZoneService = timeZoneService;
        }
        [HttpGet]
        [Route("ListTimeZone")]
        public IActionResult TimeZoneList()
        {
            var TimeZoneList = _timeZoneService.GetAllTimeZone();
            if (TimeZoneList == null)
            {
                return NotFound("TimeZone is Not Available");
            }
            else
            {
                return Ok(TimeZoneList);
            }
        }
        [HttpPost]
        [Route("CreateTimeZone")]
        public IActionResult CreateTimeZone( [FromBody] Vis.VleadProcessV3.Models.TimeZone timezone)
        {
            var createTimeZoneList = _timeZoneService.CreateTimeZone(timezone);
            if (!createTimeZoneList)
            {
                return StatusCode(500, "Unable to Create Scope");
            }
            else
            {
                return Ok(createTimeZoneList);
            }
            
        }
        [HttpGet]
        [Route("GetTimeZoneDetails")]
        public IActionResult GetTimeZoneDetails(int Id)
        {
            var getTimeZoneDetails = _timeZoneService.GetTimeZoneDetails(Id);
            if (getTimeZoneDetails == null)
            {
                return StatusCode(500, "Unable to get Scope Details");
            }
            else
            {
                return Ok(getTimeZoneDetails);
            }
           
        }
        [HttpPost]
        [Route("Update-TimeZone")]
        public IActionResult UpdateTimeZone([FromBody] Vis.VleadProcessV3.Models.TimeZone timezone)
        {
            var UpdateTimeZone = _timeZoneService.UpdateTimeZone(timezone);
            if (!UpdateTimeZone)
            {
                return StatusCode(500, "Unable to get Campaign Fields");
            }
            else
            {
                return Ok(UpdateTimeZone);
            }
        }
        [HttpGet]
        [Route("Delete-TimeZone")]
        public IActionResult RemoveTimeZone( int id)
        {
            var DeleteTimeZone = _timeZoneService.RemoveTimeZone(id);
            if (!DeleteTimeZone)
            {
                return NotFound("Client Order is Not Available");
            }
            else
            {
                return Ok(DeleteTimeZone);
            }
           
        }
    }
}

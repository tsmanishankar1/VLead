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
    public class ShiftingController : ControllerBase
    
    {
        private readonly ShiftingService _shiftingService;
        public ShiftingController(ShiftingService shiftingService)
        {
            _shiftingService = shiftingService;
        }
        [HttpGet]
        [Route("AllShifting")]
        public IActionResult GetAllShift()
        {
            var _ShiftingList = _shiftingService.GetAllShift();
            if( _ShiftingList == null )
            {
                return NotFound("Shifting is Not Available");  
            }
            else
            {
                return Ok(_ShiftingList);
            }
           
        }
        [HttpPost]
        [Route("CreateShift")]
        public IActionResult CreateShifting([FromBody] Shift shift)
        {
            var createShifting = _shiftingService.CreateShifting(shift);
            if (!createShifting)
            {
                return StatusCode(500, "Unable to Create Shifting");
            }
            else
            {
                return Ok(createShifting);
            }
          
        }
        [HttpGet]
        [Route("GetShiftingDetails")]
        public IActionResult GetShiftDetails(int id)
        {
            var getShiftDetails = _shiftingService.GetShiftDetails(id);
            if (getShiftDetails == null)
            {
                return StatusCode(500, "Unable to get Shifting Fields");
            }
            else
            {
                return Ok(getShiftDetails);
            }
            
        }
        [HttpPost]
        [Route("Update-Shift")]
        public IActionResult UpdateShifting([FromBody] Shift shift)
        {
            var ShiftingUpdate = _shiftingService.UpdateShifting(shift);
            if (!ShiftingUpdate)
            {
                return StatusCode(500, "Unable to get ShiftingUpdate Fields");
            }
            else
            {
                return Ok(ShiftingUpdate);
            }
           
        }
        [HttpGet]
        [Route("Delete-Shift")]
        public IActionResult RemoveScope(int id)
        {
            var DeleteScope = _shiftingService.RemoveShifting(id);
            if (!DeleteScope)
            {
                return NotFound("Client Order is Not Available");
            }
            else
            {
                return Ok(DeleteScope);
            }
           
        }
    }
}

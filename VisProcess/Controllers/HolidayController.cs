using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayController : ControllerBase
    {
        private readonly HolidayService _holidayService;
        public HolidayController(HolidayService holidayService)
        {
            _holidayService = holidayService;
        }
        [HttpGet]
        [Route("HolidayList")]
        public IEnumerable<Holiday> GetHoliday()
        {
            return _holidayService.GetHoliday();
        }
        [HttpPost]
        [Route("AddHoliday")]
        public Object AddHoliday(Holiday Holiday)
        {
            var addholiday = _holidayService.AddHoliday(Holiday);
            var result = new
            {
                Message = addholiday
            };
            return result;
        }
        [HttpGet]
        [Route("DeleteHoliday")]
        public Object DeleteHoliday( int Id)
        {
            var deleteholiday = _holidayService.DeleteHoliday(Id);
            var result = new
            {
                Message = deleteholiday
            };
            return result;
        }
        [HttpGet]
        [Route("GetEditHoliday")]
        public IActionResult GetEditHoliday( int Id)
        {
            var HolidayDetail = _holidayService.GetEditHoliday(Id);
            if (HolidayDetail != null)
            {
                return Ok(HolidayDetail);
            }
            else
            {
                return NotFound("Client Order is Not Available");
            }
           
        }
        [HttpPost]
        [Route("UpdateHoliday")]
        public Object UpdateHoliday( [FromBody] Holiday EditList)
        {
            var updateholiday = _holidayService.UpdateHoliday(EditList);
            var result = new
            {
                Message = updateholiday
            };
            return result;
        }
    }
}

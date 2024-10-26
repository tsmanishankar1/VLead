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
    public class EmployeeVsDivisionController : ControllerBase
    {
        private readonly EmployeeVsDivisionService _eVsDservice;
        public EmployeeVsDivisionController(EmployeeVsDivisionService employeeVsDivisionService)
        {
            _eVsDservice = employeeVsDivisionService;
        }

        [HttpGet]
        [Route("GetEmployee")]
        public IActionResult GetEmployee()
        {
            var EEvDDetails = _eVsDservice.GetEmployee();
            var Details = new
            {
                EEvDList = EEvDDetails,
            };
            return Ok(Details);
        }
        [HttpGet]
        [Route("GetDivision")]
        public IActionResult GetDivision()
        {
            var DEvDDetails = _eVsDservice.GetDivision();
            var Details = new
            {
                DEvDList = DEvDDetails,
            };
            return Ok(Details);
        }
        [HttpPost]
        [Route("SetEmployeeVsDivision")]
        public IActionResult SetEmployeeVsDivision(EvsDPara1 EDPara)
        {
            var SEvDDetails = _eVsDservice.SetEmployeeVsDivision(EDPara);
            var Details = new
            {
                SEvDList = SEvDDetails,
            };
            return Ok(Details);
        }
        [HttpGet]
        [Route("GetEmployeeVsDivision")]
        public IActionResult GetEmployeeVsDivision()
        {
            var GEvDDetails = _eVsDservice.GetEmployeeVsDivision();
            var Details = new
            {
                GEvDList = GEvDDetails,
            };
            return Ok(Details);
        }
        [HttpPost]
        [Route("DeleteEmployeeVsDivision")]
        public IActionResult DeleteEmployeeVsDivision(EvsDDDPara delId)
        {
            var DelEvDDetails = _eVsDservice.DeleteEmployeeVsDivision(delId);
            var Details = new
            {
                DelEvDList = DelEvDDetails,
            };
            return Ok(Details);
        }
    }
}


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
    public class EmployeeVsSkillsetController : ControllerBase
    {
        private readonly EmployeeVsSkillsetService _EmployeeVsSkillsetService;
        public EmployeeVsSkillsetController(EmployeeVsSkillsetService employeeVsSkillsetService)
        {
            _EmployeeVsSkillsetService = employeeVsSkillsetService;
        }
        [HttpGet]//Hitted get code,name and division
        [Route("GetDropDownList")]
        public Object ddlDetailsList()
        {
            return _EmployeeVsSkillsetService.GetDropDownList();
        }

        //=================================create skillset
        [HttpPost]
        [Route("CreateEmployeeSkillset")]
        public IActionResult CreateEmployeeSkillset(ESS para)
        {
            var EvSDetails = _EmployeeVsSkillsetService.CreateEmployeeSkillset(para);
            var Details = new
            {
                EvSList = EvSDetails,
            };
            return Ok(Details);
        }
        //==============================get EmployeeVsSkillset
        [HttpGet]
        [Route("ShowEmployeeVsSkillset")]
        public IActionResult ShowEmployeeVsSkillset()
        {
            var ShowEvSDetails = _EmployeeVsSkillsetService.ShowEmployeeVsSkillset();
            var Details = new
            {
                GEvSlist = ShowEvSDetails,
            };
            return Ok(Details);
        }
        //================================Delete Skill
        [HttpGet]
        [Route("Delete-Skill")]
        public IActionResult RemoveSkill( int Id)
        {
            object data = new { };
            var DeleteSkill = _EmployeeVsSkillsetService.RemoveSkill(Id);
            if(DeleteSkill)
            {
                return Ok(DeleteSkill);
            }
            else

            {
                data = new
                {
                    status = false,
                    message = "Id does not exist"
                };
                return NotFound(data);
            }
           

        }
        //=================================get Details
        [HttpGet]// not working navigational property not found inside GetAllVal
        [Route("GetEmployeeVsSkillsetbyId")]
        public IActionResult GetEmployeeVsSkillsetbyId(int Id)
        {
            object data;
            var getEmployeeVsSkillDetails = _EmployeeVsSkillsetService.GetEmployeeVsSkillsetbyId(Id);
            if (getEmployeeVsSkillDetails != null)
            {
                return Ok(getEmployeeVsSkillDetails);
            }
            else

            {
                data = new
                {
                    status = false,
                    message = "Id does not exist"
                };
                return StatusCode(500,data);
            }
           
        }
        //=================================update skill
        [HttpPost]
        [Route("UpdateEmployeeSkill")]
        public IActionResult UpdateEmployeeSkill(EmployeeVsSkillset empskillset)
        {
            var EditSkillset = _EmployeeVsSkillsetService.UpdateEmployeeSkill(empskillset);
            var result = new
            {
                Message = EditSkillset
            };
            return Ok(result);
        }
        //==============================get employee code using id
        [HttpGet]
        [Route("GetEmployeeCodeByEmployeeId")]
        public Object GetEmployeeCodeByEmployeeId(int employeeid)
        {
            return _EmployeeVsSkillsetService.GetEmployeeCodeByEmployeeId(employeeid);
        }
    }
}

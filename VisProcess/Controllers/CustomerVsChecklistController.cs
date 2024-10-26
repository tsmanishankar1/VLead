using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;
namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerVsChecklistController : ControllerBase
    {
        private readonly CustomerVsChecklistService _CustomerVsChecklistService;

        public CustomerVsChecklistController(CustomerVsChecklistService customerVsChecklistService)
        {
            _CustomerVsChecklistService = customerVsChecklistService;
        }
        [HttpGet]//Hitted
        [Route("GetDropDownList")]
        public Object ddlDetailList()
        {
            return _CustomerVsChecklistService.GetAllDropDownList();
        }


        [HttpPost]//Hitted
        [Route("CreateChecklist")]
        public IActionResult CreateChecklist(CvsCPara para)
        {
            var CvCDetails = _CustomerVsChecklistService.CreateChecklist(para);
            var Details = new
            {
                CvCList = CvCDetails,
            };
            return Ok(Details);
        }


        [HttpGet]//Hitted
        [Route("CustomerVsChecklist")]
        public IActionResult CustomerVsChecklist()
        {
            var GCvCDetails = _CustomerVsChecklistService.CustomerVsChecklist();
            var Details = new
            {
                GCvCList = GCvCDetails,
            };
            return Ok(Details);
        }

        [HttpGet]//Hitted
        [Route("GetChecklistDetails")]
        public IActionResult GetChecklistDetails(int Id)
        {
            var getChecklistDetails = _CustomerVsChecklistService.GetChecklistDetails(Id);
            if(getChecklistDetails != null)
            {
                return Ok(getChecklistDetails);
            }
            else
            {
                return StatusCode(500, "Unable to get Check Details");
            }
            

        }

        [HttpPost]//Hitted
        [Route("UpdateChecklist")]
        public IActionResult UpdateCheckList(Checklist checklist)
        {
            var EditChecklist = _CustomerVsChecklistService.UpdateCheckList(checklist);
            var result = new
            {
                Message = EditChecklist
            };
            return Ok(result);
        }


        [HttpGet]//Hitted
        [Route("Delete-Check")]
        public IActionResult RemoveChecklist( int id)
        {
            var DeleteCheck = _CustomerVsChecklistService.RemoveChecklist(id);
            if (DeleteCheck != null)
            {
                return Ok(DeleteCheck);
            }
            else
            {
                return NotFound("Client Order is Not Available");
            }
           
        }
    }
}

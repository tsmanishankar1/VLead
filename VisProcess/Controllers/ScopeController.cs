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
    public class ScopeController : ControllerBase
    {

        private readonly ScopeService _scopeService;
        public ScopeController(ScopeService scopeService)
        {
            _scopeService = scopeService;
        }
        [HttpGet]
        [Route("ListScope")]
        public IActionResult GetAllScopeList()
        {
            var scopeList = _scopeService.GetAllScopeList();
            if (scopeList==null)
            {
                return NotFound("Error Category is Not Available");
            }
            else
            {
                return Ok(scopeList);
            }
           
        }
        [HttpPost]
        [Route("CreateScope")]
        public IActionResult CreateScope( [FromBody] Scope1 scope)
        {
            var createScopeList = _scopeService.CreateScope(scope);
            if (!createScopeList)
            {
                return StatusCode(500, "Some thing went wrong! or Already Exists!");
            }
            else
            {
                return Ok(createScopeList);
            }
           
        }
        [HttpGet]
        [Route("GetScopeDetails")]
        public IActionResult GetScopeDetails(int Id)
        {
            var getScopeDetails = _scopeService.GetScopeDetails(Id);
            if (getScopeDetails == null)
            {
                return StatusCode(500, "Unable to get Scope Details");
            }
            else
            {
                return Ok(getScopeDetails);
            }
        }
   
        [HttpPost]
        [Route("Update-Scope")]
        public IActionResult UpdateScope(Scope scope)
        {
            var EditCampaign = _scopeService.UpdateScope(scope);
            var result = new
            {
                Message = EditCampaign
            };
            return Ok(result);
        }
        [HttpGet]
        [Route("Delete-Scope")]
        public IActionResult RemoveScope(int id)
        {
            var DeleteScope = _scopeService.RemoveScope(id);
            if (!DeleteScope)
            {
                return NotFound("Client Order is Not Available");
            }
            else
            {
                return Ok(DeleteScope);
            }
        
        }
        [HttpPost]
        [Route("AddBenchStatus")]
        public Object AddBenchStatus(BenchStatus AddBenchStatus)
        {
            var createBenchStatusList = _scopeService.AddBenchStatus(AddBenchStatus);
            var result = new
            {
                Message = createBenchStatusList
            };
            return result;

        }
        [HttpGet]
        [Route("ListofBenchStatus")]
        public IActionResult GetBenchStatus()
        {
            var GetBenchStatusList = _scopeService.GetBenchStatus();
            if (GetBenchStatusList == null)
            {
                return NotFound("Bench Status is Not Available");
            }
            else
            {
                return Ok(GetBenchStatusList);
            }
 
        }
        [HttpGet]
        [Route("GetEditBenchStatus")]
        public IActionResult EditBenchStatus( int id)
        {
            var EditBenchStatusList = _scopeService.EditBenchStatus(id);
            if (EditBenchStatusList == null)
            {
                return NotFound("Bench Status is Not Available");
            }
            else
            {
                return Ok(EditBenchStatusList);
            }
           
        }
        [HttpPost]
        [Route("UpdateBenchStatus")]
        public Object UpdateBenchStatus(BenchStatus EditList)
        {
            var UpdateBenchStatusList = _scopeService.UpdateBenchStatus(EditList);
            var result = new
            {
                Message = UpdateBenchStatusList
            };
            return result;
        }
        [HttpGet]
        [Route("DeleteBenchStatus")]
        public Object RemoveBenchStatus(int id)
        {
            var RemoveBenchStatusbyid = _scopeService.RemoveBenchStatus(id);
            var result = new
            {
                Message = RemoveBenchStatusbyid
            };
            return result;
        }
        [HttpGet]
        [Route("GetDropDownList")]
        public Object ddlDetailList()
        {
            return _scopeService.GetAllDropDownList();
        }
        [HttpGet]
        [Route("GetTrainingDropDownList")]
        public Object ddltrainingDetailList()
        {
            return _scopeService.ddltrainingDetailList();
        }
    }
}

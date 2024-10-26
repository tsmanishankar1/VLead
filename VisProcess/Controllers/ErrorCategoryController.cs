using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vis.VleadProcessV3.Models;

using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorCategoryController : ControllerBase
    {
        private readonly ErrorCategoryService _errorCategoryService;
        public ErrorCategoryController(ErrorCategoryService errorCategoryService)
        {
            _errorCategoryService = errorCategoryService;
        }
        [HttpGet]
        [Route("ListErrorCategory")]
        public IActionResult GetAllErrorCategory()
        {
            var errorCategory = _errorCategoryService.GetAllErrorCategory();
            if (errorCategory!=null)
            {
                return Ok(errorCategory);
            }
            else
            {
                return NotFound("Error Category is Not Available");
            }
           
        }
        [HttpPost]
        [Route("CreateErrorCategory")]
        public IActionResult CreateErrorCategory( [FromBody] ErrorCategory errorcategory)
        {
            var createErrorcategory = _errorCategoryService.CreateErrorCategory(errorcategory);
            if (createErrorcategory)
            {
                return Ok(createErrorcategory);
            }
            else
            {
                return StatusCode(500,"Unable to Create Error Category");
            }
     
        }
        [HttpGet]
        [Route("GetErrorCategoryDetails")]
        public IActionResult GetErrorCategoryDetails(int Id)
        {
            var getErrorCategoryDetails = _errorCategoryService.GetErrorCategoryDetails(Id);
            if (getErrorCategoryDetails != null)
            {
                return Ok(getErrorCategoryDetails);
            }
            else
            {
                return StatusCode(500, "Unable to get Campaign Fields");
            }
           
        }
        [HttpPost]
        [Route("Update-ErrorCategory")]
        public IActionResult UpdateErrorCategory([FromBody] ErrorCategory UpdateErrorCategory)
        {
            var EditCampaign = _errorCategoryService.UpdateErrorCategory(UpdateErrorCategory);
            if (EditCampaign)
            {
                return Ok(EditCampaign);
            }
            else
            {
                return StatusCode(500, "Unable to get Campaign Fields");
            }
          
        }
        [HttpGet]
        [Route("Delete-ErrorCategory")]
        public IActionResult RemoveErrorCategory( int Id)
        {
            var DeleteErrorCategory = _errorCategoryService.RemoveErrorCategory(Id);
            if (DeleteErrorCategory)
            {
                return Ok(DeleteErrorCategory);
            }
            else
            {
                return NotFound("Client Order is Not Available");
            }
           
        }
    }
}

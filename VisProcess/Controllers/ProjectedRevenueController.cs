using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectedRevenueController : ControllerBase
    {
        private readonly ProjectedRevenueService _service;
        public ProjectedRevenueController(ProjectedRevenueService service)
        {
            _service = service;
        }

        [HttpPost("CreateProjectedRevenue")]
        public IActionResult CreateProjectedRevenue(ProjectedRevenueRequest projectedRevenue)
        {
            try
            {
                var revenue = _service.CreateProjectedRevenue(projectedRevenue);
                var response = new
                {
                    Success = true,
                    Message = "Projected Revenue Assigned Successfully"
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    Success = false,
                    Message = ex.Message
                };
                return Ok(response);
            }
        }

        [HttpGet("GetProjectedRevenue")]
        public IActionResult GetProjectedRevenue()
        {
            try
            {
                var projectedRevenue = _service.GetProjectedRevenue();
                return Ok(projectedRevenue);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
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
    public class BJWishController : ControllerBase
    {
        private readonly BJWishService _bjwishservice;
        public BJWishController(BJWishService bJWishService)
        {
            _bjwishservice = bJWishService;
        }
        [HttpGet]
        [Route("GetBJWish")]
        public IActionResult GetBJWish()
        {
            var BJDetails = _bjwishservice.GetBJWishData();
            var Details = new
            {
                BJList = BJDetails,
            };
            return Ok(Details);
        }
    }
}

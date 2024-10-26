using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VisProcess.Local.Service;

namespace VisProcess.Local.Controller
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class DropdownController : ControllerBase
    {
        private readonly DropdownService service;
        public DropdownController(DropdownService dropdownService)
        {
            service = dropdownService;
        }
        [HttpGet]
        [Route("GetCustomers")]
        public IActionResult Customer()
        {
           
            var customers = service.GetCustomers();
            return Ok(customers);
        }
    }
}

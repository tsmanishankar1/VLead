using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisClosureController : ControllerBase
    {
        private readonly VisClosureService _service;
        public VisClosureController(VisClosureService service)
        {
            _service = service;
        }

        [HttpGet("SendVisClosureAlert")]
        public IActionResult SendVisClosureAlert()
        {
            try
            {
                var alert = _service.SendVisClosureAlert();
                var response = new
                {
                    Success = true,
                    Message = alert
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

        [HttpGet("SendVisClosureDailyAlert")]
        public IActionResult SendVisClosureDailyAlert()
        {
            try
            {
                var alert = _service.SendVisClosureDailyAlert();
                var response = new
                {
                    Success = true,
                    Message = alert
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
    }
}
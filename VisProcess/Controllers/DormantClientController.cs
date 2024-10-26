using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Results;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DormantClientController : ControllerBase
    {
        private readonly DormantClientService _service;
        private readonly UnitWork _unitWork;
        public DormantClientController(DormantClientService service, UnitWork unitWork)
        {
            _service = service;
            _unitWork = unitWork;
        }

        [HttpGet("GetDormantClient")]
        public IActionResult GetDormantClient(int managerId)
        {
            try
            {
                var getDormantClient = _service.GetDormantClient(managerId);
                var response = new
                {
                    Success = true,
                    Message = getDormantClient
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

        [HttpPost("MailSendToDormantClient")]
        public IActionResult MailSendToDormantClient(List<int> customerIds)
        {
            try
            {
                var responses = new List<object>();

                foreach (var customerId in customerIds)
                {
                    var customer = _unitWork.Customer.FirstOrDefault(c => c.Id == customerId && c.IsDeleted == false);

                    var dormantClientResult = _service.MailSendToDormantClient(customerId);
                    responses.Add(new
                    {
                        CustomerId = customer.ShortName,
                        Success = true,
                        Message = dormantClientResult
                    });
                }
                return Ok(responses);
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
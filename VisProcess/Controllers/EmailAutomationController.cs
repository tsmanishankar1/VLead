using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailAutomationController : ControllerBase
    {
        private readonly EmailAutomationService _emailAutomationService;
        public EmailAutomationController(EmailAutomationService emailAutomationservice) 
        {
            _emailAutomationService = emailAutomationservice;
        }
        [HttpGet]
        [Route("GetEmailDropdownOfCustomers")]
        public IActionResult GetContactEmailsList (int CustomerId)
        {
            try
            {
                if(CustomerId > 0)
                {
                    var result = _emailAutomationService.GetContactEmailsList(CustomerId);
                    if (result.Count > 0)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest("Please select any customer");
                }
               
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpPost]
        [Route("SetEmailContact")]
        public IActionResult SetEmailContact(EmailContactNotification request)
        {
            try
            {
                if(request.CustomerId > 0 && (request.Type == "ARFollowUp"|| request.Type == "Invoice"))
                {
                    var result = _emailAutomationService.SetEmailContact(request);
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Please enter the mandatory details");
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }

        }
        [HttpGet]
        [Route("GetContactEmailByCustomers")]
        public IActionResult GetContactEmailByCustomers(int customerId, string type)
        {
            try
            {
                if(customerId > 0 && (type == "ARFollowUp" || type == "Invoice"))
                {
                    var result = _emailAutomationService.GetContactEmailByCustomers(customerId, type);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                    else
                        return NotFound(result);
                }
                else
                {
                    return BadRequest("Please select customer and type");
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }

        }
        [HttpPost]
        [Route("SendInvoicesToClient")]
        public async Task<IActionResult> SendInvoicesToClients(CustomerInvoiceRequest request, string type)
        {
            try
            {
                if (request != null && (type == "ARFollowUp" || type == "Invoice"))
                {
                    var  result = await _emailAutomationService.SendInvoicesToClients(request, type);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return NotFound(result);
                    }
                }
                else
                {
                    return BadRequest("Please select Invoice to send");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpGet]
        [Route("GetARFollowUpCustomerList")]
        public IActionResult GetARFollowUpCustomerList()
        {
            try
            {
                var result = _emailAutomationService.GetARFollowUpCustomerList();
                if(result.Any())
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("No record found");
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

    }
}

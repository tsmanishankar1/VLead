using Microsoft.AspNetCore.Mvc;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricingTriggerController : ControllerBase
    {
        private readonly PricingTriggerService _pricingTriggerService;
        public PricingTriggerController(PricingTriggerService pricingTriggerService)
        {
            _pricingTriggerService = pricingTriggerService;
        }
        [HttpGet]
        [Route("GetPendingPricingForSales")]
        public IActionResult GetPendingPricingForSales(int empId)
        {
            try
            {
                var result = _pricingTriggerService.GetPendingPricingForSales(empId);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("No record found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpGet]
        [Route("GetCustomersWithNullPriceForScope")]
        public IActionResult GetCustomersWithNullPriceForScope(DateTime startDate, DateTime endDate, string customerIds)
        {
            try
            {
                var result = _pricingTriggerService.GetCustomersWithNullPriceForScope(startDate, endDate, customerIds);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("No record found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpGet]
        [Route("GetCustomerBySalesEmployeeId")]
        public IActionResult GetCustomerBySalesEmployeeId(int employeeId)
        {
            try
            {
                var result = _pricingTriggerService.GetCustomerBySalesEmployeeId(employeeId);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("No record found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

    }
}
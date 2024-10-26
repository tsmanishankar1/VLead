using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class OutStandingController : ControllerBase
    {
        private readonly OutStandingService _outStandingService;
        public OutStandingController(OutStandingService outStandingService)
        {
            _outStandingService = outStandingService;
        }
        [HttpGet]
        [Route("OutstandingReport")]
        public IActionResult OutstandingReport( string reportAsOn, int EmpId)
        {
            var Invoices = _outStandingService.GetAllInvoices(reportAsOn, EmpId);
            if (Invoices != null)
            {
                return Ok(Invoices);
            }
            else
            {
                return NotFound("Invoices Not Available");
            }
           
        }
        [HttpGet]
        [Route("GetCustomerName")]
        public IActionResult GetCustomerName()
        {
            var customerDetails = _outStandingService.GetCustomerName();
            if (customerDetails!=null)
            {
                return Ok(customerDetails);
            }
            else
            {
                return NotFound("Products Not Available");
            }
         
        }
      
        ///dashboard
        ///
        [HttpGet]
        [Route("GetOSSummary")]
        public IActionResult GetOSSummaries()
        {
            var customerDetails = _outStandingService.GetOSSummary();
            if (customerDetails != null)
            {
                return Ok(customerDetails);
            }
            else
            {
                return NotFound("Products Not Available");
            }
           
        }


        [HttpPost]
        [Route("OSSummary")]
        public IActionResult GetOSCustomerLst(SelectedCustomers SelectedCustomers)
        {
          
            var OSDetails = _outStandingService.GetOSSummaryDetails(SelectedCustomers);
            if (OSDetails != null)
            {
                return Ok(OSDetails);
            }
            else
            {
                return NotFound("Products Not Available");
            }
      
        }
        ////
        [HttpGet]
        [Route("GetVleadSalespersonList")]
        public IActionResult GetVleadSalespersonList()
        {
            var customerDetails = _outStandingService.GetVleadSalesPersonName();
            if (customerDetails != null)
            {
                return Ok(customerDetails);
            }
            else
            {
                return NotFound("Products Not Available");
            }
        }

        [HttpGet]
        [Route("OSCustomerEmpLst")]
        public IActionResult GetOSCustomerLst( string reportAsOn, string id, int EmpId)
        {
            var Ids = JsonConvert.DeserializeObject<IEnumerable<OSCustomerIds>>(id);
            var OSDetails = _outStandingService.OSDetails(reportAsOn, Ids, EmpId);
            if (OSDetails != null)
            {
                return Ok(OSDetails);
            }
            else
            {
                return NotFound("Products Not Available");
            }
        }
    }
}

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Net;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReceivableController : ControllerBase
    {
        private readonly ReceivableService _receivableService;
        public ReceivableController(ReceivableService receivableService)
        {
            _receivableService = receivableService;
        }

        [HttpGet]
        [Route("GetAllReceivables")]
        public IActionResult GetAllReceivables()
        {
            var receivablesDetails = _receivableService.GetAllReceivable();
            if (receivablesDetails != null)
            {
                return Ok(receivablesDetails);
            }
            else
            {
                return NotFound("Receivables is Not Available");
            }
          
        }


        [HttpGet]
        [Route("GetReceivableById")]
        public IActionResult GetReceivableDetails(int receivableId)
        {
            var receivableDetails = _receivableService.GetReceviableById(receivableId);
            if (receivableDetails != null)
            {
                return Ok(receivableDetails);
            }
            else
            {
                return NotFound("Receivables is Not Available");
            }
           
        }
        //
        [HttpGet]
        [Route("GetInvoice")]
        public IActionResult GetInvoiceDetails(string invoiceNo, int customerId)
        {
            var invoiceDetails = _receivableService.GetInvoiceDetails(invoiceNo, customerId);
            if (invoiceDetails != null)
            {
                return Ok(invoiceDetails);
            }
            else
            {
                return NotFound("Invoice Details is Not Available");
            }
         
        }

        [HttpGet]
        [Route("GetCustomerInvoice")]
        public IActionResult GetAllCustomerInvoice( int CustomerId)
        {
            var invoiceDetails = _receivableService.GetAllCustomerInvoice(CustomerId);
            if (invoiceDetails != null)
            {
                return Ok(invoiceDetails);
            }
            else
            {
                return NotFound("Invoice Details is Not Available");
            }
           
        }

        //Tally Integration ended
        [HttpPost]
        [Route("GetAllCreditReceivables")]
        public IActionResult GetReceivableIntegrationMaster(GetReceiptsPara Getparam)
        {
            var InvoiceDetails = _receivableService.GetReceivableIntegrationMaster(Getparam);
            var GetDetails = new
            {
                GetInvoice = InvoiceDetails,
            };
            return Ok(GetDetails);
        }

        [HttpGet]
        [Route("GetClient")]
        public IActionResult GetClient()
        {
            var CustomerDetails = _receivableService.GetClient();
            var Details = new
            {
                ClientList = CustomerDetails,
            };
            return Ok(Details);
        }

        [HttpPost]
        [Route("GetReceivableCopytoIntegration")]
        public IActionResult GetCopytoIntegration(Receivableinteglist Recinteglist)
        {
            var ReceivableDetails = _receivableService.GetReceivableCopyIntegration(Recinteglist);
            var Lists = new
            {
                StringList = ReceivableDetails,
            };
            return Ok(Lists);
        }
        //Tally Integration ended

        // getall creditnotes GetAllCreditNotes
        [HttpGet]
        [Route("GetAllCreditNotes")]
        public IActionResult GetAllCreditNotesDetails()
        {
            var CreditNotesDetails = _receivableService.GetAllCreditNote();
            if (CreditNotesDetails != null)
            {
                return Ok(CreditNotesDetails);
            }
            else
            {
                return NotFound("CreditNote is Not Available");
            }
        
        }

        // cr note
        [HttpPost]
        [Route("CreateCreditNote")]
        public IActionResult CreateCreditNote( [FromBody] ReceivableModel CreditNoteModel)
        {
            var CreditNote = _receivableService.CreateCreditNoteDetails(CreditNoteModel);
            var result = new
            {
                CreditNoteList = CreditNote
            };
            if (CreditNote != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("CreditNote is Not Available");
            }
           
        }

        [HttpPost]
        [Route("CreateReceivable")]
        public IActionResult CreateReceivable( [FromBody] ReceivableModel receivableModel)
        {
            var receivable = _receivableService.CreateReveicableDetails(receivableModel);
            var result = new
            {
                Receivables = receivable
            };
            if (receivable != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("Receivable is Not Available");
            }
           
        }
    }
}

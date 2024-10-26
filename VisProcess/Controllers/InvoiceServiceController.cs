using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceServiceController : ControllerBase
    {
       private readonly InvoiceService _invoiceservice;
        public InvoiceServiceController(InvoiceService invoiceService)
        {
            _invoiceservice = invoiceService;
        }
        [HttpGet]
        [Route("Calc")]
        public IActionResult Get()
        {
            try
            {
                var collection = _invoiceservice.ServiceDetailInvoice();

                var GetInvoice = new GetInvoiceVM();
                var getInvoice = new List<GetInvoiceVM>();
                foreach (var item in collection)
                {
                    getInvoice.Add(new GetInvoiceVM
                    {
                        JobId = item.JobId,
                        ShortName = item.ShortName,
                        ScopeId = item.ScopeId,
                        ScopeDesc = item.Description,
                        ClientId = Convert.ToInt32(item.ClientId),
                        BillingCycleType = item.BillingCycleType,
                        //DateofUpload = item.ESTDateofUpload,DateofUpload
                        DepartmentId = item.DepartmentId,
                        Id = item.Id,
                        JId = item.Jid,
                        FileReceivedDate = item.FileReceivedDate,
                        SpecialPrice = Convert.ToDecimal(item.SpecialPrice),
                        EstimatedTime = item.EstimatedTime,

                    });
                }
                GetInvoice.GetInvoice = getInvoice;
                var completedJobs = _invoiceservice.GetInvoiceCalc(GetInvoice);
            }
            catch (Exception Ex)
            {
                throw;
            }
            return Ok();
        }
    }
}

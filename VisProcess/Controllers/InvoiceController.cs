using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Policy;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController :ControllerBase
    {
        private readonly IConfiguration _configuration;
        public InvoiceController(InvoiceService invoiceService, IConfiguration configuration)
        {
            _invoiceservice = invoiceService;
            _configuration = configuration;
        }
        private readonly InvoiceService _invoiceservice;

        [HttpGet("Reports")]
        public async Task<IActionResult> GetReport(string invoiceNumber, string type)
        {
            try
            {
                string ReportPath = "";
                switch (type)
                {
                    case "artwork":
                        ReportPath = _configuration.GetSection("SSRSNetworkCredentials").GetValue<string>("ArtworkReportPath");
                        break;
                    case "digitizing":
                        ReportPath = _configuration.GetSection("SSRSNetworkCredentials").GetValue<string>("DigitizingReportPath");
                        break;
                    case "invoice":
                        ReportPath = _configuration.GetSection("SSRSNetworkCredentials").GetValue<string>("InvoiceReportPath");
                        break;
                    default:
                        return BadRequest("Please enter correct report type.");
                }
                var userName = _configuration.GetSection("SSRSNetworkCredentials").GetValue<string>("Username");
                var password = _configuration.GetSection("SSRSNetworkCredentials").GetValue<string>("Password");
                var ReportServerUrl = _configuration.GetSection("SSRSNetworkCredentials").GetValue<string>("ReportServerUrl");
                string fullReportUrl = $"{ReportServerUrl}/{ReportPath}&rs:Command=Render&InvoiceNo={invoiceNumber}&rs:Format=PDF";

                using (HttpClient client = new HttpClient(new HttpClientHandler { Credentials = new NetworkCredential(userName, password) }))
                {
                    HttpResponseMessage response = await client.GetAsync(fullReportUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        byte[] reportBytes = await response.Content.ReadAsByteArrayAsync();
                        return File(reportBytes, "application/pdf");
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return StatusCode((int)HttpStatusCode.Unauthorized, "Access denied ");
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return NotFound();
                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode, "An error occurred");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("GetClientDetails")]
        public IActionResult getinvoicedetails(GetJobsforInvoiceVM1 GetInvoice)
        {
            var InvoiceDetails = _invoiceservice.getinvoicedetails(GetInvoice);
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
            var CustomerDetails = _invoiceservice.GetClient();
            var Details = new
            {
                ClientList = CustomerDetails,
            };
            return Ok(Details);
        }
        [HttpPost]
        [Route("GetCalculatedPrice")]
        public IActionResult GetCalculatedPrice(GetJobsforInvoiceVM1 GetInvoice)
        {
            var InvoiceDetails = _invoiceservice.GetCalculatedPrice(GetInvoice);
            var GetDetails = new
            {
                GetInvoice = InvoiceDetails,
            };
            return Ok(GetDetails);
        }
        [HttpPost]
        [Route("GetStaffingPrice")]
        public IActionResult GetStaffingPriceAmount(GetJobsforInvoiceVM1 GetInvoice)
        {
            var InvoiceDetails = _invoiceservice.GetStaffingPriceAmount(GetInvoice);
            var GetDetails = new
            {
                GetInvoice = InvoiceDetails,
            };
            return Ok(GetDetails);
        }
        [HttpPost]
        [Route("GenerateInvoice")]
        public IActionResult GenerateInvoice(GetInvoiceVM GetInvoice)
        {
            var InvoiceDetails = _invoiceservice.GenerateInvoice(GetInvoice);
            var Lists = new
            {
                StringList = InvoiceDetails,
            };
            return Ok(Lists);
        }
        [HttpGet]
        [Route("GetAllInvoiceMasterDetails")]
        public IActionResult GetAllInvoiceMasterDetails()
        {
            var InvoiceDetails = _invoiceservice.GetAllInvoiceMasterDetails();
            var GetDetails = new
            {
                GetInvoice = InvoiceDetails,
            };
            return Ok(GetDetails);
        }
        [HttpPost]
        [Route("GetInvoiceselected")]
        public IActionResult GetselectedInvoiceDetailsHistory(GetInvoiceDetailVM1 invoicenumber)
        {
            var InvoiceDetails = _invoiceservice.GetselectedInvoiceDetailsforDiscount(invoicenumber);
            var GetDetails = new
            {
                invoicenumber = InvoiceDetails,
            };
            return Ok(GetDetails);
        }
        [HttpPost]
        [Route("Getselectedinvoicediscount")]
        public IActionResult GetDiscountInvoice(GetInvoiceDetailVM1 invoicenumber)
        {
            var InvoiceDetails = _invoiceservice.GetDiscountInvoice(invoicenumber);
            var GetDetails = new
            {
                invoicenumber = InvoiceDetails,
            };
            return Ok(GetDetails);
        }
        [HttpPost]
        [Route("GenerateConfirmInvoice")]
        public IActionResult GenerateConfirmInvoice(GetInvoiceVM GetInvoice)
        {
            var InvoiceDetails = _invoiceservice.GenerateConfirmInvoice(GetInvoice);
            var Lists = new
            {
                StringList = InvoiceDetails,
            };
            return Ok(Lists);
        }
        [HttpPost]
        [Route("GenerateReCalculateInvoice")]
        public IActionResult GenerateReCalculateInvoice(GetInvoiceVM GetInvoice)
        {
            var InvoiceDetails = _invoiceservice.GenerateReCalculateInvoice(GetInvoice);
            var Lists = new
            {
                StringList = InvoiceDetails,
            };
            return Ok(Lists);
        }
        [HttpPost]
        [Route("GetCalculatedInvoice")]
        public IActionResult GetInvoiceCalc(GetInvoiceVM GetInvoice)
        {
            var InvoiceDetails = _invoiceservice.GetInvoiceCalc(GetInvoice);
            var Lists = new
            {
                StringList = InvoiceDetails,
            };
            return Ok(Lists);
        }
        [HttpPost]
        [Route("GetDropInvoiceforCancel")]
        public IActionResult GetDropInvoiceforCancel(invoicesalecancel1 invoicesc)
        {
            var CustomerDetails = _invoiceservice.GetDropInvoiceforCancel(invoicesc);
            var Details = new
            {
                ClientList = CustomerDetails,
            };
            return Ok(Details);
        }
        [HttpGet]
        [Route("GetDropClientforInvoiceCancel")]
        public IActionResult GetDropClientforInvoiceCancel()
        {
            var CustomerDetails = _invoiceservice.GetDropClientforInvoiceCancel();
            var Details = new
            {
                ClientDrop = CustomerDetails,
            };
            return Ok(Details);
        }
        [HttpPost]
        [Route("GetInvoiceMasterforSalesCancel")]
        public IActionResult GetInvoiceMasterforSalesCancel(invoicesalecancel1 invoicesc)
        {
            var InvoiceDetails = _invoiceservice.GetInvoiceMasterforSalesCancel(invoicesc);
            var GetDetails = new
            {
                invoicesc = InvoiceDetails,
            };
            return Ok(GetDetails);
        }
        [HttpPost]
        [Route("GetInvoiceTranforSalesCancel")]
        public IActionResult GetInvoiceTranforSalesCancel(invoicesalecancel1 invoicesc)
        {
            var InvoiceDetails = _invoiceservice.GetInvoiceTranforSalesCancel(invoicesc);
            var GetDetails = new
            {
                invoicesc = InvoiceDetails,
            };
            return Ok(GetDetails);
        }
        [HttpGet]
        [Route("GetInvoiceMasterDetailforCancelled")]
        public IActionResult GetInvoiceMasterDetailforCancelled()
        {
            var InvoiceDetails = _invoiceservice.GetInvoiceMasterDetailforCancelled();
            var GetDetails = new
            {
                invoicesc = InvoiceDetails,
            };
            return Ok(GetDetails);
        }
        [HttpPost]
        [Route("GetInvoiceTranDetailforCancelled")]
        public IActionResult GetInvoiceTranDetailforCancelled(invoicesalecancel1 invoicesc)
        {
            var InvoiceDetails = _invoiceservice.GetInvoiceTranDetailforCancelled(invoicesc);
            var GetDetails = new
            {
                invoicesc = InvoiceDetails,
            };
            return Ok(GetDetails);
        }
        [HttpPost]
        [Route("GetUpdateMasterforSalesCancel")]
        public IActionResult GetUpdateMasterforSalesCancel(invoicesalecancel invoicesc)
        {
            var InvoiceDetails = _invoiceservice.GetUpdateMasterforSalesCancel(invoicesc);
            var Lists = new
            {
                StringList = InvoiceDetails,
            };
            return Ok(Lists);
        }
        [HttpPost]
        [Route("GetInvoiceIntegrationMaster")]
        public IActionResult GetInvoiceIntegrationMaster(integrationmasterpara Getparam)
        {
            var InvoiceDetails = _invoiceservice.GetInvoiceIntegrationMaster(Getparam);
            var GetDetails = new
            {
                GetInvoice = InvoiceDetails,
            };
            return Ok(GetDetails);
        }
        [HttpPost]
        [Route("GetExchangeRatetoInvoice")]
        public IActionResult GetExchangeRatetoInvoice(integrationxchangerate invintigxchange)
        {
            var InvoiceDetails = _invoiceservice.GetExchangeRatetoInvoice(invintigxchange);
            var Lists = new
            {
                StringList = InvoiceDetails,
            };
            return Ok(Lists);
        }
        [HttpPost]
        [Route("GetCopytoIntegration")]
        public IActionResult GetCopytoIntegration(invoiceintiglist invintiglist)
        {
            var InvoiceDetails = _invoiceservice.GetCopytoIntegration(invintiglist);
            var Lists = new
            {
                StringList = InvoiceDetails,
            };
            return Ok(Lists);
        }
        [HttpPost]
        [Route("GetWaiverJobWithclientIdfileName")]
        public IActionResult GetWaiverJobWithclientIdfileName(waiverpara jobOrder)
        {
            return Ok(_invoiceservice.GetWaiverJobWithclientIdfileName(jobOrder));
        }
        [HttpPost]
        [Route("AddWaiverJobList")]
        public IActionResult AddWaiverJobList(List<JobOrder2> jobOrder)
        {
            return Ok(_invoiceservice.AddWaiverJob(jobOrder));
        }
        [HttpGet]
        [Route("getCustomers")]
        public IActionResult GetCustomers()
        {
            var CustomerDetails = _invoiceservice.GetCustomers();
            var Lists = new
            {
                StringList = CustomerDetails,
            };
            return Ok(Lists);
         
        }
        [HttpPost("club-invoices")]
        public IActionResult ClubInvoices(ClubInvoicesDTO clubInvoicesDTO)
        {
            var InvoiceDetails = _invoiceservice.ClubInvoices(clubInvoicesDTO);
            var Lists = new
            {
                StringList = InvoiceDetails,
            };

            return Ok(Lists);
        }
        [HttpPost("SubmitInvoiceMonth")]
        public IActionResult SubmitInvoiceMonth(SubmitInvoiceMonth submitInvoiceMonth)
        {
            try
            {
                var updateInvoice = _invoiceservice.SubmitInvoiceMonth(submitInvoiceMonth);
                var response = new
                {
                    Success = true,
                    Message = updateInvoice
                };
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                var response = new
                {
                    Success = false,
                    Message = ex.Message
                };
                return NotFound(response);
            }
            catch (InvalidOperationException ex)
            {
                var response = new
                {
                    Success = false,
                    Message = ex.Message
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
        [HttpGet("GetCurrentMonthInvoice")]
        public IActionResult GetCurrentMonthInvoice()
        {
            try
            {
                var updateInvoice = _invoiceservice.GetCurrentMonthInvoice();
                return Ok(updateInvoice);
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

        [HttpGet("GetUnApprovedInvoice")]
        public IActionResult GetUnApprovedInvoice()
        {
            try
            {
                var updateInvoice = _invoiceservice.GetUnApprovedInvoice();
                return Ok(updateInvoice);
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
        [HttpPost("UpdateInvoiceMonth")]
        public IActionResult UpdateInvoiceMonth(UpdateInvoiceMonth updateInvoiceMonth)
        {
            try
            {
                var updateInvoice = _invoiceservice.UpdateInvoiceMonth(updateInvoiceMonth);
                var response = new
                {
                    Success = true,
                    Message = updateInvoice
                };
                return Ok(response);
            }
            //catch (MessageNotFoundException ex)
            //{
            //    var response = new
            //    {
            //        Success = false,
            //        Message = ex.Message
            //    };
            //    return NotFound(response);
            //}
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
        [HttpGet]
        [Route("GetMonthlyRevenue")]
        public IActionResult GetMonthlyRevenue(int? clientId, int? divisionId, int? departmentId, int? scopeId, int? startYear, int? startMonth, int? endYear, int? endMonth)
        {
            try
            {
                var monthlyRevenue = _invoiceservice.GetMonthlyRevenue(clientId, divisionId, departmentId, scopeId, startYear, startMonth, endYear, endMonth);
                var response = new
                {
                    Success = true,
                    Message = monthlyRevenue
                };
                return Ok(response);
            }
            //catch (MessageNotFoundException ex)
            //{
            //    var response = new
            //    {
            //        Success = false,
            //        Message = ex.Message
            //    };
            //    return NotFound(response);
            //}
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

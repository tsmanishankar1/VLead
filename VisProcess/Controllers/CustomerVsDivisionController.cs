using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerVsDivisionController : ControllerBase
    {
        private readonly CustomerVsDivisionService _cVsDservice;// = new CustomerVsDivisionService();
        public CustomerVsDivisionController(CustomerVsDivisionService customerVsDivisionService)
        {
            _cVsDservice = customerVsDivisionService;//new CustomerVsDivisionService(configuration);
        }

        [HttpPost]
        [Route("SetCustomerVsDivision")]
        public IActionResult SetCustomerVsDivision(CvsDPara para)
        {
            var CvDDetails = _cVsDservice.SetCustomerVsDivision(para);
            var Details = new
            {
                CvDList = CvDDetails,
            };
            return Ok(Details);
        }
        [HttpGet]
        [Route("GetCustomerVsDivision")]
        public IActionResult GetCustomerVsDivision()
        {
            var GCvDDetails = _cVsDservice.GetCustomerVsDivision();
            var Details = new
            {
                GCvDList = GCvDDetails,
            };
            return Ok(Details);
        }
        [HttpGet]
        [Route("nGetCustomer")]
        public IActionResult GetCustomerList()
        {
            var GCvDCusDetails = _cVsDservice.GetCustomerList();
            var Details = new
            {
                GCvDCusList = GCvDCusDetails,
            };
            return Ok(Details);
        }
        [HttpGet]
        [Route("RemoveCvsD")]
        public IActionResult RemoveCvsD(int Id)
        {
            var result = _cVsDservice.RemoveCvsD(Id);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("CustomerVsDivision Id Not Removed");
            }
           
        }
    }
}

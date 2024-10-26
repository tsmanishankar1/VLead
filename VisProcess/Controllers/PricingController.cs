using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class PricingController : ControllerBase
    {

        private readonly PricingService _pricingservice;
        public PricingController(PricingService pricingService)
        {
            _pricingservice = pricingService;
        }

        [HttpGet]
        [Route("pricingList")]
        public IActionResult GetPricingDetails()
        {
            var Departments = _pricingservice.GetDepartments();
            var customers = _pricingservice.GetCustomers();
            var pricingViewModel = new GetPricingVM();
            pricingViewModel.Departments = Departments;
            pricingViewModel.Customers = customers;
            if (pricingViewModel != null)
            {
                return Ok(pricingViewModel);
            }
            else
            {
                return NotFound("pricingViewModel Not Available");
            }
         
        }

     

        [HttpPost]
        [Route("CreateCustomerNormDetails")]
        public IActionResult CreateCustomerStatus(CustomerNormDataViewModel customernorminfo)
        {
            var datas = _pricingservice.CreateCustomerNormsDetails(customernorminfo);
            var Lists = new
            {
                StringList = datas,
            };
            return Ok(Lists);
        }


        //Norms end
        [HttpGet]
        [Route("CustomerListinPrice")]
        public IActionResult GetCustomerPricing()
        {
            var customers = _pricingservice.GetCustomers();
            var pricingViewModel = new GetPricingVM();
            pricingViewModel.Customers = customers;
            if (pricingViewModel != null)
            {
                return Ok(pricingViewModel);
            }
            else
            {
                return NotFound("pricingViewModel Not Available");
            }
         
        }
        [HttpGet] // new
        [Route("ScopeByDeptId")]
        public IActionResult GetScopeDepartmentId(int departmentId)
        {
            var Scope = _pricingservice.GetScopeViaDepartmentId(departmentId);
            if (Scope != null)
            {
                return Ok(Scope);
            }
            else
            {
                return NotFound("Scope Not Available");
            }
       
        }

        [HttpGet]
        [Route("PricingTypesByDeptId")]
        public IActionResult GetPricingTypesbyDeptId(int departmentId)
        {
            var pricingTypes = _pricingservice.GetPricingTypesByDepartmentId(departmentId);
            if (pricingTypes != null)
            {
                return Ok(pricingTypes);
            }
            else
            {
                return NotFound("pricingTypes Not Available");
            }
            
        }

        [HttpGet]
        [Route("ScopeByDeptCustId")]
        public IActionResult GetScopebyDeptId(int departmentId, int clientid)
        {
            var Scope = _pricingservice.GetScopeByDepartmentId(departmentId, clientid);
            if (Scope != null)
            {
                return Ok(Scope);
            }
            else
            {
                return NotFound("Scope Not Available");
            }
          
        }

        [HttpGet]
        [Route("ShowDetailsofPricing")]
        public IActionResult ShowDetailsofPricing(int clientid)
        {
            var Scope = _pricingservice.ShowDetailsofPricing(clientid);
            if (Scope != null)
            {
                return Ok(Scope);
            }
            else
            {
                return NotFound("Scope Not Available");
            }
          
        }

        [HttpGet]
        [Route("GetJobStatusList")]
        public IActionResult GetJobStatusList()
        {
            var Scope = _pricingservice.GetJobStatusList();
            var Lists = new
            {
                JSList = Scope,
            };
            return Ok(Lists);
        }

        //Norms
        [HttpGet]
        [Route("GetProcessListforNorms")]
        public IActionResult GetProcessListforNorms()
        {
            var Scope = _pricingservice.GetProcessListforNorms();
            var Lists = new
            {
                PRList = Scope,
            };
            return Ok(Lists);
        }

        [HttpGet]
        [Route("GetCusDvisionforNorms")]
        public IActionResult GetCusDvisionforNorms()
        {
            var Scope = _pricingservice.GetCusDvisionforNorms();
            var Lists = new
            {
                DIVList = Scope,
            };
            return Ok(Lists);
        }

        //Ends

        [HttpPost]
        [Route("GetPriceDetailforApproval")]
        public IActionResult GetPriceDetailforApproval(PriceApproveVM GetCollection)
        {
            var Scope = _pricingservice.GetPriceDetailforApproval(GetCollection);
            var Lists = new
            {
                StringList = Scope,
            };
            return Ok(Lists);
        }
        [HttpPost]
        [Route("CheckApproveReject")]
        public IActionResult CheckApproveReject(PriceApproveVM GetCollection)
        {
            var Scope = _pricingservice.CheckApproveReject(GetCollection);
            var Lists = new
            {
                StringList = Scope,
            };
            return Ok(Lists);
        }
        [HttpPost]
        [Route("SetApproval")]
        public IActionResult SetApproval(PriceApproveVM GetCollection)
        {
            var Scope = _pricingservice.SetApproval(GetCollection);
            var Lists = new
            {
                StringList = Scope,
            };
            return Ok(Lists);
        }
        [HttpPost]
        [Route("SetReject")]
        public IActionResult SetReject(PriceApproveVM GetCollection)
        {
            var Scope = _pricingservice.SetReject(GetCollection);
            var Lists = new
            {
                StringList = Scope,
            };
            return Ok(Lists);
        }

        [HttpPost]
        [Route("AddPricingWithScope")]
        public IActionResult AddPricingWithScope(PricingWithScopeViewModel pricingViewModel)
        {
            var datas = _pricingservice.AddPricingWithScope(pricingViewModel);
            var Lists = new
            {
                StringList = datas,
            };
            return Ok(Lists);
        }

        [HttpPost]
        [Route("ShowDetailsofPricingChart")]
        public Object ShowDetailsofPricingChart(PricingWithScopeViewModel pricingViewModel)
        {
            var datas = _pricingservice.ShowDetailsofPricingChart(pricingViewModel);
            var Lists = new
            {
                StringList = datas,
            };
            return Lists;
        }

        [HttpGet]
        [Route("editpricingList")]
        public IActionResult editpricingList(int id)
        {
            var Scope = _pricingservice.editpricingList(id);
            if (Scope != null)
            {
                return Ok(Scope);
            }
            else
            {
                return NotFound("Scope Not Available");
            }
         
        }

        [HttpGet]
        [Route("GetCustomers")]
        public IActionResult GetAllCustomers()
        {
            var customers = _pricingservice.GetCustomers();
            if (customers != null)
            {
                return Ok(customers);
            }
            else
            {
                return NotFound("customers Not Available");
            }

        }

        [HttpGet]
        [Route("RemovePricing")]
        public IActionResult RemovePricing(int pricingId)
        {
            var result = _pricingservice.RemovePricing(pricingId);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("Pricing Not Remove");
            }
          
        }

    
    }
}

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
    [Route("api/CustomerMapping")]
    [ApiController]
    public class CustomerMappingController : ControllerBase
    {
        private readonly CustomerMappingService _customerMappingService;
        public CustomerMappingController(CustomerMappingService customerMappingService)
        {
            _customerMappingService = customerMappingService;
        }
      
        [HttpGet]
        [Route("GetAllddlList")]
        public Object ddlDetailList()
        {
           
            var CustomervsempDetails = _customerMappingService.GetAllDropDownList();
            return CustomervsempDetails;
            
        }

        [HttpGet]
        [Route("ScopeByDeptIdCusId")]
        public IActionResult GetScopebyDeptId(int departmentId, int custId)
        {
            var Scope = _customerMappingService.GetScopeByDeptCustId(departmentId, custId);
            if (Scope != null)
            {
                return Ok(Scope);

            }
            else
            {
                return NotFound("Scope is not available");
            }
           
        }
        // //scopechange ui
        [HttpGet]
        [Route("DDLforScopeChange")]
        public IActionResult GetDDLforScopeChange(int departmentId, int custId)
        {
            var Scope = _customerMappingService.GetCusScopeByCusDepIds(departmentId, custId);
            if (Scope != null)
            {
                return Ok(Scope);

            }
            else
            {
                return NotFound("Scope is not available");
            }
          
        }
       
        [HttpPost]
        [Route("ChangeScopeAPI")]
        public IActionResult ChangeScopeAPI(ScopeChangeVM ChangeScope)
        {
            var ScopeChangeDetail = _customerMappingService.ChangeScopePanel(ChangeScope);
            var Lists = new
            {
                StringList = ScopeChangeDetail,
            };
            return Ok(Lists);
        }

        [HttpPost]
        [Route("ChangeStitchCount")]
        public IActionResult ChangeStitchCount(StitchCountChangeVM ChangeStitch)
        {
            var StitchCountDetails = _customerMappingService.ChangeStitchCount(ChangeStitch);
            var Lists = new
            {
                StringList = StitchCountDetails,
            };
            return Ok(Lists);

        }

        [HttpPost]
        [Route("SetSpecialPricing")]
        public IActionResult SetSpecialPricing(SetSpecialPricingVM SpecialPrice)
        {
            var SpecialPriceDetails = _customerMappingService.SetSpecialPricing(SpecialPrice);
            var Lists = new
            {
                StringList = SpecialPriceDetails,
            };
            return Ok(Lists);

        }

        // get jobststus ddl
        [HttpGet]
        [Route("JobStatusByCusId")]
        public IActionResult GetJobStatusByCusId(int custId)
        {
            var Scope = _customerMappingService.GetJobStatusByCusId(custId);
            if (Scope != null)
            {
                return Ok(Scope);

            }
            else
            {
                return NotFound("Scope is not available");
            }

        }
        //
        [HttpGet]
        [Route("CustomerScopeByCusId")]
        public IActionResult GetCustomerScopebyCusId(int cusId)
        {
            var CustomerScopeDetails = _customerMappingService.GetAllCustomerScopeByCusId(cusId);
            if (CustomerScopeDetails != null)
            {
                return Ok(CustomerScopeDetails);

            }
            else
            {
                return NotFound("Customer Scope Details is Not Available");
            }
           
        }

        [HttpGet]
        [Route("RemoveCustomerScope")]
        public IActionResult RemoveCustomerScope(int custScopeId)
        {
            var RemovecustScope = _customerMappingService.RemoveCustomerScope(custScopeId);
            if (RemovecustScope)
            {
                return Ok(RemovecustScope);

            }
            else
            {
                return NotFound("Customer Not Remove");
            }
          
        }
        //
        [HttpGet]
        [Route("RemoveCustomerTAT")]
        public IActionResult RemoveCustomerTAT(int custTATId)
        {
            var RemovecustTAT = _customerMappingService.RemoveCustomerTAT(custTATId);
            if (RemovecustTAT)
            {
                return Ok(RemovecustTAT);

            }
            else
            {
                return NotFound("TAT Not Remove");
            }
          
        }
        //
        [HttpPost]
        [Route("EditCustomerVsScope")]
        public IActionResult UpdateCustomerVsScope(CustomerVsScope2 customervsScope)
        {
            var CustomerVsScopeList = _customerMappingService.UpdateCustomerVsScopeDetails(customervsScope);
            if (CustomerVsScopeList != null)
            {
                return Ok(CustomerVsScopeList);

            }
            else
            {
                return NotFound("CustomerVsScope is not Updated");
            }
           
        }
        //
        [HttpGet]
        [Route("GetSalesEmployees")]
        public Object GetAllSalesEmployees()
        {
            return _customerMappingService.GetSalesEmployees();
        }

        [HttpPost]
        [Route("CreateCustomerVsSalesEmployee")]
        public Object CreateCustomerStatus(customerVsSalesMapping CustomerVsSalesEmp)
        {
            return _customerMappingService.CreateCustomerVsSalesEmployee(CustomerVsSalesEmp);
        }
        //
        [HttpPost]
        [Route("AddCustomerVsScope")]
        public Object CreateCustomerScope(List<CustomerVsScopeViewModel> customervsScope)
        {
            return _customerMappingService.AddCustomerVsScopeDetails(customervsScope);
        }
        //
        [HttpPost]
        [Route("AddCustomerTAT")]
        public Object CreateCustomerTATs(List<CustomerTATViewModel> customervsTat)
        {
            return _customerMappingService.AddCustomerTATsDetails(customervsTat);
        }

        // get grid data
        [HttpGet]
        [Route("GetAllCustomerTATbyCusId")]
        public IActionResult GetAllCustomerTATbyCusId(int custId)
        {
          
            var CustomerTatDetails = _customerMappingService.GetAllCustomerTATByCustId(custId);
            if (CustomerTatDetails != null)
            {
                return Ok(CustomerTatDetails);

            }
            else
            {
                return NotFound("Customer TAT Details is Not Available");
            }
          
        }
       
        // Update CusTAT
        [HttpPost]
        [Route("UpdateCustomerTATData")]
        public Object UpdateCustomerTATs(CustomerTat UpdtcustomervsTat)
        {
            return _customerMappingService.UpdateCustomerTATsDetails(UpdtcustomervsTat);
        }

        [HttpGet]
        [Route("GetAllCustomerEmployee")]
        public Object GetAllCustomerEmployee( int Id)
        {
            var CustomerEmployeeDetails = _customerMappingService.GetAllCustomerEmployee(Id);
            return CustomerEmployeeDetails;
              
        }
        
        [HttpPost]
        [Route("GetcustomerByClassId")]
        public Object GetcustomerByClassId( Customer1 CustomerClassificationId)
        {
            var CustomervsempDetails = _customerMappingService.GetcustomerByClassId(CustomerClassificationId);
            var GetCustomer = new
            {
                GetCustomerByIdList = CustomervsempDetails,
            };
            return GetCustomer;
        }

    }
}

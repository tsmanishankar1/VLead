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
    public class CustomerVsEmployeeController : ControllerBase
    {
        private readonly CustomerVsEmployeeService _customerVsEmployeeService;
        public CustomerVsEmployeeController(CustomerVsEmployeeService customerVsEmployeeService)
        {
            _customerVsEmployeeService = customerVsEmployeeService;
        }

        [HttpGet]
        [Route("GetAllddlList")]
        public Object ddlDetailList()
        {
            return _customerVsEmployeeService.GetAllDropDownList();
        }
        [HttpGet]
        [Route("CustomerVsEmployee")]
        public IEnumerable<ViewCustomerVsEmployee> GetAllCustomerVsEmployee()
        {
            return _customerVsEmployeeService.GetAllCustomerVsEmployee();
        }

        //Norms
        [HttpGet]
        [Route("GetCustomerNorms")]
        public IEnumerable<ViewCustomerNorm> GetAllCustomerNorms()
        {
            return _customerVsEmployeeService.GetAllCustomerNorms();
        }
        //Norms
        [HttpPost]
        [Route("CreateCustomerVsEmployee")]
        public IActionResult CreateCustomerStatus( [FromBody] CustomerVsEmployeeViewModel customervsemp)
        {
            var createCustomervsemp = _customerVsEmployeeService.CreateCustomerVsEmployee(customervsemp);
            if (createCustomervsemp)
            {
                return Ok(createCustomervsemp);
            }
            else
            {
                return Problem("Already Exists");
            }
           
        }
        [HttpGet]
        [Route("GetcustomerByClassId")]
        public Object GetcustomerByClassId(int CustomerClassificationId)
        {
            var CustomervsempDetails = _customerVsEmployeeService.GetcustomerByClassId(CustomerClassificationId);
            var GetCustomer = new
            {
                GetCustomerByIdList = CustomervsempDetails,
            };
            return GetCustomer;
        }
        [HttpPost]
        [Route("DeleteCustomerVsEmpById")]
        public Object DeleteCustomerVsEmpById(CustomerVsEmployee1 customerVsemp)
        {
            var GetLatestList = _customerVsEmployeeService.DeleteCustomerVsEmpById(customerVsemp);
            var GetList = new
            {
                GetViewList = GetLatestList,
            };
            return GetList;
        }


        //Norms
        [HttpPost]
        [Route("DeleteCustomerNormById")]
        public Object DeleteCustomerNormById(CustomersNorm1 customersnorm)
        {
            var GetLatestList = _customerVsEmployeeService.DeleteCustomerNormById(customersnorm);
            var GetList = new
            {
                GetViewList = GetLatestList,
            };
            return GetList;
        }


        [HttpGet]
        [Route("GetCustomerNormsById")]
        public Object GetCustomerNormsById(int Id)
        {
            var CustomerNormUpdateDetails = _customerVsEmployeeService.GetCustomerNormsById(Id);
            var GetNormList = new
            {
                GetNormsList = CustomerNormUpdateDetails,
            };
            return GetNormList;

        }

        [HttpPost]
        [Route("EditCustomerNorms")]
        public Object UpdateCustomerNorms(CustomersNorm updatecustomerNormsbyid)
        {
            var GetLatestList = _customerVsEmployeeService.UpdateCustomerNorms(updatecustomerNormsbyid);
            var GetList = new
            {
                GetViewList = GetLatestList,
            };
            return GetList;
        }

        //Norms
        [HttpGet]
        [Route("GetCustomerVsEmpById")]
        public IActionResult GetCustomerVsEmployee( int Id)
        {
            var CustomerstatusDetails = _customerVsEmployeeService.GetCustomerVsEmployee(Id);
            if (CustomerstatusDetails != null)
            {
                return Ok(CustomerstatusDetails);
            }
            else
            {
                return NotFound("Client Order is Not Available");
            }
            
        }
        [HttpPost]
        [Route("GetCustomerVsEmpByEmpId")]
        public IActionResult GetCustomerVsEmpByEmpId(ViewCustomerVsEmployee1 EmployeeId)
        {
            var CustomerstatusDetails = _customerVsEmployeeService.GetCustomerVsEmpByEmpId(EmployeeId);
            if (CustomerstatusDetails != null)
            {
                return Ok(CustomerstatusDetails);
            }
            else
            {
                return NotFound("Client Order is Not Available");
            }
          
        }
        [HttpPost]
        [Route("EditCustomerVsEmployee")]
        public IActionResult UpdateCustomerVsEmployee([FromBody] CustomerVsEmployeeViewModel customerEmp)
        {
            var updateCustomerstatus = _customerVsEmployeeService.UpdateCustomerVsEmployee(customerEmp);
            if (updateCustomerstatus)
            {
                return Ok(updateCustomerstatus);
            }
            else
            {
                return NotFound("Unable to Update Customer Vs Employee");
            }
        }
        [HttpGet]
        [Route("IsAgmApprovedNull")]
        public IActionResult GetAGMApprovedNull()
        {
            var agmApprovedList = _customerVsEmployeeService.GetAGMApprovedNull();
            return Ok(agmApprovedList);
        }
        [HttpPost]
        [Route("ApprovedAGM")]
        public IActionResult ApprovedAGM(AgmApproval agmApproval)
        {
            try
            {
                var approvedAgm = _customerVsEmployeeService.ApprovedAGM(agmApproval);
                var response = new
                {
                    Success = true,
                    Message = approvedAgm
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
    }
}

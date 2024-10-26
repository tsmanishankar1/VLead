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
    [Route("api/Customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    { 
        private readonly CustomerService _customerService;
        private readonly EmployeeService _employeeService;
        public CustomerController(CustomerService customerService,EmployeeService employeeService)
        {

            _customerService = customerService;
            _employeeService = employeeService;
        }
        [HttpPost]
        [Route("GetRemainderReportInfo")]
        public Object GetRemainderReport(GetRemainderReportData RemainderReport)
        {
            var res = _customerService.GetRemainderReport(RemainderReport);
            return res;
        }
        [HttpPost]
        [Route("GetInvoiceReminderReportInfo")]
        public IActionResult GetInvoiceReminderReport(GetRemainderReportData RemainderReport)
        {
            var res = _customerService.GetInvoiceReminderReport(RemainderReport);
            var GetDetails = new
            {
                GetIReminderA = res,
            };
            return Ok(GetDetails);
        }
        [HttpGet]
        [Route("getFullLocationByContraLocationId")]
        public IActionResult GetFullLocation(int contraLocationId)
        {
            var result = _employeeService.GetFullLocations(contraLocationId);
            if (result != null)
            {
                return Ok(result); 
            }
            else
            {
                return BadRequest("location is not Add");
            }
           
        }
        [HttpGet]
        [Route("CustomerJobLocUpdate")]
        public Object UpdateJobLocationInfo(int Id)
        {
            return _customerService.UpdateJobLocationInfo(Id);
        }
        [HttpGet]
        [Route("GetAllCustomerDepartmentInfo")]
        public Object GetCustomerDepartmentInfo()
        {
            return _customerService.GetCustomerDepartmentInfo();
        }
        [HttpPost]
        [Route("UpdateJobLocationInfo")]
        public IActionResult UpdateJobFilesLocation(JobFilesLocationMaster JobFilesLocation)
        {
            var contact = _customerService.UpdateJobFilesLocation(JobFilesLocation);
            if (contact)
            {
                return Ok(contact);
            }
            else
            {
                return NotFound("location is not Add");
            }
           
        }
        [HttpPost]
        [Route("createJobFilesLocationMaster")]
        public IActionResult createJobFilesLocationMaster(JobFilesLocationMaster jobFilesLocationMaster)
        {
            return Ok(_customerService.AddJobLocationMaster(jobFilesLocationMaster));
            //return data != null ? Ok(data) : NotFound("Contact is not Add");
          
        }
        [HttpPost]
        [Route("createNewCustomerPath")]
        public bool AddCustomerDepartmentPath(CustomerDepartment customer)
        {
            var result = _customerService.AddCustomerDepartmentPath(customer);
            return result;
        }
        [HttpPost]
        [Route("UpdateCustomerPath")]
        public IActionResult UpdateCustomerDepartmentPath(CustomerDepartment customerContact)
        {
            var contact = _customerService.UpdateCustomerDepartmentPath(customerContact);
            if (contact)
            {
                return Ok(contact);
            }
            else
            {
                return BadRequest("contact is not Updated");
            }
           
        }
        [HttpGet]
        [Route("CustomerDeptUpdate")]
        public Object UpdateCustomerDepartmentInfo(int Id)
        {
            return _customerService.UpdateCustomerDepartmentInfo(Id);
        }
        //-- Customersales started
        [HttpGet]
        [Route("getCustomerUnapproval")]
        public IActionResult GetCustomerSalesUnapproval(int EmpId)
        {

            var CusUnapp = _customerService.GetCustomerSalesUnapproval(EmpId);
            if (CusUnapp != null)
            {
                return Ok(CusUnapp);
            }
            else
            {
                return NotFound("Contact Not Available");
            }
          
        }

        [HttpGet]
        [Route("getCustomerUnapprovalcount")]
        public IActionResult GetCustomerSalesUnapprovalcount(int EmpId)
        {
            var CusUnappCount = _customerService.GetCustomerUnapprovalcount(EmpId);
            if (CusUnappCount != null)
            {
                return Ok(CusUnappCount);
            }
            else
            {
                return NotFound("contact is not Updated");
            }
           
        }

        [HttpGet]
        [Route("GetAllCustomers")]  // GetCustomers123    
        public object CustomerDetails(int EmpId)
        {
            return _customerService.GetAllCustomers(EmpId);
        }

        // --Customersales ended


        //-- Edit or View Customer sales Approval started
        [HttpGet]
        [Route("getAppCustomerSalesId")]
        public IActionResult GetAppCustomerSalesId(int CustomerId)
        {
            var CustomerReg = _customerService.GetAppCustomerSalesId(CustomerId);
            if (CustomerReg != null)
            {
                return Ok(CustomerReg);
            }
            else
            {
                return NotFound("CustomerReg Not Available");
            }
          
        }

        [HttpGet]
        [Route("GetEmployeeName")]
        public IActionResult GetEmployeeName (int customerId)
        {
            var Employee = _customerService.GetEmployeeIdByName(customerId);
            if (Employee != null)
            {
                return Ok(Employee);
            }
            else
            {
                return NotFound("Employee Not Available");
            }
           
        }


        [HttpGet]
        [Route("GetClassification")]
        public IActionResult GetAllClassifications()
        {
            var Classification = _customerService.GetClassificationList();
            if (Classification != null)
            {
                return Ok(Classification);
            }
            else
            {
                return NotFound("Classification Not Available");
            }
          
        }

        [HttpGet]
        [Route("getClientDepartment")]
        public IActionResult GetDepartments()
        {
            var Departments = _customerService.GetDepartments();
            if (Departments != null)
            {
                return Ok(Departments);
            }
            else
            {
                return NotFound("Departments Not Available");
            }
           
        }


        [HttpGet]
        [Route("getAppAllCustomerContactDetails")]
        public IActionResult GetAppCustomerContactDetails(int CustomerId)
        {
            var customer = _customerService.GetAppCustomerContactDetails(CustomerId);
            if (customer != null)
            {
                return Ok(customer);
            }
            else
            {
                return NotFound("CustomerReg Not Available");
            }
           
        }

        // country
        [HttpGet]
        [Route("GetAllCountryList")]
        public IActionResult GetAllCountryDetails()
        {
            var app = _customerService.GetCountriesDropDown();
            if (app != null)
            {
                return Ok(app);
            }
            else
            {
                return NotFound("Contact is not updated");
            }
        
        }

        // state
        [HttpGet]
        [Route("GetAllStateListbyCountryId")]
        public IActionResult GetAllStateListbyCountryId(int CountryId)
        {
            var app = _customerService.GetStatesDropDown(CountryId);
           
            if (app != null)
            {
                return Ok(app);
            }
            else
            {
                return NotFound("Contact is not updated");
            }
        }
        // city
        [HttpGet]
        [Route("GetAllCityListbyStateId")]
        public IActionResult GetAllCityListbyStateId(int StateId)
        {
            var app = _customerService.GetCitiesDropDown(StateId);
          
            if (app != null)
            {
                return Ok(app);
            }
            else
            {
                return NotFound("Contact is not updated");
            }
        }

        // TimeZone
        [HttpGet]
        [Route("GetAllTimeZoneListbyCityId")]
        public IActionResult GetAllTimeZoneListbyCityId(int CityId)
        {
            var app = _customerService.GetTimeZonebylocation(CityId);
            if (app != null)
            {
                return Ok(app);
            }
            else
            {
                return NotFound("Contact is not updated");
            }
           
        }

        [HttpPost]
        [Route("createCustomerDetails")]
        public IActionResult CreateCustomerApproved(AddCustomer customer)
        {
            var success = _customerService.AddCustomerDetails(customer);
            if (success != null)
            {
                return Ok(success);
            }
            else
            {
                return Forbid("Customer is not updated");
            }
           
        }

        [HttpPost]
        [Route("EditCustomerDetails")]
        public IActionResult EditCustomerDetails(AddCustomer customer)
        {
            var success = _customerService.EditCustomerDetails(customer);
            if (success != null)
            {
                return Ok(success);
            }
            else
            {
                return Forbid("Customer is not updated");
            }
          
        }

        [HttpGet]
        [Route("GetCustomerContacts")]
        public IActionResult GetAllCustomerContacts(int customerId)
        {
            var contacts = _customerService.GetAllCustomerContacts(customerId);
           
            if (contacts != null)
            {
                return Ok(contacts);
            }
            else
            {
                return NotFound("Contacts not available");
            }
           
        }

        //        //-- Edit or View Customer sales Approval Ended

        [HttpGet]
        [Route("getpendingapprovaljobs")]
        public Object Getpendingapprovaljobs(int EmpId)//hited
        {
            
            return _customerService.GetAllUnapprovaljobs(EmpId);

        }
        [HttpGet]
        [Route("RemoveCustomer")]
        public IActionResult RemoveCustomer(int customerId)
        {
            var customer = _customerService.RemoveCustomer(customerId);
            if (customer)
            {
                return Ok(customer);
            }
            else
            {
                return NotFound("Customer not remove");
            }
          
        }

        [HttpGet]
        [Route("GetCustomers")]
        public Object GetAllCustomers()
        {
            return _customerService.GetAllCustomers();
        }
        //Customer Credentials
        [HttpGet]
        [Route("GetCusContactsCredentials")]
        public IActionResult GetAllCustContCredentials(int customerId)
        {
            var contacts = _customerService.GetAllCusContactCredentials(customerId);
            if (contacts != null)
            {
                return Ok(contacts);
            }
            else
            {
                return NotFound("Contacts not available");
            }
            
        }

        [HttpGet]
        [Route("GetCustomerBySalesId")]
        public IActionResult GetAllCustomerddl(int customerId)
        {
            var contacts = _customerService.GetCustomerBySalesId(customerId);
            if (contacts != null)
            {
                return Ok(contacts);
            }
            else
            {
                return NotFound("Contacts not available");
            }
            
        }
    }
}

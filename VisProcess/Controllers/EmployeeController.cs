using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/Employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;
        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [Route("CheckEmplayeecode")]
        public Object CheckEmplayeecode(string name)
        {
            var result = _employeeService.CheckEmplayeecode(name);
            return new
            {
                result = result
            };
        }
        [HttpGet]
        [Route("GetEmployeeList")]
        public IEnumerable<Employee> GetEmployeeList()
        {
            return _employeeService.GetEmployeeList();
        }
        [HttpGet]
        [Route("GetDropDownList")]
        public IActionResult ddlDetailList()
        {
            try
            {
            
                     return Ok(_employeeService.GetAllDropDownList());
            }
            catch (Exception ex)
            {

                return Problem( ex.Message);
            }
           
        }
        [HttpGet]
        [Route("GetTrainingDropDownList")]
        public Object ddltrainingDetailList()
        {
            return _employeeService.ddltrainingDetailList();
        }
        [HttpGet]
        [Route("GetLocationList")]
        public Object ddlLocationList()
        {
            return _employeeService.GetLocationList();
        }
        [HttpGet]
        [Route("GetResignResaonsList")]
        public Object ddlResignResaons()
        {
            return _employeeService.GetResignResaonsList();
        }
        [HttpGet]
        [Route("GetProcessWorkFlowList")]
        public IEnumerable<Process> mslProcess()
        {
            return _employeeService.GetProcessWorkFlowList();
        }

        [HttpGet]
        [Route("GetRolesList")]
        public Object RolesEmpList()
        {
            return _employeeService.GetRolesList();
            
        }
        [HttpGet]
        [Route("GetEmployee")]
        public IEnumerable<GetViewEmployeeDetails> EmployeeDetails()
        {
            return _employeeService.GetAllEmployee();
        }
        [HttpGet]
        [Route("GetEmployeeWithDelete")]
        public IEnumerable<GetViewEmployeeDetails> GetEmployeeWithDelete(string IsDeleted, string IsResigned)
        {
            return _employeeService.GetEmployeeDetailsIncludingDelete(IsDeleted, IsResigned);
        }
        [HttpGet]
        [Route("GetEmployeeDetailsByID")]
        public Object EmployeeDetailsWithAddress(int employeeID)
        {
            var result = _employeeService.GetEmployeeDetailsWithAddress(employeeID);
            var employee = new
            {
                emp = result
            };
            return employee;
        }
        [HttpGet]
        [Route("GetEmployeeShift")]
        public IEnumerable<Shift> EmployeeShiftDetails()
        {
            return _employeeService.GetEmployeeShift();
        }
        [HttpPost]
        [Route("GetSearchEmployee")]
        public Object SearchEmployeeDetails([FromBody] GetViewEmployeeDetails ovEmp)
        {
            var emp = _employeeService.GetEmployeeByEmployeeCode(ovEmp);
            var test = new
            {
                data = emp
            };
            return test;
        }
        [HttpPost]
        [Route("AddEmployee")]
        public IActionResult AddEmployee(AddEmployee employee)
        {
            try
            {
                var emp = _employeeService.AddEmployee(employee);
                var response = new
                {
                    Success = true,
                    Message = emp
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
        //
        [HttpPost]
        [Route("AddEmpNewRoles")]
        public IActionResult AddEmployeeNewRoles(Role RoleDesc)

        {
          
            var EmpRole = _employeeService.AddEmployeeNewRoles(RoleDesc);
            if (EmpRole == null)
            {
                return NotFound("Roles is not Added");
            }
            else
            {
                return Ok(EmpRole);
            }
          
        }
        [HttpPost]
        [Route("EditEmployee")]
        public IActionResult EditEmployee(AddEmployee employee)
        {
            try
            {
                var emp = _employeeService.UpdateEmployee(employee);
                var response = new
                {
                    Success = true,
                    Message = emp
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
        [HttpDelete]
        [Route("PostDeleteEmployee")]
        public bool DeleteEmployee(int data, int data1)
        {
            var emp = _employeeService.DeleteEmployee(data, data1);
            return emp;
        }
        [HttpPost]
        [Route("PostUpdateEmployeeShift")]
        public bool AddShiftVsEmployee(ShiftVsEmployee shiftvsemp)
        {
            var emp = _employeeService.UpdateShiftVsEmployee(shiftvsemp);
            return emp;
        }
        [HttpPost]
        [Route("GetCoreDivisionFileCount")]
        public IActionResult GetCoreDivisionFileCount(EmployeeResponse employee)
        {
            try
            {
                var fileCount = _employeeService.GetCoreDivisionFileCount(employee);
                var response = new
                {
                    Success = true,
                    FileCount = fileCount
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

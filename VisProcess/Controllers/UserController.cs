using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Vis.VleadProcessV3.Models;

using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        [Route("GetAllUsers")]
        public IEnumerable<UserViewModel> GetAllUsers()
        {
            return _userService.UserDetails();
        }
        [HttpGet]
        [Route("GetEmployees")]
        public IEnumerable<EmployeeUserViewModel> GetEmployees()
        {
            var result = _userService.GetAllEmployeesDetails();
            return result;
        }
        [HttpGet]
        [Route("GetAllCustomers")]
        public IEnumerable<CustomerViewModel> GetAllCustomersDetails()
        {
            var result = _userService.GetAllCustomers();
            return result;
        }
        [HttpGet]
        [Route("GetAllEmployees")]
        public IEnumerable<Employee> GetAllEmployees()
        {
            var result = _userService.GetAllEmployees();
            return result;
        }
        [HttpPost]
        [Route("SaveUser")]
        public bool SaveUser(User user, int actionType)
        {
            return _userService.SaveUser(user, actionType);
        }
        [HttpGet]
        [Route("GetEmployeeCodeByEmployeeId")]
        public Object GetEmployeeCodeByEmployeeId(int employeeid)
        {
            return _userService.GetEmployeeCodeByEmployeeId(employeeid);
        }
        [HttpGet]
        [Route("GetCustomerContactById")]
        public IEnumerable<CustomerEmailUserViewModel> GetCustomerByCustomerId(int Id)
        {
            var result =  _userService.GetCustomerContactByCustomerId(Id);
            return result;
        }
        [HttpPost]
        [Route("DeleteUser")]
        public void DeleteUser(User deleteuser)
        {
            _userService.DeleteUser(deleteuser);
        }
        [HttpGet]
        [Route("getUserById")]
        public Object GetUserById(int Id)
        {
            return _userService.GetUserById(Id);
        }
        [HttpGet]
        [Route("getMenu")]
        public IEnumerable<Menu> GetMenu()
        {
            return _userService.GetMenu();
        }
    }
}

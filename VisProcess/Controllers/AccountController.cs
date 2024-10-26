using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;



using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }
        private readonly AccountService _accountService ;

        [HttpPost]
        [Route("externalLogin")]//hited
        public Object ExternalLogin([FromBody] LoginViewModel1 user)//hited
        {
            var loginUser = _accountService.ExternalLogin(user); // ServiceLocator.AccountRepository().ExternalLogin(user);
            if (loginUser.Id != 0)
            {
                //Test
               
                IEnumerable<Claim> tenantClaims = new Claim[]
     {
                new Claim(ClaimTypes.Name,loginUser.EmployeeName) ,
              
                new Claim(ClaimTypes.Role,loginUser.Roles),
                new Claim("Userid",loginUser.EmployeeId.ToString()),
               

     };
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                    IsPersistent = true,
                };
                ClaimsIdentity identity = new ClaimsIdentity(tenantClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
             HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,claimsPrincipal,authProperties);

                var result = new
                {
                    Success = true,
                    User = loginUser,
                    IsAuthenticated = User.Identity.IsAuthenticated
                };
                return result;

            }
            else
            {
                var result = new
                {
                    Success = false
                };
                return result;

            }
        }
        
        [HttpGet]
        [Route("checkIsAdmin/{employeeId}")]//hited
        public Object CheckIsAdmin(int employeeId)
        {
            var result = new
            {
                Success = false,
                Menu = ""
            };
            var user = _accountService.GetUserRoleByEmployeeId(employeeId); //ServiceLocator.AccountRepository().GetUserRoleByEmployeeId(employeeId);
           
                var roles = user.Roles.Split(',');
                if (roles.Any(role => role == "Admin"))
                {
                    result = new
                    {
                        Success = true,
                        Menu = user.MenuAccess
                    };
                    return result;
                }
            
           
            result = new
            {
                Success = false,
                Menu = user.MenuAccess
            };
            return result;
        }
        //    //--------------------------------------------------------------------vidhya KRA---------------------------------------------------
        [HttpGet]
        [Route("getDesignation/{EmployeeId}")]
        public IActionResult GetDesignation(int EmployeeId)
        {
            var Emp = _accountService.GetDesignation(EmployeeId);
            var result = new
            {
                Emp = Emp
            };
            return Ok(result);
        }
        [HttpGet]
        [Route("getKraDescription/{EmployeeId}")]
        public IActionResult GetKRADescription(int EmployeeId)
        {
            var KraDes = _accountService.GetKRADescription(EmployeeId);
            var result = new
            {
                KraDes = KraDes
            };
            return Ok(result);
        }
        [HttpPost]
        [Route("KRAApproval")]
        public IActionResult CreateKRAApproval(KRA para)
        {
            var KRAApp = _accountService.CreateKRAApproval(para);
            var result = new
            {
                KRAApp = KRAApp
            };
            return Ok(result);
        }
        //    //--------------------------------------------------------------------vidhya KRA---------------------------------------------------
        //    //--------------------------------------------------------------------wellness status---------------------------------------------------
        [HttpPost]
        [Route("WellnessStatus")]
        public IActionResult CreateWellnessStatus(WELL para)
        {
            var WELLApp = _accountService.CreateWellnessStatus(para);
            var result = new
            {
                WELLApp = WELLApp
            };
            return Ok(result);
        }
        //    //--------------------------------------------------------------------wellness status---------------------------------------------------
        [HttpGet]
        [Route("getEmployeeProcess/{EmployeeId}")]//hited
        public IActionResult GetEmployeeDetails(int EmployeeId)
        {
            var EmployeeProcess = _accountService.GetEmployeeProcess(EmployeeId);//ServiceLocator.AllocationRepository().GetEmployeeProcess(EmployeeId);
            var result = new
            {
                EmployeeProcess = EmployeeProcess
            };
            return Ok(result);
        }
        [HttpGet]
        [Route("logout")]//hited
        public bool ExternalLogin()
        {
            // FormsAuthentication.SignOut();
            HttpContext.SignOutAsync();
            return true;
        }
        [Route("ChangePassword")]
        [HttpPost]
        public Object ChangePassword([FromBody] NewPassword changePassword)
        {
            var result = _accountService.PasswordChange(changePassword);
            return new
            {
                result = result
            };
        }
    }
}


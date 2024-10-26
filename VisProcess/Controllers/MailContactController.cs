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
    public class MailContactController : ControllerBase
    {
        public MailContactController(MailContactService mailContactService)
        {
            _mailContactService = mailContactService;
        }
        private readonly MailContactService _mailContactService;
        [HttpGet]
        [Route("GetAllDropDown")]
        public Object GetAllDropDown()
        {
            return _mailContactService.GetAllDropDown();
        }
        [HttpPost]
        [Route("AddMailcontact")]
        public IActionResult AddMailcontact( AddMailContact AddMailcontact)
        {
            var addmailcontact = _mailContactService.AddMailContact(AddMailcontact);
            if (addmailcontact)
            {
                return Ok(addmailcontact);
            }
            else
            {
                return StatusCode(500, "Unable to Update Customer Vs Employee");
            }
          
        }
        [HttpGet]
        [Route("GetCustomerorEmployee")]
        public Object GetCustomerorEmployee(int employeeId, int clientId)
        {
            return _mailContactService.GetCustomerorEmployee(employeeId, clientId);
        }
        [HttpGet]
        [Route("GetAllMailContact")]
 
        public Object GetAllMailContact()
        {
            return _mailContactService.GetAllMailContact();
        }

        
        [HttpPost]
        [Route("UpdateMailcontact")]
        public IActionResult UpdateMailContactInfo(AddMailContact AddMailcontact)
        {
            var addmailcontact = _mailContactService.UpdateMailContact(AddMailcontact);
       
            if (addmailcontact != null)
            {
                var Lists = new
                {
                    StringList = addmailcontact,
                };
                return Ok(Lists);
            }
            return Problem("Unable to Update Mail Contacts");
           
        }

        [HttpGet]
        [Route("EditMailcontact")]
        public Object UpdateMailContactInfo(int Id)
        {
            return _mailContactService.UpdateMailContactInfo(Id);
        }
        [HttpGet]
        [Route("DeleteMailcontact")]
        public Object DeleteMailContactInfo(int employeeId, int clientId)
        {
            return _mailContactService.DeleteMailContactInfo(employeeId, clientId);
        }
    }
}

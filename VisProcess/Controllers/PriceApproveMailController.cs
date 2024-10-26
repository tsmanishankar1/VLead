using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Office.Interop.Excel;
using System.Net;
using System.Net.Mail;

using Vis.VleadProcessV3.Models;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class PriceApproveMailController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        public PriceApproveMailController(IConfiguration configuration,ApplicationDbContext dbContext)
        {
            _configuration=configuration;
            _db = dbContext;
        }
        [HttpPost]
        [Route("Mail")]
        public async Task<IActionResult> SendMail(int UId, int DepartmentId, int CustomerId, int PricingTypeId, int Approve)
        {
            try
            {
             
                var cus = _db.Customers.FirstOrDefault(x => x.Id == CustomerId);
                var url = _configuration.GetSection("PriceApproveMail").GetValue<string>("PriceApproval");//ConfigurationManager.AppSettings["PriceApproval"];
                var tomail = _configuration.GetSection("PriceApproveMail").GetValue<string>("ApproveToMail");// ConfigurationManager.AppSettings["ApproveToMail"];
                var toname = _configuration.GetSection("PriceApproveMail").GetValue<string>("ApproveToName"); //ConfigurationManager.AppSettings["ApproveToName"];
                var host = _configuration.GetSection("Smtp").GetValue<string>("host");
                var port = _configuration.GetSection("Smtp").GetValue<int>("port");
                var userName = _configuration.GetSection("Smtp").GetValue<string>("userName");
                var password = _configuration.GetSection("Smtp").GetValue<string>("password");
                var Ssl = _configuration.GetSection("Smtp").GetValue<bool>("SSL");
                var defaultCredential= _configuration.GetSection("Smtp").GetValue<bool>("defaultCredential");
                var body = "Hi " + toname + ",";
                body += "</br></br>";
                body += "Please use the below Link to Activate the new pricing for " + "<b>" + cus.ShortName + "</b>" + ".";
                body += "</br></br>";
               // body += "<a href='https://localhost:7208/api/News/GetAllNewsList' target='_blank'>Click here to Verify & Approve</a>";
                body += "<a href='" + url + "UId=" + UId + "&DepartmentId=" + DepartmentId + "&CustomerId=" + CustomerId + "&PricingTypeId=" + PricingTypeId + "&Approve=" + Approve + "' target='_blank'>Click here to Verify & Approve</a>";
                body += "</br></br>";
                body += "Thanks," + "</br>";
                body += "Sales/MIS Team" + "</br>";
                body += "VLEAD DESIGN SERVICES PVT.LTD";
                var message = new MailMessage();
                message.To.Add(new MailAddress(tomail));
                message.From = new MailAddress("visalert@vleadservices.com");
                
                //message.CC.Add(new MailAddress("support@vleadservices.com"));
                //message.CC.Add(new MailAddress("saranya@vleadservices.com"));
                message.Subject = "Pricing Approval";
                message.Body = string.Format(body, "JesusRaja", "Test");
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient(host, port))
                {

                    smtp.UseDefaultCredentials = defaultCredential;
                    smtp.Credentials = new NetworkCredential(userName, password);
                    smtp.EnableSsl = Ssl;
                    await smtp.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {

            }
            return Ok();
        }
    }
}


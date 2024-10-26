using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.Services;
using Vis.VleadProcessV3.ViewModels;

namespace VisProcess.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]

    public class GenericMailController : ControllerBase
    {
        public GenericMailController(IConfiguration configuration,JobOrderService jobOrderService,UnitWork unitWork)
        {
            _unitOfWork = unitWork;
            JOS = jobOrderService;
            _configuration=configuration;
        }
        private readonly UnitWork _unitOfWork;
        private readonly JobOrderService JOS;
        private readonly IConfiguration _configuration;

        [HttpGet]
        [Route("CreditDays")]
        public async Task<IActionResult> SendCreditDaysMail()
        {
            var host = _configuration.GetSection("Smtp").GetValue<string>("host");
            var port = _configuration.GetSection("Smtp").GetValue<int>("port");
            var userName = _configuration.GetSection("Smtp").GetValue<string>("userName");
            var password = _configuration.GetSection("Smtp").GetValue<string>("password");
            var Ssl = _configuration.GetSection("Smtp").GetValue<bool>("SSL");
            var defaultCredential = _configuration.GetSection("Smtp").GetValue<bool>("defaultCredential");
            var GetCustomerCollection = new List<GetCustomerCollectionVM>();
            string value = "";
            try
            {
                var resultCus = _unitOfWork.Customer.Where(x => x.IsDeleted == false && x.CreditDays != null).ToList();
                if (resultCus.Count() > 0)
                {
                    foreach (var item in resultCus)
                    {
                        var check = JOS.GetBlockedResult(item.Id);
                        if (check == true)
                        {
                            //collect customer name and send mail
                            var cus = _unitOfWork.Customer.FirstOrDefault(x => x.Id == item.Id);
                            GetCustomerCollection.Add(new GetCustomerCollectionVM
                            {
                                CustomerId = cus.Id,
                                CustomerName = cus.ShortName
                            });
                        }
                    }
                }
                var cnamelocal = GetCustomerCollection.ToList();
                value = string.Join(", ", from result in cnamelocal select result.CustomerName);
                if (cnamelocal.Count() != 0)
                {
                    var body = "Hi " + "Team" + ",<br/><br/>" +
    "We would like to bring to your kind attention that the following customer(s) Credit Days got exceeded.<br/><br/>" +
    "<b>" + value + "</b><br/><br/>" +
    "The intake of their jobs should not be done in VIS.<br/><br/>" +
    "Kindly contact the Customer(s) and resolve this issue.<br/><br/>" +
    "Regards,<br/><br/>" +
    "VLEAD DESIGN SERVICES PVT.LTD";

                    var message = new MailMessage();
                    var CreditDaysTo = _configuration.GetSection("GenericMail").GetValue<string>("CreditDaysTo");//ConfigurationManager.AppSettings["CreditDaysTo"];
                    var CreditDaysCC = _configuration.GetSection("GenericMail").GetValue<string>("CreditDaysCC");//ConfigurationManager.AppSettings["CreditDaysCC"];
                    message.To.Add(new MailAddress(CreditDaysTo));
                    message.From = new MailAddress("visalert@vleadservices.com");
                    message.CC.Add(new MailAddress(CreditDaysCC));
                    //message.CC.Add(new MailAddress("support@vleadservices.com"));
                    message.Subject = "Credit Days Exceeded email for Vlead Salesperson";
                    message.Body = string.Format(body, "JesusRaja", "Test");
                    message.IsBodyHtml = true;
                    using (var smtp = new SmtpClient(host,port))
                    {

                        smtp.UseDefaultCredentials = defaultCredential;
                        smtp.Credentials = new NetworkCredential(userName, password);
                        smtp.EnableSsl = Ssl;
                        await smtp.SendMailAsync(message);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Ok();
        }
    }
}

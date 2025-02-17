using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Office.Interop.Excel;
using Renci.SshNet.Messages;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Numerics;
using System.Security.Cryptography.Xml;
using System.Text;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;
using static System.Collections.Specialized.BitVector32;


namespace Vis.VleadProcessV3.Services
{
    public class EmailAutomationService
    {
        private readonly ApplicationDbContext db;
        private readonly TableWork tableWork;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        public EmailAutomationService(ApplicationDbContext db, TableWork tableWork, IConfiguration configuration, ApplicationDbContext dbcontext)
        {
            this.db = db;
            this.tableWork = tableWork;
            this._configuration = configuration;
            this._context = dbcontext;

        }

        public EmailResult GetContactEmailByCustomers(int customerId, string type)
        {
            var emailNotification = db.EmailContactNotifications
                .Where(x => x.CustomerId == customerId && x.IsActive == true && x.Type == type)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            var toEmailIds = new string[] { };
            var ccEmailIds = new string[] { };

            if (emailNotification != null)
            {
                toEmailIds = emailNotification.ToEmailId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                ccEmailIds = emailNotification.CcEmailId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
            }
            else
            {
                return new EmailResult
                {
                    Success = false,
                    ToEmail = null,
                    CcEmail = null
                };
            }
            var toEmailList = new List<string>();
            var ccEmailList = new List<string>();

            foreach (var item in toEmailIds)
            {
                var toresult = tableWork.CustomerContactRepository.Get(x => x.Id == int.Parse(item) && x.IsDeleted == false)
                    .Select(x => x.Email)
                    .FirstOrDefault();
                if (toresult != null)
                    toEmailList.Add(toresult);
            }

            foreach (var item in ccEmailIds)
            {
                var ccresult = tableWork.CustomerContactRepository.Get(x => x.Id == int.Parse(item) && x.IsDeleted == false)
                    .Select(x => x.Email)
                    .FirstOrDefault();
                if (ccresult != null)
                    ccEmailList.Add(ccresult);
            }

            return new EmailResult
            {
                Success = true,
                Type = type,
                ToEmail = toEmailList,
                CcEmail = ccEmailList
            };
        }



        public List<object> GetContactEmailsList(int customerId)
        {
            var emails = tableWork.CustomerContactRepository
                .Get(x => x.CustomerId == customerId && x.IsDeleted == false)
                .Select(result => (object)new
                {
                    id = result.Id,
                    email = result.Email,
                    contactname = result.ContactName
                })
                .ToList();
            return emails;
        }

        public object SetEmailContact(EmailContactNotification request)
        {
            var existingRecord = tableWork.EmailContactNotificationRepository.Get(x => x.CustomerId == request.CustomerId && x.IsActive == true && x.Type == request.Type)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();
            if (existingRecord == null)
            {
                request.IsApproved = null;
                request.CreatedUtc = DateTime.UtcNow;
                request.IsActive = true;
                db.EmailContactNotifications.Add(request);
                db.SaveChanges();
                return new
                {
                    success = true,
                    result = request
                };
            }
            else
            {
                existingRecord.UpdatedUtc = DateTime.UtcNow;
                existingRecord.UpdatedBy = request.UpdatedBy;
                existingRecord.CcEmailId = request.CcEmailId;
                existingRecord.ToEmailId = request.ToEmailId;
                db.Update(existingRecord);
                db.SaveChanges();
                return new
                {
                    success = true,
                    result = existingRecord
                };
            }
        }
        public async Task<object> SendInvoicesToClients(CustomerInvoiceRequest request, string type)
        {
            string pdfFilePath = "";
            var host = _configuration.GetSection("Smtp").GetValue<string>("host");
            var port = _configuration.GetSection("Smtp").GetValue<int>("port");
            var userName = _configuration.GetSection("Smtp").GetValue<string>("userName1");
            var password = _configuration.GetSection("Smtp").GetValue<string>("password1");
            var Ssl = _configuration.GetSection("Smtp").GetValue<bool>("SSL");
            var defaultCredential = _configuration.GetSection("Smtp").GetValue<bool>("defaultCredential");

            string InvoiceReportPath = _configuration.GetSection("SSRSNetworkCredentials").GetValue<string>("InvoiceReportPath");
            string ArtworkReportPath = _configuration.GetSection("SSRSNetworkCredentials").GetValue<string>("ArtworkReportPath");
            string DigitizingReportPath = _configuration.GetSection("SSRSNetworkCredentials").GetValue<string>("DigitizingReportPath");
            var userName1 = _configuration.GetSection("SSRSNetworkCredentials").GetValue<string>("Username");
            var password1 = _configuration.GetSection("SSRSNetworkCredentials").GetValue<string>("Password");
            var ReportServerUrl = _configuration.GetSection("SSRSNetworkCredentials").GetValue<string>("ReportServerUrl");

            var TempPath = _configuration.GetSection("SSRSNetworkCredentials").GetValue<string>("TempFilePath");
            var LogoPath = _configuration.GetSection("SSRSNetworkCredentials").GetValue<string>("LogoPath");
            var PortfolioLink = _configuration.GetSection("SSRSNetworkCredentials").GetValue<string>("PortfolioLink");
            var FromMail = _configuration.GetSection("Invoice").GetValue<string>("InvoiceFromMail");
            var fromName = _configuration.GetSection("Invoice").GetValue<string>("InvoiceSenderName");
            var defaultCCMail = _configuration.GetSection("Invoice").GetValue<string>("InvoiceCCDefaultMail");
            var errors = new List<object>();
            var message = new MailMessage();
            StringBuilder rows = new StringBuilder();
            bool success = false;
            decimal InvoiceValueTotal = 0;
            decimal? AmountCollectedTotal = 0;
            decimal? outstandindAmountTotal = 0;
            DateTime now = DateTime.Now;

            // Subtract one month
            DateTime previousMonth = now.AddMonths(-1);
            int lastMonth = previousMonth.Month;

            //// Get the month name and year
            string monthName = " ";
            string invoiceMonthName = now.AddMonths(-1).ToString("MMMM", CultureInfo.InvariantCulture);
            int invoiceYear = now.Year;
            int lastYear = previousMonth.Year;
            //string invoiceYear = year.ToString();

            if (type == "Invoice")
            {
                int customerId = int.Parse(request.CustomerId);
                var invoiceNos = request.InvoiceNo.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                //
                StringBuilder receipientNamesBuilder = new StringBuilder();
                var customer = tableWork.CustomerRepository
                    .Get(x => x.Id == customerId && x.IsDeleted == false)
                    .FirstOrDefault();

                try
                {
                    var emailNotification = db.EmailContactNotifications
                        .Where(x => x.CustomerId == customerId && x.IsActive == true && x.Type == type)
                        .OrderByDescending(x => x.Id)
                        .FirstOrDefault();

                    if (emailNotification != null)
                    {
                        if (!defaultCCMail.ToString().IsNullOrEmpty())
                        {
                            var defaultCCEmail = defaultCCMail.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                            foreach (var email in defaultCCEmail)
                            {
                                message.CC.Add(new MailAddress(email));
                            }
                        }
                        if (emailNotification.ToEmailId != null)
                        {
                            var toEmailIds = emailNotification.ToEmailId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                            foreach (var item in toEmailIds)
                            {
                                var toresult = tableWork.CustomerContactRepository.Get(x => x.Id == int.Parse(item) && x.IsDeleted == false)
                                    .FirstOrDefault();
                                if (toresult != null)
                                {
                                    message.To.Add(new MailAddress(toresult.Email));
                                    receipientNamesBuilder.Append(toresult.ContactName);
                                    receipientNamesBuilder.Append("/ ");
                                }
                            }
                        }
                        if (emailNotification.CcEmailId != null)
                        {
                            var ccEmailIds = emailNotification.CcEmailId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                            foreach (var item in ccEmailIds)
                            {
                                var ccresult = tableWork.CustomerContactRepository.Get(x => x.Id == int.Parse(item) && x.IsDeleted == false)
                                    .Select(x => x.Email)
                                    .FirstOrDefault();
                                if (ccresult != null)
                                    message.CC.Add(new MailAddress(ccresult));
                            }
                        }
                        InvoiceMaster latestInvoice = null;
                        foreach (string invoiceNumber in invoiceNos)
                        {
                            string InvoiceReportUrl = $"{ReportServerUrl}/{InvoiceReportPath}&rs:Command=Render&InvoiceNo={invoiceNumber}&rs:Format=PDF";
                            string ArtworkReportUrl = $"{ReportServerUrl}/{ArtworkReportPath}&rs:Command=Render&InvoiceNo={invoiceNumber}&rs:Format=PDF";
                            string DigitizingReportUrl = $"{ReportServerUrl}/{DigitizingReportPath}&rs:Command=Render&InvoiceNo={invoiceNumber}&rs:Format=PDF";

                            var annexureCheck = tableWork.InvoiceMasterRepository.Get(x => x.InvoiceNo == invoiceNumber && x.IsDeleted == false).FirstOrDefault();
                            if (annexureCheck.InvoiceMonth != null && annexureCheck.InvoiceYear != null)
                            {
                                invoiceMonthName = new DateTime(1, (int)annexureCheck.InvoiceMonth, 1).ToString("MMMM", CultureInfo.InvariantCulture);
                                invoiceYear = (int)annexureCheck.InvoiceYear;
                            }
                            if (latestInvoice == null ||
                                annexureCheck.InvoiceYear > latestInvoice.InvoiceYear ||
                                (annexureCheck.InvoiceYear == latestInvoice.InvoiceYear && annexureCheck.InvoiceMonth > latestInvoice.InvoiceMonth))
                            {
                                latestInvoice = annexureCheck;  // Store the latest invoice
                            }

                            using (HttpClient client = new HttpClient(new HttpClientHandler { Credentials = new NetworkCredential(userName1, password1) }))
                            {
                                HttpResponseMessage response = await client.GetAsync(InvoiceReportUrl);

                                if (response.IsSuccessStatusCode)
                                {
                                    byte[] reportBytes = await response.Content.ReadAsByteArrayAsync();
                                    pdfFilePath = Path.Combine(TempPath, $"{customer.ShortName}_{invoiceNumber}.pdf");
                                    await File.WriteAllBytesAsync(pdfFilePath, reportBytes);
                                    var attachment = new Attachment(pdfFilePath, "application/pdf");
                                    message.Attachments.Add(attachment);
                                }
                                else
                                {
                                    var desc1 = invoiceNumber + " not send. To-emails: " + message.To + " and CC-emails: " + message.CC + " because Failed to retrieve the invoice from the server";
                                    var logtype1 = "Error";
                                    LogError(customerId, desc1, logtype1);
                                    errors.Add(new
                                    {
                                        customer.ShortName,
                                        invoiceNumber,
                                        message = "Failed to retrieve the invoice from the server."
                                    });
                                }

                                if (annexureCheck.ArtInvoiceAmount > 0)
                                {
                                    using (HttpResponseMessage artworkResponse = await client.GetAsync(ArtworkReportUrl))
                                    {
                                        if (artworkResponse.IsSuccessStatusCode)
                                        {
                                            byte[] reportBytes = await artworkResponse.Content.ReadAsByteArrayAsync();
                                            pdfFilePath = Path.Combine(TempPath, $"{customer.ShortName}_{invoiceNumber}_ArtAnnexure.pdf");
                                            await File.WriteAllBytesAsync(pdfFilePath, reportBytes);
                                            var attachment = new Attachment(pdfFilePath, "application/pdf");
                                            message.Attachments.Add(attachment);
                                        }
                                    }
                                }

                                if (annexureCheck.DigiInvoiceAmount > 0)
                                {
                                    using (HttpResponseMessage digiResponse = await client.GetAsync(DigitizingReportUrl))
                                    {
                                        if (digiResponse.IsSuccessStatusCode)
                                        {
                                            byte[] reportBytes = await digiResponse.Content.ReadAsByteArrayAsync();
                                            pdfFilePath = Path.Combine(TempPath, $"{customer.ShortName}_{invoiceNumber}_DigiAnnexure.pdf");
                                            await File.WriteAllBytesAsync(pdfFilePath, reportBytes);
                                            var attachment = new Attachment(pdfFilePath, "application/pdf");
                                            message.Attachments.Add(attachment);
                                        }
                                    }
                                }
                            }

                        }
                        string receipientNames = receipientNamesBuilder.ToString().TrimEnd(' ', '/').Trim();
                        message.From = new MailAddress(FromMail, fromName);
                        var filteredInvoices = db.InvoiceMasters
                                                .Where(x => invoiceNos.Contains(x.InvoiceNo) && x.CustomerId == customerId && x.IsDeleted == false)
                                                .ToList();
                        var outstandingInvoices = db.InvoiceMasters
                                          .Where(x => x.CustomerId == customerId && x.InvoiceMonth < latestInvoice.InvoiceMonth && x.InvoiceYear <= latestInvoice.InvoiceYear && x.IsOutstanding == true && x.IsDeleted == false)
                                          .ToList();
                        var allInvoices = filteredInvoices
                                            .Union(outstandingInvoices)
                                            .GroupBy(x => x.InvoiceNo)
                                            .Select(g => g.First())
                                            .ToList();
                        if ((latestInvoice.InvoiceYear < lastYear ||
                         (latestInvoice.InvoiceYear == lastYear && latestInvoice.InvoiceMonth < lastMonth))
                         && latestInvoice.IsOutstanding)
                        {
                            outstandingInvoices.Add(latestInvoice);
                        }
                        if (outstandingInvoices.Count > 0)
                        {
                            int sNo = 1;
                            var sortedInvoices = allInvoices.OrderByDescending(x => x.InvoiceDate);
                            foreach (var items in sortedInvoices)
                            {
                                bool isInPreviousMonth = items.InvoiceDate.HasValue &&
                                                          items.InvoiceDate.Value.Year < DateTime.UtcNow.Year ||
                                                          (items.InvoiceDate.Value.Year == DateTime.UtcNow.Year &&
                                                          items.InvoiceDate.Value.Month < DateTime.UtcNow.Month);
                                //string rowStyle = isInPreviousMonth ? "background-color: yellow;" : "";
                                string contentStyle = isInPreviousMonth ? "style='background-color: yellow;'" : "";
                                var AdjustmentAmount = tableWork.ReceivableAdjustmentRepository
                                    .Get(x => x.InvoiceNo == items.InvoiceNo && x.IsInvoiceAdjustment == true)
                                    .Select(x => x.AdjustmentAmount)
                                    .FirstOrDefault() ?? 0;
                                var outstandindAmount = items.InvoiceValue - AdjustmentAmount;
                                var outstandingDays = (DateTime.UtcNow - items.InvoiceDate).Value;
                                rows.Append("<tr style='text-align: center;'>");
                                rows.Append($"<td><span {contentStyle}>{sNo}</span></td>");
                                rows.Append($"<td><span {contentStyle}>{items.InvoiceNo}</span></td>");
                                rows.Append($"<td><span {contentStyle}>{items.InvoiceDate?.ToString("dd-MM-yyyy")}</span></td>");
                                rows.Append($"<td><span {contentStyle}>{customer.Name}</span></td>");
                                rows.Append($"<td><span {contentStyle}>{customer.ShortName}</span></td>");
                                rows.Append($"<td><span {contentStyle}>{items.InvoiceValue}</span></td>");
                                rows.Append($"<td><span {contentStyle}>{AdjustmentAmount}</span></td>");
                                rows.Append($"<td><span {contentStyle}>{outstandindAmount}</span></td>");
                                rows.Append($"<td><span {contentStyle}>{((int)outstandingDays.TotalDays)}</span></td>");
                                rows.Append("</tr>");
                                sNo++;
                                InvoiceValueTotal += items.InvoiceValue;
                                AmountCollectedTotal += AdjustmentAmount;
                                outstandindAmountTotal += outstandindAmount;
                            }
                            string subject = $"{invoiceMonthName} {invoiceYear} invoice/Pending Invoice from VLead ";
                            var body = "<div style = 'font-family: \"Bookman Old Style\", serif; line-height: 1.5em;'>Hi " + receipientNames + ",<br><br>" +
                                       "Please find attached the invoice for the orders completed in the month of " + invoiceMonthName + " " + invoiceYear + ".<br><br>" +
                                       "As per our record the payments for the following invoices is still pending and it is highlighted below, please do the needful at the earliest." +
                                       "Many thanks for your help and understanding.<br><br>" +
                                       "<br>" + "<table border = '1' style = 'border: 5px;' > " +
                                       "<tr style='text-align: center;'>" +
                                       "<th style='background-color:#e6e6e6; color: #595959;'>S.No</th>" +
                                       "<th style='background-color:#e6e6e6; color: #595959;'>Invoice No</th>" +
                                       "<th style='background-color:#e6e6e6; color: #595959;'>Invoice Date</th>" +
                                       "<th style='background-color:#e6e6e6; color: #595959;'>Customer Name</th>" +
                                       "<th style='background-color:#e6e6e6; color: #595959;'>Customer ShortName</th>" +
                                       "<th style='background-color:#e6e6e6; color: #595959;'>Invoice Value in $</th>" +
                                       "<th style='background-color:#e6e6e6; color: #595959;'>Amount Collected in $</th>" +
                                       "<th style='background-color:#e6e6e6; color: #595959;'>Outstanding amount in $</th>" +
                                       "<th style='background-color:#e6e6e6; color: #595959;'>O/S Days</th>" +
                                       "</tr>" +
                                       "<tbody>" +
                                       rows +
                                       "</tbody>" +
                                       "<tr style='text-align: center;'>" +
                                       "<td style='background-color:#e6e6e6;'></td>" +
                                       "<td style='background-color:#e6e6e6;'></td>" +
                                       "<td style='background-color:#e6e6e6;'></td>" +
                                       "<td style='background-color:#e6e6e6;'></td>" +
                                       "<td style='background-color:#e6e6e6;'>Customer Wise Total</td>" +
                                       "<td style='background-color:#e6e6e6;'>" + InvoiceValueTotal + "</td>" +
                                       "<td style='background-color:#e6e6e6;'>" + AmountCollectedTotal + "</td>" +
                                       "<td style='background-color:#e6e6e6;'>" + outstandindAmountTotal + "</td>" +
                                       "<td style='background-color:#e6e6e6;'></td>" +
                                       "</tr>" +
                                       "</table>" +
                        "<br> If you have any questions about the invoice, please email your queries to <a href = 'mailto:accounting@vleadservices.com'> accounting@vleadservices.com </a> or <a href = 'mailto:marcus@vleadservices.com'> marcus@vleadservices.com </a>.We would be happy to help you.<br><br>" +

                        "<b> Note:</b><br>" +
                              "<ul>" +
                              "<li> <span style='background-color: yellow;'>If you pay through PayPal mention the email ID as <i> payments@vleadservices.com</mark> </span></i></li>" +
                              "<li> <span style='background-color: yellow;'>If you pay through Wire transfer mention the beneficiary name as 'VLead Design Services Private Limited'.</span></li>" +
                              "<li> <span style='background-color: yellow;'>If possible, please ask your Accounting team to send us the bank payment transfer advice or PayPal details. This would be useful to track the payment at our end.</span></li>" +
                              "<li> <span style='background-color: yellow;'>We do not accept the Check.</span></li>" +
                              "</ul><br>" +
                              "Many thanks for your business.<br><br>" +
                              "<span style = 'background-color: #00ffff;'>Check-out our new corporate video in <a href = 'https://www.youtube.com/watch?v=Hf5BlwIzdGc'style='color: #000000;'><b>Youtube</b></a> with our updated services and people and processes behind it</span><br><br>" +
                              "<span style = 'background-color: #000000;'><a href = '" + PortfolioLink + "' style='color: #ffffff;'> DOWNLOAD OUR PORTFOLIO </a></span><br><br>" +
                              "Warm regards and good luck, Accounting Team</div>" +
                              "<img src=\"" + LogoPath + "\">" +
                        "<div style = 'font-family: \"Bookman Old Style\", serif;'><br><i>Referrals are the lifeblood of our business.If you have a friend, business associate, or organization that can benefit from our experience and expert service, please contact <a href = 'mailto:marcus@vleadservices.com'> marcus@vleadservices.com </a>.Your referrals are the greatest compliment we can receive.Please do not keep us a secret!!!<br><br>" +
                        "<a href='mailto:accounting@vleadservices.com'>accounting@vleadservices.com</a><br>" +
                        "<a href='http://www.vleadservices.com'>www.vleadservices.com</a></i></div>";
                            message.Subject = subject;
                            message.Body = body;
                            message.IsBodyHtml = true;
                        }
                        else
                        {
                            message.Subject = invoiceMonthName + " " + invoiceYear + " Invoice from VLead";
                            var body = "<div style = 'font-family: \"Bookman Old Style\", serif; line-height: 1.5em;'>Hi " + receipientNames + ",<br><br>" +
                            "Please find attached the invoice for the orders completed in the month of " + invoiceMonthName + " " + latestInvoice.InvoiceYear + ".<br><br>" +
                            "Many thanks for your help and understanding.<br><br>" +

                            " If you have any questions about the invoice, please email your queries to <a href = 'mailto:accounting@vleadservices.com'> accounting@vleadservices.com </a> or <a href = 'mailto:marcus@vleadservices.com'> marcus@vleadservices.com </a>.We would be happy to help you.<br><br>" +

                            "<b> Note:</b><br>" +
                              "<ul>" +
                              "<li> <span style='background-color: yellow;'>If you pay through PayPal mention the email ID as <i> payments@vleadservices.com</mark> </span></i></li>" +
                              "<li> <span style='background-color: yellow;'>If you pay through Wire transfer mention the beneficiary name as 'VLead Design Services Private Limited'.</span></li>" +
                              "<li> <span style='background-color: yellow;'>If possible, please ask your Accounting team to send us the bank payment transfer advice or PayPal details. This would be useful to track the payment at our end.</span></li>" +
                              "<li> <span style='background-color: yellow;'>We do not accept the Check.</span></li>" +
                              "</ul><br>" +
                                  "Many thanks for your business.<br><br>" +
                                  "<span style = 'background-color: #00ffff;'>Checkout our <a href = 'https://www.youtube.com/watch?v=Hf5BlwIzdGc'style='color: #000000;'><b>Youtube</b></a> video to know more about our services</span><br><br>" +
                                  "<span style = 'background-color: #000000;'><a href = '" + PortfolioLink + "' style='color: #ffffff;'> DOWNLOAD OUR PORTFOLIO </a></span><br><br>" +
                                  "Warm regards and good luck, Accounting Team</div>" +
                                  "<img src=\"" + LogoPath + "\">" +
                            "<div style = 'font-family: \"Bookman Old Style\", serif;'><br><i>Referrals are the lifeblood of our business.If you have a friend, business associate, or organization that can benefit from our experience and expert service, please contact <a href = 'mailto:marcus@vleadservices.com'> marcus@vleadservices.com </a>.Your referrals are the greatest compliment we can receive.Please do not keep us a secret!!!<br><br>" +

                            "<a href = 'mailto:accounting@vleadservices.com'> accounting@vleadservices.com </a><br>" +
                            "<a href = 'http://www.vleadservices.com'>www.vleadservices.com </a></i></div>";
                            message.Body = body;
                            message.IsBodyHtml = true;
                        }

                        using (var smtp = new SmtpClient(host, port))
                        {
                            smtp.UseDefaultCredentials = defaultCredential;
                            smtp.Credentials = new NetworkCredential(userName, password);
                            smtp.EnableSsl = Ssl;
                            smtp.Send(message);
                            success = true;
                        }

                        message.Dispose();
                        var desc = request.InvoiceNo + " sent successfully. To-emails: " + message.To + " and  CC-emails: " + message.CC;
                        var logtype = "Success";
                        LogError(customerId, desc, logtype);
                    }
                    else
                    {
                        var desc1 = request.InvoiceNo + " not send, Reason: No email contact found for the specified customer and type";
                        var logtype = "Error";
                        LogError(customerId, desc1, logtype);
                        errors.Add(new
                        {
                            customer.ShortName,
                            request.InvoiceNo,
                            message = "No email contact found for the specified customer and type."
                        });
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex, customerId, request.InvoiceNo);
                    errors.Add(new
                    {
                        customer.ShortName,
                        request.InvoiceNo,
                        message = ex.Message
                    });
                }
                finally
                {
                    // Clean up temporary files
                    if (File.Exists(TempPath))
                    {
                        File.Delete(TempPath);
                    }
                }

                return new
                {
                    success = success,
                    statuscode = success ? 200 : 500,
                    message = success ? "Mail sent successfully." : "Failed to send mail.",
                    error = errors
                };
            }
            else if (type == "ARFollowUp")
            {
                StringBuilder receipientNamesBuilder = new StringBuilder();
                StringBuilder invoiceMonthBuilder = new StringBuilder();
                var customerIds = request.CustomerId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                foreach (var clientId in customerIds)
                {
                    int customerId = int.Parse(clientId);
                    var currentDate = DateTime.UtcNow.AddDays(-60);
                    var firstArFollowUpDate = DateTime.UtcNow.AddDays(-15);

                    var customer = tableWork.CustomerRepository
                    .Get(x => x.Id == customerId && x.IsDeleted == false)
                    .FirstOrDefault();
                    var emailNotification = db.EmailContactNotifications
                                             .Where(x => x.CustomerId == customerId && x.IsActive == true && x.Type == type)
                                             .OrderByDescending(x => x.Id)
                                            .FirstOrDefault();
                    try
                    {
                        if (emailNotification != null)
                        {
                            if (!defaultCCMail.ToString().IsNullOrEmpty())
                            {
                                var defaultCCEmail = defaultCCMail.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                                foreach (var email in defaultCCEmail)
                                {
                                    message.CC.Add(new MailAddress(email));
                                }
                            }
                            if (emailNotification.ToEmailId != null)
                            {
                                var toEmailIds = emailNotification.ToEmailId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                                foreach (var item in toEmailIds)
                                {
                                    var toresult = tableWork.CustomerContactRepository.Get(x => x.Id == int.Parse(item) && x.IsDeleted == false)
                                        .FirstOrDefault();
                                    if (toresult != null)
                                    {
                                        message.To.Add(new MailAddress(toresult.Email));
                                        receipientNamesBuilder.Append(toresult.ContactName);
                                        receipientNamesBuilder.Append("/ ");
                                    }
                                }
                            }
                            if (emailNotification.CcEmailId != null)
                            {
                                var ccEmailIds = emailNotification.CcEmailId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                                foreach (var item in ccEmailIds)
                                {
                                    var ccresult = tableWork.CustomerContactRepository.Get(x => x.Id == int.Parse(item) && x.IsDeleted == false)
                                        .Select(x => x.Email)
                                        .FirstOrDefault();
                                    if (ccresult != null)
                                        message.CC.Add(new MailAddress(ccresult));
                                }
                            }
                            var invoiceDetails = tableWork.InvoiceMasterRepository
                            .Get(x => x.CustomerId == customerId &&
                                    x.IsOutstanding == true &&
                                    x.InvoiceDate <= currentDate &&
                                    x.IsDeleted == false)
                            .ToList();
                            var firstARFollowUpInvoiceDetails = tableWork.InvoiceMasterRepository
                            .Get(x => x.CustomerId == customerId &&
                                    x.IsOutstanding == true &&
                                    x.InvoiceDate <= firstArFollowUpDate &&
                                    x.InvoiceDate >currentDate &&
                                    x.IsDeleted == false)
                            .ToList();

                            if (invoiceDetails.Any())
                            {
                                int sNo = 1;
                                int? pendingInvoiceYear = null;
                                foreach (var invoice in invoiceDetails)
                                {
                                    string InvoiceReportUrl = $"{ReportServerUrl}/{InvoiceReportPath}&rs:Command=Render&InvoiceNo={invoice.InvoiceNo}&rs:Format=PDF";
                                    string ArtworkReportUrl = $"{ReportServerUrl}/{ArtworkReportPath}&rs:Command=Render&InvoiceNo={invoice.InvoiceNo}&rs:Format=PDF";
                                    string DigitizingReportUrl = $"{ReportServerUrl}/{DigitizingReportPath}&rs:Command=Render&InvoiceNo={invoice.InvoiceNo}&rs:Format=PDF";
                                    pendingInvoiceYear = invoice.InvoiceYear;


                                    using (HttpClient client = new HttpClient(new HttpClientHandler { Credentials = new NetworkCredential(userName1, password1) }))
                                    {
                                        HttpResponseMessage response = await client.GetAsync(InvoiceReportUrl);

                                        if (response.IsSuccessStatusCode)
                                        {
                                            byte[] reportBytes = await response.Content.ReadAsByteArrayAsync();
                                            MemoryStream stream = new MemoryStream(reportBytes);
                                            var attachment = new Attachment(stream, $"Invoice_{invoice.InvoiceNo}.pdf", "application/pdf");
                                            message.Attachments.Add(attachment);
                                        }
                                        else
                                        {
                                            var desc1 = invoiceDetails + " not send. To-emails: " + message.To + " and CC-emails: " + message.CC + " because Failed to retrieve the invoice from the server";
                                            var logtype1 = "Error";
                                            LogError(customerId, desc1, logtype1);
                                            errors.Add(new
                                            {
                                                customer.ShortName,
                                                invoice.InvoiceNo,
                                                message = "Failed to retrieve the invoice from the server."
                                            });
                                        }

                                        if (invoice.ArtInvoiceAmount > 0)
                                        {
                                            using (HttpResponseMessage artworkResponse = await client.GetAsync(ArtworkReportUrl))
                                            {
                                                if (artworkResponse.IsSuccessStatusCode)
                                                {
                                                    byte[] reportBytes = await artworkResponse.Content.ReadAsByteArrayAsync();
                                                    MemoryStream stream = new MemoryStream(reportBytes);
                                                    var attachment = new Attachment(stream, $"ArtAnnexure_{invoice.InvoiceNo}.pdf", "application/pdf");
                                                    message.Attachments.Add(attachment);
                                                }
                                            }
                                        }

                                        if (invoice.DigiInvoiceAmount > 0)
                                        {
                                            using (HttpResponseMessage digiResponse = await client.GetAsync(DigitizingReportUrl))
                                            {
                                                if (digiResponse.IsSuccessStatusCode)
                                                {
                                                    byte[] reportBytes = await digiResponse.Content.ReadAsByteArrayAsync();
                                                    MemoryStream stream = new MemoryStream(reportBytes);
                                                    var attachment = new Attachment(stream, $"DigiAnnexure_{invoice.InvoiceNo}.pdf", "application/pdf");
                                                    message.Attachments.Add(attachment);
                                                }
                                            }
                                        }
                                    }
                                    string[] monthNames = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                                    int monthNumber = invoice.InvoiceMonth ?? 0;
                                    if (monthNumber > 0)
                                    {
                                        monthName = monthNames[monthNumber - 1];
                                    }
                                    invoiceMonthBuilder.Append(monthName);
                                    invoiceMonthBuilder.Append(", ");
                                    pendingInvoiceYear = invoice.InvoiceYear;
                                    var AdjustmentAmount = tableWork.ReceivableAdjustmentRepository
                                                .Get(x => x.InvoiceNo == invoice.InvoiceNo && x.IsInvoiceAdjustment == true)
                                                .Select(x => x.AdjustmentAmount)
                                                .FirstOrDefault() ?? 0;
                                    var outstandindAmount = invoice.InvoiceValue - AdjustmentAmount;
                                    var outstandingDays = (DateTime.UtcNow - invoice.InvoiceDate).Value;
                                    rows.Append("<tr style = 'text-align: center;'>");
                                    rows.Append("<td><span style='background-color: yellow;'>" + sNo + "</td>");
                                    rows.Append("<td><span style='background-color: yellow;'>" + invoice.InvoiceNo + "</td>");
                                    rows.Append("<td><span style='background-color: yellow;'>" + invoice.InvoiceDate?.ToString("dd-MM-yyyy") + "</td>");
                                    rows.Append("<td><span style='background-color: yellow;'>" + customer.Name + "</td>");
                                    rows.Append("<td><span style='background-color: yellow;'>" + customer.ShortName + "</td>");
                                    rows.Append("<td><span style='background-color: yellow;'>" + invoice.InvoiceValue + "</td>");
                                    rows.Append("<td><span style='background-color: yellow;'>" + AdjustmentAmount + "</td>");
                                    rows.Append("<td><span style='background-color: yellow;'>" + outstandindAmount + "</td>");
                                    rows.Append("<td><span style='background-color: yellow;'>" + ((int)outstandingDays.TotalDays) + "</td>");
                                    rows.Append("</tr>");
                                    sNo++;
                                    InvoiceValueTotal += invoice.InvoiceValue;
                                    AmountCollectedTotal += AdjustmentAmount;
                                    outstandindAmountTotal += outstandindAmount;
                                }
                                string receipientNames = receipientNamesBuilder.ToString().TrimEnd(' ', '/').Trim();
                                string invoiceMonths = invoiceMonthBuilder.ToString().TrimEnd(' ', ',').Trim();
                                message.From = new MailAddress(FromMail, fromName);

                                message.Subject = "Pending Invoice from VLead - " + monthName + " " + pendingInvoiceYear;
                                var body = "<div style = 'font-family: \"Bookman Old Style\", serif; line-height: 1.5em;'>Hi " + receipientNames + ",<br><br>" +
                                "As per our record the payment for invoice is still pending and it is highlighted below, please do the needful at the earliest." +
                                "Many thanks for your help and understanding.<br><br>" +
                                               "<br>" + "<table border = '1' style = 'border: 5px;' > " +
                                               "<tr style='text-align: center;'>" +
                                               "<th style='background-color:#e6e6e6; color: #595959;'>S.No</th>" +
                                               "<th style='background-color:#e6e6e6; color: #595959;'>Invoice No</th>" +
                                               "<th style='background-color:#e6e6e6; color: #595959;'>Invoice Date</th>" +
                                               "<th style='background-color:#e6e6e6; color: #595959;'>Customer Name</th>" +
                                               "<th style='background-color:#e6e6e6; color: #595959;'>Customer ShortName</th>" +
                                               "<th style='background-color:#e6e6e6; color: #595959;'>Invoice Value in $</th>" +
                                               "<th style='background-color:#e6e6e6; color: #595959;'>Amount Collected in $</th>" +
                                               "<th style='background-color:#e6e6e6; color: #595959;'>Outstanding amount in $</th>" +
                                               "<th style='background-color:#e6e6e6; color: #595959;'>O/S Days</th>" +
                                               "</tr>" +
                                               "<tbody>" +
                                               rows +
                                               "</tbody>" +
                                               "<tr style='text-align: center;'>" +
                                               "<td style='background-color:#e6e6e6;'></td>" +
                                               "<td style='background-color:#e6e6e6;'></td>" +
                                               "<td style='background-color:#e6e6e6;'></td>" +
                                               "<td style='background-color:#e6e6e6;'></td>" +
                                               "<td style='background-color:#e6e6e6;'>Customer Wise Total</td>" +
                                               "<td style='background-color:#e6e6e6;'>" + InvoiceValueTotal + "</td>" +
                                               "<td style='background-color:#e6e6e6;'>" + AmountCollectedTotal + "</td>" +
                                               "<td style='background-color:#e6e6e6;'>" + outstandindAmountTotal + "</td>" +
                                               "<td style='background-color:#e6e6e6;'></td>" +
                                               "</tr>" +
                                               "</table>" +
                                "<br> If you have any questions about the invoice, please email your queries to <a href = 'mailto:accounting@vleadservices.com'> accounting@vleadservices.com </a> or <a href = 'mailto:marcus@vleadservices.com'> marcus@vleadservices.com </a>.We would be happy to help you.<br><br>" +

                                "<b> Note:</b><br>" +
                                      "<ul>" +
                                      "<li> <span style='background-color: yellow;'>If you pay through PayPal mention the email ID as <i> payments@vleadservices.com</mark> </span></i></li>" +
                                      "<li> <span style='background-color: yellow;'>If you pay through Wire transfer mention the beneficiary name as 'VLead Design Services Private Limited'.</span></li>" +
                                      "<li> <span style='background-color: yellow;'>If possible, please ask your Accounting team to send us the bank payment transfer advice or PayPal details. This would be useful to track the payment at our end.</span></li>" +
                                      "<li> <span style='background-color: yellow;'>We do not accept the Check.</span></li>" +
                                      "</ul><br>" +
                                      "Many thanks for your business.<br><br>" +
                                      "<span style = 'background-color: #00ffff;'>Check-out our new corporate video in <a href = 'https://www.youtube.com/watch?v=Hf5BlwIzdGc'style='color: #000000;'><b>Youtube</b></a> with our updated services and people and processes behind it</span><br><br>" +
                                      "<span style = 'background-color: #000000;'><a href = '" + PortfolioLink + "' style='color: #ffffff;'> DOWNLOAD OUR PORTFOLIO </a></span><br><br>" +
                                      "Warm regards and good luck, Accounting Team</div>" +
                                      "<img src=\"" + LogoPath + "\">" +
                                "<div style = 'font-family: \"Bookman Old Style\", serif;'><br><i>Referrals are the lifeblood of our business.If you have a friend, business associate, or organization that can benefit from our experience and expert service, please contact <a href = 'mailto:marcus@vleadservices.com'> marcus@vleadservices.com </a>.Your referrals are the greatest compliment we can receive.Please do not keep us a secret!!!<br><br>" +

                                "<a href = 'mailto:accounting@vleadservices.com'> accounting@vleadservices.com </a><br>" +
                                "<a href = 'http://www.vleadservices.com'>www.vleadservices.com </a></i></div>";
                                message.Body = body;
                                message.IsBodyHtml = true;
                                using (var smtp = new SmtpClient(host, port))
                                {
                                    smtp.UseDefaultCredentials = defaultCredential;
                                    smtp.Credentials = new NetworkCredential(userName, password);
                                    smtp.EnableSsl = Ssl;
                                    smtp.Send(message);
                                    success = true;
                                }
                                var desc = "Mail sent to " + customer.Name + " successfully. To-emails: " + message.To + " and  CC-emails: " + message.CC;
                                var logtype = "Success";
                                LogError(customerId, desc, logtype);
                            }
                            if (firstARFollowUpInvoiceDetails.Any())
                            {
                                int sNo = 1;
                                foreach (var invoice in firstARFollowUpInvoiceDetails)
                                {
                                    string InvoiceReportUrl = $"{ReportServerUrl}/{InvoiceReportPath}&rs:Command=Render&InvoiceNo={invoice.InvoiceNo}&rs:Format=PDF";
                                    string ArtworkReportUrl = $"{ReportServerUrl}/{ArtworkReportPath}&rs:Command=Render&InvoiceNo={invoice.InvoiceNo}&rs:Format=PDF";
                                    string DigitizingReportUrl = $"{ReportServerUrl}/{DigitizingReportPath}&rs:Command=Render&InvoiceNo={invoice.InvoiceNo}&rs:Format=PDF";

                                    using (HttpClient client = new HttpClient(new HttpClientHandler { Credentials = new NetworkCredential(userName1, password1) }))
                                    {
                                        HttpResponseMessage response = await client.GetAsync(InvoiceReportUrl);

                                        if (response.IsSuccessStatusCode)
                                        {
                                            byte[] reportBytes = await response.Content.ReadAsByteArrayAsync();
                                            MemoryStream stream = new MemoryStream(reportBytes);
                                            var attachment = new Attachment(stream, $"Invoice_{invoice.InvoiceNo}.pdf", "application/pdf");
                                            message.Attachments.Add(attachment);
                                        }
                                        else
                                        {
                                            var desc1 = invoiceDetails + " not send. To-emails: " + message.To + " and CC-emails: " + message.CC + " because Failed to retrieve the invoice from the server";
                                            var logtype1 = "Error";
                                            LogError(customerId, desc1, logtype1);
                                            errors.Add(new
                                            {
                                                customer.ShortName,
                                                invoice.InvoiceNo,
                                                message = "Failed to retrieve the invoice from the server."
                                            });
                                        }

                                        if (invoice.ArtInvoiceAmount > 0)
                                        {
                                            using (HttpResponseMessage artworkResponse = await client.GetAsync(ArtworkReportUrl))
                                            {
                                                if (artworkResponse.IsSuccessStatusCode)
                                                {
                                                    byte[] reportBytes = await artworkResponse.Content.ReadAsByteArrayAsync();
                                                    MemoryStream stream = new MemoryStream(reportBytes);
                                                    var attachment = new Attachment(stream, $"ArtAnnexure_{invoice.InvoiceNo}.pdf", "application/pdf");
                                                    message.Attachments.Add(attachment);
                                                }
                                            }
                                        }

                                        if (invoice.DigiInvoiceAmount > 0)
                                        {
                                            using (HttpResponseMessage digiResponse = await client.GetAsync(DigitizingReportUrl))
                                            {
                                                if (digiResponse.IsSuccessStatusCode)
                                                {
                                                    byte[] reportBytes = await digiResponse.Content.ReadAsByteArrayAsync();
                                                    MemoryStream stream = new MemoryStream(reportBytes);
                                                    var attachment = new Attachment(stream, $"DigiAnnexure_{invoice.InvoiceNo}.pdf", "application/pdf");
                                                    message.Attachments.Add(attachment);
                                                }
                                            }
                                        }
                                    }
                                }
                                string receipientNames = receipientNamesBuilder.ToString().TrimEnd(' ', '/').Trim();
                                message.From = new MailAddress(FromMail, fromName);

                                message.Subject = "Please ignore if already paid - " + monthName + " " + invoiceYear + " Invoice from VLead";
                                var body = "<div style = 'font-family: \"Bookman Old Style\", serif; line-height: 1.5em;'>Hi " + receipientNames + ",<br><br>" +
                                "Please find attached the invoice for the orders completed in the month of " + monthName + " " + invoiceYear + ".<br><br>" +
                                "Many thanks for your help and understanding.<br><br>" +
                                "<br> If you have any questions about the invoice, please email your queries to <a href = 'mailto:accounting@vleadservices.com'> accounting@vleadservices.com </a> or <a href = 'mailto:marcus@vleadservices.com'> marcus@vleadservices.com </a>.We would be happy to help you.<br><br>" +

                                "<b> Note:</b><br>" +
                                      "<ul>" +
                                      "<li> <span style='background-color: yellow;'>If you pay through PayPal mention the email ID as <i> payments@vleadservices.com</mark> </span></i></li>" +
                                      "<li> <span style='background-color: yellow;'>If you pay through Wire transfer mention the beneficiary name as 'VLead Design Services Private Limited'.</span></li>" +
                                      "<li> <span style='background-color: yellow;'>If possible, please ask your Accounting team to send us the bank payment transfer advice or PayPal details. This would be useful to track the payment at our end.</span></li>" +
                                      "<li> <span style='background-color: yellow;'>We do not accept the Check.</span></li>" +
                                      "</ul><br>" +
                                      "Many thanks for your business.<br><br>" +
                                      "<span style = 'background-color: #00ffff;'>Check-out our new corporate video in <a href = 'https://www.youtube.com/watch?v=Hf5BlwIzdGc'style='color: #000000;'><b>Youtube</b></a> with our updated services and people and processes behind it</span><br><br>" +
                                      "<span style = 'background-color: #000000;'><a href = '" + PortfolioLink + "' style='color: #ffffff;'> DOWNLOAD OUR PORTFOLIO </a></span><br><br>" +
                                      "Warm regards and good luck, Accounting Team</div>" +
                                      "<img src=\"" + LogoPath + "\">" +
                                "<div style = 'font-family: \"Bookman Old Style\", serif;'><br><i>Referrals are the lifeblood of our business.If you have a friend, business associate, or organization that can benefit from our experience and expert service, please contact <a href = 'mailto:marcus@vleadservices.com'> marcus@vleadservices.com </a>.Your referrals are the greatest compliment we can receive.Please do not keep us a secret!!!<br><br>" +

                                "<a href = 'mailto:accounting@vleadservices.com'> accounting@vleadservices.com </a><br>" +
                                "<a href = 'http://www.vleadservices.com'>www.vleadservices.com </a></i></div>";
                                message.Body = body;
                                message.IsBodyHtml = true;
                                using (var smtp = new SmtpClient(host, port))
                                {
                                    smtp.UseDefaultCredentials = defaultCredential;
                                    smtp.Credentials = new NetworkCredential(userName, password);
                                    smtp.EnableSsl = Ssl;
                                    smtp.Send(message);
                                    success = true;
                                }

                                message.Dispose();
                                var desc = "Mail sent to " + customer.Name + " successfully. To-emails: " + message.To + " and  CC-emails: " + message.CC;
                                var logtype = "Success";
                                LogError(customerId, desc, logtype);
                            }
                            else if(!invoiceDetails.Any() && !firstARFollowUpInvoiceDetails.Any())
                            {
                                var desc1 = "Invoice not found for specified customer in InvoiceMaster";
                                var logtype = "Error";
                                LogError(customerId, desc1, logtype);
                                errors.Add(new
                                {
                                    customer.ShortName,
                                    message = "Invoice Not found"
                                });
                            }
                        }
                        else
                        {
                            var desc1 = "ARFollow Up not send for " + customer.Name + ", Reason: No email contact found for the specified customer and type";
                            var logtype = "Error";
                            LogError(customerId, desc1, logtype);
                            errors.Add(new
                            {
                                customer.ShortName,
                                customer.Id,
                                message = "No email contact found for the specified customer and type."
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, customerId, "");
                        errors.Add(new
                        {
                            customer.Name,
                            message = ex.Message
                        });
                    }
                }
                return new
                {
                    success = success,
                    statuscode = success ? 200 : 500,
                    message = success ? "Mail sent successfully." : "Failed to send mail.",
                    error = errors
                };
            }

            return null;
        }





        private void LogError(Exception ex, int customerId, string invoiceNumber)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                Log savelog = new Log();
                savelog.Module = "Invoice reminder";
                savelog.Description = $"{invoiceNumber} not sent. Error message: {ex.Message}. Innerexception: {ex.InnerException}. Stack trace: {ex.StackTrace}";
                savelog.Type = "Error Occ";
                savelog.ClientId = customerId;
                savelog.CreatedUtc = DateTime.UtcNow;
                _context.Logs.Add(savelog);
                _context.SaveChanges();
                transaction.Commit();
            }
        }
        private void LogError(int customerId, string desc, string type)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                Log savelog = new Log();
                savelog.Module = "Invoice reminder";
                savelog.Description = desc;
                savelog.Type = type;
                savelog.ClientId = customerId;
                savelog.CreatedUtc = DateTime.UtcNow;
                _context.Logs.Add(savelog);
                _context.SaveChanges();
                transaction.Commit();
            }
        }
        public List<InvoiceMaster> GetARFollowUpCustomerList()
        {
            var currentDate = DateTime.UtcNow.AddDays(-60);

            var list = tableWork.InvoiceMasterRepository
                .GetAll()
                .Where(x => x.IsOutstanding == true &&
                            x.IsDeleted == false &&
                            x.InvoiceDate <= currentDate)
                .GroupBy(x => x.CustomerId)
                .Select(group => group.FirstOrDefault())
                .ToList();

            return list ?? new List<InvoiceMaster>();
        }

    }
}
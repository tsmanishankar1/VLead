using System.Net.Mail;
using System.Net;
using Vis.VleadProcessV3.Models;
//using System.Data.SqlClient;
using Vis.VleadProcessV3.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Vis.VleadProcessV3.Repositories;

namespace Vis.VleadProcessV3.Services
{
    public class CustomerService
    {
        private readonly TableWork _tableWork;
        private readonly TableWork tow;
        private readonly UnitWork _unitWork;
        private readonly ViewWork _viewWork;
        private readonly ProcedureWork _procedureWork;
        private readonly IConfiguration _configuration;
       
        public CustomerService(IConfiguration configuration,TableWork tableWork,UnitWork unitWork,ViewWork viewWork,ProcedureWork procedureWork)
        {
            _configuration = configuration;
            this.tow= tableWork;    
            _tableWork = tableWork;
            _unitWork = unitWork; 
            _viewWork = viewWork;
            _procedureWork = procedureWork;
        }
      
        public IEnumerable<Customer> GetCustomers()
        {
            return _tableWork.CustomerRepository.Get(x => x.IsBlacklisted == false && x.IsDeleted == false);
        }
        public Customer GetCustomer(int customerId, bool includeCustomerClassification = false)
        {
            return (includeCustomerClassification) ?
                _tableWork.CustomerRepository.GetAllVal(c => c.CustomerClassification).FirstOrDefault(x => x.Id == customerId) :
                _tableWork.CustomerRepository.GetSingle(x => x.Id == customerId);
        }
      
        public Object GetRemainderReport(GetRemainderReportData RemainderReport)
        {
            var MCClients = "";
            string idval = string.Empty;
            var ClientId = RemainderReport.customerId;
            bool ReturnSuccess = false;
            int GcountS = 0;
            int GcountE = 0;
            foreach (var id in ClientId)
            {
                var myCheck = OSDetails(RemainderReport.Date.Value.Date, Convert.ToInt32(id), 0);
                int idval1 = Convert.ToInt32(id);
                var message = new System.Net.Mail.MailMessage();
                if (myCheck.Count() > 0)
                {
                    GcountS++;
                    //Using Mail Contacts Form Mail Send to Remainder Letter for outstanding of all clients
                    var MailNotification = _tableWork.MailContactRepository.Get(x => x.IsDeleted == false && x.ClientId == idval1 && x.ProcessName == "Remainder Letter").ToList();
                    if (MailNotification.Count > 0)
                    {
                        var FromMail = _tableWork.MailContactRepository.Get(x => x.IsDeleted == false && x.ClientId == idval1 && x.ProcessName == "Remainder Letter").FirstOrDefault().FromMail;
                        message.From = new MailAddress(FromMail.ToString());
                    }

                    if (MailNotification.Count >= 1)
                    {

                        foreach (var Mail in MailNotification)
                        {
                            if (Mail.ToMail != "" && Mail.ToMail != null)
                            {
                                message.To.Add(new MailAddress(Mail.ToMail));

                            }
                            if (Mail.Ccmail != "" && Mail.Ccmail != null)
                            {
                                message.CC.Add(new MailAddress(Mail.Ccmail));
                            }
                        }

                        var CCmail = _configuration.GetSection("RemainderLetter").GetValue<string>("ReminderLetterCCMail"); //
                        var ToName = _configuration.GetSection("RemainderLetter").GetValue<string>("ReminderLetterToName"); //
                        message.CC.Add(new MailAddress(CCmail));

                        string htmlTableStart = "<html lang='en'><head><meta content='text/html; charset=utf-8' http-equiv='Content-Type'>" +
                                            "<style type='text/css'>" +
                                            "HTML{background-color: #e8e8e8;}" +
                                            ".courses-table{font-size: 14px; padding: 10px; border-collapse: collapse; border-spacing: 0;}" +
                                            ".courses-table .description{color: #505050;}" +
                                            ".courses-table td{border: 1px solid #D1D1D1; background-color: #F3F3F3; padding: 0 10px;}" +
                                            ".courses-table th{border: 1px solid #424242; color: #FFFFFF;text-align: left; padding: 0 10px;}" +
                                            ".green{background-color: #1E90FF;}" +
                                            "</style>" +
                                            "</head><body><div><table class='courses-table'><thead><tr><th class='green'>S.No</th><th class='green'>InvoiceNo</th><th class='green'>InvoiceDate</th><th class='green'>Customer Name</th><th class='green'>Total InvoiceValue</th><th class='green'>Amount Collected</th><th class='green'>Outstanding Amount</th><th class='green'>Outstanding Day</th></tr></thead><tbody>";
                        string htmlTrStart = "<tr>";

                        string htmlTrEnd = "</tr>";

                        string htmlTdStart = "<td>";
                        string htmlTdEnd = "</td>";
                        string htmlTableEnd = "</tbody></table></div></body></html>";

                        string Content = htmlTableStart;
                        Nullable<decimal> TotalInvoiceValue = 0;
                        Nullable<decimal> AmountCollected = 0;
                        Nullable<decimal> OutstandingAmount = 0;
                        int autoint = 0;
                        foreach (var items in myCheck.OrderByDescending(x => x.Id))
                        {
                            if (items.InvoiceNo != null)
                            {
                                autoint++;
                                //TotalAmount = TotalAmount + items.Out;
                                Content += htmlTrStart + htmlTdStart + autoint + htmlTdEnd + htmlTdStart + items.InvoiceNo + htmlTdEnd + htmlTdStart + items.InvoiceDate + htmlTdEnd + htmlTdStart + items.CustomerName + htmlTdEnd + htmlTdStart + "&#36;" + items.TotalInvoiceValue + htmlTdEnd + htmlTdStart + "&#36;" + items.AmountCollected + htmlTdEnd + htmlTdStart + "&#36;" + items.OutstandingAmount + htmlTdEnd + htmlTdStart + items.OutstandingDay + htmlTdEnd + htmlTrEnd;
                            }
                            else
                            {
                                TotalInvoiceValue = items.TotalInvoiceValue;
                                AmountCollected = items.AmountCollected;
                                OutstandingAmount = items.OutstandingAmount;
                            }

                        }

                        Content += "<tr><td></td><td></td><td></td><td><b>Customer Wise Total</b></td><td>" + "&#36;" + TotalInvoiceValue + "</td><td>" + "&#36;" + AmountCollected + "</td><td>" + "&#36;" + OutstandingAmount + "</td><td></td></tr>" + htmlTableEnd;
                        message.Subject = "Reminder Letter";
                        message.Body = "Hi </br></br>" + "This is " + ToName + " from the Accounting team.<br/><br/> As per reconciliation of records we have below invoice(s) pending. If already paid please send us the receipt for reconciliation." +
                                       "<br/><br/>" + Content + "<br/><br/>" +
                                       "Many thanks for your help and understanding.<br/><br/>" +
                                       "If you have any questions about the invoice, please e-mail your queries to accounting@vleadservices.com or marcus@vleadservices.com. We would be happy to help you. <br/><br/>" +
                                       "Note: <br/>" +
                                       "<ul><li>If you pay through PayPal mention the email ID as payments@vleadservices.com</li><li>If you pay through Wire transfer mention the beneficiary name as " + "' VLead Design Services Private Limited '" + "</li><li>If possible, please ask your Accounting team to send us the bank payment transfer advice or PayPal details.  This would be useful to track the payment at our end.</li></ul>" +
                                       "Many thanks for your business.<br/><br/>" +
                                       "Checkout our " + "<a href=" + "https://youtu.be/QYkH57teex4" + ">YouTube</a>" + " video to know more about our services <br/><br/>" +
                                       "<a href=" + "https://goo.gl/AkzfQ1" + ">DOWNLOAD OUR PORTFOLIO</a> <br/><br/>" +
                                       "Warm regards and good luck, Accounting Team <br/><br/>" +
                                       "<h1><span><span style=" + "color:red" + ">V</span>lead</span></h1>" +
                                       "<i>Referrals are the lifeblood of our business.  If you have a friend, business associate or organization that can benefit from our experience and expert service, please contact marcus@vleadservices.com.   Your referrals are the greatest compliment we can receive.  Please do not keep us a secret!!! </i> <br/>" +
                                       "US Phone:  (248) 525-6936 (Direct) <br/>" +
                                       "accounting@vleadservices.com <br/>" +
                        "<a href=" + "http://www.vleadservices.com" + ">www.vleadservices.com</a>";
                        message.IsBodyHtml = true;
                        var host = _configuration.GetSection("Smtp").GetValue<string>("host");
                        var port = _configuration.GetSection("Smtp").GetValue<int>("port");
                        var userName = _configuration.GetSection("Smtp").GetValue<string>("userName");
                        var password = _configuration.GetSection("Smtp").GetValue<string>("password");
                        var userName1 = _configuration.GetSection("Smtp").GetValue<string>("userName1");
                        var password1 = _configuration.GetSection("Smtp").GetValue<string>("password1");
                        var Ssl = _configuration.GetSection("Smtp").GetValue<bool>("SSL");
                        var defaultCredential = _configuration.GetSection("Smtp").GetValue<bool>("defaultCredential");
                        using (var smtp = new SmtpClient(host,port))
                        {
                            smtp.UseDefaultCredentials = defaultCredential;
                         
                            if (message.From.ToString() == "accounting@vleadservices.com")
                            {
                                smtp.Credentials = new NetworkCredential(userName1, password1);

                            }
                            else
                            {
                                smtp.Credentials = new NetworkCredential(userName, password);

                            }
                            smtp.EnableSsl = Ssl;
                            smtp.Send(message);
                        }
                        ReturnSuccess = true;
                    }

                    else
                    {
                        var ClientName = _tableWork.CustomerRepository.Get(x => x.IsDeleted == false && x.Id == idval1).FirstOrDefault().ShortName;
                        MCClients += ClientName + ",";
                        ReturnSuccess = false;
                    }
                }
                else
                {
                    GcountE++;
                }
            }
            if (ReturnSuccess == false && MCClients != "")
            {
                var res = new
                {
                    ClientNames = "Please add mail contacts to these clients,  " + MCClients,
                };
                return res;
            }
            else if (ReturnSuccess == true && MCClients != "")
            {
                var res = new
                {
                    ClientNames = "Mail sent successfully and Please add mail contacts to these clients,  " + MCClients,
                };
                return res;
            }
            else
            {
                if (GcountE >= 1 && GcountS == 0)
                {
                    var res0 = new
                    {
                        ClientNames = "No Outstanding Balance",
                    };
                    return res0;
                }
                else if (GcountE >= 1 && GcountS >= 1)
                {
                    var res1 = new
                    {
                        ClientNames = "Mail sent successfully,but few has no Outstanding Balance",
                    };
                    return res1;
                }
                var res = new
                {
                    ClientNames = "Mail sent successfully",
                };
                return res;
            }

        }

        public List<OutStandingViewModel> OSDetails(DateTime reportAsOn, int Ids, int EmpId)
        {
            var outstandingAmount = 0L;
            IQueryable<OutStandingViewModel> outstandingDetailsBasedOnCustomer = null;
            List<OutStandingViewModel> outstandingDetailsBasedOnCustomer123 = new List<OutStandingViewModel>();
            //var dateSplit = reportAsOn.Split('T');
            //var date = Convert.ToDateTime(dateSplit[0]);
            var date = reportAsOn;
            var Id = 1;
            var recDate = date.AddDays(1);
            var outStandingDetailsLst = new List<OutStandingViewModel>();
            try
            {
                //if (Ids.Any())
                //{
                //foreach (var customerId in Ids)//selectedCustomers.
                //{
              
                var cancelledInvoices = (from A in _tableWork.InvoiceTranRepository.Get().Where(x => x.InvoiceNo != null)
                                         join B in _tableWork.InvoiceMasterRepository.Get() on A.Imid equals B.Id
                                         select B);


                var cancelledInvoices2 = (from invMaster in _tableWork.InvoiceMasterRepository.Get()
                                          join tran in _tableWork.InvoiceTranRepository.Get().Where(x => x.InvoiceNo != null) on invMaster.InvoiceNo equals tran.InvoiceNo
                                          select invMaster);

                var AllCancelledInvoices = cancelledInvoices.Union(cancelledInvoices2);
               

                outstandingDetailsBasedOnCustomer = (from i in _tableWork.InvoiceMasterRepository.Get().Except(AllCancelledInvoices)
                                                     join c in _tableWork.CustomerRepository.Get() on i.CustomerId equals c.Id
                                                     //SP
                                                     join cvse in _tableWork.CustomerVsSalesEmployeeRepository.Get() on c.Id equals cvse.CustomerId into tempT
                                                     from jcvse in tempT.DefaultIfEmpty() //SP
                                                     join recA in _tableWork.ReceivableAdjustmentRepository.Get() on i.Id equals recA.InvoiceId into jo_recA
                                                     from jrecA in jo_recA.DefaultIfEmpty()
                                                     join rec in _tableWork.ReceivableRepository.Get() on jrecA.ReceivableId equals rec.Id into jo_rec
                                                     from jrec in jo_rec.DefaultIfEmpty()
                                                    
                                                     select new OutStandingViewModel
                                                     {
                                                         Id = Id + 1,
                                                         CustomerId = i.CustomerId,
                                                         InvoiceNo = i.InvoiceNo,
                                                         InvoiceDate = i.InvoiceDate.Value.AddDays(-5),
                                                         TotalInvoiceValue = i.TotalInvoiceValue,//InvoiceValue = i.InvoiceValue,
                                                         IsDeleted = i.IsDeleted,
                                                         IsOutstanding = i.IsOutstanding,
                                                         CollectionDate = jrec.CollectionDate != null ? jrec.CollectionDate.Date : recDate,
                                                         AmountCollected = jrecA.AdjustmentAmount != null ? jrecA.AdjustmentAmount : 0,
                                                         OutstandingAmount = jrec.CollectionAmount != null ? jrec.CollectionAmount : 0,
                                                         CustomerName = c.Name,

                                                         ShortName = c.ShortName,
                                                         CustomerwithShortName = c.Name + " - " + c.ShortName,
                                                         VleadSalesPersonId = jcvse.EmployeeId,
                                                         VleadSalesPersonIsActive = jcvse.IsActive

                                                     }).Where(i => i.CustomerId == Ids &&
                                                         i.VleadSalesPersonId == (EmpId == 0 ? i.VleadSalesPersonId : EmpId) && i.VleadSalesPersonIsActive == true && //VSP
                                                                                                                                                                      //customerId
                                                          i.IsOutstanding == true && i.IsDeleted == false &&
                                                          i.InvoiceDate.Date <= recDate).AsQueryable();  //  date                  
                outstandingDetailsBasedOnCustomer123.AddRange(outstandingDetailsBasedOnCustomer);

                //}
                //}

                if (outstandingDetailsBasedOnCustomer123 != null)
                {
                    List<OutStandingViewModel> OutStandingDetailsList = new List<OutStandingViewModel>();
                    foreach (var item in outstandingDetailsBasedOnCustomer123)
                    {
                        bool alreadyExists = OutStandingDetailsList.Any(x => x.InvoiceNo == item.InvoiceNo);

                        if (alreadyExists == false)
                        {
                            OutStandingViewModel outstandingDetail = new OutStandingViewModel
                            {
                                CustomerName = item.CustomerName,
                                ShortName = item.ShortName,
                                CustomerwithShortName = item.CustomerName + " - " + item.ShortName,
                                CustomerId = item.CustomerId,
                                InvoiceNo = item.InvoiceNo,
                                InvoiceDate = item.InvoiceDate,
                                TotalInvoiceValue = item.TotalInvoiceValue,//InvoiceValue = item.InvoiceValue,
                                AmountCollected = 0,
                                OutstandingAmount = (decimal)(item.TotalInvoiceValue),// OutstandingAmount = (decimal)(item.InvoiceValue),                               
                                OutstandingDay = getday1(date, item.InvoiceDate),
                                FirstQuater = firstQuater(getday1(date, item.InvoiceDate), (decimal)(item.TotalInvoiceValue)), //(decimal)(item.InvoiceValue)
                                SecondQuater = secondQuater(getday1(date, item.InvoiceDate), (decimal)(item.TotalInvoiceValue)),//(decimal)(item.InvoiceValue)
                                ThirdQuater = thirdQuater(getday1(date, item.InvoiceDate), (decimal)(item.TotalInvoiceValue)),//(decimal)(item.InvoiceValue)
                                FoureQuater = foureQuater(getday1(date, item.InvoiceDate), (decimal)(item.TotalInvoiceValue)),//(decimal)(item.InvoiceValue)
                                FiveQuater = fiveQuater(getday1(date, item.InvoiceDate), (decimal)(item.TotalInvoiceValue)),//(decimal)(item.InvoiceValue)
                            };
                            OutStandingDetailsList.Add(outstandingDetail);
                        }

                    }

                    foreach (var item in outstandingDetailsBasedOnCustomer123)
                    {
                        bool alreadyExists = OutStandingDetailsList.Any(x => x.InvoiceNo == item.InvoiceNo);
                        if (alreadyExists == false)
                        {
                            OutStandingViewModel outstandingDetail = new OutStandingViewModel
                            {
                                CustomerName = item.CustomerName,
                                ShortName = item.ShortName,
                                CustomerwithShortName = item.CustomerName + " - " + item.ShortName,
                                CustomerId = item.CustomerId,
                                InvoiceNo = item.InvoiceNo,
                                InvoiceDate = item.InvoiceDate,
                                TotalInvoiceValue = item.TotalInvoiceValue,// InvoiceValue = item.InvoiceValue,
                                AmountCollected = item.AmountCollected,
                                OutstandingAmount = (decimal)(item.TotalInvoiceValue - item.AmountCollected),//OutstandingAmount = (decimal)(item.InvoiceValue - item.AmountCollected),
                                OutstandingDay = getday1(date, item.InvoiceDate),

                                FirstQuater = firstQuater(getday1(date, item.InvoiceDate), (decimal)(item.TotalInvoiceValue - item.AmountCollected)), //(decimal)(item.InvoiceValue)
                                SecondQuater = secondQuater(getday1(date, item.InvoiceDate), (decimal)(item.TotalInvoiceValue - item.AmountCollected)),//(decimal)(item.InvoiceValue)
                                ThirdQuater = thirdQuater(getday1(date, item.InvoiceDate), (decimal)(item.TotalInvoiceValue - item.AmountCollected)),//(decimal)(item.InvoiceValue)
                                FoureQuater = foureQuater(getday1(date, item.InvoiceDate), (decimal)(item.TotalInvoiceValue - item.AmountCollected)),//(decimal)(item.InvoiceValue)
                                FiveQuater = fiveQuater(getday1(date, item.InvoiceDate), (decimal)(item.TotalInvoiceValue - item.AmountCollected)),//(decimal)(item.InvoiceValue)

                            };
                            OutStandingDetailsList.Add(outstandingDetail);
                        }
                        else
                        {
                            if (item.CollectionDate <= recDate) //date
                            {
                                long NoDays = getday1(recDate, item.InvoiceDate); // date
                                var i = OutStandingDetailsList.FindIndex(x => x.InvoiceNo == item.InvoiceNo);

                                OutStandingDetailsList[i].AmountCollected = OutStandingDetailsList[i].AmountCollected + item.AmountCollected;
                                OutStandingDetailsList[i].OutstandingAmount = (decimal)(OutStandingDetailsList[i].TotalInvoiceValue - OutStandingDetailsList[i].AmountCollected);// OutStandingDetailsList[i].OutstandingAmount = (decimal)(OutStandingDetailsList[i].InvoiceValue - OutStandingDetailsList[i].AmountCollected);
                                OutStandingDetailsList[i].OutstandingDay = OutStandingDetailsList[i].OutstandingDay <= NoDays ? OutStandingDetailsList[i].OutstandingDay : NoDays;
                                OutStandingDetailsList[i].FirstQuater = firstQuater(OutStandingDetailsList[i].OutstandingDay, OutStandingDetailsList[i].OutstandingAmount);
                                OutStandingDetailsList[i].SecondQuater = secondQuater(OutStandingDetailsList[i].OutstandingDay, OutStandingDetailsList[i].OutstandingAmount);
                                OutStandingDetailsList[i].ThirdQuater = thirdQuater(OutStandingDetailsList[i].OutstandingDay, OutStandingDetailsList[i].OutstandingAmount);
                                OutStandingDetailsList[i].FoureQuater = foureQuater(OutStandingDetailsList[i].OutstandingDay, OutStandingDetailsList[i].OutstandingAmount);
                                OutStandingDetailsList[i].FiveQuater = fiveQuater(OutStandingDetailsList[i].OutstandingDay, OutStandingDetailsList[i].OutstandingAmount);
                            }
                        }
                    }

                    outStandingDetailsLst = OutStandingDetailsList.OrderBy(x => x.Id).ToList(); ;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            List<OutStandingViewModel> temp = (from t in outStandingDetailsLst
                                               group t by t.CustomerId into g
                                               select new OutStandingViewModel
                                               {

                                                   CustomerId = g.First().CustomerId,
                                                   CustomerName = g.First().CustomerName,
                                                   ShortName = g.First().ShortName,
                                                   CustomerwithShortName = g.First().CustomerName + " - " + g.First().ShortName,
                                                   TotalInvoiceValue = g.Sum(x => x.TotalInvoiceValue),//InvoiceValue = g.Sum(x => x.InvoiceValue),
                                                   AmountCollected = g.Sum(x => x.AmountCollected),
                                                   OutstandingAmount = g.Sum(x => x.OutstandingAmount),
                                                   FirstQuater = g.Sum(x => x.FirstQuater),
                                                   SecondQuater = g.Sum(x => x.SecondQuater),
                                                   ThirdQuater = g.Sum(x => x.ThirdQuater),
                                                   FoureQuater = g.Sum(x => x.FoureQuater),
                                                   FiveQuater = g.Sum(x => x.FiveQuater),
                                                   Status = "Total"
                                               }).ToList();
            var OutStanding = outStandingDetailsLst.Union(temp).ToList();
            var OutstandingDetail = OutStanding.OrderBy(x => x.ShortName).ThenByDescending(x => x.InvoiceDate).ToList();//CustomerName - shortname
            List<OutStandingViewModel> TotalOutstanddingList = new List<OutStandingViewModel>();
            if (OutstandingDetail != null)
            {
                foreach (var item in OutstandingDetail)
                {
                    if (item.Status == "Total")
                    {
                        TotalOutstanddingList.Add(new OutStandingViewModel
                        {
                            CustomerId = item.CustomerId,
                            CustomerName = item.CustomerName,
                            ShortName = item.ShortName,
                            CustomerwithShortName = item.CustomerName + " - " + item.ShortName,
                            InvoiceNo = item.InvoiceNo,
                            InvoiceDate = item.InvoiceDate,
                            TotalInvoiceValue = item.TotalInvoiceValue,//InvoiceValue = item.InvoiceValue,
                            AmountCollected = item.AmountCollected,
                            OutstandingAmount = item.OutstandingAmount,
                            OutstandingDay = item.OutstandingDay,

                            FirstQuater = item.FirstQuater,
                            SecondQuater = item.SecondQuater,
                            ThirdQuater = item.ThirdQuater,
                            FoureQuater = item.FoureQuater,
                            FiveQuater = item.FiveQuater,
                            Status = item.Status
                        });
                    }
                    else
                    {
                        TotalOutstanddingList.Add(new OutStandingViewModel
                        {
                            Id = Id++,
                            CustomerId = item.CustomerId,
                            CustomerName = item.CustomerName,
                            ShortName = item.ShortName,
                            CustomerwithShortName = item.CustomerName + " - " + item.ShortName,
                            InvoiceNo = item.InvoiceNo,
                            InvoiceDate = item.InvoiceDate,
                            TotalInvoiceValue = item.TotalInvoiceValue,//InvoiceValue = item.InvoiceValue,
                            AmountCollected = item.AmountCollected,
                            OutstandingAmount = item.OutstandingAmount,
                            OutstandingDay = item.OutstandingDay,

                            FirstQuater = item.FirstQuater,
                            SecondQuater = item.SecondQuater,
                            ThirdQuater = item.ThirdQuater,
                            FoureQuater = item.FoureQuater,
                            FiveQuater = item.FiveQuater,
                            Status = item.Status
                        });
                    }
                }
            }
            return TotalOutstanddingList.OrderBy(x => x.ShortName).ThenByDescending(x => x.InvoiceDate).ToList();   // customer name - shortname        
        }

        private long getday1(DateTime rptDate, DateTime invoiceDate)
        {
            var difference = rptDate.Date - invoiceDate.Date;
            var days = difference.TotalDays;
            return (long)days;
        }
        private decimal fiveQuater(long days, decimal outstandingAmount)
        {
            var amount = days >= 121 ? outstandingAmount : 0;
            return amount;
        }
        private decimal foureQuater(long days, decimal outstandingAmount)
        {
            var amount = days >= 91 && days <= 120 ? outstandingAmount : 0;
            return amount;
        }
        private decimal thirdQuater(long days, decimal outstandingAmount)
        {
            var amount = days >= 61 && days <= 90 ? outstandingAmount : 0;
            return amount;
        }
        private decimal secondQuater(long days, decimal outstandingAmount)
        {
            var amount = days >= 31 && days <= 60 ? outstandingAmount : 0;
            return amount;
        }

        private decimal firstQuater(long days, decimal outstandingAmount)
        {
            var amount = days <= 30 ? outstandingAmount : 0;
            return amount;
        }

        public String GetInvoiceReminderReport(GetRemainderReportData RemainderReport)
        {
            var ClientId = RemainderReport.customerId;
            int CId = 0;
            var message = new System.Net.Mail.MailMessage();
            string alert = "Mail sent successfully";
            foreach (var id in ClientId)
            {
                try
                {
                    CId = Convert.ToInt32(id);
                    var MailNotification = _tableWork.MailContactRepository.Get(x => x.IsDeleted == false && x.ClientId == CId && x.ProcessName == "Remainder Letter").ToList();
                    if (MailNotification.Count > 0)
                    {
                        var FromMail = _tableWork.MailContactRepository.Get(x => x.IsDeleted == false && x.ClientId == CId && x.ProcessName == "Remainder Letter").FirstOrDefault().FromMail;
                        message.From = new MailAddress(FromMail.ToString());
                        foreach (var Mail in MailNotification)
                        {
                            if (Mail.ToMail != "" && Mail.ToMail != null)
                            {
                                message.To.Add(new MailAddress(Mail.ToMail));

                            }
                            if (Mail.Ccmail != "" && Mail.Ccmail != null)
                            {
                                message.CC.Add(new MailAddress(Mail.Ccmail));
                            }
                        }
                        var CCmail = _configuration.GetSection("Invoice").GetValue<string>("InvoiceReminderLetterCCMail");
                        var ILurl = _configuration.GetSection("Invoice").GetValue<string>("InvoiceReminderLoginURL"); //
                        message.CC.Add(new MailAddress(CCmail));
                        message.Subject = "Invoice Reminder Letter";
                        message.Body = "Hi," +
                                       "</br></br>" +
                                       "We have generated the invoice and annexure for the orders that we did in the Previous Month.  Would appreciate your payment within 5-7 working day’s time. Many thanks for your help and understanding." +
                                       "<br/><br/>" +
                                       "We have provided the link to your VIS account login below." +
                                       "<br/><br/>" +
                                       "<a href=" + ILurl + ">Click to Login VIS</a>" +
                                       "<br/><br/>" +
                                       "If you have any questions about the invoice, please e-mail your queries to accounting@vleadservices.com or marcus@vleadservices.com. We would be happy to help you." +
                                       "<br/><br/>" +
                                       "<b>Note:</b> If possible, please ask your Accounting team to send us the bank payment transfer advice or PayPal details. This would be useful to track the payment at our end." +
                                       "<br/><br/>" +
                                       "Many thanks for your business." +
                                       "<br/><br/>" +
                                       "Warm regards and good luck, Accounting Team" +
                                       "<br/><br/>" +
                                       //"<img src=" + "~/Images/mailpic1.png" + " />" +
                                       "<h1><span><span style=" + "color:red" + ">V</span>lead</span></h1>" +
                                       //"<br/><br/>" +
                                       "<i>Referrals are the lifeblood of our business.  If you have a friend, business associate or organization that can benefit from our experience and expert service, please contact marcus@vleadservices.com.   Your referrals are the greatest compliment we can receive.  Please do not keep us a secret!!!</i>" +
                                       "<br/>" +
                                       "US Toll Free : 1800-957-1698" +
                                       "<br/>" +
                                       "accounting@vleadservices.com" +
                                       "<br/>" +
                                       "<a href=" + "http://www.vleadservices.com" + ">www.vleadservices.com</a>";
                        message.IsBodyHtml = true;
                        var host = _configuration.GetSection("Smtp").GetValue<string>("host");
                        var port = _configuration.GetSection("Smtp").GetValue<int>("port");
                        var userName = _configuration.GetSection("Smtp").GetValue<string>("userName");
                        var password = _configuration.GetSection("Smtp").GetValue<string>("password");
                        var userName1 = _configuration.GetSection("Smtp").GetValue<string>("userName1");
                        var password1 = _configuration.GetSection("Smtp").GetValue<string>("password1");
                        var Ssl = _configuration.GetSection("Smtp").GetValue<bool>("SSL");
                        var defaultCredential = _configuration.GetSection("Smtp").GetValue<bool>("defaultCredential");
                        using (var smtp = new SmtpClient(host,port))
                        {
                            smtp.UseDefaultCredentials = defaultCredential;
                            smtp.Credentials = new NetworkCredential(userName, password);
                            
                            if (message.From.ToString() == "accounting@vleadservices.com")
                            {
                                smtp.Credentials = new NetworkCredential(userName1, password1);

                            }
                            else
                            {
                                smtp.Credentials = new NetworkCredential(userName, password);
                            }
                            smtp.EnableSsl = Ssl;
                            smtp.Send(message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    alert = "There is a problems while sending mail! Try Again";
                }
            }
            return alert;
        }
        public Object UpdateJobLocationInfo(int Id)
        {
            var res = _tableWork.JobFilesLocationMasterRepository.GetSingle(x => x.Id == Id);
            var GetList = new
            {
                jobslist = res
            };
            return GetList;
        }
        public Object GetCustomerDepartmentInfo()
        {
            var res = _tableWork.CustomerDepartmentRepository.GetAllVal(x => x.Department, x => x.Customer).Where(x => x.IsActive == true && x.Customer.IsBulk == true).ToList();
            var GetList = new
            {
                jobslist = res
            };
            return GetList;
        }

        public Object AddJobLocationMaster(JobFilesLocationMaster jobFilesLocationMaster)
        {
            var JobFilesLocationMaster = new JobFilesLocationMaster();

            bool status = false;
            var result = new
            {
                Success = false,
                Message = ""
            };
            try
            {
                var IsExistsJobFilesLocation = _tableWork.JobFilesLocationMasterRepository.Get(x => x.ClientId == jobFilesLocationMaster.ClientId && x.DepartmentId == jobFilesLocationMaster.DepartmentId && x.ProcessName == jobFilesLocationMaster.ProcessName).ToList();
                if (jobFilesLocationMaster != null && IsExistsJobFilesLocation.Count == 0)
                {
                    JobFilesLocationMaster.ClientId = jobFilesLocationMaster.ClientId;
                    JobFilesLocationMaster.DepartmentId = jobFilesLocationMaster.DepartmentId;
                    JobFilesLocationMaster.FtpfilePath = jobFilesLocationMaster.FtpfilePath;
                    JobFilesLocationMaster.ProcessName = jobFilesLocationMaster.ProcessName;
                    if (JobFilesLocationMaster.ProcessName == "Production")
                    {
                        JobFilesLocationMaster.ProcessId = 3;
                    }
                    else
                    {
                        JobFilesLocationMaster.ProcessId = 5;
                    }

                    JobFilesLocationMaster.CreatedBy = jobFilesLocationMaster.UpdatedBy;
                    JobFilesLocationMaster.CreatedUtc= DateTime.UtcNow;
                    JobFilesLocationMaster.IsActive = true;

                    //JobFilesLocationMaster.UpdatedBy = jobFilesLocationMaster.ClientId;
                    //JobFilesLocationMaster.UpdatedUTC = DateTime.UtcNow;
                    //JobFilesLocationMaster.IsActive = true;
                    //}
                    _tableWork.JobFilesLocationMasterRepository.Insert(JobFilesLocationMaster);
                    //}

                    long dbStatus = _tableWork.SaveChanges();
                    status = dbStatus > 0;
                    status = true;
                    result = new
                    {
                        Success = true,
                        Message = "Customer job File path added successfully....!"
                    };
                }
                else
                {
                    result = new
                    {
                        Success = true,
                        Message = "Customer job File path already exists....!"
                    };
                }
            }

            catch (Exception e)
            {
                throw;
            }
            return result;
        }
        public bool AddCustomerDepartmentPath(CustomerDepartment customerdept)
        {
            var result = false;
            var newCustomer1 = new CustomerDepartment();
            var checkCustomer = _tableWork.CustomerDepartmentRepository.GetSingle(x => x.CustomerId == customerdept.CustomerId && x.DepartmentId == customerdept.DepartmentId && x.IsActive == true);
            if (checkCustomer == null)
            {
                long Id = 0L;
                newCustomer1.DepartmentId = customerdept.DepartmentId;
                newCustomer1.CustomerId = customerdept.CustomerId;

                newCustomer1.CreatedUtc = DateTime.UtcNow;
                newCustomer1.CreatedBy = customerdept.CreatedBy;
                newCustomer1.IsActive = true;
                newCustomer1.FtpuserName= customerdept.FtpuserName;
                newCustomer1.Ftppassword = customerdept.Ftppassword;
                newCustomer1.InFtppath= customerdept.InFtppath;
                newCustomer1.OutFtppath = customerdept.OutFtppath;
                newCustomer1.Host = customerdept.Host;
                newCustomer1.Port = customerdept.Port;
                newCustomer1.InputType = customerdept.InputType;
                _tableWork.CustomerDepartmentRepository.Insert(newCustomer1);
                _tableWork.SaveChanges();
                result = true;
                return result;
            }
            else
            {
                return result;
            }
        }
        public bool UpdateCustomerDepartmentPath(CustomerDepartment customerdeptPath)
        {
            bool status = false;
            try
            {
              
                    var existingCustomerContact = tow.CustomerDepartmentRepository.GetSingle(x => x.Id == customerdeptPath.Id);
                    existingCustomerContact.CustomerId = customerdeptPath.CustomerId;
                    existingCustomerContact.DepartmentId = customerdeptPath.DepartmentId;
                    existingCustomerContact.Host = customerdeptPath.Host;
                    existingCustomerContact.Port = customerdeptPath.Port;
                    existingCustomerContact.InputType = customerdeptPath.InputType;
                    existingCustomerContact.InFtppath = customerdeptPath.InFtppath;
                    existingCustomerContact.OutFtppath = customerdeptPath.OutFtppath;
                    existingCustomerContact.IsActive = true;
                    existingCustomerContact.FtpuserName = customerdeptPath.FtpuserName;
                    existingCustomerContact.Ftppassword = customerdeptPath.Ftppassword;
                    tow.CustomerDepartmentRepository.Update(existingCustomerContact);
                    long dbstatus = tow.SaveChanges();
                    status = dbstatus > 0;
               
            }
            catch (Exception e)
            {
                throw;
            }
            return status;
        }
        public Object UpdateCustomerDepartmentInfo(int Id)
        {
            var res = _tableWork.CustomerDepartmentRepository.GetSingle(x => x.Id == Id);
            var GetList = new
            {
                jobslist = res
            };
            return GetList;
        }

        // Bind Approved customers
        public object GetAllCustomers(int EmpId)
        {
            var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@Id", EmpId)
                    };
            var ApprovedList = _procedureWork.ExecStoredProcedure<GetCustomersApprovedBySales_Result>("GetCustomersApprovedBySales @Id", sqlpara).ToList();
            return ApprovedList;

        }

        // Bind UnApproved Customers

        public object GetCustomerSalesUnapproval(int EmpId)
        {
           var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@Id", EmpId)
                    };
            var UnAppList = _procedureWork.ExecStoredProcedure<GetCustomersUnApprovedBySales_Result>("GetCustomersUnApprovedBySales @Id", sqlpara).ToList();
            return UnAppList;

        }

        // Bind Approved and UnApproved Customers Count

        public Object GetCustomerUnapprovalcount(int EmpId)
        {
            var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@Id", EmpId)
                    };
            var UnAppList = _procedureWork.ExecStoredProcedure<GetCustomersAppUnAppbySalesCount_Result>("GetCustomersAppUnAppbySalesCount @Id", sqlpara).FirstOrDefault();
            return UnAppList;

        }


        //-- 1st section details
        public object GetAppCustomerSalesId(int Id)
        {
            var sqlpara = new SqlParameter[]
                  {
                        new SqlParameter("@Id", Id)
                  };
            var AppCusReg = _procedureWork.ExecStoredProcedure<GetCustomerRegDetails_Result>("GetCustomerRegDetails @Id", sqlpara).FirstOrDefault();
            return AppCusReg;
        }

        //-- 2nd Section Details
        public Object GetAppCustomerContactDetails(int Id)
        {
            var cust = _tableWork.CustomerRepository.Get(x => x.Id == Id && x.IsDeleted == false).FirstOrDefault();
            var departments = _tableWork.CustomerDepartmentRepository.Get(x => x.CustomerId == Id).Select(x => x.DepartmentId).ToArray();
            if (departments == null)
            {
                var customer = _tableWork.CustomerRepository.Get(x => x.Id == Id).FirstOrDefault();
                return customer;
            }
            else
            {
                var sqlpara = new SqlParameter[]
                   {
                        new SqlParameter("ClientId", Id)
                   };
                var customer = _procedureWork.ExecStoredProcedure<GetCustomerDetails_Result>("GetCustomerDetails @ClientId", sqlpara).FirstOrDefault();

                var customeDetails = new
                {
                    Id = customer.Id,
                    ShortName = customer.ShortName,
                    CustomerClassificationId = customer.CustomerClassificationId,
                    CustomerJobType = customer.CustomerJobType,
                    CreditDays = customer.CreditDays,
                    CreditLimit = customer.CreditLimit,
                    SelectedDepartments = departments,
                    ReportTimeZone = customer.ReportTimeZone,
                    TimeZone = customer.Timezone,
                    Country = customer.Country,
                    State = customer.State,
                    City = customer.City,
                    PrivilegedClient = customer.PrivilegedClient,
                    PaymentMode = customer.PaymentMode,
                    InputType = customer.InputType,
                    OutputType = customer.OutputType,
                    BillingCycleType = customer.BillingCycleType,
                    Approvedby = customer.Approvedby,
                    Approved = customer.IsApproved,
                    IsBulk = customer.IsBulk,
                    Checklist = customer.Checklist,//----------------------------------------------------------------final CL---------------------------------------------------------
                    IsRush = customer.IsRush,
                    EmpName = customer.EmployeeName,
                    ApprovedDate = customer.ApprovedDate,
                    BunchMail = customer.BunchMail,
                    IsManualUpload = customer.IsManualUpload,
                    TrialStartDate = customer.TrialStartDate,
                    LiveStartDate = customer.LiveStartDate,
                    ModeofSales = customer.ModeofSales,
                    CurrencyMode = customer.CurrencyMode
                };
                return customeDetails;
            }
        }


        public Employee GetEmployeeIdByName(int Id)
        {
            return _tableWork.EmployeeRepository.Get(x => x.EmployeeId == Id && x.IsDeleted == false).Select(row => row).FirstOrDefault();
        }

        public Object GetClassificationList()
        {
            return _tableWork.CustomerClassificationRepository.Get(x => x.IsDeleted == false).OrderBy(x => x.Description).ToList();
        }

        public IQueryable<Department> GetDepartments()
        {
            return _tableWork.DepartmentRepository.Get(x => x.Description == "Artwork" || x.Description == "Digitizing");
        }

        public Object GetCountriesDropDown()
        {
            var Ctry = _tableWork.CountryRepository.Get(x => x.IsDeleted == false).AsQueryable();
            var DetailList = new
            {
                CountryDetails = Ctry,
            };
            return DetailList;
        }

        public IEnumerable<State> GetStatesDropDown(int CountryId)
        {

            return _tableWork.StateRepository.Get(x => x.CountryId == CountryId && x.IsDeleted == false);
        }

        public IEnumerable<City1> GetCitiesDropDown(int StateId)
        {
            return _tableWork.CityRepository.Get(x => x.StateId == StateId && x.IsDeleted == false);
        }


        public IEnumerable<City1> GetTimeZonebylocation(int CityId)
        {
            return _tableWork.CityRepository.Get(x => x.Id == CityId && x.IsDeleted == false);
        }


        public Customer AddCustomerDetails(AddCustomer customer)
        {

            var user = new User();
            try
            {
                var checkCustomer = _tableWork.CustomerRepository.Get(x => x.ShortName == customer.ShortName).FirstOrDefault();

                var Customername = _tableWork.CustomerRepository.Get(x => x.Id == customer.Id).FirstOrDefault();
                if (checkCustomer == null)
                {
                    var validcustomer = _tableWork.CustomerContactRepository.Get(x => x.ContactName == customer.UserName && x.Email == customer.EmailID).FirstOrDefault();
                    var updatecustomer = _tableWork.CustomerRepository.Get(x => x.Id == validcustomer.CustomerId).FirstOrDefault();
                    if (validcustomer != null)
                    {
                        updatecustomer.Name = customer.UserName;
                        updatecustomer.ShortName = customer.ShortName;
                        updatecustomer.CustomerClassificationId = customer.CustomerClassificationId;
                        updatecustomer.CreditDays = customer.CreditDays;
                        updatecustomer.CreditLimit = customer.CreditLimit;
                        updatecustomer.IsDeleted = false;
                        updatecustomer.CompanyId = 1;
                        if (customer.IsApproved == true)
                        {
                            updatecustomer.ApprovedBy = customer.EmployeeId;
                            updatecustomer.ApprovedDate = DateTime.UtcNow;
                            updatecustomer.IsApproved = true;
                        }

                        updatecustomer.CustomerJobType = customer.CustomerJobType;
                        updatecustomer.Country = customer.Country;
                        updatecustomer.State = customer.State;
                        updatecustomer.City = customer.City;
                        updatecustomer.InputType = customer.InputType;
                        updatecustomer.OutputType = customer.OutputType;
                        updatecustomer.PrivilegedClient = customer.PrivilegedClient;
                        updatecustomer.PaymentMode = customer.PaymentMode;
                        updatecustomer.BillingCycleType = customer.BillingCycleType;
                        updatecustomer.ReportTimeZone = customer.ReportTimeZone;
                        updatecustomer.CreatedUtc = DateTime.UtcNow;
                        updatecustomer.CreatedBy = 1;
                        updatecustomer.IsAdmin = true;
                        updatecustomer.RpttimeZoneDifference = customer.RPTTimeZoneDifference;

                        //_tableWork.Entry(updatecustomer).State = EntityState.Modified;                       
                        _tableWork.CustomerRepository.Update(updatecustomer);

                        //_tableWork.CustomerDepartmentRepository.RemoveRange(_tableWork.CustomerDepartmentRepository.Get(x => x.CustomerId == updatecustomer.Id));
                        _tableWork.CustomerDepartmentRepository.Delete(_tableWork.CustomerDepartmentRepository.Get(x => x.CustomerId == updatecustomer.Id));

                        foreach (var department in customer.SelectedDepartments)
                        {
                            var customerDepartments = new CustomerDepartment();
                            customerDepartments.CustomerId = updatecustomer.Id;
                            customerDepartments.DepartmentId = department.Id;
                            customerDepartments.IsActive = true;
                            _tableWork.CustomerDepartmentRepository.Insert(customerDepartments);
                        }
                        _tableWork.SaveChanges();
                        // SendMailtoApproval(updatecustomer);
                    }
                    var Newcustomer = _tableWork.CustomerRepository.Get(x => x.Id == customer.Id).FirstOrDefault();
                    return updatecustomer;
                }
                else
                {
                    return Customername;
                }
            }
            catch (Exception ex)
            {
               
                throw ex;
            }

        }

        public Customer EditCustomerDetails(AddCustomer customer)
        {
            var customerDepartments = new CustomerDepartment();
            var user = new User();
            try
            {
                Customer cus = new Customer();
                var checkCustomer = _tableWork.CustomerRepository.Get(x => x.Id == customer.Id && x.IsDeleted == false).FirstOrDefault();
                var Customername = _tableWork.CustomerRepository.Get(x => x.Id == customer.Id).FirstOrDefault();
                var ShortNameExists = _tableWork.CustomerRepository.Exist(check => check.ShortName == customer.ShortName && check.IsDeleted == false && check.Id != customer.Id);
                if (ShortNameExists == false)
                {
                    checkCustomer.Name = Customername.Name;
                    checkCustomer.ShortName = customer.ShortName;
                    checkCustomer.CustomerClassificationId = customer.CustomerClassificationId;
                    checkCustomer.CreditDays = customer.CreditDays;
                    checkCustomer.CreditLimit = customer.CreditLimit;
                    checkCustomer.IsDeleted = false;
                    checkCustomer.CompanyId = 1;
                    checkCustomer.IsBulk = customer.IsBulk;
                    checkCustomer.Checklist = customer.Checklist;//--------------------------------------------final CL----------------------------------------------
                    checkCustomer.IsRush = customer.IsRush;
                    checkCustomer.BunchMail = customer.BunchMail;
                    checkCustomer.IsManualUpload = customer.IsManualUpload;
                    checkCustomer.ModeofSales = customer.ModeofSales;
                    checkCustomer.CurrencyMode = customer.CurrencyMode;
                    checkCustomer.IsEstimatedTime = customer.IsEstimatedTime;

                    if (checkCustomer.BunchMail == true)
                    {
                        var updateclientorder = _unitWork.ClientOrder.Where(x => x.ClientId == checkCustomer.Id && x.IsDeleted == false).ToList();
                        updateclientorder.ForEach(x => x.IsMailSent = true);
                        _unitWork.SaveChanges();
                    }
                    if (customer.IsApproved == true)
                    {
                        checkCustomer.ApprovedBy = customer.EmployeeId;
                        checkCustomer.ApprovedDate = DateTime.UtcNow;
                        checkCustomer.IsApproved = true;
                    }
                    if (customer.TimeZone == null)
                    {
                        checkCustomer.Timezone = customer.DropdownTimeZone;
                    }
                    else
                    {
                        checkCustomer.Timezone = customer.TimeZone;
                    }
                    string dbcustype = checkCustomer.CustomerJobType;
                    if (checkCustomer.IsApproved == true)
                    {
                        if (customer.CustomerJobType == "Trial")
                        {
                            if ((dbcustype == "Trial" || dbcustype == "Live") && customer.CustomerJobType == "Trial" && checkCustomer.TrialStartDate != null)
                            {

                            }
                            else
                            {
                                if (dbcustype == "Live" && customer.CustomerJobType == "Trial")
                                {

                                }
                                else
                                {
                                    checkCustomer.TrialStartDate = DateTime.UtcNow;
                                    checkCustomer.CustomerJobType = customer.CustomerJobType;
                                }
                            }
                        }
                        else if (customer.CustomerJobType == "Live")
                        {
                            if (dbcustype == "Live" && (customer.CustomerJobType == "Live" || customer.CustomerJobType == "Trial") && checkCustomer.LiveStartDate != null)
                            {

                            }
                            else
                            {
                                checkCustomer.LiveStartDate = DateTime.UtcNow;
                                checkCustomer.CustomerJobType = customer.CustomerJobType;
                            }
                        }
                    }
                    else
                    {
                        checkCustomer.CustomerJobType = customer.CustomerJobType;
                    }
                    checkCustomer.IsBlacklisted = customer.IsBlacklisted;
                    checkCustomer.BlacklistedReasons = customer.BlacklistedReasons;
                    checkCustomer.Country = customer.Country;
                    checkCustomer.State = customer.State;
                    checkCustomer.City = customer.City;
                    checkCustomer.BillingCycleType = customer.BillingCycleType;
                    checkCustomer.InputType = customer.InputType;
                    checkCustomer.OutputType = customer.OutputType;
                    checkCustomer.PrivilegedClient = customer.PrivilegedClient;
                    checkCustomer.PaymentMode = customer.PaymentMode;
                    checkCustomer.ReportTimeZone = customer.ReportTimeZone;
                    checkCustomer.UpdatedUtc = DateTime.UtcNow;
                    checkCustomer.UpdatedBy = 1;
                    checkCustomer.IsAdmin = true;
                    _tableWork.CustomerRepository.Update(checkCustomer);
                    //long dbstatus = _tableWork.SaveChanges();
                    foreach (var department in customer.SelectedDepartments)
                    {
                        var CusDepRep = _tableWork.CustomerDepartmentRepository.Get(x => x.CustomerId == checkCustomer.Id && x.DepartmentId != department.Id).FirstOrDefault();
                        if (CusDepRep != null)
                        {
                            _tableWork.CustomerDepartmentRepository.Delete(CusDepRep);
                            _tableWork.SaveChanges();
                        }

                    }

                    foreach (var department in customer.SelectedDepartments)
                    {
                        if (!_tableWork.CustomerDepartmentRepository.Get(x => x.CustomerId == checkCustomer.Id && x.DepartmentId == department.Id).Any())
                        {
                            _tableWork.CustomerDepartmentRepository.Insert(new CustomerDepartment
                            {
                                CustomerId = checkCustomer.Id,
                                DepartmentId = department.Id,
                                IsActive = true
                            });
                        }
                    }

                    _tableWork.SaveChanges();
                    return checkCustomer;
                }
                else
                {
                    Customername.ShortName = "True";
                    return Customername;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Object GetAllUnapprovaljobs(int EmpId)
        {
            var MappedEmployees = _tableWork.EmployeeHierarchyRepository
              .Get(x => x.SubEmpId == EmpId || x.EmployeeId == EmpId)
              .Select(x => new { EmpId = x.EmployeeId }).Distinct();

            //if hierarchy does not  exists for employee 

            bool EmphierExists = _tableWork.EmployeeHierarchyRepository
              .Exist(x => x.SubEmpId == EmpId || x.EmployeeId == EmpId);
            if (EmphierExists == false)
            {
                MappedEmployees = _tableWork.EmployeeRepository.Get(x => x.EmployeeId == EmpId).Select(x => new { EmpId = x.EmployeeId });
            }
            //if Role exists for employee 

            bool RolesExists = _tableWork.EmployeeVsRoleRepository.Exist(x => x.EmployeeId == EmpId && x.RoleDescription == "salesadmin" && x.IsDeleted == false);
            if (RolesExists == true)
            {
                MappedEmployees = _tableWork.EmployeeRepository.Get(x => x.DepartmentId == 9 && x.IsDeleted == false).Select(x => new { EmpId = x.EmployeeId });
            }


            var appjobs = (from cus in _tableWork.CustomerRepository.Get()
                           join cse in _tableWork.CustomerVsSalesEmployeeRepository.Get() on cus.Id equals cse.CustomerId
                           // join EMH in _db.EmployeeHierarchies on cse.EmployeeId equals EMH.EmployeeId
                           join MapEmp in MappedEmployees on cse.EmployeeId equals MapEmp.EmpId
                           join co in _tableWork.ClientOrderRepository.Get() on cus.Id equals co.ClientId
                           join js in _tableWork.JobStatusRepository.Get() on co.JobStatus equals js.Id
                           join cc in _tableWork.CustomerContactRepository.Get() on cus.Id equals cc.CustomerId
                           join cr in _tableWork.CustomerRegistrationRepository.Get() on cc.Email equals cr.EmailId
                           where cus.ApprovedBy == null && cse.IsActive == true
                           orderby co.OrderId descending
                           select new
                           {
                               co.OrderId,
                               co.FileName,
                               co.Pono,
                               co.Instruction,
                               co.SalesPersonName,
                               js.JobStatusDescription,
                               cr.CompanyName,
                           }).ToList();

            return appjobs;
        }


        public IEnumerable<CustomerContact> GetAllCustomerContacts(int cusomerId)
        {
            //return _db.CustomerContacts.Where(x => x.CustomerId == cusomerId).OrderBy(x => x.ContactName);
            return _tableWork.CustomerContactRepository.Get(x => x.CustomerId == cusomerId && x.IsDeleted == false).OrderBy(x => x.ContactName).AsQueryable();
        }

        public bool RemoveCustomer(int customerId)
        {
            bool status = false;
            var checkCommunication = _tableWork.CustomerCommunicationRepository.Exist(x => x.CustomerId == customerId);
            var checkContact = _tableWork.CustomerContactRepository.Exist(x => x.CustomerId == customerId);
            var UserEntry = _tableWork.UserRepository.Exist(x => x.CustomerId == customerId);
            if (checkCommunication == true)
            {
                var deleteCustomerCommunication = _tableWork.CustomerCommunicationRepository.Get(x => x.CustomerId == customerId).FirstOrDefault();
                //foreach (var item in deleteCustomerCommunication)
                //{
                //    item.IsDeleted = true;
                //}
                _tableWork.CustomerCommunicationRepository.Delete(deleteCustomerCommunication);
                long CommunicationDbstatus = _tableWork.SaveChanges();
                if (checkContact == true)
                {
                    var deleteContact = _tableWork.CustomerContactRepository.Get(x => x.CustomerId == customerId);
                    // _db.CustomerContacts.Remove(deleteContact);
                    foreach (var item in deleteContact)
                    {
                        item.IsDeleted = true;
                    }
                    //long ContactDbstatus = _db.SaveChanges();

                    var deleteCustomer = _tableWork.CustomerRepository.Get(x => x.Id == customerId).First();
                    deleteCustomer.IsDeleted = true;
                    //_db.Entry(deleteCustomer).State = EntityState.Modified;
                    _tableWork.CustomerRepository.Update(deleteCustomer);
                    //long dbstatus = _db.SaveChanges();

                    var deleteUserDetails = _tableWork.UserRepository.Get(x => x.Id == customerId).First();
                    deleteUserDetails.IsDeleted = true;
                    // _db.Entry(deleteUserDetails).State = EntityState.Modified;
                    _tableWork.UserRepository.Update(deleteUserDetails);
                    long UserdbStatus = _tableWork.SaveChanges();

                    status = UserdbStatus > 0;
                }
                else
                {
                    var deleteCustomer = _tableWork.CustomerRepository.Get(x => x.Id == customerId).First();
                    deleteCustomer.IsDeleted = true;
                    _tableWork.CustomerRepository.Update(deleteCustomer);
                    //_db.Entry(deleteCustomer).State = EntityState.Modified;
                    //long dbstatus = _db.SaveChanges();

                    var deleteUserDetails = _tableWork.UserRepository.Get(x => x.Id == customerId).First();
                    deleteUserDetails.IsDeleted = true;
                    //_db.Entry(deleteUserDetails).State = EntityState.Modified;
                    _tableWork.UserRepository.Update(deleteUserDetails);
                    long UserdbStatus = _tableWork.SaveChanges();

                    status = UserdbStatus > 0;
                }
            }
            else
            {
                if (checkContact == true)
                {
                    var deleteContact = _tableWork.CustomerContactRepository.Get(x => x.CustomerId == customerId);
                    foreach (var item in deleteContact)
                    {
                        item.IsDeleted = true;
                    }
                    //_db.CustomerContacts.Remove(deleteContact);
                    // long ContactDbstatus = _db.SaveChanges();

                    var deleteCustomer = _tableWork.CustomerRepository.Get(x => x.Id == customerId).FirstOrDefault();
                    deleteCustomer.IsDeleted = true;
                    //_db.Entry(deleteCustomer).State = EntityState.Modified;
                    _tableWork.CustomerRepository.Update(deleteCustomer);
                    //long dbstatus = _db.SaveChanges();

                    var deleteUserDetails = _tableWork.UserRepository.Get(x => x.CustomerId == customerId).FirstOrDefault();
                    deleteUserDetails.IsDeleted = true;
                    _tableWork.UserRepository.Update(deleteUserDetails);
                    // _db.Entry(deleteUserDetails).State = EntityState.Modified;
                    long UserdbStatus = _tableWork.SaveChanges();

                    status = UserdbStatus > 0;
                }
                else
                {
                    var deleteCustomer = _tableWork.CustomerRepository.Get(x => x.Id == customerId).FirstOrDefault();
                    deleteCustomer.IsDeleted = true;
                    //_db.Entry(deleteCustomer).State = EntityState.Modified;
                    _tableWork.CustomerRepository.Update(deleteCustomer);
                    // long dbstatus = _db.SaveChanges();

                    var deleteUserDetails = _tableWork.UserRepository.Get(x => x.Id == customerId).FirstOrDefault();
                    deleteUserDetails.IsDeleted = true;
                    //_db.Entry(deleteUserDetails).State = EntityState.Modified;
                    _tableWork.UserRepository.Update(deleteUserDetails);
                    long UserdbStatus = _tableWork.SaveChanges();

                    status = UserdbStatus > 0;
                }
            }
            return status;
        }

        public IEnumerable<ViewCustomerDetail> GetAllCustomers()
        {
            var result = _viewWork.ViewCustomerDetailsRepository.Get(x => x.IsDeleted == false && x.ApprovedBy != null).OrderByDescending(x => x.Id).AsQueryable();
            return result;
        }

        // CustomerCredentials Starts
        public Object GetAllCusContactCredentials(int customerId)
        {
            var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@customerId", customerId)
                    };
            var GetCustomerCredentials = _procedureWork.ExecStoredProcedure<GetCustomerContactCredentials_Result>("GetCustomerContactCredentials @customerId", sqlpara).ToList();

            //  var GetCustomerCredentials = _tableWork.GetCustomerContactCredentials(customerId).ToList();
            return GetCustomerCredentials;
        }
        public Object GetCustomerBySalesId(int EmpId)
        {
            var CustomerList = (from j in _tableWork.CustomerVsSalesEmployeeRepository.Get(j => j.EmployeeId == EmpId && j.IsActive == true && j.IsDeleted == false)
                                join p in _tableWork.CustomerRepository.Get(k => k.IsDeleted == false) on j.CustomerId equals p.Id
                                select new
                                {
                                    p.Name,
                                    p.Id,
                                    p.ShortName,
                                }).ToList();
            return CustomerList;
        }
        // CustomerCredentials Ends
        public bool UpdateJobFilesLocation(JobFilesLocationMaster JobFilesLocation)
        {
            bool status = false;
            try
            {
                var IsExistsJobFilesLocation = _unitWork.JobFilesLocationMaster.Where(x => x.ClientId == JobFilesLocation.ClientId && x.DepartmentId == JobFilesLocation.DepartmentId && x.ProcessName == JobFilesLocation.ProcessName).ToList();
                if (IsExistsJobFilesLocation.Count == 1)
                {
                    var existingCustomerContact = _unitWork.JobFilesLocationMaster.FirstOrDefault(x => x.Id == JobFilesLocation.Id);
                    existingCustomerContact.ClientId = JobFilesLocation.ClientId;
                    existingCustomerContact.DepartmentId = JobFilesLocation.DepartmentId;
                    existingCustomerContact.ProcessId = JobFilesLocation.ProcessId;
                    if (existingCustomerContact.ProcessId == 5)
                    {
                        existingCustomerContact.ProcessName = "Quality";
                    }
                    else
                    {
                        existingCustomerContact.ProcessName = "Production";
                    }
                    existingCustomerContact.FtpfilePath = JobFilesLocation.FtpfilePath;
                    _unitWork.JobFilesLocationMaster.Update(existingCustomerContact);
                    long dbstatus = _unitWork.Save();
                    status = dbstatus > 0;
                }
                else
                {
                    status = false;
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return status;
        }
    }
}

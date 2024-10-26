using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class OutStandingService
    {
        private readonly TableWork _tableWork;
        private readonly ProcedureWork _procedureWork;

 

        public OutStandingService(TableWork tableWork,ProcedureWork procedureWork)
        {
            _tableWork = tableWork;
            _procedureWork = procedureWork;
           
        }
        public List<Customer> GetCustomerName()
        {
            var customerDetails = _tableWork.CustomerRepository.Get(row => row.IsDeleted == false && row.IsBlacklisted == false)
                .OrderBy(row => row.Name).ToList();

            return customerDetails;
        }
        ////
        public object GetVleadSalesPersonName()
        {
            var VleadSPDetails = _tableWork.EmployeeRepository.Get(row => row.IsDeleted == false && row.DepartmentId == 9)
                .OrderBy(row => row.EmployeeName).ToList();

            return VleadSPDetails;
        }
        ////
        // summary
        public List<OutStandingViewModel> GetAllInvoices(string reportAsOn, int EmpId)
        {
            var dateSplit = reportAsOn.Split('T');

            var date = Convert.ToDateTime(dateSplit[0]);
            var recDate = date.AddDays(1);
            var outStandingDetailsLst = new List<OutStandingViewModel>();
            List<OutStandingViewModel> reports = new List<OutStandingViewModel>();
            try
            {

                var cancelledInvoices = (from A in _tableWork.InvoiceTranRepository.Get().Where(x => x.InvoiceNo != null)
                                         join B in _tableWork.InvoiceMasterRepository.Get() on A.Imid equals B.Id
                                         select B);


                var cancelledInvoices2 = (from invMaster in _tableWork.InvoiceMasterRepository.Get()
                                          join tran in _tableWork.InvoiceTranRepository.Get().Where(x => x.InvoiceNo != null) on invMaster.InvoiceNo equals tran.InvoiceNo
                                          select invMaster);

                var AllCancelledInvoices = cancelledInvoices.Union(cancelledInvoices2);



                var outStandingReport = (from i in _tableWork.InvoiceMasterRepository.Get().Except(AllCancelledInvoices)
                                             //from i in _db.InvoiceMasters
                                         join c in _tableWork.CustomerRepository.Get() on i.CustomerId equals c.Id
                                         //SP
                                         join cvse in _tableWork.CustomerVsSalesEmployeeRepository.Get() on c.Id equals cvse.CustomerId into tempT
                                         from jcvse in tempT.DefaultIfEmpty() //SP
                                         join recA in _tableWork.ReceivableAdjustmentRepository.Get() on i.InvoiceNo equals recA.InvoiceNo into jo_recA
                                         from jrecA in jo_recA.DefaultIfEmpty()
                                         join rec in _tableWork.ReceivableRepository.Get() on jrecA.ReceivableId equals rec.Id into jo_rec
                                         from jrec in jo_rec.DefaultIfEmpty()
                                         select new OutStandingViewModel
                                         {
                                             CustomerId = i.CustomerId,
                                             InvoiceNo = i.InvoiceNo,
                                             InvoiceDate = i.InvoiceDate.Value.AddHours(-5),
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
                                         }).Where(i => i.IsOutstanding == true && i.IsDeleted == false &&
                                             i.VleadSalesPersonId == (EmpId == 0 ? i.VleadSalesPersonId : EmpId) && i.VleadSalesPersonIsActive == true && //VSP
                                             (i.InvoiceDate.Date <= date)).AsQueryable();

                if (outStandingReport != null)
                {

                    List<OutStandingViewModel> OutStandingDetailsList = new List<OutStandingViewModel>();

                    foreach (var item in outStandingReport)
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
                                AmountCollected = 0,
                                OutstandingAmount = (decimal)(item.TotalInvoiceValue), //OutstandingAmount = (decimal)(item.InvoiceValue),
                            };
                            OutStandingDetailsList.Add(outstandingDetail);
                        }

                    }

                    foreach (var item in outStandingReport)
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
                                AmountCollected = item.AmountCollected,
                                OutstandingAmount = (decimal)(item.TotalInvoiceValue - item.AmountCollected),// OutstandingAmount = (decimal)(item.InvoiceValue - item.AmountCollected),
                            };
                            OutStandingDetailsList.Add(outstandingDetail);
                        }
                        else
                        {
                            if (item.CollectionDate <= recDate) // Date
                            {

                                var i = OutStandingDetailsList.FindIndex(x => x.InvoiceNo == item.InvoiceNo);
                                OutStandingDetailsList[i].AmountCollected = OutStandingDetailsList[i].AmountCollected + item.AmountCollected;
                                //OutStandingDetailsList[i].OutstandingAmount = (decimal)(OutStandingDetailsList[i].InvoiceValue - OutStandingDetailsList[i].AmountCollected);
                                OutStandingDetailsList[i].OutstandingAmount = (decimal)(OutStandingDetailsList[i].TotalInvoiceValue - OutStandingDetailsList[i].AmountCollected);
                            }
                        }

                    }

                    outStandingDetailsLst = OutStandingDetailsList;
                    var myRef = outStandingDetailsLst.GroupBy(i => new { i.CustomerId }).Select(x => x.Select(y => new { CustomerName = y.CustomerName, CustomerId = y.CustomerId, ShortName = y.ShortName, OutstandingAmount = x.Sum(z => (z.OutstandingAmount)) }).FirstOrDefault()).ToList(); // CustomerName - shortname 

                    var test = myRef;

                    foreach (var item in test)
                    {
                        var res = new OutStandingViewModel
                        {
                            CustomerName = item.CustomerName,
                            ShortName = item.ShortName,
                            CustomerwithShortName = item.CustomerName + " - " + item.ShortName,
                            OutstandingAmount = Convert.ToDecimal(item.OutstandingAmount),

                        };

                        reports.Add(res);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reports;
        }

        // Ageing & Statement
        //public List<OutStandingViewModel> OSDetails(SelectedCustomers selectedCustomers)
        public List<OutStandingViewModel> OSDetails(string reportAsOn, IEnumerable<OSCustomerIds> Ids, int EmpId)
        {
            var outstandingAmount = 0L;
            IQueryable<OutStandingViewModel> outstandingDetailsBasedOnCustomer = null;
            List<OutStandingViewModel> outstandingDetailsBasedOnCustomer123 = new List<OutStandingViewModel>();
            // var dateSplit = selectedCustomers.ReportAsOn.Split('T');   
            var dateSplit = reportAsOn.Split('T');
            var date = Convert.ToDateTime(dateSplit[0]);
            var Id = 1;
            var recDate = date.AddDays(1);
            var outStandingDetailsLst = new List<OutStandingViewModel>();
            try
            {
                //if (selectedCustomers.Ids.Any())
                if (Ids.Any())
                {
                    foreach (var customerId in Ids)//selectedCustomers.
                    {

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
                                                                                                  //join recA in _tableWork.ReceivableAdjustmentRepository.Get() on i.InvoiceNo equals recA.InvoiceNo into jo_recA
                                                             join recA in _tableWork.ReceivableAdjustmentRepository.Get() on i.Id equals recA.InvoiceId into jo_recA
                                                             from jrecA in jo_recA.DefaultIfEmpty()
                                                             join rec in _tableWork.ReceivableRepository.Get() on jrecA.ReceivableId equals rec.Id into jo_rec
                                                             from jrec in jo_rec.DefaultIfEmpty()
                                                             select new OutStandingViewModel
                                                             {
                                                                 Id = Id + 1,
                                                                 CustomerId = i.CustomerId,
                                                                 InvoiceNo = i.InvoiceNo,
                                                                 InvoiceDate = i.InvoiceDate.Value.AddHours(-5) ,
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
                                                                 //}).Where(i => i.CustomerId == customerId.Id && i.IsOutstanding == true && i.IsDeleted == false && i.InvoiceDate <= date);

                                                             }).Where(i => i.CustomerId == customerId.Id &&
                                                                 i.VleadSalesPersonId == (EmpId == 0 ? i.VleadSalesPersonId : EmpId) && i.VleadSalesPersonIsActive == true && //VSP
                                                                                                                                                                              //customerId
                                                                  i.IsOutstanding == true && i.IsDeleted == false &&
                                                                  i.InvoiceDate.Date <= recDate).AsQueryable();  //  date                  
                        outstandingDetailsBasedOnCustomer123.AddRange(outstandingDetailsBasedOnCustomer);

                    }
                }
                else
                {
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
                                                                                              //join recA in _tableWork.ReceivableAdjustmentRepository.Get() on i.InvoiceNo equals recA.InvoiceNo into jo_recA
                                                         join recA in _tableWork.ReceivableAdjustmentRepository.Get() on i.Id equals recA.InvoiceId into jo_recA
                                                         from jrecA in jo_recA.DefaultIfEmpty()
                                                         join rec in _tableWork.ReceivableRepository.Get() on jrecA.ReceivableId equals rec.Id into jo_rec
                                                         from jrec in jo_rec.DefaultIfEmpty()
                                                         select new OutStandingViewModel
                                                         {
                                                             Id = Id + 1,
                                                             CustomerId = i.CustomerId,
                                                             InvoiceNo = i.InvoiceNo,
                                                             InvoiceDate = i.InvoiceDate.Value.AddHours(-5),
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
                                                         }).Where(i => i.IsOutstanding == true && i.IsDeleted == false &&
                                                             i.VleadSalesPersonId == (EmpId == 0 ? i.VleadSalesPersonId : EmpId) && i.VleadSalesPersonIsActive == true && //VSP
                                                             (i.InvoiceDate.Date <= recDate)).AsQueryable();  //date              

                    outstandingDetailsBasedOnCustomer123.AddRange(outstandingDetailsBasedOnCustomer);
                }

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

        //dashboard start
        //public IEnumerable<OutStandingViewModel> GetOSSummaryDetails(IEnumerable<OSCustomerIdsSum> Ids)

        public IEnumerable<OutStandingViewModel> GetOSSummaryDetails(SelectedCustomers selectedCustomers) // dashboard aging report

        {
            //return null;
            if (selectedCustomers.Ids != null && selectedCustomers.Ids.Count() > 0)
            {
                var clientID = new List<Nullable<int>>();
                foreach (var item in selectedCustomers.Ids)
                {
                    clientID.Add(item);
                }
                var customerDetails = _procedureWork.ExecProcedureOrView<GetAgeingOutstandingSummary_Result>("GetAgeingOutstandingSummary").Where(x => clientID.Contains(x.CustomerId)).ToList();
                var Summary = customerDetails
                     .Select(x => new OutStandingViewModel
                     {
                         Total = x.Total,
                         CustomerName = x.CustomerName,
                         Firstq = x.Firstq,
                         Second = x.Second,
                         Third = x.Third,
                         Fourth = x.Fourth,
                         Fifth = x.Fifth
                     }).OrderBy(x => x.CustomerName);
                return Summary;
            }
            else
            {
                var customerDetails = _procedureWork.ExecProcedureOrView<GetAgeingOutstandingSummary_Result>("GetAgeingOutstandingSummary").Where(x => x.CustomerId != 0).ToList();
                var Summary = customerDetails
                    .Select(x => new OutStandingViewModel
                    {
                        Total = x.Total,
                        CustomerName = x.CustomerName,
                        Firstq = x.Firstq,
                        Second = x.Second,
                        Third = x.Third,
                        Fourth = x.Fourth,
                        Fifth = x.Fifth
                    }).OrderBy(x => x.CustomerName);
                return Summary;
            }
        }

        public IEnumerable<GetAgeingOutstandingSummary_Result> GetOSSummary()
        {

            var customerDetails = _procedureWork.ExecProcedureOrView<GetAgeingOutstandingSummary_Result>("GetAgeingOutstandingSummary").ToList().Where(x => x.CustomerId == 0).OrderBy(x => x.CustomerName);

            return customerDetails;
        }

        //dashboard end
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
    }
}

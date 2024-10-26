using Microsoft.Data.SqlClient;
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
    public class ReceivableService
    {
        private readonly UnitWork _unitWork;
        private readonly TableWork _tableWork;
        private readonly ProcedureWork _procedureWork;
        private readonly UnitProcedure _unitprocedure;
        public ReceivableService(UnitWork unitWork,TableWork tableWork,
            ProcedureWork procedureWork,UnitProcedure unitProcedure)
        {
            _unitWork = unitWork;
            _tableWork = tableWork;
            _procedureWork = procedureWork;
            _unitprocedure = unitProcedure;
        }

    

        //loading client in RecTallyInteg
        public IEnumerable<Customer> GetClient()
        {
            try
            {
                return _tableWork.CustomerRepository.Get(x => x.IsDeleted == false).OrderByDescending(x => x.Id).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<GetReceivablesforIntegrationMaster_Result> GetReceivableIntegrationMaster(GetReceiptsPara Getparam)
        {
            var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@Clientid", Getparam.CustomerID),
                        new SqlParameter("@FromDate", Getparam.FromDate),
                        new SqlParameter("@ToDate", Getparam.ToDate),
                    };
            return _procedureWork.ExecStoredProcedure<GetReceivablesforIntegrationMaster_Result>("GetReceivablesforIntegrationMaster @clientid,@FromDate,@ToDate", sqlpara).ToList();
        }

        public string GetReceivableCopyIntegration(Receivableinteglist Recinteglist)
        {
            string Message = "Selected Receivables Moved to Integration";
            var selectedReceipts = Recinteglist.Recinteglist;
            foreach (var item in selectedReceipts)
            {
                var checkintegrationmaster = _unitWork.IntegrationMaster.First(x => x.DocumentNumber == item.VoucherNo);
                if (checkintegrationmaster == null)
                {
                    //if (item.ExchangeRate != null)
                    //{
                    var intigmaster = new IntegrationMaster();
                    intigmaster.TransactionId = item.TransactionId;
                    intigmaster.DocumentNumber = item.VoucherNo;
                    intigmaster.DocumentDate = item.CollectionDate;
                    intigmaster.ClientName = item.ShortName;
                    intigmaster.ProductValue = item.CollectionAmount;
                    intigmaster.TotalValue = item.CollectionAmount;
                    intigmaster.IsCancelled = false;
                    intigmaster.IsDeleted = false;
                    if (item.TransactionId == 12)
                    {
                        intigmaster.DestinationBank = item.DestinationBank;
                        intigmaster.Mode = item.ReceiptMode;
                        intigmaster.TransactionNumber = item.TransactionNo;
                        intigmaster.TransactionDate = item.TransactionDate;
                        intigmaster.Amount = item.Amount;
                        intigmaster.BankName = item.BankName;

                    }
                    intigmaster.CreatedBy = Recinteglist.EmployeeId;
                    intigmaster.CreatedUtc = DateTime.UtcNow;
                    intigmaster.ExchangeRate = item.ExchangeRate;
                    intigmaster.CurrencyName = "USD";
                    intigmaster.CurrencySymbol = "$";
                    _unitWork.IntegrationMaster.Insert(intigmaster);

                    //Tally IntTran

                    var getRcbtran = _unitprocedure.GetReceiptsIntegrationTran(item.Id);

                    foreach (var itemtran in getRcbtran)
                    {
                        var Rcbtran = new IntegrationTran();
                        Rcbtran.IntegrationMasterId = intigmaster.Id;
                        Rcbtran.Value = itemtran.AdjustmentAmount;
                        Rcbtran.InvoiceNumber = itemtran.InvoiceNo;
                        Rcbtran.InvoiceDate = itemtran.InvoiceDate;
                        _unitWork.IntegrationTran.Insert(Rcbtran);
                    }

                } // if end

                else
                {
                    var getRcbtran = _unitprocedure.GetReceiptsIntegrationTran(item.Id).Where(x => x.IsTallyIntegrated == false || x.IsTallyIntegrated == null);
                    foreach (var itemtran in getRcbtran)
                    {
                        var Rcbtran = new IntegrationTran();
                        Rcbtran.IntegrationMasterId = checkintegrationmaster.Id;
                        Rcbtran.Value = itemtran.AdjustmentAmount;
                        Rcbtran.InvoiceNumber = itemtran.InvoiceNo;
                        Rcbtran.InvoiceDate = itemtran.InvoiceDate;
                        _unitWork.IntegrationTran.Insert(Rcbtran);
                    }
                } //else end

                //ReceivableAdjustmet Istally
                var GetRcbAdj = _unitWork.ReceivableAdjustment.Where(x => x.ReceivableId == item.Id && (x.IsTallyIntegrated == false || x.IsTallyIntegrated == null)).ToList();
                GetRcbAdj.ForEach(x => x.IsTallyIntegrated = true);
                _unitWork.SaveChanges();

                //Receivable Istally
                var GetRcbAdjAdvforRec = _unitWork.ReceivableAdjustment.Where(x => x.ReceivableId == item.Id && x.IsAvailableAdvance == true).ToList();
                var GetRcbAdjforRec = _unitWork.ReceivableAdjustment.Where(x => x.ReceivableId == item.Id && (x.IsTallyIntegrated == false || x.IsTallyIntegrated == null)).ToList();
                if (GetRcbAdjAdvforRec.Count() == 0 && GetRcbAdjforRec.Count() == 0)
                {
                    var Recmaster = _unitWork.Receivable.FirstOrDefault(x => x.Id == item.Id);
                    Recmaster.IsTallyIntegrated = true;
                    _unitWork.Receivable.Update(Recmaster);
                }

                _unitWork.SaveTally();
                _unitWork.SaveChanges();
            }
            return Message;
        }


        public Object GetAllReceivable()
        {
            int TranId = 12;
            //var AllReceivableList = new List<GetAllReceivables_Result>();
            var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@Id", TranId)
                    };
            return _procedureWork.ExecStoredProcedure<GetAllReceivables_Result>("GetAllReceivables @Id", sqlpara).ToList().OrderByDescending(x => x.Id);
            //return AllReceivableList;
        }

        public IEnumerable<InvoiceMaster> GetAllCustomerInvoice(int customerId)
        {
            //var sqlpara = new SqlParameter[]
            //        {                                             
            //            new SqlParameter("@ClientId", customerId)
            //        };
            //return _procedureWork.ExecStoredProcedure<InvoiceMaster>("GetCustomerPendingInvoices @ClientId", sqlpara).ToList();

            var cancelledInvoices = (from A in _tableWork.InvoiceTranRepository.Get(x => x.InvoiceNo != null)
                                     join B in _tableWork.InvoiceMasterRepository.Get() on A.Imid equals B.Id
                                     select B);


            var cancelledInvoices2 = (from invMaster in _tableWork.InvoiceMasterRepository.Get()
                                      join tran in _tableWork.InvoiceTranRepository.Get(x => x.InvoiceNo != null) on invMaster.InvoiceNo equals tran.InvoiceNo
                                      select invMaster);

            var AllCancelledInvoices = cancelledInvoices.Union(cancelledInvoices2);


            return _tableWork.InvoiceMasterRepository.Get(x => x.IsOutstanding == true && x.CustomerId == customerId &&
                x.IsCancelled == false && x.IsDeleted == false && x.IsSample == false && x.InvoiceNo != null).Except(AllCancelledInvoices).ToList();
        }
        public ReceivableAdjustmentModel GetInvoiceDetails(string invoiceNumber, int customerId)
        {
            var invoiceDetails = _tableWork.InvoiceMasterRepository.GetAllVal(x => x.ReceivableAdjustments).FirstOrDefault(x => x.IsOutstanding == true && x.CustomerId == customerId &&
            x.InvoiceNo == invoiceNumber && x.IsCancelled == false && x.IsDeleted == false && x.IsSample == false);
            var customerInvoice = new ReceivableAdjustmentModel()
            {
                Id = invoiceDetails.Id,
                InvoiceNo = invoiceDetails.InvoiceNo,
                InvoiceDate = invoiceDetails.InvoiceDate.Value,
                InvoiceId = invoiceDetails.Id,
                //InvoiceValue = invoiceDetails.InvoiceValue
                InvoiceValue = invoiceDetails.TotalInvoiceValue
            };
            var adjustmentAmount = 0.0M;
            foreach (var adjustment in invoiceDetails.ReceivableAdjustments)
            {
                customerInvoice.IsInvoiceAdjustment = adjustment.IsInvoiceAdjustment;
                if (adjustment.AdjustmentAmount != null)
                {
                    adjustmentAmount = adjustmentAmount + adjustment.AdjustmentAmount.Value;
                }
            }
            customerInvoice.AdjustmentAmount = adjustmentAmount;
            customerInvoice.ReferenceNo = invoiceDetails.InvoiceNo;
            customerInvoice.CRDR = "CR";
            return customerInvoice;
        }

        public object GetReceviableById(int receivableId)
        {
            //var receivables = _db.Receivables.Include("ReceivableAdjustments").Include("Customer").Include("ReceivableExts").Include("TransactionType").Where(x => x.Id == receivableId).FirstOrDefault();
            //return receivables;

            var receivables = _tableWork.ReceivableRepository.GetAllVal(x => x.ReceivableAdjustments, x => x.Customer, x => x.ReceivableExts, x => x.Transaction).FirstOrDefault(x => x.Id == receivableId);
            return receivables;
            //return receivableId;

        }
        public string CreateReveicableDetails(ReceivableModel receivableModel)
        {
            bool status = false;
            string prefix;
            string voucherNumber;
            long receiptNumber;
            int DepartmentId = 4;
            //int transactionId = 12;
            var voucher = new VoucherControl();
            if (receivableModel != null)
            {
                try
                {
                    var transtype = _unitWork.TransactionType.FirstOrDefault(x => x.Description == "Collection");
                    var fd = receivableModel.Receivables.CollectionDate.ToUniversalTime();
                    var voucherControl = _unitWork.VoucherControl.FirstOrDefault(x => x.TransactionId == transtype.Id && x.IsDeleted == false &&
                        x.DepartmentId == DepartmentId && x.EffectiveFrom.Date <= fd.Date && x.EffectiveTo.Date >= fd.Date);
                    if (voucherControl == null)
                    {
                        return "False";
                    }
                    else
                    {
                        prefix = voucherControl.Prefix.ToString();
                        receiptNumber = voucherControl.Voucherno + 1;
                        string monthsingle = receivableModel.Receivables.CollectionDate.ToString("MM");
                        string daysingle = receivableModel.Receivables.CollectionDate.ToString("dd");
                        voucherNumber = prefix + monthsingle +
                        daysingle + "/" + receiptNumber;
                    }
                    receivableModel.Receivables.VoucherNo = voucherNumber;
                    receivableModel.Receivables.TransactionId = transtype.Id; // transactionId
                    receivableModel.Receivables.CreatedUtc = DateTime.UtcNow;
                    receivableModel.Receivables.CollectionDate = receivableModel.Receivables.CollectionDate.ToUniversalTime();
                    receivableModel.Receivables.ReferenceDate = receivableModel.Receivables.ReferenceDate.ToUniversalTime();
                    _unitWork.Receivable.Add(receivableModel.Receivables);
                    _unitWork.SaveChanges();
                    if (receivableModel.ReceivableExts.Count() > 0)
                    {
                        foreach (var item in receivableModel.ReceivableExts)
                        {
                            item.ReceivableId = receivableModel.Receivables.Id;
                        }
                        _unitWork.ReceivableExt.AddRange(receivableModel.ReceivableExts);
                    }
                    if (receivableModel.ReceivableAdjustments.Count() > 0)
                    {
                        foreach (var item in receivableModel.ReceivableAdjustments)
                        {
                            if (item.IsInvoiceAdjustment == true)
                            {
                                item.ReceivableId = receivableModel.Receivables.Id;
                                item.ReferenceNo = item.InvoiceNo;
                                item.IsAvailableAdvance = false;
                                item.Crdr = "CR";
                                item.AvailableAdvance = 0;
                            }
                            else
                            {
                                item.ReceivableId = receivableModel.Receivables.Id;
                                item.ReferenceNo = receivableModel.Receivables.VoucherNo;
                                item.IsAvailableAdvance = true;
                                item.Crdr = "CR";
                                item.AvailableAdvance = item.AdjustmentAmount;
                            }
                        }
                        _unitWork.ReceivableAdjustment.AddRange(receivableModel.ReceivableAdjustments);
                    }
                    //long dbstatus = _tableWork.SaveChanges();
                    //status = dbstatus > 0;
                    if (receivableModel.AlreadyAdjusted.Count() > 0)
                    {
                        foreach (var item in receivableModel.AlreadyAdjusted)
                        {
                            //if (item.InvoiceValue == item.AlreadyAdjustedAmount + item.CurrentAdjustedAmount)
                            if (item.InvoiceValue == item.AlreadyAdjustedAmount + item.CurrentAdjustedAmount)
                            {
                                var invoiceDetails = _unitWork.InvoiceMaster.FirstOrDefault(x => x.Id == item.InvoiceId && x.InvoiceNo == item.InvoiceNo);
                                invoiceDetails.IsOutstanding = false;
                                invoiceDetails.UpdatedBy = receivableModel.Receivables.CreatedBy;
                                invoiceDetails.UpdatedUtc = DateTime.UtcNow;
                                //_tableWork.Entry(invoiceDetails).State = EntityState.Modified;
                                _unitWork.InvoiceMaster.Update(invoiceDetails);
                                //  _tableWork.SaveChanges();
                            }
                        }
                    }
                    //transactionId
                    var UpdatedVocherNumber = _unitWork.VoucherControl.FirstOrDefault(x => x.TransactionId == transtype.Id &&
                       x.DepartmentId == DepartmentId && x.EffectiveFrom.Date <= receivableModel.Receivables.CollectionDate.Date && x.EffectiveTo.Date >= receivableModel.Receivables.CollectionDate.Date);
                    UpdatedVocherNumber.Voucherno = receiptNumber;
                    UpdatedVocherNumber.UpdatedBy = receivableModel.Receivables.CreatedBy;
                    UpdatedVocherNumber.UpdatedUtc = DateTime.UtcNow;
                    //_tableWork.Entry(UpdatedVocherNumber).State = EntityState.Modified;
                    _unitWork.VoucherControl.Update(UpdatedVocherNumber);
                    _unitWork.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return "True";
        }


        // cr
        public string CreateCreditNoteDetails(ReceivableModel CreditNoteModel)
        {
            bool status = false;
            string prefix;
            string voucherNumber;
            long receiptNumber;
            int DepartmentId = 4;

            var voucher = new VoucherControl();
            if (CreditNoteModel != null)
            {
                try
                {
                    var fd = CreditNoteModel.Receivables.CollectionDate.ToUniversalTime();
                    var transtype = _unitWork.TransactionType.FirstOrDefault(x => x.Description == "Credit Note");
                    var voucherControl = _unitWork.VoucherControl.FirstOrDefault(x => x.TransactionId == transtype.Id && x.IsDeleted == false &&
                        x.DepartmentId == DepartmentId && x.EffectiveFrom.Date <= fd.Date && x.EffectiveTo.Date >= fd.Date);

                    // x.DepartmentId == DepartmentId && DbFunctions.TruncateTime(x.EffectiveFrom) <= DbFunctions.TruncateTime(receivableModel.Receivables.CollectionDate) && DbFunctions.TruncateTime(x.EffectiveTo) >= lastOfThisMonth).FirstOrDefault();
                    if (voucherControl == null)
                    {
                        return "False";
                    }
                    else
                    {
                        prefix = voucherControl.Prefix.ToString();
                        receiptNumber = voucherControl.Voucherno + 1;
                        string monthsingle = CreditNoteModel.Receivables.CollectionDate.ToString("MM");
                        string daysingle = CreditNoteModel.Receivables.CollectionDate.ToString("dd");
                        voucherNumber = prefix + monthsingle +
                        daysingle + "/" + receiptNumber;

                        //voucherNumber = prefix + receivableModel.Receivables.CollectionDate.Month +
                        //receivableModel.Receivables.CollectionDate.Day + receiptNumber;
                    }
                    CreditNoteModel.Receivables.VoucherNo = voucherNumber;
                    CreditNoteModel.Receivables.TransactionId = transtype.Id; //transactionId
                    CreditNoteModel.Receivables.CreatedUtc = DateTime.UtcNow;
                    //receivableModel.Receivables.CollectionDate = DateTime.UtcNow;                  
                    CreditNoteModel.Receivables.CollectionDate = CreditNoteModel.Receivables.CollectionDate.ToUniversalTime();
                    CreditNoteModel.Receivables.ReferenceDate = CreditNoteModel.Receivables.ReferenceDate.ToUniversalTime();
                    _unitWork.Receivable.Add(CreditNoteModel.Receivables);
                    _unitWork.Save();

                    if (CreditNoteModel.ReceivableAdjustments.Count() > 0)
                    {
                        foreach (var item in CreditNoteModel.ReceivableAdjustments)
                        {
                            if (item.IsInvoiceAdjustment == true)
                            {
                                item.ReceivableId = CreditNoteModel.Receivables.Id;
                                item.ReferenceNo = item.InvoiceNo;
                                item.IsAvailableAdvance = false;
                                item.Crdr = "CR";
                                item.AvailableAdvance = 0;
                                item.AdjustmentAmount = CreditNoteModel.Receivables.CollectionAmount;
                            }

                        }
                        _unitWork.ReceivableAdjustment.AddRange(CreditNoteModel.ReceivableAdjustments);
                    }
                    long dbstatus = _unitWork.Save();
                    status = dbstatus > 0;
                    if (CreditNoteModel.AlreadyAdjusted.Count() > 0)
                    {
                        foreach (var item in CreditNoteModel.AlreadyAdjusted)
                        {
                            //if (item.InvoiceValue == item.AlreadyAdjustedAmount + item.CurrentAdjustedAmount)
                            if (item.InvoiceValue == item.AlreadyAdjustedAmount + item.CurrentAdjustedAmount)
                            {
                                var invoiceDetails = _unitWork.InvoiceMaster.FirstOrDefault(x => x.Id == item.InvoiceId && x.InvoiceNo == item.InvoiceNo);
                                invoiceDetails.IsOutstanding = false;
                                invoiceDetails.UpdatedBy = CreditNoteModel.Receivables.CreatedBy;
                                invoiceDetails.UpdatedUtc = DateTime.UtcNow;
                                //_tableWork.Entry(invoiceDetails).State = EntityState.Modified;
                                _unitWork.InvoiceMaster.Update(invoiceDetails);
                                _unitWork.Save();
                            }
                        }
                    }
                    //transactionId
                    var UpdatedVocherNumber = _unitWork.VoucherControl.FirstOrDefault(x => x.TransactionId == transtype.Id &&
                       x.DepartmentId == DepartmentId && x.EffectiveFrom.Date <= CreditNoteModel.Receivables.CollectionDate.Date && x.EffectiveTo.Date >= CreditNoteModel.Receivables.CollectionDate.Date);
                    UpdatedVocherNumber.Voucherno = receiptNumber;
                    UpdatedVocherNumber.UpdatedBy = CreditNoteModel.Receivables.CreatedBy;
                    UpdatedVocherNumber.UpdatedUtc = DateTime.UtcNow;
                    // _db.Entry(UpdatedVocherNumber).State = EntityState.Modified;
                    _unitWork.VoucherControl.Update(UpdatedVocherNumber);
                    _unitWork.Save();
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
            }
            return "True";
        }

        // cr grid start
        public IEnumerable<ReceivableModel> GetAllCreditNote()
        {
            List<ReceivableModel> AllCreditNote = new List<ReceivableModel>();
            var AllCreditNoteList = new List<ReceivableModel>();
            // int trantypeid = 1020;
            var transtype = _tableWork.TransactionTypeRepository.Get(x => x.Description == "Credit Note").FirstOrDefault();
            //return _db.Receivables.Where(x => x.IsDeleted == false).ToList();
            // return _db.Receivables.Where(x => x.IsDeleted == false).OrderByDescending(x => x.Id).ToList();
            var temp = (from rcb in _tableWork.ReceivableRepository.Get()
                        join cust in _tableWork.CustomerRepository.Get() on rcb.CustomerId equals cust.Id
                        join rca in _tableWork.ReceivableAdjustmentRepository.Get() on rcb.Id equals rca.ReceivableId
                        select new
                        {
                            id = rcb.Id,
                            VoucherNo = rcb.VoucherNo,
                            CollectionDate = rcb.CollectionDate.AddMinutes((double)cust.RpttimeZoneDifference),
                            CollectionAmount = rcb.CollectionAmount,
                            ReferenceNo = rcb.ReferenceNo,
                            ReferenceDate = rcb.ReferenceDate.AddMinutes((double)cust.RpttimeZoneDifference),
                            Description = rcb.Description,
                            IsDeleted = rcb.IsDeleted,
                            CustomerName = cust.Name,
                            CustomerShortName = cust.ShortName,
                            InvoiceNo = rca.InvoiceNo, //
                            TransactionId = rcb.TransactionId
                        }).Where(x => x.IsDeleted == false && x.TransactionId == transtype.Id).AsEnumerable();
            try
            {
                foreach (var k in temp)
                {
                    ReceivableModel rec = new ReceivableModel();
                    rec.Id = k.id;
                    rec.VoucherNo = k.VoucherNo;
                    rec.CollectionDate = Convert.ToDateTime(k.CollectionDate);
                    rec.CollectionAmount = k.CollectionAmount;
                    rec.ReferenceNo = k.ReferenceNo;
                    rec.ReferenceDate = Convert.ToDateTime(k.ReferenceDate);
                    rec.Description = k.Description;
                    rec.CustomerName = k.CustomerName;
                    rec.CustomerShortName = k.CustomerShortName;
                    rec.InvoiceNo = k.InvoiceNo; //
                    AllCreditNote.Add(rec);
                }
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
            AllCreditNoteList = AllCreditNote.OrderByDescending(x => x.Id).ToList(); ;

            return AllCreditNoteList;
        }

        // cr grid end
    }
}

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;

//using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class InvoiceService
    {
        private readonly TableWork _tableWork;

        private readonly UnitWork _unitWork;
        private readonly UnitViewWork _unitviewWork;
        private readonly ProcedureWork _procedureWork;
        private readonly UnitProcedure _unitprocedure;
        private readonly ApplicationDbContext db;
        private readonly ApplicationDbContext _db;
       
        public InvoiceService(TableWork tableWork,UnitWork unitWork,
            UnitProcedure unitProcedure,UnitViewWork unitViewWork,ApplicationDbContext dbContext, ProcedureWork procedureWork)
        {
            _tableWork = tableWork;
            _unitWork = unitWork;
            _procedureWork = procedureWork;
            _unitprocedure = unitProcedure;
            _unitviewWork = unitViewWork;
            db = dbContext;
            _db = dbContext;

        }
        Decimal rate = 0;
        Decimal productvalue = 0;
        int qty = 0;
        string Invoiceno = "";
        int fileCounts = 0;
        //display in 1st tab
        public IEnumerable<GetGenerateforInvoice_Result> getinvoicedetails(GetJobsforInvoiceVM1 GetInvoice)
        {
            try
            {
                var result = _unitprocedure.GetGenerateforInvoice(GetInvoice.ClientId, GetInvoice.FromDate, GetInvoice.ToDate).OrderByDescending(x => x.ESTDateofUpload).AsQueryable();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //loading client in 1st tab
        public IEnumerable<ViewGetCustomerforSelection> GetClient()
        {
            try
            {
                var result = _unitviewWork.ViewGetCustomerforSelection.OrderBy(x => x.ShortName).AsQueryable();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //loading data in 2nd tab
        public IEnumerable<GeneratedInvoice_Result> GetCalculatedPrice(GetJobsforInvoiceVM1 GetInvoice)
        {
            try
            {
                var result = _unitprocedure.GeneratedInvoice(GetInvoice.ClientId).AsQueryable();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //loading data in 2nd tab for staffing
        public IEnumerable<GetStaffingAmount_Result> GetStaffingPriceAmount(GetJobsforInvoiceVM1 GetInvoice)
        {
            try
            {
                var result = _unitprocedure.GetStaffingAmount(GetInvoice.ClientId).AsQueryable();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //price calc in 2nd tab
        public string GenerateInvoice(GetInvoiceVM GetInvoice)
        {
            string Message = "Invoice Preview for Selected Job(s).";
            var selectedJobs = GetInvoice.GetInvoice;
            db.ChangeTracker.AutoDetectChangesEnabled = false;
            //db.Configuration.AutoDetectChangesEnabled = false;
            //db.Configuration.ValidateOnSaveEnabled = false;
            int gcount = 0;
            /*  Dictionary<string, List<string>> unbilledFilesByScope = new Dictionary<string, List<string>>();

              foreach (var item1 in selectedJobs)
              {
                  try
                  {
                      var unBilledFiles = _unitWork.JobOrder
                          .Where(jo => jo.JobId == item1.JobId && jo.ScopeId == item1.ScopeId && jo.ClientId == item1.ClientId && jo.JobStatusId == item1.JobStatusId && (jo.PricingTypeId == null || jo.PricingTypeId == 0))
                          .FirstOrDefault();

                      if (unBilledFiles != null)
                      {
                          string commonDetails = $"JobStatusId: {item1.JobStatusId}, ScopeId: {item1.ScopeId}, ClientId: {item1.ClientId}";
                          string jobDetails = $"{commonDetails}; Job ID: {item1.JobId}";

                          if (unbilledFilesByScope.ContainsKey(commonDetails))
                          {
                              unbilledFilesByScope[commonDetails].Add(item1.JobId);
                          }
                          else
                          {
                              unbilledFilesByScope.Add(commonDetails, new List<string> { item1.JobId });
                          }
                      }

                      var aggregatedDetails = unbilledFilesByScope
                         .Where(kv => kv.Value.Count > 1)
                         .Select(kv => $"{kv.Key}; Job ID: {string.Join(", ", kv.Value)}")
                         .ToList();

                      if (aggregatedDetails.Any())
                      {
                          int totalJobCount = aggregatedDetails.SelectMany(s => s.Split(';')[1].Split(',')).Select(j => j.Trim()).Distinct().Count();
                          string pluralS = totalJobCount > 1 ? "s" : "";
                          Message = $"There are {totalJobCount} job{pluralS} without pricing in {string.Join("; ", aggregatedDetails)}. Do you want to proceed? (yes/no)";
                      }

                  }
                  catch (Exception ex)
                  {
                      Log addlog = new Log();
                      addlog.Module = "Invoice Rep" + item1.JId;
                      addlog.Description = "Error Msg: " + ex.Message + ", Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source;
                      addlog.ClientId = item1.ClientId;
                      addlog.JobId = item1.JId;
                      addlog.Type = "Error Occ";
                      addlog.CreatedUtc = DateTime.UtcNow;
                      _unitWork.Log.Add(addlog);
                      _unitWork.SaveChanges();
                  }

              }

              UnbilledInvoice unbilledInvoice = new UnbilledInvoice();
              if (unbilledInvoice.IsContinue == "Yes")
              {*/
            foreach (var item in selectedJobs)
            {
                try
                {
                    //1.check its has special price thn only insert and update
                    var checkspecialprice = _unitWork.JobOrder.Where(x => x.Id == item.JId && x.IsDeleted == false && x.DateofUpload != null && x.SpecialPrice != null)
                                .Select(x => new { x.PricingTypeId, x.IsSpecialPrice, x.SpecialPrice, x.IsWaiver }).FirstOrDefault();

                    if (checkspecialprice != null)
                    {
                        ////split staffing and normal job for insert and update
                        //var alreadyexiststaffingpricing = _unitprocedure.GetCheckStaffing(item.ClientId, item.DepartmentId, item.FileReceivedDate);
                        //int staffingdesig = alreadyexiststaffingpricing.Count();
                        var invoicemaster = _unitWork.InvoiceMaster.FirstOrDefault(x => x.CustomerId == item.ClientId && x.InvoiceNo == null);
                        var jidExists = _unitWork.InvoiceJob.FirstOrDefault(x => x.Jid == item.JId) != null;

                        var dapartmentname = _unitWork.Department.FirstOrDefault(x => x.Id == item.DepartmentId && x.IsDeleted == false).Description;
                        //if (staffingdesig > 0)
                        //{
                        //    //staffing jobs only insert and update
                        //    if (invoicemaster == null)
                        //    {
                        //        //only insert
                        //    }
                        //    else
                        //    {
                        //        //insert and update
                        //    }
                        //}
                        //else
                        //{
                        ////normal jobs only insert and update
                        if (invoicemaster == null)
                        {
                            //only insert
                            var saveinvoicemaster = new InvoiceMaster();
                            saveinvoicemaster.ReferenceNo = "123";
                            saveinvoicemaster.ReferenceDate = DateTime.UtcNow;
                            saveinvoicemaster.CustomerId = item.ClientId;
                            saveinvoicemaster.IsDeleted = false;
                            saveinvoicemaster.IsCancelled = false;
                            saveinvoicemaster.IsSample = false;
                            saveinvoicemaster.CreatedUtc = DateTime.UtcNow;
                            saveinvoicemaster.CreatedBy = GetInvoice.CreatedBy;
                            saveinvoicemaster.IsOutstanding = true;
                            saveinvoicemaster.PrimaryDateTime = DateTime.UtcNow;
                            saveinvoicemaster.PrimaryUpdatedBy = GetInvoice.CreatedBy;
                            if (item.DepartmentId == 1)
                            {
                                if (checkspecialprice.IsWaiver == true)
                                {
                                    saveinvoicemaster.ArtInvoiceAmount = checkspecialprice.SpecialPrice;
                                    saveinvoicemaster.DigiInvoiceAmount = 0;
                                    saveinvoicemaster.ArtWaiver = checkspecialprice.SpecialPrice;
                                    saveinvoicemaster.DigiWaiver = 0;
                                }
                                else
                                {
                                    saveinvoicemaster.ArtInvoiceAmount = checkspecialprice.SpecialPrice;
                                    saveinvoicemaster.DigiInvoiceAmount = 0;
                                    saveinvoicemaster.ArtWaiver = 0;
                                    saveinvoicemaster.DigiWaiver = 0;
                                }
                            }
                            else if (item.DepartmentId == 2)
                            {
                                if (checkspecialprice.IsWaiver == true)
                                {
                                    saveinvoicemaster.ArtInvoiceAmount = 0;
                                    saveinvoicemaster.DigiInvoiceAmount = checkspecialprice.SpecialPrice;
                                    saveinvoicemaster.DigiWaiver = checkspecialprice.SpecialPrice;
                                    saveinvoicemaster.ArtWaiver = 0;
                                }
                                else
                                {
                                    saveinvoicemaster.DigiInvoiceAmount = checkspecialprice.SpecialPrice;
                                    saveinvoicemaster.ArtInvoiceAmount = 0;
                                    saveinvoicemaster.ArtWaiver = 0;
                                    saveinvoicemaster.DigiWaiver = 0;
                                }
                            }
                            saveinvoicemaster.ProductValue = checkspecialprice.SpecialPrice;
                            saveinvoicemaster.Discount = 0;
                            //
                            saveinvoicemaster.Waiver = saveinvoicemaster.ArtWaiver + saveinvoicemaster.DigiWaiver;
                            //
                            saveinvoicemaster.ReceivableValue = saveinvoicemaster.ProductValue - saveinvoicemaster.Waiver;
                            decimal invoicevalue = Convert.ToDecimal(saveinvoicemaster.ReceivableValue);
                            saveinvoicemaster.RoundOff = Math.Round(invoicevalue, 2) - saveinvoicemaster.ReceivableValue;
                            saveinvoicemaster.InvoiceValue = Math.Round(invoicevalue, 2);
                            saveinvoicemaster.TotalInvoiceValue = Math.Round(invoicevalue, 2);
                            _unitWork.InvoiceMaster.Add(saveinvoicemaster);
                            _unitWork.SaveChanges();

                            var saveinvoicetran = new InvoiceTran();
                            saveinvoicetran.Imid = saveinvoicemaster.Id;
                            saveinvoicetran.Qty = 1;
                            saveinvoicetran.Rate = checkspecialprice.SpecialPrice;
                            saveinvoicetran.Value = saveinvoicetran.Qty * saveinvoicetran.Rate;
                            saveinvoicetran.PricingTypeId = (int)checkspecialprice.PricingTypeId;
                            saveinvoicetran.Createdby = GetInvoice.CreatedBy;
                            saveinvoicetran.CreatedUtc = DateTime.UtcNow;
                            if (item.DepartmentId == 1)
                            {
                                saveinvoicetran.ScopeId = item.ScopeId.Value;
                            }
                            else if (item.DepartmentId == 2)
                            {
                                saveinvoicetran.ScopeId = item.ScopeId.Value;
                            }
                            saveinvoicetran.DepartmentId = item.DepartmentId;
                            saveinvoicetran.DepartmentName = dapartmentname;
                            _unitWork.InvoiceTran.Add(saveinvoicetran);
                            _unitWork.SaveChanges();

                            var saveinvoicejob = new InvoiceJob();
                            saveinvoicejob.It = saveinvoicetran.Id;
                            saveinvoicejob.ItNavigation = saveinvoicetran;
                            saveinvoicejob.JobId = item.JobId;
                            saveinvoicejob.CreatedBy = GetInvoice.CreatedBy;
                            saveinvoicejob.CreatedUtc = DateTime.UtcNow;
                            saveinvoicejob.Imid = saveinvoicemaster.Id;
                            saveinvoicejob.Jid = item.JId;
                            _unitWork.InvoiceJob.Add(saveinvoicejob);
                            if (!jidExists) _unitWork.SaveChanges();
                        }
                        else
                        {
                            ////insert and update
                            var saveinvoicetran = new InvoiceTran();
                            saveinvoicetran.Imid = invoicemaster.Id;
                            saveinvoicetran.Qty = 1;
                            saveinvoicetran.Rate = checkspecialprice.SpecialPrice;
                            saveinvoicetran.Value = saveinvoicetran.Qty * saveinvoicetran.Rate;
                            saveinvoicetran.PricingTypeId = (int)checkspecialprice.PricingTypeId;
                            saveinvoicetran.Createdby = GetInvoice.CreatedBy;
                            saveinvoicetran.CreatedUtc = DateTime.UtcNow;
                            if (item.DepartmentId == 1)
                            {
                                saveinvoicetran.ScopeId = item.ScopeId.Value;
                            }
                            else if (item.DepartmentId == 2)
                            {
                                saveinvoicetran.ScopeId = item.ScopeId.Value;
                            }
                            saveinvoicetran.DepartmentId = item.DepartmentId;
                            saveinvoicetran.DepartmentName = dapartmentname;
                            _unitWork.InvoiceTran.Add(saveinvoicetran);
                            _unitWork.SaveChanges();

                            var saveinvoicejob = new InvoiceJob();
                            saveinvoicejob.It = saveinvoicetran.Id;
                            saveinvoicejob.ItNavigation = saveinvoicetran;
                            saveinvoicejob.JobId = item.JobId;
                            saveinvoicejob.CreatedBy = GetInvoice.CreatedBy;
                            saveinvoicejob.CreatedUtc = DateTime.UtcNow;
                            saveinvoicejob.Imid = invoicemaster.Id;
                            saveinvoicejob.Jid = item.JId;
                            _unitWork.InvoiceJob.Add(saveinvoicejob);

                            invoicemaster.ProductValue = invoicemaster.ProductValue + checkspecialprice.SpecialPrice;
                            if (item.DepartmentId == 1)
                            {
                                if (checkspecialprice.IsWaiver == true)
                                {
                                    invoicemaster.ArtInvoiceAmount = invoicemaster.ArtInvoiceAmount + checkspecialprice.SpecialPrice;
                                    invoicemaster.ArtWaiver = invoicemaster.ArtWaiver + checkspecialprice.SpecialPrice;
                                    invoicemaster.Waiver = invoicemaster.ArtWaiver + invoicemaster.DigiWaiver;
                                }
                                else
                                {
                                    invoicemaster.ArtInvoiceAmount = invoicemaster.ArtInvoiceAmount + checkspecialprice.SpecialPrice;
                                }
                            }
                            else if (item.DepartmentId == 2)
                            {
                                if (checkspecialprice.IsWaiver == true)
                                {
                                    invoicemaster.DigiInvoiceAmount = invoicemaster.DigiInvoiceAmount + checkspecialprice.SpecialPrice;
                                    invoicemaster.DigiWaiver = invoicemaster.DigiWaiver + checkspecialprice.SpecialPrice;
                                    invoicemaster.Waiver = invoicemaster.ArtWaiver + invoicemaster.DigiWaiver;
                                }
                                else
                                {
                                    invoicemaster.DigiInvoiceAmount = invoicemaster.DigiInvoiceAmount + checkspecialprice.SpecialPrice;
                                }
                            }
                            //
                            //invoicemaster.Waiver = invoicemaster.Waiver + (invoicemaster.ArtWaiver + invoicemaster.DigiWaiver);
                            //
                            invoicemaster.ReceivableValue = invoicemaster.ProductValue - invoicemaster.Waiver;
                            decimal invoicevalue = Convert.ToDecimal(invoicemaster.ReceivableValue);
                            invoicemaster.RoundOff = Math.Round(invoicevalue, 2) - invoicemaster.ReceivableValue;
                            invoicemaster.InvoiceValue = Math.Round(invoicevalue, 2);
                            invoicemaster.TotalInvoiceValue = Math.Round(invoicevalue, 2);
                            _unitWork.InvoiceMaster.Update(invoicemaster);
                        }
                        if (jidExists) _unitWork.SaveChanges();
                        //}
                    }
                    else
                    {
                        //price calculation goes here //old code
                        var myCollection = new List<GetInvoiceVM>();
                        var jobord = _unitWork.JobOrder.FirstOrDefault(x => x.Id == item.JId && x.IsDeleted == false);
                        var jidExists = _unitWork.InvoiceJob.FirstOrDefault(x => x.Jid == item.JId) != null;

                        var alreadyexiststaffingpricing = _unitprocedure.GetCheckStaffing(item.ClientId, item.DepartmentId, item.FileReceivedDate);
                        int staffingdesig = alreadyexiststaffingpricing.Count();
                        if (staffingdesig > 0)
                        { //staffing if start
                            var getstaffing = alreadyexiststaffingpricing.Where(x => x.FromDate <= item.FileReceivedDate && x.ToDate >= item.FileReceivedDate).ToList();
                            decimal sumofprice = Convert.ToDecimal(getstaffing.Select(x => x.Price).Sum());
                            var checkspecialpriceold = _unitWork.JobOrder.Where(x => x.Id == item.JId && x.IsDeleted == false && x.DateofUpload != null && x.SpecialPrice != null)
                                .Select(x => new { x.PricingTypeId, x.IsSpecialPrice, x.SpecialPrice }).FirstOrDefault();

                            int countOfJobs = selectedJobs.Count();
                            var invoicemasterId = _unitWork.InvoiceMaster.Where(x => x.CustomerId == item.ClientId && x.InvoiceNo == null).Select(x => x.Id).FirstOrDefault();
                            var dapartmentname = _unitWork.Department.FirstOrDefault(x => x.Id == item.DepartmentId && x.IsDeleted == false).Description;
                            if (invoicemasterId == null || invoicemasterId == 0)
                            {
                                var saveinvoicemaster = new InvoiceMaster();
                                saveinvoicemaster.ReferenceDate = DateTime.UtcNow;
                                saveinvoicemaster.ReferenceNo = "123";
                                saveinvoicemaster.CustomerId = item.ClientId;
                                saveinvoicemaster.ProductValue = sumofprice;
                                saveinvoicemaster.Discount = 0;
                                saveinvoicemaster.ArtWaiver = 0;
                                saveinvoicemaster.DigiWaiver = 0;
                                saveinvoicemaster.Waiver = 0;
                                saveinvoicemaster.ReceivableValue = saveinvoicemaster.ProductValue;
                                decimal invoicevalue = Convert.ToDecimal(saveinvoicemaster.ReceivableValue);
                                saveinvoicemaster.RoundOff = Math.Round(invoicevalue, 2) - saveinvoicemaster.ReceivableValue;
                                saveinvoicemaster.InvoiceValue = Math.Round(invoicevalue, 2);
                                saveinvoicemaster.TotalInvoiceValue = Math.Round(invoicevalue, 2);
                                saveinvoicemaster.IsDeleted = false;
                                saveinvoicemaster.IsCancelled = false;
                                saveinvoicemaster.IsSample = false;
                                saveinvoicemaster.CreatedBy = GetInvoice.CreatedBy;
                                saveinvoicemaster.CreatedUtc = DateTime.UtcNow;
                                saveinvoicemaster.IsOutstanding = true;
                                saveinvoicemaster.PrimaryUpdatedBy = GetInvoice.CreatedBy;
                                saveinvoicemaster.PrimaryDateTime = DateTime.UtcNow;
                                _unitWork.InvoiceMaster.Add(saveinvoicemaster);

                                var saveinvoicetran = new InvoiceTran();
                                saveinvoicetran.Imid = saveinvoicemaster.Id;
                                saveinvoicetran.Value = sumofprice/countOfJobs;
                                if (item.DepartmentId == 1)
                                {
                                    saveinvoicetran.PricingTypeId = 5;
                                }
                                else if (item.DepartmentId == 2)
                                {
                                    saveinvoicetran.PricingTypeId = 15;
                                }
                                saveinvoicetran.DepartmentId = item.DepartmentId;
                                saveinvoicetran.DepartmentName = dapartmentname;
                                saveinvoicetran.Createdby = GetInvoice.CreatedBy;
                                saveinvoicetran.CreatedUtc = DateTime.UtcNow;
                                _unitWork.InvoiceTran.Add(saveinvoicetran);
                                _unitWork.SaveChanges();

                                var saveinvoicejob = new InvoiceJob();
                                saveinvoicejob.It = saveinvoicetran.Id;
                                saveinvoicejob.ItNavigation = saveinvoicetran;
                                saveinvoicejob.Imid = saveinvoicemaster.Id;
                                saveinvoicejob.JobId = item.JobId;
                                saveinvoicejob.CreatedBy = GetInvoice.CreatedBy;
                                saveinvoicejob.CreatedUtc = DateTime.UtcNow;
                                saveinvoicejob.Jid = item.JId;
                                _unitWork.InvoiceJob.Add(saveinvoicejob);
                                if (!jidExists) _unitWork.SaveChanges();
                            }
                            else
                            {
                                var updateinvoicemaster = _unitWork.InvoiceMaster.FirstOrDefault(x => x.Id == invoicemasterId);
                                var itID = _unitWork.InvoiceTran.FirstOrDefault(x => x.Imid == invoicemasterId);
                                var checkexicting = _unitWork.InvoiceTran.FirstOrDefault(x => x.Imid == invoicemasterId && x.DepartmentId == item.DepartmentId && x.PricingTypeId == 5 || x.PricingTypeId == 15);
                                if (itID.PricingTypeId == 5 || itID.PricingTypeId == 15)
                                {

                                }
                                else
                                {
                                    if (checkexicting == null)
                                    {
                                        var saveinvoicetran = new InvoiceTran();
                                        saveinvoicetran.Imid = itID.Imid;
                                        saveinvoicetran.Value = sumofprice/countOfJobs;
                                        if (item.DepartmentId == 1)
                                        {
                                            saveinvoicetran.PricingTypeId = 5;
                                        }
                                        else if (item.DepartmentId == 2)
                                        {
                                            saveinvoicetran.PricingTypeId = 15;
                                        }
                                        saveinvoicetran.DepartmentId = item.DepartmentId;
                                        saveinvoicetran.DepartmentName = dapartmentname;
                                        saveinvoicetran.Createdby = GetInvoice.CreatedBy;
                                        saveinvoicetran.CreatedUtc = DateTime.UtcNow;
                                        _unitWork.InvoiceTran.Add(saveinvoicetran);
                                        _unitWork.SaveChanges();

                                        var totalTranRecords = new List<InvoiceTran>();
                                        var myCheckForIT = _unitWork.InvoiceTran.Where(x => x.Imid == updateinvoicemaster.Id && x.PricingTypeId == 4).OrderByDescending(x => x.Id).FirstOrDefault();
                                        var tranRecord = _unitWork.InvoiceTran.Local().ToList();
                                        var subQuery = tranRecord.Select(x => x.Id).ToList();
                                        var tranTotalRecordsWithIM = _unitWork.InvoiceTran.Where(x => !subQuery.Contains(x.Id) && x.Imid == updateinvoicemaster.Id).ToList();
                                        totalTranRecords = tranTotalRecordsWithIM.Concat(tranRecord).ToList();
                                        var totalInvoiceValue = totalTranRecords.Where(x => x.Imid == updateinvoicemaster.Id).ToList();
                                        var invoiceValue = totalInvoiceValue.Sum(x => x.Value);
                                        Decimal deciinv = Convert.ToDecimal(invoiceValue);
                                        updateinvoicemaster.ProductValue = invoiceValue;
                                        updateinvoicemaster.InvoiceValue = Math.Round(deciinv, 2);
                                        updateinvoicemaster.Discount = 0;
                                        updateinvoicemaster.UpdatedBy = GetInvoice.CreatedBy;
                                        updateinvoicemaster.UpdatedUtc = DateTime.UtcNow;
                                        updateinvoicemaster.PrimaryUpdatedBy = GetInvoice.CreatedBy;
                                        updateinvoicemaster.PrimaryDateTime = DateTime.UtcNow;

                                        var invoicetranDetails = _unitWork.InvoiceTran.Local().Where(x => x.Id == 0).OrderByDescending(x => x.Id).FirstOrDefault();
                                        if (invoicetranDetails == null && myCheckForIT != null)
                                        {
                                            invoicetranDetails = _unitWork.InvoiceTran.Local().OrderByDescending(x => x.Id == myCheckForIT.Id).FirstOrDefault();
                                        }
                                        var invoiceid = _unitWork.InvoiceMaster.Local().FirstOrDefault();
                                        updateinvoicemaster.Waiver = invoiceid.ArtWaiver + invoiceid.DigiWaiver;
                                        if (invoiceid.Waiver != null)
                                        {
                                            updateinvoicemaster.ReceivableValue = invoiceid.ProductValue - invoiceid.Waiver;
                                        }
                                        else
                                        {
                                            updateinvoicemaster.ReceivableValue = invoiceid.ProductValue;
                                        }
                                        Decimal reciroundoff = Convert.ToDecimal(invoiceid.ReceivableValue);
                                        updateinvoicemaster.InvoiceValue = Math.Round(reciroundoff, 2);
                                        updateinvoicemaster.TotalInvoiceValue = Math.Round(reciroundoff, 2);
                                        updateinvoicemaster.RoundOff = Math.Round(reciroundoff, 2) - invoiceid.ReceivableValue;
                                        _unitWork.InvoiceMaster.Update(updateinvoicemaster);
                                        _unitWork.SaveChanges();
                                    }
                                    else
                                    {

                                    }
                                }
                                var invoicejob = new InvoiceJob();
                                var updatetaninvoice = _unitWork.InvoiceTran.FirstOrDefault(x => x.Imid == invoicemasterId && x.PricingTypeId == 5 || x.PricingTypeId == 15);
                                if (checkexicting != null)
                                {
                                    var saveinvoicetran = new InvoiceTran();
                                    saveinvoicetran.Imid = itID.Imid;
                                    saveinvoicetran.Value = sumofprice / countOfJobs;
                                    if (item.DepartmentId == 1)
                                    {
                                        saveinvoicetran.PricingTypeId = 5;
                                    }
                                    else if (item.DepartmentId == 2)
                                    {
                                        saveinvoicetran.PricingTypeId = 15;
                                    }
                                    saveinvoicetran.DepartmentId = item.DepartmentId;
                                    saveinvoicetran.DepartmentName = dapartmentname;
                                    saveinvoicetran.Createdby = GetInvoice.CreatedBy;
                                    saveinvoicetran.CreatedUtc = DateTime.UtcNow;
                                    _unitWork.InvoiceTran.Add(saveinvoicetran);
                                    _unitWork.SaveChanges();

                                    var checkexicting1 = _unitWork.InvoiceTran.Where(x => x.Imid == invoicemasterId && x.DepartmentId == item.DepartmentId && (x.PricingTypeId == 5 || x.PricingTypeId == 15)).OrderByDescending(x => x.Id).FirstOrDefault();
                                    invoicejob.It = checkexicting1.Id;
                                    invoicejob.ItNavigation = checkexicting1;

                                    //updatetaninvoice.Value = sumofprice;
                                    var totalTranRecords = new List<InvoiceTran>();
                                    var myCheckForIT = _unitWork.InvoiceTran.Where(x => x.Imid == updateinvoicemaster.Id && x.PricingTypeId == 4).OrderByDescending(x => x.Id).FirstOrDefault();
                                    var tranRecord = _unitWork.InvoiceTran.Local().ToList();
                                    var subQuery = tranRecord.Select(x => x.Id).ToList();
                                    var tranTotalRecordsWithIM = _unitWork.InvoiceTran.Where(x => !subQuery.Contains(x.Id) && x.Imid == updateinvoicemaster.Id).ToList();
                                    totalTranRecords = tranTotalRecordsWithIM.Concat(tranRecord).ToList();
                                    var totalInvoiceValue = totalTranRecords.Where(x => x.Imid == updateinvoicemaster.Id).ToList();
                                    var invoiceValue = totalInvoiceValue.Sum(x => x.Value);
                                    Decimal deciinv = Convert.ToDecimal(invoiceValue);
                                    updateinvoicemaster.ProductValue = invoiceValue;
                                    updateinvoicemaster.InvoiceValue = Math.Round(deciinv, 2);
                                    updateinvoicemaster.Discount = 0;
                                    updateinvoicemaster.UpdatedBy = GetInvoice.CreatedBy;
                                    updateinvoicemaster.UpdatedUtc = DateTime.UtcNow;
                                    updateinvoicemaster.PrimaryUpdatedBy = GetInvoice.CreatedBy;
                                    updateinvoicemaster.PrimaryDateTime = DateTime.UtcNow;

                                    var invoicetranDetails = _unitWork.InvoiceTran.Local().Where(x => x.Id == 0).OrderByDescending(x => x.Id).FirstOrDefault();
                                    if (invoicetranDetails == null && myCheckForIT != null)
                                    {
                                        invoicetranDetails = _unitWork.InvoiceTran.Local().OrderByDescending(x => x.Id == myCheckForIT.Id).FirstOrDefault();
                                    }
                                    var invoiceid = _unitWork.InvoiceMaster.Local().FirstOrDefault();
                                    updateinvoicemaster.Waiver = invoiceid.ArtWaiver + invoiceid.DigiWaiver;
                                    if (invoiceid.Waiver != null)
                                    {
                                        updateinvoicemaster.ReceivableValue = invoiceid.ProductValue - invoiceid.Waiver;
                                    }
                                    else
                                    {
                                        updateinvoicemaster.ReceivableValue = invoiceid.ProductValue;
                                    }
                                    Decimal reciroundoff = Convert.ToDecimal(invoiceid.ReceivableValue);
                                    updateinvoicemaster.InvoiceValue = Math.Round(reciroundoff, 2);
                                    updateinvoicemaster.TotalInvoiceValue = Math.Round(reciroundoff, 2);
                                    updateinvoicemaster.RoundOff = Math.Round(reciroundoff, 2) - invoiceid.ReceivableValue;
                                    _unitWork.InvoiceMaster.Update(updateinvoicemaster);
                                    _unitWork.InvoiceTran.Update(updatetaninvoice);
                                    _unitWork.SaveChanges();
                                }
                                else
                                {
                                    invoicejob.It = invoicejob.Id;

                                }
                                invoicejob.Imid = itID.Imid;
                                invoicejob.JobId = item.JobId;
                                invoicejob.Jid = item.JId;
                                invoicejob.CreatedBy = GetInvoice.CreatedBy;
                                invoicejob.CreatedUtc = DateTime.UtcNow;
                                _unitWork.InvoiceJob.Add(invoicejob);
                                if (!jidExists) _unitWork.SaveChanges();
                            }
                        } //staffing if end
                        else
                        { // else start
                            if (item.DepartmentId == 2 && item.ScopeId == null)
                            {
                                item.ScopeId = 21;
                            }
                            var checkspecialpriceold = _unitWork.JobOrder.Where(x => x.Id == item.JId && x.IsDeleted == false && x.DateofUpload != null && x.SpecialPrice != null)
                                .Select(x => new { x.PricingTypeId, x.IsSpecialPrice, x.SpecialPrice, x.JobStatusId }).FirstOrDefault();
                            var getjobstatus = _unitWork.JobOrder.FirstOrDefault(x => x.Id == item.JId && x.IsDeleted == false);
                            var pricingWithScopeValues = _unitWork.PricingWithScope.Where(x => x.CustomerId == item.ClientId && x.DepartmentId == item.DepartmentId
                            && x.ScopeId == item.ScopeId && x.IsDelete != true && x.IsUpdated != true && x.JobStatusId == getjobstatus.JobStatusId && x.IsApproved == true &&
                            x.WefromDate.Value.Date <= item.FileReceivedDate.Date &&
                            (x.WetoDate.Value.Date >= item.FileReceivedDate.Date || x.WetoDate == null)).OrderByDescending(x => x.Id).ToList(); //

                            var checkchoosepricingtype = pricingWithScopeValues.Where(x => x.PricingTypeId != 2).Select(x => new { x.PricingTypeId, x.Price, x.RatePerHour }).FirstOrDefault();

                            int PricingTypeId = 0;
                            if (checkchoosepricingtype == null && checkspecialpriceold == null)
                            {
                                var customername = _unitWork.Customer.FirstOrDefault(x => x.Id == item.ClientId && x.IsDeleted == false).Name;
                                Message = "No Pricing for " + customername + " Customer.";
                            }
                            else
                            {
                                myCollection.Add(item);
                                var invoicemasterId = _unitWork.InvoiceMaster.Where(x => x.CustomerId == item.ClientId && x.InvoiceNo == null).Select(x => x.Id).FirstOrDefault();

                                if (checkspecialpriceold != null)
                                {
                                    //PricingTypeId = 2;
                                    if (checkspecialpriceold.IsSpecialPrice == true)
                                    {
                                        PricingTypeId = 2;
                                    }
                                    else if (checkspecialpriceold.IsSpecialPrice == false)
                                    {
                                        PricingTypeId = Convert.ToInt32(checkspecialpriceold.PricingTypeId);
                                    }
                                    else
                                    {
                                        PricingTypeId = checkchoosepricingtype.PricingTypeId;
                                    }
                                }
                                else
                                {
                                    PricingTypeId = checkchoosepricingtype.PricingTypeId;
                                }
                                //Pricing Based on Scope
                                if (PricingTypeId == 1 || PricingTypeId == 9)
                                {
                                    if (checkspecialpriceold != null)
                                    {
                                        qty = 1;
                                        rate = checkspecialpriceold.SpecialPrice.Value;
                                        productvalue = qty * rate;
                                    }
                                    else
                                    {
                                        qty = 1;
                                        rate = checkchoosepricingtype.Price.Value;
                                        productvalue = qty * checkchoosepricingtype.Price.Value;
                                    }
                                }
                                //Special Pricing
                                else if (PricingTypeId == 2)
                                {
                                    qty = 1;
                                    rate = checkspecialpriceold.SpecialPrice.Value;
                                    productvalue = qty * rate;
                                }
                                //Rate Based File count
                                else if (PricingTypeId == 3)
                                {
                                    var generateinvoie = _unitviewWork.ViewGetGenerateforInvoice.Where(x => x.ClientId == item.ClientId).Select(x => x.Id).ToList();
                                    if (generateinvoie.Count() >= 1)
                                    {
                                        var invoicemasteridinjob = (from invoiceJob in _tableWork.InvoiceJobRepository.Get()
                                                                    join invoiceMaster in _tableWork.InvoiceMasterRepository.Get()
                                                                    on invoiceJob.Imid equals invoiceMaster.Id
                                                                    join invoiceTran in _tableWork.InvoiceTranRepository.Get()
                                                                    on invoiceMaster.Id equals invoiceTran.Imid
                                                                    where (generateinvoie.Contains(invoiceJob.Jid) && invoiceMaster.InvoiceNo == null && invoiceTran.PricingTypeId == 3)
                                                                    select invoiceJob.Jid).ToList();

                                        var notInFirstTab = myCollection.Where(x => x.DepartmentId == item.DepartmentId && x.ClientId == item.ClientId && x.ScopeId == item.ScopeId && x.SpecialPrice == 0).Select(x => x.JId).ToList();
                                        if (invoicemasteridinjob.Count() >= 1)
                                        {
                                            var pricingforInvoice = _unitviewWork.ViewGetGenerateforInvoice.Where(x => invoicemasteridinjob.Contains(x.Id) && !notInFirstTab.Contains(x.Jid))
                                                .Select(x => new { x.ScopeId, x.DepartmentId, x.ClientId, x.FileReceivedDate, x.JobId, x.Jid }).ToList();
                                            foreach (var priceForInvoice in pricingforInvoice)
                                            {
                                                myCollection.Add(new GetInvoiceVM
                                                {
                                                    ScopeId = priceForInvoice.ScopeId,
                                                    DepartmentId = priceForInvoice.DepartmentId,
                                                    ClientId = Convert.ToInt32(priceForInvoice.ClientId),
                                                    SpecialPrice = 0,
                                                    FileReceivedDate = priceForInvoice.FileReceivedDate,
                                                    JobId = priceForInvoice.JobId,
                                                    JId = priceForInvoice.Jid
                                                });
                                            }
                                        }
                                    }
                                    var filecountrate = pricingWithScopeValues;
                                    var minFromDateCheck = filecountrate.Min(x => x.WefromDate);
                                    var minToDateCheck = filecountrate.Where(x => x.WefromDate == minFromDateCheck).FirstOrDefault();
                                    DateTime? checkForToDate = null;
                                    if (minToDateCheck != null)
                                    {
                                        checkForToDate = minToDateCheck.WetoDate;
                                    }
                                    var gettotalfilecountforartwork = myCollection;
                                    if (checkForToDate == null)
                                    {
                                        gettotalfilecountforartwork = myCollection.Where(x => x.DepartmentId == item.DepartmentId && x.ClientId == item.ClientId && x.ScopeId == item.ScopeId && x.SpecialPrice == 0 && (x.FileReceivedDate.Date >= minFromDateCheck.Value.Date)).ToList();
                                    }
                                    else
                                    {
                                        gettotalfilecountforartwork = myCollection.Where(x => x.DepartmentId == item.DepartmentId && x.ClientId == item.ClientId && x.ScopeId == item.ScopeId && x.SpecialPrice == 0 && (x.FileReceivedDate.Date >= minFromDateCheck.Value.Date) && (x.FileReceivedDate.Date <= checkForToDate.Value.Date)).ToList();
                                    }
                                    ////int JobCount = Return3Tab1Collection(GetInvoice, getjobstatus.JobStatusId);
                                    ////int? compareCount = JobCount;
                                    //int? compareCount = gettotalfilecountforartwork.Count(); // exact count replace
                                    ++gcount;
                                    var pricingWithScope = filecountrate.FirstOrDefault(x => x.JobStatusId == getjobstatus.JobStatusId && (x.From <= gcount && x.To >= gcount));
                                    if (pricingWithScope != null)
                                    {
                                        //int? myCheck = pricingWithScope.From;
                                        //if (myCheck == 1)
                                        //{
                                        //    qty = gettotalfilecountforartwork.Count;
                                        //}
                                        //else
                                        //{
                                        //    var totalCount = gettotalfilecountforartwork.Skip(Convert.ToInt32(myCheck - 1));
                                        //    qty = totalCount.Count();
                                        //}
                                        //rate = pricingWithScope.Price.Value;
                                        //productvalue = qty * pricingWithScope.Price.Value;
                                        qty = 1;
                                        rate = pricingWithScope.Price.Value;
                                        productvalue = qty * rate;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                    //int? compareCount = gettotalfilecountforartwork.Count(); // exact count replace
                                    //var pricingWithScope = filecountrate.FirstOrDefault(x => x.From <= compareCount && (x.To >= compareCount || x.To == null));
                                    //if (pricingWithScope != null)
                                    //{
                                    //    int? myCheck = pricingWithScope.From;
                                    //    if (myCheck == 1)
                                    //    {
                                    //        qty = gettotalfilecountforartwork.Count;
                                    //    }
                                    //    else
                                    //    {
                                    //        var totalCount = gettotalfilecountforartwork.Skip(Convert.ToInt32(myCheck - 1));
                                    //        qty = totalCount.Count();
                                    //    }
                                    //    rate = pricingWithScope.Price.Value;
                                    //    productvalue = qty * pricingWithScope.Price.Value;
                                    //}
                                    //else
                                    //{
                                    //    continue;
                                    //}
                                }
                                //price based on estimation time
                                else if (PricingTypeId == 11 || PricingTypeId == 12)
                                {
                                    if (checkspecialpriceold != null)
                                    {
                                        qty = 1;
                                        rate = checkspecialpriceold.SpecialPrice.Value;
                                        productvalue = qty * rate;
                                    }
                                    else
                                    {
                                        var price = _unitprocedure.GetPricingforTandM(item.ClientId, item.DepartmentId, item.ScopeId, PricingTypeId, getjobstatus.JobStatusId, item.EstimatedTime).FirstOrDefault();

                                        if (price != null)
                                        {
                                            decimal holdingprice = Convert.ToDecimal(price.Price);
                                            qty = 1;
                                            rate = holdingprice;
                                            productvalue = qty * rate;
                                        }
                                        else
                                        {
                                            var updatejoborder = _unitWork.JobOrder.FirstOrDefault(x => x.Id == item.JId);
                                            updatejoborder.SpecialPrice = null;
                                            updatejoborder.IsSpecialPrice = null;
                                            updatejoborder.IsQuatation = null;
                                            updatejoborder.PricingTypeId = null;
                                            _unitWork.JobOrder.Update(updatejoborder);
                                        }
                                    }
                                }
                                //Rate based on hour
                                else if (PricingTypeId == 13 || PricingTypeId == 14)
                                {
                                    if (checkspecialpriceold != null)
                                    {
                                        qty = 1;
                                        rate = checkspecialpriceold.SpecialPrice.Value;
                                        productvalue = qty * rate;
                                    }
                                    else
                                    {
                                        //var ratevalue = _unitprocedure.GetRateforTandM(item.ClientId, item.DepartmentId, item.ScopeId, PricingTypeId).Select(x => x.RatePerHour).FirstOrDefault();
                                        var ratevalue = _unitprocedure.GetRateforTandM(item.ClientId, item.DepartmentId, item.ScopeId, PricingTypeId, getjobstatus.JobStatusId).FirstOrDefault();

                                        if (ratevalue != null)
                                        {
                                            decimal estime = Convert.ToDecimal(item.EstimatedTime);
                                            decimal holdingrate = Convert.ToDecimal(ratevalue.RatePerHour);
                                            qty = 1;
                                            rate = estime / 60 * holdingrate;
                                            productvalue = qty * rate;
                                        }
                                        else
                                        {
                                            var updatejoborder = _unitWork.JobOrder.FirstOrDefault(x => x.Id == item.JId);
                                            updatejoborder.SpecialPrice = null;
                                            updatejoborder.IsSpecialPrice = null;
                                            updatejoborder.IsQuatation = null;
                                            updatejoborder.PricingTypeId = null;
                                            _unitWork.JobOrder.Update(updatejoborder);
                                        }
                                    }
                                }
                                //Rate consession based on File Count
                                else if (PricingTypeId == 4)
                                {
                                    var generateinvoie = _unitviewWork.ViewGetGenerateforInvoice.Where(x => x.ClientId == item.ClientId).Select(x => x.Id).ToList();
                                    if (generateinvoie.Count() >= 1)
                                    {
                                        var invoicemasteridinjob = (from invoiceJob in _tableWork.InvoiceJobRepository.Get()
                                                                    join invoiceMaster in _tableWork.InvoiceMasterRepository.Get()
                                                                    on invoiceJob.Imid equals invoiceMaster.Id
                                                                    join invoiceTran in _tableWork.InvoiceTranRepository.Get()
                                                                    on invoiceMaster.Id equals invoiceTran.Imid
                                                                    where (generateinvoie.Contains(invoiceJob.Jid) && invoiceMaster.InvoiceNo == null && invoiceTran.PricingTypeId == 4)
                                                                    select invoiceJob.Jid).ToList();


                                        var notInFirstTab = myCollection.Where(x => x.DepartmentId == item.DepartmentId && x.ClientId == item.ClientId && x.ScopeId == item.ScopeId && x.SpecialPrice == 0).Select(x => x.JId).ToList();
                                        if (invoicemasteridinjob.Count() >= 1)
                                        {
                                            var pricingforInvoice = _unitviewWork.ViewGetGenerateforInvoice.Where(x => invoicemasteridinjob.Contains(x.Id) && !notInFirstTab.Contains(x.Jid))
                                                .Select(x => new { x.ScopeId, x.DepartmentId, x.ClientId, x.FileReceivedDate, x.JobId, x.Jid }).ToList();
                                            foreach (var priceForInvoice in pricingforInvoice)
                                            {
                                                myCollection.Add(new GetInvoiceVM
                                                {
                                                    ScopeId = priceForInvoice.ScopeId,
                                                    DepartmentId = priceForInvoice.DepartmentId,
                                                    ClientId = Convert.ToInt32(priceForInvoice.ClientId),
                                                    SpecialPrice = 0,
                                                    FileReceivedDate = priceForInvoice.FileReceivedDate,
                                                    JobId = priceForInvoice.JobId,
                                                    JId = priceForInvoice.Jid
                                                });
                                            }
                                        }
                                    }
                                    var filecountrate = pricingWithScopeValues;
                                    var minFromDateCheck = filecountrate.Min(x => x.WefromDate);// min ??
                                    var minToDateCheck = filecountrate.Where(x => x.WefromDate == minFromDateCheck).FirstOrDefault();
                                    DateTime? checkForToDate = null;
                                    if (minToDateCheck != null)
                                    {
                                        checkForToDate = minToDateCheck.WetoDate;
                                    }
                                    var gettotalfilecountforartwork = myCollection;
                                    if (checkForToDate == null)
                                    {
                                        gettotalfilecountforartwork = myCollection.Where(x => x.DepartmentId == item.DepartmentId && x.ClientId == item.ClientId && x.ScopeId == item.ScopeId && x.SpecialPrice == 0 && (x.FileReceivedDate.Date >= minFromDateCheck.Value.Date)).ToList();
                                    }
                                    else
                                    {
                                        gettotalfilecountforartwork = myCollection.Where(x => x.DepartmentId == item.DepartmentId && x.ClientId == item.ClientId && x.ScopeId == item.ScopeId && x.SpecialPrice == 0 && (x.FileReceivedDate.Date >= minFromDateCheck.Value.Date) && (x.FileReceivedDate.Date <= checkForToDate.Value.Date)).ToList();
                                    }
                                    var minto = filecountrate.Min(x => x.To);
                                    //
                                    int JobCount = Return4Tab1Collection(GetInvoice, getjobstatus.JobStatusId);
                                    int? compareCount = JobCount;
                                    //
                                    //int? compareCount = gettotalfilecountforartwork.Count(); // need to take exact 4rth PT Id jobs count
                                    //var pricingWithScope = filecountrate.FirstOrDefault(x => x.From <= compareCount && (x.To >= compareCount || x.To == null));
                                    var pricingWithScope = filecountrate.Where(x => x.From <= compareCount && (x.To >= compareCount || x.To == null)).ToList();
                                    var nearestPrice = pricingWithScope.OrderBy(x => x.To).FirstOrDefault();
                                    if (pricingWithScope != null)
                                    {
                                        qty = gettotalfilecountforartwork.Count;
                                        //rate = pricingWithScope.Price.Value;
                                        //productvalue = qty * pricingWithScope.Price.Value;
                                        rate = nearestPrice.Price.Value;
                                        productvalue = qty * nearestPrice.Price.Value;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else if (PricingTypeId == 16 || PricingTypeId == 17)
                                {
                                    PricingWithScopeViewModel pricingViewModel = new PricingWithScopeViewModel();
                                    pricingViewModel.PricingTypeId = item.PricingTypeId;
                                    pricingViewModel.ScopeId = (int)item.ScopeId;
                                    pricingViewModel.JobStatusId = (int)item.JobStatusId;
                                    pricingViewModel.CustomerId = item.ClientId;

                                    var pricing = _unitWork.JobOrder
                                    .Where(f => f.JobId == item.JobId && f.IsDeleted == false)
                                    .ToList();
                                    var fileCount = pricing.Count;
                                    fileCounts += fileCount;

                                    if (fileCounts != null)
                                    {
                                        if (item.DepartmentId == 2)
                                        {
                                            rate = CalculateSlabBasedPrice(fileCounts, pricingViewModel);
                                            var stitchcount = _unitWork.JobOrder
                                            .Where(x => x.Id == item.JId && x.IsDeleted == false)
                                            .Select(x => x.StitchCount)
                                            .FirstOrDefault();

                                            float getworkedcount = Convert.ToInt64(stitchcount.Value);

                                            qty = 1;
                                            float totalStitchCount = ((getworkedcount) / (1000));
                                            decimal getrate = Convert.ToDecimal(totalStitchCount) * rate;
                                            if (getrate >= checkchoosepricingtype.RatePerHour)
                                            {
                                                qty = 1;
                                                rate = checkchoosepricingtype.RatePerHour.Value;
                                                productvalue = rate;
                                            }
                                            else
                                            {
                                                qty = 1;
                                                rate = getrate;
                                                productvalue = rate;
                                            }
                                            productvalue = qty * rate;
                                        }
                                        else
                                        {
                                            rate = CalculateSlabBasedPrice(fileCounts, pricingViewModel);
                                            qty = 1;
                                            productvalue = qty * rate;
                                        }
                                    }
                                }
                                //Rate Based On Stitch Count
                                else if (PricingTypeId == 6)
                                {
                                    if (checkspecialpriceold != null)
                                    {
                                        qty = 1;
                                        rate = checkspecialpriceold.SpecialPrice.Value;
                                        productvalue = qty * rate;
                                    }
                                    else
                                    {
                                        var stitchcount = _unitWork.JobOrder.Where(x => x.Id == item.JId).Select(x => x.StitchCount).FirstOrDefault();
                                        float getworkedcount = Convert.ToInt64(stitchcount.Value);
                                        if (getworkedcount <= 1000)
                                        {
                                            qty = 1;
                                            rate = checkchoosepricingtype.Price.Value;
                                            productvalue = rate;
                                        }
                                        else
                                        {
                                            float GetTotalstitchcount = ((getworkedcount) / (1000));
                                            decimal getrate = Convert.ToDecimal(GetTotalstitchcount) * checkchoosepricingtype.Price.Value;
                                            if (getrate >= checkchoosepricingtype.RatePerHour)
                                            {
                                                qty = 1;
                                                rate = checkchoosepricingtype.RatePerHour.Value;
                                                productvalue = rate;
                                            }
                                            else
                                            {
                                                qty = 1;
                                                rate = getrate;
                                                productvalue = rate;
                                            }
                                        }
                                    }
                                }
                                //Rate based file count
                                else if (PricingTypeId == 10)
                                {
                                    var generateinvoie = _unitviewWork.ViewGetGenerateforInvoice.Where(x => x.ClientId == item.ClientId).Select(x => x.Id).ToList();
                                    if (generateinvoie.Count() >= 1)
                                    {
                                        var invoicemasteridinjob = (from invoiceJob in _tableWork.InvoiceJobRepository.Get()
                                                                    join invoiceMaster in _tableWork.InvoiceMasterRepository.Get()
                                                                    on invoiceJob.Imid equals invoiceMaster.Id
                                                                    join invoiceTran in _tableWork.InvoiceTranRepository.Get()
                                                                    on invoiceMaster.Id equals invoiceTran.Imid
                                                                    where (generateinvoie.Contains(invoiceJob.Jid) && invoiceMaster.InvoiceNo == null && invoiceTran.PricingTypeId == 10)
                                                                    select invoiceJob.Jid).ToList();

                                        var notInFirstTab = myCollection.Where(x => x.DepartmentId == item.DepartmentId && x.ClientId == item.ClientId && x.ScopeId == item.ScopeId && x.SpecialPrice == 0).Select(x => x.JId).ToList();
                                        if (invoicemasteridinjob.Count() >= 1)
                                        {
                                            var pricingforInvoice = _unitviewWork.ViewGetGenerateforInvoice.Where(x => invoicemasteridinjob.Contains(x.Id) && !notInFirstTab.Contains(x.Jid))
                                                .Select(x => new { x.ScopeId, x.DepartmentId, x.ClientId, x.FileReceivedDate, x.JobId, x.Jid }).ToList();
                                            foreach (var priceForInvoice in pricingforInvoice)
                                            {
                                                myCollection.Add(new GetInvoiceVM
                                                {
                                                    ScopeId = priceForInvoice.ScopeId,
                                                    DepartmentId = priceForInvoice.DepartmentId,
                                                    ClientId = Convert.ToInt32(priceForInvoice.ClientId),
                                                    SpecialPrice = 0,
                                                    FileReceivedDate = priceForInvoice.FileReceivedDate,
                                                    JobId = priceForInvoice.JobId,
                                                    JId = priceForInvoice.Jid
                                                });
                                            }
                                        }
                                    }
                                    var filecountrate = pricingWithScopeValues;
                                    var minFromDateCheck = filecountrate.Min(x => x.WefromDate);
                                    var minToDateCheck = filecountrate.Where(x => x.WefromDate == minFromDateCheck).FirstOrDefault();
                                    DateTime? checkForToDate = null;
                                    if (minToDateCheck != null)
                                    {
                                        checkForToDate = minToDateCheck.WetoDate;
                                    }
                                    var gettotalfilecountforartwork = myCollection;
                                    if (checkForToDate == null)
                                    {
                                        gettotalfilecountforartwork = myCollection.Where(x => x.DepartmentId == item.DepartmentId && x.ClientId == item.ClientId && x.ScopeId == item.ScopeId && x.SpecialPrice == 0 && (x.FileReceivedDate.Date >= minFromDateCheck.Value.Date)).ToList();
                                    }
                                    else
                                    {
                                        gettotalfilecountforartwork = myCollection.Where(x => x.DepartmentId == item.DepartmentId && x.ClientId == item.ClientId && x.ScopeId == item.ScopeId && x.SpecialPrice == 0 && (x.FileReceivedDate.Date >= minFromDateCheck.Value.Date) && (x.FileReceivedDate.Date <= checkForToDate.Value.Date)).ToList();
                                    }

                                    // bug fixing 
                                    ++gcount;
                                    var pricingWithScope = filecountrate.FirstOrDefault(x => x.JobStatusId == getjobstatus.JobStatusId && (x.From <= gcount && x.To >= gcount));
                                    if (pricingWithScope != null)
                                    {
                                        rate = pricingWithScope.Price.Value;
                                        float workedcount = 0;
                                        var stitchcount = _unitWork.JobOrder.Where(x => x.Id == item.JId).Select(x => x.StitchCount).FirstOrDefault();
                                        workedcount = Convert.ToInt64(stitchcount.Value);
                                        float Totalstitchcount = ((workedcount) / (1000));
                                        decimal getrate = Convert.ToDecimal(Totalstitchcount) * rate;
                                        qty = 1;
                                        productvalue = qty * getrate;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                    // bug fixing

                                    //int? compareCount = gettotalfilecountforartwork.Count(); //exact count to replace
                                    //var pricingWithScope = filecountrate.FirstOrDefault(x => x.From <= compareCount && (x.To >= compareCount || x.To == null));
                                    //if (pricingWithScope != null)
                                    //{
                                    //    rate = pricingWithScope.Price.Value;
                                    //    float workedcount = 0;
                                    //    var stitchcount = _unitWork.JobOrder.Where(x => x.Id == item.JId).Select(x => x.StitchCount).FirstOrDefault();
                                    //    workedcount = Convert.ToInt64(stitchcount.Value);
                                    //    float Totalstitchcount = ((workedcount) / (1000));
                                    //    decimal getrate = Convert.ToDecimal(Totalstitchcount) * rate;
                                    //    qty = 1;
                                    //    productvalue = qty * getrate;
                                    //}
                                    //else
                                    //{
                                    //    continue;
                                    //}


                                }
                                var dapartmentname = _unitWork.Department.FirstOrDefault(x => x.Id == item.DepartmentId && x.IsDeleted == false).Description;
                                var cusvsscope = _unitWork.CustomerVsScope.FirstOrDefault(x => x.ScopeId == item.ScopeId.Value);
                                if (invoicemasterId == null || invoicemasterId == 0)
                                {
                                    var saveinvoicemaster = new InvoiceMaster();
                                    saveinvoicemaster.ReferenceDate = DateTime.UtcNow;
                                    saveinvoicemaster.ReferenceNo = "123";
                                    saveinvoicemaster.CustomerId = item.ClientId;
                                    if (item.DepartmentId == 1)
                                    {
                                        if (jobord.IsWaiver == true)
                                        {
                                            saveinvoicemaster.ArtInvoiceAmount = productvalue;
                                            saveinvoicemaster.DigiInvoiceAmount = 0;
                                            saveinvoicemaster.ArtWaiver = productvalue;
                                            saveinvoicemaster.DigiWaiver = 0;
                                            //saveinvoicemaster.Waiver = productvalue;
                                        }
                                        else
                                        {
                                            saveinvoicemaster.ArtInvoiceAmount = productvalue;
                                            saveinvoicemaster.DigiInvoiceAmount = 0;
                                            saveinvoicemaster.ArtWaiver = 0;
                                            saveinvoicemaster.DigiWaiver = 0;
                                        }
                                    }
                                    else if (item.DepartmentId == 2)
                                    {
                                        if (jobord.IsWaiver == true)
                                        {
                                            saveinvoicemaster.ArtInvoiceAmount = 0;
                                            saveinvoicemaster.DigiInvoiceAmount = productvalue;
                                            saveinvoicemaster.DigiWaiver = productvalue;
                                            saveinvoicemaster.ArtWaiver = 0;
                                            //saveinvoicemaster.Waiver = productvalue;
                                        }
                                        else
                                        {
                                            saveinvoicemaster.DigiInvoiceAmount = productvalue;
                                            saveinvoicemaster.ArtInvoiceAmount = 0;
                                            saveinvoicemaster.ArtWaiver = 0;
                                            saveinvoicemaster.DigiWaiver = 0;
                                        }
                                    }
                                    saveinvoicemaster.ProductValue = productvalue;
                                    saveinvoicemaster.Discount = 0;
                                    saveinvoicemaster.Waiver = saveinvoicemaster.ArtWaiver + saveinvoicemaster.DigiWaiver;
                                    saveinvoicemaster.ReceivableValue = saveinvoicemaster.ProductValue - saveinvoicemaster.Waiver;
                                    decimal invoicevalue = Convert.ToDecimal(saveinvoicemaster.ReceivableValue);
                                    saveinvoicemaster.RoundOff = Math.Round(invoicevalue, 2) - saveinvoicemaster.ReceivableValue;
                                    saveinvoicemaster.InvoiceValue = Math.Round(invoicevalue, 2);
                                    saveinvoicemaster.TotalInvoiceValue = Math.Round(invoicevalue, 2);
                                    saveinvoicemaster.IsDeleted = false;
                                    saveinvoicemaster.IsCancelled = false;
                                    saveinvoicemaster.IsSample = false;
                                    saveinvoicemaster.CreatedBy = GetInvoice.CreatedBy;
                                    saveinvoicemaster.CreatedUtc = DateTime.UtcNow;
                                    saveinvoicemaster.IsOutstanding = true;
                                    saveinvoicemaster.PrimaryUpdatedBy = GetInvoice.CreatedBy;
                                    saveinvoicemaster.PrimaryDateTime = DateTime.UtcNow;
                                    _unitWork.InvoiceMaster.Add(saveinvoicemaster);
                                    _unitWork.SaveChanges();

                                    var saveinvoicetran = new InvoiceTran();
                                    saveinvoicetran.Imid = saveinvoicemaster.Id;
                                    saveinvoicetran.Qty = qty;
                                    saveinvoicetran.Rate = rate;

                                    if (PricingTypeId == 10)
                                    {
                                        saveinvoicetran.Rate = productvalue;
                                    }
                                    saveinvoicetran.Value = productvalue;
                                    saveinvoicetran.PricingTypeId = PricingTypeId;
                                    if (item.DepartmentId == 1)
                                    {
                                        if (cusvsscope == null)
                                        {
                                            saveinvoicetran.ScopeId = item.ScopeId.Value;
                                        }
                                        else
                                        {
                                            saveinvoicetran.ScopeId = item.ScopeId.Value;
                                            saveinvoicetran.ScopeGroupId = cusvsscope.ScopeGroupId;
                                            saveinvoicetran.ScopeGroupDescription = cusvsscope.ScopeGroupDescription;
                                        }
                                    }
                                    else if (item.DepartmentId == 2)
                                    {
                                        if (cusvsscope == null)
                                        {
                                            saveinvoicetran.ScopeId = item.ScopeId.Value;
                                        }
                                        else
                                        {
                                            saveinvoicetran.ScopeId = item.ScopeId.Value;
                                            saveinvoicetran.ScopeGroupId = cusvsscope.ScopeGroupId;
                                            saveinvoicetran.ScopeGroupDescription = cusvsscope.ScopeGroupDescription;
                                        }
                                    }
                                    saveinvoicetran.DepartmentId = item.DepartmentId;
                                    saveinvoicetran.DepartmentName = dapartmentname;
                                    saveinvoicetran.Createdby = GetInvoice.CreatedBy;
                                    saveinvoicetran.CreatedUtc = DateTime.UtcNow;
                                    _unitWork.InvoiceTran.Add(saveinvoicetran);
                                    _unitWork.SaveChanges();

                                    var saveinvoicejob = new InvoiceJob();
                                    saveinvoicejob.It = saveinvoicetran.Id;
                                    saveinvoicejob.ItNavigation = saveinvoicetran;
                                    saveinvoicejob.Imid = saveinvoicemaster.Id;
                                    saveinvoicejob.JobId = item.JobId;
                                    saveinvoicejob.CreatedBy = GetInvoice.CreatedBy;
                                    saveinvoicejob.CreatedUtc = DateTime.UtcNow;
                                    saveinvoicejob.Jid = item.JId;
                                    _unitWork.InvoiceJob.Add(saveinvoicejob);
                                    if (!jidExists) _unitWork.SaveChanges();
                                }
                                else
                                {
                                    var updateinvoicemaster = _unitWork.InvoiceMaster.FirstOrDefault(x => x.Id == invoicemasterId);

                                    var totalTranRecords = new List<InvoiceTran>();
                                    //Harsha removed rate for this query because, for rate concession, 1-5 jobs 5$, 1-10 jobs 7$ means, total jobs have to affect 7$, so all time update willl be there
                                    var myCheckForIT = _unitWork.InvoiceTran.Where(x => x.Imid == updateinvoicemaster.Id && x.ScopeId == item.ScopeId.Value && x.PricingTypeId == 4 || (x.PricingTypeId == 16 || x.PricingTypeId == 17)).OrderByDescending(x => x.Id).FirstOrDefault();
                                    //For 4th type working fine startes
                                    if (PricingTypeId == 4)
                                    {
                                        var filecountrate = pricingWithScopeValues;
                                        var minFromDateCheck = filecountrate.Min(x => x.WefromDate);
                                        //this part added for second tab issue resolving...
                                        var minToDateCheck = filecountrate.Where(x => x.WefromDate == minFromDateCheck).FirstOrDefault();
                                        DateTime? checkForToDate = null;
                                        if (minToDateCheck != null)
                                        {
                                            checkForToDate = minToDateCheck.WetoDate;
                                        }
                                        var gettotalfilecountforartwork = myCollection.ToList();
                                        if (checkForToDate == null)
                                        {
                                            gettotalfilecountforartwork = myCollection.Where(x => x.DepartmentId == item.DepartmentId && x.ClientId == item.ClientId && x.ScopeId == item.ScopeId && x.SpecialPrice == 0 && (x.FileReceivedDate.Date >= minFromDateCheck.Value.Date)).ToList();
                                        }
                                        else
                                        {
                                            gettotalfilecountforartwork = myCollection.Where(x => x.DepartmentId == item.DepartmentId && x.ClientId == item.ClientId && x.ScopeId == item.ScopeId && x.SpecialPrice == 0 && (x.FileReceivedDate.Date >= minFromDateCheck.Value.Date) && (x.FileReceivedDate.Date <= checkForToDate.Value.Date)).ToList();
                                        }
                                        // this part for second tab issue solving
                                        //For 4th type always update only will happen
                                        if (gettotalfilecountforartwork.Count() != 1 && gettotalfilecountforartwork.Count() != 0)
                                        {
                                            var totalRecords = gettotalfilecountforartwork.Where(x => x.JId != null).ToList().Select(x => x.JId);
                                            var tranId = _unitWork.InvoiceJob.Where(x => totalRecords.Contains(x.Jid) && x.It != null).FirstOrDefault();
                                            myCheckForIT = _unitWork.InvoiceTran.FirstOrDefault(x => x.Id == tranId.It);
                                            myCheckForIT.Imid = updateinvoicemaster.Id;
                                            myCheckForIT.Qty = qty;
                                            myCheckForIT.Rate = rate;
                                            myCheckForIT.Value = productvalue;
                                            myCheckForIT.PricingTypeId = PricingTypeId;
                                            if (item.DepartmentId == 1)
                                            {
                                                if (cusvsscope == null)
                                                {
                                                    myCheckForIT.ScopeId = item.ScopeId.Value;
                                                }
                                                else
                                                {
                                                    myCheckForIT.ScopeId = item.ScopeId.Value;
                                                    myCheckForIT.ScopeGroupId = cusvsscope.ScopeGroupId;
                                                    myCheckForIT.ScopeGroupDescription = cusvsscope.ScopeGroupDescription;
                                                }
                                            }
                                            else if (item.DepartmentId == 2)
                                            {
                                                if (cusvsscope == null)
                                                {
                                                    myCheckForIT.ScopeId = item.ScopeId.Value;
                                                }
                                                else
                                                {
                                                    myCheckForIT.ScopeId = item.ScopeId.Value;
                                                    myCheckForIT.ScopeGroupId = cusvsscope.ScopeGroupId;
                                                    myCheckForIT.ScopeGroupDescription = cusvsscope.ScopeGroupDescription;
                                                }
                                            }
                                            myCheckForIT.UpdatedBy = GetInvoice.CreatedBy;
                                            myCheckForIT.UpdatedUtc = DateTime.UtcNow;
                                            _unitWork.InvoiceTran.Update(myCheckForIT);
                                        }
                                        else
                                        {
                                            var saveinvoicetran = new InvoiceTran();
                                            saveinvoicetran.Imid = updateinvoicemaster.Id;
                                            saveinvoicetran.Qty = qty;
                                            saveinvoicetran.Rate = rate;
                                            saveinvoicetran.Value = qty * productvalue;
                                            saveinvoicetran.PricingTypeId = PricingTypeId;
                                            if (item.DepartmentId == 1)
                                            {
                                                if (cusvsscope == null)
                                                {
                                                    saveinvoicetran.ScopeId = item.ScopeId.Value;
                                                }
                                                else
                                                {
                                                    saveinvoicetran.ScopeId = item.ScopeId.Value;
                                                    saveinvoicetran.ScopeGroupId = cusvsscope.ScopeGroupId;
                                                    saveinvoicetran.ScopeGroupDescription = cusvsscope.ScopeGroupDescription;
                                                }
                                            }
                                            else if (item.DepartmentId == 2)
                                            {
                                                if (cusvsscope == null)
                                                {
                                                    saveinvoicetran.ScopeId = item.ScopeId.Value;
                                                }
                                                else
                                                {
                                                    saveinvoicetran.ScopeId = item.ScopeId.Value;
                                                    saveinvoicetran.ScopeGroupId = cusvsscope.ScopeGroupId;
                                                    saveinvoicetran.ScopeGroupDescription = cusvsscope.ScopeGroupDescription;
                                                }
                                            }
                                            saveinvoicetran.DepartmentId = item.DepartmentId;
                                            saveinvoicetran.DepartmentName = dapartmentname;
                                            saveinvoicetran.Createdby = GetInvoice.CreatedBy;
                                            saveinvoicetran.CreatedUtc = DateTime.UtcNow;
                                            _unitWork.InvoiceTran.Add(saveinvoicetran);
                                        }
                                    }
                                    else if (PricingTypeId == 3)
                                    {
                                        myCheckForIT = _unitWork.InvoiceTran.Where(x => x.Imid == updateinvoicemaster.Id && x.ScopeId == item.ScopeId.Value && x.Rate == rate && x.PricingTypeId == 3).OrderByDescending(x => x.Id).FirstOrDefault();
                                        if (myCheckForIT == null)
                                        {
                                            var saveinvoicetran = new InvoiceTran();
                                            saveinvoicetran.Imid = updateinvoicemaster.Id;
                                            saveinvoicetran.Qty = qty;
                                            saveinvoicetran.Rate = rate;
                                            saveinvoicetran.Value = productvalue;
                                            saveinvoicetran.PricingTypeId = PricingTypeId;
                                            if (item.DepartmentId == 1)
                                            {
                                                if (cusvsscope == null)
                                                {
                                                    saveinvoicetran.ScopeId = item.ScopeId.Value;
                                                }
                                                else
                                                {
                                                    saveinvoicetran.ScopeId = item.ScopeId.Value;
                                                    saveinvoicetran.ScopeGroupId = cusvsscope.ScopeGroupId;
                                                    saveinvoicetran.ScopeGroupDescription = cusvsscope.ScopeGroupDescription;
                                                }
                                            }
                                            else if (item.DepartmentId == 2)
                                            {
                                                if (cusvsscope == null)
                                                {
                                                    saveinvoicetran.ScopeId = item.ScopeId.Value;
                                                }
                                                else
                                                {
                                                    saveinvoicetran.ScopeId = item.ScopeId.Value;
                                                    saveinvoicetran.ScopeGroupId = cusvsscope.ScopeGroupId;
                                                    saveinvoicetran.ScopeGroupDescription = cusvsscope.ScopeGroupDescription;
                                                }
                                            }
                                            saveinvoicetran.DepartmentId = item.DepartmentId;
                                            saveinvoicetran.DepartmentName = dapartmentname;
                                            saveinvoicetran.Createdby = GetInvoice.CreatedBy;
                                            saveinvoicetran.CreatedUtc = DateTime.UtcNow;
                                            _unitWork.InvoiceTran.Add(saveinvoicetran);
                                        }
                                        else
                                        {
                                            var updateinvoicetran = _unitWork.InvoiceTran.FirstOrDefault(x => x.Id == myCheckForIT.Id);
                                            updateinvoicetran.Imid = updateinvoicemaster.Id;
                                            updateinvoicetran.Qty = qty;
                                            updateinvoicetran.Rate = rate;
                                            updateinvoicetran.Value = productvalue;
                                            updateinvoicetran.PricingTypeId = PricingTypeId;
                                            if (item.DepartmentId == 1)
                                            {
                                                if (cusvsscope == null)
                                                {
                                                    updateinvoicetran.ScopeId = item.ScopeId.Value;
                                                }
                                                else
                                                {
                                                    updateinvoicetran.ScopeId = item.ScopeId.Value;
                                                    updateinvoicetran.ScopeGroupId = cusvsscope.ScopeGroupId;
                                                    updateinvoicetran.ScopeGroupDescription = cusvsscope.ScopeGroupDescription;
                                                }
                                            }
                                            else if (item.DepartmentId == 2)
                                            {
                                                if (cusvsscope == null)
                                                {
                                                    updateinvoicetran.ScopeId = item.ScopeId.Value;
                                                }
                                                else
                                                {
                                                    updateinvoicetran.ScopeId = item.ScopeId.Value;
                                                    updateinvoicetran.ScopeGroupId = cusvsscope.ScopeGroupId;
                                                    updateinvoicetran.ScopeGroupDescription = cusvsscope.ScopeGroupDescription;
                                                }
                                            }
                                            updateinvoicetran.UpdatedBy = GetInvoice.CreatedBy;
                                            updateinvoicetran.UpdatedUtc = DateTime.UtcNow;
                                            _unitWork.InvoiceTran.Update(updateinvoicetran);
                                        }
                                    }
                                    else
                                    {
                                        qty = 1;
                                        var saveinvoicetran = new InvoiceTran();
                                        saveinvoicetran.Imid = updateinvoicemaster.Id;
                                        saveinvoicetran.Qty = qty;
                                        saveinvoicetran.Rate = rate;
                                        if (PricingTypeId == 10)
                                        {
                                            saveinvoicetran.Rate = productvalue;
                                        }
                                        saveinvoicetran.Value = qty * rate;
                                        saveinvoicetran.PricingTypeId = PricingTypeId;
                                        if (item.DepartmentId == 1)
                                        {
                                            if (cusvsscope == null)
                                            {
                                                saveinvoicetran.ScopeId = item.ScopeId.Value;
                                            }
                                            else
                                            {
                                                saveinvoicetran.ScopeId = item.ScopeId.Value;
                                                saveinvoicetran.ScopeGroupId = cusvsscope.ScopeGroupId;
                                                saveinvoicetran.ScopeGroupDescription = cusvsscope.ScopeGroupDescription;
                                            }
                                        }
                                        else if (item.DepartmentId == 2)
                                        {
                                            if (cusvsscope == null)
                                            {
                                                saveinvoicetran.ScopeId = item.ScopeId.Value;
                                            }
                                            else
                                            {
                                                saveinvoicetran.ScopeId = item.ScopeId.Value;
                                                saveinvoicetran.ScopeGroupId = cusvsscope.ScopeGroupId;
                                                saveinvoicetran.ScopeGroupDescription = cusvsscope.ScopeGroupDescription;
                                            }
                                        }
                                        saveinvoicetran.DepartmentId = item.DepartmentId;
                                        saveinvoicetran.DepartmentName = dapartmentname;
                                        saveinvoicetran.Createdby = GetInvoice.CreatedBy;
                                        saveinvoicetran.CreatedUtc = DateTime.UtcNow;
                                        _unitWork.InvoiceTran.Add(saveinvoicetran);
                                        _unitWork.SaveChanges();

                                        var invoicetranDetails = _unitWork.InvoiceTran.Local().Where(x => x.Id == 0).OrderByDescending(x => x.Id).FirstOrDefault();
                                        if (invoicetranDetails == null && myCheckForIT != null)
                                        {
                                            invoicetranDetails = _unitWork.InvoiceTran.Local().OrderByDescending(x => x.Id == saveinvoicetran.Id).FirstOrDefault();
                                        }
                                        var saveinvoicejob = new InvoiceJob();
                                        saveinvoicejob.It = invoicetranDetails.Id;
                                        saveinvoicejob.ItNavigation = invoicetranDetails;
                                        saveinvoicejob.Imid = updateinvoicemaster.Id;
                                        saveinvoicejob.JobId = item.JobId;
                                        saveinvoicejob.Jid = item.JId;
                                        saveinvoicejob.CreatedBy = GetInvoice.CreatedBy;
                                        saveinvoicejob.CreatedUtc = DateTime.UtcNow;
                                        _unitWork.InvoiceJob.Add(saveinvoicejob);
                                    }
                                    var tranRecord = _unitWork.InvoiceTran.Local().ToList();
                                    var subQuery = tranRecord.Select(x => x.Id).ToList();
                                    var tranTotalRecordsWithIM = _unitWork.InvoiceTran.Where(x => !subQuery.Contains(x.Id) && x.Imid == updateinvoicemaster.Id).ToList();
                                    totalTranRecords = tranTotalRecordsWithIM.Concat(tranRecord).ToList();

                                    var totalInvoiceValue = totalTranRecords.Where(x => x.Imid == updateinvoicemaster.Id).ToList();
                                    var invoiceValue = totalInvoiceValue.Sum(x => x.Value);

                                    Decimal deciinv = Convert.ToDecimal(invoiceValue);
                                    updateinvoicemaster.ProductValue = invoiceValue;
                                    updateinvoicemaster.InvoiceValue = Math.Round(deciinv, 2);
                                    updateinvoicemaster.Discount = 0;
                                    updateinvoicemaster.UpdatedBy = GetInvoice.CreatedBy;
                                    updateinvoicemaster.UpdatedUtc = DateTime.UtcNow;
                                    updateinvoicemaster.PrimaryUpdatedBy = GetInvoice.CreatedBy;
                                    updateinvoicemaster.PrimaryDateTime = DateTime.UtcNow;
                                    _unitWork.InvoiceMaster.Update(updateinvoicemaster);

                                    var it = _unitWork.InvoiceJob.Local().FirstOrDefault(x => x.Jid == item.JId);
                                    var totalamount = _unitWork.InvoiceTran.Local().FirstOrDefault(x => x.Id == it.It);
                                    var invoiceid = _unitWork.InvoiceMaster.Local().FirstOrDefault();
                                    if (item.DepartmentId == 1 && jobord.Id == item.JId)
                                    {
                                        if (jobord.IsWaiver == true)
                                        {
                                            updateinvoicemaster.ArtWaiver = invoiceid.ArtWaiver + totalamount.Rate;
                                            updateinvoicemaster.ArtInvoiceAmount = invoiceid.ArtInvoiceAmount + totalamount.Rate;
                                        }
                                        else
                                        {
                                            updateinvoicemaster.ArtInvoiceAmount = invoiceid.ArtInvoiceAmount + totalamount.Rate;
                                        }
                                    }
                                    else if (item.DepartmentId == 2 && jobord.Id == item.JId)
                                    {
                                        if (jobord.IsWaiver == true)
                                        {
                                            updateinvoicemaster.DigiWaiver = invoiceid.DigiWaiver + totalamount.Rate;
                                            updateinvoicemaster.ArtInvoiceAmount = invoiceid.ArtInvoiceAmount + totalamount.Rate;
                                        }
                                        else
                                        {
                                            updateinvoicemaster.DigiInvoiceAmount = invoiceid.DigiInvoiceAmount + totalamount.Rate;
                                        }
                                    }
                                    updateinvoicemaster.Waiver = invoiceid.ArtWaiver + invoiceid.DigiWaiver;
                                    if (invoiceid.Waiver != null)
                                    {
                                        updateinvoicemaster.ReceivableValue = invoiceid.ProductValue - invoiceid.Waiver;
                                    }
                                    else
                                    {
                                        updateinvoicemaster.ReceivableValue = invoiceid.ProductValue;
                                    }
                                    Decimal reciroundoff = Convert.ToDecimal(invoiceid.ReceivableValue);
                                    updateinvoicemaster.InvoiceValue = Math.Round(reciroundoff, 2);
                                    updateinvoicemaster.TotalInvoiceValue = Math.Round(reciroundoff, 2);
                                    updateinvoicemaster.RoundOff = Math.Round(reciroundoff, 2) - invoiceid.ReceivableValue;
                                    if (jidExists) _unitWork.SaveChanges();
                                }
                            }
                        } // else end
                    }
                }
                catch (Exception ex)
                {
                    Log addlog = new Log();
                    addlog.Module = "Invoice Rep" + item.JId;
                    addlog.Description = "Error Msg: " + ex.Message + ", Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source;
                    addlog.ClientId = item.ClientId;
                    addlog.JobId = item.JId;
                    addlog.Type = "Error Occ";
                    addlog.CreatedUtc = DateTime.UtcNow;
                    _unitWork.Log.Add(addlog);
                    _unitWork.SaveChanges();
                }
                finally
                {
                    db.ChangeTracker.AutoDetectChangesEnabled = false;
                    //db.Configuration.AutoDetectChangesEnabled = true;
                    //db.Configuration.ValidateOnSaveEnabled = true;
                }
            }
            /*
                        }
                        else
                        {
                            Message = "Unbilled jobs are skipped and Invoice Preview for billed Job(s).";
                        }*/

            var jid = selectedJobs.Select(x => x.Id);
            var updateOrders = _unitWork.JobOrder.Where(x => jid.Contains(x.Id)).ToList();
            updateOrders.ForEach(x => x.IsBillable = true);
            _unitWork.SaveChanges();
            return Message;
        }
        //
        public int Return3Tab1Collection(GetInvoiceVM GetInvoice, int JobStatus)
        {
            var selectedJobs = GetInvoice.GetInvoice;
            //db.Configuration.AutoDetectChangesEnabled = false;
            //db.Configuration.ValidateOnSaveEnabled = false;
            db.ChangeTracker.AutoDetectChangesEnabled = false;
            var result = new List<ReturnCollectionVM>();
            foreach (var item in selectedJobs)
            {
                var getjobstatus = _unitWork.JobOrder.FirstOrDefault(x => x.Id == item.JId && x.IsDeleted == false);

                //3
                var pricingWithScopeValues3 = _unitWork.PricingWithScope.Where(x => x.CustomerId == item.ClientId && x.DepartmentId == item.DepartmentId
                && x.ScopeId == item.ScopeId && x.IsDelete != true && x.IsUpdated != true && x.JobStatusId == getjobstatus.JobStatusId && x.PricingTypeId == 3 &&
                (x.WefromDate.Value.Date <= item.FileReceivedDate.Date) &&
                ((x.WetoDate.Value.Date >= item.FileReceivedDate.Date) || x.WetoDate == null)).OrderByDescending(x => x.Id).ToList();
                if (pricingWithScopeValues3.Count() > 0)
                {
                    //pricingtype 3
                    result.Add(new ReturnCollectionVM
                    {
                        JId = item.Id,
                        JobStatusId = getjobstatus.JobStatusId
                    });
                }
            }
            return result.Where(x => x.JobStatusId == JobStatus).Count();
        }
        public int Return4Tab1Collection(GetInvoiceVM GetInvoice, int JobStatus)
        {
            var selectedJobs = GetInvoice.GetInvoice;
            db.ChangeTracker.AutoDetectChangesEnabled = false;
            //db.Configuration.AutoDetectChangesEnabled = false;
            //db.Configuration.ValidateOnSaveEnabled = false;
            var result = new List<ReturnCollectionVM>();
            foreach (var item in selectedJobs)
            {
                var getjobstatus = _unitWork.JobOrder.FirstOrDefault(x => x.Id == item.JId && x.IsDeleted == false);
                //4
                var pricingWithScopeValues4 = _unitWork.PricingWithScope.Where(x => x.CustomerId == item.ClientId && x.DepartmentId == item.DepartmentId
                && x.ScopeId == item.ScopeId && x.IsDelete != true && x.IsUpdated != true && x.JobStatusId == getjobstatus.JobStatusId && x.PricingTypeId == 4 &&
                (x.WefromDate.Value.Date <= item.FileReceivedDate.Date) &&
                ((x.WetoDate.Value.Date >= item.FileReceivedDate.Date) || x.WetoDate == null)).OrderByDescending(x => x.Id).ToList();
                if (pricingWithScopeValues4.Count() > 0)
                {
                    //pricingtype 4
                    result.Add(new ReturnCollectionVM
                    {
                        JId = item.Id,
                        JobStatusId = getjobstatus.JobStatusId
                    });
                }
                ////10
                //var pricingWithScopeValues10 = _unitWork.PricingWithScope.Where(x => x.CustomerId == item.ClientId && x.DepartmentId == item.DepartmentId
                //&& x.ScopeId == item.ScopeId && x.IsDelete != true && x.IsUpdated != true && x.JobStatusId == getjobstatus.JobStatusId && x.PricingTypeId == 10 &&
                //(DbFunctions.TruncateTime(x.WefromDate) <= DbFunctions.TruncateTime(item.FileReceivedDate)) &&
                //((DbFunctions.TruncateTime(x.WetoDate) >= DbFunctions.TruncateTime(item.FileReceivedDate)) || x.WetoDate == null)).OrderByDescending(x => x.Id).ToList();
                //if (pricingWithScopeValues10.Count() > 0)
                //{
                //    //pricingtype 4
                //    result.Add(new ReturnCollectionVM
                //    {
                //        JId = item.Id,
                //        JobStatusId = getjobstatus.JobStatusId
                //    });
                //}
            }
            return result.Where(x => x.JobStatusId == JobStatus).Count();
        }
       

        public IEnumerable<ViewGetInvoiceMaster> GetAllInvoiceMasterDetails()
        {
            var result = _unitviewWork.ViewGetInvoiceMaster.OrderByDescending(x => x.Id);
            return result;
        }
        //getting single data for discount in 3rd tab
        public ViewGetInvoiceMaster GetselectedInvoiceDetailsforDiscount(GetInvoiceDetailVM1 invoicenumber)
        {
            try
            {
                var result = _unitviewWork.ViewGetInvoiceMaster.FirstOrDefault(x => x.InvoiceNo == invoicenumber.InvoiceNo);
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //applying discount in 3rd tab
        public string GetDiscountInvoice(GetInvoiceDetailVM1 invoicenumber)
        {
          //  using (var _db = new ApplicationDbContext(_configuration))
          //  {
                using (Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerTransaction dbTran = (Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerTransaction)_db.Database.BeginTransaction())
                {
                    try
                    {
                        string message = "Discount Applied for " + invoicenumber.InvoiceNo;
                        var selectedjob = _db.InvoiceMasters.FirstOrDefault(x => x.InvoiceNo == invoicenumber.InvoiceNo);
                        decimal invdiscount = Convert.ToDecimal(invoicenumber.Discount);
                        decimal invtotalvalue = Convert.ToDecimal(invoicenumber.TotalInvoiceValue);
                        decimal dicount = Math.Round(invdiscount, 2);
                        selectedjob.Discount = dicount;
                        selectedjob.TotalInvoiceValue = Math.Round(invtotalvalue, 2);
                        decimal getart = Convert.ToDecimal(selectedjob.ArtInvoiceAmount - selectedjob.ArtWaiver);
                        decimal getdigi = Convert.ToDecimal(selectedjob.DigiInvoiceAmount - selectedjob.DigiWaiver);
                        decimal invoicevalue = Convert.ToDecimal(selectedjob.InvoiceValue);
                        decimal artdisround = Math.Round(getart / invoicevalue * 100, 2);
                        decimal digidisround = Math.Round(getdigi / invoicevalue * 100, 2);
                        selectedjob.ArtDiscountPercent = artdisround;
                        selectedjob.ArtDiscountAmount = Math.Round(artdisround / 100 * dicount, 2);
                        selectedjob.DigiDiscountPercent = digidisround;
                        selectedjob.DigiDiscountAmount = Math.Round(digidisround / 100 * dicount, 2);
                        _db.Entry(selectedjob).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _db.SaveChanges();

                        dbTran.Commit();

                        return message;
                    }
                    catch (Exception ex)
                    {
                        dbTran.Rollback();
                        return null;
                    }
                }
         //   }
            //try
            //{
            //    string message = "Discount Applied for " + invoicenumber.InvoiceNo;
            //    var selectedjob = _unitWork.InvoiceMaster.FirstOrDefault(x => x.InvoiceNo == invoicenumber.InvoiceNo);
            //    decimal invdiscount = Convert.ToDecimal(invoicenumber.Discount);
            //    decimal invtotalvalue = Convert.ToDecimal(invoicenumber.TotalInvoiceValue);
            //    decimal dicount = Math.Round(invdiscount, 2);
            //    selectedjob.Discount = dicount;
            //    selectedjob.TotalInvoiceValue = Math.Round(invtotalvalue, 2);
            //    decimal getart = Convert.ToDecimal(selectedjob.ArtInvoiceAmount - selectedjob.ArtWaiver);
            //    decimal getdigi = Convert.ToDecimal(selectedjob.DigiInvoiceAmount - selectedjob.DigiWaiver);
            //    decimal invoicevalue = Convert.ToDecimal(selectedjob.InvoiceValue);
            //    decimal artdisround = Math.Round(getart / invoicevalue * 100, 2);
            //    decimal digidisround = Math.Round(getdigi / invoicevalue * 100, 2);
            //    selectedjob.ArtDiscountPercent = artdisround;
            //    selectedjob.ArtDiscountAmount = Math.Round(artdisround / 100 * dicount, 2);
            //    selectedjob.DigiDiscountPercent = digidisround;
            //    selectedjob.DigiDiscountAmount = Math.Round(digidisround / 100 * dicount, 2);
            //    _unitWork.InvoiceMaster.Update(selectedjob);
            //    _unitWork.SaveChanges();
            //    return message;
            //}
            //catch
            //{
            //    return null;
            //}            
        }
        //generate invoice from 2nd
        public string GenerateConfirmInvoice(GetInvoiceVM GetInvoice)
        {
            string saveconfirminvoice = "Confirm";
            string Message = "Invoice Generated for Selected Item(s)";
            var customerName = "";
            var getlist = GetInvoice.GetInvoice.ToList();
            var results = getlist.GroupBy(p => p.ClientId, (key) => new { ClientId = key, });
            var invoicelistadd = new GetInvoiceVM();
            var clientids = results.ToList();
            //GetInvoiceCalcConfirm(GetInvoice);
            foreach (var item in clientids.AsQueryable())
            {
                var customername = _unitWork.Customer.FirstOrDefault(x => x.Id == item.Key && x.IsDeleted == false);
                try
                {
                    int pricingType = 0;
                    int selectclientid = item.Key;
                    if (customerName != "")
                    {
                        continue;
                    }
                    var getlistoflastinvoice = _unitWork.InvoiceMaster.FirstOrDefault(x => x.CustomerId == selectclientid && x.InvoiceNo == null);
                    DateTime jobdate = DateTime.UtcNow;
                    var transtype = _unitWork.TransactionType.FirstOrDefault(x => x.Description == "Invoice");
                    var VoucherControlDetail = _unitWork.VoucherControl.FirstOrDefault(x => x.TransactionId == transtype.Id && x.EffectiveFrom <= jobdate && x.EffectiveTo >= jobdate && x.IsDeleted == false && x.IsActive == true);
                    if (VoucherControlDetail == null)
                    {
                        return Message = "VoucherControl is Missing";
                    }
                    string Prefix = VoucherControlDetail.Prefix.ToString();
                    long VoucherNo = VoucherControlDetail.Voucherno + 1;
                    var oVoucherControlsingle = _unitWork.VoucherControl.Find(VoucherControlDetail.Id);
                    oVoucherControlsingle.Voucherno = VoucherNo;
                    oVoucherControlsingle.UpdatedBy = GetInvoice.CreatedBy;
                    oVoucherControlsingle.UpdatedUtc = DateTime.UtcNow;
                    _unitWork.VoucherControl.Update(oVoucherControlsingle);
                    string month = jobdate.ToString("MM");
                    string day = jobdate.ToString("dd");
                    int year = jobdate.Year;
                    string fmt = "0000.##";
                    string safix = VoucherNo.ToString(fmt);
                    Invoiceno = Prefix + safix;
                    if (getlistoflastinvoice != null)
                    {
                        var saveinvoicemaster = _unitWork.InvoiceMaster.FirstOrDefault(x => x.Id == getlistoflastinvoice.Id);
                        int artfilecount = _unitWork.InvoiceTran.Where(x => x.Imid == getlistoflastinvoice.Id && x.DepartmentId == 1).Count();
                        int digifilecount = _unitWork.InvoiceTran.Where(x => x.Imid == getlistoflastinvoice.Id && x.DepartmentId == 2).Count();
                        saveinvoicemaster.InvoiceDate = DateTime.UtcNow;
                        saveinvoicemaster.UpdatedBy = GetInvoice.CreatedBy;
                        saveinvoicemaster.UpdatedUtc = DateTime.UtcNow;
                        saveinvoicemaster.SecondaryUpdatedBy = GetInvoice.CreatedBy;
                        saveinvoicemaster.SecondaryDateTime = DateTime.UtcNow;
                        Decimal reciroundoff = Convert.ToDecimal(saveinvoicemaster.ReceivableValue);
                        saveinvoicemaster.RoundOff = Math.Round(reciroundoff, 2) - saveinvoicemaster.ReceivableValue;
                        saveinvoicemaster.TransactionId = VoucherControlDetail.TransactionId;
                        saveinvoicemaster.ArtFileCount = artfilecount;
                        saveinvoicemaster.DigiFileCount = digifilecount;
                        _unitWork.InvoiceMaster.Update(saveinvoicemaster);
                        saveinvoicemaster.InvoiceNo = Invoiceno;
                        _unitWork.SaveChanges();
                    }
                }
                catch (Exception ex)
                {

                    Log addlog = new Log();
                    addlog.Module = "Confirm Invoice";
                    addlog.Description = "Error Msg: " + ex.Message + ", Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source;
                    addlog.ClientId = item.Key;
                    addlog.Type = "Error Occd in Confirm Invoice";
                    addlog.CreatedUtc = DateTime.UtcNow;
                    _unitWork.Log.Add(addlog);
                    _unitWork.SaveChanges();

                    if (customerName == "")
                    {
                        customerName = customername.Name;
                    }
                    else
                    {
                        customerName = customerName + ", " + customername.Name;
                    }
                }
                finally
                {
                    if (customerName != "")
                    {
                        Message = "No Pricing for " + customerName + " Customer";
                    }
                }
            }
            return Message;
        }

        //public string GetInvoiceCalcConfirm(GetInvoiceVM GetInvoice)
        //{
        //    string Message = "Price has been Updated";
        //    var selectedJobs = GetInvoice.GetInvoice;
        //    decimal calcproductvalue = 0;
        //    /*            var invoiceid = _unitWork.InvoiceMaster.OrderByDescending(i => i.Id).FirstOrDefault();
        //                invoiceid.DigiInvoiceAmount = 0;
        //                invoiceid.ArtInvoiceAmount = 0;
        //    */
        //    foreach (var item in selectedJobs)
        //    {
        //        try
        //        {
        //            var checkspecialprice = _unitWork.JobOrder.Where(x => x.Id == item.JId && x.IsDeleted == false && x.DateofUpload != null && x.SpecialPrice != null)
        //            .Select(x => new { x.PricingTypeId, x.IsSpecialPrice, x.SpecialPrice, x.IsWaiver }).FirstOrDefault();

        //            //var invoice = _unitWork.InvoiceMaster.Where(i => i.CustomerId == item.ClientId).OrderByDescending(i => i.Id).FirstOrDefault();

        //            var alreadyexiststaffingpricing = _unitprocedure.GetCheckStaffing(item.ClientId, item.DepartmentId, item.FileReceivedDate);
        //            int staffingdesig = alreadyexiststaffingpricing.Count();
        //            if (staffingdesig > 0)
        //            {
        //                continue;
        //            }
        //            else
        //            {
        //                if (item.DepartmentId == 2 && item.ScopeId == null)
        //                {
        //                    item.ScopeId = 21;
        //                }
        //                /*                            var checkspecialpriceold = _unitWork.JobOrder.Where(x => x.Id == item.JId && x.IsDeleted == false && x.DateofUpload != null && x.SpecialPrice != null)
        //                                                .Select(x => new { x.PricingTypeId, x.IsSpecialPrice, x.SpecialPrice, x.JobStatusId }).FirstOrDefault();
        //                */
        //                var getjobstatus = _unitWork.JobOrder.FirstOrDefault(x => x.Id == item.JId && x.IsDeleted == false);
        //                var pricingWithScopeValues = _unitWork.PricingWithScope.Where(x => x.CustomerId == item.ClientId && x.DepartmentId == item.DepartmentId
        //                                            && x.ScopeId == item.ScopeId && x.IsDelete != true && x.IsUpdated != true && x.JobStatusId == getjobstatus.JobStatusId && x.IsApproved == true &&
        //                                            ((x.WefromDate.Value.Date <= item.FileReceivedDate.Date) &&
        //                                            (x.WetoDate.Value.Date >= item.FileReceivedDate.Date) || x.WetoDate == null)).OrderByDescending(x => x.Id).ToList();
        //                var checkchoosepricingtype = pricingWithScopeValues.Where(x => x.PricingTypeId != 2).Select(x => new { x.PricingTypeId, x.Price, x.RatePerHour }).FirstOrDefault();
        //                int PricingTypeId = 0;
        //                if (checkchoosepricingtype == null && checkspecialprice == null)
        //                {
        //                    var customername = _unitWork.Customer.Where(x => x.Id == item.ClientId && x.IsDeleted == false).FirstOrDefault().Name;
        //                    Message = "No Pricing for " + customername + " Customer.";
        //                }
        //                else
        //                {
        //                    var invoicemasterId = _unitWork.InvoiceMaster.Where(x => x.CustomerId == item.ClientId && x.InvoiceNo == null).Select(x => x.Id).FirstOrDefault();
        //                    if (checkspecialprice != null)
        //                    {
        //                        if (checkspecialprice.IsSpecialPrice == true)
        //                        {
        //                            PricingTypeId = 2;
        //                        }
        //                        else if (checkspecialprice.IsSpecialPrice == false)
        //                        {
        //                            PricingTypeId = Convert.ToInt32(checkspecialprice.PricingTypeId);
        //                        }
        //                        else
        //                        {
        //                            PricingTypeId = checkchoosepricingtype.PricingTypeId;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        PricingTypeId = checkchoosepricingtype.PricingTypeId;
        //                    }

        //                    //Pricing Based on Scope
        //                    if (PricingTypeId == 1 || PricingTypeId == 9)
        //                    {
        //                        var scopebased = _unitWork.JobOrder.Where(x => x.Id == item.JId).FirstOrDefault();
        //                        qty = 1;
        //                        rate = checkchoosepricingtype.Price.Value;
        //                        calcproductvalue = qty * checkchoosepricingtype.Price.Value;

        //                        scopebased.SpecialPrice = calcproductvalue;
        //                        scopebased.IsSpecialPrice = false;
        //                        scopebased.IsQuatation = false;
        //                        scopebased.PricingTypeId = PricingTypeId;
        //                        scopebased.UpdatedUtc = DateTime.UtcNow;
        //                        //scopebased.DateofDownload = new DateTime(2018,01,01);
        //                        _unitWork.JobOrder.Update(scopebased);
        //                        _unitWork.SaveChanges();
        //                    }
        //                    //Rate Based On Stitch Count
        //                    else if (PricingTypeId == 6)
        //                    {
        //                        var stitchcount = _unitWork.JobOrder.Where(x => x.Id == item.JId).FirstOrDefault();
        //                        float getworkedcount = Convert.ToInt64(stitchcount.StitchCount);
        //                        if (getworkedcount <= 1000)
        //                        {
        //                            qty = 1;
        //                            rate = checkchoosepricingtype.Price.Value;
        //                            calcproductvalue = rate;
        //                        }
        //                        else
        //                        {
        //                            float GetTotalstitchcount = ((getworkedcount) / (1000));
        //                            decimal getrate = Convert.ToDecimal(GetTotalstitchcount) * checkchoosepricingtype.Price.Value;
        //                            if (getrate >= checkchoosepricingtype.RatePerHour)
        //                            {
        //                                qty = 1;
        //                                rate = checkchoosepricingtype.RatePerHour.Value;
        //                                calcproductvalue = rate;
        //                            }
        //                            else
        //                            {
        //                                qty = 1;
        //                                rate = getrate;
        //                                calcproductvalue = rate;
        //                            }
        //                        }
        //                        stitchcount.SpecialPrice = calcproductvalue;
        //                        var CheckMaxStitchCountPrice = _unitWork.PricingWithScope.FirstOrDefault(x => x.CustomerId == item.ClientId && x.DepartmentId == item.DepartmentId && x.JobStatusId == getjobstatus.JobStatusId && x.PricingTypeId == 6 && x.IsDelete != true);
        //                        if (CheckMaxStitchCountPrice != null)
        //                        {
        //                            if (CheckMaxStitchCountPrice.RatePerHour == stitchcount.SpecialPrice)
        //                            {
        //                                stitchcount.IsSpecialPrice = true;
        //                            }
        //                            else
        //                            {
        //                                stitchcount.IsSpecialPrice = false;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            stitchcount.IsSpecialPrice = false;
        //                        }
        //                        stitchcount.IsQuatation = false;
        //                        stitchcount.PricingTypeId = PricingTypeId;
        //                        stitchcount.UpdatedUtc = DateTime.UtcNow;
        //                        //stitchcount.DateofDownload = new DateTime(2018, 01, 01);
        //                        _unitWork.JobOrder.Update(stitchcount);
        //                        //_unitWork.SaveChanges();
        //                    }
        //                    //price based on estimation time
        //                    else if (PricingTypeId == 11 || PricingTypeId == 12)
        //                    {
        //                        //var price = pricingWithScopeValues.Where(x => x.CustomerId == item.ClientId && x.DepartmentId == item.DepartmentId && x.ScopeId == item.ScopeId && x.PricingTypeId == PricingTypeId && ((x.From <= item.EstimatedTime) && (x.To >= item.EstimatedTime))).FirstOrDefault();
        //                        var estimated = _unitWork.JobOrder.Where(x => x.Id == item.JId).FirstOrDefault();
        //                        var price = _unitprocedure.GetPricingforTandM(item.ClientId, item.DepartmentId, item.ScopeId, PricingTypeId, estimated.JobStatusId, item.EstimatedTime).FirstOrDefault();

        //                        if (price != null)
        //                        {
        //                            decimal holdingprice = Convert.ToDecimal(price.Price);
        //                            qty = 1;
        //                            rate = holdingprice;
        //                            calcproductvalue = qty * rate;

        //                            estimated.SpecialPrice = calcproductvalue;
        //                            estimated.IsSpecialPrice = false;
        //                            estimated.IsQuatation = false;
        //                            estimated.PricingTypeId = PricingTypeId;
        //                            estimated.UpdatedUtc = DateTime.UtcNow;
        //                            //estimated.DateofDownload = new DateTime(2018, 01, 01);
        //                            _unitWork.JobOrder.Update(estimated);
        //                            //_unitWork.SaveChanges();
        //                        }
        //                        else
        //                        {
        //                            estimated.SpecialPrice = null;
        //                            estimated.IsSpecialPrice = null;
        //                            estimated.IsQuatation = null;
        //                            estimated.PricingTypeId = null;
        //                            //estimated.DateofDownload = new DateTime(2018, 01, 01);
        //                            _unitWork.JobOrder.Update(estimated);
        //                        }

        //                    }
        //                    //rate base on hour
        //                    else if (PricingTypeId == 13 || PricingTypeId == 14)
        //                    {
        //                        var hour = _unitWork.JobOrder.Where(x => x.Id == item.JId).FirstOrDefault();
        //                        var ratevalue = _unitprocedure.GetRateforTandM(item.ClientId, item.DepartmentId, item.ScopeId, PricingTypeId, hour.JobStatusId).FirstOrDefault();

        //                        if (ratevalue != null)
        //                        {
        //                            decimal estime = Convert.ToDecimal(item.EstimatedTime);
        //                            decimal holdingrate = Convert.ToDecimal(ratevalue.RatePerHour);
        //                            qty = 1;
        //                            rate = estime / 60 * holdingrate;
        //                            calcproductvalue = qty * rate;

        //                            hour.SpecialPrice = calcproductvalue;
        //                            hour.IsSpecialPrice = false;
        //                            hour.IsQuatation = false;
        //                            hour.PricingTypeId = PricingTypeId;
        //                            hour.UpdatedUtc = DateTime.UtcNow;
        //                            //hour.DateofDownload = new DateTime(2018, 01, 01);
        //                            _unitWork.JobOrder.Update(hour);
        //                            //_unitWork.SaveChanges();
        //                        }
        //                        else
        //                        {
        //                            hour.SpecialPrice = null;
        //                            hour.IsSpecialPrice = null;
        //                            hour.IsQuatation = null;
        //                            hour.PricingTypeId = null;
        //                            //hour.DateofDownload = new DateTime(2018, 01, 01);
        //                            _unitWork.JobOrder.Update(hour);
        //                        }

        //                    }
        //                    else if (PricingTypeId == 16 || PricingTypeId == 17)
        //                    {
        //                        PricingWithScopeViewModel pricingViewModel = new PricingWithScopeViewModel();
        //                        pricingViewModel.PricingTypeId = item.PricingTypeId;
        //                        pricingViewModel.ScopeId = (int)item.ScopeId;
        //                        pricingViewModel.JobStatusId = (int)item.JobStatusId;
        //                        pricingViewModel.CustomerId = item.ClientId;

        //                        var pricing = _unitWork.JobOrder
        //                        .Where(f => f.JobId == item.JobId && f.IsDeleted == false)
        //                        .ToList();
        //                        var fileCount = pricing.Count;
        //                        fileCounts += fileCount;

        //                        if (fileCounts != null)
        //                        {
        //                            if (item.DepartmentId == 2)
        //                            {
        //                                rate = CalculateSlabBasedPrice(fileCounts, pricingViewModel);
        //                                var stitchcount = _unitWork.JobOrder
        //                                .Where(x => x.Id == item.JId && x.IsDeleted == false)
        //                                .Select(x => x.StitchCount)
        //                                .FirstOrDefault();
        //                                float getworkedcount = Convert.ToInt64(stitchcount.Value);
        //                                qty = 1;
        //                                float totalStitchCount = ((getworkedcount) / (1000));
        //                                decimal getrate = Convert.ToDecimal(totalStitchCount) * rate;
        //                                if (getrate >= checkchoosepricingtype.RatePerHour)
        //                                {
        //                                    qty = 1;
        //                                    rate = checkchoosepricingtype.RatePerHour.Value;
        //                                    productvalue = rate;
        //                                }
        //                                else
        //                                {
        //                                    qty = 1;
        //                                    rate = getrate;
        //                                    productvalue = rate;
        //                                }
        //                                productvalue = qty * rate;
        //                            }
        //                            else
        //                            {
        //                                rate = CalculateSlabBasedPrice(fileCounts, pricingViewModel);
        //                                qty = 1;
        //                                productvalue = qty * rate;
        //                            }
        //                        }
        //                    }

        //                    else if (PricingTypeId == 3 || PricingTypeId == 4 || PricingTypeId == 10)
        //                    {
        //                        continue;
        //                    }

        //                    var updateinvoicemaster = _unitWork.InvoiceMaster.FirstOrDefault(x => x.Id == invoicemasterId);

        //                    var totalTranRecords = new List<InvoiceTran>();
        //                    //Harsha removed rate for this query because, for rate concession, 1-5 jobs 5$, 1-10 jobs 7$ means, total jobs have to affect 7$, so all time update willl be there
        //                    var myCheckForIT = _unitWork.InvoiceTran.Where(x => x.Imid == updateinvoicemaster.Id && x.ScopeId == item.ScopeId.Value && x.PricingTypeId == 4 || (x.PricingTypeId == 16 || x.PricingTypeId == 17)).OrderByDescending(x => x.Id).FirstOrDefault();
        //                    var jobord = _unitWork.JobOrder.FirstOrDefault(x => x.Id == item.JId && x.IsDeleted == false);
        //                    var jidExists = _unitWork.InvoiceJob.FirstOrDefault(x => x.Jid == item.JId) != null;

        //                    var updateinvoicetran = _unitWork.InvoiceTran
        //                        .Where(x => x.Imid == invoicemasterId && x.PricingTypeId == item.PricingTypeId)
        //                        .OrderBy(x => x.Id)
        //                        .LastOrDefault();

        //                    var cusvsscope = _unitWork.CustomerVsScope.FirstOrDefault(x => x.ScopeId == item.ScopeId.Value);
        //                    var dapartmentname = _unitWork.Department.FirstOrDefault(x => x.Id == item.DepartmentId && x.IsDeleted == false).Description;

        //                    if (updateinvoicetran != null)
        //                    {
        //                        qty = 1;
        //                        updateinvoicetran.Imid = invoicemasterId;
        //                        updateinvoicetran.Qty = qty;
        //                        updateinvoicetran.Rate = rate;
        //                        if (PricingTypeId == 10)
        //                        {
        //                            updateinvoicetran.Rate = productvalue;
        //                        }
        //                        updateinvoicetran.Value = qty * rate;
        //                        if (item.DepartmentId == 1)
        //                        {
        //                            if (cusvsscope == null)
        //                            {
        //                                updateinvoicetran.ScopeId = item.ScopeId.Value;
        //                            }
        //                            else
        //                            {
        //                                updateinvoicetran.ScopeId = item.ScopeId.Value;
        //                                updateinvoicetran.ScopeGroupId = cusvsscope.ScopeGroupId;
        //                                updateinvoicetran.ScopeGroupDescription = cusvsscope.ScopeGroupDescription;
        //                            }
        //                        }
        //                        else if (item.DepartmentId == 2)
        //                        {
        //                            if (cusvsscope == null)
        //                            {
        //                                updateinvoicetran.ScopeId = item.ScopeId.Value;
        //                            }
        //                            else
        //                            {
        //                                updateinvoicetran.ScopeId = item.ScopeId.Value;
        //                                updateinvoicetran.ScopeGroupId = cusvsscope.ScopeGroupId;
        //                                updateinvoicetran.ScopeGroupDescription = cusvsscope.ScopeGroupDescription;
        //                            }
        //                        }
        //                        updateinvoicetran.DepartmentId = item.DepartmentId;
        //                        updateinvoicetran.DepartmentName = dapartmentname;
        //                        updateinvoicetran.Createdby = GetInvoice.CreatedBy;
        //                        updateinvoicetran.CreatedUtc = DateTime.UtcNow;
        //                        _unitWork.InvoiceTran.Update(updateinvoicetran);
        //                    }

        //                    _unitWork.SaveChanges();
        //                    var tranRecord = _unitWork.InvoiceTran.Local().ToList();
        //                    var subQuery = tranRecord.Select(x => x.Id).ToList();
        //                    var tranTotalRecordsWithIM = _unitWork.InvoiceTran.Where(x => !subQuery.Contains(x.Id) && x.Imid == updateinvoicemaster.Id).ToList();
        //                    totalTranRecords = tranTotalRecordsWithIM.Concat(tranRecord).ToList();

        //                    var totalInvoiceValue = totalTranRecords.Where(x => x.Imid == updateinvoicemaster.Id).ToList();
        //                    var invoiceValue = totalInvoiceValue.Sum(x => x.Value);

        //                    Decimal deciinv = Convert.ToDecimal(invoiceValue);
        //                    updateinvoicemaster.ProductValue = invoiceValue;
        //                    updateinvoicemaster.InvoiceValue = Math.Round(deciinv, 2);
        //                    updateinvoicemaster.Discount = 0;
        //                    updateinvoicemaster.UpdatedBy = GetInvoice.CreatedBy;
        //                    updateinvoicemaster.UpdatedUtc = DateTime.UtcNow;
        //                    updateinvoicemaster.PrimaryUpdatedBy = GetInvoice.CreatedBy;
        //                    updateinvoicemaster.PrimaryDateTime = DateTime.UtcNow;
        //                    _unitWork.InvoiceMaster.Update(updateinvoicemaster);
        //                    _unitWork.SaveChanges();

        //                    /*  var invoicetranDetails = _unitWork.InvoiceTran.Local().Where(x => x.Id == 0).OrderByDescending(x => x.Id).FirstOrDefault();
        //                      if (invoicetranDetails == null && myCheckForIT != null)
        //                      {
        //                          invoicetranDetails = _unitWork.InvoiceTran.Local().OrderByDescending(x => x.Id == myCheckForIT.Id).FirstOrDefault();
        //                      }*/

        //                    var it = _unitWork.InvoiceJob.Local().FirstOrDefault(x => x.Jid == item.JId);
        //                    var totalamount = _unitWork.InvoiceTran.Local().FirstOrDefault(x => x.Id == it.It);
        //                    var invoiceid = _unitWork.InvoiceMaster.Local().FirstOrDefault();
        //                    var sumOfRate1 = _unitWork.InvoiceTran.Local().Where(x => x.Imid == invoicemasterId && x.DepartmentId == 1).Sum(x => x.Rate);
        //                    var sumOfRate2 = _unitWork.InvoiceTran.Local().Where(x => x.Imid == invoicemasterId && x.DepartmentId == 2).Sum(x => x.Rate);
        //                    if (item.DepartmentId == 1 && jobord.Id == item.JId)
        //                    {
        //                        if (jobord.IsWaiver == true)
        //                        {
        //                            updateinvoicemaster.ArtWaiver = invoiceid.ArtWaiver + totalamount.Rate;
        //                            updateinvoicemaster.ArtInvoiceAmount = sumOfRate1;
        //                        }
        //                        else
        //                        {
        //                            updateinvoicemaster.ArtInvoiceAmount = sumOfRate1;
        //                        }
        //                    }
        //                    else if (item.DepartmentId == 2 && jobord.Id == item.JId)
        //                    {
        //                        if (jobord.IsWaiver == true)
        //                        {
        //                            updateinvoicemaster.DigiWaiver = invoiceid.DigiWaiver + totalamount.Rate;
        //                            updateinvoicemaster.ArtInvoiceAmount = sumOfRate2;
        //                        }
        //                        else
        //                        {
        //                            updateinvoicemaster.DigiInvoiceAmount = sumOfRate2;
        //                        }
        //                    }
        //                    updateinvoicemaster.Waiver = invoiceid.ArtWaiver + invoiceid.DigiWaiver;
        //                    if (invoiceid.Waiver != null)
        //                    {
        //                        updateinvoicemaster.ReceivableValue = invoiceid.ProductValue - invoiceid.Waiver;
        //                    }
        //                    else
        //                    {
        //                        updateinvoicemaster.ReceivableValue = invoiceid.ProductValue;
        //                    }
        //                    Decimal reciroundoff = Convert.ToDecimal(invoiceid.ReceivableValue);
        //                    updateinvoicemaster.InvoiceValue = Math.Round(reciroundoff, 2);
        //                    updateinvoicemaster.TotalInvoiceValue = Math.Round(reciroundoff, 2);
        //                    updateinvoicemaster.RoundOff = Math.Round(reciroundoff, 2) - invoiceid.ReceivableValue;
        //                    if (jidExists) _unitWork.SaveChanges();
        //                }
        //            }

        //            //}
        //        }
        //        catch (Exception ex)
        //        {

        //            Log addlog = new Log();
        //            addlog.Module = "Invoice Pre-Calculation";
        //            addlog.Description = "Error Msg in Pre-calculation: " + ex.Message + ", Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source;
        //            addlog.ClientId = item.ClientId;
        //            addlog.JobId = item.JId;
        //            addlog.Type = "Error Occured in Invoice calc";
        //            addlog.CreatedUtc = DateTime.UtcNow;
        //            _unitWork.Log.Add(addlog);
        //            _unitWork.SaveChanges();

        //        }
        //        finally
        //        {
        //            if (Message != "")
        //            {
        //                Message = Message;
        //            }
        //        }
        //    }
        //    return Message;
        //}

        //price recalc in 2nd tab
        public string GenerateReCalculateInvoice(GetInvoiceVM GetInvoice)
        {
            string Message = "Selected Job Moved for Recalculation";
            var selectedjob = GetInvoice.GetInvoice;
            var jid = selectedjob.Select(x => x.JId);
            int Imid = selectedjob.Select(x => x.Id).FirstOrDefault();
            int invjobcount = _unitWork.InvoiceJob.Where(x => x.Imid == Imid).Count();
            int selectedcount = selectedjob.Count();
            if (invjobcount == selectedcount)
            {
                var invjob = _unitWork.InvoiceJob.Where(x => x.Imid == Imid);
                var invtran = _unitWork.InvoiceTran.Where(x => x.Imid == Imid);
                var invmaster = _unitWork.InvoiceMaster.Where(x => x.Id == Imid);
                _unitWork.InvoiceJob.RemoveRange(invjob);
                _unitWork.InvoiceTran.RemoveRange(invtran);
                _unitWork.InvoiceMaster.RemoveRange(invmaster);
            }
            else
            {
                db.ChangeTracker.AutoDetectChangesEnabled = false;
                //db.Configuration.AutoDetectChangesEnabled = false;
                //db.Configuration.ValidateOnSaveEnabled = false;
                foreach (var item in selectedjob)
                {
                    try
                    {
                        var deleteinvoicetran = _unitWork.InvoiceTran.FirstOrDefault(x => x.Id == item.TranId);
                        var deleteinvoicejob = _unitWork.InvoiceJob.FirstOrDefault(x => x.Jid == item.JId);
                        var updateinvoicemaster = _unitWork.InvoiceMaster.FirstOrDefault(x => x.Id == item.Id);
                        if (item.PricingTypeId == 5 || item.PricingTypeId == 15)
                        {
                            //staffing deletion goes here
                            int checkinvoiceJobs = _unitWork.InvoiceJob.Count(x => x.It == deleteinvoicetran.Id && x.Id != deleteinvoicejob.Id);
                            if (checkinvoiceJobs == 0)
                            {
                                updateinvoicemaster.ProductValue = updateinvoicemaster.ProductValue - deleteinvoicetran.Value;
                                updateinvoicemaster.Waiver = updateinvoicemaster.ArtWaiver + updateinvoicemaster.DigiWaiver;
                                updateinvoicemaster.ReceivableValue = updateinvoicemaster.ProductValue - updateinvoicemaster.Waiver;
                                decimal invround = Convert.ToDecimal(updateinvoicemaster.ReceivableValue);
                                updateinvoicemaster.RoundOff = Math.Round(invround, 2) - updateinvoicemaster.ReceivableValue;
                                updateinvoicemaster.InvoiceValue = Math.Round(invround, 2);
                                updateinvoicemaster.TotalInvoiceValue = Math.Round(invround, 2);
                                _unitWork.InvoiceJob.Delete(deleteinvoicejob);
                                _unitWork.InvoiceTran.Delete(deleteinvoicetran);
                                _unitWork.InvoiceMaster.Update(updateinvoicemaster);
                            }
                            else
                            {
                                _unitWork.InvoiceJob.Delete(deleteinvoicejob);
                            }
                        }
                        else
                        {
                            //normal deletion goes here
                            if (item.DepartmentId == 1)
                            {
                                if (item.IsWaiver == true)
                                {
                                    updateinvoicemaster.ProductValue = updateinvoicemaster.ProductValue - deleteinvoicetran.Value;
                                    updateinvoicemaster.ArtInvoiceAmount = updateinvoicemaster.ArtInvoiceAmount - deleteinvoicetran.Value;
                                    updateinvoicemaster.ArtWaiver = updateinvoicemaster.ArtWaiver - deleteinvoicetran.Value;
                                }
                                else
                                {
                                    updateinvoicemaster.ProductValue = updateinvoicemaster.ProductValue - deleteinvoicetran.Value;
                                    updateinvoicemaster.ArtInvoiceAmount = updateinvoicemaster.ArtInvoiceAmount - deleteinvoicetran.Value;
                                }
                            }
                            else
                            {
                                if (item.IsWaiver == true)
                                {
                                    updateinvoicemaster.ProductValue = updateinvoicemaster.ProductValue - deleteinvoicetran.Value;
                                    updateinvoicemaster.DigiInvoiceAmount = updateinvoicemaster.DigiInvoiceAmount - deleteinvoicetran.Value;
                                    updateinvoicemaster.DigiWaiver = updateinvoicemaster.DigiWaiver - deleteinvoicetran.Value;
                                }
                                else
                                {
                                    updateinvoicemaster.ProductValue = updateinvoicemaster.ProductValue - deleteinvoicetran.Value;
                                    updateinvoicemaster.DigiInvoiceAmount = updateinvoicemaster.DigiInvoiceAmount - deleteinvoicetran.Value;
                                }
                            }
                            updateinvoicemaster.Waiver = updateinvoicemaster.ArtWaiver + updateinvoicemaster.DigiWaiver;
                            updateinvoicemaster.ReceivableValue = updateinvoicemaster.ProductValue - updateinvoicemaster.Waiver;
                            decimal invround = Convert.ToDecimal(updateinvoicemaster.ReceivableValue);
                            updateinvoicemaster.RoundOff = Math.Round(invround, 2) - updateinvoicemaster.ReceivableValue;
                            updateinvoicemaster.InvoiceValue = Math.Round(invround, 2);
                            updateinvoicemaster.TotalInvoiceValue = Math.Round(invround, 2);
                            _unitWork.InvoiceJob.Delete(deleteinvoicejob);
                            int checkinvoiceJobs = _unitWork.InvoiceJob.Count(x => x.It == deleteinvoicetran.Id && x.Id != deleteinvoicejob.Id);
                            if (checkinvoiceJobs == 0)
                            {
                                _unitWork.InvoiceTran.Delete(deleteinvoicetran);
                            }
                            _unitWork.InvoiceMaster.Update(updateinvoicemaster);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        db.ChangeTracker.AutoDetectChangesEnabled = false;
                       // db.Configuration.AutoDetectChangesEnabled = true;
                      //  db.Configuration.ValidateOnSaveEnabled = true; //Validation does not exist in ef7
                    }
                }
            }
            var updateOrders = _unitWork.JobOrder.Where(x => jid.Contains(x.Id)).ToList();
            updateOrders.ForEach(x => x.IsBillable = false);
            _unitWork.SaveChanges();
            return Message;
        }

        //update price before price calc
        public string GetInvoiceCalc(GetInvoiceVM GetInvoice)
        {
            string Message = "Price has been Updated";
            var selectedJobs = GetInvoice.GetInvoice;
            decimal calcproductvalue = 0;
            foreach (var item in selectedJobs)
            {
                try
                {
                    var alreadyexiststaffingpricing = _unitprocedure.GetCheckStaffing(item.ClientId, item.DepartmentId, item.FileReceivedDate);
                    int staffingdesig = alreadyexiststaffingpricing.Count();
                    if (staffingdesig > 0)
                    {
                        continue;
                    }
                    else
                    {
                        if (item.DepartmentId == 2 && item.ScopeId == null)
                        {
                            item.ScopeId = 21;
                        }
                        var checkspecialprice = _unitWork.JobOrder.Where(x => x.Id == item.JId && x.IsDeleted == false && x.DateofUpload != null && x.SpecialPrice != null)
                            .Select(x => new { x.PricingTypeId, x.IsSpecialPrice, x.SpecialPrice, x.JobStatusId }).FirstOrDefault();
                        var getjobstatus = _unitWork.JobOrder.FirstOrDefault(x => x.Id == item.JId && x.IsDeleted == false);
                        var pricingWithScopeValues = _unitWork.PricingWithScope.Where(x => x.CustomerId == item.ClientId && x.DepartmentId == item.DepartmentId && x.IsDelete != true && x.JobStatusId == getjobstatus.JobStatusId && x.ScopeId == item.ScopeId && (x.WefromDate.Value.Date <= item.FileReceivedDate.Date) && x.IsApproved == true &&
                        ((x.WetoDate.Value.Date >= item.FileReceivedDate.Date) || x.WetoDate == null)).OrderByDescending(x => x.Id).ToList(); //
                        var checkchoosepricingtype = pricingWithScopeValues.Where(x => x.PricingTypeId != 2).Select(x => new { x.PricingTypeId, x.Price, x.RatePerHour }).FirstOrDefault();
                        int PricingTypeId = 0;
                        if (checkchoosepricingtype == null && checkspecialprice == null)
                        {
                            var customername = _unitWork.Customer.Where(x => x.Id == item.ClientId && x.IsDeleted == false).FirstOrDefault().Name;
                            Message = "No Pricing for " + customername + " Customer.";
                        }
                        else
                        {
                            var invoicemasterId = _unitWork.InvoiceMaster.Where(x => x.CustomerId == item.ClientId && x.InvoiceNo == null).Select(x => x.Id).FirstOrDefault();
                            if (checkspecialprice != null)
                            {
                                if (checkspecialprice.IsSpecialPrice == true)
                                {
                                    PricingTypeId = 2;
                                }
                                else if (checkspecialprice.IsSpecialPrice == false)
                                {
                                    PricingTypeId = Convert.ToInt32(checkspecialprice.PricingTypeId);
                                }
                                else
                                {
                                    PricingTypeId = checkchoosepricingtype.PricingTypeId;
                                }
                            }
                            else
                            {
                                PricingTypeId = checkchoosepricingtype.PricingTypeId;
                            }

                            //Pricing Based on Scope
                            if (PricingTypeId == 1 || PricingTypeId == 9)
                            {
                                var scopebased = _unitWork.JobOrder.Where(x => x.Id == item.JId).FirstOrDefault();
                                qty = 1;
                                rate = checkchoosepricingtype.Price.Value;
                                calcproductvalue = qty * checkchoosepricingtype.Price.Value;

                                scopebased.SpecialPrice = calcproductvalue;
                                scopebased.IsSpecialPrice = false;
                                scopebased.IsQuatation = false;
                                scopebased.PricingTypeId = PricingTypeId;
                                scopebased.UpdatedUtc = DateTime.UtcNow;
                                //scopebased.DateofDownload = new DateTime(2018,01,01);
                                _unitWork.JobOrder.Update(scopebased);
                                //_unitWork.SaveChanges();
                            }
                            //Rate Based On Stitch Count
                            else if (PricingTypeId == 6)
                            {
                                var stitchcount = _unitWork.JobOrder.Where(x => x.Id == item.JId).FirstOrDefault();
                                float getworkedcount = Convert.ToInt64(stitchcount.StitchCount);
                                if (getworkedcount <= 1000)
                                {
                                    qty = 1;
                                    rate = checkchoosepricingtype.Price.Value;
                                    calcproductvalue = rate;
                                }
                                else
                                {
                                    float GetTotalstitchcount = ((getworkedcount) / (1000));
                                    decimal getrate = Convert.ToDecimal(GetTotalstitchcount) * checkchoosepricingtype.Price.Value;
                                    if (getrate >= checkchoosepricingtype.RatePerHour)
                                    {
                                        qty = 1;
                                        rate = checkchoosepricingtype.RatePerHour.Value;
                                        calcproductvalue = rate;
                                    }
                                    else
                                    {
                                        qty = 1;
                                        rate = getrate;
                                        calcproductvalue = rate;
                                    }
                                }
                                stitchcount.SpecialPrice = calcproductvalue;
                                var CheckMaxStitchCountPrice = _unitWork.PricingWithScope.FirstOrDefault(x => x.CustomerId == item.ClientId && x.DepartmentId == item.DepartmentId && x.JobStatusId == getjobstatus.JobStatusId && x.PricingTypeId == 6 && x.IsDelete != true);
                                if (CheckMaxStitchCountPrice != null)
                                {
                                    if (CheckMaxStitchCountPrice.RatePerHour == stitchcount.SpecialPrice)
                                    {
                                        stitchcount.IsSpecialPrice = true;
                                    }
                                    else
                                    {
                                        stitchcount.IsSpecialPrice = false;
                                    }
                                }
                                else
                                {
                                    stitchcount.IsSpecialPrice = false;
                                }
                                stitchcount.IsQuatation = false;
                                stitchcount.PricingTypeId = PricingTypeId;
                                stitchcount.UpdatedUtc = DateTime.UtcNow;
                                //stitchcount.DateofDownload = new DateTime(2018, 01, 01);
                                _unitWork.JobOrder.Update(stitchcount);
                                //_unitWork.SaveChanges();
                            }
                            //price based on estimation time
                            else if (PricingTypeId == 11 || PricingTypeId == 12)
                            {
                                //var price = pricingWithScopeValues.Where(x => x.CustomerId == item.ClientId && x.DepartmentId == item.DepartmentId && x.ScopeId == item.ScopeId && x.PricingTypeId == PricingTypeId && ((x.From <= item.EstimatedTime) && (x.To >= item.EstimatedTime))).FirstOrDefault();
                                var estimated = _unitWork.JobOrder.Where(x => x.Id == item.JId).FirstOrDefault();
                                var price = _unitprocedure.GetPricingforTandM(item.ClientId, item.DepartmentId, item.ScopeId, PricingTypeId, estimated.JobStatusId, item.EstimatedTime).FirstOrDefault();

                                if (price != null)
                                {
                                    decimal holdingprice = Convert.ToDecimal(price.Price);
                                    qty = 1;
                                    rate = holdingprice;
                                    calcproductvalue = qty * rate;

                                    estimated.SpecialPrice = calcproductvalue;
                                    estimated.IsSpecialPrice = false;
                                    estimated.IsQuatation = false;
                                    estimated.PricingTypeId = PricingTypeId;
                                    estimated.UpdatedUtc = DateTime.UtcNow;
                                    //estimated.DateofDownload = new DateTime(2018, 01, 01);
                                    _unitWork.JobOrder.Update(estimated);
                                    //_unitWork.SaveChanges();
                                }
                                else
                                {
                                    estimated.SpecialPrice = null;
                                    estimated.IsSpecialPrice = null;
                                    estimated.IsQuatation = null;
                                    estimated.PricingTypeId = null;
                                    //estimated.DateofDownload = new DateTime(2018, 01, 01);
                                    _unitWork.JobOrder.Update(estimated);
                                }

                            }
                            //rate base on hour
                            else if (PricingTypeId == 13 || PricingTypeId == 14)
                            {
                                var hour = _unitWork.JobOrder.Where(x => x.Id == item.JId).FirstOrDefault();
                                var ratevalue = _unitprocedure.GetRateforTandM(item.ClientId, item.DepartmentId, item.ScopeId, PricingTypeId, hour.JobStatusId).FirstOrDefault();

                                if (ratevalue != null)
                                {
                                    decimal estime = Convert.ToDecimal(item.EstimatedTime);
                                    decimal holdingrate = Convert.ToDecimal(ratevalue.RatePerHour);
                                    qty = 1;
                                    rate = estime / 60 * holdingrate;
                                    calcproductvalue = qty * rate;

                                    hour.SpecialPrice = calcproductvalue;
                                    hour.IsSpecialPrice = false;
                                    hour.IsQuatation = false;
                                    hour.PricingTypeId = PricingTypeId;
                                    hour.UpdatedUtc = DateTime.UtcNow;
                                    //hour.DateofDownload = new DateTime(2018, 01, 01);
                                    _unitWork.JobOrder.Update(hour);
                                    //_unitWork.SaveChanges();
                                }
                                else
                                {
                                    hour.SpecialPrice = null;
                                    hour.IsSpecialPrice = null;
                                    hour.IsQuatation = null;
                                    hour.PricingTypeId = null;
                                    //hour.DateofDownload = new DateTime(2018, 01, 01);
                                    _unitWork.JobOrder.Update(hour);
                                }

                            }
                            else if (PricingTypeId == 3 || PricingTypeId == 4 || PricingTypeId == 10)
                            {
                                continue;
                            }
                            //
                            _unitWork.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {

                    Log addlog = new Log();
                    addlog.Module = "Invoice Pre-Calculation";
                    addlog.Description = "Error Msg in Pre-calculation: " + ex.Message + ", Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source;
                    addlog.ClientId = item.ClientId;
                    addlog.JobId = item.JId;
                    addlog.Type = "Error Occured in Invoice calc";
                    addlog.CreatedUtc = DateTime.UtcNow;
                    _unitWork.Log.Add(addlog);
                    _unitWork.SaveChanges();

                }
                finally
                {
                    if (Message != "")
                    {
                        Message = Message;
                    }
                }
            }
            return Message;
        }
        //load invoiceno after client in invoice cancel
        public IEnumerable<GetDropInvoiceCancel_Result> GetDropInvoiceforCancel(invoicesalecancel1 invoicesc)
        {
            try
            {
                var result = _unitprocedure.GetDropInvoiceCancel(invoicesc.Id).OrderByDescending(x => x.Id).AsQueryable();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //loading client in invoice cancel
        public IEnumerable<ViewGetCustomerforInvoiceCancel> GetDropClientforInvoiceCancel()
        {
            try
            {
                var result = _unitviewWork.ViewGetCustomerforInvoiceCancel.OrderBy(x => x.ShortName).AsQueryable();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //loading invoice master before cancel
        public IEnumerable<InvoiceMaster> GetInvoiceMasterforSalesCancel(invoicesalecancel1 invoicesc)
        {
            try
            {
                var result = _tableWork.InvoiceMasterRepository.Get(x => x.InvoiceNo == invoicesc.InvoiceNo).ToList();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //loading detailed jobs for invoice in invoice cancel
        public IEnumerable<GetTranMergeInvoiceCancel_Result> GetInvoiceTranforSalesCancel(invoicesalecancel1 invoicesc)
        {
            try
            {
                var invoicemaster = _unitWork.InvoiceMaster.FirstOrDefault(x => x.InvoiceNo == invoicesc.InvoiceNo);
                return _unitprocedure.GetTranMergeInvoiceCancel(invoicemaster.Id).AsQueryable();
            }
            catch (Exception)
            {

                throw;
            }
        }
        //loading invoice master after cancel
        public IEnumerable<ViewGetCancelledInvoiceList> GetInvoiceMasterDetailforCancelled()
        {
            try
            {
                return _unitviewWork.ViewGetCancelledInvoiceList.OrderByDescending(x => x.Id).AsQueryable();
            }
            catch (Exception)
            {

                throw;
            }
        }
        //loading detailed jobs for canceled invoice
        public IEnumerable<GetTranMergeInvoiceCancel_Result> GetInvoiceTranDetailforCancelled(invoicesalecancel1 invoicesc)
        {
            try
            {
                var invoicemaster = _unitWork.InvoiceMaster.FirstOrDefault(x => x.InvoiceNo == invoicesc.InvoiceNo);
                return _unitprocedure.GetTranMergeInvoiceCancel(invoicemaster.Id).AsQueryable();
            }
            catch (Exception)
            {

                throw;
            }
        }
        //cancelling invoice
        public string GetUpdateMasterforSalesCancel(invoicesalecancel invoicesc)
        {
            try
            {
                string message = "";
                string invoiceno = "";
                DateTime invoicedate = DateTime.UtcNow;
                var invoicemasterrow = _unitviewWork.ViewGetInvoiceMasterforDayDiff.FirstOrDefault(x => x.InvoiceNo == invoicesc.InvoiceNo);
                var checkinvoicetran = _unitWork.InvoiceTran.FirstOrDefault(x => x.InvoiceNo == invoicesc.InvoiceNo);
                if (invoicemasterrow.DateDifference < 60)
                {
                    var transtype = _unitWork.TransactionType.FirstOrDefault(x => x.Description == "Invoice Cancellation");
                    DateTime jobdate = DateTime.UtcNow;
                    var voucher = _unitWork.VoucherControl.FirstOrDefault(x => x.TransactionId == transtype.Id && x.EffectiveFrom <= jobdate && x.EffectiveTo >= jobdate && x.IsDeleted == false && x.IsActive == true);
                    if (voucher == null)
                    {
                        return message = "VoucherControl is Missing";
                    }
                    string prefix = voucher.Prefix.ToString();
                    long vocherno = voucher.Voucherno + 1;
                    string fmt = "0000.##";
                    string safix = vocherno.ToString(fmt);
                    var saveinvoicemaster = new InvoiceMaster();
                    saveinvoicemaster.InvoiceNo = prefix + safix;
                    invoiceno = prefix + safix;
                    saveinvoicemaster.InvoiceDate = invoicedate;
                    saveinvoicemaster.ReferenceNo = invoicemasterrow.ReferenceNo;
                    saveinvoicemaster.ReferenceDate = invoicemasterrow.ReferenceDate;
                    saveinvoicemaster.CustomerId = invoicemasterrow.CustomerId;
                    saveinvoicemaster.ProductValue = invoicemasterrow.ProductValue;
                    saveinvoicemaster.Discount = invoicemasterrow.Discount;
                    saveinvoicemaster.InvoiceValue = invoicemasterrow.InvoiceValue;
                    saveinvoicemaster.IsDeleted = invoicemasterrow.IsDeleted;
                    saveinvoicemaster.IsCancelled = invoicemasterrow.IsCancelled;
                    saveinvoicemaster.IsSample = invoicemasterrow.IsSample;
                    saveinvoicemaster.CreatedUtc = invoicemasterrow.CreatedUtc;
                    saveinvoicemaster.UpdatedUtc = invoicemasterrow.UpdatedUtc;
                    saveinvoicemaster.CreatedBy = invoicesc.EmployeeId;
                    saveinvoicemaster.UpdatedBy = invoicemasterrow.UpdatedBy;
                    saveinvoicemaster.IsOutstanding = invoicemasterrow.IsOutstanding;
                    saveinvoicemaster.PrimaryDateTime = invoicemasterrow.PrimaryDateTime;
                    saveinvoicemaster.PrimaryUpdatedBy = invoicemasterrow.PrimaryUpdatedBy;
                    saveinvoicemaster.SecondaryDateTime = invoicemasterrow.SecondaryDateTime;
                    saveinvoicemaster.SecondaryUpdatedBy = invoicemasterrow.SecondaryUpdatedBy;
                    saveinvoicemaster.BillingCycleId = invoicemasterrow.BillingCycleId;
                    saveinvoicemaster.ArtInvoiceAmount = invoicemasterrow.ArtInvoiceAmount;
                    saveinvoicemaster.DigiInvoiceAmount = invoicemasterrow.DigiInvoiceAmount;
                    saveinvoicemaster.RoundOff = invoicemasterrow.RoundOff;
                    saveinvoicemaster.ArtWaiver = invoicemasterrow.ArtWaiver;
                    saveinvoicemaster.DigiWaiver = invoicemasterrow.DigiWaiver;
                    saveinvoicemaster.Waiver = invoicemasterrow.Waiver;
                    saveinvoicemaster.ReceivableValue = invoicemasterrow.ReceivableValue;
                    saveinvoicemaster.TotalInvoiceValue = invoicemasterrow.TotalInvoiceValue;
                    saveinvoicemaster.TransactionId = voucher.TransactionId;
                    _unitWork.InvoiceMaster.Add(saveinvoicemaster);
                    var oVoucherControlsingle = _unitWork.VoucherControl.Find(voucher.Id);
                    oVoucherControlsingle.Voucherno = vocherno;
                    oVoucherControlsingle.UpdatedBy = 1;
                    oVoucherControlsingle.UpdatedUtc = DateTime.UtcNow;
                    _unitWork.VoucherControl.Update(oVoucherControlsingle);
                    _unitWork.SaveChanges();
                    var getimidtran = _unitWork.InvoiceTran.Where(x => x.Imid == invoicemasterrow.Id).ToList();
                    foreach (var item in getimidtran)
                    {
                        var invoicetranrow = _unitWork.InvoiceTran.FirstOrDefault(x => x.Id == item.Id);
                        var invoicemastercancel = _unitWork.InvoiceMaster.FirstOrDefault(x => x.InvoiceNo == invoiceno);
                        var saveinvoicetran = new InvoiceTran();
                        saveinvoicetran.Imid = invoicemastercancel.Id;
                        saveinvoicetran.Uom = item.Uom;
                        saveinvoicetran.Qty = item.Qty;
                        saveinvoicetran.Rate = item.Rate;
                        saveinvoicetran.Value = item.Value;
                        saveinvoicetran.PricingTypeId = item.PricingTypeId;
                        saveinvoicetran.Createdby = item.Createdby;
                        saveinvoicetran.CreatedUtc = item.CreatedUtc;
                        saveinvoicetran.UpdatedBy = item.UpdatedBy;
                        saveinvoicetran.UpdatedUtc = item.UpdatedUtc;
                        saveinvoicetran.Description = item.Description;
                        saveinvoicetran.ScopeId = item.ScopeId;
                        saveinvoicetran.DepartmentId = item.DepartmentId;
                        saveinvoicetran.DepartmentName = item.DepartmentName;
                        saveinvoicetran.ScopeGroupId = item.ScopeGroupId;
                        saveinvoicetran.ScopeGroupDescription = item.ScopeGroupDescription;
                        saveinvoicetran.InvoiceNo = invoicemasterrow.InvoiceNo;
                        saveinvoicetran.InvoiceDate = invoicemasterrow.InvoiceDate;
                        _unitWork.InvoiceTran.Add(saveinvoicetran);

                        var invoicejobcancel = _unitWork.InvoiceJob.FirstOrDefault(x => x.It == item.Id);
                        invoicejobcancel.InvoiceCancelNo = invoiceno;
                        invoicejobcancel.InvoiceCancelDate = invoicedate;
                        _unitWork.InvoiceJob.Update(invoicejobcancel);
                    }
                    _unitWork.SaveChanges();

                    message = "Invoice Cancelled " + invoiceno + " for Selected " + invoicesc.InvoiceNo;
                }
                else
                {
                    message = " Sorry Invoice " + invoicesc.InvoiceNo + " Exceeded " + invoicemasterrow.DateDifference + " of 60 Days";
                }
                return message;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //loading data in tally
        public IEnumerable<GetIntegrationMaster_Result> GetInvoiceIntegrationMaster(integrationmasterpara Getparam)
        {
            try
            {
                var result = _unitprocedure.GetIntegrationMaster(Getparam.CustomerID, Getparam.FromDate, Getparam.ToDate).OrderByDescending(x => x.ESTInvoiceDate).AsQueryable();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //applying exchange rate in tally
        public string GetExchangeRatetoInvoice(integrationxchangerate invintigxchange)
        {
            try
            {
                string Message = "ExchangeRate Applied for Selected Invoice";
                var selectedJobs = invintigxchange.invintigxchange;
                foreach (var item in selectedJobs)
                {
                    if (item.ExchangeRate != null)
                    {
                        var updateinvoicemaster = _unitWork.InvoiceMaster.FirstOrDefault(x => x.Id == item.Id);
                        updateinvoicemaster.ExchangeRate = item.ExchangeRate;
                        _unitWork.InvoiceMaster.Update(updateinvoicemaster);
                        _unitWork.SaveChanges();
                    }
                    else
                    {
                        Message = "ExchangeRate Can't be an Empty!";
                    }
                }
                return Message;
            }
            catch
            {

                return null;
            }
        }
        //copy to tally
        public string GetCopytoIntegration(invoiceintiglist invintiglist)
        {
            try
            {
                string Message = "Selected Invoice Moved to Integration";
                var selectedJobs = invintiglist.invintiglist;
                foreach (var item in selectedJobs)
                {
                    var checkintegrationmaster = _unitWork.IntegrationMaster.First(x => x.Id == item.Id);
                    if (checkintegrationmaster == null)
                    {
                        if (item.ExchangeRate != null)
                        {
                            var intigmaster = new IntegrationMaster();
                            intigmaster.TransactionId = item.TransactionId;
                            intigmaster.DocumentNumber = item.InvoiceNo;
                            intigmaster.DocumentDate = item.InvoiceDate;
                            intigmaster.ClientName = item.ShortName;
                            intigmaster.ProductValue = item.ProductValue;
                            intigmaster.Roundoff = item.RoundOff;
                            intigmaster.WaiverAmount = item.Waiver;
                            intigmaster.DiscountAmount = item.Discount;
                            intigmaster.TotalValue = item.TotalInvoiceValue;
                            intigmaster.IsCancelled = false;
                            intigmaster.IsDeleted = false;
                            //intigmaster.IsLinked = ;
                            //intigmaster.IsFailed = ;
                            //intigmaster.FailedReason = ;
                            //intigmaster.DestinationBank = ;
                            //intigmaster.Mode = ;
                            //intigmaster.TransactionNumber = ;
                            //intigmaster.TransactionDate = ;
                            //intigmaster.Amount = ;
                            //intigmaster.BankName = ;
                            intigmaster.CreatedBy = invintiglist.EmployeeId;
                            intigmaster.CreatedUtc = DateTime.UtcNow;
                            //intigmaster.UpdatedBy = ;
                            //intigmaster.UpdatedUtc = ;                        
                            intigmaster.ExchangeRate = item.ExchangeRate;
                            intigmaster.CurrencyName = "USD";
                            intigmaster.CurrencySymbol = "$";
                            _unitWork.IntegrationMaster.Insert(intigmaster);

                            var getintitran = _unitprocedure.GetIntegrationTran(item.Id);
                            foreach (var itemtran in getintitran)
                            {
                                var intitran = new IntegrationTran();
                                intitran.IntegrationMaster = intigmaster;
                                intitran.IntegrationMasterId = intigmaster.Id;
                                intitran.Scope = itemtran.Description;
                                intitran.Qty = itemtran.Qty;
                                intitran.Rate = itemtran.Rate;
                                intitran.Value = itemtran.Value;
                                intitran.InvoiceNumber = item.InvoiceNo;
                                intitran.InvoiceDate = item.InvoiceDate;
                                _unitWork.IntegrationTran.Insert(intitran);
                            }
                            var invmaster = _unitWork.InvoiceMaster.FirstOrDefault(x => x.Id == item.Id);
                            invmaster.IsTallyIntegrated = true;
                            _unitWork.InvoiceMaster.Update(invmaster);
                            _unitWork.SaveTally();
                            _unitWork.SaveChanges();
                        }
                        else
                        {
                            Message = "Please Update ExchangeRate for All Invoice!";
                        }
                    }
                    else
                    {
                        Message = "Seleted InvoiceNo Already Copyed to Integration";
                    }
                }
                return Message;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //service for i
        //nvoice auto calc
        public IEnumerable<ViewAutoGetGenerateforInvoice> ServiceDetailInvoice()
        {
            var servicedata = _unitviewWork.ViewAutoGetGenerateforInvoice.OrderByDescending(x => x.Id).ToList();
            return servicedata;
        }
        //apply waiver for jobs
        public Object GetWaiverJobWithclientIdfileName(waiverpara jobOrder)
        {
            var WaiverJob = new List<WaiverJobforInvoice_Result>();
            if (jobOrder.DateofUpload == null)
            {
                WaiverJob = _unitprocedure.WaiverJobforInvoice(jobOrder.DepartmentId, jobOrder.ClientId, jobOrder.FileName).Where(x => x.IsWaiver == null && x.IsSpecialPrice != true).ToList();
            }
            else if (jobOrder.DateofUpload != null)
            {
                WaiverJob = _unitprocedure.WaiverJobforInvoice(jobOrder.DepartmentId, jobOrder.ClientId, jobOrder.FileName).Where(x => (x.JobDate >= jobOrder.JobClosedUTC && x.JobDate <= jobOrder.DateofUpload) && (x.IsWaiver == null && x.IsSpecialPrice != true)).ToList();
            }
            var result = new
            {
                WaiverJobList = WaiverJob
            };
            return result;
        }

        public object AddWaiverJob(List<JobOrder2> jobOrder)
        {
            bool status = false;
            var result = new
            {
                Success = false,
                Message = ""
            };
            var JIds = jobOrder.Select(x => x.Id);
            var updateOrders = _unitWork.JobOrder.Where(x => JIds.Contains(x.Id)).ToList();
            updateOrders.ForEach(x => x.IsWaiver = true);
            long dbStatus = _unitWork.Save();
            status = dbStatus > 0;
            if (status == true)
            {
                result = new
                {
                    Success = true,
                    Message = "Selected Job(s) updated as Waiver Successfully ...!"
                };
            }
            return result;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _tableWork.CustomerRepository.Get(x => x.IsDeleted == false).OrderBy(x => x.Name).ToList();
        }
        public string SubmitInvoiceMonth(SubmitInvoiceMonth submitInvoiceMonth)
        {
            var message = "Invoice month submitted successfully.";

            var invoice = _db.InvoiceMasters.FirstOrDefault(i => i.InvoiceNo == submitInvoiceMonth.toInvoiceNumber);

            if (invoice == null)
            {
                throw new MessageNotFoundException("To invoice number not found.");
            }

            var user = _db.Employees.FirstOrDefault(e => e.EmployeeId == submitInvoiceMonth.createdBy && e.IsDeleted == false);

            if (user == null)
            {
                throw new MessageNotFoundException("User not found.");
            }

            var fromInvoiceNumber = _unitWork.InvoiceMaster
                .Where(i => (i.InvoiceMonth == 0 || i.InvoiceMonth == null) &&
                            i.IsDeleted == false)
                .Select(im => im.InvoiceNo)
                .FirstOrDefault();

            var existingapprovedInvoices = _db.InvoiceDateUpdates
                .Any(idu =>
                    idu.FromInvoiceNumber == fromInvoiceNumber &&
                    idu.ToInvoiceNumber == submitInvoiceMonth.toInvoiceNumber &&
                    idu.IsApproved == true);

            if (existingapprovedInvoices)
            {
                throw new InvalidOperationException("Invoices are already updated.");
            }

            if (!existingapprovedInvoices)
            {
                InvoiceDateUpdate dateSubmit = new InvoiceDateUpdate
                {
                    FromInvoiceNumber = fromInvoiceNumber,
                    ToInvoiceNumber = submitInvoiceMonth.toInvoiceNumber,
                    Month = submitInvoiceMonth.month,
                    Year = submitInvoiceMonth.year,
                    CreatedBy = submitInvoiceMonth.createdBy,
                    CreatedUtc = DateTime.UtcNow
                };

                _db.Add(dateSubmit);
                _db.SaveChanges();
            }
            return message;
        }
        public List<CurrentMonthInvoice> GetCurrentMonthInvoice()
        {
            var currentMonthInvoices = _db.InvoiceMasters
                .Where(i => i.IsDeleted == false)
                .Select(i => new CurrentMonthInvoice
                {
                    InvoiceNo = i.InvoiceNo
                })
                .ToList();

            return currentMonthInvoices;
        }
        public List<SubmitInvoiceMonthRequest> GetUnApprovedInvoice()
        {
            var unapprovedInvoices = _db.InvoiceDateUpdates
                    .Where(idu => idu.IsApproved == false)
                    .Select(idu => new SubmitInvoiceMonthRequest
                    {
                        fromInvoiceNumber = idu.FromInvoiceNumber,
                        toInvoiceNumber = idu.ToInvoiceNumber,
                        year = idu.Year,
                        month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(idu.Month),
                        createdBy = idu.CreatedBy
                    })
                    .ToList();
            return unapprovedInvoices;
        }
        public string UpdateInvoiceMonth(UpdateInvoiceMonth updateInvoiceMonth)
        {
            var message = "Invoice month updated";

            var selectedRows = updateInvoiceMonth.SelectedRows;
            foreach (var item in selectedRows)
            {
                if (item.Approved == "Approved")
                {
                    var invoiceDateUpdate = _db.InvoiceDateUpdates.FirstOrDefault(i => i.FromInvoiceNumber == item.FromInvoiceNumber && i.ToInvoiceNumber == item.ToInvoiceNumber && i.IsApproved == false);
                    invoiceDateUpdate.IsApproved = true;
                    invoiceDateUpdate.ApprovedBy = updateInvoiceMonth.ApprovedBy;

                    var invoiceId1 = _unitWork.InvoiceMaster.Where(i => i.InvoiceNo == item.FromInvoiceNumber && i.IsDeleted == false).Select(im => im.Id).FirstOrDefault();
                    var invoiceId2 = _unitWork.InvoiceMaster.Where(i => i.InvoiceNo == item.ToInvoiceNumber && i.IsDeleted == false).Select(im => im.Id).FirstOrDefault();

                    var invoiceMaster = _unitWork.InvoiceMaster
                        .Where(i =>
                            i.Id >= invoiceId1 && i.Id <= invoiceId2 && i.IsDeleted == false)
                        .ToList();

                    if (invoiceMaster != null || invoiceMaster.Count > 0)
                    {
                        foreach (var invoice in invoiceMaster)
                        {
                            if (invoice.InvoiceMonth == 0 || invoice.InvoiceMonth == null)
                            {
                                invoice.InvoiceMonth = item.Month;
                                invoice.InvoiceYear = item.Year;
                                invoice.IsUpdate = true;
                            }
                        }
                    }
                    else
                    {
                        message = "Between the invoices not found";
                    }
                }
                else
                {
                    message = "Invoice month not updated.";
                }
                _db.SaveChanges();
            }

            return message;
        }

        //private void ValidateMonthAndYear(int month, int year)
        //{
        //    if (month < 1 || month > 12)
        //    {
        //        throw new InvalidOperationException("Invalid month. Month must be between 1 and 12.");
        //    }

        //    if (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year)
        //    {
        //        throw new InvalidOperationException("Invalid year. Please provide a valid year.");
        //    }
        //}

        //private string GetLastInvoiceOfPreviousMonth(DateTime lastDayOfPreviousMonth)
        //{
        //    var fromInvoiceNumber = _db.InvoiceMasters
        //        .Where(m => m.InvoiceDate <= lastDayOfPreviousMonth)
        //        .OrderByDescending(m => m.InvoiceDate)
        //        .Select(m => m.InvoiceNo)
        //        .FirstOrDefault();

        //    return fromInvoiceNumber;
        //}

        public IEnumerable<MonthlyRevenueResult> GetMonthlyRevenue(int? clientId, int? divisionId, int? departmentId, int? scopeId, int? startYear, int? startMonth, int? endYear, int? endMonth)
        {
            var parameters = new[]
            {
                new SqlParameter("@ClientId", clientId ?? (object)DBNull.Value),
                new SqlParameter("@DivisionId", divisionId ?? (object)DBNull.Value),
                new SqlParameter("@DepartmentId", departmentId ?? (object)DBNull.Value),
                new SqlParameter("@ScopeId", scopeId ?? (object)DBNull.Value),
                new SqlParameter("@StartYear", startYear ?? (object)DBNull.Value),
                new SqlParameter("@EndYear", endYear ?? (object)DBNull.Value),
                new SqlParameter("@StartMonth", startMonth ?? (object)DBNull.Value),
                new SqlParameter("@EndMonth", endMonth ?? (object)DBNull.Value)
            };

            var results = _procedureWork.ExecStoredProcedure<MonthlyRevenueResult>("EXEC GetMonthlyRevenueDetails @ClientId, @DivisionId, @DepartmentId, @ScopeId, @StartYear, @EndYear, @StartMonth, @EndMonth", parameters).ToList();

            if (results.Count == 0)
            {
                throw new MessageNotFoundException("Revenue not found.");
            }

            return results;
        }

        public string ClubInvoices(ClubInvoicesDTO clubInvoicesDTO)
        {
            string Message = "Invoice Generated for Selected Item(s)";
            var customerName = "";
            db.ChangeTracker.AutoDetectChangesEnabled = false;

            var invoiceMasters = _unitWork.InvoiceMaster
                .Where(i => clubInvoicesDTO.InvoiceNumbers.Contains(i.InvoiceNo))
                .ToList();

            int selectedCustomerId = clubInvoicesDTO.SelectedCustomerId;

            foreach (var item in invoiceMasters)
            {
                var invoiceId = item.Id;
                var selectedJobs = _unitWork.InvoiceJob.Where(j => j.Imid == invoiceId).ToList();

                foreach (var item1 in selectedJobs)
                {
                    var jobOrder1 = _unitWork.JobOrder.Where(j => j.Id == item1.Jid).FirstOrDefault();

                    try
                    {
                        var checkspecialprice = _unitWork.JobOrder.Where(x => x.Id == item1.Jid && x.IsDeleted == false && x.DateofUpload != null && x.SpecialPrice != null)
                        .Select(x => new { x.PricingTypeId, x.IsSpecialPrice, x.SpecialPrice, x.IsWaiver }).FirstOrDefault();
                        var tran = _unitWork.InvoiceTran.Where(i => i.Id == item1.It).FirstOrDefault();

                        if (checkspecialprice != null)
                        {
                            var jobOrder = _unitWork.JobOrder.Where(j => j.ClientId == item.CustomerId && j.Id == item1.Jid).FirstOrDefault();
                            var invoicemaster = _unitWork.InvoiceMaster.FirstOrDefault(x => x.CustomerId == jobOrder.ClientId && x.InvoiceNo == null);
                            var jidExists = _unitWork.InvoiceJob.FirstOrDefault(x => x.Jid == jobOrder.Id) != null;

                            var dapartmentname = _unitWork.Department.FirstOrDefault(x => x.Id == jobOrder.DepartmentId && x.IsDeleted == false).Description;
                            if (invoicemaster == null)
                            {
                                var saveinvoicemaster = new InvoiceMaster();
                                saveinvoicemaster.ReferenceNo = "123";
                                saveinvoicemaster.ReferenceDate = DateTime.UtcNow;
                                saveinvoicemaster.CustomerId = selectedCustomerId;
                                saveinvoicemaster.IsDeleted = false;
                                saveinvoicemaster.IsCancelled = false;
                                saveinvoicemaster.IsSample = false;
                                saveinvoicemaster.CreatedUtc = DateTime.UtcNow;
                                saveinvoicemaster.CreatedBy = clubInvoicesDTO.ClubbedBy;
                                saveinvoicemaster.IsOutstanding = true;
                                saveinvoicemaster.PrimaryDateTime = DateTime.UtcNow;
                                saveinvoicemaster.PrimaryUpdatedBy = clubInvoicesDTO.ClubbedBy;
                                if (jobOrder.DepartmentId == 1)
                                {
                                    if (checkspecialprice.IsWaiver == true)
                                    {
                                        saveinvoicemaster.ArtInvoiceAmount = checkspecialprice.SpecialPrice;
                                        saveinvoicemaster.DigiInvoiceAmount = 0;
                                        saveinvoicemaster.ArtWaiver = checkspecialprice.SpecialPrice;
                                        saveinvoicemaster.DigiWaiver = 0;
                                    }
                                    else
                                    {
                                        saveinvoicemaster.ArtInvoiceAmount = checkspecialprice.SpecialPrice;
                                        saveinvoicemaster.DigiInvoiceAmount = 0;
                                        saveinvoicemaster.ArtWaiver = 0;
                                        saveinvoicemaster.DigiWaiver = 0;
                                    }
                                }
                                else if (jobOrder.DepartmentId == 2)
                                {
                                    if (checkspecialprice.IsWaiver == true)
                                    {
                                        saveinvoicemaster.ArtInvoiceAmount = 0;
                                        saveinvoicemaster.DigiInvoiceAmount = checkspecialprice.SpecialPrice;
                                        saveinvoicemaster.DigiWaiver = checkspecialprice.SpecialPrice;
                                        saveinvoicemaster.ArtWaiver = 0;
                                    }
                                    else
                                    {
                                        saveinvoicemaster.DigiInvoiceAmount = checkspecialprice.SpecialPrice;
                                        saveinvoicemaster.ArtInvoiceAmount = 0;
                                        saveinvoicemaster.ArtWaiver = 0;
                                        saveinvoicemaster.DigiWaiver = 0;
                                    }
                                }
                                saveinvoicemaster.ProductValue = checkspecialprice.SpecialPrice;
                                saveinvoicemaster.Discount = 0;
                                saveinvoicemaster.Waiver = saveinvoicemaster.ArtWaiver + saveinvoicemaster.DigiWaiver;
                                saveinvoicemaster.ReceivableValue = saveinvoicemaster.ProductValue - saveinvoicemaster.Waiver;
                                decimal invoicevalue = Convert.ToDecimal(saveinvoicemaster.ReceivableValue);
                                saveinvoicemaster.RoundOff = Math.Round(invoicevalue, 2) - saveinvoicemaster.ReceivableValue;
                                saveinvoicemaster.InvoiceValue = Math.Round(invoicevalue, 2);
                                saveinvoicemaster.TotalInvoiceValue = Math.Round(invoicevalue, 2);
                                _unitWork.InvoiceMaster.Add(saveinvoicemaster);
                                _unitWork.SaveChanges();

                                tran.Imid = saveinvoicemaster.Id;
                                tran.UpdatedBy = clubInvoicesDTO.ClubbedBy;
                                tran.UpdatedUtc = DateTime.UtcNow;
                                _unitWork.InvoiceTran.Update(tran);
                                _unitWork.SaveChanges();

                                item1.Imid = saveinvoicemaster.Id;
                                item1.UpdatedBy = clubInvoicesDTO.ClubbedBy;
                                item1.UpdatedUtc = DateTime.UtcNow;
                                _unitWork.InvoiceJob.Update(item1);
                                if (jidExists) _unitWork.SaveChanges();
                            }
                            else
                            {
                                tran.Imid = invoicemaster.Id;
                                tran.UpdatedBy = clubInvoicesDTO.ClubbedBy;
                                tran.UpdatedUtc = DateTime.UtcNow;
                                _unitWork.InvoiceTran.Update(tran);
                                _unitWork.SaveChanges();

                                item1.Imid = invoicemaster.Id;
                                item1.UpdatedBy = clubInvoicesDTO.ClubbedBy;
                                item1.UpdatedUtc = DateTime.UtcNow;
                                _unitWork.InvoiceJob.Update(item1);
                                if (jidExists) _unitWork.SaveChanges();

                                invoicemaster.ProductValue = invoicemaster.ProductValue + checkspecialprice.SpecialPrice;
                                if (jobOrder.DepartmentId == 1)
                                {
                                    if (checkspecialprice.IsWaiver == true)
                                    {
                                        invoicemaster.ArtInvoiceAmount = invoicemaster.ArtInvoiceAmount + checkspecialprice.SpecialPrice;
                                        invoicemaster.ArtWaiver = invoicemaster.ArtWaiver + checkspecialprice.SpecialPrice;
                                        invoicemaster.Waiver = invoicemaster.ArtWaiver + invoicemaster.DigiWaiver;
                                    }
                                    else
                                    {
                                        invoicemaster.ArtInvoiceAmount = invoicemaster.ArtInvoiceAmount + checkspecialprice.SpecialPrice;
                                    }
                                }
                                else if (jobOrder.DepartmentId == 2)
                                {
                                    if (checkspecialprice.IsWaiver == true)
                                    {
                                        invoicemaster.DigiInvoiceAmount = invoicemaster.DigiInvoiceAmount + checkspecialprice.SpecialPrice;
                                        invoicemaster.DigiWaiver = invoicemaster.DigiWaiver + checkspecialprice.SpecialPrice;
                                        invoicemaster.Waiver = invoicemaster.ArtWaiver + invoicemaster.DigiWaiver;
                                    }
                                    else
                                    {
                                        invoicemaster.DigiInvoiceAmount = invoicemaster.DigiInvoiceAmount + checkspecialprice.SpecialPrice;
                                    }
                                }
                                invoicemaster.ReceivableValue = invoicemaster.ProductValue - invoicemaster.Waiver;
                                decimal invoicevalue = Convert.ToDecimal(invoicemaster.ReceivableValue);
                                invoicemaster.RoundOff = Math.Round(invoicevalue, 2) - invoicemaster.ReceivableValue;
                                invoicemaster.InvoiceValue = Math.Round(invoicevalue, 2);
                                invoicemaster.TotalInvoiceValue = Math.Round(invoicevalue, 2);
                                _unitWork.InvoiceMaster.Update(invoicemaster);
                            }
                            _unitWork.SaveChanges();
                        }
                        else
                        {
                            var myCollection = new List<InvoiceJob>();
                            var jobOrder = _unitWork.JobOrder.Where(j => j.Id == item1.Jid).FirstOrDefault();
                            var jidExists = _unitWork.InvoiceJob.FirstOrDefault(x => x.Jid == jobOrder.Id) != null;

                            var alreadyexiststaffingpricing = _unitprocedure.GetCheckStaffing(jobOrder.ClientId, jobOrder.DepartmentId, jobOrder.FileReceivedDate);
                            int staffingdesig = alreadyexiststaffingpricing.Count();
                            if (staffingdesig > 0)
                            { //staffing if start
                                var getstaffing = alreadyexiststaffingpricing.Where(x => x.FromDate <= jobOrder.FileReceivedDate && x.ToDate >= jobOrder.FileReceivedDate).ToList();
                                decimal sumofprice = Convert.ToDecimal(getstaffing.Select(x => x.Price).Sum());
                                var checkspecialpriceold = _unitWork.JobOrder.Where(x => x.Id == item1.Jid && x.IsDeleted == false && x.DateofUpload != null && x.SpecialPrice != null)
                                    .Select(x => new { x.PricingTypeId, x.IsSpecialPrice, x.SpecialPrice }).FirstOrDefault();

                                var invoicemasterId = _unitWork.InvoiceMaster.Where(x => x.Id == item1.Imid && x.InvoiceNo == null).Select(x => x.Id).FirstOrDefault();
                                var dapartmentname = _unitWork.Department.FirstOrDefault(x => x.Id == jobOrder.DepartmentId && x.IsDeleted == false).Description;
                                if (invoicemasterId == null || invoicemasterId == 0)
                                {
                                    var saveinvoicemaster = new InvoiceMaster();
                                    saveinvoicemaster.ReferenceDate = DateTime.UtcNow;
                                    saveinvoicemaster.ReferenceNo = "123";
                                    saveinvoicemaster.CustomerId = selectedCustomerId;
                                    saveinvoicemaster.ProductValue = sumofprice;
                                    saveinvoicemaster.Discount = 0;
                                    saveinvoicemaster.ArtWaiver = 0;
                                    saveinvoicemaster.DigiWaiver = 0;
                                    saveinvoicemaster.Waiver = 0;
                                    saveinvoicemaster.ReceivableValue = saveinvoicemaster.ProductValue;
                                    decimal invoicevalue = Convert.ToDecimal(saveinvoicemaster.ReceivableValue);
                                    saveinvoicemaster.RoundOff = Math.Round(invoicevalue, 2) - saveinvoicemaster.ReceivableValue;
                                    saveinvoicemaster.InvoiceValue = Math.Round(invoicevalue, 2);
                                    saveinvoicemaster.TotalInvoiceValue = Math.Round(invoicevalue, 2);
                                    saveinvoicemaster.IsDeleted = false;
                                    saveinvoicemaster.IsCancelled = false;
                                    saveinvoicemaster.IsSample = false;
                                    saveinvoicemaster.CreatedBy = clubInvoicesDTO.ClubbedBy;
                                    saveinvoicemaster.CreatedUtc = DateTime.UtcNow;
                                    saveinvoicemaster.IsOutstanding = true;
                                    saveinvoicemaster.PrimaryUpdatedBy = clubInvoicesDTO.ClubbedBy;
                                    saveinvoicemaster.PrimaryDateTime = DateTime.UtcNow;
                                    _unitWork.InvoiceMaster.Add(saveinvoicemaster);
                                    _unitWork.SaveChanges();

                                    tran.Imid = saveinvoicemaster.Id;
                                    tran.UpdatedBy = clubInvoicesDTO.ClubbedBy;
                                    tran.UpdatedUtc = DateTime.UtcNow;
                                    _unitWork.InvoiceTran.Update(tran);
                                    _unitWork.SaveChanges();

                                    item1.Imid = saveinvoicemaster.Id;
                                    item1.UpdatedBy = clubInvoicesDTO.ClubbedBy;
                                    item1.UpdatedUtc = DateTime.UtcNow;
                                    _unitWork.InvoiceJob.Update(item1);
                                    if (jidExists) _unitWork.SaveChanges();
                                }
                                else
                                {
                                    var updateinvoicemaster = _unitWork.InvoiceMaster.FirstOrDefault(x => x.Id == invoicemasterId);
                                    var itID = _unitWork.InvoiceTran.FirstOrDefault(x => x.Imid == invoicemasterId);
                                    var checkexicting = _unitWork.InvoiceTran.FirstOrDefault(x => x.Imid == invoicemasterId && x.DepartmentId == jobOrder.DepartmentId && x.PricingTypeId == 5 || x.PricingTypeId == 15);
                                    if (itID.PricingTypeId == 5 || itID.PricingTypeId == 15)
                                    {

                                    }
                                    else
                                    {
                                        if (checkexicting == null)
                                        {
                                            _unitWork.InvoiceTran.Update(tran);
                                            _unitWork.SaveChanges();

                                            var totalTranRecords = new List<InvoiceTran>();
                                            var myCheckForIT = _unitWork.InvoiceTran.Where(x => x.Imid == updateinvoicemaster.Id && x.PricingTypeId == 4).OrderByDescending(x => x.Id).FirstOrDefault();
                                            var tranRecord = _unitWork.InvoiceTran.Local().ToList();
                                            var subQuery = tranRecord.Select(x => x.Id).ToList();
                                            var tranTotalRecordsWithIM = _unitWork.InvoiceTran.Where(x => !subQuery.Contains(x.Id) && x.Imid == updateinvoicemaster.Id).ToList();
                                            totalTranRecords = tranTotalRecordsWithIM.Concat(tranRecord).ToList();
                                            var totalInvoiceValue = totalTranRecords.Where(x => x.Imid == updateinvoicemaster.Id).ToList();
                                            var invoiceValue = totalInvoiceValue.Sum(x => x.Value);
                                            Decimal deciinv = Convert.ToDecimal(invoiceValue);
                                            updateinvoicemaster.ProductValue = invoiceValue;
                                            updateinvoicemaster.InvoiceValue = Math.Round(deciinv, 2);
                                            updateinvoicemaster.Discount = 0;
                                            updateinvoicemaster.UpdatedBy = clubInvoicesDTO.ClubbedBy;
                                            updateinvoicemaster.UpdatedUtc = DateTime.UtcNow;
                                            updateinvoicemaster.PrimaryUpdatedBy = clubInvoicesDTO.ClubbedBy;
                                            updateinvoicemaster.PrimaryDateTime = DateTime.UtcNow;

                                            var invoicetranDetails = _unitWork.InvoiceTran.Local().Where(x => x.Id == 0).OrderByDescending(x => x.Id).FirstOrDefault();
                                            if (invoicetranDetails == null && myCheckForIT != null)
                                            {
                                                invoicetranDetails = _unitWork.InvoiceTran.Local().OrderByDescending(x => x.Id == myCheckForIT.Id).FirstOrDefault();
                                            }
                                            var invoiceid = _unitWork.InvoiceMaster.Local().FirstOrDefault();
                                            updateinvoicemaster.Waiver = invoiceid.ArtWaiver + invoiceid.DigiWaiver;
                                            if (invoiceid.Waiver != null)
                                            {
                                                updateinvoicemaster.ReceivableValue = invoiceid.ProductValue - invoiceid.Waiver;
                                            }
                                            else
                                            {
                                                updateinvoicemaster.ReceivableValue = invoiceid.ProductValue;
                                            }
                                            Decimal reciroundoff = Convert.ToDecimal(invoiceid.ReceivableValue);
                                            updateinvoicemaster.InvoiceValue = Math.Round(reciroundoff, 2);
                                            updateinvoicemaster.TotalInvoiceValue = Math.Round(reciroundoff, 2);
                                            updateinvoicemaster.RoundOff = Math.Round(reciroundoff, 2) - invoiceid.ReceivableValue;
                                            _unitWork.InvoiceMaster.Update(updateinvoicemaster);
                                            _unitWork.SaveChanges();
                                        }
                                        else
                                        {

                                        }
                                    }
                                    var updatetaninvoice = _unitWork.InvoiceTran.FirstOrDefault(x => x.Imid == invoicemasterId && x.PricingTypeId == 5 || x.PricingTypeId == 15);
                                    if (checkexicting != null)
                                    {
                                        updatetaninvoice.Value = tran.Value;
                                        var totalTranRecords = new List<InvoiceTran>();
                                        var myCheckForIT = _unitWork.InvoiceTran.Where(x => x.Imid == updateinvoicemaster.Id && x.PricingTypeId == 4).OrderByDescending(x => x.Id).FirstOrDefault();
                                        var tranRecord = _unitWork.InvoiceTran.Local().ToList();
                                        var subQuery = tranRecord.Select(x => x.Id).ToList();
                                        var tranTotalRecordsWithIM = _unitWork.InvoiceTran.Where(x => !subQuery.Contains(x.Id) && x.Imid == updateinvoicemaster.Id).ToList();
                                        totalTranRecords = tranTotalRecordsWithIM.Concat(tranRecord).ToList();
                                        var totalInvoiceValue = totalTranRecords.Where(x => x.Imid == updateinvoicemaster.Id).ToList();
                                        var invoiceValue = totalInvoiceValue.Sum(x => x.Value);
                                        Decimal deciinv = Convert.ToDecimal(invoiceValue);
                                        updateinvoicemaster.ProductValue = invoiceValue;
                                        updateinvoicemaster.InvoiceValue = Math.Round(deciinv, 2);
                                        updateinvoicemaster.Discount = 0;
                                        updateinvoicemaster.UpdatedBy = clubInvoicesDTO.ClubbedBy;
                                        updateinvoicemaster.UpdatedUtc = DateTime.UtcNow;
                                        updateinvoicemaster.PrimaryUpdatedBy = clubInvoicesDTO.ClubbedBy;
                                        updateinvoicemaster.PrimaryDateTime = DateTime.UtcNow;

                                        var invoiceid = _unitWork.InvoiceMaster.Local().FirstOrDefault();
                                        updateinvoicemaster.Waiver = invoiceid.ArtWaiver + invoiceid.DigiWaiver;
                                        if (invoiceid.Waiver != null)
                                        {
                                            updateinvoicemaster.ReceivableValue = invoiceid.ProductValue - invoiceid.Waiver;
                                        }
                                        else
                                        {
                                            updateinvoicemaster.ReceivableValue = invoiceid.ProductValue;
                                        }
                                        Decimal reciroundoff = Convert.ToDecimal(invoiceid.ReceivableValue);
                                        updateinvoicemaster.InvoiceValue = Math.Round(reciroundoff, 2);
                                        updateinvoicemaster.TotalInvoiceValue = Math.Round(reciroundoff, 2);
                                        updateinvoicemaster.RoundOff = Math.Round(reciroundoff, 2) - invoiceid.ReceivableValue;
                                        _unitWork.InvoiceMaster.Update(updateinvoicemaster);
                                        _unitWork.InvoiceTran.Update(updatetaninvoice);
                                        _unitWork.SaveChanges();
                                    }
                                    else
                                    {
                                        item1.It = item1.Id;

                                    }
                                    item1.Imid = updateinvoicemaster.Id;
                                    item1.UpdatedBy = clubInvoicesDTO.ClubbedBy;
                                    item1.UpdatedUtc = DateTime.UtcNow;
                                    _unitWork.InvoiceJob.Update(item1);
                                    if (jidExists) _unitWork.SaveChanges();
                                }
                            } //staffing if end
                            else
                            { // else start

                                myCollection.Add(item1);
                                var invoicemasterId = _unitWork.InvoiceMaster.Where(x => x.CustomerId == selectedCustomerId && x.InvoiceNo == null).Select(x => x.Id).FirstOrDefault();
                                var dapartmentname = _unitWork.Department.FirstOrDefault(x => x.Id == jobOrder.DepartmentId && x.IsDeleted == false).Description;
                                var cusvsscope = _unitWork.CustomerVsScope.FirstOrDefault(x => x.ScopeId == jobOrder.ScopeId.Value);
                                if (invoicemasterId == null || invoicemasterId == 0)
                                {
                                    var saveinvoicemaster = new InvoiceMaster();
                                    saveinvoicemaster.ReferenceDate = DateTime.UtcNow;
                                    saveinvoicemaster.ReferenceNo = "123";
                                    saveinvoicemaster.CustomerId = selectedCustomerId;
                                    if (jobOrder.DepartmentId == 1)
                                    {
                                        if (jobOrder.IsWaiver == true)
                                        {
                                            saveinvoicemaster.ArtInvoiceAmount = productvalue;
                                            saveinvoicemaster.DigiInvoiceAmount = 0;
                                            saveinvoicemaster.ArtWaiver = productvalue;
                                            saveinvoicemaster.DigiWaiver = 0;
                                            //saveinvoicemaster.Waiver = productvalue;
                                        }
                                        else
                                        {
                                            saveinvoicemaster.ArtInvoiceAmount = productvalue;
                                            saveinvoicemaster.DigiInvoiceAmount = 0;
                                            saveinvoicemaster.ArtWaiver = 0;
                                            saveinvoicemaster.DigiWaiver = 0;
                                        }
                                    }
                                    else if (jobOrder.DepartmentId == 2)
                                    {
                                        if (jobOrder.IsWaiver == true)
                                        {
                                            saveinvoicemaster.ArtInvoiceAmount = 0;
                                            saveinvoicemaster.DigiInvoiceAmount = productvalue;
                                            saveinvoicemaster.DigiWaiver = productvalue;
                                            saveinvoicemaster.ArtWaiver = 0;
                                            //saveinvoicemaster.Waiver = productvalue;
                                        }
                                        else
                                        {
                                            saveinvoicemaster.DigiInvoiceAmount = productvalue;
                                            saveinvoicemaster.ArtInvoiceAmount = 0;
                                            saveinvoicemaster.ArtWaiver = 0;
                                            saveinvoicemaster.DigiWaiver = 0;
                                        }
                                    }
                                    saveinvoicemaster.ProductValue = productvalue;
                                    saveinvoicemaster.Discount = 0;
                                    saveinvoicemaster.Waiver = saveinvoicemaster.ArtWaiver + saveinvoicemaster.DigiWaiver;
                                    saveinvoicemaster.ReceivableValue = saveinvoicemaster.ProductValue - saveinvoicemaster.Waiver;
                                    decimal invoicevalue = Convert.ToDecimal(saveinvoicemaster.ReceivableValue);
                                    saveinvoicemaster.RoundOff = Math.Round(invoicevalue, 2) - saveinvoicemaster.ReceivableValue;
                                    saveinvoicemaster.InvoiceValue = Math.Round(invoicevalue, 2);
                                    saveinvoicemaster.TotalInvoiceValue = Math.Round(invoicevalue, 2);
                                    saveinvoicemaster.IsDeleted = false;
                                    saveinvoicemaster.IsCancelled = false;
                                    saveinvoicemaster.IsSample = false;
                                    saveinvoicemaster.CreatedBy = clubInvoicesDTO.ClubbedBy;
                                    saveinvoicemaster.CreatedUtc = DateTime.UtcNow;
                                    saveinvoicemaster.IsOutstanding = true;
                                    saveinvoicemaster.PrimaryUpdatedBy = clubInvoicesDTO.ClubbedBy;
                                    saveinvoicemaster.PrimaryDateTime = DateTime.UtcNow;
                                    _unitWork.InvoiceMaster.Add(saveinvoicemaster);
                                    _unitWork.SaveChanges();

                                    tran.Imid = saveinvoicemaster.Id;
                                    tran.UpdatedBy = clubInvoicesDTO.ClubbedBy;
                                    tran.UpdatedUtc = DateTime.UtcNow;
                                    _unitWork.InvoiceTran.Update(tran);
                                    _unitWork.SaveChanges();

                                    item1.Imid = saveinvoicemaster.Id;
                                    item1.UpdatedBy = clubInvoicesDTO.ClubbedBy;
                                    item1.UpdatedUtc = DateTime.UtcNow;
                                    _unitWork.InvoiceJob.Update(item1);
                                    if (jidExists) _unitWork.SaveChanges();
                                }
                                else
                                {
                                    var updateinvoicemaster = _unitWork.InvoiceMaster.FirstOrDefault(x => x.Id == invoicemasterId);

                                    var totalTranRecords = new List<InvoiceTran>();
                                    //Harsha removed rate for this query because, for rate concession, 1-5 jobs 5$, 1-10 jobs 7$ means, total jobs have to affect 7$, so all time update willl be there
                                    var myCheckForIT = _unitWork.InvoiceTran.Where(x => x.Imid == updateinvoicemaster.Id && x.ScopeId == jobOrder.ScopeId.Value && x.PricingTypeId == 4 || (x.PricingTypeId == 16 || x.PricingTypeId == 17)).OrderByDescending(x => x.Id).FirstOrDefault();

                                    tran.Imid = updateinvoicemaster.Id;
                                    tran.UpdatedBy = clubInvoicesDTO.ClubbedBy;
                                    tran.UpdatedUtc = DateTime.UtcNow;
                                    _unitWork.InvoiceTran.Update(tran);
                                    _unitWork.SaveChanges();

                                    item1.Imid = updateinvoicemaster.Id;
                                    item1.UpdatedBy = clubInvoicesDTO.ClubbedBy;
                                    item1.UpdatedUtc = DateTime.UtcNow;
                                    _unitWork.InvoiceJob.Update(item1);
                                    _unitWork.SaveChanges();

                                    var tranRecord = _unitWork.InvoiceTran.Local().ToList();
                                    var subQuery = tranRecord.Select(x => x.Id).ToList();
                                    var tranTotalRecordsWithIM = _unitWork.InvoiceTran.Where(x => !subQuery.Contains(x.Id) && x.Imid == updateinvoicemaster.Id).ToList();
                                    totalTranRecords = tranTotalRecordsWithIM.Concat(tranRecord).ToList();

                                    var totalInvoiceValue = totalTranRecords.Where(x => x.Imid == updateinvoicemaster.Id).ToList();
                                    var invoiceValue = totalInvoiceValue.Sum(x => x.Value);

                                    Decimal deciinv = Convert.ToDecimal(invoiceValue);
                                    updateinvoicemaster.ProductValue = invoiceValue;
                                    updateinvoicemaster.InvoiceValue = Math.Round(deciinv, 2);
                                    updateinvoicemaster.Discount = 0;
                                    updateinvoicemaster.UpdatedBy = item1.CreatedBy;
                                    updateinvoicemaster.UpdatedUtc = DateTime.UtcNow;
                                    updateinvoicemaster.PrimaryUpdatedBy = item1.CreatedBy;
                                    updateinvoicemaster.PrimaryDateTime = DateTime.UtcNow;
                                    _unitWork.InvoiceMaster.Update(updateinvoicemaster);

                                    var it = _unitWork.InvoiceJob.Local().FirstOrDefault(x => x.Jid == jobOrder.Id);
                                    var totalamount = _unitWork.InvoiceTran.Local().FirstOrDefault(x => x.Id == it.It);
                                    var sumOfRate1 = _unitWork.InvoiceTran.Local().Where(x => x.Imid == invoicemasterId && x.DepartmentId == 1).Sum(x => x.Rate);
                                    var sumOfRate2 = _unitWork.InvoiceTran.Local().Where(x => x.Imid == invoicemasterId && x.DepartmentId == 2).Sum(x => x.Rate);
                                    var invoiceid = _unitWork.InvoiceMaster.Local().FirstOrDefault();
                                    if (jobOrder.DepartmentId == 1 && jobOrder.Id == item1.Jid)
                                    {
                                        if (jobOrder.IsWaiver == true)
                                        {
                                            updateinvoicemaster.ArtWaiver = invoiceid.ArtWaiver + totalamount.Rate;
                                            updateinvoicemaster.ArtInvoiceAmount = sumOfRate1;
                                        }
                                        else
                                        {
                                            updateinvoicemaster.ArtInvoiceAmount = sumOfRate1;
                                        }
                                    }
                                    else if (jobOrder.DepartmentId == 2 && jobOrder.Id == item1.Jid)
                                    {
                                        if (jobOrder.IsWaiver == true)
                                        {
                                            updateinvoicemaster.DigiWaiver = invoiceid.DigiWaiver + totalamount.Rate;
                                            updateinvoicemaster.ArtInvoiceAmount = sumOfRate2;
                                        }
                                        else
                                        {
                                            updateinvoicemaster.DigiInvoiceAmount = sumOfRate2;
                                        }
                                    }
                                    updateinvoicemaster.Waiver = invoiceid.ArtWaiver + invoiceid.DigiWaiver;
                                    if (invoiceid.Waiver != null)
                                    {
                                        updateinvoicemaster.ReceivableValue = invoiceid.ProductValue - invoiceid.Waiver;
                                    }
                                    else
                                    {
                                        updateinvoicemaster.ReceivableValue = invoiceid.ProductValue;
                                    }
                                    Decimal reciroundoff = Convert.ToDecimal(invoiceid.ReceivableValue);
                                    updateinvoicemaster.InvoiceValue = Math.Round(reciroundoff, 2);
                                    updateinvoicemaster.TotalInvoiceValue = Math.Round(reciroundoff, 2);
                                    updateinvoicemaster.RoundOff = Math.Round(reciroundoff, 2) - invoiceid.ReceivableValue;
                                    _unitWork.SaveChanges();
                                }
                                // }
                            } // else end
                        }

                        var jid = selectedJobs.Select(x => x.Jid);
                        var updateOrders = _unitWork.JobOrder.Where(x => jid.Contains(x.Id)).ToList();
                        updateOrders.ForEach(x => x.IsBillable = true);
                    }
                    catch (Exception ex)
                    {
                        Log addlog = new Log();
                        addlog.Module = "Invoice Rep" + item1.Jid;
                        addlog.Description = "Error Msg: " + ex.Message + ", Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source;
                        addlog.ClientId = selectedCustomerId;
                        addlog.JobId = item1.Jid;
                        addlog.Type = "Error Occ";
                        addlog.CreatedUtc = DateTime.UtcNow;
                        _unitWork.Log.Add(addlog);
                        _unitWork.SaveChanges();
                    }
                    finally
                    {
                        db.ChangeTracker.AutoDetectChangesEnabled = false;
                    }
                }

                item.IsClubInvoice = true;
                item.IsDeleted = true;
                _unitWork.InvoiceMaster.Update(item);
                _unitWork.SaveChanges();
            }

            var customername = _unitWork.Customer.FirstOrDefault(x => x.Id == selectedCustomerId && x.IsDeleted == false);
            try
            {
                int selectclientid = selectedCustomerId;
                var getlistoflastinvoice = _unitWork.InvoiceMaster.FirstOrDefault(x => x.CustomerId == selectclientid && x.InvoiceNo == null);
                DateTime jobdate = DateTime.UtcNow;
                var transtype = _unitWork.TransactionType.FirstOrDefault(x => x.Description == "Invoice");
                var VoucherControlDetail = _unitWork.VoucherControl.FirstOrDefault(x => x.TransactionId == transtype.Id && x.EffectiveFrom <= jobdate && x.EffectiveTo >= jobdate && x.IsDeleted == false && x.IsActive == true);
                if (VoucherControlDetail == null)
                {
                    return Message = "VoucherControl is Missing";
                }
                string Prefix = VoucherControlDetail.Prefix.ToString();
                long VoucherNo = VoucherControlDetail.Voucherno + 1;
                var oVoucherControlsingle = _unitWork.VoucherControl.Find(VoucherControlDetail.Id);
                oVoucherControlsingle.Voucherno = VoucherNo;
                oVoucherControlsingle.UpdatedBy = getlistoflastinvoice.CreatedBy;
                oVoucherControlsingle.UpdatedUtc = DateTime.UtcNow;
                _unitWork.VoucherControl.Update(oVoucherControlsingle);
                string month = jobdate.ToString("MM");
                string day = jobdate.ToString("dd");
                int year = jobdate.Year;
                string fmt = "0000.##";
                string safix = VoucherNo.ToString(fmt);
                Invoiceno = Prefix + safix;
                if (getlistoflastinvoice != null)
                {
                    var saveinvoicemaster = _unitWork.InvoiceMaster.FirstOrDefault(x => x.Id == getlistoflastinvoice.Id);
                    int artfilecount = _unitWork.InvoiceTran.Where(x => x.Imid == getlistoflastinvoice.Id && x.DepartmentId == 1).Count();
                    int digifilecount = _unitWork.InvoiceTran.Where(x => x.Imid == getlistoflastinvoice.Id && x.DepartmentId == 2).Count();
                    saveinvoicemaster.InvoiceDate = DateTime.UtcNow;
                    saveinvoicemaster.UpdatedBy = getlistoflastinvoice.CreatedBy;
                    saveinvoicemaster.UpdatedUtc = DateTime.UtcNow;
                    saveinvoicemaster.SecondaryUpdatedBy = getlistoflastinvoice.CreatedBy;
                    saveinvoicemaster.SecondaryDateTime = DateTime.UtcNow;
                    Decimal reciroundof = Convert.ToDecimal(saveinvoicemaster.ReceivableValue);
                    saveinvoicemaster.RoundOff = Math.Round(reciroundof, 2) - saveinvoicemaster.ReceivableValue;
                    saveinvoicemaster.TransactionId = VoucherControlDetail.TransactionId;
                    saveinvoicemaster.ArtFileCount = artfilecount;
                    saveinvoicemaster.DigiFileCount = digifilecount;
                    _unitWork.InvoiceMaster.Update(saveinvoicemaster);
                    saveinvoicemaster.InvoiceNo = Invoiceno;
                    saveinvoicemaster.ClubbedBy = clubInvoicesDTO.ClubbedBy;
                }
                _unitWork.SaveChanges();
            }
            catch (Exception ex)
            {

                Log addlog = new Log();
                addlog.Module = "Confirm Invoice";
                addlog.Description = "Error Msg: " + ex.Message + ", Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source;
                addlog.ClientId = selectedCustomerId;
                addlog.Type = "Error Occd in Confirm Invoice";
                addlog.CreatedUtc = DateTime.UtcNow;
                _unitWork.Log.Add(addlog);
                _unitWork.SaveChanges();

                if (customerName == "")
                {
                    customerName = customername.Name;
                }
                else
                {
                    customerName = customerName + ", " + customername.Name;
                }
            }
            finally
            {
                if (customerName != "")
                {
                    Message = "No Pricing for " + customerName + " Customer";
                }
            }

            return Message;
        }
        private decimal CalculateSlabBasedPrice(int fileCounts, PricingWithScopeViewModel pricingViewModel)
        {
            decimal totalPrice = 0;

            var priceRanges = _unitWork.PricingWithScope
                .Where(p => p.PricingTypeId == pricingViewModel.PricingTypeId && p.ScopeId == pricingViewModel.ScopeId && p.JobStatusId == pricingViewModel.JobStatusId && p.CustomerId == pricingViewModel.CustomerId)
                .ToList();

            int files = fileCounts;

            foreach (var priceRange in priceRanges)
            {
                int rangeFrom = (int)priceRange.From;
                int rangeTo = (int)priceRange.To;

                if (files >= rangeFrom && files <= rangeTo)
                {
                    decimal rangePrice = priceRange.Price ?? 0;
                    totalPrice = rangePrice;
                }
            }

            return totalPrice;
        }
    }
}

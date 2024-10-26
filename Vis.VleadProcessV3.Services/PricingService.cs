using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
   public class PricingService
    {
        private readonly TableWork _tableWork;
        private readonly UnitWork _unitWork;
        private readonly UnitProcedure _unitprocedure;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext db;

        public PricingService(IConfiguration configuration,TableWork tableWork,UnitWork unitWork,
            ApplicationDbContext dbContext,UnitProcedure unitProcedure)
        {
            db = dbContext;
            _configuration = configuration;
            _tableWork = tableWork;
            _unitWork = unitWork;
            _unitprocedure = unitProcedure;
           
        }
        public string AddPricingWithScope(PricingWithScopeViewModel pricingViewModel)
        {
            string status = "Pricing Added Successfully";
            int approve = 1;
            int linkclicked = 0;
            if (pricingViewModel.PricingTypeId == 5 || pricingViewModel.PricingTypeId == 15)
            {
                var alreadyexiststaffingpricing = _unitWork.PricingWithScope.FirstOrDefault(x => x.CustomerId == pricingViewModel.CustomerId && x.WefromDate == pricingViewModel.WEFromDate && x.Designation == pricingViewModel.ScopeTempDesc && x.PricingTypeId == pricingViewModel.PricingTypeId && x.IsDelete != true && x.IsApproved == true && x.IsUpdated != true); //  
                if (alreadyexiststaffingpricing == null)
                {
                    if (pricingViewModel.AddCountDatas.Count() == 0)
                    {
                        PricingWithScope savePricingWithScope = new PricingWithScope();
                        savePricingWithScope.CustomerId = pricingViewModel.CustomerId;
                        savePricingWithScope.DepartmentId = pricingViewModel.DepartmentId;
                        savePricingWithScope.PricingTypeId = pricingViewModel.PricingTypeId;
                        savePricingWithScope.FromDate = pricingViewModel.FromDate;
                        savePricingWithScope.ToDate = pricingViewModel.ToDate;
                        savePricingWithScope.WefromDate = pricingViewModel.WEFromDate;
                        savePricingWithScope.WetoDate = pricingViewModel.WEToDate;
                        savePricingWithScope.Price = pricingViewModel.Price;
                        savePricingWithScope.Designation = pricingViewModel.ScopeTempDesc;
                        savePricingWithScope.IsUpdated = false;
                        savePricingWithScope.CreatedBy = pricingViewModel.CreatedBy;
                        savePricingWithScope.CreatedUtc = DateTime.UtcNow;
                        _unitWork.PricingWithScope.Add(savePricingWithScope);
                        _unitWork.SaveChanges();
                        savePricingWithScope.Id = _unitWork.PricingWithScope.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                        SendGenricMail(savePricingWithScope.Id, pricingViewModel.DepartmentId, pricingViewModel.CustomerId, pricingViewModel.PricingTypeId, 1);
                    }
                    else
                    {
                        foreach (var item in pricingViewModel.AddCountDatas)
                        {
                            var lastrecordofstaffingpricingcount = _unitWork.PricingWithScope.Where(x => x.DepartmentId == pricingViewModel.DepartmentId && x.CustomerId == pricingViewModel.CustomerId && x.WetoDate == null && x.IsDelete != true && x.IsApproved == true && x.IsUpdated != true).ToList(); // 
                            int lapscount = lastrecordofstaffingpricingcount.Count();
                            if (lapscount > 0)
                            {
                                foreach (var lapsitem in lastrecordofstaffingpricingcount)
                                {
                                    lapsitem.WetoDate = pricingViewModel.WEFromDate.AddDays(-1);
                                    lapsitem.UpdatedBy = pricingViewModel.CreatedBy;
                                    lapsitem.UpdatedUtc = DateTime.UtcNow;
                                    lapsitem.IsUpdated = true;
                                    _unitWork.PricingWithScope.Update(lapsitem);
                                }
                                _unitWork.SaveChanges();
                            }
                            PricingWithScope savePricingWithScope = new PricingWithScope();
                            savePricingWithScope.CustomerId = pricingViewModel.CustomerId;
                            savePricingWithScope.DepartmentId = pricingViewModel.DepartmentId;
                            savePricingWithScope.PricingTypeId = pricingViewModel.PricingTypeId;
                            savePricingWithScope.FromDate = item.FromDate;
                            if (item.ToDate != DateTime.MinValue)
                            {
                                savePricingWithScope.ToDate = item.ToDate;
                            }
                            savePricingWithScope.WefromDate = pricingViewModel.WEFromDate;
                            savePricingWithScope.WetoDate = pricingViewModel.WEToDate;
                            savePricingWithScope.Price = item.Price;
                            savePricingWithScope.Designation = item.ScopeTempDesc;
                            savePricingWithScope.IsUpdated = false;
                            savePricingWithScope.CreatedBy = pricingViewModel.CreatedBy;
                            savePricingWithScope.CreatedUtc = DateTime.UtcNow;
                            _unitWork.PricingWithScope.Add(savePricingWithScope);
                            _unitWork.SaveChanges();
                            savePricingWithScope.Id = _unitWork.PricingWithScope.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                            SendGenricMail(savePricingWithScope.Id, pricingViewModel.DepartmentId, pricingViewModel.CustomerId, pricingViewModel.PricingTypeId, 1);
                        }
                    }
                }
                else
                {
                    status = "Already Exist";
                }
            }
            else if (pricingViewModel.PricingTypeId == 16 || pricingViewModel.PricingTypeId == 17)
            {
                if (pricingViewModel.AddCountDatas.Count() == 0)
                {
                    PricingWithScope savePricingWithScopes = new PricingWithScope();
                    savePricingWithScopes.CustomerId = pricingViewModel.CustomerId;
                    savePricingWithScopes.DepartmentId = pricingViewModel.DepartmentId;
                    savePricingWithScopes.ScopeId = pricingViewModel.ScopeId;
                    savePricingWithScopes.PricingTypeId = pricingViewModel.PricingTypeId;
                    savePricingWithScopes.JobStatusId = pricingViewModel.JobStatusId;
                    savePricingWithScopes.From = pricingViewModel.FromRange;
                    savePricingWithScopes.To = pricingViewModel.ToRange;
                    savePricingWithScopes.WefromDate = pricingViewModel.WEFromDate;
                    savePricingWithScopes.Price = pricingViewModel.Price;
                    savePricingWithScopes.IsDelete = false;
                    savePricingWithScopes.IsApproved = true;
                    savePricingWithScopes.ScopeTempDesc = pricingViewModel.ScopeTempDesc;
                    savePricingWithScopes.IsUpdated = false;
                    savePricingWithScopes.CreatedBy = pricingViewModel.CreatedBy;
                    savePricingWithScopes.CreatedUtc = DateTime.UtcNow;

                    _unitWork.PricingWithScope.Add(savePricingWithScopes);
                    _unitWork.SaveChanges();
                }
                else
                {
                    var priceRanges = pricingViewModel.AddCountDatas;
                    foreach (var ranges in priceRanges)
                    {
                        var lastrecordofpricingcount = _unitWork.PricingWithScope.FirstOrDefault(x => x.ScopeId == ranges.ScopeId && x.CustomerId == pricingViewModel.CustomerId && x.WetoDate == null && x.JobStatusId == pricingViewModel.JobStatusId && x.IsDelete != true && x.From == pricingViewModel.FromRange && x.To == pricingViewModel.ToRange && x.IsApproved == true && x.IsUpdated != true); // 
                        if (lastrecordofpricingcount != null)
                        {
                            lastrecordofpricingcount.WetoDate = pricingViewModel.WEFromDate.AddDays(-1);
                            lastrecordofpricingcount.UpdatedBy = pricingViewModel.CreatedBy;
                            lastrecordofpricingcount.UpdatedUtc = DateTime.UtcNow;
                            lastrecordofpricingcount.IsUpdated = true;
                            _unitWork.PricingWithScope.Update(lastrecordofpricingcount);
                            _unitWork.SaveChanges();
                        }
                        PricingWithScope savePricingWithScopes = new PricingWithScope();
                        savePricingWithScopes.CustomerId = ranges.CustomerId;
                        savePricingWithScopes.DepartmentId = ranges.DepartmentId;
                        savePricingWithScopes.ScopeId = ranges.ScopeId;
                        savePricingWithScopes.PricingTypeId = ranges.PricingTypeId;
                        savePricingWithScopes.JobStatusId = ranges.JobStatusId;
                        savePricingWithScopes.From = ranges.FromRange;
                        savePricingWithScopes.To = ranges.ToRange;
                        savePricingWithScopes.WefromDate = ranges.WEFromDate;
                        savePricingWithScopes.Price = ranges.Price;
                        savePricingWithScopes.IsDelete = false;
                        savePricingWithScopes.IsApproved = true;
                        savePricingWithScopes.ScopeTempDesc = ranges.ScopeTempDesc;
                        savePricingWithScopes.IsUpdated = false;
                        savePricingWithScopes.CreatedBy = ranges.CreatedBy;
                        savePricingWithScopes.CreatedUtc = DateTime.UtcNow;

                        _unitWork.PricingWithScope.Add(savePricingWithScopes);
                        _unitWork.SaveChanges();
                    }
                }
            }
            else
            {
                int alreadyexiststaffingpricing = _unitprocedure.GetCheckStaffing(pricingViewModel.CustomerId, pricingViewModel.DepartmentId, pricingViewModel.WEFromDate).Count();

                if (alreadyexiststaffingpricing > 0)
                {
                    status = "Staffing is Available for this Period";
                }
                else
                {
                    var alreadyexistpricing = _unitWork.PricingWithScope.FirstOrDefault(x => x.CustomerId == pricingViewModel.CustomerId && x.WefromDate == pricingViewModel.WEFromDate && x.ScopeId == pricingViewModel.ScopeId && x.PricingTypeId == pricingViewModel.PricingTypeId && x.JobStatusId == pricingViewModel.JobStatusId && x.IsDelete != true && x.IsApproved == true && x.IsUpdated != true);// 

                    if (alreadyexistpricing == null)
                    {
                        var lastrecordofpricing = _unitWork.PricingWithScope.FirstOrDefault(x => x.ScopeId == pricingViewModel.ScopeId && x.CustomerId == pricingViewModel.CustomerId && x.PricingTypeId == pricingViewModel.PricingTypeId && x.WetoDate == null && x.JobStatusId == pricingViewModel.JobStatusId && x.IsDelete != true && x.IsApproved == true && x.IsUpdated != true);//

                        if (lastrecordofpricing != null)
                        {
                            lastrecordofpricing.WetoDate = pricingViewModel.WEFromDate.AddDays(-1);
                            lastrecordofpricing.UpdatedBy = pricingViewModel.CreatedBy;
                            lastrecordofpricing.UpdatedUtc = DateTime.UtcNow;
                            lastrecordofpricing.IsUpdated = true;
                            _unitWork.PricingWithScope.Update(lastrecordofpricing);
                            _unitWork.SaveChanges();
                            status = "Pricing Updated Successfully";
                        }
                        if (pricingViewModel.PricingTypeId == 1 || pricingViewModel.PricingTypeId == 9)
                        {
                            PricingWithScope savePricingWithScope = new PricingWithScope();
                            savePricingWithScope.CustomerId = pricingViewModel.CustomerId;
                            savePricingWithScope.DepartmentId = pricingViewModel.DepartmentId;
                            savePricingWithScope.ScopeId = pricingViewModel.ScopeId;
                            savePricingWithScope.PricingTypeId = pricingViewModel.PricingTypeId;
                            savePricingWithScope.JobStatusId = pricingViewModel.JobStatusId;
                            savePricingWithScope.WefromDate = pricingViewModel.WEFromDate;
                            savePricingWithScope.Price = pricingViewModel.Price;
                            savePricingWithScope.ScopeTempDesc = pricingViewModel.ScopeTempDesc;
                            savePricingWithScope.IsUpdated = false;
                            savePricingWithScope.CreatedBy = pricingViewModel.CreatedBy;
                            savePricingWithScope.CreatedUtc = DateTime.UtcNow;
                            _unitWork.PricingWithScope.Add(savePricingWithScope);
                            _unitWork.SaveChanges();
                            savePricingWithScope.Id = _unitWork.PricingWithScope.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                            SendGenricMail(savePricingWithScope.Id, pricingViewModel.DepartmentId, pricingViewModel.CustomerId, pricingViewModel.PricingTypeId, 1);
                        }
                        else if (pricingViewModel.PricingTypeId == 2)
                        {
                            PricingWithScope savePricingWithScope = new PricingWithScope();
                            savePricingWithScope.CustomerId = pricingViewModel.CustomerId;
                            savePricingWithScope.DepartmentId = pricingViewModel.DepartmentId;
                            savePricingWithScope.ScopeId = pricingViewModel.ScopeId;
                            savePricingWithScope.PricingTypeId = pricingViewModel.PricingTypeId;
                            savePricingWithScope.JobStatusId = pricingViewModel.JobStatusId;
                            savePricingWithScope.RatePerHour = pricingViewModel.RatePerHour;
                            savePricingWithScope.EstimationTime = pricingViewModel.EstimatedTime;
                            savePricingWithScope.WefromDate = pricingViewModel.WEFromDate;
                            savePricingWithScope.Price = pricingViewModel.Price;
                            savePricingWithScope.ScopeTempDesc = pricingViewModel.ScopeTempDesc;
                            savePricingWithScope.IsUpdated = false;
                            savePricingWithScope.CreatedBy = pricingViewModel.CreatedBy;
                            savePricingWithScope.CreatedUtc = DateTime.UtcNow;
                            _unitWork.PricingWithScope.Add(savePricingWithScope);
                            _unitWork.SaveChanges();
                            savePricingWithScope.Id = _unitWork.PricingWithScope.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                            SendGenricMail(savePricingWithScope.Id, pricingViewModel.DepartmentId, pricingViewModel.CustomerId, pricingViewModel.PricingTypeId, 1);
                        }
                        else if (pricingViewModel.PricingTypeId == 13 || pricingViewModel.PricingTypeId == 14)
                        {
                            PricingWithScope savePricingWithScope = new PricingWithScope();
                            savePricingWithScope.CustomerId = pricingViewModel.CustomerId;
                            savePricingWithScope.DepartmentId = pricingViewModel.DepartmentId;
                            savePricingWithScope.ScopeId = pricingViewModel.ScopeId;
                            savePricingWithScope.PricingTypeId = pricingViewModel.PricingTypeId;
                            savePricingWithScope.JobStatusId = pricingViewModel.JobStatusId;
                            savePricingWithScope.RatePerHour = pricingViewModel.RatePerHour;
                            //savePricingWithScope.EstimationTime = pricingViewModel.EstimatedTime;
                            savePricingWithScope.WefromDate = pricingViewModel.WEFromDate;
                            //savePricingWithScope.Price = null;
                            savePricingWithScope.ScopeTempDesc = pricingViewModel.ScopeTempDesc;
                            savePricingWithScope.IsUpdated = false;
                            savePricingWithScope.CreatedBy = pricingViewModel.CreatedBy;
                            savePricingWithScope.CreatedUtc = DateTime.UtcNow;
                            _unitWork.PricingWithScope.Add(savePricingWithScope);
                            _unitWork.SaveChanges();
                            savePricingWithScope.Id = _unitWork.PricingWithScope.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                            SendGenricMail(savePricingWithScope.Id, pricingViewModel.DepartmentId, pricingViewModel.CustomerId, pricingViewModel.PricingTypeId, 1);
                        }
                        else if (pricingViewModel.PricingTypeId == 3 || pricingViewModel.PricingTypeId == 4 || pricingViewModel.PricingTypeId == 10 || pricingViewModel.PricingTypeId == 11 || pricingViewModel.PricingTypeId == 12)
                        {
                            if (pricingViewModel.AddCountDatas.Count() == 0)
                            {
                                PricingWithScope savePricingWithScope = new PricingWithScope();
                                savePricingWithScope.CustomerId = pricingViewModel.CustomerId;
                                savePricingWithScope.DepartmentId = pricingViewModel.DepartmentId;
                                savePricingWithScope.ScopeId = pricingViewModel.ScopeId;
                                savePricingWithScope.PricingTypeId = pricingViewModel.PricingTypeId;
                                savePricingWithScope.JobStatusId = pricingViewModel.JobStatusId;
                                savePricingWithScope.From = pricingViewModel.FromRange;
                                savePricingWithScope.To = pricingViewModel.ToRange;
                                savePricingWithScope.WefromDate = pricingViewModel.WEFromDate;
                                savePricingWithScope.Price = pricingViewModel.Price;
                                savePricingWithScope.ScopeTempDesc = pricingViewModel.ScopeTempDesc;
                                savePricingWithScope.IsUpdated = false;
                                savePricingWithScope.CreatedBy = pricingViewModel.CreatedBy;
                                savePricingWithScope.CreatedUtc = DateTime.UtcNow;
                                _unitWork.PricingWithScope.Add(savePricingWithScope);
                                _unitWork.SaveChanges();
                                savePricingWithScope.Id = _unitWork.PricingWithScope.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                                SendGenricMail(savePricingWithScope.Id, pricingViewModel.DepartmentId, pricingViewModel.CustomerId, pricingViewModel.PricingTypeId, 1);
                            }
                            else
                            {
                                foreach (var item in pricingViewModel.AddCountDatas)
                                {
                                    var lastrecordofpricingcount = _unitWork.PricingWithScope.FirstOrDefault(x => x.ScopeId == item.ScopeId && x.CustomerId == pricingViewModel.CustomerId && x.WetoDate == null && x.JobStatusId == pricingViewModel.JobStatusId && x.IsDelete != true && x.From == pricingViewModel.FromRange && x.To == pricingViewModel.ToRange && x.IsApproved == true && x.IsUpdated != true); // 

                                    if (lastrecordofpricingcount != null)
                                    {
                                        lastrecordofpricingcount.WetoDate = pricingViewModel.WEFromDate.AddDays(-1);
                                        lastrecordofpricingcount.UpdatedBy = pricingViewModel.CreatedBy;
                                        lastrecordofpricingcount.UpdatedUtc = DateTime.UtcNow;
                                        lastrecordofpricingcount.IsUpdated = true;
                                        _unitWork.PricingWithScope.Update(lastrecordofpricingcount);
                                        _unitWork.SaveChanges();
                                    }
                                    PricingWithScope savePricingWithScope = new PricingWithScope();
                                    savePricingWithScope.CustomerId = pricingViewModel.CustomerId;
                                    savePricingWithScope.DepartmentId = pricingViewModel.DepartmentId;
                                    savePricingWithScope.ScopeId = item.ScopeId;
                                    savePricingWithScope.PricingTypeId = pricingViewModel.PricingTypeId;
                                    savePricingWithScope.JobStatusId = pricingViewModel.JobStatusId;
                                    savePricingWithScope.From = item.FromRange;
                                    savePricingWithScope.To = item.ToRange;
                                    savePricingWithScope.WefromDate = pricingViewModel.WEFromDate;
                                    savePricingWithScope.Price = item.Price;
                                    savePricingWithScope.ScopeTempDesc = item.ScopeTempDesc;
                                    savePricingWithScope.IsUpdated = false;
                                    savePricingWithScope.CreatedBy = pricingViewModel.CreatedBy;
                                    savePricingWithScope.CreatedUtc = DateTime.UtcNow;
                                    _unitWork.PricingWithScope.Add(savePricingWithScope);
                                    _unitWork.SaveChanges();
                                    savePricingWithScope.Id = _unitWork.PricingWithScope.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                                    SendGenricMail(savePricingWithScope.Id, pricingViewModel.DepartmentId, pricingViewModel.CustomerId, pricingViewModel.PricingTypeId, 1);
                                }
                            }
                        }
                        else if (pricingViewModel.PricingTypeId == 6)
                        {
                            PricingWithScope savePricingWithScope = new PricingWithScope();
                            savePricingWithScope.CustomerId = pricingViewModel.CustomerId;
                            savePricingWithScope.DepartmentId = pricingViewModel.DepartmentId;
                            savePricingWithScope.ScopeId = pricingViewModel.ScopeId;
                            savePricingWithScope.PricingTypeId = pricingViewModel.PricingTypeId;
                            savePricingWithScope.JobStatusId = pricingViewModel.JobStatusId;
                            savePricingWithScope.RatePerHour = pricingViewModel.RatePerHour;
                            savePricingWithScope.WefromDate = pricingViewModel.WEFromDate;
                            savePricingWithScope.Price = pricingViewModel.Price;
                            savePricingWithScope.ScopeTempDesc = pricingViewModel.ScopeTempDesc;
                            savePricingWithScope.IsUpdated = false;
                            savePricingWithScope.CreatedBy = pricingViewModel.CreatedBy;
                            savePricingWithScope.CreatedUtc = DateTime.UtcNow;
                            _unitWork.PricingWithScope.Add(savePricingWithScope);
                            _unitWork.SaveChanges();
                            savePricingWithScope.Id = _unitWork.PricingWithScope.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                            SendGenricMail(savePricingWithScope.Id, pricingViewModel.DepartmentId, pricingViewModel.CustomerId, pricingViewModel.PricingTypeId, 1);
                        }
                        return status;
                    }
                    else
                    {
                        status = "Already Exist";
                    }
                }
            }
            return status;
        }
        public void SendGenricMail(int UId, int DepartmentId, int CustomerId, int PricingTypeId, int Approve)
        {
            var url = _configuration.GetSection("JobOrder").GetValue<string>("apiurl");//ConfigurationManager.AppSettings["apiurl"];
            var request = (HttpWebRequest)WebRequest.Create(url + "api/PriceApproveMail/Mail?UId=" + UId + " &DepartmentId=" + DepartmentId + " &CustomerId=" + CustomerId + " &PricingTypeId=" + PricingTypeId + " &Approve=" + Approve);
            var postData = "UId=" + UId;
            postData += " DepartmentId" + DepartmentId;
            postData += " CustomerId" + CustomerId;
            postData += " PricingTypeId" + PricingTypeId;
            postData += " Approve=" + Approve;
            var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
        public string SetApproval(PriceApproveVM GetCollection)
        {
            string message = "";
            try
            {
              
                var upPrice = db.PricingWithScopes.FirstOrDefault(x => x.Id == GetCollection.Id);
                upPrice.IsApproved = true;
                upPrice.ApprovedBy = GetCollection.EmployeeId;
                upPrice.IsRejected = null;
                upPrice.RejectedBy = null;
                db.SaveChanges();
                message = "Price has been Approved Successfuly";
            }
            catch (Exception ex)
            {

            }
            return message;
        }
        public string SetReject(PriceApproveVM GetCollection)
        {
            string message = "";
            try
            {
             
                var upPrice = db.PricingWithScopes.FirstOrDefault(x => x.Id == GetCollection.Id);
                upPrice.IsRejected = true;
                upPrice.RejectedBy = GetCollection.EmployeeId;
                upPrice.IsApproved = null;
                upPrice.ApprovedBy = null;
                db.SaveChanges();
                message = "Price has been Rejected Successfuly";
            }
            catch (Exception ex)
            {

            }
            return message;
        }
        public int CheckApproveReject(PriceApproveVM GetCollection)
        {
            int link = 0;
          
            var result = db.PricingWithScopes.FirstOrDefault(x => x.Id == GetCollection.Id);
            if (result.IsApproved == true || result.IsRejected == true)
            {
                link = 1;
            }
            return link;
        }
        public Object GetJobStatusList()
        {
           
            var result = db.JobStatuses.Where(x => x.Id != 10 && x.Id != 11 && x.IsActive == true).Select(x => new { x.Id, x.JobStatusDescription, x.ShortDescription }).ToList();
            return result;
        }
        //Norms
        public Object GetProcessListforNorms()
        {
           
            var result = db.Processes.Where(x => x.Id == 3 || x.Id == 5 || x.Id == 7 || x.Id == 9 || x.Id == 11 && x.IsActive == true).Select(x => new { x.Id, x.Name, x.ShortName }).ToList();
            return result;
        }

        public Object GetCusDvisionforNorms()
        {
          
            var result = db.Divisions.Where(x => x.IsDeleted == false).Select(x => new { x.Id, x.DivisionName }).ToList();
            return result;
        }

        //create norms

        public string CreateCustomerNormsDetails(CustomerNormDataViewModel customernorminfo)
        {
          

            string status = "Aleardy Norms Added for the Selected Customer";
            if (customernorminfo != null)
            {
                try
                {
                    //foreach (var item in customerids)
                    //{

                    // var CheckAlreadyExist = _tablework.CustomerNormsRepository.Get(x => x.CustomerId == item && x.EmployeeId == customervsemp.EmployeeId && x.IsDeleted == false).ToList();                        
                    var CheckAlreadyExist = _unitWork.CustomersNorm.FirstOrDefault(x => x.CustomerId == customernorminfo.CustomerId && x.DepartmentId == customernorminfo.DepartmentId
                        && x.ProcessId == customernorminfo.ProcessId && x.ScopeId == customernorminfo.ScopeId && x.JobStatusId == customernorminfo.JobStatusId && x.DivisionId == customernorminfo.DivisionId
                        && x.IsDeleted != true); //  
                    var CheckCusShortName = _unitWork.Customer.FirstOrDefault(x => x.Id == customernorminfo.CustomerId && x.IsDeleted != true);

                    if (CheckAlreadyExist == null)
                    {
                        CustomersNorm savecustomerNorms = new CustomersNorm();
                        savecustomerNorms.CustomerId = customernorminfo.CustomerId;
                        savecustomerNorms.CustomerShortName = CheckCusShortName.ShortName;
                        savecustomerNorms.DepartmentId = customernorminfo.DepartmentId;
                        savecustomerNorms.ProcessId = customernorminfo.ProcessId;
                        savecustomerNorms.JobStatusId = customernorminfo.JobStatusId;
                        savecustomerNorms.ScopeId = customernorminfo.ScopeId;
                        savecustomerNorms.Norms = customernorminfo.Norms;
                        savecustomerNorms.DivisionId = customernorminfo.DivisionId;
                        savecustomerNorms.IsDeleted = false;
                        savecustomerNorms.CreatedBy = customernorminfo.CreatedBy;
                        savecustomerNorms.CreatedUtc = DateTime.UtcNow;
                        //savecustomerNorms.UpdatedBy = 1;//
                        //savecustomerNorms.UpdatedUTC = DateTime.UtcNow;//                          
                        _unitWork.CustomersNorm.Add(savecustomerNorms);
                        _unitWork.SaveChanges();
                        status = "Customer Norms Details created successfully";
                    }
                    // }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return status;
        }


        // ends
        public Object GetPriceDetailforApproval(PriceApproveVM GetCollection)
        {
            return (from PWS in _tableWork.PricingWithScopeRepository.Get()
                    join JS in _tableWork.JobStatusRepository.Get() on PWS.JobStatusId equals JS.Id
                    join PRT in _tableWork.PricingTypeRepository.Get() on PWS.PricingTypeId equals PRT.Id
                    join DEP in _tableWork.DepartmentRepository.Get() on PWS.DepartmentId equals DEP.Id
                    join CUS in _tableWork.CustomerRepository.Get() on PWS.CustomerId equals CUS.Id
                    select new
                    {
                        CustomerId = PWS.CustomerId,
                        CusShortName = CUS.ShortName,
                        Department = DEP.Description,
                        ScopeTempDesc = PWS.ScopeTempDesc,
                        PricingType = PRT.Description,
                        WEFromDate = PWS.WefromDate,
                        WEToDate = PWS.WetoDate,
                        From = PWS.From,
                        To = PWS.To,
                        FromDate = PWS.FromDate,
                        ToDate = PWS.ToDate,
                        Designation = PWS.Designation,
                        NumberofArtist = PWS.NumberofArtist,
                        Price = PWS.Price,
                        EstimationTime = PWS.EstimationTime,
                        RatePerHour = PWS.RatePerHour,
                        PricingTypeId = PWS.PricingTypeId,
                        DepartmentId = PWS.DepartmentId,
                        Id = PWS.Id,
                        ScopeId = PWS.ScopeId,
                        JobStatusDes = JS.JobStatusDescription,
                        IsDelete = PWS.IsDelete,
                        IsApproved = PWS.IsApproved,
                        ApprovedBy = PWS.ApprovedBy,
                        IsRejected = PWS.IsRejected,
                        RejectedBy = PWS.RejectedBy
                    }).Where(x => x.Id == GetCollection.Id && x.IsDelete != true).OrderByDescending(x => x.Id);
        }

        public Object ShowDetailsofPricing(int clientid)
        {
            return (from PWS in _tableWork.PricingWithScopeRepository.Get()
                    join JS in _tableWork.JobStatusRepository.Get() on PWS.JobStatusId equals JS.Id
                    join PRT in _tableWork.PricingTypeRepository.Get() on PWS.PricingTypeId equals PRT.Id
                    join DEP in _tableWork.DepartmentRepository.Get() on PWS.DepartmentId equals DEP.Id
                    join CUS in _tableWork.CustomerRepository.Get() on PWS.CustomerId equals CUS.Id
                    select new
                    {
                        CustomerId = PWS.CustomerId,
                        CusShortName = CUS.ShortName,
                        Department = DEP.Description,
                        ScopeTempDesc = PWS.ScopeTempDesc,
                        PricingType = PRT.Description,
                        WEFromDate = PWS.WefromDate,
                        WEToDate = PWS.WetoDate,
                        From = PWS.From,
                        To = PWS.To,
                        FromDate = PWS.FromDate,
                        ToDate = PWS.ToDate,
                        Designation = PWS.Designation,
                        NumberofArtist = PWS.NumberofArtist,
                        Price = PWS.Price,
                        EstimationTime = PWS.EstimationTime,
                        RatePerHour = PWS.RatePerHour,
                        PricingTypeId = PWS.PricingTypeId,
                        DepartmentId = PWS.DepartmentId,
                        Id = PWS.Id,
                        ScopeId = PWS.ScopeId,
                        JobStatusDes = JS.JobStatusDescription,
                        IsDelete = PWS.IsDelete,
                        IsApproved = PWS.IsApproved,
                        ApprovedBy = PWS.ApprovedBy,
                        IsRejected = PWS.IsRejected,
                        RejectedBy = PWS.RejectedBy
                    }).Where(x => x.CustomerId == clientid && x.IsDelete != true).OrderByDescending(x => x.Id);
        }
        // new
        public Object GetScopeViaDepartmentId(int deptId)
        {
            List<Scope> lstScope = (from A in _tableWork.ScopeRepository.Get
                                        (x => x.DepartmentId == (deptId == 0 ? x.DepartmentId : deptId))

                                    select A
                          ).Where(x => x.IsDeleted == false).OrderBy(x => x.Description).ToList();
            return lstScope;
        }
        public Object ShowDetailsofPricingChart(PricingWithScopeViewModel pricingViewModel)
        {
            var result =
             (from PWS in _tableWork.PricingWithScopeRepository.Get()
              join PRT in _tableWork.PricingTypeRepository.Get() on PWS.PricingTypeId equals PRT.Id
              join DEP in _tableWork.DepartmentRepository.Get() on PWS.DepartmentId equals DEP.Id
              join CUS in _tableWork.CustomerRepository.Get() on PWS.CustomerId equals CUS.Id
              join JS in _tableWork.JobStatusRepository.Get() on PWS.JobStatusId equals JS.Id into JoinedT
              from JS in JoinedT.DefaultIfEmpty()
              select new
              {
                  CustomerId = PWS.CustomerId,
                  CusShortName = CUS.ShortName,
                  CusName = CUS.Name,
                  Department = DEP.Description,
                  ScopeTempDesc = PWS.ScopeTempDesc,
                  JobstatusDES = JS.JobStatusDescription,
                  PricingType = PRT.Description,
                  WithEffectiveFrom = PWS.WefromDate,
                  WithEffectiveTo = PWS.WetoDate,
                  From = PWS.From,
                  To = PWS.To,
                  FromDate = PWS.FromDate,
                  ToDate = PWS.ToDate,
                  Designation = PWS.Designation,
                  NoofArtist = PWS.NumberofArtist,
                  Price = PWS.Price,
                  EstimationTime = PWS.EstimationTime,
                  RatePerhour = PWS.RatePerHour,
                  PricingTypeId = PWS.PricingTypeId,
                  DepartmentId = PWS.DepartmentId,
                  Id = PWS.Id,
                  ScopeId = PWS.ScopeId,
                  IsDelete = PWS.IsDelete,
                  IsApproved = PWS.IsApproved,
                  ApprovedBy = PWS.ApprovedBy,
                  IsRejected = PWS.IsRejected,
                  RejectedBy = PWS.RejectedBy
              }).Where(x => x.CustomerId == (pricingViewModel.CustomerId == 0 ? x.CustomerId : pricingViewModel.CustomerId)
                        && x.DepartmentId == (pricingViewModel.DepartmentId == 0 ? x.DepartmentId : pricingViewModel.DepartmentId) &&
                        x.ScopeId == (pricingViewModel.ScopeId == 0 ? x.ScopeId : pricingViewModel.ScopeId) && x.IsDelete != true && x.IsApproved == true).OrderBy(x => x.CusShortName).ToList();
            return result;
         
        }

        public PricingWithScope editpricingList(int id)
        {
            return _unitWork.PricingWithScope.Include("PricingType").Include("Department").FirstOrDefault(x => x.Id == id && x.IsDelete != true);
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _tableWork.CustomerRepository.Get(x => x.IsDeleted == false && x.CustomerJobType == "Live").OrderBy(x => x.Name).AsQueryable();
        }

        public IQueryable<Department> GetDepartments()
        {
            return _tableWork.DepartmentRepository.Get(x => x.Description == "Artwork" || x.Description == "Digitizing").AsQueryable();
        }

        public bool RemovePricing(int pricingId)
        {
            bool status = false;
            try
            {
                var deletePricing = _unitWork.PricingWithScope.FirstOrDefault(x => x.Id == pricingId);
                deletePricing.IsDelete = true;
                _unitWork.PricingWithScope.Update(deletePricing);
                long dbstatus = _unitWork.Save();
                status = dbstatus > 0;
            }
            catch (Exception e)
            {
                throw;
            }
            return status;
        }

        public IEnumerable<PricingType> GetPricingTypesByDepartmentId(int deptId)
        {
            var pricingTypes = _tableWork.PricingTypeRepository.Get(x => x.IsDeleted == false).ToList();

            if (deptId == 1)
            {
                return pricingTypes.Where(x => x.Applicable == 1 || x.Applicable == 4).ToList();
            }
            else
            {
                return pricingTypes.Where(x => x.Applicable == 3 || x.Applicable == 2).ToList();
            }
        }

        public Object GetScopeByDepartmentId(int deptId, int customerId)
        {
            List<Scope> lstScope = (from A in _tableWork.ScopeRepository.Get(x => x.DepartmentId == deptId && x.IsDeleted == false)
                                    join B in _tableWork.CustomerVsScopeRepository.Get(x => x.CustomerId == customerId && x.DeptId == deptId && x.IsDeleted == false) on A.Id equals B.ScopeId
                                    select A
                          ).ToList();
            return lstScope;
        }

        public decimal? GetPriceBy(int departmentId, int customerId, int scopeId)
        {
            var result = _unitWork.PricingWithScope.FirstOrDefault(x => x.DepartmentId == departmentId && x.CustomerId == customerId && x.ScopeId == scopeId);
            if (result != null) return result.Price;
            return null;
        }
    }
}

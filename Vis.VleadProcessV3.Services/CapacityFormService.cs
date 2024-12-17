using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Security.Policy;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class CapacityFormService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext db;
        private readonly TableWork tb;
        private readonly TableWork tow;//= new TableWork();

        public CapacityFormService(ApplicationDbContext dbContext, TableWork tableWork)
        {
            db = dbContext;
            tb = tableWork;
            tow = tableWork;
        }

        //[Route("GetCapacityForm")]==================================read 
        public Object GetCapacityForm()
        {
            //ApplicationDbContext db = new ApplicationDbContext(_configuration);
            var result = (from cf in db.CapacityForm
                          join emp in db.Employees on cf.EmpId equals emp.EmployeeId
                          join div in db.Divisions on cf.DivisionId equals div.Id
                          join cust in db.Customers on cf.CustomerId equals cust.Id
                          where (emp.IsDeleted == false && cf.IsActive == true)
                          select new { cf.Id, emp.EmployeeCode, emp.EmployeeId, emp.EmployeeName, div.DivisionName, DivisionId = div.Id, cust.ShortName, CustomerId = cust.Id, cf.Fresh, cf.Revision, cf.QC, cf.Remarks }).OrderBy(x => x.Id).ToList();
            return result;
        }


        //[Route("SetCapacityForm")]==================================create 
        //public String SetCapacityForm(CapFormPara CFPara)
        //{
        //    string message = "Inserted Sucessfully";
        //    int counter = 0;
        //    try
        //    {
        //        //ApplicationDbContext db = new ApplicationDbContext(_configuration);
        //        var Employee = CFPara.GivenEmployee.ToList();
        //        var Division = CFPara.GivenDivision.ToList();
        //        var Customer = CFPara.GivenCustomer.ToList();
        //        int? Fresh = CFPara.Fresh;
        //        int? Revision = CFPara.Revision;
        //        int? QC = CFPara.QC;
        //        string? Remarks = CFPara.Remarks;
        //        int EmployeeId = CFPara.CreatedBy;
        //        DateTime EffectiveFrom = CFPara.EffectiveFrom;
        //        foreach (var itemE in Employee)
        //        {
        //            foreach (var itemD in Division)
        //            {
        //                foreach (var itemC in  Customer)
        //                {
        //                    CapacityForm cf = new CapacityForm();
        //                    var checkCF = db.CapacityForm.Where(x => x.EmpId == itemE.EmpId && x.DivisionId == itemD.DivisionId && x.CustomerId == itemC.CustomerId && x.IsActive == true).ToList();
        //                    if (checkCF.Count() == 0)
        //                    {
        //                        cf.EmpId = itemE.EmpId;
        //                        cf.DivisionId = itemD.DivisionId;
        //                        cf.CustomerId = itemC.CustomerId;
        //                        cf.Fresh = Fresh;
        //                        cf.Revision = Revision;
        //                        cf.QC = QC;
        //                        cf.Remarks = Remarks;
        //                        cf.CreatedBy = EmployeeId;
        //                        cf.CreatedUtc = DateTime.UtcNow;
        //                        cf.IsActive = true;
        //                        cf.EffectiveFrom = EffectiveFrom;
        //                        db.CapacityForm.Add(cf);
        //                        db.SaveChanges();
        //                    }
        //                    else
        //                    {
        //                        counter++;
        //                        continue;
        //                    }
        //                }

        //            }
        //        }
        //        if (counter > 0)
        //        {
        //            message = "Already Exists";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return message;
        //}



        public String SetCapacityForm(CapFormPara CFPara)
        {
            string message = "Inserted Sucessfully";
            int counter = 0;
            try
            {
                //ApplicationDbContext db = new ApplicationDbContext(_configuration);
                var Employee = CFPara.GivenEmployee.ToList();
                var Division = CFPara.GivenDivision.ToList();
                var Customer = CFPara.GivenCustomer.ToList();
                int? Fresh = CFPara.Fresh;
                int? Revision = CFPara.Revision;
                int? QC = CFPara.QC;
                string? Remarks = CFPara.Remarks;
                int EmployeeId = CFPara.CreatedBy;
                DateTime EffectiveFrom = CFPara.EffectiveFrom;
                foreach (var itemE in Employee)
                {
                    foreach (var itemD in Division)
                    {
                        foreach (var itemC in Customer)
                        {

                            var checkCFexists = tb.CapacityFormRepository.GetSingle(x => x.EmpId == itemE.EmpId && x.DivisionId == itemD.DivisionId && x.CustomerId == itemC.CustomerId && x.IsActive == false && x.EffectiveTo != null);
                            if (checkCFexists != null)
                            {
                                counter++;
                                continue;
                            }

                            if (counter == 0)
                            {
                                var checkCF = tb.CapacityFormRepository.GetSingle(x => x.EmpId == itemE.EmpId && x.DivisionId == itemD.DivisionId && x.CustomerId == itemC.CustomerId && x.IsActive == true && x.EffectiveTo == null);
                                if (checkCF != null)
                                {
                                    checkCF.EffectiveTo = EffectiveFrom.AddDays(-1);
                                    checkCF.IsActive = false;
                                    checkCF.UpdatedBy = CFPara.UpdatedBy;
                                    checkCF.UpdatedUtc = DateTime.Now;
                                    tow.CapacityFormRepository.Update(checkCF);
                                }

                                var cf = new CapacityForm
                                {
                                    EmpId = itemE.EmpId,
                                    DivisionId = itemD.DivisionId,
                                    CustomerId = itemC.CustomerId,
                                    Fresh = Fresh,
                                    Revision = Revision,
                                    QC = QC,
                                    Remarks = Remarks,
                                    CreatedBy = EmployeeId,
                                    CreatedUtc = DateTime.UtcNow,
                                    IsActive = true,
                                    EffectiveFrom = EffectiveFrom,

                                };
                                tb.CapacityFormRepository.Insert(cf);
                            }

                        }

                    }
                }
                tb.SaveChanges();
                if (counter > 0)
                {
                    message = "Already Exists";
                }
            }
            catch (Exception)
            {
                message ="Error occured while inserting";
                throw;
            }
            return message;
        }


        //[Route("UpdateCapacityForm")]==================================update 
        public bool UpdateCapacityForm(CapacityForm CFsetPara)
        {
            bool status = false;
            try
            {

                var edit = tb.CapacityFormRepository.GetSingle(x => x.EmpId == CFsetPara.EmpId && x.DivisionId == CFsetPara.DivisionId && x.CustomerId == CFsetPara.CustomerId && x.IsActive == true);
                if (edit != null)
                {
                    var UpdateCf = tb.CapacityFormRepository.GetSingle(x => x.Id == CFsetPara.Id);
                    UpdateCf.EmpId = CFsetPara.EmpId;
                    UpdateCf.DivisionId = CFsetPara.DivisionId;
                    UpdateCf.CustomerId = CFsetPara.CustomerId;
                    UpdateCf.Fresh = CFsetPara.Fresh;
                    UpdateCf.Revision = CFsetPara.Revision;
                    UpdateCf.QC = CFsetPara.QC;
                    UpdateCf.Remarks = CFsetPara.Remarks;
                    UpdateCf.IsActive = true;
                    UpdateCf.UpdatedBy = CFsetPara.UpdatedBy;
                    UpdateCf.UpdatedUtc = DateTime.UtcNow;
                    UpdateCf.EffectiveFrom = CFsetPara.EffectiveFrom;
                    tb.CapacityFormRepository.Update(UpdateCf);
                    long dbstatus = tb.SaveChanges();
                    if (dbstatus != 0)
                    {
                        status = true;
                    }
                }
                else
                {
                    status = false;
                }

            }
            catch (Exception)
            {
                status = false;
                throw;
            }
            return status;
        }


        //[Route("DeleteCapacityForm")]==================================delete 
        public String DeleteCapacityForm(CapFormDelPara CFdelId)
        {
            string message = "";
            // ApplicationDbContext db = new ApplicationDbContext(_configuration);
            var result = db.CapacityForm.FirstOrDefault(x => x.Id == CFdelId.CapFormDelete && x.IsActive==true);


            if (result == null)
            {
                message = "Capacity Form does not exist";
            }
            else
            {
                int val = db.Database.ExecuteSql($"UPDATE  [dbo].[CapacityForm] SET IsActive=0 WHERE Id={CFdelId.CapFormDelete}");
                db.SaveChanges();
                message="Capacity Form deleted successfully";
            }

            return message;
        }
    }
}

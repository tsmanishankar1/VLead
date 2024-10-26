using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class CustomerVsChecklistService
    {
      
        private readonly TableWork _tablework;
        private readonly TableWork tow;
        private readonly ApplicationDbContext db;
        public CustomerVsChecklistService(TableWork tableWork, ApplicationDbContext db)
        {
            tow = tableWork;
            _tablework = tableWork;
            this.db = db;
        }
        //[Route("GetDropDownList")]------------------------------------------------------------Get Cusromer and Department Dropdown
        public Object GetAllDropDownList()
        {
            var Dept = _tablework.DepartmentRepository.Get(x => x.IsDeleted == false).AsQueryable();
            var customer = _tablework.CustomerRepository.Get(x => x.IsDeleted == false).AsQueryable();

            var DetailList = new
            {
                DepartmentList = Dept,
                CustomerList = customer,

            };
            return DetailList;
        }

        //[Route("CreateChecklist")]------------------------------------------------------------create the new checklist
        public string CreateChecklist(CvsCPara para)
        {
            string message = "Inserted Sucessfully";
            try
            {
                var checkCvC = db.Checklists.Where(x => x.CustomerId == para.CustomerId && x.DeptId == para.DepartmentId && x.Description == para.Description && x.IsDeleted == false).ToList();
                if (checkCvC.Count() == 0)
                {
                    Checklist cvc = new Checklist();
                    cvc.CustomerId = para.CustomerId;
                    cvc.DeptId = para.DepartmentId;
                    cvc.Description = para.Description;
                    cvc.CreatedBy = para.EmployeeId;
                    cvc.CreatedUtc = DateTime.UtcNow;
                    cvc.IsDeleted = false;
                    db.Checklists.Add(cvc);
                    db.SaveChanges();
                }
                else
                {
                    message = "Already Exists";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return message;
        }

        //[Route("CustomerVsChecklist")]--------------------------------------------------------list all newly created checklist
        public Object CustomerVsChecklist()
        {
          
            var result = (from cvc in db.Checklists
                          join cus in db.Customers on cvc.CustomerId equals cus.Id
                          join dept in db.Departments on cvc.DeptId equals dept.Id
                          where (cvc.IsDeleted == false)
                          select new
                          {
                              Id = cvc.Id,
                              Description = cvc.Description,
                              ShortName = cus.ShortName,
                              Department = dept.Description,
                          }).OrderByDescending(x => x.Id).ToList();
            return result;
        }

        //[Route("Delete-Check")]-----------------------------------------------------------Delete checklist
        public bool RemoveChecklist(int Id)
        {
            bool status = false;
            try
            {
               
                    var existingScope = tow.ChecklistRepository.GetSingle(x => x.Id == Id);
                    existingScope.IsDeleted = true;
                    tow.ChecklistRepository.Update(existingScope);
                    long dbstatus = tow.SaveChanges();
                    status = dbstatus > 0;
               
            }
            catch (Exception)
            {
                throw;
            }
            return status;
        }

        //[Route("GetChecklistDetails")]------------------------------------------------------view checklist
        public Checklist GetChecklistDetails(int Id)
        {
           
                return tow.ChecklistRepository.GetAllVal(x => x.Dept, x => x.Customer).Where(x => x.Id == Id).FirstOrDefault();
         
        }

        //[Route("UpdateChecklist")]----------------------------------------------------------Update Checklist
        public bool UpdateCheckList(Checklist checklist)
        {
            bool status = false;
            try
            {
          
                    var checkdetails = tow.ChecklistRepository.GetSingle(x => x.CustomerId == checklist.CustomerId && x.DeptId == checklist.DeptId && x.Description == checklist.Description && x.IsDeleted == false);
                    if (checkdetails != null)
                    {
                        status = false;
                    }
                    else
                    {
                        var updateCheckDetails = tow.ChecklistRepository.GetSingle(x => x.Id == checklist.Id);
                        updateCheckDetails.DeptId = checklist.DeptId;
                        updateCheckDetails.CustomerId = checklist.CustomerId;
                        updateCheckDetails.Description = checklist.Description;
                        updateCheckDetails.UpdatedBy = checklist.UpdatedBy;
                        updateCheckDetails.UpdatedUtc = DateTime.UtcNow;
                        updateCheckDetails.IsDeleted = false;
                        tow.ChecklistRepository.Update(updateCheckDetails);
                        long dbstatus = tow.SaveChanges();
                        if (dbstatus != 0)
                        {
                            status = true;
                        }
                    }
             
            }
            catch (Exception)
            {
                status = false;
                throw;
            }
            return status;
        }
    }
}

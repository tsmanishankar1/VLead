using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class CustomerVsDivisionService
    {
       
        private readonly ApplicationDbContext db;
        private readonly UnitWork _unitWork;
        public CustomerVsDivisionService( ApplicationDbContext db, UnitWork unitWork)
        {
           
            this.db = db;
            _unitWork = unitWork;
        }
        public string SetCustomerVsDivision(CvsDPara para)
        {
            string message = "Inserted Sucessfully";
            try
            {
                
                var checkCvD = db.CustomerVsDivisions.Where(x => x.CustomerId == para.CustomerId && x.DivisionId == para.DivisionId && x.DeptId == para.DepartmentId && x.IsDeleted == false).ToList();
                if (checkCvD.Count() == 0)
                {
                    CustomerVsDivision cvd = new CustomerVsDivision();
                    cvd.CustomerId = para.CustomerId;
                    cvd.DivisionId = para.DivisionId;
                    cvd.DeptId = para.DepartmentId;
                    cvd.CreatedBy = para.EmployeeId;
                    cvd.CreatedDate = DateTime.UtcNow;
                    cvd.IsDeleted = false;
                    db.CustomerVsDivisions.Add(cvd);
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
        public Object GetCustomerVsDivision()
        {
        
            var result = (from cvd in db.CustomerVsDivisions
                          join cus in db.Customers on cvd.CustomerId equals cus.Id
                          join div in db.Divisions on cvd.DivisionId equals div.Id
                          join dept in db.Departments on cvd.DeptId equals dept.Id
                          where (cvd.IsDeleted == false)
                          select new
                          {
                              Id = cvd.Id,
                              CustomerName = cus.Name,
                              CustomerShortName = cus.ShortName,
                              Department = dept.Description,
                              Division = div.DivisionName,
                          }).OrderByDescending(x => x.Id).ToList();
            return result;
        }
        public bool RemoveCvsD(int Id)
        {
            bool status = false;
            try
            {
                var deletePricing = _unitWork.CustomerVsDivision.FirstOrDefault(x => x.Id == Id);
                deletePricing.IsDeleted = true;
                _unitWork.CustomerVsDivision.Update(deletePricing);
                long dbstatus = _unitWork.Save();
                status = dbstatus > 0;
            }
            catch (Exception e)
            {
                throw;
            }
            return status;
        }
        public Object GetCustomerList()
        {
         
            var result = db.Customers.Where(x => x.IsDeleted == false).ToList();
            return result;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class EmployeeVsDivisionService
    {
       
        private readonly ApplicationDbContext db;
        public EmployeeVsDivisionService(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }
        public IEnumerable<Employee> GetEmployee()
        {
          
            var result = db.Employees.Where(x => x.IsDeleted == false && (x.DepartmentId == 1 || x.DepartmentId == 2)).ToList();
            return result;
        }
        public IEnumerable<Division> GetDivision()
        {
          
            var result = db.Divisions.Where(x => x.IsDeleted == false).ToList();
            return result;
        }
        public String SetEmployeeVsDivision(EvsDPara1 EDPara)
        {
            string message = "Inserted Sucessfully";
            int Gint = 0;
            try
            {
                
                var Employee = EDPara.SelectedEmployee.ToList();
                var Division = EDPara.SelectedDivision.ToList();
                int EmployeeId = EDPara.CreatedBy;
                foreach (var itemE in Employee)
                {
                    foreach (var itemD in Division)
                    {
                        EmployeeVsDivision evd = new EmployeeVsDivision();
                        var checkEvD = db.EmployeeVsDivisions.Where(x => x.EmployeeId == itemE.EmployeeId && x.DivisionId == itemD.Id && x.DeptId == itemE.DepartmentId && x.IsDeleted == false).ToList();
                        if (checkEvD.Count() == 0)
                        {
                            evd.EmployeeId = itemE.EmployeeId;
                            evd.DivisionId = itemD.Id;
                            evd.DeptId = itemE.DepartmentId;
                            evd.CreatedBy = EmployeeId;
                            evd.CreatedUtc = DateTime.UtcNow;
                            evd.IsDeleted = false;
                            db.EmployeeVsDivisions.Add(evd);
                            db.SaveChanges();
                        }
                        else
                        {
                            Gint++;
                            continue;
                        }
                    }
                }
                if (Gint > 0)
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
        public Object GetEmployeeVsDivision()
        {
            
            var result = (from evd in db.EmployeeVsDivisions
                          join emp in db.Employees on evd.EmployeeId equals emp.EmployeeId
                          join div in db.Divisions on evd.DivisionId equals div.Id
                          join dept in db.Departments on evd.DeptId equals dept.Id
                          where (emp.IsDeleted == false && evd.IsDeleted == false)
                          select new { evd.Id, emp.EmployeeCode, emp.EmployeeName, div.DivisionName }).OrderByDescending(x => x.Id).ToList();
            return result;
        }
        public String DeleteEmployeeVsDivision(EvsDDDPara delId)
        {
            string message = "";
          
            var result = db.EmployeeVsDivisions.FirstOrDefault(x => x.Id == delId.Delete && x.IsDeleted==false);
            //result.IsDeleted = true;
            // db.EmployeeVsDivisions.Update(result);

            if(result == null)
            {
                message = "Division does not exist";
            }
            else
            {
               int val= db.Database.ExecuteSql($"UPDATE  [dbo].[EmployeeVsDivision] SET IsDeleted=1 WHERE Id={delId.Delete}");
                db.SaveChanges();
                message="Division deleted successfully";
            }
          
          // db.EmployeeVsDivisions.Remove(result);
            
            return message;
        }
    }
}

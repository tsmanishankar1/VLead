using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.ViewModels;
using static System.Formats.Asn1.AsnWriter;

namespace Vis.VleadProcessV3.Services
{
    public class WorkFromHomeNormService
    {
        private readonly ApplicationDbContext _context;
        public WorkFromHomeNormService(ApplicationDbContext context)
        {
            _context = context;
        }

/*        public Norm GetEffectiveNorm(Employee employee)
        {
            var norms = _context.Norms
                .Where(n => n.WorkMode == employee.WorkingStatus && n.EffectiveDate <= employee.CategoryEffectiveDate)
                .OrderByDescending(n => n.EffectiveDate)
                .FirstOrDefault();
            return norms;
        }
*/
        public Norm AddNorm(CreateNormRequest request)
        {
            var existingNorm = _context.Norms.FirstOrDefault(n => n.EmployeeId == request.EmployeeId && n.IsDeleted == false);
            if (existingNorm != null)
            {
                throw new CustomException("Specified employee's norm is already exist.");
            }
            var norm = new Norm
            {
                DivisionId = request.DivisionId,
                ClientId = request.ClientId,
                ScopeId = request.ScopeId,
                EmployeeId = request.EmployeeId,
                ProductivityNorm = request.ProductivityNorm,
                EffectiveFromDate = DateTime.UtcNow,
                WorkMode = request.WorkMode,
                IsDeleted = false,
                CreatedBy = request.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            _context.Norms.Add(norm);
            _context.SaveChanges();

            return norm;
        }

        public EmployeeCategoryChange UpdateEmployeeCategory(EmployeeCategoryChangeRequest request)
        {
            var workMode = _context.Norms.FirstOrDefault(n => n.EmployeeId == request.EmployeeId && n.IsDeleted == false).WorkMode;
            var change = new EmployeeCategoryChange
            {
                EmployeeId = request.EmployeeId,
                PreviousWorkMode = workMode,
                NewWorkMode = request.NewWorkMode,
                EffectiveFromDate = DateTime.UtcNow,
                IsDeleted = false,
                CreatedBy = request.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };
            _context.EmployeeCategoryChanges.Add(change);

            var existingEmployeeCategoryChange = _context.EmployeeCategoryChanges.FirstOrDefault(e => e.EmployeeId == request.EmployeeId && e.IsDeleted == false);
            if(existingEmployeeCategoryChange != null)
            {
                existingEmployeeCategoryChange.EffectiveToDate = change.EffectiveFromDate.AddDays(-1);
                existingEmployeeCategoryChange.UpdatedBy = request.CreatedBy;
                existingEmployeeCategoryChange.UpdatedUtc = DateTime.UtcNow;
                existingEmployeeCategoryChange.IsDeleted = true;
                _context.EmployeeCategoryChanges.Update(existingEmployeeCategoryChange);
            }

            var existingNorm = _context.Norms.FirstOrDefault(e => e.EmployeeId == request.EmployeeId && e.IsDeleted == false);
            if(existingNorm != null)
            {
                Norm norm = new Norm();
                norm.EmployeeId = request.EmployeeId;
                norm.ProductivityNorm = request.NewNorm;
                norm.WorkMode = request.NewWorkMode;
                norm.CreatedBy = request.CreatedBy;
                norm.CreatedUtc = DateTime.UtcNow;
                norm.EffectiveFromDate = DateTime.UtcNow;
                norm.IsDeleted = false;
                norm.DivisionId = request.DivisionId;
                norm.ClientId = request.ClientId;
                norm.ScopeId = request.ScopeId;
                _context.Norms.Add(norm);

                existingNorm.EffectiveToDate = norm.EffectiveFromDate.AddDays(-1);
                existingNorm.UpdatedBy = request.CreatedBy;
                existingNorm.UpdatedUtc = DateTime.UtcNow;
                existingNorm.IsDeleted = true;
                _context.Norms.Update(existingNorm);
            }

            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == request.EmployeeId && e.IsDeleted == false);
            if (employee != null)
            {
                employee.WorkingStatus = request.NewWorkMode;
                employee.CategoryEffectiveDate = change.EffectiveFromDate;
                _context.Employees.Update(employee);
            }

            _context.SaveChanges();

            return change;
        }

        public List<NormResponse> GetWorkFromHomeNorm()
        {
            var norms = _context.Norms.Where(n => n.WorkMode == "WFH" && n.IsDeleted == false).ToList();
            if(norms == null)
            {
                throw new MessageNotFoundException("Work from Home norm not found. Please update the norm.");
            }

            var norm = (from n in norms
                        join d in _context.Divisions on n.DivisionId equals d.Id
                        join c in _context.Customers on n.ClientId equals c.Id
                        join s in _context.Scopes on n.ScopeId equals s.Id
                        join e in _context.Employees on n.EmployeeId equals e.EmployeeId
                        join cr in _context.Employees on n.CreatedBy equals cr.EmployeeId
                        select new NormResponse
                        {
                            DivisionId = n.DivisionId,
                            Division = d.DivisionName,
                            ClientId = n.ClientId,
                            Client = c.Name,
                            ScopeId = n.ScopeId,
                            Scope = s.Description,
                            EmployeeId = n.EmployeeId,
                            Employee = e.EmployeeName,
                            ProductivityNorm = n.ProductivityNorm,
                            EffectiveFromDate = n.EffectiveFromDate,
                            WorkMode = n.WorkMode,
                            CreatedBy = n.CreatedBy,
                            CreatedByName = cr.EmployeeName,
                            CreatedUtc = n.CreatedUtc,
                        }).ToList();

            return norm;
        }
    }
}

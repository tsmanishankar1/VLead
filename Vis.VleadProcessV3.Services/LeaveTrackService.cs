using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.Services
{
    public class LeaveTrackService
    {
        public readonly ApplicationDbContext _context;

        public LeaveTrackService(ApplicationDbContext context)
        {
            _context = context;
        }

        public LeaveRequest LeaveRequest(SubmitLeave submitLeave)
        {
            var leaveRequest = new LeaveRequest
            {
                EmployeeId = submitLeave.EmployeeId,
                LeaveCategory = submitLeave.LeaveCategory,
                StartDate = submitLeave.StartDate,
                EndDate = submitLeave.EndDate,
                Comments = submitLeave.Comments
            };

            _context.LeaveRequests.Add(leaveRequest);
            _context.SaveChanges();

            return leaveRequest;
        }

        public LeaveRequest UpdateLeave(UpdateLeave updateLeave)
        {
            var employeeLeave = _context.LeaveRequests.Where(l => l.Id == updateLeave.Id).FirstOrDefault();
            if (employeeLeave == null)
            {
                throw new MessageNotFoundException("Leave not found.");
            }

            employeeLeave.LeaveCategory = updateLeave.LeaveCategory;
            employeeLeave.Comments = updateLeave.Comments;
            employeeLeave.StartDate = updateLeave.StartDate;
            employeeLeave.EndDate = updateLeave.EndDate;
            employeeLeave.UpdatedBy = updateLeave.UpdatedBy;
            employeeLeave.UpdatedUtc = DateTime.UtcNow;

            _context.LeaveRequests.Update(employeeLeave);
            _context.SaveChanges();

            return employeeLeave;
        }

        public List<GetLeaveRequests> GetEmployeeLeaves(GetLeave getLeave)
        {
            var department = _context.Employees.FirstOrDefault(d => d.EmployeeId == getLeave.EmployeeId && d.IsDeleted == false);
            var role = _context.EmployeeVsRoles.FirstOrDefault(r => r.EmployeeId == getLeave.EmployeeId && r.IsDeleted == false);
            if ((department != null && department.DepartmentId == 7) || (role != null && role.RoleId == 7))
            {
                var leaveRequests = _context.LeaveRequests
                    .Where(l => l.IsDelete == false)
                    .Join(_context.Employees,
                          leave => leave.EmployeeId,
                          employee => employee.EmployeeId,
                          (leave, employee) => new GetLeaveRequests
                          {
                              Id = leave.Id,
                              EmployeeName = employee.EmployeeName,
                              LeaveCategoryId = leave.LeaveCategory,
                              LeaveCategory = leave.LeaveCategoryName,
                              StartDate = leave.StartDate,
                              EndDate = leave.EndDate,
                              Comments = leave.Comments
                          })
                    .ToList();

                if (leaveRequests.Count == 0)
                {
                    throw new MessageNotFoundException("Leave not found.");
                }
                return leaveRequests;
            }
            else
            {
                var leaveRequests = _context.LeaveRequests
                    .Where(l => l.EmployeeId == getLeave.EmployeeId && l.IsDelete == false)
                    .Join(_context.Employees,
                          leave => leave.EmployeeId,
                          employee => employee.EmployeeId,
                          (leave, employee) => new GetLeaveRequests
                          {
                              Id = leave.Id,
                              EmployeeName = employee.EmployeeName,
                              LeaveCategoryId = leave.LeaveCategory,
                              LeaveCategory = leave.LeaveCategoryName,
                              StartDate = leave.StartDate,
                              EndDate = leave.EndDate,
                              Comments = leave.Comments
                          })
                    .ToList();

                if (leaveRequests.Count == 0)
                {
                    throw new MessageNotFoundException("Leave not found.");
                }
                return leaveRequests;
            }
        }

        public LeaveRequest DeleteLeave(int id)
        {
            var deleteLeave = _context.LeaveRequests
                .FirstOrDefault(l => l.Id == id);

            if (deleteLeave == null)
            {
                throw new MessageNotFoundException("Leave not found.");
            }

            deleteLeave.IsDelete = true;

            _context.SaveChanges();
            return deleteLeave;
        }
    }
}

using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class TrainingService
    {
        private readonly TableWork _tableWork;
        private readonly ProcedureWork _procedureWork;
        private readonly ApplicationDbContext _context;
        private readonly AllocationService allocationService;
        public TrainingService(TableWork tableWork, ProcedureWork procedureWork, ApplicationDbContext context, AllocationService allocationService)
        {
            _tableWork = tableWork;
            _procedureWork = procedureWork;
            _context = context;
            this.allocationService = allocationService;
        }

        public object GetPendingJobsAndEmployeesForTraining(int employeeId, int processId, int isPendingJob)
        {
            var AllocationJobs = new List<ViewJobAllocation>();
            var allocation = new List<JobAllocation_Result>();
            var employees = new List<GetEmployee>();
            try
            {
                if (processId == 14)
                {
                    var employeeProcesses = _tableWork.EmployeeProcessRepository.GetAllVal(x => x.Employee).Where(x => (x.ProcessId == (processId + 1) && x.IsDeleted == false) && (x.Employee.IsDeleted == false && x.Employee.DateOfResignation == null && x.Employee.DepartmentId == 12)).OrderBy(x => x.Employee.EmployeeName).ToList(); //&& x.Employee.DepartmentId == DepartmentId
                    foreach (var item in employeeProcesses)
                    {
                        string employeeNameWithCode = item.Employee.EmployeeCode + " - " + item.Employee.EmployeeName;
                        var shiftDetails = _tableWork.ShiftVsEmployeeRepository.GetAllVal(x => x.Shift).FirstOrDefault(x => x.EmployeeId == item.Employee.EmployeeId && x.IsDeleted == false);
                        var shift = "";
                        if (shiftDetails != null)
                        {
                            shift = shiftDetails.Shift.Description;
                        }
                        employees.Add(new GetEmployee
                        {
                            EmployeeId = item.Employee.EmployeeId,
                            EmployeeName = item.Employee.EmployeeName,
                            EmployeeCode = item.Employee.EmployeeCode,
                            EmployeenameWithCode = employeeNameWithCode,
                            ShiftName = shift
                        });
                    }
                    var sqlPara = new SqlParameter[]
                    {
                        new SqlParameter("@EmployeeId",employeeId),
                        new SqlParameter("@IsPendingJob",isPendingJob),
                        new SqlParameter("@ProcessId",processId)
                    };
                    allocation = _procedureWork.ExecStoredProcedure<JobAllocation_Result>("JobAllocationTraining @EmployeeId,@IsPendingJob, @ProcessId", sqlPara).ToList();
                }
                else
                {
                    var result1 = new
                    {
                        success = false,
                        message = "No jobs and employees found for this process",
                    };
                    return result1;
                }
            }
            catch (Exception ex)
            {
                Log addlog = new Log();
                addlog.Module = "Get Pending jobs and emps";
                addlog.Description = "Error Msg in training alloc trays :  " + ex.Message.ToString() + "-" + "Inner Exc: " + ex.InnerException + ", Stack Trace: " + ex.StackTrace + ", Source:" + ex.Source;
                addlog.Type = "Error Occured in training alloc trays";
                addlog.CreatedUtc = DateTime.UtcNow;
                _context.Logs.Add(addlog);
                _context.SaveChanges();
            }
            var result = new
            {
                AllocationJobs = allocation,
                Employees = employees
            };
            return result;
        }

    }
}

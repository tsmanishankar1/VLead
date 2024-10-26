using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;

namespace Vis.VleadProcessV3.Services.WindowsService
{
    public class AsyncWindowsService
    {
        private readonly ApplicationDbContext _context;
        public AsyncWindowsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ViewJobForcompletedJob>> GetCompletedJobsforService(int EmpId)
        {
            var CompletedJobsList = new List<ViewJobForcompletedJob>();
            var resultCompletedJobsList = new List<ViewJobForcompletedJob>();

            var salesCheck = await _context.CustomerVsSalesEmployees.Where(x => x.EmployeeId == EmpId && x.IsActive == true).FirstOrDefaultAsync();
            if (salesCheck != null)
            {
                CompletedJobsList = await _context.ViewJobForcompletedJobs.Where(x => x.ProcessId == 1 && 
                (x.StatusId == 1 || x.StatusId == 18 || x.StatusId == 26) 
                && x.PreviousProcessId != null && x.JobClosedUtc != null &&
                x.DateofUpload == null && x.IsDeleted == false && x.IsActive == true &&
                x.Scopetype == "Trial" && x.SalesEmpId == EmpId).
                OrderByDescending(x => x.TranId).ToListAsync();

                return resultCompletedJobsList = CompletedJobsList;
            }
            else
            {
                CompletedJobsList =await _context.ViewJobForcompletedJobs
                    .Where(x => x.ProcessId == 1 && 
                    (x.StatusId == 1 || x.StatusId == 18 || x.StatusId == 26) &&
                    x.PreviousProcessId != null && x.JobClosedUtc != null && 
                    x.DateofUpload == null && x.IsDeleted == false &&
                    x.IsActive == true && x.Scopetype != "Trial")
                    .OrderByDescending(x => x.TranId).ToListAsync();

                return resultCompletedJobsList = CompletedJobsList;
            }
        }
    }
}

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class BenchOptionService
    {
       
        private readonly TableWork _tableWork;
        private readonly UnitWork _unitofwork;
        private readonly ProcedureWork _procedureWork;

        private readonly ApplicationDbContext _db;
        public BenchOptionService(TableWork tableWork, UnitWork unitWork,
            ProcedureWork procedureWork, ApplicationDbContext db)
        {
           

            _tableWork = tableWork;
            _unitofwork = unitWork;
            _procedureWork = procedureWork;
            _db = db;
        }
        public IEnumerable<BenchStatus> GetStatus(int EmployeeId)
        {
            return _tableWork.BenchStatusRepository.Get(x => x.IsDeleted == false);
        }
        public Object Getbutton(int EmployeeId)
        {
            bool start = false;
            var message = "";
            var listEA = _unitofwork.EmployeeAssign.Include("JobOrder").Where(x => x.JobOrder.IsDeleted == false && x.IsActive == true && x.EmployeeId == EmployeeId).ToList();
            var empAssignRecords = listEA.Where(x => (x.Status == "Assigned" || x.Status == "In Process" || x.Status == "Bulk Order Complete" || x.Status == "Attachment Missing")).ToList();
            if (empAssignRecords.Count() != 0)
            {
                var FirstempAssignRecords = listEA.OrderByDescending(x => x.Id).FirstOrDefault();
                if (FirstempAssignRecords.IsBench == true && FirstempAssignRecords.JobOrder.JobStatusId == 11)
                {
                    start = true;
                    if (FirstempAssignRecords.Status == "Break")
                    {
                        start = false;
                    }
                    else if (FirstempAssignRecords.Status == "Worked")
                    {
                        start = false;
                        message = "Null";
                    }
                }
                else if (FirstempAssignRecords.Status == "Assigned")
                {
                    var verifybenchjobornot = listEA.Where(x => x.IsBench == true && x.JobOrder.JobStatusId == 11).OrderByDescending(x => x.Id).FirstOrDefault();
                    if (verifybenchjobornot != null)
                    {
                        if (verifybenchjobornot.Status == "Working")
                        {
                            start = true;
                        }
                        else if (verifybenchjobornot.Status == "Worked")
                        {
                            start = false;
                            message = "Null";
                        }
                    }
                    else
                    {
                        message = "Null";
                    }
                }
                else
                {
                    message = "Null";
                }
            }
            //start = true;
            //var empass = listEA.Where(x => x.IsBench == true).OrderByDescending(c => c.Id).FirstOrDefault();
            //if (empass != null)
            //{
            //    if (empass.Status == "Break")
            //    {
            //        start = false;
            //    }
            //}


            var result = new
            {
                Start = start,
                Message = message
            };
            return result;
        }
        public IEnumerable<ListofBenchWork_Result> BenchOptionWorkWithRange(BenchOptionViewModel benchrange)
        {
            var result = new List<ListofBenchWork_Result>();
            string temp = string.Join(",", benchrange.EmployeeId);
            if (benchrange.ReportType == "Ideal Time")
            {
                benchrange.ClientId = 0;
                var sqlPara = new SqlParameter[]
                    {
                        new SqlParameter("@EmployeeId",temp),
                        new SqlParameter("@ClientId", benchrange.ClientId),
                        new SqlParameter("@DepartmentId",benchrange.DepartmentId),
                        new SqlParameter("@FromDate",benchrange.StartDatetime),
                        new SqlParameter("@ToDate",benchrange.EndDatetime),
                        new SqlParameter("@rptType",benchrange.ReportType)
                    };
                result = _procedureWork.ExecStoredProcedure<ListofBenchWork_Result>("ListofBenchWork @EmployeeId, @ClientId, @DepartmentId, @FromDate, @ToDate, @rptType", sqlPara).ToList();
            }
            else if (benchrange.ReportType == "Production Report")
            {
                var sqlPara = new SqlParameter[]
                    {
                        new SqlParameter("@EmployeeId",temp),
                        new SqlParameter("@ClientId",benchrange.ClientId),
                        new SqlParameter("@DepartmentId",benchrange.DepartmentId),
                        new SqlParameter("@FromDate",benchrange.StartDatetime),
                        new SqlParameter("@ToDate",benchrange.EndDatetime),
                        new SqlParameter("@rptType",benchrange.ReportType)
                    };
                result = _procedureWork.ExecStoredProcedure<ListofBenchWork_Result>("ListofBenchWork @EmployeeId, @ClientId, @DepartmentId, @FromDate, @ToDate, @rptType", sqlPara).ToList();
            }
            else
            {
                var sqlPara = new SqlParameter[]
                    {
                        new SqlParameter("@EmployeeId",temp),
                        new SqlParameter("@ClientId",benchrange.ClientId),
                        new SqlParameter("@DepartmentId",benchrange.DepartmentId),
                        new SqlParameter("@FromDate",benchrange.StartDatetime),
                        new SqlParameter("@ToDate",benchrange.EndDatetime),
                        new SqlParameter("@rptType",benchrange.ReportType)
                    };
                result = _procedureWork.ExecStoredProcedure<ListofBenchWork_Result>("ListofBenchWork @EmployeeId, @ClientId, @DepartmentId, @FromDate, @ToDate, @rptType", sqlPara).ToList();
            }
            return result;
        }
        public bool Startbench(EmployeeAssign1 Startbench, string Worktype)
        {
          
                var dept = _db.Departments.Where(x => x.Description == "Bench Option").FirstOrDefault();
                var pwtDetails = _db.ProcessWorkFlowTrans.Where(x => x.DepartmentId == dept.Id && x.AllocattedById == Startbench.EmployeeId).OrderByDescending(x => x.Id).FirstOrDefault();
                if (Worktype == "Start")
                {
                    var startpwt = new ProcessWorkFlowTran();
                    var employeeAssign = new int();
                    if (pwtDetails == null)
                    {
                        startpwt = _db.ProcessWorkFlowTrans.Where(x => x.DepartmentId == dept.Id).OrderByDescending(x => x.Id).FirstOrDefault();
                        employeeAssign = _db.EmployeeAssigns.Count(x => x.Wftid == startpwt.Id && x.EmployeeId == Startbench.EmployeeId && x.Status == "In Process" && x.IsActive == true);
                    }
                    else
                    {
                        startpwt = _db.ProcessWorkFlowTrans.Where(x => x.DepartmentId == dept.Id && x.AllocattedById == Startbench.EmployeeId).OrderByDescending(x => x.Id).FirstOrDefault();
                        employeeAssign = _db.EmployeeAssigns.Count(x => x.Wftid == startpwt.Id && x.EmployeeId == Startbench.EmployeeId && x.Status == "In Process" && x.IsActive == true);
                    }
                    //var startpwt = _db.ProcessWorkFlowTrans.Where(x => x.DepartmentId == dept.Id).OrderByDescending(x => x.Id).FirstOrDefault();
                    //var employeeAssign = _db.EmployeeAssigns.Count(x => x.WFTId == startpwt.Id && x.EmployeeId == Startbench.EmployeeId && x.Status == "In Process" && x.IsActive == true);
                    if (employeeAssign == 0)
                    {
                        ProcessWorkFlowTran insertPWT = new ProcessWorkFlowTran();
                        insertPWT.Wfmid = startpwt.Wfmid;
                        insertPWT.Jid = startpwt.Jid;
                        insertPWT.DepartmentId = startpwt.DepartmentId;
                        insertPWT.JobStatusId = startpwt.JobStatusId;
                        insertPWT.AllocattedById = Startbench.EmployeeId;
                        insertPWT.StatusId = 1;
                        insertPWT.CreatedBy = Startbench.EmployeeId;
                        insertPWT.CreatedUtc = DateTime.UtcNow;
                        insertPWT.IsActive = false;
                        _db.ProcessWorkFlowTrans.Add(insertPWT);

                        EmployeeAssign empAssign = new EmployeeAssign();
                        empAssign.AllocattedDate = DateTime.UtcNow;
                        empAssign.StartDate = DateTime.UtcNow;
                        empAssign.EmployeeId = Startbench.EmployeeId;
                    //empAssign.Wftid = insertPWT.Id;
                       empAssign.Wft = insertPWT;
                        empAssign.Wfmid = startpwt.Wfmid;
                        empAssign.Jid = startpwt.Jid;
                        empAssign.Status = "In Process";
                        empAssign.IsActive = true;
                        empAssign.IsBench = true;
                        empAssign.ProcessId = null;
                        empAssign.CreatedBy = Startbench.EmployeeId;
                        empAssign.CreatedUtc = DateTime.UtcNow;
                        _db.EmployeeAssigns.Add(empAssign);

                        EmployeeAssign insertEmployeeAssign = new EmployeeAssign();
                        // insertEmployeeAssign.Wftid = insertPWT.Id;
                        insertEmployeeAssign.Wft = insertPWT;

                        insertEmployeeAssign.Wfmid = startpwt.Wfmid;
                        insertEmployeeAssign.Jid = startpwt.Jid;
                        insertEmployeeAssign.EmployeeId = Startbench.EmployeeId;
                        insertEmployeeAssign.AllocattedDate = empAssign.AllocattedDate;
                        insertEmployeeAssign.Status = "Working";
                        insertEmployeeAssign.StartDate = DateTime.UtcNow;
                        insertEmployeeAssign.IsActive = true;
                        insertEmployeeAssign.IsBench = true;
                        insertEmployeeAssign.CreatedBy = Startbench.EmployeeId;
                        insertEmployeeAssign.CreatedUtc = DateTime.UtcNow;
                        _db.EmployeeAssigns.Add(insertEmployeeAssign);
                    }
                    else
                    {
                        var employeeAssignLastRecord = _db.EmployeeAssigns.Where(x => x.Wftid == pwtDetails.Id && x.EmployeeId == Startbench.EmployeeId && x.IsActive == true).OrderByDescending(x => x.Id).FirstOrDefault();
                        employeeAssignLastRecord.EndDate = DateTime.UtcNow;
                        DateTime starttimedate = employeeAssignLastRecord.StartDate.Value;
                        DateTime endtimedate = DateTime.UtcNow;
                        TimeSpan gettime = endtimedate - starttimedate;
                        double totalmins = Math.Floor(gettime.TotalMinutes);
                        int timetaken = (int)totalmins;

                        employeeAssignLastRecord.TotalTimeTaken = timetaken;
                        employeeAssignLastRecord.UpdatedBy = Startbench.EmployeeId;
                        employeeAssignLastRecord.BenchTypeId = Startbench.BenchTypeId;
                        employeeAssignLastRecord.UpdatedUtc = DateTime.UtcNow;
                        _db.Entry(employeeAssignLastRecord).State = EntityState.Modified;
                        EmployeeAssign insertEmployeeAssign = new EmployeeAssign();
                        insertEmployeeAssign.Status = Worktype;
                        insertEmployeeAssign.Wftid = employeeAssignLastRecord.Wftid;
                        insertEmployeeAssign.Wfmid = pwtDetails.Wfmid;
                        insertEmployeeAssign.Jid = pwtDetails.Jid;
                        insertEmployeeAssign.EmployeeId = Startbench.EmployeeId;
                        insertEmployeeAssign.AllocattedDate = employeeAssignLastRecord.AllocattedDate;
                        insertEmployeeAssign.EstimatedTime = employeeAssignLastRecord.EstimatedTime;
                        insertEmployeeAssign.Status = "Working";
                        insertEmployeeAssign.BenchTypeId = Startbench.BenchTypeId;
                        insertEmployeeAssign.StartDate = DateTime.UtcNow;
                        insertEmployeeAssign.IsActive = true;
                        insertEmployeeAssign.IsBench = true;
                        insertEmployeeAssign.CreatedBy = Startbench.EmployeeId;
                        insertEmployeeAssign.CreatedUtc = DateTime.UtcNow;
                        _db.EmployeeAssigns.Add(insertEmployeeAssign);
                    }
                }
                else if (Worktype == "End")
                {
                    var employeeAssignLastRecord = _db.EmployeeAssigns.Where(x => x.Wftid == pwtDetails.Id && x.EmployeeId == Startbench.EmployeeId && x.IsActive == true).OrderByDescending(x => x.Id).FirstOrDefault();
                    employeeAssignLastRecord.EndDate = DateTime.UtcNow;
                    DateTime starttimedate = employeeAssignLastRecord.StartDate.Value;
                    DateTime endtimedate = DateTime.UtcNow;
                    TimeSpan gettime = endtimedate - starttimedate;
                    double totalmins = Math.Floor(gettime.TotalMinutes);
                    int timetaken = (int)totalmins;
                    employeeAssignLastRecord.Status = "Worked";
                    employeeAssignLastRecord.TotalTimeTaken = timetaken;
                    employeeAssignLastRecord.UpdatedBy = Startbench.EmployeeId;
                    employeeAssignLastRecord.BenchTypeId = Startbench.BenchTypeId;
                    employeeAssignLastRecord.UpdatedUtc = DateTime.UtcNow;
                    _db.Entry(employeeAssignLastRecord).State = EntityState.Modified;

                    var employeeAssignRecords = _db.EmployeeAssigns.Where(x => x.Wfmid == pwtDetails.Wfmid).OrderBy(x => x.Id).ToList();
                    var employeeAssignFirstRecord = employeeAssignRecords.Where(x => x.Wftid == pwtDetails.Id && x.EmployeeId == Startbench.EmployeeId && x.IsActive == true && x.Status == "In Process").FirstOrDefault();
                    var totalTime = employeeAssignRecords.Where(x => x.Wftid == pwtDetails.Id && x.EmployeeId == Startbench.EmployeeId && x.IsActive == true && (x.Status == "Worked")).Sum(x => x.TotalTimeTaken);

                    employeeAssignFirstRecord.TotalTimeTaken = totalTime;
                    employeeAssignFirstRecord.EndDate = DateTime.UtcNow;
                    employeeAssignFirstRecord.Status = "Completed";
                    employeeAssignFirstRecord.IsActive = false;
                    employeeAssignFirstRecord.Remarks = Startbench.Remarks;
                    employeeAssignFirstRecord.UpdatedBy = Startbench.EmployeeId;
                    employeeAssignFirstRecord.UpdatedUtc = DateTime.UtcNow;
                    employeeAssignFirstRecord.TrainningId = Convert.ToInt32(Startbench.Status);
                    _db.Entry(employeeAssignFirstRecord).State = EntityState.Modified;
                }
                else
                {
                    var employeeAssignLastRecord = _db.EmployeeAssigns.Where(x => x.Wftid == pwtDetails.Id && x.EmployeeId == Startbench.EmployeeId && x.IsActive == true).OrderByDescending(x => x.Id).FirstOrDefault();
                    employeeAssignLastRecord.EndDate = DateTime.UtcNow;
                    DateTime starttimedate = employeeAssignLastRecord.StartDate.Value;
                    DateTime endtimedate = DateTime.UtcNow;
                    TimeSpan gettime = endtimedate - starttimedate;
                    double totalmins = Math.Floor(gettime.TotalMinutes);
                    int timetaken = (int)totalmins;
                    employeeAssignLastRecord.Status = "Worked";
                    employeeAssignLastRecord.TotalTimeTaken = timetaken;
                    employeeAssignLastRecord.UpdatedBy = Startbench.EmployeeId;
                    employeeAssignLastRecord.BenchTypeId = Startbench.BenchTypeId;
                    employeeAssignLastRecord.UpdatedUtc = DateTime.UtcNow;
                    _db.Entry(employeeAssignLastRecord).State = EntityState.Modified;
                    EmployeeAssign insertEmployeeAssign = new EmployeeAssign();
                    insertEmployeeAssign.Status = Worktype;
                    insertEmployeeAssign.Wftid = employeeAssignLastRecord.Wftid;
                    insertEmployeeAssign.Wfmid = pwtDetails.Wfmid;
                    insertEmployeeAssign.Jid = pwtDetails.Jid;
                    insertEmployeeAssign.EmployeeId = Startbench.EmployeeId;
                    insertEmployeeAssign.AllocattedDate = employeeAssignLastRecord.AllocattedDate;
                    insertEmployeeAssign.EstimatedTime = employeeAssignLastRecord.EstimatedTime;
                    insertEmployeeAssign.Status = "Break";
                    insertEmployeeAssign.BenchTypeId = Startbench.BenchTypeId;
                    insertEmployeeAssign.StartDate = DateTime.UtcNow;
                    insertEmployeeAssign.IsActive = true;
                    insertEmployeeAssign.IsBench = true;
                    insertEmployeeAssign.CreatedBy = Startbench.EmployeeId;
                    insertEmployeeAssign.CreatedUtc = DateTime.UtcNow;
                    _db.EmployeeAssigns.Add(insertEmployeeAssign);
                }
                _db.SaveChanges();
                return true;
           
        }
        public IEnumerable<Employee> GetEmployee(string DepartmentId)
        {
            var test = _tableWork.EmployeeRepository.Get(x => x.IsDeleted == false && (x.DateOfResignation == null || x.ResignReasons == null)).OrderBy(x => x.EmployeeName).ToList();
            return test;
        }
    }
}

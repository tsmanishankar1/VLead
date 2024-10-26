using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class CustomerVsProcessService
    {

        private readonly TableWork _tableWork;
        private readonly ViewWork _viewWork;
        private readonly TableWork tow;
        private readonly ApplicationDbContext _db;
        public CustomerVsProcessService(TableWork tableWork,ViewWork viewWork,ApplicationDbContext dbContext)
        {
            _db=dbContext;
            tow = tableWork;
            _tableWork = tableWork;
            _viewWork = viewWork;
        }
      

        public Object GetAllDropDownList()
        {
            var Dept = _tableWork.DepartmentRepository.Get(x => (x.IsDeleted == false) && (x.Description.Contains("Artwork") || x.Description.Contains("Digi") || x.Description.Contains("Train"))).AsQueryable();
            var currentprocess = _tableWork.ProcessRepository.Get(x => x.IsActive == true).Select(x => new { x.Id, x.Name }).AsQueryable();
            var jobstatus = _tableWork.JobStatusRepository.Get(x => x.IsActive == true).OrderByDescending(x => x.Id).Select(x => new { x.Id, x.JobStatusDescription }).AsQueryable();
            var status = _tableWork.StatusRepository.Get(x => x.IsActive == true).Select(x => new { x.Id, x.Name }).AsQueryable();

            var DetailList = new
            {
                DepartmentList = Dept,
                jobstatuslist = jobstatus,
                statuslist = status,
                CurrentProcessList = currentprocess
            };
            return DetailList;
        }

        public IEnumerable<Status> GetStatusddlList()
        {
            return _tableWork.StatusRepository.Get(x => x.IsActive == true).AsQueryable();
        }
        public IEnumerable<CustomerVsScope> GetScopeList(CustomerVsScope1 getscopeid)
        {
            var customerwithnull = new List<CustomerVsScope>();
            if (getscopeid.DeptId != null && getscopeid.CustomerJobType != null)
            // if (getscopeid.DeptId != null && getscopeid.CustomerId != 0)
            {
                if (getscopeid.CustomerId == 0)
                {
                    customerwithnull = _tableWork.CustomerVsScopeRepository.Get(x => x.IsActive == true && x.DeptId == getscopeid.DeptId && x.CustomerJobType == getscopeid.CustomerJobType).ToList();
                }
                else
                {
                    customerwithnull = _tableWork.CustomerVsScopeRepository.Get(x => x.IsActive == true && x.DeptId == getscopeid.DeptId && x.CustomerJobType == getscopeid.CustomerJobType && x.CustomerId == getscopeid.CustomerId).ToList();
                }
            }
            return customerwithnull;
        }
        public IEnumerable<ViewCustomerVsProcess> GetCustomerVsProcess()
        {
            return _viewWork.ViewCustomerVsProcessRepository.Get(x => x.IsActive == true).OrderByDescending(x => x.Id).AsQueryable();
        }
        private IEnumerable<ProcessWorkFlow> BuildProcessWorkFlow(int departmentId, int? customerId, int? currentProcessId, int statusId, int? nextProcessId, int? jobStatusId, int? scopeId, string customJobType, int createdBy)
        {
            //ProcessWorkFlow processWorkFlow = new ProcessWorkFlow();
            var processWorkFlows = new List<ProcessWorkFlow>();
            //var check = _db.View_CustomerVsProcess.FirstOrDefault(x => x.DepartmentId == departmentId && (x.CustomerId == null || x.CustomerId == customerId) && (x.CustomJobType == null || x.CustomJobType == customJobType) && (x.CurrentProcessId == currentProcessId) && (x.NextProcessId == nextProcessId) && (x.StatusId == statusId) && (x.JobStatusId == null) && (x.ScopeId == null));
            //var check = _db.ProcessWorkFlows.FirstOrDefault(x => x.IsActive == true && x.DepartmentId == departmentId && (x.CustomerId == null || x.CustomerId == customerId) && (x.CustomJobType == null || x.CustomJobType == customJobType) && (x.CurrentProcessId == currentProcessId) && (x.NextProcessId == nextProcessId) && (x.StatusId == statusId) && (x.JobStatusId == null) && (x.ScopeId == null));
            var check = _tableWork.ProcessWorkFlowRepository.GetSingle(x => x.IsActive == true && x.DepartmentId == departmentId && (x.CustomerId == null || x.CustomerId == customerId) && (x.CustomJobType == null || x.CustomJobType == customJobType) && (x.CurrentProcessId == currentProcessId) && (x.NextProcessId == nextProcessId) && (x.StatusId == statusId) && (x.JobStatusId == null) && (x.ScopeId == null));
            if (check == null)
            {
                processWorkFlows.Add(new ProcessWorkFlow
                {
                    DepartmentId = departmentId,
                    CustomerId = customerId,
                    CurrentProcessId = currentProcessId,
                    StatusId = statusId,
                    NextProcessId = nextProcessId,
                    JobStatusId = jobStatusId,
                    ScopeId = scopeId,
                    CustomJobType = customJobType,
                    IsActive = true,
                    CreatedBy = createdBy,
                    CreatedUtc = DateTime.UtcNow
                });
            }
            else
            {
                processWorkFlows.Remove(new ProcessWorkFlow
                {
                    DepartmentId = departmentId,
                    CustomerId = customerId,
                    CurrentProcessId = currentProcessId,
                    StatusId = statusId,
                    NextProcessId = nextProcessId,
                    JobStatusId = jobStatusId,
                    ScopeId = scopeId,
                    CustomJobType = customJobType,
                    IsActive = true,
                    CreatedBy = createdBy,
                    CreatedUtc = DateTime.UtcNow
                });
            }
            return processWorkFlows;

        }

        private IEnumerable<ProcessWorkFlow> BuildProcessWorkFlowsWithJobStatusandScope(int[] jobStatusIds, int departmentId, int? customerId, int? currentProcessId, int statusId, int? nextProcessId, int? scopeId, string customJobType, int createdBy)
        {
            var processWorkFlows = new List<ProcessWorkFlow>();
            foreach (var jobstatusId in jobStatusIds)
            {

                //var check = _db.View_CustomerVsProcess.FirstOrDefault(x => x.DepartmentId == departmentId && (x.CustomerId == null || x.CustomerId == customerId) && (x.CustomJobType == null || x.CustomJobType == customJobType) && (x.CurrentProcessId == currentProcessId) && (x.NextProcessId == nextProcessId) && (x.StatusId == statusId) && (x.JobStatusId == jobstatusId) && (x.ScopeId == scopeId));
                var check = _tableWork.ProcessWorkFlowRepository.GetSingle(x => x.IsActive == true && x.DepartmentId == departmentId && (x.CustomerId == null || x.CustomerId == customerId) && (x.CustomJobType == null || x.CustomJobType == customJobType) && (x.CurrentProcessId == currentProcessId) && (x.StatusId == statusId) && (x.JobStatusId == jobstatusId) && (x.ScopeId == scopeId));
                if (check == null)
                {
                    processWorkFlows.Add(new ProcessWorkFlow
                    {
                        DepartmentId = departmentId,
                        CustomerId = customerId,
                        CurrentProcessId = currentProcessId,
                        StatusId = statusId,
                        NextProcessId = nextProcessId,
                        JobStatusId = jobstatusId,
                        ScopeId = scopeId,
                        IsActive = true,
                        CustomJobType = customJobType,
                        CreatedBy = createdBy,
                        CreatedUtc = DateTime.UtcNow
                    });
                }
                else
                {
                    processWorkFlows.Remove(new ProcessWorkFlow
                    {
                        DepartmentId = departmentId,
                        CustomerId = customerId,
                        CurrentProcessId = currentProcessId,
                        StatusId = statusId,
                        NextProcessId = nextProcessId,
                        JobStatusId = jobstatusId,
                        ScopeId = scopeId,
                        IsActive = true,
                        CustomJobType = customJobType,
                        CreatedBy = createdBy,
                        CreatedUtc = DateTime.UtcNow
                    });
                }

            }
            return processWorkFlows;
        }

        private IEnumerable<ProcessWorkFlow> BuildProcessWorkFlowsWithJobStatus(int[] jobStatusIds, int departmentId, int? customerId, int? currentProcessId, int statusId, int? nextProcessId, int? scopeId, string customJobType, int createdBy)
        {
            var processWorkFlows = new List<ProcessWorkFlow>();
            foreach (var jobstatusId in jobStatusIds)
            {
                var check = _tableWork.ProcessWorkFlowRepository.GetSingle(x => x.IsActive == true && x.DepartmentId == departmentId && (x.CustomerId == null || x.CustomerId == customerId) && (x.CustomJobType == null || x.CustomJobType == customJobType) && (x.CurrentProcessId == currentProcessId) && (x.StatusId == statusId) && (x.JobStatusId == jobstatusId) && (x.ScopeId == null));
                if (check == null)
                {
                    processWorkFlows.Add(new ProcessWorkFlow
                    {
                        DepartmentId = departmentId,
                        CustomerId = customerId,
                        CurrentProcessId = currentProcessId,
                        StatusId = statusId,
                        NextProcessId = nextProcessId,
                        JobStatusId = jobstatusId,
                        ScopeId = scopeId,
                        IsActive = true,
                        CustomJobType = customJobType,
                        CreatedBy = createdBy,
                        CreatedUtc= DateTime.UtcNow
                    });
                }
                else
                {
                    processWorkFlows.Remove(new ProcessWorkFlow
                    {
                        DepartmentId = departmentId,
                        CustomerId = customerId,
                        CurrentProcessId = currentProcessId,
                        StatusId = statusId,
                        NextProcessId = nextProcessId,
                        JobStatusId = jobstatusId,
                        ScopeId = scopeId,
                        IsActive = true,
                        CustomJobType = customJobType,
                        CreatedBy = createdBy,
                        CreatedUtc= DateTime.UtcNow
                    });
                }

            }
            return processWorkFlows;
        }
        private IEnumerable<ProcessWorkFlow> BuildProcessWorkFlowsWithScope(int[] scopeIds, int departmentId, int? customerId, int? currentProcessId, int statusId, int? nextProcessId, int? jobStatusId, string customJobType, int createdBy)
        {
            var processWorkFlows = new List<ProcessWorkFlow>();
            foreach (var scopeId in scopeIds)
            {
                var check = _tableWork.ProcessWorkFlowRepository.GetSingle(x => x.IsActive == true && x.DepartmentId == departmentId && (x.CustomerId == null || x.CustomerId == customerId) && (x.CustomJobType == null || x.CustomJobType == customJobType) && x.CurrentProcessId == currentProcessId && x.StatusId == statusId && x.ScopeId == scopeId && x.JobStatusId == null);
                if (check == null)
                {
                    processWorkFlows.Add(new ProcessWorkFlow
                    {
                        DepartmentId = departmentId,
                        CustomerId = customerId,
                        CurrentProcessId = currentProcessId,
                        StatusId = statusId,
                        NextProcessId = nextProcessId,
                        JobStatusId = jobStatusId,
                        ScopeId = scopeId,
                        IsActive = true,
                        CustomJobType = customJobType,
                        CreatedBy = createdBy,
                        CreatedUtc = DateTime.UtcNow
                    });
                }
                else
                {
                    processWorkFlows.Remove(new ProcessWorkFlow
                    {
                        DepartmentId = departmentId,
                        CustomerId = customerId,
                        CurrentProcessId = currentProcessId,
                        StatusId = statusId,
                        NextProcessId = nextProcessId,
                        JobStatusId = jobStatusId,
                        ScopeId = scopeId,
                        IsActive = true,
                        CustomJobType = customJobType,
                        CreatedBy = createdBy,
                        CreatedUtc= DateTime.UtcNow
                    });
                }
            }
            return processWorkFlows;
        }

        public bool AddProcessworkflow(AddProcessWorkFlow AddCustomerandprocess)
        {
            bool status = false;
            var result = new
            {
                Message = ""
            };
            try
            {
               
                    string CurStatusName = _db.Processes.Where(x => x.Id == AddCustomerandprocess.CurrentProcessId).FirstOrDefault().Name;
                    string NextStatusName = _db.Processes.Where(x => x.Id == AddCustomerandprocess.NextProcessId).FirstOrDefault().Name;

                    if (CurStatusName == "Client")
                    {
                        AddCustomerandprocess.CurrentProcessId = null;
                    }
                    if (NextStatusName == "Client")
                    {
                        AddCustomerandprocess.NextProcessId = null;
                    }

                    if (AddCustomerandprocess.DepartmentId != null && AddCustomerandprocess.NextProcessId != 0 && AddCustomerandprocess.CurrentProcessId != 0 && AddCustomerandprocess.StatusId != 0)
                    {
                        if (AddCustomerandprocess.selectedScopes != null && AddCustomerandprocess.JobStatusId != null)
                        {
                            foreach (var scope in AddCustomerandprocess.selectedScopes)
                            {
                                _db.ProcessWorkFlows.AddRange(BuildProcessWorkFlowsWithJobStatusandScope(
                                    jobStatusIds: AddCustomerandprocess.JobStatusId,
                                        departmentId: AddCustomerandprocess.DepartmentId,
                                        customerId: AddCustomerandprocess.CustomerId,
                                        currentProcessId: AddCustomerandprocess.CurrentProcessId,
                                        statusId: AddCustomerandprocess.StatusId,
                                        nextProcessId: AddCustomerandprocess.NextProcessId,
                                        scopeId: scope,
                                        customJobType: AddCustomerandprocess.CustomJobType,
                                        createdBy: AddCustomerandprocess.CreatedBy
                                        ));
                            }
                        }
                        if (AddCustomerandprocess.JobStatusId == null && AddCustomerandprocess.selectedScopes == null)
                        {
                            _db.ProcessWorkFlows.AddRange(BuildProcessWorkFlow(
                                        departmentId: AddCustomerandprocess.DepartmentId,
                                        customerId: AddCustomerandprocess.CustomerId,
                                        currentProcessId: AddCustomerandprocess.CurrentProcessId,
                                        statusId: AddCustomerandprocess.StatusId,
                                        nextProcessId: AddCustomerandprocess.NextProcessId,
                                        jobStatusId: null,
                                        scopeId: null,
                                        customJobType: AddCustomerandprocess.CustomJobType,
                                        createdBy: AddCustomerandprocess.CreatedBy
                                        ));
                        }
                        if (AddCustomerandprocess.selectedScopes == null && AddCustomerandprocess.JobStatusId != null)
                        {
                            _db.ProcessWorkFlows.AddRange(BuildProcessWorkFlowsWithJobStatus(
                                    jobStatusIds: AddCustomerandprocess.JobStatusId,
                                        departmentId: AddCustomerandprocess.DepartmentId,
                                        customerId: AddCustomerandprocess.CustomerId,
                                        currentProcessId: AddCustomerandprocess.CurrentProcessId,
                                        statusId: AddCustomerandprocess.StatusId,
                                        nextProcessId: AddCustomerandprocess.NextProcessId,
                                        scopeId: null,
                                        customJobType: AddCustomerandprocess.CustomJobType,
                                        createdBy: AddCustomerandprocess.CreatedBy
                                        ));
                        }
                        if (AddCustomerandprocess.selectedScopes != null && AddCustomerandprocess.JobStatusId == null)
                        {
                            _db.ProcessWorkFlows.AddRange(BuildProcessWorkFlowsWithScope(
                                scopeIds: AddCustomerandprocess.selectedScopes,
                                        departmentId: AddCustomerandprocess.DepartmentId,
                                        customerId: AddCustomerandprocess.CustomerId,
                                        currentProcessId: AddCustomerandprocess.CurrentProcessId,
                                        statusId: AddCustomerandprocess.StatusId,
                                        nextProcessId: AddCustomerandprocess.NextProcessId,
                                        jobStatusId: null,
                                        customJobType: AddCustomerandprocess.CustomJobType,
                                        createdBy: AddCustomerandprocess.CreatedBy
                                        ));
                        }
                        var LocalCount = _db.ProcessWorkFlows.Local.Count();

                        var localCheckcount = _db.ProcessWorkFlows.Local.FirstOrDefault();

                        if (LocalCount == 1)
                        {
                            var InDb = _db.ProcessWorkFlows.Where(x => x.DepartmentId == localCheckcount.DepartmentId && x.CustomerId == localCheckcount.CustomerId
                            && x.CustomJobType == localCheckcount.CustomJobType && x.JobStatusId == localCheckcount.JobStatusId && x.ScopeId == localCheckcount.ScopeId
                            && x.CurrentProcessId == localCheckcount.CurrentProcessId && x.StatusId == localCheckcount.StatusId && x.IsActive == localCheckcount.IsActive).ToList();

                            if (InDb.Count() != 0)
                            {
                                return status = false;
                            }
                            else
                            {
                                _db.SaveChanges();
                                return status = true;
                            }
                        }

                        if (LocalCount > 1)
                        {
                            _db.SaveChanges();
                            return status = true;
                        }
                        else
                        {
                            return status = false;
                        }
                    }
            
            }
            catch (Exception e)
            {
                //Message = "CustomervsProcess Error to set Processworkflow engine";
                Log savelog = new Log();
                savelog.Module = "ProcessFlow engine error  in catch- CustomervsProcess";
                savelog.Description = e.Message + " " + e.InnerException + " " + e.StackTrace + " " + e.Source;
                savelog.Type = "Error";
                savelog.CreatedUtc = DateTime.UtcNow;
                _tableWork.LogRepository.Insert(savelog);
                _tableWork.SaveChanges();
            }

            return status;
        }
        public ViewCustomerVsProcess GetspecifiedCustomerVsProcessDetail(CustomerVsEmployee EditCustomerandprocess)
        {
            return _viewWork.ViewCustomerVsProcessRepository.GetSingle(x => x.Id == EditCustomerandprocess.Id);
        }
        public string UpdateProcessworkflow(ProcessWorkFlow UpdateCustomerandprocess)
        {
            bool status = false;
            var message = "";
            try
            {
              
                    if (UpdateCustomerandprocess.Id != null)
                    {
                        if (UpdateCustomerandprocess.DepartmentId != null && UpdateCustomerandprocess.CurrentProcessId != null && UpdateCustomerandprocess.StatusId != null && UpdateCustomerandprocess.NextProcessId != null)
                        {
                            var checkRecordDetails = tow.ProcessWorkFlowRepository.GetSingle(x => x.DepartmentId == UpdateCustomerandprocess.DepartmentId && (x.CustomerId == null || x.CustomerId == UpdateCustomerandprocess.CustomerId) && (x.ScopeId == null || x.ScopeId == UpdateCustomerandprocess.ScopeId) && (x.JobStatusId == null || x.JobStatusId == UpdateCustomerandprocess.JobStatusId) && x.CurrentProcessId == UpdateCustomerandprocess.CurrentProcessId && x.StatusId == UpdateCustomerandprocess.StatusId && x.NextProcessId == UpdateCustomerandprocess.NextProcessId && x.IsActive == true);
                            if (checkRecordDetails == null)
                            {
                                var updateprocess = tow.ProcessWorkFlowRepository.GetSingle(x => x.Id == UpdateCustomerandprocess.Id);
                                updateprocess.DepartmentId = UpdateCustomerandprocess.DepartmentId;
                                //if (UpdateCustomerandprocess.CustomerId == null)
                                //{
                                //    updateprocess.CustomerId = null;
                                //}
                                //else
                                //{
                                //    updateprocess.CustomerId = UpdateCustomerandprocess.CustomerId;
                                //}
                                updateprocess.CustomerId = UpdateCustomerandprocess.CustomerId;
                                updateprocess.CurrentProcessId = UpdateCustomerandprocess.CurrentProcessId;
                                updateprocess.StatusId = UpdateCustomerandprocess.StatusId;
                                updateprocess.NextProcessId = UpdateCustomerandprocess.NextProcessId;
                                updateprocess.IsActive = true;
                                updateprocess.CreatedBy = UpdateCustomerandprocess.CreatedBy;
                                updateprocess.ScopeId = UpdateCustomerandprocess.ScopeId;
                                updateprocess.JobStatusId = UpdateCustomerandprocess.JobStatusId;
                                updateprocess.CustomJobType = UpdateCustomerandprocess.CustomJobType;
                                updateprocess.CreatedUtc = DateTime.UtcNow;
                                tow.ProcessWorkFlowRepository.Update(updateprocess);
                                long dbstatus = tow.SaveChanges();
                                status = dbstatus > 0;
                                message = "Updated Successfully";
                            }
                            else
                            {
                                message = "Record Already available";
                            }
                        }
                    }
              
            }
            catch (Exception e)
            {
                Log addlog = new Log();
                addlog.Module = "UpdateProcessworkflow Function in Customer vs Process Rep.";
                addlog.Description = "Error Msg UpdateProcessworkflow Function: " + e.Message.ToString() + "-" + "Inner Exc: " + e.InnerException + ", Stack Trace: " + e.StackTrace + ", Source:" + e.Source + "string";
                addlog.Type = "Error Occ";
                addlog.CreatedUtc = DateTime.UtcNow;
                _tableWork.LogRepository.Insert(addlog);
                _tableWork.SaveChanges();
            }

            return message;
        }
        public bool DeleteProcessworkflow(int Id, int EmployeeId)
        {
            bool status = false;
            try
            {
              
                    var deleteProcessWorkFlow = tow.ProcessWorkFlowRepository.GetSingle(x => x.Id == Id);
                    deleteProcessWorkFlow.IsActive = false;
                    deleteProcessWorkFlow.UpdatedUtc= DateTime.UtcNow;
                    deleteProcessWorkFlow.UpdatedBy = EmployeeId;
                    tow.ProcessWorkFlowRepository.Update(deleteProcessWorkFlow);
                    long dbstatus = tow.SaveChanges();
                    status = dbstatus > 0;
                    status = true;
             
            }
            catch (Exception e)
            {
                Log savelog = new Log();
                savelog.Module = "ProcessFlow Delete error in catch- CustomervsProcess";
                savelog.Description = e.Message + " " + e.InnerException + " " + e.StackTrace + " " + e.Source;
                savelog.Type = "Error";
                savelog.CreatedUtc = DateTime.UtcNow;
                _tableWork.LogRepository.Insert(savelog);
                _tableWork.SaveChanges();
            }
            return status;

        }
        public ViewCustomerVsProcess GetEditCustomerVsProcess(int Id)
        {
            return _viewWork.ViewCustomerVsProcessRepository.GetSingle(x => x.Id == Id);
        }
        public IEnumerable<Process> GetNextProcessddlList(Process1 getnextprocess)
        {
            return _tableWork.ProcessRepository.Get(x => x.IsActive == true && x.Id != getnextprocess.Id).ToArray();
        }
        public ViewCustomerVsProcess GetCustomerVsProcessById(int Id)
        {
            return _viewWork.ViewCustomerVsProcessRepository.GetSingle(x => x.IsActive == true && x.Id == Id);
        }
        public IEnumerable<Customer> GetCustomerddlList(ProcessWorkFlow1 getdeptid)
        {
            var customerIds = _tableWork.ProcessWorkFlowRepository.Get(x => x.DepartmentId == getdeptid.DepartmentId && x.CustomerId != null).Select(x => new { CustomerId = x.CustomerId }).GroupBy(x => x.CustomerId);
            List<int?> tempCustomerIdList = customerIds.Select(x => x.Key).ToList();
            var customers = _tableWork.CustomerRepository.Get(x => !tempCustomerIdList.Contains(x.Id));
            return customers;
        }
        public IEnumerable<ViewCustomerVsProcess> GetCustomerListddl(ProcessWorkFlow getdeptid)
        {
            //var cust = new List<View_CustomerVsProcess>();
            var cust = _viewWork.ViewCustomerVsProcessRepository.Get(x => x.DepartmentId == getdeptid.DepartmentId).ToList();
            return cust;
        }
        public bool SaveCustomerVsProcess(SaveCustomerVsProcessViewModel customervsprocess)
        {
            bool status = false;
            try
            {
                var Addedprocess = customervsprocess.AddedProcess;
                var customerids = customervsprocess.CustomerId;
                if (customerids == null)
                {
                    foreach (var saveitem in Addedprocess)
                    {
                        ProcessWorkFlow saveprocess = new ProcessWorkFlow();
                        saveprocess.DepartmentId = customervsprocess.DepartmentId;
                        saveprocess.CustomerId = null;
                        saveprocess.CurrentProcessId = saveitem.CurrentProcessId;
                        saveprocess.StatusId = saveitem.StatusId;
                        saveprocess.NextProcessId = saveitem.NextProcessId;
                        saveprocess.IsActive = true;
                        saveprocess.CreatedBy = customervsprocess.CreatedBy;
                        saveprocess.CreatedUtc = DateTime.UtcNow;
                        _tableWork.ProcessWorkFlowRepository.Insert(saveprocess);
                        long dbstatus = _tableWork.SaveChanges();
                        status = dbstatus > 0;
                    }
                }
                else
                {
                    foreach (var item in customerids)
                    {
                        foreach (var saveitem in Addedprocess)
                        {
                            ProcessWorkFlow saveprocess = new ProcessWorkFlow();
                            saveprocess.DepartmentId = customervsprocess.DepartmentId;
                            saveprocess.CustomerId = item;
                            saveprocess.CurrentProcessId = saveitem.CurrentProcessId;
                            saveprocess.StatusId = saveitem.StatusId;
                            saveprocess.NextProcessId = saveitem.NextProcessId;
                            saveprocess.IsActive = true;
                            saveprocess.CreatedBy = customervsprocess.CreatedBy;
                            saveprocess.CreatedUtc = DateTime.UtcNow;
                            _tableWork.ProcessWorkFlowRepository.Insert(saveprocess);
                            long dbstatus = _tableWork.SaveChanges();
                            status = dbstatus > 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log addlog = new Log();
                addlog.Module = "SaveCustomerVsProcess Function in Customer vs Process Rep.";
                addlog.Description = "Error Msg SaveCustomerVsProcess Function: " + e.Message.ToString() + "-" + "Inner Exc: " + e.InnerException + ", Stack Trace: " + e.StackTrace + ", Source:" + e.Source + "string";
                addlog.Type = "Error Occ";
                addlog.CreatedUtc = DateTime.UtcNow;
                _tableWork.LogRepository.Insert(addlog);
                _tableWork.SaveChanges();
            }
            return status;
        }
    }
}

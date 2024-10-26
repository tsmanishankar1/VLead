using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class JobTransferService
    {
        private readonly TableWork _tableWork;
        private readonly UnitWork _unitWork;
        public JobTransferService(TableWork tableWork,UnitWork unitWork)
        {
            _tableWork = tableWork;
            _unitWork = unitWork;
        }
        public IEnumerable<JobOrder> JobOrderDetails(JobOrder1 joborder)
        {
            if (joborder.JobId != null)
            {
                return _tableWork.JobOrderRepository.GetAllVal(x => x.Customer, x => x.Department).Where(x => x.IsDeleted == false &&
                x.JobId.Contains(joborder.JobId) && x.DateofUpload == null && x.IsConvertDepartment == null).ToList();
            }
            else if (joborder.FileName != null)
            {
                return _tableWork.JobOrderRepository.GetAllVal(x => x.Customer, x => x.Department).Where(x => x.IsDeleted == false &&
                x.FileName.Contains(joborder.FileName) && x.DateofUpload == null && x.IsConvertDepartment == null).ToList();
            }
            else if (joborder.ClientId != null)
            {
                return _tableWork.JobOrderRepository.GetAllVal(x => x.Customer, x => x.Department).Where(x => x.IsDeleted == false &&
                x.ClientId == joborder.ClientId && x.DateofUpload == null && x.IsConvertDepartment == null).ToList();
            }
            else if (joborder.FileReceivedDate != null)
            {
                return _tableWork.JobOrderRepository.GetAllVal(x => x.Customer, x => x.Department).Where(x => x.IsDeleted == false &&
                x.FileReceivedDate == joborder.FileReceivedDate && x.DateofUpload == null && x.IsConvertDepartment == null).ToList();
            }
            else
            {
                return _tableWork.JobOrderRepository.GetAllVal(x => x.Customer, x => x.Department).Where(x => x.IsDeleted == false && x.ClientId == joborder.ClientId && x.FileReceivedDate == joborder.FileReceivedDate &&
                x.JobId.Contains(joborder.JobId) && x.FileName.Contains(joborder.FileName) && x.DateofUpload == null && x.IsConvertDepartment == null).ToList();
            }
        }
        public IEnumerable<Customer> AllCustomerlist()
        {
            return _tableWork.CustomerRepository.Get(x => x.IsDeleted == false && x.IsBlacklisted == false).ToList();
        }
        public FileMovement ConvertDepartment(JobTransferViewModel joborder)
        {
            var fileMovement = new FileMovement();
            foreach (var item in joborder.ConvertDepartment)
            {
                var deletejoborder = _unitWork.JobOrder.FirstOrDefault(x => x.Id == item.Id);
                deletejoborder.IsDeleted = true;
                deletejoborder.IsConvertDepartment = true;
                deletejoborder.UpdatedBy = joborder.UpdatedBy;
                deletejoborder.UpdatedUtc = DateTime.UtcNow;
                //_db.Entry(deletejoborder).State = EntityState.Modified;
                _unitWork.JobOrder.Update(deletejoborder);

                var deletetdm = _unitWork.TrayDetailMovement.FirstOrDefault(x => x.JobId == item.JobId);
                deletetdm.IsDeleted = true;
                _unitWork.TrayDetailMovement.Update(deletetdm);

                _unitWork.SaveChanges();

                var updateclientorder = _unitWork.ClientOrder.FirstOrDefault(x => x.OrderId == deletejoborder.ClientOrderId);
                if (updateclientorder != null)
                {
                    if (item.DepartmentId == 1)
                    {
                        updateclientorder.WorkType = 2;
                    }
                    else
                    {
                        updateclientorder.WorkType = 1;
                    }
                    updateclientorder.IsCompleted = false;
                    updateclientorder.IsConverted = false;
                    //_db.Entry(updateclientorder).State = EntityState.Modified;
                    _unitWork.ClientOrder.Update(updateclientorder);
                    _unitWork.SaveChanges();
                }
                else
                {
                    fileMovement.JobId = "";
                    if (item.JobStatusId == 7 || item.JobStatusId == 8 || item.JobStatusId == 9)
                    {
                        var CheckFileNameExist = _unitWork.JobOrder.Where(x => x.FileName == item.FileName && x.ClientId == item.ClientId && x.DepartmentId == item.DepartmentId && x.JobStatusId != 10 && x.IsDeleted == false).ToList();
                        //var CheckFileNameExist = _unitWork.JobOrder.Where(x => x.FileName == item.FileName && x.ClientId == item.ClientId && x.JobStatusId != 10 && x.IsDeleted == false).ToList();//
                        if (CheckFileNameExist.Count != 0)
                        {
                            fileMovement.JobId = "File Name Already Exist!";
                            return fileMovement;
                        }
                    }
                    int deptid = 0;
                    if (item.DepartmentId == 1)
                    {
                        deptid = 2;
                    }
                    else
                    {
                        deptid = 1;
                    }
                    int jobStatusId = item.JobStatusId;
                    var jobStatusGroupId = _unitWork.JobStatus.Where(x => x.Id == jobStatusId).Select(x => x.JobStatusGroupId).FirstOrDefault();
                    DateTime fileReceivedDate = item.FileReceivedDate;
                    var transactionTypeId = _unitWork.TransactionType.Where(x => x.JobStatusGroupId == jobStatusGroupId).Select(x => x.Id).FirstOrDefault();
                    //var voucherControl = _unitWork.VoucherControl.FirstOrDefault(x => x.TransactionId == transactionTypeId && x.DepartmentId == deptid);
                    DateTime jobdate = DateTime.UtcNow;
                    var voucherControl = _unitWork.VoucherControl.FirstOrDefault(x => x.TransactionId == transactionTypeId && x.DepartmentId == deptid && x.EffectiveFrom <= jobdate && x.EffectiveTo >= jobdate && x.IsDeleted == false && x.IsActive == true);
                    if (voucherControl == null)
                    {
                        fileMovement.Message = "VoucherControl is Missing";
                        var updtdjoborder = _unitWork.JobOrder.FirstOrDefault(x => x.Id == item.Id);
                        updtdjoborder.IsDeleted = false;
                        _unitWork.JobOrder.Update(updtdjoborder);
                        _unitWork.SaveChanges();
                        return fileMovement;
                    }
                    string voucherControlPrefix = voucherControl.Prefix.ToString();
                    long voucherNo = voucherControl.Voucherno + 1;
                    string month = fileReceivedDate.ToString("MM");
                    string day = fileReceivedDate.ToString("dd");
                    string dynamicJobId = voucherControlPrefix + month + day + "/" + voucherNo;

                    var jobOrder = new JobOrder();
                    jobOrder.TransactionId = transactionTypeId;
                    jobOrder.JobId = dynamicJobId;
                    jobOrder.JobDate = DateTime.UtcNow;
                    jobOrder.JobDescription = ".";
                    jobOrder.JobStatusId = item.JobStatusId;
                    if (item.DepartmentId == 1)
                    {
                        jobOrder.DepartmentId = 2;
                    }
                    else
                    {
                        jobOrder.DepartmentId = 1;
                    }
                    jobOrder.FileReceivedDate = item.FileReceivedDate;
                    jobOrder.FileName = item.FileName;
                    jobOrder.FileInwardTypeId = item.FileInwardTypeId;
                    jobOrder.Username = item.Username;
                    jobOrder.SalesPersonName = item.SalesPersonName;
                    jobOrder.CustomerName = item.CustomerName;
                    jobOrder.Temp = item.Temp;
                    jobOrder.Style = item.Style;
                    jobOrder.ProjectCode = item.ProjectCode;
                    jobOrder.TeamCode = item.TeamCode;
                    jobOrder.SchoolName = item.SchoolName;
                    jobOrder.IsDeleted = false;
                    jobOrder.ClientId = item.ClientId;
                    jobOrder.CustomerJobType = item.CustomerJobType;
                    jobOrder.CreatedBy = joborder.UpdatedBy;
                    jobOrder.ClientOrderId = null;
                    jobOrder.FileUploadPath = item.FileUploadPath;
                    //
                    jobOrder.EmployeeId = item.EmployeeId;
                    jobOrder.Ccid = item.Ccid;
                    jobOrder.CcemailId = item.CcemailId;
                    jobOrder.ClientSalesPerson = item.ClientSalesPerson;
                    //
                    var checkFreshJobOrder = _unitWork.JobOrder.FirstOrDefault(x => x.ClientId == item.ClientId && x.FileName == item.FileName && x.DepartmentId == item.DepartmentId && x.IsDeleted == false);
                    var checkFreshJobOrderList = _unitWork.JobOrder.Where(x => x.ClientId == item.ClientId && x.FileName.Equals(item.FileName) && x.DepartmentId == item.DepartmentId && x.IsDeleted == false).OrderBy(x => x.Id).ToList();

                    var insertJobOrdersingle = false;
                    if (checkFreshJobOrder != null)
                    {
                        if (checkFreshJobOrder.DateofUpload != null && checkFreshJobOrder.DateofUpload < jobOrder.JobDate)
                        {
                            if (checkFreshJobOrderList.Count > 0)
                            {
                                var parentjobidsingle = checkFreshJobOrderList.First();
                                var referencejobidsingle = checkFreshJobOrderList.Last();

                                jobOrder.ParentJobId = parentjobidsingle.JobId;
                                jobOrder.JobReferenceId = referencejobidsingle.JobId;
                            }
                            insertJobOrdersingle = true;
                        }
                        else
                        {
                            fileMovement.JobId = "Previous Job is not closed for the File Name and Client!";
                            return fileMovement;
                        }
                    }
                    else
                    {
                        insertJobOrdersingle = true;
                    }
                    if (insertJobOrdersingle)
                    {
                        _unitWork.JobOrder.Add(jobOrder);
                        _unitWork.SaveChanges();
                        var processWorkFlowMaster = new ProcessWorkFlowMaster();
                        processWorkFlowMaster.TransactionId = jobOrder.TransactionId;
                        processWorkFlowMaster.JobId = _tableWork.JobOrderRepository.GetLastRecord(s => s.Id).Id;
                        processWorkFlowMaster.CustomerId = jobOrder.ClientId.Value;
                        processWorkFlowMaster.DepartmentId = jobOrder.DepartmentId;
                        processWorkFlowMaster.IsDeleted = jobOrder.IsDeleted;
                        processWorkFlowMaster.CreatedBy = jobOrder.CreatedBy;
                        processWorkFlowMaster.CreatedUtc = DateTime.UtcNow;
                        _unitWork.ProcessWorkFlowMaster.Add(processWorkFlowMaster);
                        _unitWork.SaveChanges();
                        var processWorkFlowTran = new ProcessWorkFlowTran();
                        processWorkFlowTran.Wfmid = _tableWork.ProcessWorkFlowMasterRepository.GetLastRecord(s=>s.Id).Id;
                        processWorkFlowTran.Jid = jobOrder.Id;
                        processWorkFlowTran.StatusId = 1;
                        processWorkFlowTran.DepartmentId = jobOrder.DepartmentId;
                        processWorkFlowTran.PreviousProcessId = null;
                        processWorkFlowTran.ProcessId = 2;
                        processWorkFlowTran.JobStatusId = jobOrder.JobStatusId;
                        processWorkFlowTran.IsDeleted = processWorkFlowMaster.IsDeleted;
                        processWorkFlowTran.CreatedBy = jobOrder.CreatedBy;
                        processWorkFlowTran.CreatedUtc = DateTime.UtcNow;
                        processWorkFlowTran.IsActive = true;
                        _unitWork.ProcessWorkFlowTran.Add(processWorkFlowTran);
                        _unitWork.SaveChanges();
                        //
                        var customerShortName = _unitWork.Customer.Where(x => x.Id == item.ClientId).FirstOrDefault();
                        var dept = _unitWork.Department.Where(x => x.Id == jobOrder.DepartmentId).FirstOrDefault();
                        var jobStatus = _unitWork.JobStatus.Where(x => x.Id == item.JobStatusId).FirstOrDefault();
                        var ccIntake = _unitWork.Employee.Where(x => x.EmployeeId == jobOrder.CreatedBy).FirstOrDefault();
                        var empname = _unitWork.Employee.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();
                        var classiid = _unitWork.Customer.FirstOrDefault(x => x.Id == item.ClientId).CustomerClassificationId;
                        var classification = _unitWork.CustomerClassification.FirstOrDefault(x => x.Id == classiid).Description;
                        var lastrecord = _tableWork.JobOrderRepository.GetLastRecord(s => s.Id);
                        TrayDetailMovement trayDetailMovement = new TrayDetailMovement();
                        trayDetailMovement.JobDate = DateTime.UtcNow;
                        trayDetailMovement.FileName = item.FileName;
                        trayDetailMovement.ClientId = item.ClientId;
                        trayDetailMovement.ClientShortName = customerShortName.ShortName;
                        trayDetailMovement.Department = dept.Description;
                        trayDetailMovement.JobStatus = jobStatus.JobStatusDescription;
                        trayDetailMovement.CustomerJobType = lastrecord.CustomerJobType;
                        trayDetailMovement.CurrentTray = "Production Allocation";
                        trayDetailMovement.CurrentStatus = "Pending";
                        trayDetailMovement.CcintakenBy = ccIntake.EmployeeCode + "-" + ccIntake.EmployeeName;
                        trayDetailMovement.IsDeleted = false;
                        trayDetailMovement.CreatedBy = item.EmployeeId;
                        trayDetailMovement.CreatedUtc = DateTime.UtcNow;
                        trayDetailMovement.Jid = lastrecord.Id;
                        trayDetailMovement.JobId = lastrecord.JobId;
                        trayDetailMovement.DepartmentId = lastrecord.DepartmentId;
                        trayDetailMovement.EmployeeId = item.EmployeeId;
                        trayDetailMovement.EmployeeName = empname.EmployeeCode + "-" + empname.EmployeeName;
                        trayDetailMovement.StatusId = 1;
                        trayDetailMovement.ClassificationId = classiid;
                        trayDetailMovement.Classification = classification;
                        trayDetailMovement.CcintakenDate = DateTime.UtcNow;
                        _unitWork.TrayDetailMovement.Add(trayDetailMovement);
                        //

                        JobOrderExtension joe = new JobOrderExtension();
                        joe.Jid = lastrecord.Id;
                        joe.Date = DateTime.UtcNow;
                      
                        joe.Wftid = _tableWork.ProcessWorkFlowTranRepository.GetLastRecord(s=>s.Id).Id;
                        _unitWork.JobOrderExtension.Add(joe);

                        voucherControl.Voucherno = voucherNo;
                        voucherControl.UpdatedBy = jobOrder.CreatedBy;
                        voucherControl.UpdatedUtc = DateTime.UtcNow;
                        //_db.Entry(voucherControl).State = EntityState.Modified;
                        _unitWork.VoucherControl.Update(voucherControl);

                        int year = jobOrder.FileReceivedDate.Year;
                        var Getjobsummary = _unitWork.JobSummary.Where(x => x.ClientId == item.ClientId && x.MonthYear == month + "-" + year).ToList();
                        var GetLastRecord = Getjobsummary.LastOrDefault();
                        if (Getjobsummary.Count == 0)
                        {
                            var savejobsummary = new JobSummary();
                            savejobsummary.ClientId = item.ClientId.Value;
                            savejobsummary.MonthYear = month + "-" + year;
                            if (GetLastRecord == null)
                            {
                                savejobsummary.Pending = 1;
                                savejobsummary.TotalFiles = 1;
                            }
                            _unitWork.JobSummary.Add(savejobsummary);
                        }
                        else
                        {
                            var Updatejobsummary = _unitWork.JobSummary.FirstOrDefault(x => x.Id == GetLastRecord.Id);
                            Updatejobsummary.Pending = GetLastRecord.Pending + 1;
                            Updatejobsummary.TotalFiles = GetLastRecord.TotalFiles + 1;
                            //_db.Entry(Updatejobsummary).State = EntityState.Modified;
                            _unitWork.JobSummary.Update(Updatejobsummary);
                        }
                        _unitWork.SaveChanges();
                        var jobOrder1 = _unitWork.JobOrder.Local().OrderByDescending(x => x.Id).FirstOrDefault();
                        var pwmId = _unitWork.ProcessWorkFlowMaster.Where(x => x.JobId == jobOrder1.Id).Select(x => x.Id).FirstOrDefault();
                        var pwtLast = _unitWork.ProcessWorkFlowTran.FirstOrDefault(x => x.Wfmid == pwmId);
                        fileMovement.OrderId = jobOrder1.Id;
                        fileMovement.ProcessId = pwtLast.ProcessId;
                        fileMovement.StatusId = pwtLast.StatusId;
                        fileMovement.JobId = jobOrder1.JobId;
                        fileMovement.IsProcessWorkFlowTranInserted = 1;
                    }
                }
            }
            //_unitWork.SaveChanges();
            return fileMovement;
        }
    }
}

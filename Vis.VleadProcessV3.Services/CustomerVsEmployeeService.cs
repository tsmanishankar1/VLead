using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Security.Policy;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class CustomerVsEmployeeService
    {
        private readonly UnitWork _unitwork;
        private readonly ViewWork _viewwork;
        private readonly TableWork _tablework;
        public CustomerVsEmployeeService(IConfiguration configuration,UnitWork unitWork,ViewWork viewWork,
            TableWork tableWork)
        {
            _unitwork = unitWork;
            _tablework = tableWork;
            _viewwork = viewWork;
        }
        public Object GetAllDropDownList()
        {
            var customerclass = _tablework.CustomerClassificationRepository.Get(x => x.IsDeleted == false).ToArray();
            var getemployee = _tablework.EmployeeRepository.Get(x => x.IsDeleted == false && (x.DateOfResignation == null || x.ResignReasons == null)).OrderBy(x => x.EmployeeName).AsQueryable();
            var customers = _tablework.CustomerRepository.Get(x => x.IsDeleted == false && x.IsBlacklisted == false).Select(x => new { Id = x.Id, ShortName = x.ShortName, Name = x.Name }).ToArray();

            var DetailList = new
            {
                ClassificationList = customerclass,
                EmployeeList = getemployee,
                CustomerList = customers,
            };
            return DetailList;
        }
        public bool CreateCustomerVsEmployee(CustomerVsEmployeeViewModel customervsemp)
        {
            bool status = false;
            if (customervsemp != null)
            {
                try
                {
                    var customerids = customervsemp.CustomerId;
                    foreach (var item in customerids)
                    {
                        var CheckAlreadyExist = _viewwork.ViewCustomerVsEmployeeRepository.Get(x => x.CustomerId == item && x.EmployeeId == customervsemp.EmployeeId && x.IsDeleted == false).ToList();
                        var IsProofNeed = _tablework.ProcessWorkFlowRepository.Get(x => x.CustomerId == item && x.StatusId == 2 && x.CurrentProcessId == 5).FirstOrDefault();
                        if (CheckAlreadyExist.Count == 0)
                        {
                            CustomerVsEmployee savecustomervsemp = new CustomerVsEmployee();
                            savecustomervsemp.CustomerId = item;
                            savecustomervsemp.EmployeeId = customervsemp.EmployeeId;
                            savecustomervsemp.IsDeleted = false;
                            savecustomervsemp.CurrentProcessId = 3;
                            
                            if (IsProofNeed == null)
                            {
                                savecustomervsemp.NextProcessId = 1;
                            }
                            else
                            {
                                savecustomervsemp.NextProcessId = 6;
                            }
                            savecustomervsemp.CreatedBy = customervsemp.CreatedBy;
                            savecustomervsemp.CreatedUtc = DateTime.UtcNow;
                            _tablework.CustomerVsEmployeeRepository.Insert(savecustomervsemp);
                            long dbstatus = _tablework.SaveChanges();
                            status = dbstatus > 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //return new
                    //{
                    //    Status = false,
                    //    Message = ex.Message,
                    //    StackTrace = ex.StackTrace
                    //};
                }
            }
            return status;
        }
        public IEnumerable<ViewCustomerVsEmployee> GetAllCustomerVsEmployee()
        {
            return _viewwork.ViewCustomerVsEmployeeRepository.Get(x => x.IsAgmapproved == true && x.IsDeleted == false).OrderByDescending(x => x.Id).ToList();
        }

        //Norms
        public IEnumerable<ViewCustomerNorm> GetAllCustomerNorms() //View_CustomerNorms
        {

            return _viewwork.ViewCustomerNormsRepository.Get(x => x.IsDeleted == false).OrderByDescending(x => x.Id).ToList();
        }

        public IEnumerable<ViewCustomerNorm> GetCustomerNormsById(int Id)
        {
            return _viewwork.ViewCustomerNormsRepository.Get(x => x.Id == Id);
        }

        //Norms
        public IEnumerable<Customer> GetcustomerByClassId(int CustomerClassificationId)
        {
            return _tablework.CustomerRepository.Get(x => x.CustomerClassificationId == CustomerClassificationId && x.IsBlacklisted == false && x.IsDeleted == false).ToList();
        }
        public bool UpdateCustomerVsEmployee(CustomerVsEmployeeViewModel customerEmp)
        {
            bool status = false;
            try
            {
                var customerids = customerEmp.CustomerId;
                foreach (var item in customerids)
                {
                    var existingCustomerVsEmployeeExist = _viewwork.ViewCustomerVsEmployeeRepository.Get(x => x.EmployeeId == customerEmp.EmployeeId && x.CustomerId == item && x.ClassId == customerEmp.ClassId && x.IsDeleted == false).ToList();
                    if (existingCustomerVsEmployeeExist.Count == 0)
                    {
                        var existingCustomerExt = _unitwork.CustomerVsEmployee.FirstOrDefault(x => x.Id == customerEmp.Id);
                        existingCustomerExt.CustomerId = item;
                        existingCustomerExt.EmployeeId = customerEmp.EmployeeId;
                        existingCustomerExt.IsDeleted = customerEmp.IsDeleted;
                        existingCustomerExt.UpdatedBy = customerEmp.UpdatedBy;
                        existingCustomerExt.UpdatedUtc = DateTime.UtcNow;
                        _unitwork.CustomerVsEmployee.Update(existingCustomerExt);
                        _unitwork.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return status;
        }
        public IEnumerable<ViewCustomerVsEmployee> DeleteCustomerVsEmpById(CustomerVsEmployee1 customerVsemp)
        {
            var existingCustomerVsEmp = _tablework.CustomerVsEmployeeRepository.GetSingle(x => x.Id == customerVsemp.Id);
            existingCustomerVsEmp.IsDeleted = true;
            _tablework.CustomerVsEmployeeRepository.Update(existingCustomerVsEmp);
            _tablework.SaveChanges();
            var LastestList = _viewwork.ViewCustomerVsEmployeeRepository.Get(x => x.IsDeleted == false).OrderBy(x => x.Id).ToList();
            return LastestList;
        }
        //Norms

        public IEnumerable<ViewCustomerNorm> DeleteCustomerNormById(CustomersNorm1 customersnorm)
        {
            var existingCustomerNorm = _tablework.CustomersNormsRepository.GetSingle(x => x.Id == customersnorm.Id);
            existingCustomerNorm.IsDeleted = true;
            existingCustomerNorm.UpdatedBy = customersnorm.UpdatedBy;
            existingCustomerNorm.UpdatedUtc = customersnorm.UpdatedUtc;
            _tablework.CustomersNormsRepository.Update(existingCustomerNorm);
            _tablework.SaveChanges();
            var LastestList = _viewwork.ViewCustomerNormsRepository.Get(x => x.IsDeleted == false).OrderByDescending(x => x.Id).ToList();
            return LastestList;
        }

        public IEnumerable<ViewCustomerNorm> UpdateCustomerNorms(CustomersNorm customersnorm)
        {
            var existingCustomerNorm = _tablework.CustomersNormsRepository.GetSingle(x => x.Id == customersnorm.Id);
            existingCustomerNorm.Norms = customersnorm.Norms;
            existingCustomerNorm.UpdatedBy = customersnorm.UpdatedBy;
            existingCustomerNorm.UpdatedUtc = customersnorm.UpdatedUtc;
            _tablework.CustomersNormsRepository.Update(existingCustomerNorm);
            _tablework.SaveChanges();
            var LastestList = _viewwork.ViewCustomerNormsRepository.Get(x => x.IsDeleted == false).OrderByDescending(x => x.Id).ToList();
            return LastestList;
        }

        //Norms
        public ViewCustomerVsEmployee GetCustomerVsEmployee(int Id)
        {
            return _viewwork.ViewCustomerVsEmployeeRepository.GetSingle(x => x.Id == Id);
        }
        public IEnumerable<ViewCustomerVsEmployee> GetCustomerVsEmpByEmpId(ViewCustomerVsEmployee1 EmployeeId)
        {
            return _viewwork.ViewCustomerVsEmployeeRepository.Get(x => x.EmployeeId == EmployeeId.EmployeeId).AsQueryable();
        }
        public List<CustomerVsEmployeeResponse> GetAGMApprovedNull()
        {
            var approvedList = _unitwork.CustomerVsEmployee
                .Where(cve => (cve.IsAgmapproved == null) &&
                              (cve.IsAgmrejected == null) && cve.IsDeleted == false)
                .Select(cve => new CustomerVsEmployeeResponse
                {
                    CustomerName = cve.Customer.Name,
                    EmployeeCode = cve.Employee.EmployeeCode,
                    CustomerShortName = cve.Customer.ShortName
                })
                .ToList();

            return approvedList;
        }

        public string ApprovedAGM(AgmApproval agmApproval)
        {
            var message = "";
            var selectedRows = agmApproval.SelectedRows;
            foreach (var item in selectedRows)
            {
                if (item.ApprovedStatus == "Approved")
                {
                    message = "Agm approved successfully.";

                    var agmApprove = _unitwork.CustomerVsEmployee.FirstOrDefault(c => c.Customer.Name == item.CustomerName && c.Customer.ShortName == item.CustomerShortName && c.Employee.EmployeeCode == item.EmployeeCode && c.IsAgmapproved == null && c.IsDeleted == false);
                    if (agmApprove != null)
                    {
                        agmApprove.IsAgmapproved = true;
                        agmApprove.ApprovedBy = item.ApprovedBy;
                        agmApprove.ApprovedUtc = DateTime.UtcNow;
                        agmApprove.RejectedBy = item.RejectedBy;
                        agmApprove.RejectedUtc = DateTime.UtcNow;
                        agmApprove.IsAgmrejected = false;
                        _unitwork.SaveChanges();
                    }
                    else
                    {
                        message = "Agm is already approved";
                    }
                }
                if (item.ApprovedStatus == "Rejected")
                {
                    message = "Agm rejected successfully.";

                    var agmReject = _unitwork.CustomerVsEmployee.FirstOrDefault(c => c.Customer.Name == item.CustomerName && c.Customer.ShortName == item.CustomerShortName && c.Employee.EmployeeCode == item.EmployeeCode && c.IsAgmrejected == null && c.IsDeleted == false);
                    if (agmReject != null)
                    {
                        agmReject.IsAgmrejected = true;
                        agmReject.RejectedBy = item.RejectedBy;
                        agmReject.RejectedUtc = DateTime.UtcNow;
                        agmReject.IsAgmapproved = false;
                        agmReject.ApprovedBy = item.ApprovedBy;
                        agmReject.ApprovedUtc = DateTime.UtcNow;
                        _unitwork.SaveChanges();
                    }
                    else
                    {
                        message = "Agm is already rejected";
                    }
                }
            }
            return message;
        }
    }
}

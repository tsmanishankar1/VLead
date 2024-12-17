using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class CustomerMappingService
    {
        private readonly TableWork _tableWork; 
        private readonly UnitWork _unitWork;
        private readonly ProcedureWork _procedureWork;
       
        private readonly ApplicationDbContext db;
        public CustomerMappingService(TableWork tableWork, UnitWork unitWork, ProcedureWork procedureWork, ApplicationDbContext db)
        {
            
            _tableWork = tableWork;
            _unitWork = unitWork;
            _procedureWork = procedureWork;
            this.db = db;
        }
        public Object GetSalesEmployees()
        {
            var employees = new List<GetEmployee>();
            var getemployee = _tableWork.EmployeeRepository.Get(x => x.IsDeleted == false && x.DepartmentId == 9 && (x.DateOfResignation == null || x.ResignReasons == null)).OrderBy(x => x.EmployeeName).AsQueryable();
            foreach (var item in getemployee)
            {
                string employeeNameWithCode = item.EmployeeCode + " - " + item.EmployeeName;
                employees.Add(new GetEmployee
                {
                    EmployeenameWithCode = employeeNameWithCode,
                });
            }
            return employees;
        }
        public Object GetAllDropDownList()
        {
            var getemployee = _tableWork.EmployeeRepository.Get(x => x.IsDeleted == false && x.DepartmentId == 9 && (x.DateOfResignation == null || x.ResignReasons == null)).OrderBy(x => x.EmployeeName).AsQueryable();
            var customers = _tableWork.CustomerRepository.Get(x => x.IsDeleted == false && x.IsBlacklisted == false).ToArray();
            var Department = _tableWork.DepartmentRepository.Get(x => (x.IsDeleted == false) && (x.Description.Contains("Artwork") || x.Description.Contains("Digi"))).AsQueryable();
            var JobStatus = _tableWork.JobStatusRepository.Get(x => x.IsActive == true).AsQueryable();
            var DetailList = new
            {
                DepartmentList = Department,
                EmployeeList = getemployee,
                CustomerList = customers,
                JobStatusList = JobStatus,
            };
            return DetailList;
        }

        public Object GetScopeByDeptCustId(int deptId, int customerId)
        {

            var sqlpara = new SqlParameter[]
                    {
                        new SqlParameter("@DeptId", deptId),
                        new SqlParameter("@ClientId", customerId),
                    };
            return _procedureWork.ExecStoredProcedure<GetScopeByDeptIdCustId_Result>("GetScopeByDeptIdCustId @DeptId,@ClientId", sqlpara);
        }

        public Object GetCusScopeByCusDepIds(int DeptId, int CustomerId) //scopechange ui
        {
         
            var getCusScope = (from cs in db.CustomerVsScopes
                               join sc in db.Scopes on cs.ScopeId equals sc.Id
                               where (sc.DepartmentId == DeptId && sc.IsDeleted == false && cs.CustomerId == CustomerId && cs.IsActive == true)
                               select new { sc.Description, sc.Id, sc.DepartmentId }).ToList();
            return getCusScope;
        }

        public string ChangeScopePanel(ScopeChangeVM ChangeScope)
        {
            string message = "Scope has been updated successfully";
          
            var selectedData = ChangeScope.ChangeScope;
            var jid = selectedData.Select(x => x.JId).ToList();
            var upjoborder = db.JobOrders.Where(x => jid.Contains(x.Id)).ToList();
            var uppwt = db.ProcessWorkFlowMasters.Where(x => jid.Contains(x.JobId)).ToList();
            upjoborder.ForEach(x => x.ScopeId = ChangeScope.ScopeId);
            upjoborder.ForEach(x => x.SpecialPrice = null);
            upjoborder.ForEach(x => x.PricingTypeId = null);
            uppwt.ForEach(x => x.ScopeId = ChangeScope.ScopeId);
            db.SaveChanges();
            return message;
        }
        public string ChangeStitchCount(StitchCountChangeVM changeStitch)
        {
            string message = "Stitch Count has been updated succesfully";

            var selectedData = changeStitch.ChangeStitchCount;
            var jid = selectedData.Select(x => x.JId).ToList();
            var upjoborder = db.JobOrders.Where(x => jid.Contains(x.Id)).ToList();
            var uppwt = db.ProcessWorkFlowMasters.Where(x => jid.Contains(x.JobId)).ToList();
            upjoborder.ForEach(x => x.StitchCount = changeStitch.StitchCount);
            upjoborder.ForEach(x => x.SpecialPrice = null);
            upjoborder.ForEach(x => x.PricingTypeId = null);
            uppwt.ForEach(x => x.StitchCount = changeStitch.StitchCount);
            db.SaveChanges();
            return message;
        }
        public string SetSpecialPricing(SetSpecialPricingVM specialPrice)
        {
            string message = "Special price has been updated succesfully";

            var selectedData = specialPrice.SpecialPricing;
            var jid = selectedData.Select(x => x.JId).ToList();
            var upjoborder = db.JobOrders.Where(x => jid.Contains(x.Id)).ToList();
            upjoborder.ForEach(x => x.SpecialPrice = specialPrice.SpecialPrice);
            upjoborder.ForEach(x => x.IsSpecialPrice = true);
            if (specialPrice.DepartmentId == 1)
            {
                upjoborder.ForEach(x => x.PricingTypeId = 2);
            }
            else if (specialPrice.DepartmentId == 2)
            {
                upjoborder.ForEach(x => x.PricingTypeId = 6);
            }
            db.SaveChanges();
            return message;
        }

        public bool RemoveCustomerScope(int CustomerScopeId)
        {
            bool status = false;
            try
            {
                var deleteCustomerScope = _unitWork.CustomerVsScope.FirstOrDefault(x => x.Id == CustomerScopeId);
                deleteCustomerScope.IsDeleted = true;
                deleteCustomerScope.IsActive = false;
                deleteCustomerScope.UpdatedUtc = DateTime.UtcNow;

                //_db.Entry(deleteCustomerScope).State = EntityState.Modified;
                _unitWork.CustomerVsScope.Update(deleteCustomerScope);
                // _db.SaveChanges();
                long dbstatus = _unitWork.Save();
                status = dbstatus > 0;
                //status = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public object UpdateCustomerVsScopeDetails(CustomerVsScope2 customerScopes)
        {
            bool status = false;
            try
            {
                var UpdateCustomerScope = _unitWork.CustomerVsScope.FirstOrDefault(x => x.Id == customerScopes.Id);
                UpdateCustomerScope.CustomerJobType = customerScopes.CustomerJobType;
                UpdateCustomerScope.UpdatedUtc = DateTime.UtcNow;
                UpdateCustomerScope.UpdatedBy = customerScopes.UpdatedBy;
                //_db.Entry(UpdateCustomerScope).State = EntityState.Modified;                
                //long dbstatus = _db.SaveChanges();
                _unitWork.CustomerVsScope.Update(UpdateCustomerScope);
                long dbstatus = _unitWork.Save();
                status = dbstatus > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        //public IEnumerable<CustomerVsScope> GetAllCustomerScopeByCusId(int CustomerId)
        public Object GetAllCustomerScopeByCusId(int CustomerId)
        {
            // return _unitWork.CustomerVsScope.Include("Customer").Where(x => x.CustomerId == CustomerId && x.IsActive == true && x.IsDeleted == false);
            //var CusScopedetails = _tableWork.CustomerVsScopeRepository.GetAllVal(x => x.Customer).Where(x => x.CustomerId == CustomerId && x.IsActive == true && x.IsDeleted == false);
            //return CusScopedetails;
            var CVSList = (from CVS in _tableWork.CustomerVsScopeRepository.Get()
                           join cus in _tableWork.CustomerRepository.Get() on CVS.CustomerId equals cus.Id
                           where cus.IsDeleted == false && CVS.IsActive == true && CVS.CustomerId == CustomerId
                           select new
                           {
                               CustomerName = cus.Name,
                               ShortName = cus.ShortName,
                               DeptName = CVS.DeptName,
                               ScopeName = CVS.ScopeName,
                               CustomerJobType = CVS.CustomerJobType,
                               Id = CVS.Id
                           }).OrderByDescending(x => x.Id).ToList();
            return CVSList;
            //
        }


        public object AddCustomerVsScopeDetails(List<CustomerVsScopeViewModel> customerScopes)
        {
            bool status = false;
            var result = new
            {
                Success = false,
                Message = ""
            };

            if (customerScopes != null)
            {
                try
                {
                    foreach (CustomerVsScopeViewModel item in customerScopes)
                    {
                        CustomerVsScope _Cvs = new CustomerVsScope();
                        _Cvs.CustomerId = item.CustId;
                        _Cvs.ScopeId = item.ScopeId;
                        _Cvs.DeptId = item.DeptId;
                        _Cvs.CustomerName = item.CustName;
                        _Cvs.ScopeName = item.ScopeName;
                        _Cvs.DeptName = item.DeptName;
                        _Cvs.CustomerJobType = item.CustJobType;

                        if (item.IsEstimatedTime && CheckIsEstimatedTime(item.CustId))
                        {
                            _Cvs.EstimatedTime = item.EstimatedTime;

                            _Cvs.EffectiveFromDate = DateTime.UtcNow;

                            var existingRecord = _unitWork.CustomerVsScope
                                .Where(c => c.CustomerId == item.CustId && c.ScopeId == item.ScopeId && c.IsActive == true)
                                .FirstOrDefault();

                            if (existingRecord != null)
                            {
                                existingRecord.EffectiveToDate = _Cvs.EffectiveFromDate.Value.AddDays(-1);
                                existingRecord.IsActive = false;
                                existingRecord.IsDeleted = true;
                            }
                        }

                        _Cvs.CreatedUtc = DateTime.UtcNow;
                        _Cvs.CreatedBy = item.CreatedBy;
                        _Cvs.IsDeleted = item.IsDeleted;
                        _Cvs.IsActive = item.IsActive;
                        _unitWork.CustomerVsScope.Add(_Cvs);
                    }

                    long dbstatus = _unitWork.Save();
                    status = dbstatus > 0;

                    result = new
                    {
                        Success = true,
                        Message = "Scope assigned to customer successfully"
                    };
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return result;
        }

        private bool CheckIsEstimatedTime(int customerId)
        {
            var customer = _tableWork.CustomerRepository.Get(x => x.Id == customerId && x.IsDeleted == false).FirstOrDefault();
            return customer != null && customer.IsEstimatedTime == true;
        }

        public object GetAllCustomerTATByCustId(int CustomerId)
        {
            var sqlpara = new SqlParameter[]
                    {

                        new SqlParameter("@ClientId", CustomerId),
                    };
            return _procedureWork.ExecStoredProcedure<GetCustomerTATByCustId_Result>("GetCustomerTATByCustId @ClientId", sqlpara);

        }

    

        public object CreateCustomerVsSalesEmployee(customerVsSalesMapping CustomerVsSalesEmp)
        {
            bool status = false;
            var result = new
            {
                Success = false,
                Message = ""
            };
            if (CustomerVsSalesEmp != null)
            {
                try
                {
                    var selectedCustomers = CustomerVsSalesEmp.selectedCustomers;
                    var SelectedEmployee = CustomerVsSalesEmp.selectedEmployee.FirstOrDefault();
                    foreach (var Cust in selectedCustomers)
                    {
                        var activeExistForOther = _unitWork.CustomerVsSalesEmployee
                                               .FirstOrDefault(x => x.CustomerId == Cust.Id && x.EmployeeId != SelectedEmployee.EmployeeId && x.IsActive == true);

                        var AlreadyExistCustomerVsEmployee = _unitWork.CustomerVsSalesEmployee
                                              .FirstOrDefault(x => x.CustomerId == Cust.Id && x.EmployeeId == SelectedEmployee.EmployeeId && x.IsActive == true);

                        if (activeExistForOther != null)
                        {
                            activeExistForOther.IsActive = false;
                            activeExistForOther.UpdatedBy = CustomerVsSalesEmp.CreatedBy;
                            activeExistForOther.UpdatedUtc = DateTime.UtcNow;
                            //_db.SaveChanges();
                            //_db.Entry(activeExistForOther).State = EntityState.Modified;
                            _unitWork.CustomerVsSalesEmployee.Update(activeExistForOther);
                        }

                        if (AlreadyExistCustomerVsEmployee == null)
                        {
                            CustomerVsSalesEmployee savecustomervsSalesemp = new CustomerVsSalesEmployee();
                            savecustomervsSalesemp.CustomerId = Cust.Id;
                            savecustomervsSalesemp.EmployeeId = SelectedEmployee.EmployeeId;
                            savecustomervsSalesemp.EmployeeName = SelectedEmployee.EmployeeName;
                            savecustomervsSalesemp.CustomerName = Cust.Name;
                            savecustomervsSalesemp.CreatedUtc = DateTime.UtcNow;
                            savecustomervsSalesemp.CreatedBy = CustomerVsSalesEmp.CreatedBy;
                            savecustomervsSalesemp.IsDeleted = false;
                            savecustomervsSalesemp.IsActive = true;
                            _unitWork.CustomerVsSalesEmployee.Add(savecustomervsSalesemp);
                            long dbstatus = _unitWork.Save();
                            status = dbstatus > 0;
                            if (status == true)
                            {
                                result = new
                                {
                                    Success = true,
                                    Message = "Salesperson assigned successfully"
                                };

                            }

                        }
                        else
                        {
                            result = new
                            {
                                Success = false,
                                Message = "Customer assigned to selected employee already"
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                   
                    throw ex;
                }
            }
            return result;
        }

        public Object GetJobStatusByCusId(int customerId)
        {

            var sqlpara = new SqlParameter[]
                    {

                        new SqlParameter("@ClientId", customerId)
                    };
            return _procedureWork.ExecStoredProcedure<GetJobStatusByCustId_Result>("GetJobStatusByCustId  @ClientId", sqlpara);

        }

        public object AddCustomerTATsDetails(List<CustomerTATViewModel> customervsTats)
        {
            bool status = false;
            var result = new
            {
                Success = false,
                Message = ""
            };
            if (customervsTats != null)
            {
                try
                {


                    foreach (CustomerTATViewModel item in customervsTats)
                    {
                        CustomerTat _JSE = new CustomerTat();
                        _JSE.CustomerId = item.CustomerId;
                        _JSE.JobStatusId = item.JobStatusId;

                        _JSE.JobStatusDescription = item.JobStatusDescription;
                        _JSE.CustomerShortName = item.CustomerShortName;
                        _JSE.Tat = item.TAT;
                        _JSE.CreatedBy = item.CreatedBy;
                        _JSE.CreatedUtc= DateTime.UtcNow;
                        _JSE.IsActive = item.IsActive;
                        //_db.CustomerTATs.Add(_JSE);
                        _unitWork.CustomerTAT.Add(_JSE);
                    }
                    long dbstatus = _unitWork.Save();
                    status = dbstatus > 0;
                    //status = true;
                    result = new
                    {
                        Success = true,
                        Message = "TAT assigned successfully"
                    };
                }
                catch (Exception ex)
                {
                    throw ex;

                }
            }
            //  return status == true ? "Success" : "There is a problem in Adding data. Please contact administrator";
            return result;
        }

        public object UpdateCustomerTATsDetails(CustomerTat UpdtcustomervsTat)
        {
            bool status = false;
            var result = new
            {
                Success = false,
                Message = ""
            };
            if (UpdtcustomervsTat != null)
            {
                try
                {
                    var UpdateCustomerTAT = _unitWork.CustomerTAT.FirstOrDefault(x => x.Id == UpdtcustomervsTat.Id);
                    UpdateCustomerTAT.Tat = UpdtcustomervsTat.Tat;
                    UpdateCustomerTAT.UpdatedUtc = DateTime.UtcNow;
                    UpdateCustomerTAT.UpdatedBy = UpdtcustomervsTat.UpdatedBy;
                    //_db.Entry(UpdateCustomerTAT).State = EntityState.Modified;
                    _unitWork.CustomerTAT.Update(UpdateCustomerTAT);
                    //_db.SaveChanges();
                    long dbstatus = _unitWork.Save();
                    status = dbstatus > 0;
                    //status = true;
                    result = new
                    {
                        Success = true,
                        Message = "TAT updated successfully"
                    };
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            return result;
        }

        public bool RemoveCustomerTAT(int CustomerTATId)
        {
            bool status = false;
            try
            {
                var deleteCustomerTAT = _unitWork.CustomerTAT.FirstOrDefault(x => x.Id == CustomerTATId);
                deleteCustomerTAT.IsActive = false;
                deleteCustomerTAT.UpdatedUtc = DateTime.UtcNow;
                //deleteCustomerTAT.UpdatedBy = deleteCustomerScope.;
                //_db.Entry(deleteCustomerTAT).State = EntityState.Modified;
                _unitWork.CustomerTAT.Update(deleteCustomerTAT);
                // _db.SaveChanges();
                long dbstatus = _unitWork.Save();
                status = dbstatus > 0;
                //status = true;
            }
            catch (Exception e)
            {
                throw;
            }
            return status;
        }

        public IEnumerable<Customer> GetcustomerByClassId(Customer1 CustomerClassificationId)
        {
            // return _tableWork.CustomerRepository.Get(x => x.CustomerClassificationId == CustomerClassificationId.CustomerClassificationId && x.IsBlacklisted == false && x.IsDeleted == false).ToList();
            return _unitWork.Customer.Where(x => x.CustomerClassificationId == CustomerClassificationId.CustomerClassificationId && x.IsBlacklisted == false && x.IsDeleted == false).ToList();
        }

        public Object GetAllCustomerEmployee(int Id)
        {
            //List<CustomerVsSalesEmpViewModel> ListCustomerVsSalesViewModel = new List<CustomerVsSalesEmpViewModel>();

            var sqlpara = new SqlParameter[]
                {

                        new SqlParameter("@Id", Id)
                };
            var AllCusEmpList = _procedureWork.ExecStoredProcedure<GetAllCustomerSalesEmployee_Result>("GetAllCustomerSalesEmployee @Id", sqlpara).ToList();
            return AllCusEmpList;
            // return _procedureWork.ExecStoredProcedure<GetAllCustomerSalesEmployee_Result>("GetAllCustomerSalesEmployee ,@Id", sqlpara);
            // var ListCustomerVsSalesViewModel  =  _procedureWork.ExecStoredProcedure<GetAllCustomerSalesEmployee_Result>("GetAllCustomerSalesEmployee ,@Id", sqlpara).ToList();

            //return ListCustomerVsSalesViewModel;
        }

    }
}

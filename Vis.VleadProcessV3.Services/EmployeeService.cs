using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class EmployeeService
    {

        private readonly TableWork _tableWork;
        private readonly TableWork tow;
        private readonly ViewWork _viewWork;
        private readonly ViewWork vow;
        private readonly ProcedureWork _procedureWork;
       
        public EmployeeService(TableWork tableWork,ViewWork viewWork,ProcedureWork procedureWork )
        {
          
            tow = tableWork;
            vow = viewWork;
            _tableWork = tableWork;
            _viewWork = viewWork;
            _procedureWork = procedureWork;
        }
            public Department GetDepartment(int departmentId)
            {
                return _tableWork.DepartmentRepository.GetSingle(x => x.Id == departmentId);
            }

            public IEnumerable<PricingWithScope> GetAllPricing()
            {
                //var pricings = _db.PricingWithScopes.Include("Customer").Include("Department").Include("PricingType").ToList();
                var pricings = _tableWork.PricingWithScopeRepository.GetAllVal(x => x.Customer, x => x.Department, x => x.PricingType).ToList();
                return pricings;
            }
            public IQueryable<Department> GetDepartments()
            {
                return _tableWork.DepartmentRepository.Get(x => x.Description == "Artwork" || x.Description == "Digitizing");
            }
            public IEnumerable<PricingType> GetPricingTypesByDepartmentId(int deptId)
            {
                var pricingTypes = _tableWork.PricingTypeRepository.Get(x => x.IsDeleted == false).ToList();

                if (deptId == 1)
                {
                    return pricingTypes.Where(x => x.Applicable == 1 || x.Applicable == 4).ToList();
                }
                else
                {
                    return pricingTypes.Where(x => x.Applicable == 3 || x.Applicable == 2).ToList();
                }
            }
            public Object GetScopeByDepartmentId(int deptId, int customerId)
            {
                return _viewWork.ViewCustomerVsScopeRepository.Get(x => x.DepartmentId == deptId && x.CustomerDepart == deptId && x.CustomerId == customerId).ToList();
            }
            public Object ScopeByDeptIdandPricingtypeId(int deptId, int clientid)
            {
                var sqlPara = new SqlParameter[]
                   {
                   new SqlParameter("@CustomerId",clientid),
                   new SqlParameter("@DepartmentId",deptId)
                   };
                var scopelist = _procedureWork.ExecStoredProcedure<ScopeforPricing_Result>("ScopeforPricing @CustomerId, @DepartmentId", sqlPara).Where(x => x.DepartmentId == deptId);
                return scopelist;
            }
            public IEnumerable<Designation> GetAllDesignations()
            {
                return _tableWork.DesignationRepository.Get(x => x.IsDeleted == false).OrderBy(x => x.Description);
            }
            public bool AddPricingFileRangeOfDigitizing(Pricing pricing, List<PricingFileRange> viewModelpricingFileRanges)
            {
                bool status = false;
                var pricings = new Pricing();
                pricings.DepartmentId = pricing.DepartmentId;
                pricings.PricingTypeId = pricing.PricingTypeId;
                pricings.CustomerId = pricing.CustomerId;
                _tableWork.PricingRepository.Insert(pricings);
                _tableWork.SaveChanges();
                foreach (var fileRange in viewModelpricingFileRanges)
                {
                    var pricingFileRange = new PricingFileRange();
                    pricingFileRange.PricingId = pricings.Id;
                    pricingFileRange.FromRange = fileRange.FromRange;
                    pricingFileRange.ToRange = fileRange.ToRange;
                    pricingFileRange.ThousandStitchCharge = fileRange.ThousandStitchCharge;
                    _tableWork.PricingFileRangeRepository.Insert(pricingFileRange);
                    _tableWork.SaveChanges();
                    long dbStatus = _tableWork.SaveChanges();
                    status = dbStatus > 0;
                }
                return status;
            }
            public bool AddPricingStitchCount(List<Pricing> ViewModelpricing)
            {
                bool status = false;
                foreach (var pricingStitchCounts in ViewModelpricing)
                {
                    var pricingStitchCount = new Pricing();
                    pricingStitchCount.DepartmentId = pricingStitchCounts.DepartmentId;
                    pricingStitchCount.PricingTypeId = pricingStitchCounts.PricingTypeId;
                    pricingStitchCount.CustomerId = pricingStitchCounts.CustomerId;
                    pricingStitchCount.ThousandStitchCharge = pricingStitchCounts.ThousandStitchCharge;
                    pricingStitchCount.MaxPricingLimit = pricingStitchCounts.MaxPricingLimit;
                    pricingStitchCount.MoreStitchCount = pricingStitchCounts.MoreStitchCount;
                    pricingStitchCount.FlatRate = pricingStitchCounts.FlatRate;
                    _tableWork.PricingRepository.Insert(pricingStitchCount);
                    _tableWork.SaveChanges();
                    long dbStatus = _tableWork.SaveChanges();
                    status = dbStatus > 0;
                }
                return status;
            }
            public bool AddPricingStaffing(Pricing pricing, List<PricingStaffing> viewModelPricingStaffings)
            {
                bool status = false;
                var pricings = new Pricing
                {
                    DepartmentId = pricing.DepartmentId,
                    PricingTypeId = pricing.PricingTypeId,
                    CustomerId = pricing.CustomerId
                };
                _tableWork.PricingRepository.Insert(pricings);
                _tableWork.SaveChanges();
                foreach (var pricingStaffing in viewModelPricingStaffings)
                {
                    var pricingStaffings = new PricingStaffing
                    {
                        PricingId = pricings.Id,
                        DesignationId = pricingStaffing.DesignationId,
                        RatePerMonth = pricingStaffing.RatePerMonth
                    };
                    _tableWork.PricingStaffingRepository.Insert(pricingStaffings);
                    _tableWork.SaveChanges();
                    long dbStatus = _tableWork.SaveChanges();
                    status = dbStatus > 0;
                }
                return status;

            }
            public bool AddPricingScope(Pricing pricing, List<PricingScope> viewModelpricingScopes)
            {
                bool status = false;
                var pricings = new Pricing();
                pricings.DepartmentId = pricing.DepartmentId;
                pricings.PricingTypeId = pricing.PricingTypeId;
                pricings.CustomerId = pricing.CustomerId;
                _tableWork.PricingRepository.Insert(pricings);
                _tableWork.SaveChanges();
                foreach (var pricingScope in viewModelpricingScopes)
                {
                    var pricingScopes = new PricingScope();
                    pricingScopes.PricingId = pricings.Id;
                    pricingScopes.ScopeId = pricingScope.ScopeId;
                    pricingScopes.EstimatedTime = pricingScope.EstimatedTime;
                    pricingScopes.RatePerHour = pricingScope.RatePerHour;
                    pricingScopes.RateperFile = pricingScope.RateperFile;
                    pricingScopes.FromRange = pricingScope.FromRange;
                    pricingScopes.ToRange = pricingScope.ToRange;
                    pricingScopes.Price = pricingScope.Price;
                    pricingScopes.MaxPrice = pricingScope.MaxPrice;
                    pricingScopes.ModeOfPricing = pricingScope.ModeOfPricing;
                    _tableWork.PricingScopeRepository.Insert(pricingScopes);
                    _tableWork.SaveChanges();
                    long dbStatus = _tableWork.SaveChanges();
                    status = dbStatus > 0;
                }
                return status;
            }
            public PricingScope AddPricingScope(PricingScope pricingScope)
            {
                bool status = false;
                try
                {
                    if (pricingScope != null)
                    {
                        _tableWork.PricingScopeRepository.Insert(pricingScope);
                        long dbStatus = _tableWork.SaveChanges();
                        status = dbStatus > 0;
                    }
                }
                catch (Exception e)
                {
                }
                return pricingScope;
            }
            public IEnumerable<PricingScope> GetPricingScopeByPricingId(int pricingId)
            {
                var pricingScopes = _tableWork.PricingScopeRepository.GetAllVal(x => x.Scope).Where(x => x.PricingId == pricingId).ToList();
                return pricingScopes;
            }
            public IEnumerable<PricingStaffing> GetPricingStaffingByPricingId(int pricingId)
            {
                var pricingStaffing = _tableWork.PricingStaffingRepository.GetAllVal(x => x.Designation).Where(x => x.PricingId == pricingId).ToList();
                return pricingStaffing;
            }
            public IEnumerable<Pricing> GetPricingStitchCountByPricingId(int pricingId)
            {
                var pricing = _tableWork.PricingRepository.GetAllVal(x => x.Customer).Where(x => x.Id == pricingId).ToList();
                return pricing;
            }
            public IEnumerable<PricingFileRange> GetPricingFileRangeByPricingId(int pricingId)
            {
                var pricingFileRanges = _tableWork.PricingFileRangeRepository.Get(x => x.PricingId == pricingId).ToList();
                return pricingFileRanges;
            }
            public bool DeleteFileRange(IEnumerable<int> fileRangeIds)
            {
                bool status = false;
                try
                {
                    var pricingFileRanges = _tableWork.PricingFileRangeRepository.Get(x => !fileRangeIds.Contains(x.Id));
                    foreach (var pricingFileRange in pricingFileRanges)
                    {
                        _tableWork.PricingFileRangeRepository.Update(pricingFileRange);
                    }
                    long dbstatus = _tableWork.SaveChanges();

                }
                catch (Exception e)
                {
                    throw;
                }
                return status;
            }
            public PricingFileRange AddPricingFileRange(PricingFileRange pricingFileRange)
            {
                bool status = false;
                try
                {
                    if (pricingFileRange != null)
                    {
                        _tableWork.PricingFileRangeRepository.Insert(pricingFileRange);
                        long dbStatus = _tableWork.SaveChanges();
                        status = dbStatus > 0;
                    }
                }
                catch (Exception e)
                {
                }
                return pricingFileRange;
            }
            public bool UpdatePricingStitchCount(Pricing pricing)
            {
                bool status = false;
                try
                {
                    var existingPricing = _tableWork.PricingRepository.GetSingle(x => x.Id == pricing.Id);
                    existingPricing.CustomerId = pricing.CustomerId;
                    existingPricing.DepartmentId = pricing.DepartmentId;
                    existingPricing.PricingTypeId = pricing.PricingTypeId;
                    existingPricing.ThousandStitchCharge = pricing.ThousandStitchCharge;
                    existingPricing.MaxPricingLimit = pricing.MaxPricingLimit;
                    existingPricing.MoreStitchCount = pricing.MoreStitchCount;
                    existingPricing.FlatRate = pricing.FlatRate;

                    _tableWork.PricingRepository.Update(existingPricing);
                    long dbstatus = _tableWork.SaveChanges();
                    status = dbstatus > 0;
                }
                catch (Exception e)
                {
                    throw;
                }
                return status;
            }
            public bool DeletePricingScopes(IEnumerable<int> existingPricingScopeIds, IEnumerable<int> existingPricingIds)
            {
                bool status = false;
                try
                {
                    var pricingScopes = _tableWork.PricingScopeRepository.Get(x => !existingPricingScopeIds.Contains(x.Id) && existingPricingIds.Contains(x.PricingId) && x.FromRange != null && x.ToRange != null);

                    foreach (var pricingScope in pricingScopes)
                    {
                        _tableWork.PricingScopeRepository.Delete(pricingScope);
                    }
                    long dbstatus = _tableWork.SaveChanges();
                }
                catch (Exception e)
                {
                    throw;
                }
                return status;
            }
            public PricingScope GetPricingScopeById(int id)
            {
                var pricingScope = _tableWork.PricingScopeRepository.GetSingle(x => x.Id == id);
                return pricingScope;
            }
            public bool UpdatePricingScope(PricingScope pricingScope)
            {
                try
                {
                    _tableWork.PricingScopeRepository.Update(pricingScope);
                    _tableWork.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            public bool DeleteStaffing(IEnumerable<int> staffIds)
            {
                bool status = false;
                try
                {
                    var pricingStaffings = _tableWork.PricingStaffingRepository.Get(x => !staffIds.Contains(x.Id));
                    foreach (var pricingStaffing in pricingStaffings)
                    {
                        _tableWork.PricingStaffingRepository.Delete(pricingStaffings);
                    }
                    long dbstatus = _tableWork.SaveChanges();
                }
                catch (Exception e)
                {
                    throw;
                }
                return status;
            }
            public PricingStaffing AddPricingStaff(PricingStaffing pricingStaffing)
            {
                bool status = false;
                try
                {
                    if (pricingStaffing != null)
                    {
                        _tableWork.PricingStaffingRepository.Insert(pricingStaffing);
                        long dbStatus = _tableWork.SaveChanges();
                        status = dbStatus > 0;
                    }
                }
                catch (Exception e)
                {
                }
                return pricingStaffing;
            }
            public bool RemovePricing(int pricingId)
            {
                bool status = false;
                try
                {
                    var deletePricing = _tableWork.PricingWithScopeRepository.GetSingle(x => x.Id == pricingId);
                    _tableWork.PricingWithScopeRepository.Delete(deletePricing);
                    long dbstatus = _tableWork.SaveChanges();
                    status = dbstatus > 0;
                }
                catch (Exception e)
                {
                    throw;
                }
                return status;
            }
            public Pricing GetPricingTypeAndCustomerByPricingId(int pricingId)
            {
                var pricing = _tableWork.PricingRepository.GetAllVal(x => x.Customer, x => x.PricingType, x => x.Department).FirstOrDefault(x => x.Id == pricingId);
                return pricing;
            }
            public bool CheckCustomerAndPricingType(Pricing pricing)
            {
                bool pricingExist;
                var pricingData = _tableWork.PricingRepository.GetSingle(x => x.PricingTypeId == pricing.PricingTypeId && x.CustomerId == pricing.CustomerId);
                if (pricingData != null)
                {
                    pricingExist = true;
                }
                else
                {
                    pricingExist = false;
                }
                return pricingExist;
            }
            public IEnumerable<Scope> GetAllScopesForArtWork()
            {
                return _tableWork.ScopeRepository.Get(x => x.IsDeleted == false && x.DepartmentId == 1).OrderBy(x => x.Description);
            }
            public IEnumerable<Customer> GetCustomers()
            {
                return _tableWork.CustomerRepository.Get(x => x.IsDeleted == false).OrderBy(x => x.Name).AsQueryable();
            }
            public IEnumerable<PricingType> GetAllPricingTypes()
            {
                return _tableWork.PricingTypeRepository.Get(x => x.IsDeleted == false).OrderBy(x => x.Description);
            }
            public bool CheckEmplayeecode(string name)
            {
                return _tableWork.EmployeeRepository.Exist(j => j.EmployeeCode == name && !j.IsDeleted);
            }
            public DropdownCollection GetAllDropDownList()
            {
              
                    var Dept = tow.DepartmentRepository.Get(x => x.IsDeleted == false).ToList();
                    var Desig = tow.DesignationRepository.Get(x => x.IsDeleted == false).ToList();
                    var Comp = tow.CompanyRepository.Get().OrderBy(x => x.Id).ToList();
                    var Profi = tow.ProficiencyRepository.Get().OrderBy(x => x.Id).ToList();
            var coreDivision = tow.DivisionRepository.Get().OrderBy(x => x.Id).ToList();
            var detailList = new DropdownCollection
                    {
                        DepartmentList = Dept,
                        DesignationList = Desig,
                        CompanyList = Comp,
                        ProficiencyList = Profi,
                        DivisionList = coreDivision
            };
                    return detailList;
               
            }
            public Object ddltrainingDetailList()
            {
                var Trainig = _tableWork.ScopeRepository.Get(x => x.IsDeleted == false  && x.ScopeGroupId == null).AsQueryable(); //&& x.DepartmentId == 13
            var DetailList = new
                {
                    TrainingList = Trainig,
                };
                return DetailList;
            }
            public Object GetLocationList()
            {
                return _tableWork.LocationRepository.Get(x => x.IsDeleted == false).AsQueryable();
            }
            public Object GetResignResaonsList()
            {
                var resign = _tableWork.ResignReasonRepository.Get(x => x.IsDeleted == false).OrderBy(x => x.Id).ToList();
                return resign;
            }
            public IEnumerable<Process> GetProcessWorkFlowList()
            {
                var resign = _tableWork.ProcessRepository.Get(x => x.IsActive == true).Where(x => x.ShortName != "BPA" && x.ShortName != "SOA").Select(row => row);
                return resign;
            }

            public Object GetRolesList()
            {
                var GetAllRolesList = _tableWork.RoleRepository.Get(x => x.IsActive == true).Select(row => row);
                return GetAllRolesList;
            }

            public Employee GetEmployeeIdByName(int Id)
            {
                return _tableWork.EmployeeRepository.Get(x => x.EmployeeId == Id && x.IsDeleted == false).Select(row => row).FirstOrDefault();
            }
            public IEnumerable<Employee> GetEmployeeList()
            {
                return _tableWork.EmployeeRepository.Get(x => x.IsDeleted == false && x.ResignReasons == null).OrderBy(x => x.EmployeeCode).AsQueryable();
            }
            public IEnumerable<JobCategory> GetEstTime()
            {
                return _tableWork.JobCategoryRepository.Get().AsQueryable();
            }
            public IEnumerable<GetViewEmployeeDetails> GetAllEmployee()
            {
                var shiftvsEmployeeDetail = new List<GetViewEmployeeDetails>();
                var lstEmployee = _viewWork.ViewEmployeeDetailsfromEMPRepository.Get(x => x.IsDeleted == false && (x.DateOfResignation == null || x.ResignReasons == null)).OrderByDescending(x => x.EmployeeId).ToList();
                foreach (var item in lstEmployee)
                {
                    if (item.Expr1 == null)
                    {
                        shiftvsEmployeeDetail.Add(new GetViewEmployeeDetails
                        {
                            EmployeeId = item.EmployeeId,
                            EmployeeCode = item.EmployeeCode,
                            EmployeeName = item.EmployeeName,
                            DepartmentDesc = item.DepartmentDesc,
                            DesignationDesc = item.DesignationDesc,
                            ProfiencyDesc = item.ProfiencyDesc,
                            ManagerName1 = item.ManagerName1,
                            LeaderName1 = item.LeaderName1,
                            EffectFrom = null,
                            EffectTo = null,
                            Result = false,
                        });
                    }
                    else
                    {
                        shiftvsEmployeeDetail.Add(new GetViewEmployeeDetails
                        {
                            EmployeeId = item.EmployeeId,
                            EmployeeCode = item.EmployeeCode,
                            EmployeeName = item.EmployeeName,
                            DepartmentDesc = item.DepartmentDesc,
                            DesignationDesc = item.DesignationDesc,
                            ProfiencyDesc = item.ProfiencyDesc,
                            ManagerName1 = item.ManagerName1,
                            LeaderName1 = item.LeaderName1,
                            ShiftId = item.Expr1,
                            Description = item.Description,
                            ShiftType = item.ShiftType,
                            EffectFrom = item.EffectiveFrom.ToString(),
                            EffectTo = item.EffectiveTo.ToString(),
                            Result = true,
                        });
                    }
                }
                return shiftvsEmployeeDetail;
            }
            public IEnumerable<GetViewEmployeeDetails> GetEmployeeDetailsIncludingDelete(string IsDeleted, string IsResigned)
            {
                List<ViewEmployeeDetailsfromEmp> lstEmployee = null;
                var ShiftvsEmployeeDetail = new List<GetViewEmployeeDetails>();
              
                    if ((IsDeleted == "true") || (IsResigned == "true"))
                    {
                        if (IsDeleted == "true")
                        {
                            lstEmployee = vow.ViewEmployeeDetailsfromEMPRepository.Get(x => x.IsDeleted == true || x.IsDeleted == false).OrderBy(x => x.EmployeeCode).ToList();
                        }
                        else
                        {
                            lstEmployee = vow.ViewEmployeeDetailsfromEMPRepository.Get(x => x.ResignReasons != null || x.ResignReasons == null)
                                    .OrderBy(x => x.EmployeeCode)
                                    .ToList();
                        }
                        if (IsDeleted == "true" && IsResigned == "true")

                            lstEmployee = vow.ViewEmployeeDetailsfromEMPRepository.Get(x => x.IsDeleted == true || x.IsDeleted == false || x.ResignReasons != null || x.ResignReasons == null).OrderBy(x => x.EmployeeCode).ToList();
                    }
                    else
                    {
                        lstEmployee = vow.ViewEmployeeDetailsfromEMPRepository.Get(x => x.IsDeleted == false && x.ResignReasons == null).OrderBy(x => x.EmployeeCode).ToList();
                    }
                    foreach (var item in lstEmployee)
                    {
                        if (item.Expr1 == null)
                        {
                            ShiftvsEmployeeDetail.Add(new GetViewEmployeeDetails
                            {
                                EmployeeId = item.EmployeeId,
                                EmployeeCode = item.EmployeeCode,
                                EmployeeName = item.EmployeeName,
                                DepartmentDesc = item.DepartmentDesc,
                                DesignationDesc = item.DesignationDesc,
                                ProfiencyDesc = item.ProfiencyDesc,
                                ManagerName1 = item.ManagerName1,
                                LeaderName1 = item.LeaderName1,
                                EffectFrom = null,
                                EffectTo = null,
                                Result = false,
                            });
                        }
                        else
                        {
                            ShiftvsEmployeeDetail.Add(new GetViewEmployeeDetails
                            {
                                EmployeeId = item.EmployeeId,
                                EmployeeCode = item.EmployeeCode,
                                EmployeeName = item.EmployeeName,
                                DepartmentDesc = item.DepartmentDesc,
                                DesignationDesc = item.DesignationDesc,
                                ProfiencyDesc = item.ProfiencyDesc,
                                ManagerName1 = item.ManagerName1,
                                LeaderName1 = item.LeaderName1,
                                ShiftId = item.Expr1,
                                Description = item.Description,
                                ShiftType = item.ShiftType,
                                EffectFrom = item.EffectiveFrom.ToString(),
                                EffectTo = item.EffectiveTo.ToString(),
                                Result = true,
                            });
                        }
                    }
              
                return ShiftvsEmployeeDetail;
            }
            public IEnumerable<Shift> GetEmployeeShift()
            {
                return _tableWork.ShiftRepository.Get().OrderBy(x => x.Id).AsQueryable();
            }
            public Employee getEmployeebyID(int employeeId)
            {
                return _tableWork.EmployeeRepository.Get(x => x.EmployeeId == employeeId).OrderBy(x => x.EmployeeCode).FirstOrDefault();
            }
            //
            public Object GetEmployeeDetailsWithAddress(int employeeId)
            {
                var Id = new List<int>();
                var employeeDetailsWithAddress = _viewWork.ViewEmployeeDetailsWithADDRepository.Get(x => x.EmployeeId == employeeId).ToList();
                var EmployeeDetailsWithAddressDetail = new GetViewEmployeeDetailsWithAddress();
                foreach (var item in employeeDetailsWithAddress)
                {
                    EmployeeDetailsWithAddressDetail.EmployeeName = item.EmployeeName;
                    EmployeeDetailsWithAddressDetail.EmployeeId = item.EmployeeId;
                    EmployeeDetailsWithAddressDetail.EmployeeCode = item.EmployeeCode;
                    EmployeeDetailsWithAddressDetail.CompanyId = item.CompanyId;
                    EmployeeDetailsWithAddressDetail.CreatedBy = item.CreatedBy;
                    EmployeeDetailsWithAddressDetail.DepartmentId = item.DepartmentId;
                    EmployeeDetailsWithAddressDetail.DesignationId = item.DesignationId;
                    EmployeeDetailsWithAddressDetail.ProfiencyId = item.ProfiencyId;
                    EmployeeDetailsWithAddressDetail.DateOfBirth = item.DateOfBirth;
                    EmployeeDetailsWithAddressDetail.DateOfJoining = item.DateOfJoining;
                    EmployeeDetailsWithAddressDetail.DateOfResignation = item.DateOfResignation;
                    EmployeeDetailsWithAddressDetail.Email = item.Email;
                    EmployeeDetailsWithAddressDetail.WorkingStatus = item.WorkingStatus;
                    EmployeeDetailsWithAddressDetail.PersonalEmail = item.PersonalEmail;
                    EmployeeDetailsWithAddressDetail.MaritalStatus = item.MaritalStatus;
                    EmployeeDetailsWithAddressDetail.Gender = item.Gender;
                    EmployeeDetailsWithAddressDetail.PhoneNo = item.PhoneNo;
                    EmployeeDetailsWithAddressDetail.MobileNo = item.MobileNo;
                    EmployeeDetailsWithAddressDetail.BloodGroup = item.BloodGroup;
                    EmployeeDetailsWithAddressDetail.EmergencyContactName = item.EmergencyContactName;
                    EmployeeDetailsWithAddressDetail.EmergencyContactNo = item.EmergencyContactNo;
                    EmployeeDetailsWithAddressDetail.DesigID = item.DesignationId;
                    EmployeeDetailsWithAddressDetail.ProfID = item.ProfiencyId;
                    EmployeeDetailsWithAddressDetail.ReportingManager1 = item.ReportingManager1;
                    EmployeeDetailsWithAddressDetail.ManagerName1 = item.ManagerName1;
                    EmployeeDetailsWithAddressDetail.ReportLeader1 = item.ReportLeader1;
                    EmployeeDetailsWithAddressDetail.ReportingManager2 = item.ReportingManager2;
                    EmployeeDetailsWithAddressDetail.ReportingLeader2 = item.ReportingLeader2;
                    EmployeeDetailsWithAddressDetail.LeaderName2 = item.LeaderName2;
                    EmployeeDetailsWithAddressDetail.ResignReasons = item.ResignReasons;
                    EmployeeDetailsWithAddressDetail.IsOutsource = item.IsOutsource;
                    //--------------------------------------vidhya live now---------------------------------------
                    EmployeeDetailsWithAddressDetail.IsInternetConnection = item.IsInternetConnection;
                    EmployeeDetailsWithAddressDetail.IsSystem = item.IsSystem;
                    EmployeeDetailsWithAddressDetail.ServiceProvider = item.ServiceProvider;
                    EmployeeDetailsWithAddressDetail.NetWorkType = item.NetWorkType;
                    EmployeeDetailsWithAddressDetail.SystemConfig = item.SystemConfig;
                    EmployeeDetailsWithAddressDetail.CoreDivision = item.CoreDivision;
                    EmployeeDetailsWithAddressDetail.BatchNo = item.BatchNo;
                //--------------------------------------vidhya live now---------------------------------------

                if (item.AddressType == "Present")
                    {
                        EmployeeDetailsWithAddressDetail.Address11 = item.Address1;
                        EmployeeDetailsWithAddressDetail.Address22 = item.Address2;
                        EmployeeDetailsWithAddressDetail.Address33 = item.Address3;
                        EmployeeDetailsWithAddressDetail.Address1 = item.Address1;
                        EmployeeDetailsWithAddressDetail.Address2 = item.Address2;
                        EmployeeDetailsWithAddressDetail.Address3 = item.Address3;
                        EmployeeDetailsWithAddressDetail.LocationId = (int)item.LocationId;
                        EmployeeDetailsWithAddressDetail.chksame = true;
                    }
                    else if (item.AddressType == "Permanent")
                    {
                        EmployeeDetailsWithAddressDetail.Address1 = item.Address1;
                        EmployeeDetailsWithAddressDetail.Address2 = item.Address2;
                        EmployeeDetailsWithAddressDetail.Address3 = item.Address3;
                        EmployeeDetailsWithAddressDetail.Address11 = item.Address1;
                        EmployeeDetailsWithAddressDetail.Address22 = item.Address2;
                        EmployeeDetailsWithAddressDetail.Address33 = item.Address3;
                        EmployeeDetailsWithAddressDetail.LocationId1 = (int)item.LocationId;
                        EmployeeDetailsWithAddressDetail.chksame = true;
                    }
                }
                var locations = new List<Location>();
              
                    var location = tow.LocationRepository.GetSingle(x => x.Id == EmployeeDetailsWithAddressDetail.LocationId);
                    if (location != null)
                    {
                        locations.Add(location);
                        var city = tow.LocationRepository.GetSingle(x => x.Id == location.ContraLocationId);
                        if (city != null)
                        {
                            locations.Add(city);
                            var state = tow.LocationRepository.GetSingle(x => x.Id == city.ContraLocationId);
                            if (state != null)
                            {
                                locations.Add(state);
                                var country = tow.LocationRepository.GetSingle(x => x.Id == state.ContraLocationId);
                                locations.Add(country);
                            }
                        }
                    }
                    var codes = (from j in tow.EmployeeProcessRepository.Get(j => j.EmployeeId == employeeId)
                                 join p in tow.ProcessRepository.Get() on j.ProcessId equals p.Id
                                 select new
                                 {
                                     p.Id,
                                     p.Name,
                                     p.Description,
                                     p.IsActive,
                                     p.CreatedUtc,
                                     p.UpdatedUtc,
                                     p.CreatedBy,
                                     p.UpdatedBy
                                 }).ToList();


                    var Roles = (from j in tow.EmployeeVsRoleRepository.Get(j => j.EmployeeId == employeeId && j.IsDeleted == false)
                                 join p in tow.RoleRepository.Get() on j.RoleId equals p.Id
                                 select new
                                 {
                                     p.Description,
                                     p.Id,
                                 }).ToList();

                    var EmpHier = (from j in tow.EmployeeHierarchyRepository.Get(j => j.EmployeeId == employeeId && j.IsDeleted == false)
                                   join k in tow.EmployeeRepository.Get() on j.EmployeeId equals k.EmployeeId
                                   select new
                                   {
                                       j.SubEmpId,
                                       j.SubEmpName,
                                       j.IsDeleted
                                   }).ToList();

                    //--
                    var EmpHierDetails = new List<Employee>();
                    foreach (var item in EmpHier)
                    {
                        if (item.SubEmpId != null)
                        {
                            EmpHierDetails.Add(new Employee
                            {
                                EmployeeId = item.SubEmpId,
                                EmployeeName = item.SubEmpName,
                            });
                        }
                    }
                    //--
                    var result = new
                    {
                        AddressDetail = EmployeeDetailsWithAddressDetail,
                        Code = codes,
                        Location = locations,
                        Role = Roles,
                        EmpHry = EmpHierDetails
                    };
                    return result;
               
            }
            public class Description
            {
                public string code { get; set; }
            }
            public IEnumerable<GetViewEmployeeVsShiftDetails> GetViewEmployeeShift()
            {
                var shiftdetails = _viewWork.ViewShiftVsEmployee_ShiftRepository.Get().OrderBy(x => x.EmployeeCode).AsQueryable();
                var ShiftvsEmployeeDetail = new List<GetViewEmployeeVsShiftDetails>();
                foreach (var item in shiftdetails)
                {
                    if (item.ShiftId != null)
                    {
                        ShiftvsEmployeeDetail.Add(new GetViewEmployeeVsShiftDetails
                        {
                            EmployeeId = item.EmployeeId,
                            EmployeeCode = item.EmployeeCode,
                            EmployeeName = item.EmployeeName,
                            ShiftId = item.ShiftId,
                            EffectiveFrom = item.EffectiveFrom,
                            ShiftVsEmployeeId = item.ShiftVsEmployeeId,
                            Result = true,
                        });
                    }
                    else
                    {
                        ShiftvsEmployeeDetail.Add(new GetViewEmployeeVsShiftDetails
                        {
                            EmployeeId = item.EmployeeId,
                            EmployeeCode = item.EmployeeCode,
                            EmployeeName = item.EmployeeName,
                            Result = false,
                        });
                    }
                }
                return ShiftvsEmployeeDetail;
            }
            public IEnumerable<GetViewEmployeeVsShiftDetails> GetViewEmployeeSearchShift(SearchEmployeeinShift searchEmployeeinShift)
            {
                var shiftdetails = _viewWork.ViewShiftVsEmployee_ShiftRepository.Get().AsQueryable();
                if (searchEmployeeinShift.EmployeeCode != null || searchEmployeeinShift.ShiftId != 0)
                {
                    var search = new String[] { "" };
                    if (searchEmployeeinShift.EmployeeCode != null)
                    {
                        search = searchEmployeeinShift.EmployeeCode.Split(',');
                    }
                    shiftdetails = shiftdetails.Where(x => search.Any(s => x.EmployeeCode.Contains(s)) || x.ShiftId == searchEmployeeinShift.ShiftId);
                }
                var ShiftvsEmployeeDetail = new List<GetViewEmployeeVsShiftDetails>();
                foreach (var item in shiftdetails)
                {
                    if (item.ShiftId != null)
                    {
                        ShiftvsEmployeeDetail.Add(new GetViewEmployeeVsShiftDetails
                        {
                            EmployeeId = item.EmployeeId,
                            EmployeeCode = item.EmployeeCode,
                            EmployeeName = item.EmployeeName,
                            ShiftId = item.ShiftId,
                            ShiftVsEmployeeId = item.ShiftVsEmployeeId,
                            EffectiveFrom = item.EffectiveFrom,
                            Result = true,
                        });
                    }
                    else
                    {
                        ShiftvsEmployeeDetail.Add(new GetViewEmployeeVsShiftDetails
                        {
                            EmployeeId = item.EmployeeId,
                            EmployeeCode = item.EmployeeCode,
                            EmployeeName = item.EmployeeName,
                            Result = false,
                        });
                    }
                }
                return ShiftvsEmployeeDetail;
            }
            public IEnumerable<GetViewEmployeeDetails> GetEmployeeByEmployeeCode(GetViewEmployeeDetails ovEmp)
            {
                var ShiftvsEmployeeDetail = new List<GetViewEmployeeDetails>();
                var lstEmployee = _viewWork.ViewEmployeeDetailswithShiftRepository.Get().AsQueryable();
                if ((ovEmp.EmployeeCode == "" || ovEmp.EmployeeCode == null) && (ovEmp.EmployeeName == "" || ovEmp.EmployeeName == null) &&
                     (ovEmp.DepartmentDesc == "" || ovEmp.DepartmentDesc == null) && (ovEmp.DesignationDesc == "" || ovEmp.DesignationDesc == null) &&
                     (ovEmp.ProfiencyDesc == "" || ovEmp.ProfiencyDesc == null) && (ovEmp.LeaderName1 == "" || ovEmp.LeaderName1 == null) &&
                    (ovEmp.ManagerName1 == "" || ovEmp.ManagerName1 == null))
                {
                    lstEmployee = lstEmployee.OrderBy(x => x.EmployeeCode);

                    if (ovEmp.IsDeleted == false)
                    {
                        lstEmployee = lstEmployee.Where(x => x.IsDeleted.Equals(ovEmp.IsDeleted));
                    }
                    if (ovEmp.IsResigned == false)
                    {
                        lstEmployee = lstEmployee.Where(x => x.ResignReasons.Equals(null));
                    }
                }
                else
                {
                    lstEmployee = lstEmployee.Where(x => (x.EmployeeCode ?? "").Contains(ovEmp.EmployeeCode) || (x.EmployeeName ?? "").Contains(ovEmp.EmployeeName)
                         || (x.DepartmentDesc ?? "").Contains(ovEmp.DepartmentDesc) || (x.DesignationDesc ?? "").Contains(ovEmp.DesignationDesc)
                         || (x.ProfiencyDesc ?? "").Contains(ovEmp.ProfiencyDesc) || (x.LeaderName1 ?? "").Contains(ovEmp.LeaderName1)
                        || (x.ManagerName1 ?? "").Contains(ovEmp.ManagerName1)).OrderBy(x => x.EmployeeCode);

                    if (ovEmp.IsDeleted == false)
                    {
                        lstEmployee = lstEmployee.Where(x => x.IsDeleted.Equals(ovEmp.IsDeleted));
                    }
                    if (ovEmp.IsResigned == false)
                    {
                        lstEmployee = lstEmployee.Where(x => x.ResignReasons.Equals(null));
                    }
                }
                foreach (var item in lstEmployee)
                {
                    if (item.Id == null)
                    {
                        ShiftvsEmployeeDetail.Add(new GetViewEmployeeDetails
                        {
                            EmployeeId = item.EmployeeId,
                            EmployeeCode = item.EmployeeCode,
                            EmployeeName = item.EmployeeName,
                            DepartmentDesc = item.DepartmentDesc,
                            DesignationDesc = item.DesignationDesc,
                            ProfiencyDesc = item.ProfiencyDesc,
                            ManagerName1 = item.ManagerName1,
                            LeaderName1 = item.LeaderName1,
                            EffectFrom = null,
                            EffectTo = null,
                            Result = false,
                        });
                    }
                    else
                    {
                        ShiftvsEmployeeDetail.Add(new GetViewEmployeeDetails
                        {
                            EmployeeId = item.EmployeeId,
                            EmployeeCode = item.EmployeeCode,
                            EmployeeName = item.EmployeeName,
                            DepartmentDesc = item.DepartmentDesc,
                            DesignationDesc = item.DesignationDesc,
                            ProfiencyDesc = item.ProfiencyDesc,
                            ManagerName1 = item.ManagerName1,
                            LeaderName1 = item.LeaderName1,
                            ShiftId = item.Id,
                            Description = item.Description,
                            ShiftType = item.ShiftType,
                            EffectFrom = item.EffectiveFrom.ToString(),
                            EffectTo = item.EffectiveTo.ToString(),
                            Result = true,
                        });
                    }
                }
                return ShiftvsEmployeeDetail;
            }
            public string AddEmployee(AddEmployee employee)
            {
               
                    var result = "";
                    try
                    {
                        var employees = new Employee
                        {
                            EmployeeCode = employee.EmployeeCode,
                            EmployeeName = employee.EmployeeName,
                            DepartmentId = employee.DepartmentId,
                            DesignationId = employee.DesignationId,
                            DateOfJoining = employee.DateOfJoining,
                            DateOfBirth = employee.DateOfBirth,
                            BloodGroup = employee.BloodGroup,
                            Gender = employee.Gender,
                            MaritalStatus = employee.MaritalStatus,
                            CompanyId = 1,
                            ProfiencyId = employee.ProfiencyId,
                            EmergencyContactName = employee.EmergencyContactName,
                            EmergencyContactNo = employee.EmergencyContactNo,
                            Email = employee.Email,
                            WorkingStatus=employee.WorkingStatus,
                            PersonalEmail = employee.PersonalEmail, //
                            CreatedUtc = DateTime.UtcNow,
                            CreatedBy = employee.CreatedBy,
                            ReportingManager1 = employee.ReportingManager1,
                            ReportLeader1 = employee.ReportLeader1,
                            ReportingManager2 = employee.ReportingManager2,
                            ReportingLeader2 = employee.ReportingLeader2,
                            MobileNo = employee.MobileNo,
                            PhoneNo = employee.PhoneNo,
                            DateOfResignation = null,
                            ResignReasons = null,
                            IsOutsource = employee.IsOutsource,
                            //--------------------------------------vidhya live now---------------------------------------
                            IsInternetConnection = employee.IsInternetConnection,
                            IsSystem = employee.IsSystem,
                            NetWorkType = employee.NetWorkType,
                            ServiceProvider = employee.ServiceProvider,
                            SystemConfig = employee.SystemConfig,
                            CoreDivision = employee.CoreDivision,
                            BatchNo = employee.BatchNo
                            //--------------------------------------vidhya live now---------------------------------------

                        };
                        tow.EmployeeRepository.Insert(employees);
                        tow.SaveChanges();
                var addEmployee = new EmployeeCoreDivision
                {
                    EmployeeId = employees.EmployeeId,
                    EmployeeName = employee.EmployeeName,
                    CoreDivisionId = employee.CoreDivision,
                    EffectiveFromDate = DateTime.UtcNow,
                    IsActive = true,
                    CreatedBy = employee.CreatedBy,
                    CreatedUtc = DateTime.UtcNow
                };
                _tableWork.EmployeeCoreDivisionRepository.Insert(addEmployee);
                _tableWork.SaveChanges();
                var employeeCommunication = new EmployeeCommunication
                        {
                            EmployeeId = employees.EmployeeId,
                            Address1 = employee.Address1,
                            Address2 = employee.Address2,
                            Address3 = employee.Address3,
                            LocationId = employee.LocationId,
                            AddressType = "Present"
                        };
                        tow.EmployeeCommunicationRepository.Insert(employeeCommunication);
                        tow.SaveChanges();

                        if (!string.IsNullOrEmpty(employee.Address11) && !string.IsNullOrEmpty(employee.Address22) &&
                            !string.IsNullOrEmpty(employee.Address33) && (employee.LocationId1 != 0))
                        {
                            employeeCommunication.EmployeeId = employees.EmployeeId;
                            employeeCommunication.Address1 = employee.Address11;
                            employeeCommunication.Address2 = employee.Address22;
                            employeeCommunication.Address3 = employee.Address33;
                            employeeCommunication.LocationId = employee.LocationId1;
                            employeeCommunication.AddressType = "Permanent";
                            tow.EmployeeCommunicationRepository.Insert(employeeCommunication);
                            tow.SaveChanges();
                        }
/*                        try
                        {
*/                            if (employee.ProcessCode != null)
                            {
                                foreach (var value in employee.ProcessCode)
                                {
                                    var employeeProcess = new EmployeeProcess
                                    {
                                        EmployeeId = employees.EmployeeId,
                                        ProcessId = value,
                                        CreatedUtc = employees.CreatedUtc,
                                        CreatedBy = employees.CreatedBy,
                                        IsDeleted = false
                                    };
                                    tow.EmployeeProcessRepository.Insert(employeeProcess);
                                }
                                tow.SaveChanges();
                            }

                            //

                            int employeeId = employees.EmployeeId;
                            string EmployeeCode = employees.EmployeeCode;
                            string EmployeeName = employees.EmployeeName;

                            if (employee.EmpRolesList != null)
                            {
                                foreach (var item in employee.EmpRolesList)
                                {
                                    EmployeeVsRole employeevsRoles = new EmployeeVsRole
                                    {
                                        EmployeeId = employeeId,
                                        EmployeeCode = EmployeeCode,
                                        EmployeeName = EmployeeName,
                                        RoleId = item.RoleId,
                                        RoleDescription = item.RoleDescription,
                                        CreatedUtc = DateTime.Now,
                                        // CreatedUTC = _db.Employees.First(f => f.EmployeeId == employee.EmployeeId).CreatedUTC,                                   
                                        CreatedBy = item.CreatedBy,
                                        IsDeleted = false
                                    };
                                    tow.EmployeeVsRoleRepository.Insert(employeevsRoles);
                                }
                                tow.SaveChanges();
                            }
                            //
                            if (employee.EmpHierarchyList != null)
                            {
                                foreach (var item in employee.EmpHierarchyList)
                                {
                                    var employeeHierarchy = new EmployeeHierarchy
                                    {
                                        EmployeeId = employees.EmployeeId,
                                        EmployeeName = employees.EmployeeName,
                                        DesignationId = employees.DesignationId,
                                        SubEmpId = item.SubEmpId,
                                        SubEmpName = item.SubEmpName,
                                        CreatedBy = item.CreatedBy,
                                        IsDeleted = false,
                                        // CreatedUTC = _db.Employees.First(f => f.EmployeeId == employee.EmployeeId).CreatedUTC,
                                        CreatedUtc = DateTime.Now,
                                    };
                                    tow.EmployeeHierarchyRepository.Insert(employeeHierarchy);
                                }
                                tow.SaveChanges();
                            }
                            //

/*                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
*/

                        result = "Employee Added Successfully";
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    return result;
               
            }
            public string UpdateEmployee(AddEmployee employee)
            {
              
                    var result = "";
                    ShiftVsEmployee shiftVsEmployee = null;
                    ErrorCategory errorCategory = null;
                    try
                    {
                        if (employee != null)
                        {
                            var employeeCheck = tow.EmployeeRepository.Exist(detail => detail.EmployeeId == employee.EmployeeId);
                            if (employeeCheck == true)
                            {
                                var employeeDetail = tow.EmployeeRepository.GetSingle(detail => detail.EmployeeId == employee.EmployeeId);
                                employeeDetail.EmployeeCode = employee.EmployeeCode;
                                employeeDetail.EmployeeName = employee.EmployeeName;
                                employeeDetail.DepartmentId = employee.DepartmentId;
                                employeeDetail.DesignationId = employee.DesignationId;
                                employeeDetail.DateOfJoining = employee.DateOfJoining;
                                employeeDetail.DateOfBirth = employee.DateOfBirth;
                                employeeDetail.BloodGroup = employee.BloodGroup;
                                employeeDetail.Gender = employee.Gender;
                                employeeDetail.MaritalStatus = employee.MaritalStatus;
                                employeeDetail.CompanyId = 1;
                                employeeDetail.ProfiencyId = employee.ProfiencyId;
                                employeeDetail.EmergencyContactName = employee.EmergencyContactName;
                                employeeDetail.EmergencyContactNo = employee.EmergencyContactNo;
                                employeeDetail.Email = employee.Email;
                                employeeDetail.WorkingStatus=employee.WorkingStatus;
                                employeeDetail.PersonalEmail = employee.PersonalEmail; //
                                //employeeDetail.CreatedBy = employee.CreatedBy;
                                employeeDetail.UpdatedBy = employee.UpdatedBy;
                                employeeDetail.UpdatedUtc = DateTime.UtcNow;
                                employeeDetail.ReportingManager1 = employee.ReportingManager1;
                                employeeDetail.ReportLeader1 = employee.ReportLeader1;
                                employeeDetail.ReportingManager2 = employee.ReportingManager2;
                                employeeDetail.ReportingLeader2 = employee.ReportingLeader2;
                                employeeDetail.MobileNo = employee.MobileNo;
                                employeeDetail.PhoneNo = employee.PhoneNo;
                                employeeDetail.IsOutsource = employee.IsOutsource;
                                employeeDetail.CoreDivision = employee.CoreDivision;
                                employeeDetail.BatchNo = employee.BatchNo;


                        //--------------------------------------vidhya live now---------------------------------------
                        employeeDetail.IsInternetConnection = employee.IsInternetConnection;
                                employeeDetail.IsSystem = employee.IsSystem;
                                if (employee.IsInternetConnection == "NO")
                                {
                                    employee.NetWorkType = null;
                                    employee.ServiceProvider = null;
                                }
                                employeeDetail.NetWorkType = employee.NetWorkType;
                                employeeDetail.ServiceProvider = employee.ServiceProvider;
                                if (employee.IsSystem == "NO")
                                {
                                    employee.SystemConfig = null;
                                }
                                employeeDetail.SystemConfig = employee.SystemConfig;
                                //--------------------------------------vidhya live now---------------------------------------

                                employeeDetail.DateOfResignation = employee.DateOfResignation.HasValue ? employee.DateOfResignation : null;
                                if (employee.ResignReasons != 0)
                                {
                                    employeeDetail.ResignReasons = employee.ResignReasons;
                                    employeeDetail.IsDeleted = true;

                                    User user = new User();
                                    var usercheck = tow.UserRepository.GetSingle(x => x.EmployeeId == employee.EmployeeId);
                                    usercheck.IsDeleted = true;
                                    tow.UserRepository.Update(usercheck);
                                    tow.SaveChanges();
                                }
                                else
                                {
                                    employeeDetail.ResignReasons = null;
                                }
                                tow.EmployeeRepository.Update(employeeDetail);
                                tow.SaveChanges();
                            }
                            else
                            {
                                var employees = new Employee
                                {
                                    EmployeeCode = employee.EmployeeCode,
                                    EmployeeName = employee.EmployeeName,
                                    DepartmentId = employee.DepartmentId,
                                    DesignationId = employee.DesignationId,
                                    DateOfJoining = employee.DateOfJoining,
                                    DateOfBirth = employee.DateOfBirth,
                                    BloodGroup = employee.BloodGroup,
                                    Gender = employee.Gender,
                                    MaritalStatus = employee.MaritalStatus,
                                    CompanyId = 1,
                                    ProfiencyId = employee.ProfiencyId,
                                    EmergencyContactName = employee.EmergencyContactName,
                                    EmergencyContactNo = employee.EmergencyContactNo,
                                    Email = employee.Email,
                                    WorkingStatus=employee.WorkingStatus,
                                    PersonalEmail = employee.PersonalEmail, //
                                    CreatedUtc = DateTime.UtcNow,
                                    CreatedBy = employee.CreatedBy,
                                    ReportingManager1 = employee.ReportingManager1,
                                    ReportLeader1 = employee.ReportLeader1,
                                    ReportingManager2 = employee.ReportingManager2,
                                    ReportingLeader2 = employee.ReportingLeader2,
                                    MobileNo = employee.MobileNo,
                                    PhoneNo = employee.PhoneNo,
                                    IsOutsource = employee.IsOutsource,
                                    DateOfResignation = employee.DateOfResignation.HasValue ? employee.DateOfResignation : null,
                                    CoreDivision = employee.CoreDivision,
                                    BatchNo = employee.BatchNo
                                };
                                if (employee.ResignReasons != 0)
                                {
                                    employees.ResignReasons = employee.ResignReasons;
                                }
                                else
                                {
                                    employees.ResignReasons = null;
                                }
                                tow.EmployeeRepository.Insert(employees);
                                tow.SaveChanges();
                            }
                    var existingEmployee = _tableWork.EmployeeCoreDivisionRepository.GetSingle(e => e.EmployeeId == employee.EmployeeId && e.IsActive == true);
                    if (existingEmployee != null)
                    {
                        EmployeeCoreDivision employeeCoreDivision = new EmployeeCoreDivision();
                        employeeCoreDivision.EmployeeId = employee.EmployeeId;
                        employeeCoreDivision.EmployeeName = employee.EmployeeName;
                        employeeCoreDivision.CoreDivisionId = employee.CoreDivision;
                        employeeCoreDivision.EffectiveFromDate = DateTime.UtcNow;
                        employeeCoreDivision.IsActive = true;
                        employeeCoreDivision.CreatedBy = employee.UpdatedBy;
                        employeeCoreDivision.CreatedUtc = DateTime.UtcNow;
                        _tableWork.EmployeeCoreDivisionRepository.Insert(employeeCoreDivision);

                        existingEmployee.EffectiveToDate = employeeCoreDivision.EffectiveFromDate.AddDays(-1);
                        existingEmployee.UpdatedBy = employee.UpdatedBy;
                        existingEmployee.UpdatedUtc = DateTime.UtcNow;
                        existingEmployee.IsActive = false;
                        _tableWork.EmployeeCoreDivisionRepository.Update(existingEmployee);
                        _tableWork.SaveChanges();
                    }
                    var employeeCommunicationCheck = tow.EmployeeCommunicationRepository.Exist(detail => detail.EmployeeId == employee.EmployeeId);
                            if (employeeCommunicationCheck == true)
                            {
                                var employeeCommunicationDetails = tow.EmployeeCommunicationRepository.GetSingle(detail => detail.EmployeeId == employee.EmployeeId);
                                employeeCommunicationDetails.EmployeeId = employee.EmployeeId;
                                employeeCommunicationDetails.Address1 = employee.Address1;
                                employeeCommunicationDetails.Address2 = employee.Address2;
                                employeeCommunicationDetails.Address3 = employee.Address3;
                                employeeCommunicationDetails.LocationId = employee.LocationId;
                                employeeCommunicationDetails.AddressType = "Present";
                                tow.EmployeeCommunicationRepository.Update(employeeCommunicationDetails);
                                tow.SaveChanges();
                            }
                            else
                            {
                                var employeeCommunication = new EmployeeCommunication
                                {
                                    EmployeeId = employee.EmployeeId,
                                    Address1 = employee.Address1,
                                    Address2 = employee.Address2,
                                    Address3 = employee.Address3,
                                    LocationId = employee.LocationId,
                                    AddressType = "Present"
                                };
                                tow.EmployeeCommunicationRepository.Insert(employeeCommunication);
                                tow.SaveChanges();
                            }
                            List<EmployeeProcess> deleteEmployeeProcessById = new List<EmployeeProcess>();
                            deleteEmployeeProcessById = tow.EmployeeProcessRepository.Get(delete => delete.EmployeeId == employee.EmployeeId).ToList();
                            if (deleteEmployeeProcessById != null)
                            {
                                foreach (var value in deleteEmployeeProcessById.Select(x => x.Id))
                                {
                                    var deletingrmployeevsprocess = tow.EmployeeProcessRepository.GetSingle(x => x.Id == value);
                                    tow.EmployeeProcessRepository.Delete(deletingrmployeevsprocess);
                                }
                            }
                            tow.SaveChanges();
                            List<EmployeeProcess> lstemployeeProcesses = new List<EmployeeProcess>();
/*                            try
                            {
*/                                if (employee.ProcessCode != null)
                                {
                                    foreach (var value in employee.ProcessCode)
                                    {
                                        EmployeeProcess employeeProcess = new EmployeeProcess
                                        {
                                            EmployeeId = employee.EmployeeId,
                                            ProcessId = value,
                                            CreatedUtc = tow.EmployeeRepository.GetSingle(f => f.EmployeeId == employee.EmployeeId).CreatedUtc,
                                            UpdatedUtc = DateTime.UtcNow,
                                            CreatedBy = employee.CreatedBy,
                                            IsDeleted = false
                                        };
                                        lstemployeeProcesses.Add(employeeProcess);
                                    }
                                    tow.EmployeeProcessRepository.AddRange(lstemployeeProcesses);
                                    tow.SaveChanges();
                                }
/*                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }*/
                            List<EmployeeHierarchy> lstemployeeHierarchy = new List<EmployeeHierarchy>();
/*                            try
                            {
*/                                if (employee.EmpHierarchyList != null)
                                {
                                    var deleteEmployeeHierarchyById = tow.EmployeeHierarchyRepository.Get(delete => delete.EmployeeId == employee.EmployeeId).ToList();
                                    if (deleteEmployeeHierarchyById != null)
                                    {
                                        foreach (var item in deleteEmployeeHierarchyById)
                                        {
                                            item.IsDeleted = true;
                                            item.UpdatedUtc = DateTime.UtcNow;
                                            item.UpdatedBy = employee.UpdatedBy;
                                            // _db.Entry(deleteEmployeeRolesById).State = EntityState.Modified;
                                        }
                                        //_db.Entry(deleteEmployeeRolesById).State = EntityState.Modified;
                                        tow.SaveChanges();
                                    }
                                    foreach (var value in employee.EmpHierarchyList)
                                    {
                                        EmployeeHierarchy employeeHierarchy = new EmployeeHierarchy
                                        {
                                            EmployeeId = employee.EmployeeId,
                                            EmployeeName = employee.EmployeeName,
                                            DesignationId = employee.DesignationId,
                                            SubEmpId = value.SubEmpId,
                                            SubEmpName = value.SubEmpName,
                                            CreatedBy = value.CreatedBy,
                                            IsDeleted = false,
                                            CreatedUtc = tow.EmployeeRepository.GetSingle(f => f.EmployeeId == employee.EmployeeId).CreatedUtc,

                                        };
                                        lstemployeeHierarchy.Add(employeeHierarchy);
                                    }
                                    tow.EmployeeHierarchyRepository.AddRange(lstemployeeHierarchy);
                                    tow.SaveChanges();
                                }
/*                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
*/
                            //
                            //var deleteEmployeeRolesById = _db.EmployeeVsRoles.FirstOrDefault(delete => delete.EmployeeId == employee.EmployeeId);
                            //deleteEmployeeRolesById.IsDeleted = true;
                            //deleteEmployeeRolesById.UpdatedUTC = DateTime.UtcNow;
                            //deleteEmployeeRolesById.UpdatedBy = deleteEmployeeRolesById.EmployeeId;
                            //_db.Entry(deleteEmployeeRolesById).State = EntityState.Modified;
                            // _db.EmployeeProcesses.RemoveRange(deleteEmployeeProcessById);
                            //result = _db.SaveChanges() > 0;

                            List<EmployeeVsRole> lstemployeeRole = new List<EmployeeVsRole>();
/*                            try
                            {
*/                                if (employee.EmpRolesList != null)
                                {
                                    var deleteEmployeeRolesById = tow.EmployeeVsRoleRepository.Get(delete => delete.EmployeeId == employee.EmployeeId).ToList();
                                    if (deleteEmployeeRolesById != null)
                                    {
                                        foreach (var item in deleteEmployeeRolesById)
                                        {
                                            item.IsDeleted = true;
                                            item.UpdatedUtc = DateTime.UtcNow;
                                            item.UpdatedBy = employee.UpdatedBy;
                                            // _db.Entry(deleteEmployeeRolesById).State = EntityState.Modified;
                                        }
                                        //_db.Entry(deleteEmployeeRolesById).State = EntityState.Modified;
                                        tow.SaveChanges();
                                    }
                                    foreach (var item in employee.EmpRolesList)
                                    {
                                        EmployeeVsRole EmpRole = new EmployeeVsRole
                                        {
                                            EmployeeId = employee.EmployeeId,
                                            EmployeeCode = employee.EmployeeCode,
                                            EmployeeName = employee.EmployeeName,
                                            RoleId = item.RoleId,
                                            RoleDescription = item.RoleDescription,
                                            CreatedUtc = tow.EmployeeRepository.GetSingle(f => f.EmployeeId == employee.EmployeeId).CreatedUtc,
                                            UpdatedUtc = DateTime.UtcNow,
                                            CreatedBy = item.CreatedBy,
                                            IsDeleted = false
                                        };
                                        lstemployeeRole.Add(EmpRole);
                                    }
                                    tow.EmployeeVsRoleRepository.AddRange(lstemployeeRole);
                                    tow.SaveChanges();
                                }
/*                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
*/                            //
                        }

                        result = "Employee Updated Successfully";
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    return result;
               
            }


            public bool DeleteEmployee(int employeeId, int loginId)
            {
              
                    var employee = getEmployeebyID(employeeId);
                    employee.IsDeleted = true;
                    employee.UpdatedUtc = DateTime.UtcNow;
                    employee.UpdatedBy = loginId;
                    tow.EmployeeRepository.Update(employee);
                    return tow.SaveChanges() > 0;
               
            }
            public bool AddShiftVsEmployee(AddShiftVsEmployee shiftvsemp)
            {
                string empidshiftid = shiftvsemp.Test;
                empidshiftid = empidshiftid.Replace("\\", "").Replace("\"", "").Replace("'", "").Replace("{", "").Replace("}", "");
                var empidshiftidvalue = empidshiftid.Split(',');
                foreach (var item in empidshiftidvalue)
                {
                    var empidshiftidsplit = item.Split(':');
                    var empid = empidshiftidsplit[0];
                    var shiftid = empidshiftidsplit[1];
                    int EmployeeId = Convert.ToInt32(empid);
                    int ShiftId = Convert.ToInt32(shiftid);
                  
                        var ShiftDetails = _tableWork.ShiftVsEmployeeRepository.GetSingle((x => x.EmployeeId == EmployeeId && x.EffectiveTo == null));
                        if (ShiftDetails != null)
                        {
                            ShiftDetails.EffectiveTo = shiftvsemp.EffectiveFrom.ToUniversalTime().AddDays(-1);
                            ShiftDetails.IsDeleted = shiftvsemp.IsDeleted;
                            ShiftDetails.UpdatedById = shiftvsemp.UpdatedById;
                            ShiftDetails.UpdatedUtc = DateTime.Now.ToUniversalTime();
                            tow.ShiftVsEmployeeRepository.Update(ShiftDetails);
                        }
                
                    var oShiftVsEmployee = new ShiftVsEmployee
                    {
                        EmployeeId = EmployeeId,
                        ShiftId = ShiftId,
                        EffectiveFrom = shiftvsemp.EffectiveFrom.ToUniversalTime(),
                        IsDeleted = shiftvsemp.IsDeleted,
                        CreatedById = shiftvsemp.CreatedById,
                        CreatedUtc = DateTime.Now.ToUniversalTime()
                    };
                    _tableWork.ShiftVsEmployeeRepository.Insert(oShiftVsEmployee);
                }
                return _tableWork.SaveChanges() > 0;
            }
            public bool SaveShiftVsEmployee(SaveShiftVsEmployee shiftvsemp)
            {
                IEnumerable<GetViewEmployeeVsShiftDetails> oGetViewEmployeeVsShiftDetails = shiftvsemp.Test;
                foreach (var item in oGetViewEmployeeVsShiftDetails)
                {
                    int EmployeeId = item.EmployeeId;
                 
                        var ShiftDetails = _tableWork.ShiftVsEmployeeRepository.GetSingle((x => x.EmployeeId == EmployeeId && x.EffectiveTo == null));
                        if (ShiftDetails != null)
                        {
                            ShiftDetails.EffectiveTo = shiftvsemp.EffectiveFrom.ToUniversalTime().AddDays(-1);
                            ShiftDetails.IsDeleted = shiftvsemp.IsDeleted;
                            ShiftDetails.UpdatedById = shiftvsemp.UpdatedById;
                            ShiftDetails.UpdatedUtc = DateTime.UtcNow;
                            tow.ShiftVsEmployeeRepository.Update(ShiftDetails);
                        }
                   
                    ShiftVsEmployee oShiftVsEmployee = new ShiftVsEmployee();
                    oShiftVsEmployee.EmployeeId = EmployeeId;
                    oShiftVsEmployee.ShiftId = shiftvsemp.ShiftId;
                    oShiftVsEmployee.EffectiveFrom = shiftvsemp.EffectiveFrom.ToUniversalTime();
                    oShiftVsEmployee.IsDeleted = shiftvsemp.IsDeleted;
                    oShiftVsEmployee.CreatedById = shiftvsemp.CreatedById;
                    oShiftVsEmployee.CreatedUtc = DateTime.UtcNow;
                    _tableWork.ShiftVsEmployeeRepository.Insert(oShiftVsEmployee);
                }
                return _tableWork.SaveChanges() > 0;
            }
            public bool UpdateShiftVsEmployee(ShiftVsEmployee shiftvsemp)
            {
              
                    var ShiftDetails = tow.ShiftVsEmployeeRepository.GetSingle(x => x.Id == shiftvsemp.Id);
                    if (ShiftDetails != null)
                    {
                        ShiftDetails.Id = shiftvsemp.Id;
                        ShiftDetails.EffectiveTo = shiftvsemp.EffectiveFrom.ToUniversalTime().AddDays(-1);
                        ShiftDetails.IsDeleted = shiftvsemp.IsDeleted;
                        ShiftDetails.UpdatedById = shiftvsemp.UpdatedById;
                        ShiftDetails.UpdatedUtc = DateTime.Now.ToUniversalTime();
                        tow.ShiftVsEmployeeRepository.Update(ShiftDetails);
                    }
              
                ShiftVsEmployee oShiftVsEmployee = new ShiftVsEmployee();
                oShiftVsEmployee.EmployeeId = shiftvsemp.EmployeeId;
                oShiftVsEmployee.ShiftId = shiftvsemp.ShiftId;
                oShiftVsEmployee.EffectiveFrom = shiftvsemp.EffectiveFrom.ToUniversalTime();
                oShiftVsEmployee.IsDeleted = shiftvsemp.IsDeleted;
                oShiftVsEmployee.CreatedById = shiftvsemp.CreatedById;
                oShiftVsEmployee.CreatedUtc = DateTime.Now.ToUniversalTime();
                _tableWork.ShiftVsEmployeeRepository.Insert(oShiftVsEmployee);
                return _tableWork.SaveChanges() > 0;
            }


            public object AddEmployeeNewRoles(Role RoleDesc)
            {
                bool status = false;
                var result = new
                {
                    Success = false,
                    Message = ""
                };
                if (RoleDesc != null)
                {
                    try
                    {
                        var AlreadyRoleExists = _tableWork.RoleRepository.GetSingle(x => x.Description == RoleDesc.Description);
                        if (AlreadyRoleExists != null)
                        {
                            result = new
                            {
                                Success = false,
                                Message = "Role Already Exists ...!"
                            };
                        }
                        else
                        {
                            Role _role = new Role();
                            _role.CompanyId = 1;
                            _role.Description = RoleDesc.Description;
                            _role.CreatedBy = RoleDesc.CreatedBy;
                            _role.CreatedUtc = DateTime.UtcNow;
                            _role.IsActive = true;
                            _tableWork.RoleRepository.Insert(_role);
                            long dbstatus = _tableWork.SaveChanges();
                            status = dbstatus > 0;
                            result = new
                            {
                                Success = true,
                                Message = "Role Added Successfully ...!"
                            };
                            // _db.SaveChanges();

                            // return status == true ? "Role Added Successfully ...!" : "Roles already Created";                              
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return result;
            }
            public IEnumerable<Location> GetFullLocations(int contraLocationId)
            {
              
                    var locations = new List<Location>();
                    var location = tow.LocationRepository.GetSingle(x => x.Id == contraLocationId);
                    if (location != null)
                    {
                        locations.Add(location);
                        var city = tow.LocationRepository.GetSingle(x => x.Id == location.ContraLocationId);
                        if (city != null)
                        {
                            locations.Add(city);
                            var state = tow.LocationRepository.GetSingle(x => x.Id == city.ContraLocationId);
                            if (state != null)
                            {
                                locations.Add(state);
                                var country = tow.LocationRepository.GetSingle(x => x.Id == state.ContraLocationId);
                                locations.Add(country);
                            }
                        }
                    }
                    return locations;
              
            }
        public int GetCoreDivisionFileCount(EmployeeResponse employee)
        {
            var startOfYesterday = DateTime.Today.AddDays(-1);
            var endOfYesterday = startOfYesterday.AddDays(1).AddTicks(-1);

            var coreDivision = _tableWork.EmployeeRepository.GetSingle(e => e.CoreDivision == employee.CoreDivision && e.EmployeeId == employee.EmployeeId && e.ReportLeader1.HasValue && e.ReportingLeader2.HasValue && e.IsDeleted == false);
            var fileCount = _tableWork.JobOrderRepository.Count(j => j.DivisionId == employee.CoreDivision && (j.EmployeeId == coreDivision.ReportLeader1 || j.EmployeeId == coreDivision.ReportingLeader2) && j.JobDate >= startOfYesterday && j.JobDate <= endOfYesterday && j.IsDeleted == false);
            return fileCount;
        }
    }
    }




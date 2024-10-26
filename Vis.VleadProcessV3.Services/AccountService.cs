
using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class AccountService
    {
        private ApplicationDbContext context;
        private readonly TableWork _tableWork;
        public AccountService(TableWork tableWork,ApplicationDbContext applicationDbContext)
        {
            this.context = applicationDbContext;
            _tableWork = tableWork;
        }

        public LoginViewModel ExternalLogin(LoginViewModel1 user)
        {
            var ListofUserandCustomer = new LoginViewModel();
            //var Userdata = _tableWork.UserRepository.Get(x => x.Username == user.Username && x.Password == user.Password && x.IsDeleted == false).FirstOrDefault();
            ////var Employeedata
            //Userdata.Employee = _tableWork.EmployeeRepository.Get(x => x.EmployeeId == Userdata.EmployeeId).FirstOrDefault();

            var Userdata = _tableWork.UserRepository.GetAllVal(x => x.Employee).Where(x => x.Username == user.Username && x.Password == user.Password && x.IsDeleted == false).FirstOrDefault();
            if (Userdata != null)
            {
                var customer = new Customer();
                if (Userdata.CustomerId != null)
                {
                    var customerdata = _tableWork.UserRepository.Get(x => x.CustomerId == Userdata.CustomerId && x.IsDeleted == false).FirstOrDefault();
                    customerdata.Customer = _tableWork.CustomerRepository.Get(x => x.Id == customerdata.CustomerId).FirstOrDefault();
                    // _db.Users.Include(x => x.Customer).Where(x => x.CustomerId == Userdata.CustomerId && x.IsDeleted == false).FirstOrDefault();
                    customer = _tableWork.CustomerRepository.Get(x => x.Id == customerdata.CustomerId && x.IsDeleted == false).FirstOrDefault();
                    //  _db.Customers.Where(x => x.Id == customerdata.CustomerId && x.IsDeleted == false).FirstOrDefault();
                }

                var salespersonname = new CustomerContact();
                if (Userdata.CustomerId != null)
                {
                    salespersonname = _tableWork.CustomerContactRepository.Get(x => x.CustomerId == Userdata.CustomerId && x.Email == Userdata.Username).FirstOrDefault();
                    //_db.CustomerContacts.FirstOrDefault(x => x.CustomerId == Userdata.CustomerId && x.Email == Userdata.Username);
                }
                ListofUserandCustomer.Id = Userdata.Id;
                ListofUserandCustomer.Username = Userdata.Username;
                ListofUserandCustomer.Password = Userdata.Password;
                ListofUserandCustomer.Domain = Userdata.Domain;
                ListofUserandCustomer.Roles = Userdata.Roles;
                if (customer != null)
                {
                    ListofUserandCustomer.Approvedby = customer.ApprovedBy;
                }
                ListofUserandCustomer.UserType = Userdata.UserType;
                ListofUserandCustomer.MenuAccess = Userdata.MenuAccess;
                ListofUserandCustomer.EmployeeId = Userdata.EmployeeId;
                ListofUserandCustomer.CustomerId = Userdata.CustomerId;
                ListofUserandCustomer.IsDeleted = Userdata.IsDeleted;
                ListofUserandCustomer.CreatedBy = Convert.ToInt32(Userdata.CreatedBy);
                ListofUserandCustomer.CreatedDate = Userdata.CreatedDate;

                if (salespersonname != null)
                {
                    ListofUserandCustomer.SalesPersonName = salespersonname.ContactName;
                    ListofUserandCustomer.IsAdmin = salespersonname.IsAdmin;
                    ListofUserandCustomer.CCId = salespersonname.Id;
                }
                if (Userdata.Employee != null)
                {
                    ListofUserandCustomer.EmployeeName = Userdata.Employee.EmployeeName;
                }
            }

            return ListofUserandCustomer;
        }
        public User GetUserRoleByEmployeeId(int employeeId)
        {
          
                var user = _tableWork.UserRepository.Get(x => x.EmployeeId == employeeId && x.IsDeleted == false).FirstOrDefault();  //_db.Users.Where(x => x.EmployeeId == employeeId && x.IsDeleted == false).FirstOrDefault();
                return user;
        
        }
        public Object GetDesignation(int EmployeeId)
        {
           
            var Emp = (from E in context.Employees
                       join D in context.Designations on E.DesignationId equals D.Id
                       join DE in context.Departments on E.DepartmentId equals DE.Id
                       where (E.IsDeleted == false && E.EmployeeId == EmployeeId)
                       select new
                       {
                           Designation = D.Description,
                           Department = E.DepartmentId,
                           EmpCode = E.EmployeeCode,//------wellness status
                           EmpName = E.EmployeeName//----wellness status
                       }).FirstOrDefault();
            return Emp;
        }
        public Object GetKRADescription(int EmployeeId)        {
          
            var KraDes = (from E in context.Employees
                          join KRA in context.KravsDesignations on E.DepartmentId equals KRA.DepartmentId
                          join D in context.Designations on KRA.DesignationId equals D.Id
                          where (KRA.KraisDeleted == false && E.EmployeeId == EmployeeId)
                          select new
                          {
                              Description = KRA.Kradescription,
                              Value = KRA.ValueInPercentage
                          }).ToList();
            //var KraDes = (from KRA in db.KRAVsDesignation
            //              join D in db.Designations on KRA.DesignationId equals D.Id
            //              join Dep in db.Departments on KRA.DepartmentId equals Dep.Id
            //              where (KRA.KRAIsDeleted == false)
            //              select new
            //              {
            //                  Description = KRA.KRADescription
            //              }).ToList();
            return KraDes;
        }
        public Object CreateKRAApproval(KRA para)
        {
            string message = "Inserted Sucessfully";
            try
            {

                var Emp = context.Employees.Where(x => x.EmployeeId == para.EmployeeId && x.IsDeleted == false).FirstOrDefault();
                var KraDes = context.KravsDesignations.Where(x => x.DepartmentId == Emp.DepartmentId && x.KraisDeleted == false).FirstOrDefault();
                if (KraDes != null)
                {
                    var K = context.Kraapprovals.Where(x => x.EmployeeId == para.EmployeeId).FirstOrDefault();
                    if (K == null)
                    {
                        Kraapproval ka = new Kraapproval();
                        ka.EmployeeId = para.EmployeeId;
                        ka.DepartmentId = KraDes.DepartmentId;
                        ka.IsApproved = true;
                        ka.Krayear = DateTime.Today.Year;
                        ka.CreatedBy = para.EmployeeId;
                        ka.CreatedUtc = DateTime.UtcNow;
                        context.Kraapprovals.Add(ka);
                        context.SaveChanges();
                    }
                    else
                    {
                        message = "Already Exists";
                    }
                }
            }
            catch (Exception ex)
            {
                throw  ex;
            }
            return message;
        }
        public Object CreateWellnessStatus(WELL para)
        {
            string message = "Inserted Successfully";
            try
            {
             
                var Emp = context.Employees.Where(x => x.EmployeeId == para.EmployeeId && x.IsDeleted == false).FirstOrDefault();
                if (Emp != null)
                {
                    //var welSta = db.WellnessStatus.Where(x =>(x.Monthlystatus == null || x.Monthlystatus != DateTime.Today.Month) && (x.EmployeeId == para.EmployeeId || x.EmployeeId != para.EmployeeId)).ToList();
                    //var welSta = db.WellnessStatus.Where(x =>  x.Monthlystatus != DateTime.Today.Month && (x.EmployeeId == para.EmployeeId || x.EmployeeId != para.EmployeeId)).ToList();
                    var Empcheck = context.WellnessStatuses.Where(x => x.EmployeeId == para.EmployeeId).FirstOrDefault();
                    if (Empcheck == null)
                    {
                        //var Monthcheck = db.WellnessStatus.Where(x => x.Monthlystatus != DateTime.Today.Month).FirstOrDefault();-----old
                        var Monthcheck = context.WellnessStatuses.Where(x => x.Monthlystatus == DateTime.Today.Month && x.EmployeeId == para.EmployeeId).FirstOrDefault();//-----new
                        if (Monthcheck == null)
                        {

                            WellnessStatus well = new WellnessStatus();
                            well.EmployeeId = para.EmployeeId;
                            well.Employeecode = para.EmployeeCode;
                            well.EmployeeName = para.EmployeeName;
                            well.Emobile = para.EMobile;
                            well.Email = para.EMail;
                            well.Stay = para.Stay;
                            well.Transport = para.Transport;
                            well.Aarogya = para.Aarogya;
                            well.Covid = para.Covid;
                            well.Foreigns = para.Foreigns;
                            well.Uforeign = para.UForeign;
                            well.Contact = para.Contact;
                            well.Area = para.Area;
                            well.ContactF = para.ContactF;
                            well.Symptoms = para.Symptoms;
                            well.Family = para.Family;
                            well.CreatedBy = para.EmployeeId;
                            well.CreatedUtc= DateTime.UtcNow;
                            well.Monthlystatus = DateTime.Today.Month;
                            context.WellnessStatuses.Add(well);
                            context.SaveChanges();
                        }
                        else
                        {
                            message = "Already Exists";
                        }
                    }
                    else if (Empcheck != null)
                    {
                        //var Monthcheck = db.WellnessStatus.Where(x => x.Monthlystatus == DateTime.Today.Month).FirstOrDefault();--------old
                        var Monthcheck = context.WellnessStatuses.Where(x => x.Monthlystatus == DateTime.Today.Month && x.EmployeeId == para.EmployeeId).FirstOrDefault();//-----new
                        if (Monthcheck == null)
                        {

                            WellnessStatus well = new WellnessStatus();
                            well.EmployeeId = para.EmployeeId;
                            well.Employeecode = para.EmployeeCode;
                            well.EmployeeName = para.EmployeeName;
                            well.Emobile= para.EMobile;
                            well.Email = para.EMail;
                            well.Stay = para.Stay;
                            well.Transport = para.Transport;
                            well.Aarogya = para.Aarogya;
                            well.Covid = para.Covid;
                            well.Foreigns = para.Foreigns;
                            well.Uforeign = para.UForeign;
                            well.Contact = para.Contact;
                            well.Area = para.Area;
                            well.ContactF = para.ContactF;
                            well.Symptoms = para.Symptoms;
                            well.Family = para.Family;
                            well.CreatedBy = para.EmployeeId;
                            well.CreatedUtc = DateTime.UtcNow;
                            well.Monthlystatus = DateTime.Today.Month;
                            context.WellnessStatuses.Add(well);
                            context.SaveChanges();
                        }
                        if (Monthcheck != null)
                        {

                            message = "Already Exists";
                        }

                    }
                }

                else
                {
                    message = "Already Exists";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return message;
        }
        public class DropdownValues
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public Object GetEmployeeProcess(int EmployeeId)
        {
            var result = new List<DropdownValues>();
           // using (var context = new ApplicationDbContext())
          //  {
                var employeProcess = (from EP in context.EmployeeProcesses
                                      join P in context.Processes on EP.ProcessId equals P.Id

                                      orderby P.Id
                                      select new
                                      {
                                          EP.ProcessId,
                                          P.Name,
                                          P.ShortName,
                                          P.IsActive,
                                          EP.EmployeeId,
                                          P.Id
                                      }).Where(x => x.EmployeeId == EmployeeId);

                foreach (var item in employeProcess)
                {
                    result.Add(new DropdownValues
                    {
                        Id = item.Id,
                        Name = item.Name
                    });
                }
          //  }
            return result;
        }
        public bool PasswordChange(NewPassword newPassword)
        {
            bool status = false;
            var existingUser = _tableWork.UserRepository.Get(x => x.Id == newPassword.UserId).FirstOrDefault();
            if (existingUser.Password == newPassword.OldPassword)
            {
                existingUser.Password = newPassword.LatestPassword;
                _tableWork.UserRepository.Update(existingUser);
                //_db.Entry(existingUser).State = EntityState.Modified;
                status = _tableWork.SaveChanges() > 0;
            }
            return status;
        }
    }
    }

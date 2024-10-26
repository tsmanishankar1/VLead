using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;
using Vis.VleadProcessV3.ViewModels;

namespace Vis.VleadProcessV3.Services
{
    public class UserService
    {
        private readonly TableWork _tableWork;
        private readonly ViewWork _viewWork;
       
        private readonly TableWork tow;
        public UserService(TableWork tableWork,ViewWork viewWork)
        {
          
            tow=tableWork;
            _tableWork = tableWork;
            _viewWork = viewWork;
        }
        public IEnumerable<UserViewModel> UserDetails()
        {
            return _viewWork.ViewGetUserRepository.Get(x => x.IsDeleted == false).Select(x => new UserViewModel { Id = x.Id, UserTypeDesc = x.UserTypeDesc, Username = x.Username, UserType = x.UserType, Roles = x.Roles });
        }
        public IEnumerable<EmployeeUserViewModel> GetAllEmployeesDetails()
        {
            var getuseremployeeid = _tableWork.UserRepository.Get().Select(c => c.EmployeeId).ToList();
            return _tableWork.EmployeeRepository.Get(x => !getuseremployeeid.Contains(x.EmployeeId)).Where(x => x.IsDeleted == false && x.DateOfResignation == null && x.ResignReasons == null).Select(x => new EmployeeUserViewModel { EmployeeId = x.EmployeeId, EmployeeName = x.EmployeeName }).ToList();
        }
        public IEnumerable<Employee> GetAllEmployees()
        {
            return _tableWork.EmployeeRepository.Get(x => x.IsDeleted == false && x.ResignReasons == null && x.DateOfResignation == null);
        }
        public IEnumerable<CustomerViewModel> GetAllCustomers()
        {
            return _tableWork.CustomerRepository.Get(x => x.IsDeleted == false && x.IsBlacklisted == false).Select(x => new CustomerViewModel { Id = x.Id, ShortName = x.ShortName, Name = x.Name });
        }
        public bool SaveUser(User user, int actionType)
        {
            bool status = false;
            if (actionType == 1)
            {
                user.CreatedDate = DateTime.UtcNow;
                _tableWork.UserRepository.Insert(user);
            }
            else if (actionType == 2)
            {
                var updateUser = _tableWork.UserRepository.GetSingle(x => x.Id == user.Id);
                updateUser.Username = user.Username;
                updateUser.Password = user.Password;
                updateUser.Roles = user.Roles;
                updateUser.MenuAccess = user.MenuAccess;
                updateUser.UserType = user.UserType;
                updateUser.CreatedDate = DateTime.UtcNow;
                updateUser.CreatedBy = user.CreatedBy;
                updateUser.IsDeleted = false;
                _tableWork.UserRepository.Update(updateUser);
            }
            long dbStatus = _tableWork.SaveChanges();
            status = dbStatus > 0;
            return status;
        }
        public Employee GetEmployeeCodeByEmployeeId(int employeeid)
        {
            return _tableWork.EmployeeRepository.GetSingle(x => x.EmployeeId == employeeid);
        }
        public IEnumerable<CustomerEmailUserViewModel> GetCustomerContactByCustomerId(int Id)
        {
         
            
                var result = tow.CustomerContactRepository.Get(x => x.CustomerId == Id && x.IsDeleted == false).Select(x => new CustomerEmailUserViewModel { CustomerId = x.CustomerId, Email = x.Email, IsDeleted = x.IsDeleted }).ToList().Where(x => (!tow.UserRepository.Exist(u => (u.Username == x.Email && x.IsDeleted == false)))).OrderBy(x => x.Email);
                return result;
            
        }
        public void DeleteUser(User deleteuser)
        {
            User getid = _tableWork.UserRepository.GetSingle(x => x.Id == deleteuser.Id);
            getid.IsDeleted = true;
            _tableWork.UserRepository.Update(getid);
            _tableWork.SaveChanges();
        }
        public User GetUserById(int Id)
        {
            return _tableWork.UserRepository.GetSingle(x => x.Id == Id);
        }
        public IEnumerable<Menu> GetMenu()
        {
            return _tableWork.MenuRepository.Get(x => x.IsDeleted == false).AsQueryable();
        }
    }
}

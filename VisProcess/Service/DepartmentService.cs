using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Services;

namespace VisProcess.Service
{
    public class DepartmentService
    {
    
        private readonly EmployeeService _employeeService;
        public DepartmentService(EmployeeService employeeService)
        {
           
            _employeeService=employeeService;
        }
        public Department GetDepartment(int departmentId)
        {
            return _employeeService.GetDepartment(departmentId);
        }
    }
}

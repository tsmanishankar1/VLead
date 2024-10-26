namespace Vis.VleadProcessV3.ViewModels
{
    public class ScopeViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string DepartmentDescription { get; set; }
    }
    public class UserViewModel
    {
        public int Id { get; set; }
        public string UserTypeDesc { get; set; }
        public string Username { get; set; }
        public string UserType { get; set; }
        public string Roles { get; set; }
    }
    public class EmployeeUserViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }
    public class CustomerUserViewModel
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
    }

    public class CustomerEmailUserViewModel
    {
        public int CustomerId { get; set; }
        public string Email { get; set; }
        public bool? IsDeleted { get; set; }
    }
    //public class CustomerViewModel
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string ShortName { get; set; }
    //}
}

namespace Vis.VleadProcessV3.ViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public Nullable<int> Approvedby { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public string Roles { get; set; }
        public string UserType { get; set; }
        public string MenuAccess { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string EmployeeName { get; set; }
        public string SalesPersonName { get; set; }
        public Nullable<bool> IsAdmin { get; set; }
        public Nullable<bool> IsAdmin123 { get; set; }
        public Nullable<int> CCId { get; set; }
    }

    public class LoginViewModel1
    {
  
        public string Username { get; set; }
       
        public string Password { get; set; }
      
    }

}

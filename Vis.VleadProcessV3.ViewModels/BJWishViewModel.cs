namespace Vis.VleadProcessV3.ViewModels
{
    public class BJWishViewModel
    {

    }
    public class DOBWishViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public int DesignationId { get; set; }
        public string Designation { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string Code { get; set; }
    }
    public class DOJWishViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public int DesignationId { get; set; }
        public string Designation { get; set; }
        public System.DateTime DateOfJoining { get; set; }
        public int Anniversary { get; set; }
        public string Code { get; set; }
    }
}

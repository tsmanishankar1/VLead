namespace Vis.VleadProcessV3.ViewModels
{
    public class AddEmployee
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string BloodGroup { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public int CompanyId { get; set; }
        public int ProfiencyId { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNo { get; set; }
        public string Email { get; set; }
        public string? WorkingStatus { get; set; }
        public string PersonalEmail { get; set; }
        public DateTime CreatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedUTC { get; set; }
        public int UpdatedBy { get; set; }
        public int ReportingManager1 { get; set; }
        public int ReportLeader1 { get; set; }
        public int ReportingManager2 { get; set; }
        public int ReportingLeader2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address11 { get; set; }
        public string Address22 { get; set; }
        public string Address33 { get; set; }
        public int LocationId { get; set; }
        public int LocationId1 { get; set; }
        public string AddressType { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public int ResignReasons { get; set; }
        public DateTime? DateOfResignation { get; set; }
        //public int[] ProcessId { get; set; }
        public int[] ProcessCode { get; set; }
        //public int[] RoleId { get; set; }
        //public int[] RolesCode { get; set; }
        public bool result { get; set; }
        public string RoleDescription { get; set; }
        public bool IsOutsource { get; set; }
        public List<EmployeeRoles> EmpRolesList { get; set; }
        public List<EmpHierarchy> EmpHierarchyList { get; set; }
        //--------------------------------------vidhya live now---------------------------------------
        public string IsInternetConnection { get; set; }
        public string IsSystem { get; set; }
        public string NetWorkType { get; set; }
        public string ServiceProvider { get; set; }
        public string SystemConfig { get; set; }
        public int CoreDivision { get; set; }
        public string? BatchNo { get; set; }
        //--------------------------------------vidhya live now---------------------------------------

    }
    public class AddEmployee1
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string BloodGroup { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public int CompanyId { get; set; }
        public int ProfiencyId { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNo { get; set; }
        public string Email { get; set; }
        public string PersonalEmail { get; set; }
        public DateTime CreatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedUTC { get; set; }
        public int UpdatedBy { get; set; }
        public int ReportingManager1 { get; set; }
        public int ReportLeader1 { get; set; }
        public int ReportingManager2 { get; set; }
        public int ReportingLeader2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address11 { get; set; }
        public string Address22 { get; set; }
        public string Address33 { get; set; }
        public int LocationId { get; set; }
        public int LocationId1 { get; set; }
        public string AddressType { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public int ResignReasons { get; set; }
        public DateTime? DateOfResignation { get; set; }
        //public int[] ProcessId { get; set; }
        //public int[] ProcessCode { get; set; }
        //public int[] RoleId { get; set; }
        //public int[] RolesCode { get; set; }
        public bool result { get; set; }
        public string RoleDescription { get; set; }
        public bool IsOutsource { get; set; }
        public bool IsDeleted { get; set; }
        public int? Id { get; set; }
        public string DepartmentDesc { get; set; }
        public int? DesigID { get; set; }
        public string DesignationDesc { get; set; }
        public int? ProfID { get; set; }
        public string ProfiencyDesc { get; set; }
        public string ManagerName1 { get; set; }
        public string LeaderName1 { get; set; }
        public string ManagerName2 { get; set; }
        public string LeaderName2 { get; set; }
        public List<EmployeeRoles> EmpRolesList { get; set; }
        public List<EmpHierarchy> EmpHierarchyList { get; set; }
    }
    public class SearchEmployeeinShift
    {
        public string EmployeeCode { get; set; }
        public int ShiftId { get; set; }
    }

    public class EmployeeRoles
    {
        public string RoleDescription { get; set; }
        public int RoleId { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class EmpHierarchy
    {
        public int SubEmpId { get; set; }
        public string SubEmpName { get; set; }
        public int CreatedBy { get; set; }
    }
}

namespace Vis.VleadProcessV3.Models
{
    public class GetViewEmployeeDetails
    {
        public int? EmployeeId { get; set; }
        public int CmpID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public int ProfiencyId { get; set; }
        public Nullable<int> ReportLeader1 { get; set; }
        public Nullable<int> ReportingManager1 { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public System.DateTime DateOfJoining { get; set; }
        public Nullable<System.DateTime> DateOfResignation { get; set; }
        public Nullable<int> ResignReasons { get; set; }
        public string Email { get; set; }
        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string BloodGroup { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNo { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public string DepartmentDesc { get; set; }
        public int DesigID { get; set; }
        public string DesignationDesc { get; set; }
        public int ProfID { get; set; }
        public string ProfiencyDesc { get; set; }
        public string ManagerName1 { get; set; }
        public string LeaderName1 { get; set; }
        public Nullable<int> ReportingManager2 { get; set; }
        public string ManagerName2 { get; set; }
        public Nullable<int> ReportingLeader2 { get; set; }
        public string LeaderName2 { get; set; }
        public bool IsResigned { get; set; }
        public int Id { get; set; }
        public int ShiftemployeeId { get; set; }
        public Nullable<int> ShiftId { get; set; }
        public string EffectFrom { get; set; }
        public string EffectTo { get; set; }
        public bool ShiftIsDeleted { get; set; }
        public bool Result { get; set; }
        public string Description { get; set; }
        public string ShiftType { get; set; }
        public int CoreDivision { get; set; }
    }
    public class GetViewEmployeeVsShiftDetails
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public Nullable<int> ShiftVsEmployeeId { get; set; }
        public Nullable<int> Id { get; set; }
        public Nullable<int> ShiftId { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> EffectiveFrom { get; set; }
        public Nullable<System.DateTime> EffectiveTo { get; set; }
        public bool Result { get; set; }
    }
}

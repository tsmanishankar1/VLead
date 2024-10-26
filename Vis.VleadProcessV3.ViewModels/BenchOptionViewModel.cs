namespace Vis.VleadProcessV3.ViewModels
{
    public class EmployeewithShift
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int DepartmentId { get; set; }
        public string ShiftName { get; set; }
        public string EmployeeDesc { get; set; }
        public string ShiftType { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string Description { get; set; }
        public DateTime StartDatetime { get; set; }
        public DateTime EndDatetime { get; set; }
        public bool IsShift { get; set; }
        public int[] Shiftid { get; set; }
    }
    public class BenchOptionViewModel
    {
        public int[] EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FileName { get; set; }
        public string EmployeeName { get; set; }
        public int? TotalTimeTaken { get; set; }
        public string ProcessName { get; set; }
        public string Status { get; set; }
        public int DepartmentId { get; set; }
        public string ShiftName { get; set; }
        public string EmployeeDesc { get; set; }
        public IEnumerable<BenchOptionViewModel> SelectedEmployees { get; set; }
        public int CreatedBy { get; set; }
        public DateTime StartDatetime { get; set; }
        public DateTime EndDatetime { get; set; }
        public int? BenchType { get; set; }
        public int? TrainingOn { get; set; }
        public int ClientId { get; set; }
        public string Remarks { get; set; }
        public string ReportType { get; set; }
        public DateTime AllocattedDate { get; set; }
    }
    public class BenchStatusViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string DepartmentName { get; set; }
    }

}

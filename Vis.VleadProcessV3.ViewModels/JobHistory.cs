namespace Vis.VleadProcessV3.ViewModels
{
    public class JobHistory
    {
        public int? ProcessId { get; set; }
        public string ProcessName { get; set; }
        public int? PreviousProcessId { get; set; }
        public string PreviousProcessName { get; set; }
        public DateTime CreatedUTC { get; set; }
        public int? CreatedBy { get; set; }
        public int? EmployeeId { get; set; }
        public int? UpdatedBy { get; set; }
        public string EmployeeName { get; set; }
        public string WorkingName { get; set; }
        public string Remarks { get; set; }
        public string Comments { get; set; }
        public int TranId { get; set; }
        public int StatusId { get; set; }
        public int DepartmentId { get; set; }
        public string FileFolderPath { get; set; }
        public virtual ICollection<JobHistory> JobQuery { get; set; }
    }
}

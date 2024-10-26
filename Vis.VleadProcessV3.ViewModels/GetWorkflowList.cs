namespace Vis.VleadProcessV3.ViewModels
{
    public class GetWorkflowList
    {
        public int Id { get; set; }
        public int WFMId { get; set; }
        public Nullable<int> EstimatedTime { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> TotalTimeTaken { get; set; }
        public Nullable<int> DeviationTime { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string WorkflowTypeDescription { get; set; }
        public string TimeTaken { get; set; }
        public int Worked { get; set; }
        public int Break { get; set; }
        public int TrainingorMeeting { get; set; }
        public int Hold { get; set; }
        public int Others { get; set; }
        public int TotalTime { get; set; }
        public int Deviation { get; set; }
        public int EstTime { get; set; }
        public string EmployeeName { get; set; }
    }
}

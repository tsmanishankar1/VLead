namespace Vis.VleadProcessV3.ViewModels
{
    public class ClientStatusReportViewModel
    {
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public int[] CustomerId { get; set; }
        public String department { get; set; }
    }


    public class GetRemainderReportData
    {
        public Nullable<DateTime> Date { get; set; }
        //public Nullable<int> RemainderDays { get; set; }
        public string[] customerId { get; set; }

        public IEnumerable<GetRemainderReportData> RemainderReport { get; set; }
    }
    public class GetRemainderReportData1
    {
        public Nullable<DateTime> Date { get; set; }
        //public Nullable<int> RemainderDays { get; set; }
        public string[] customerId { get; set; }

       
    }
}

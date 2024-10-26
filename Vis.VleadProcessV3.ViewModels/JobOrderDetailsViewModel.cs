namespace Vis.VleadProcessV3.ViewModels
{
    public class JobOrderDetailsViewModel
    {
        public int ClientId { get; set; }
        public int DepartmentId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
    public class NonBillablePara
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int ClientId { get; set; }
        public int DepartmentId { get; set; }
    }
    public class NBJIdPara
    {
        public int Id { get; set; }
        public int NonBillableId { get; set; }
        public IEnumerable<NBJIdPara> GetNBPara { get; set; }
    }
    public class CreditDaysOutHighVM
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<DateTime> InvoiceDate { get; set; }
        public int CustomerID { get; set; }
        public int TotalDays { get; set; }
    }
}

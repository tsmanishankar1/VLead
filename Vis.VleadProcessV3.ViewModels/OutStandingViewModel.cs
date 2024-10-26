namespace Vis.VleadProcessV3.ViewModels
{
    public class OutStandingViewModel
    {
        public bool IsDeleted { get; set; }
        public bool IsOutstanding { get; set; }
        public Nullable<int> Id { get; set; }
        public Nullable<int> IMID { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? SalesDate { get; set; }
        public DateTime? CollectionDate { get; set; }
        public string CustomerName { get; set; }
        public string ShortName { get; set; }
        public string CustomerwithShortName { get; set; }
        public decimal? InvoiceValue { get; set; }
        public decimal? TotalInvoiceValue { get; set; }
        public decimal? AmountCollected { get; set; }
        public decimal OutstandingAmount { get; set; }
        public long OutstandingDay { get; set; }
        public int CustomerId { get; set; }
        public DateTime? ReportAsOn { get; set; }
        public decimal FirstQuater { get; set; }
        public decimal SecondQuater { get; set; }
        public decimal ThirdQuater { get; set; }
        public decimal FoureQuater { get; set; }
        public decimal FiveQuater { get; set; }
        public Nullable<decimal> outstandingAmount { get; set; }
        public string Status { get; set; }
        public virtual ICollection<OutStandingViewModel> sampleCollection { get; set; }
        public decimal Firstq { get; set; }
        public decimal Second { get; set; }
        public decimal Third { get; set; }
        public decimal Fourth { get; set; }
        public decimal Fifth { get; set; }
        public decimal Total { get; set; }
        public Nullable<int> VleadSalesPersonId { get; set; }  //
        public Nullable<bool> VleadSalesPersonIsActive { get; set; } //
    }
    public class CustomerIds
    {
        public int Id { get; set; }
    }
    public class OSCustomerIds
    {
        public int Id { get; set; }
    }
    public class SelectedCustomers
    {
        public int[] Ids { get; set; }
        public string ReportAsOn { get; set; }
    }
}

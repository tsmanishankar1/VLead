namespace Vis.VleadProcessV3.Models
{
    public partial class View_BenchScheduler
    {
        public int Id { get; set; }
        public System.DateTime FromDateTime { get; set; }
        public Nullable<System.DateTime> ToDateTime { get; set; }
        public string Status { get; set; }
        public string ShortName { get; set; }
        public string ScheduledTo { get; set; }
        public string ScheduledBy { get; set; }
        public string BenchType { get; set; }
        public string TrainingOn { get; set; }
    }
    public partial class View_GetDropInvoiceDetailTab
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
    }
    public partial class View_GetDropInvoiceIntegration
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
    }
    public partial class View_OutStandingList
    {
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public decimal InvoiceValue { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public Nullable<decimal> AdjustmentAmount { get; set; }
        public System.DateTime CollectionDate { get; set; }
        public decimal CollectionAmount { get; set; }
        public int CustomerId { get; set; }
        public bool IsDeleted { get; set; }
    }
    public partial class View_InvoiceMater
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<decimal> RoundOff { get; set; }
        public Nullable<decimal> Waiver { get; set; }
        public decimal Discount { get; set; }
        public Nullable<decimal> TotalInvoiceValue { get; set; }
    }
    public partial class View_GetIntegrationMaster
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<System.DateTime> ESTInvoiceDate { get; set; }
        public System.DateTime ReferenceDate { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> ProductValue { get; set; }
        public decimal Discount { get; set; }
        public decimal InvoiceValue { get; set; }
        public System.DateTime CreatedUTC { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
        public Nullable<System.DateTime> PrimaryDateTime { get; set; }
        public Nullable<System.DateTime> SecondaryDateTime { get; set; }
        public Nullable<int> BillingCycleId { get; set; }
        public Nullable<decimal> ArtInvoiceAmount { get; set; }
        public Nullable<decimal> DigiInvoiceAmount { get; set; }
        public Nullable<decimal> RoundOff { get; set; }
        public Nullable<decimal> ArtWaiver { get; set; }
        public Nullable<decimal> DigiWaiver { get; set; }
        public Nullable<decimal> Waiver { get; set; }
        public Nullable<decimal> ReceivableValue { get; set; }
        public Nullable<decimal> TotalInvoiceValue { get; set; }
        public Nullable<int> TransactionId { get; set; }
    }
    public partial class View_GetGeneratingInvoice
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<int> BillingCycleId { get; set; }
        public decimal Discount { get; set; }
        public decimal InvoiceValue { get; set; }
        public int CustomerID { get; set; }
        public int TranId { get; set; }
        public string JobId { get; set; }
        public int IMId { get; set; }
        public int JId { get; set; }
        public int PricingTypeId { get; set; }
        public Nullable<int> ScopeId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public string Department { get; set; }
        public string FileName { get; set; }
        public Nullable<System.DateTime> ESTFileReceivedDate { get; set; }
        public Nullable<System.DateTime> ESTDateofUpload { get; set; }
        public Nullable<int> EstimatedTime { get; set; }
        public Nullable<long> StitchCount { get; set; }
        public string JobStatusDescription { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string ScopeDesc { get; set; }
    }
    
}

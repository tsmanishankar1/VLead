namespace Vis.VleadProcessV3.ViewModels
{
    public class AlreadyAdjustedModel
    {
        public int? InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public decimal AlreadyAdjustedAmount { get; set; }
        public decimal? InvoiceValue { get; set; }
        public decimal TotalInvoiceValue { get; set; }
        public decimal? CurrentAdjustedAmount { get; set; }
        public bool IsInvoiceAdjustment { get; set; }
        public DateTime InvoiceDate { get; set; }
    }
}

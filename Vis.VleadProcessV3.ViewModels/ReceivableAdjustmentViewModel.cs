using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.ViewModels
{
    
    public class ReceivableAdjustViewModel1
    {
        public Nullable<int> InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<decimal> AdjustmentAmount { get; set; }
    }
        public class ReceivableAdjustViewModel
    {
        public int Id { get; set; }
        public int ReceivableId { get; set; }
        public bool IsInvoiceAdjustment { get; set; }
        public Nullable<int> InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<decimal> AdjustmentAmount { get; set; }
        public string ReferenceNo { get; set; }
        public Nullable<decimal> AvailableAdvance { get; set; }
        public Nullable<bool> IsAvailableAdvance { get; set; }
        public string CRDR { get; set; }

        public virtual InvoiceMaster InvoiceMaster { get; set; }
        public virtual Receivable Receivable { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.ViewModels
{
    public class ReceivableAdjustmentModel
    {
        public int Id { get; set; }
        public int ReceivableId { get; set; }
        public bool IsInvoiceAdjustment { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<decimal> AdjustmentAmount { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal? InvoiceValue { get; set; }
        public decimal TotalInvoiceValue { get; set; }
        public string ReferenceNo { get; set; }
        public string CRDR { get; set; }
    }
}

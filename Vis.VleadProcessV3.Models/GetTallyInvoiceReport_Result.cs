using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class GetTallyInvoiceReport_Result
    {
        public int Id { get; set; }
        public string? InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? ESTInvoiceDate { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public int? CustomerID { get; set; }
        public string? ShortName { get; set; }
        public string? Description { get; set; }
        public decimal? ProductValue { get; set; }
        public decimal? Discount { get; set; }
        public decimal? InvoiceValue { get; set; }
        public DateTime? CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public DateTime? PrimaryDateTime { get; set; }
        public DateTime? SecondaryDateTime { get; set; }
        public int? BillingCycleId { get; set; }
        public decimal? ArtInvoiceAmount { get; set; }
        public decimal? DigiInvoiceAmount { get; set; }
        public decimal? RoundOff { get; set; }
        public decimal? ArtWaiver { get; set; }
        public decimal? DigiWaiver { get; set; }
        public decimal? Waiver { get; set; }
        public decimal? ReceivableValue { get; set; }
        public decimal? TotalInvoiceValue { get; set; }
        public int? TransactionId { get; set; }
        public decimal? ExchangeRate { get; set; }
        public string? PaymentMode { get; set; }
        public decimal? FinalAmount { get; set; }
    }
}
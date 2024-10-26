using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.ViewModels
{
    public class ReceivableModel
    {
        public IEnumerable<ReceivableExt> ReceivableExts { get; set; }
        public IEnumerable<ReceivableAdjustment> ReceivableAdjustments { get; set; }
        public Receivable Receivables { get; set; }
        public IEnumerable<AlreadyAdjustedModel> AlreadyAdjusted { get; set; }



        public int Id { get; set; }
        public string? VoucherNo { get; set; } = null;
        public DateTime? CollectionDate { get; set; }
        public decimal CollectionAmount { get; set; }
        public string? ReferenceNo { get; set; } = null;
        public DateTime? ReferenceDate { get; set; }
        public string? Description { get; set; } = null;
        public string? CustomerName { get; set; } = null;
        public string? CustomerShortName { get; set; } = null;
        public bool IsDeleted { get; set; }
        public DateTime? CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedBy { get; set; }
        public int TransactionId { get; set; }
        public int CustomerId { get; set; }
        public DateTime? RemovalReasonId { get; set; }

        public Nullable<int> InvoiceId { get; set; }
        public string? InvoiceNo { get; set; } = null;
        public Nullable<decimal> AdjustmentAmount { get; set; }
    }


    public class ReceivableModel1
    {
        public IEnumerable<ReceivableAdjustment> ReceivableAdjustments { get; set; }
        public Receivable1 Receivables { get; set; }
        public IEnumerable<AlreadyAdjustedModel> AlreadyAdjusted { get; set; }
    }
    public class Receivableinteglist
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public string VoucherNo { get; set; }
        public System.DateTime CollectionDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerShortName { get; set; }
        public string ShortName { get; set; }
        public decimal CollectionAmount { get; set; }
        public string DestinationBank { get; set; }
        public string ReceiptMode { get; set; }
        public string TransactionNo { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string BankName { get; set; }
        public System.DateTime CreatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<System.DateTime> ESTInvoiceDate { get; set; }
        public IEnumerable<Receivableinteglist> Recinteglist { get; set; }
        //public string Description { get; set; }
        //public bool IsDeleted { get; set; }        
        public int EmployeeId { get; set; }
    }


    public class GetReceiptsPara
    {
        public int CustomerID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}

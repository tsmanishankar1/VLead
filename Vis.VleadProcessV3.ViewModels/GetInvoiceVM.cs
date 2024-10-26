using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.ViewModels
{
    public class GetInvoiceVM
    {
        public GetInvoiceVM()
        {
            GetInvoice=new List<GetInvoiceVM>();
        }
        public string JobId { get; set; }
        public string ShortName { get; set; }
        public Nullable<int> ScopeId { get; set; }
        public string ScopeDesc { get; set; }
        public int ClientId { get; set; }
        public string BillingCycleType { get; set; }
        public DateTime DateofUpload { get; set; }
        public int CreatedBy { get; set; }
        public int DepartmentId { get; set; }
        public int TranId { get; set; }
        public int Id { get; set; }
        public int JId { get; set; }
        public int PricingTypeId { get; set; }
        public IEnumerable<GetInvoiceVM> GetInvoice { get; set; }
        public DateTime FileReceivedDate { get; set; }
        public Nullable<bool> IsBillable { get; set; }
        public decimal SpecialPrice { get; set; }
        public Nullable<int> EstimatedTime { get; set; }
        public Nullable<bool> IsWaiver { get; set; }
        public Nullable<int> JobStatusId { get; set; }
       
    }
    public class GetJobsforInvoiceVM1
    {
        public int ClientId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
        public class GetJobsforInvoiceVM
    {
        public int ClientId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public IEnumerable<GetJobsforInvoiceVM> GetInvoice { get; set; }
    }
    public class GetInvoiceDetailVM1
    {
      
        public string? InvoiceNo { get; set; }
        public decimal? Discount { get; set; }
        public Nullable<decimal> TotalInvoiceValue { get; set; }
    }
        public class GetInvoiceDetailVM
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
        public IEnumerable<GetInvoiceDetailVM> invoicenumber { get; set; }
    }
    public class invoicesalecancel1
    {
        public int? Id { get; set; }
        public string? InvoiceNo { get; set; }

    }
    public class invoicesalecancel
    {
        public int Id { get; set; }
        public int CustomerID { get; set; }
        public int EmployeeId { get; set; }
        public string InvoiceNo { get; set; }
        public IEnumerable<invoicesalecancel> invoicesc { get; set; }
    }
    public class integrationxchangerate
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public IEnumerable<integrationxchangerate> invintigxchange { get; set; }
    }
    public class invoiceintiglist
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
        public int CreatedBy { get; set; }
        public int EmployeeId { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public IEnumerable<invoiceintiglist> invintiglist { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
        public Nullable<System.DateTime> PrimaryDateTime { get; set; }
        public Nullable<System.DateTime> SecondaryDateTime { get; set; }
        public Nullable<decimal> ArtInvoiceAmount { get; set; }
        public Nullable<decimal> DigiInvoiceAmount { get; set; }
        public Nullable<decimal> RoundOff { get; set; }
        public Nullable<decimal> ArtWaiver { get; set; }
        public Nullable<decimal> DigiWaiver { get; set; }
        public Nullable<decimal> Waiver { get; set; }
        public Nullable<decimal> ReceivableValue { get; set; }
        public Nullable<decimal> TotalInvoiceValue { get; set; }
        public int TransactionId { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
    }
    public class integrationmasterpara
    {
        public int CustomerID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
    public class waiverpara
    {
        public int DepartmentId { get; set; }
        public Nullable<int> ClientId { get; set; }
        public string FileName { get; set; }
        public Nullable<System.DateTime> DateofUpload { get; set; }
        public Nullable<System.DateTime> JobClosedUTC { get; set; }
    }
    public class ReturnCollectionVM
    {
        public int JId { get; set; }
        public int PricingJobCount { get; set; }
        public int JobStatusId { get; set; }
        public IEnumerable<ReturnCollectionVM> GetReturnCollection { get; set; }
    }
}

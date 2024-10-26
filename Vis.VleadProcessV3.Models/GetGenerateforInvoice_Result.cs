using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Vis.VleadProcessV3.Models
{
    public partial class GetGenerateforInvoice_Result
    {
        public int Id { get; set; }
        public int JId { get; set; }
        public string? JobId { get; set; }
        public Nullable<System.DateTime> ESTJobDate { get; set; }
        public int DepartmentId { get; set; }
        public Nullable<int> ClientId { get; set; }
        public System.DateTime FileReceivedDate { get; set; }
        public Nullable<System.DateTime> ESTFileReceivedDate { get; set; }
        public Nullable<System.DateTime> ESTDateofUpload { get; set; }
        public string? FileName { get; set; }
        public Nullable<System.DateTime> DateofUpload { get; set; }
        public Nullable<bool> IsBillable { get; set; }
        public Nullable<decimal> SpecialPrice { get; set; }
        public Nullable<int> EstimatedTime { get; set; }
        public Nullable<int> ScopeId { get; set; }
        public Nullable<long> StitchCount { get; set; }
        public string? JobStatusDescription { get; set; }
        public string? ProjectCode { get; set; }
        public string? Department { get; set; }
        public string? ShortName { get; set; }
        public string? BillingCycleType { get; set; }
        public string? Description { get; set; }
        public string? NonBillable { get; set; }
    }

    public partial class GeneratedInvoice_Result
    {
        public string? InvoiceNo { get; set; }
        public Nullable<decimal> ProductValue { get; set; }
        [Key]
        public int TranId { get; set; }
        public int Id { get; set; }
        public Nullable<int> BillingCycleId { get; set; }
        public Nullable<int> EstimatedTime { get; set; }
        public string? Description { get; set; }
        public string? Department { get; set; }
        public string? ProjectCode { get; set; }
        public string? JobStatusDescription { get; set; }
        public Nullable<long> StitchCount { get; set; }
        public string? ShortName { get; set; }
        public string? ScopeDesc { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public decimal Discount { get; set; }
        public Nullable<System.DateTime> ESTDateofUpload { get; set; }
        public Nullable<System.DateTime> ESTJobDate { get; set; }
        public Nullable<System.DateTime> ESTFileReceivedDate { get; set; }
        public string? FileName { get; set; }
        public System.DateTime FileReceivedDate { get; set; }
        public Nullable<decimal> SpecialPrice { get; set; }
        public Nullable<int> ScopeId { get; set; }
        public int ClientId { get; set; }
        public decimal InvoiceValue { get; set; }
        public int PricingTypeId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string? JobId { get; set; }
        public int JId { get; set; }
        public Nullable<bool> IsWaiver { get; set; }
    }
    [Keyless]
    public partial class GetStaffingAmount_Result
    {
        public string? ShortName { get; set; }
        public int ClientId { get; set; }
        public int PricingTypeId { get; set; }
        public Nullable<decimal> Value { get; set; }
    }
    [Keyless]
    public partial class GetCheckStaffing_Result
    {
        public int CustomerId { get; set; }
        public int DepartmentId { get; set; }
        public int PricingTypeId { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string? Designation { get; set; }
        public Nullable<System.DateTime> WEFromDate { get; set; }
        public Nullable<System.DateTime> WEToDate { get; set; }
    }
    public partial class GetPricingforTandM_Result
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int DepartmentId { get; set; }
        public Nullable<int> ScopeId { get; set; }
        public int PricingTypeId { get; set; }
        public Nullable<int> From { get; set; }
        public Nullable<int> To { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> RatePerHour { get; set; }
        public string? ScopeTempDesc { get; set; }
        public Nullable<System.DateTime> WEFromDate { get; set; }
        public Nullable<System.DateTime> WEToDate { get; set; }
        public Nullable<int> JobStatusId { get; set; }
    }
    public partial class GetRateforTandM_Result
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int DepartmentId { get; set; }
        public Nullable<int> ScopeId { get; set; }
        public int PricingTypeId { get; set; }
        public Nullable<decimal> RatePerHour { get; set; }
        public string? ScopeTempDesc { get; set; }
        public Nullable<System.DateTime> WEFromDate { get; set; }
        public Nullable<System.DateTime> WEToDate { get; set; }
        public Nullable<int> JobStatusId { get; set; }
    }
    public partial class GetDropInvoiceCancel_Result
    {
        public int Id { get; set; }
        public string? InvoiceNo { get; set; }
        public string? ShortName { get; set; }
    }
    [Keyless]
    public partial class GetTranMergeInvoiceCancel_Result
    {
        public int IMId { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> Value { get; set; }
        public int PricingTypeId { get; set; }
        public string? PricingType { get; set; }
        public Nullable<int> ScopeId { get; set; }
        public string? Scope { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string? InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
    }
    public partial class GetIntegrationMaster_Result
    {
        public int Id { get; set; }
        public string? InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<System.DateTime> ESTInvoiceDate { get; set; }
        public System.DateTime ReferenceDate { get; set; }
        public int CustomerID { get; set; }
        public string? ShortName { get; set; }
        public string? Description { get; set; }
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
        public Nullable<decimal> ExchangeRate { get; set; }
        public string? PaymentMode { get; set; }
    }
    [Keyless]
    public partial class GetIntegrationTran_Result
    {
        public int IMId { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> Value { get; set; }
        public int PricingTypeId { get; set; }
        public Nullable<int> ScopeId { get; set; }
        public string? Description { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string? InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
    }
    [Keyless]
    public partial class GetReceiptsIntegrationTran_Result
    {
        public int? ReceivableId { get; set; }
        public Nullable<decimal> AdjustmentAmount { get; set; }
        public string? InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public int? InvoiceId { get; set; }
        public bool? IsInvoiceAdjustment { get; set; }
        public bool? IsAvailableAdvance { get; set; }
        public bool? IsTallyIntegrated { get; set; }

    }
    public partial class WaiverJobforInvoice_Result
    {
        public System.DateTime JobDate { get; set; }
        public string? JobId { get; set; }
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? ShortName { get; set; }
        public Nullable<bool> IsWaiver { get; set; }
        public Nullable<bool> IsSpecialPrice { get; set; }
        public string? Description { get; set; }
        public string? JobStatusDescription { get; set; }
        public string? Department { get; set; }
        public Nullable<System.DateTime> DateofUpload { get; set; }
    }
    public partial class SP_SingleTableDPR_Result
    {
        public int Id { get; set; }
        public Nullable<int> TDMId { get; set; }
        public Nullable<int> JId { get; set; }
        public string? JobId { get; set; }
        public Nullable<System.DateTime> JobDate { get; set; }
        public string? FileName { get; set; }
        public Nullable<int> WFMId { get; set; }
        public Nullable<int> ClientId { get; set; }
        public string? ClientShortName { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string? Department { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string? Process { get; set; }
        public string? Status { get; set; }
        public string? CurrentTray { get; set; }
        public string? CurrentStatus { get; set; }
        public Nullable<int> EstimatedTime { get; set; }
        public Nullable<int> WorkedTime { get; set; }
        public Nullable<int> BreakTime { get; set; }
        public Nullable<long> StitchCount { get; set; }
        public Nullable<int> ScopeId { get; set; }
        public string? Scope { get; set; }
        public Nullable<int> ClassificationId { get; set; }
        public string? Classification { get; set; }
        public string? JobStatus { get; set; }
        public string? CustomerJobType { get; set; }
        public string? CCIntakenBy { get; set; }
        public Nullable<System.DateTime> CCIntakenDate { get; set; }
        public string? UploadedBy { get; set; }
        public Nullable<System.DateTime> UploadedUTC { get; set; }
        public string? QueryRaisedBy { get; set; }
        public Nullable<System.DateTime> QueryRaisedDate { get; set; }
        public string? QueryResponseBy { get; set; }
        public Nullable<System.DateTime> QueryResponseDate { get; set; }
        public string? AllocatedBy { get; set; }
        public Nullable<System.DateTime> AllocatedDate { get; set; }
        public Nullable<int> AllocatedToEmpId { get; set; }
        public string? AllocatedTo { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> CompletedDateTime { get; set; }
        public Nullable<bool> IsBench { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedUTC { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
        public string? ArtistName { get; set; }
        public Nullable<System.DateTime> CompletedDate { get; set; }
    }

}

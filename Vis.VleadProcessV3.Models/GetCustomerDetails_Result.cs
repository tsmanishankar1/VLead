using Microsoft.EntityFrameworkCore;

namespace Vis.VleadProcessV3.Models
{

    [Keyless]
        public partial class GetCustomerDetails_Result
    {
        public int Id { get; set; }
        public string? ShortName { get; set; }
        public int CustomerClassificationId { get; set; }
        public string? CustomerJobType { get; set; }
        public Nullable<short> CreditDays { get; set; }
        public Nullable<decimal> CreditLimit { get; set; }
        public string? ReportTimeZone { get; set; }
        public string? Timezone { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? PrivilegedClient { get; set; }
        public string? PaymentMode { get; set; }
        public string? InputType { get; set; }
        public string? OutputType { get; set; }
        public string? BillingCycleType { get; set; }
        public Nullable<int> Approvedby { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<bool> IsBulk { get; set; }
        public Nullable<bool> Checklist { get; set; }//----------------------------------------final CL--------------------------------------------
        public Nullable<bool> IsRush { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public string? EmployeeName { get; set; }
        public bool BunchMail { get; set; }
        public Nullable<bool> IsManualUpload { get; set; }
        public Nullable<DateTime> TrialStartDate { get; set; }
        public Nullable<DateTime> LiveStartDate { get; set; }
        public string? ModeofSales { get; set; }
        public string? CurrencyMode { get; set; }
        public string? LostCustomerStatus { get; set; }
    }
}


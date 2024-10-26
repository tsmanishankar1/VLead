using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Vis.VleadProcessV3.Models;

public class Customer1 {
    public int CustomerClassificationId { get; set; }
}

public partial class Customer
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string? ShortName { get; set; }

    public int CustomerClassificationId { get; set; }

    public string CustomerJobType { get; set; } = null!;

    public short? CreditDays { get; set; }

    public bool IsBlacklisted { get; set; }

    public string? BlacklistedReasons { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public int? TimeZoneId { get; set; }

    public decimal? CreditLimit { get; set; }

    public decimal? CreditLimitAvailed { get; set; }

    public string? BillingCycleType { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public string? Fax { get; set; }

    public string? Phone1 { get; set; }

    public bool? IsApproved { get; set; }

    public string? Timezone { get; set; }

    public string? TimezoneDescription { get; set; }

    public string? TimezoneType { get; set; }

    public string? ReportTimeZone { get; set; }

    public string? Country { get; set; }

    public string? State { get; set; }

    public string? City { get; set; }

    public bool? IsAdmin { get; set; }

    public string? InputType { get; set; }

    public string? OutputType { get; set; }

    public int? NativeTimeZoneDifference { get; set; }

    public int? RpttimeZoneDifference { get; set; }

    public bool? IsBulk { get; set; }

    public string? PrivilegedClient { get; set; }

    public string? PaymentMode { get; set; }

    public int? CusRegId { get; set; }

    public bool BunchMail { get; set; }

    public bool? IsManualUpload { get; set; }

    public bool? IsJobFilesNotTransfer { get; set; }

    public int? DivisionId { get; set; }

    public bool? IsRush { get; set; }

    public DateTime? TrialStartDate { get; set; }

    public DateTime? LiveStartDate { get; set; }

    public string? ModeofSales { get; set; }

    public string? CurrencyMode { get; set; }

    public bool? Checklist { get; set; }

    public string? LostCustomerStatus { get; set; }
    public bool? IsEstimatedTime { get; set; }

    public virtual ICollection<BenchScheduler> BenchSchedulers { get; } = new List<BenchScheduler>();

    public virtual ICollection<Checklist> Checklists { get; } = new List<Checklist>();

    public virtual Company Company { get; set; } = null!;
    public virtual CustomerClassification CustomerClassification { get; set; } = null!;

    public virtual ICollection<CustomerCommunication> CustomerCommunications { get; } = new List<CustomerCommunication>();

    public virtual ICollection<CustomerContact> CustomerContacts { get; } = new List<CustomerContact>();

    public virtual ICollection<CustomerDepartment> CustomerDepartments { get; } = new List<CustomerDepartment>();

    public virtual ICollection<CustomerExt> CustomerExts { get; } = new List<CustomerExt>();

    public virtual ICollection<CustomerVsDivision> CustomerVsDivisions { get; } = new List<CustomerVsDivision>();

    public virtual ICollection<CustomerVsEmployee> CustomerVsEmployees { get; } = new List<CustomerVsEmployee>();

    public virtual ICollection<CustomerVsSalesEmployee> CustomerVsSalesEmployees { get; } = new List<CustomerVsSalesEmployee>();
    public virtual ICollection<CustomerVsManager> CustomerVsManagers { get; } = new List<CustomerVsManager>();
    public virtual ICollection<CustomerCallFollowUp> CustomerCallFollowUps { get; } = new List<CustomerCallFollowUp>();
    public virtual ICollection<BillingCycleMonthly> BillingCycleMonthlies { get; } = new List<BillingCycleMonthly>();
    public virtual ICollection<CustomerVsScope> CustomerVsScopes { get; } = new List<CustomerVsScope>();
    public virtual ICollection<EmailContactNotification> EmailContactNotifications { get; } = new List<EmailContactNotification>();

    public virtual ICollection<InvoiceMaster> InvoiceMasters { get; } = new List<InvoiceMaster>();

    public virtual ICollection<JobApprovalExt> JobApprovalExts { get; } = new List<JobApprovalExt>();

    public virtual ICollection<JobStatusExt> JobStatusExts { get; } = new List<JobStatusExt>();

    public virtual ICollection<PricingWithScope> PricingWithScopes { get; } = new List<PricingWithScope>();

    public virtual ICollection<Pricing> Pricings { get; } = new List<Pricing>();

    public virtual ICollection<ProcessWorkFlowMaster> ProcessWorkFlowMasters { get; } = new List<ProcessWorkFlowMaster>();

    public virtual ICollection<Norm> Norms { get; set; } = new List<Norm>();

    public virtual ICollection<OutsourceCount> OutsourceCounts { get; set; } = new List<OutsourceCount>();

    public virtual ICollection<ProcessWorkFlow> ProcessWorkFlows { get; } = new List<ProcessWorkFlow>();

    public virtual ICollection<Receivable> Receivables { get; } = new List<Receivable>();

    public virtual TimeZone? TimeZone { get; set; }

    public virtual ICollection<User> Users { get; } = new List<User>();
}

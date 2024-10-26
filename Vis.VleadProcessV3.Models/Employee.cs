using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Vis.VleadProcessV3.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public int CompanyId { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public int DepartmentId { get; set; }

    public int DesignationId { get; set; }

    public int ProfiencyId { get; set; }

    public int? ReportLeader1 { get; set; }

    public int? ReportingManager1 { get; set; }

    public int? ReportingLeader2 { get; set; }

    public int? ReportingManager2 { get; set; }

    public DateTime DateOfBirth { get; set; }

    public DateTime DateOfJoining { get; set; }

    public DateTime? DateOfResignation { get; set; }

    public int? ResignReasons { get; set; }

    public string? Email { get; set; }
    public string? WorkingStatus { get; set; }
    public string MaritalStatus { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string? PhoneNo { get; set; }

    public string MobileNo { get; set; } = null!;

    public string BloodGroup { get; set; } = null!;

    public string EmergencyContactName { get; set; } = null!;

    public string EmergencyContactNo { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public string? PersonalEmail { get; set; }

    public int? DivisionId { get; set; }

    public bool? IsOutsource { get; set; }

    public string? IsInternetConnection { get; set; }

    public string? NetWorkType { get; set; }

    public string? ServiceProvider { get; set; }

    public string? IsSystem { get; set; }

    public string? SystemConfig { get; set; }

    public DateTime? CategoryEffectiveDate { get; set; }
    public int CoreDivision { get; set; }

    public string? BatchNo { get; set; }

    public virtual ICollection<BeanchUpload> BeanchUploads { get; } = new List<BeanchUpload>();

    public virtual ICollection<BenchScheduler> BenchSchedulerEmployees { get; } = new List<BenchScheduler>();

    public virtual ICollection<BenchScheduler> BenchSchedulerScheduledByNavigations { get; } = new List<BenchScheduler>();

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<CustomerVsEmployee> CustomerVsEmployees { get; } = new List<CustomerVsEmployee>();
    public virtual ICollection<CustomerVsManager> CustomerVsManagers { get; } = new List<CustomerVsManager>();

    public virtual ICollection<CustomerVsSalesEmployee> CustomerVsSalesEmployees { get; } = new List<CustomerVsSalesEmployee>();

    [JsonIgnore]
    public virtual Department? Department { get; set; } = null;

    public virtual Designation Designation { get; set; } = null!;

    public virtual ICollection<EmployeeAssign> EmployeeAssignCreatedByNavigations { get; } = new List<EmployeeAssign>();

    public virtual ICollection<EmployeeAssign> EmployeeAssignEmployees { get; } = new List<EmployeeAssign>();

    public virtual ICollection<EmployeeAssign> EmployeeAssignUpdatedByNavigations { get; } = new List<EmployeeAssign>();

    public virtual ICollection<EmployeeCommunication> EmployeeCommunications { get; } = new List<EmployeeCommunication>();

    public virtual ICollection<EmployeeProcess> EmployeeProcessCreatedByNavigations { get; } = new List<EmployeeProcess>();

    public virtual ICollection<EmployeeProcess> EmployeeProcessEmployees { get; } = new List<EmployeeProcess>();

    public virtual ICollection<EmployeeProcess> EmployeeProcessUpdatedByNavigations { get; } = new List<EmployeeProcess>();

    public virtual ICollection<ErrorCategory> ErrorCategoryCreatedByNavigations { get; } = new List<ErrorCategory>();
    public virtual ICollection<ExternalErrorReport> ExternalErrorReportCreatedByNavigations { get; set; } = new List<ExternalErrorReport>();
    //public virtual ICollection<ExternalErrorReport> ExternalErrorReports { get; set; } = new List<ExternalErrorReport>();

    public virtual ICollection<EmployeeCategoryChange> EmployeeCategoryChangeCreatedByNavigations { get; set; } = new List<EmployeeCategoryChange>();

    public virtual ICollection<EmployeeCategoryChange> EmployeeCategoryChangeEmployees { get; set; } = new List<EmployeeCategoryChange>();
    public virtual ICollection<ExternalErrorReport> ExternalErrorReportEmployees { get; set; } = new List<ExternalErrorReport>();

    public virtual ICollection<ErrorCategory> ErrorCategoryUpdatedByNavigations { get; } = new List<ErrorCategory>();

    public virtual ICollection<JobOrderFile> JobOrderFiles { get; } = new List<JobOrderFile>();

    public virtual ICollection<JobQuery> JobQueryCreatedByNavigations { get; } = new List<JobQuery>();

    public virtual ICollection<JobQuery> JobQueryQueryRaisedByNavigations { get; } = new List<JobQuery>();

    public virtual ICollection<JobQuery> JobQueryUpdatedByNavigations { get; } = new List<JobQuery>();

    public virtual ICollection<JobStatus> JobStatusCreatedByNavigations { get; } = new List<JobStatus>();

    public virtual ICollection<JobStatus> JobStatusUpdatedByNavigations { get; } = new List<JobStatus>();
    public virtual ICollection<Kraapproval> Kraapprovals { get; } = new List<Kraapproval>();

    public virtual ICollection<Norm> NormCreatedByNavigations { get; set; } = new List<Norm>();

    public virtual ICollection<Norm> NormEmployees { get; set; } = new List<Norm>();

    public virtual ICollection<News> News { get; } = new List<News>();

    public virtual ICollection<PricingType> PricingTypeCreatedByNavigations { get; } = new List<PricingType>();

    public virtual ICollection<PricingType> PricingTypeUpdatedByNavigations { get; } = new List<PricingType>();

    public virtual ICollection<Process> ProcessCreatedByNavigations { get; } = new List<Process>();

    public virtual ICollection<Process> ProcessUpdatedByNavigations { get; } = new List<Process>();

    public virtual ICollection<ProcessWorkFlow> ProcessWorkFlowCreatedByNavigations { get; } = new List<ProcessWorkFlow>();

    public virtual ICollection<ProcessWorkFlowTran> ProcessWorkFlowTranCreatedByNavigations { get; } = new List<ProcessWorkFlowTran>();

    public virtual ICollection<ProcessWorkFlowTran> ProcessWorkFlowTranUpdatedByNavigations { get; } = new List<ProcessWorkFlowTran>();

    public virtual ICollection<ProcessWorkFlow> ProcessWorkFlowUpdatedByNavigations { get; } = new List<ProcessWorkFlow>();

    public virtual Proficiency Profiency { get; set; } = null!;

    public virtual ResignReason? ResignReasonsNavigation { get; set; }

    public virtual ICollection<ShiftVsEmployee> ShiftVsEmployees { get; } = new List<ShiftVsEmployee>();

    public virtual ICollection<Status> StatusCreatedByNavigations { get; } = new List<Status>();

    public virtual ICollection<Status> StatusUpdatedByNavigations { get; } = new List<Status>();

    public virtual ICollection<TrainingFeedbackForm> TrainingFeedbackForms { get; set; } = new List<TrainingFeedbackForm>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}

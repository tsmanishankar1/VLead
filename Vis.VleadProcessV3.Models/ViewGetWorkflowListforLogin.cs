using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewGetWorkflowListforLogin
{
    public int Id { get; set; }

    public int Wftid { get; set; }

    public DateTime AllocattedDate { get; set; }

    public int EmployeeId { get; set; }

    public int EstimatedTime { get; set; }

    public bool IsActive { get; set; }

    public string Status { get; set; } = null!;

    public int? ProcessId { get; set; }

    public int Wfmid { get; set; }

    public int Jid { get; set; }

    public int DepartmentId { get; set; }

    public int JobOrderId { get; set; }

    public string JobId { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public string? Instruction { get; set; }

    public string? CommentsToClient { get; set; }

    public DateTime JobDate { get; set; }

    public string? ProjectCode { get; set; }

    public string CustomerClassification { get; set; } = null!;

    public DateTime? DateofDelivery { get; set; }

    public DateTime? JobDateQueryDate { get; set; }

    public DateTime? EstjobDate { get; set; }

    public DateTime? EstfileReceivedDate { get; set; }

    public DateTime? JobDateEst { get; set; }

    public string EmployeeName { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public string CustomerJobType { get; set; } = null!;

    public string? ShortName { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? QueryJobDate { get; set; }

    public int? ClientId { get; set; }

    public string? ScopeDesc { get; set; }

    public int? JobStatusId { get; set; }

    public string? JobStatusDescription { get; set; }

    public string? JobCategoryDesc { get; set; }

    public string Department { get; set; } = null!;

    public DateTime FileReceivedDate { get; set; }

    public string? JobDescription { get; set; }

    public string AssignedFrom { get; set; } = null!;

    public string? TranFileUploadPath { get; set; }

    public string? JofileUploadPath { get; set; }

    public string ProcessName { get; set; } = null!;

    public int? ScopeId { get; set; }

    public long? StitchCount { get; set; }

    public string? PoNo { get; set; }

    public string? SalesPersonName { get; set; }

    public string WorkStatus { get; set; } = null!;

    public string? ParentJobId { get; set; }

    public string? ReferenceJobId { get; set; }

    public string ParentJobFilePath { get; set; } = null!;

    public string ReferenceJobFilePath { get; set; } = null!;

    public int ParentJobtranId { get; set; }

    public int ReferenceJobTranId { get; set; }

    public int ReferenceJobStatusId { get; set; }

    public string FileInwardType { get; set; } = null!;

    public string? Color { get; set; }

    public bool? IsBulk { get; set; }

    public int? OverAllTime { get; set; }

    public int? TrayTime { get; set; }

    public int? BalanceTime { get; set; }
}

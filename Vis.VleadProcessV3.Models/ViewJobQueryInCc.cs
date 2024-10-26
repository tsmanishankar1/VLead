using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewJobQueryInCc
{
    public string JobId { get; set; } = null!;

    public string JobStatusDescription { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public string CustomerType { get; set; } = null!;

    public string? CustomerClassification { get; set; }

    public string? SalesPersonName { get; set; }

    public DateTime JobDate { get; set; }

    public int? ClientId { get; set; }

    public DateTime? FileReceivedDate { get; set; }

    public string? Instruction { get; set; }

    public string? CommentsToClient { get; set; }

    public string? Color { get; set; }

    public string? ShortName { get; set; }

    public int CustomerId { get; set; }

    public int StatusId { get; set; }

    public int? DivEmpId { get; set; }

    public int TranId { get; set; }

    public string? ProjectCode { get; set; }

    public int TranMasterId { get; set; }

    public int Jid { get; set; }

    public int DepartmentId { get; set; }

    public int? PreviousProcessId { get; set; }

    public int? ProcessId { get; set; }

    public bool IsActive { get; set; }

    public string? Name { get; set; }

    public string? Remarks { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? CreatedBy { get; set; }

    public string? TranFileUploadPath { get; set; }

    public string? JofileUploadPath { get; set; }

    public long? StitchCount { get; set; }

    public int? ScopeId { get; set; }

    public string? ParentJobId { get; set; }

    public string? ReferenceJobId { get; set; }

    public string ParentJobFilePath { get; set; } = null!;

    public string ReferenceJobFilePath { get; set; } = null!;

    public int ParentJobtranId { get; set; }

    public int ReferenceJobTranId { get; set; }

    public int ReferenceJobStatusId { get; set; }

    public byte[] TimeStamp { get; set; } = null!;

    public int? EmployeeCount { get; set; }

    public int? Value { get; set; }

    public string? Description { get; set; }

    public bool IsDeleted { get; set; }

    public string DepartmentName { get; set; } = null!;

    public int? Ccid { get; set; }

    public string? CcemailId { get; set; }

    public DateTime? JobDateQueryDate { get; set; }

    public DateTime? EstjobDate { get; set; }

    public DateTime? EstfileReceivedDate { get; set; }

    public DateTime? JobDateEst { get; set; }

    public string FileInwardType { get; set; } = null!;

    public int? OverAllTime { get; set; }

    public int? TrayTime { get; set; }

    public int? BalanceTime { get; set; }
}

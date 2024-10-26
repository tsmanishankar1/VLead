using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewJobAllocation
{
    public int TranId { get; set; }

    public int TranMasterId { get; set; }

    public int? EstimatedTime { get; set; }

    public int StatusId { get; set; }

    public int? DivEmpId { get; set; }

    public string? EmployeeName { get; set; }

    public int? EmployeeCount { get; set; }

    public byte[] TimeStamp { get; set; } = null!;

    public int DepartmentId { get; set; }

    public int? ProcessId { get; set; }

    public bool IsActive { get; set; }

    public int? AllocatedEstimatedTime { get; set; }

    public string? ProjectCode { get; set; }

    public string? CustomerClassification { get; set; }

    public DateTime? DateofDelivery { get; set; }

    public int? PreviousProcessId { get; set; }

    public string? ArtistName { get; set; }

    public string? Name { get; set; }

    public int Jid { get; set; }

    public string JobId { get; set; } = null!;

    public DateTime JobDate { get; set; }

    public DateTime? QueryJobDate { get; set; }

    public bool IsDeleted { get; set; }

    public string FileName { get; set; } = null!;

    public string CustomerJobType { get; set; } = null!;

    public string? CommentsToClient { get; set; }

    public int JobStatusId { get; set; }

    public string JobStatusDescription { get; set; } = null!;

    public int CustomerId { get; set; }

    public string? ShortName { get; set; }

    public bool? IsBulk { get; set; }

    public string? CategoryDesc { get; set; }

    public int? EmployeeId { get; set; }

    public string? WorkStatus { get; set; }

    public string? ScopeDesc { get; set; }

    public DateTime? JobDateQueryDate { get; set; }

    public DateTime? EstjobDate { get; set; }

    public DateTime? EstfileReceivedDate { get; set; }

    public DateTime? JobDateEst { get; set; }

    public string FileInwardType { get; set; } = null!;

    public int? OverAllTime { get; set; }

    public int? TrayTime { get; set; }

    public int? BalanceTime { get; set; }
}

using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewJobDetail
{
    public int TranId { get; set; }

    public int TransactionId { get; set; }

    public string JobId { get; set; } = null!;

    public DateTime JobDate { get; set; }

    public int JobStatusId { get; set; }

    public int DepartmentId { get; set; }

    public int? ProcessId { get; set; }

    public int StatusId { get; set; }

    public string? Status { get; set; }

    public DateTime FileReceivedDate { get; set; }

    public DateTime? JobDateQueryDate { get; set; }

    public DateTime? EstjobDate { get; set; }

    public DateTime? EstfileReceivedDate { get; set; }

    public DateTime? JobDateEst { get; set; }

    public string FileName { get; set; } = null!;

    public int? FileInwardTypeId { get; set; }

    public string? Username { get; set; }

    public string? SalesPersonName { get; set; }

    public string? CustomerName { get; set; }

    public DateTime? DateofUpload { get; set; }

    public bool IsDeleted { get; set; }

    public int? ClientOrderId { get; set; }

    public int? IsAutoUploadCount { get; set; }

    public string Description { get; set; } = null!;

    public string JobStatusDescription { get; set; } = null!;

    public string? FileInwardType { get; set; }

    public string Name { get; set; } = null!;

    public string? ShortName { get; set; }

    public string CustomerJobType { get; set; } = null!;

    public int? PreviousProcessId { get; set; }

    public DateTime? JobClosedUtc { get; set; }

    public bool IsActive { get; set; }

    public string? JobDescription { get; set; }

    public string? CommentsToClient { get; set; }

    public int ClientId { get; set; }

    public int Jid { get; set; }

    public int? EmployeeId { get; set; }

    public int? DivEmpId { get; set; }

    public int TranMasterId { get; set; }

    public byte[] TimeStamp { get; set; } = null!;

    public string? FileInwardMode { get; set; }

    public int? OverAllTime { get; set; }

    public int? TrayTime { get; set; }

    public int? BalanceTime { get; set; }
}

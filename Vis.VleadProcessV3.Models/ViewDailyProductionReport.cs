using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewDailyProductionReport
{
    public int TranId { get; set; }

    public int TranMasterId { get; set; }

    public int TransactionId { get; set; }

    public string JobId { get; set; } = null!;

    public DateTime JobDate { get; set; }

    public int JobStatusId { get; set; }

    public int DepartmentId { get; set; }

    public int? ProcessId { get; set; }

    public int StatusId { get; set; }

    public string? Status { get; set; }

    public DateTime FileReceivedDate { get; set; }

    public string FileName { get; set; } = null!;

    public int? FileInwardTypeId { get; set; }

    public string? Username { get; set; }

    public string? SalesPersonName { get; set; }

    public string? CustomerName { get; set; }

    public DateTime? DateofUpload { get; set; }

    public bool IsDeleted { get; set; }

    public string Description { get; set; } = null!;

    public string JobStatusDescription { get; set; } = null!;

    public string? FileInwardType { get; set; }

    public string Name { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public string CustomerJobType { get; set; } = null!;

    public int? PreviousProcessId { get; set; }

    public DateTime? JobClosedUtc { get; set; }

    public bool IsActive { get; set; }

    public int JobOrderId { get; set; }

    public string JobDescription { get; set; } = null!;

    public int ClientId { get; set; }

    public int? EmployeeId { get; set; }

    public string? CcteamIntake { get; set; }

    public string? CcteamUpload { get; set; }

    public string? ArtistName { get; set; }

    public int? EstTime { get; set; }

    public int? ArtistWorkedTime { get; set; }

    public int? BenchTime { get; set; }

    public string? Qcname { get; set; }

    public int? Qctime { get; set; }

    public string? ProofReaderName { get; set; }

    public int? Prtime { get; set; }

    public string? FinalStatus { get; set; }

    public string? ProofAccepted { get; set; }

    public string? QcerrorStatus { get; set; }

    public string? PrerrorStatus { get; set; }

    public string? ScopeDesc { get; set; }

    public long? StitchCount { get; set; }

    public string? AcceptedProof { get; set; }
}

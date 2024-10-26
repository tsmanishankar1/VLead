using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewTatreport
{
    public string? ShortName { get; set; }

    public int? ProcessId { get; set; }

    public string Description { get; set; } = null!;

    public string ClientName { get; set; } = null!;

    public string CustomerJobType { get; set; } = null!;

    public int DepartmentId { get; set; }

    public int? ClientId { get; set; }

    public DateTime FileReceivedDate { get; set; }

    public DateTime? DateofUpload { get; set; }

    public bool IsActive { get; set; }

    public int StatusId { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public string FileName { get; set; } = null!;

    public string JobId { get; set; } = null!;

    public string JobStatusDescription { get; set; } = null!;

    public int? PreviousProcessId { get; set; }

    public string? HoldStatus { get; set; }
}

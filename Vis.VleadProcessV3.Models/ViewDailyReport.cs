using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewDailyReport
{
    public string JobId { get; set; } = null!;

    public int MasterId { get; set; }

    public DateTime FileReceivedDate { get; set; }

    public DateTime? Est { get; set; }

    public string ShortName { get; set; } = null!;

    public string CustomerJobType { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public string Department { get; set; } = null!;

    public string JobStatusDescription { get; set; } = null!;

    public string CcintakeBy { get; set; } = null!;

    public int DepartmentId { get; set; }

    public int? ClientId { get; set; }

    public string? ScopeDescription { get; set; }

    public long? StitchCount { get; set; }

    public DateTime? DateofUpload { get; set; }

    public string? UploadedBy { get; set; }
}

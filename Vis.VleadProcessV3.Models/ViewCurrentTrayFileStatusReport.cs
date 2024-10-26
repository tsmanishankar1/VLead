using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewCurrentTrayFileStatusReport
{
    public int Id { get; set; }

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

    public int EstimatedTime { get; set; }

    public string AllocatedBy { get; set; } = null!;

    public string AllocatedTo { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime? Est { get; set; }
}

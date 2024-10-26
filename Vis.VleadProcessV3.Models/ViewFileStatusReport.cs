using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewFileStatusReport
{
    public string ClassificationName { get; set; } = null!;

    public int ClassificationId { get; set; }

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

    public int JobStatusId { get; set; }

    public string JobStatusDescription { get; set; } = null!;

    public int? PreviousProcessId { get; set; }

    public int? EmployeeCount { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime JobDate { get; set; }

    public DateTime? Est { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? EstqueryDate { get; set; }

    public DateTime? EstjobDate { get; set; }

    public DateTime? EstqueryJobDate { get; set; }
}

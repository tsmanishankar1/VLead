using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewJobMovement
{
    public int TranId { get; set; }

    public string JobId { get; set; } = null!;

    public DateTime JobDate { get; set; }

    public int DepartmentId { get; set; }

    public int? ProcessId { get; set; }

    public int StatusId { get; set; }

    public DateTime FileReceivedDate { get; set; }

    public string FileName { get; set; } = null!;

    public DateTime? DateofUpload { get; set; }

    public bool IsDeleted { get; set; }

    public string Description { get; set; } = null!;

    public string JobStatusDescription { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public string CustomerJobType { get; set; } = null!;

    public bool IsActive { get; set; }

    public string JobDescription { get; set; } = null!;

    public int TranMasterId { get; set; }

    public string Status { get; set; } = null!;

    public string? ProcessName { get; set; }
}

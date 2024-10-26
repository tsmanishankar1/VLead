using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewAssignedJob
{
    public string JobId { get; set; } = null!;

    public DateTime JobDate { get; set; }

    public string JobStatusDescription { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public string CustomerJobType { get; set; } = null!;

    public string? CategoryDesc { get; set; }

    public int? EstimatedTime { get; set; }

    public int StatusId { get; set; }

    public int TranId { get; set; }

    public int TranMasterId { get; set; }

    public int? ProcessId { get; set; }

    public string Name { get; set; } = null!;
}

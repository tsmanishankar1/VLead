using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewQuotationDetailsinJobOrder
{
    public string? ParentJobId { get; set; }

    public string Instruction { get; set; } = null!;

    public string? Comments { get; set; }

    public long? StitchCount { get; set; }

    public int? ScopeId { get; set; }

    public int WorkType { get; set; }

    public string Description { get; set; } = null!;

    public string? Scope { get; set; }

    public string? Suggestion { get; set; }

    public string? Remarks { get; set; }

    public decimal? SpecialPrice { get; set; }

    public int? EstimatedTime { get; set; }
}

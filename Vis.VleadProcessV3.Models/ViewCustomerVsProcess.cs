using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewCustomerVsProcess
{
    public int Id { get; set; }

    public int DepartmentId { get; set; }

    public int? CustomerId { get; set; }

    public int? CurrentProcessId { get; set; }

    public int StatusId { get; set; }

    public int? NextProcessId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public string Description { get; set; } = null!;

    public string? Name { get; set; }

    public string? ShortName { get; set; }

    public string? CustomJobType { get; set; }

    public string? CurrentProcess { get; set; }

    public string? NextProcess { get; set; }

    public string StatusDescription { get; set; } = null!;

    public string? JobStatusDescription { get; set; }

    public string? Scope { get; set; }

    public int? JobStatusId { get; set; }

    public int? ScopeId { get; set; }
}

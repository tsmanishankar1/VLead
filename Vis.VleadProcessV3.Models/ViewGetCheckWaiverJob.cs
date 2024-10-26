using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewGetCheckWaiverJob
{
    public int Id { get; set; }

    public int Imid { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Rate { get; set; }

    public decimal? Value { get; set; }

    public int? ScopeId { get; set; }

    public int? DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    public int? ScopeGroupId { get; set; }

    public string? ScopeGroupDescription { get; set; }

    public int Jid { get; set; }

    public bool? IsWaiver { get; set; }
}

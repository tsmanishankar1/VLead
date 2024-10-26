using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewCustomerStatus
{
    public int Id { get; set; }

    public string CustomerJobType { get; set; } = null!;

    public bool IsBillable { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? LiveDate { get; set; }

    public string Name { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public string ScopeDesc { get; set; } = null!;

    public string DepartmentDesc { get; set; } = null!;

    public int ScopeId { get; set; }

    public int CustomerId { get; set; }

    public int DepartmentId { get; set; }
}

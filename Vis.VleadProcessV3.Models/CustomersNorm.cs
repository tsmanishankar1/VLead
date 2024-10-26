using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class CustomersNorm1
{
    public int Id { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }
}
    public partial class CustomersNorm
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public string CustomerShortName { get; set; } = null!;

    public int DepartmentId { get; set; }

    public int ProcessId { get; set; }

    public int JobStatusId { get; set; }

    public int ScopeId { get; set; }

    public int Norms { get; set; }

    public int DivisionId { get; set; }

    public bool IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }
}

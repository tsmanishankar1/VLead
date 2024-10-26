using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewCustomerNorm
{
    public int Id { get; set; }

    public string CustomerShortName { get; set; } = null!;

    public string Departmentname { get; set; } = null!;

    public string Processname { get; set; } = null!;

    public string Jobstatus { get; set; } = null!;

    public string ScopeName { get; set; } = null!;

    public string DivisionName { get; set; } = null!;

    public int Norms { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }
}

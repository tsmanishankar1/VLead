using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class EmployeeVsDivision
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public int? DivisionId { get; set; }

    public int? DeptId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public bool? IsDeleted { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }
}

using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewCustomerVsEmployee1
{
    public int EmployeeId { get; set; }
}
    public partial class ViewCustomerVsEmployee
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int EmployeeId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public string Name { get; set; } = null!;

    public string? ShortName { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public string CustomerClassification { get; set; } = null!;

    public string EmployeeNameCode { get; set; } = null!;

    public int ClassId { get; set; }
    public bool? IsAgmapproved { get; set; }

    public int? RejectedBy { get; set; }

    public DateTime? RejectedUtc { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedUtc { get; set; }

    public bool? IsAgmrejected { get; set; }

}

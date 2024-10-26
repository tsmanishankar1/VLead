using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class NonBillable
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

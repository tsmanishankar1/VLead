using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class CustomerVsDivision
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public int? DivisionId { get; set; }

    public int? DeptId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? IsDeleted { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Department? Dept { get; set; }

    public virtual Division? Division { get; set; }
}

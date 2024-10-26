using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class TblItcategory
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public string Description { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }
}

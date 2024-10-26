using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class Holiday
{
    public int Id { get; set; }

    public string HolidayDescription { get; set; } = null!;

    public DateTime HolidayDate { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }
}

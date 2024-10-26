using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vis.VleadProcessV3.Models;

public partial class EmployeeProcess
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int ProcessId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    [ForeignKey("EmployeeId")]
    public virtual Employee CreatedByNavigation { get; set; } = null!;
    [ForeignKey("CreatedBy")]
    public virtual Employee Employee { get; set; } = null!;
    [ForeignKey("ProcessId")]
    public virtual Process Process { get; set; } = null!;
    [ForeignKey("UpdatedBy")]
    public virtual Employee? UpdatedByNavigation { get; set; }
}

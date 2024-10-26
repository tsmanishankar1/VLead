using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vis.VleadProcessV3.Models;

public partial class JobFilesLocationMaster
{
    public int Id { get; set; }

    public int? ClientId { get; set; }

    public int? DepartmentId { get; set; }

    public string? FtpfilePath { get; set; }

    public int? ProcessId { get; set; }

    public string? ProcessName { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool? IsActive { get; set; }

    [ForeignKey("ClientId")]
    public virtual Customer? Customer { get; set; }
    [ForeignKey("DepartmentId")]
    public virtual Department? Department { get; set; }
}

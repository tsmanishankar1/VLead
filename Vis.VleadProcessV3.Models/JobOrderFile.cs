using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class JobOrderFile
{
    public int Id { get; set; }

    public int Wfmid { get; set; }

    public int Wftid { get; set; }

    public int JobId { get; set; }

    public int? ProcessId { get; set; }

    public bool IsActive { get; set; }

    public string FileName { get; set; } = null!;

    public int? CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual JobOrder Job { get; set; } = null!;

    public virtual Process? Process { get; set; }

    public virtual ProcessWorkFlowMaster Wfm { get; set; } = null!;

    public virtual ProcessWorkFlowTran Wft { get; set; } = null!;
}

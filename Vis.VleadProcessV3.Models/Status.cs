using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class Status
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Employee CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<JobOrderExtension> JobOrderExtensions { get; } = new List<JobOrderExtension>();

    public virtual ICollection<JobQuery> JobQueries { get; } = new List<JobQuery>();

    public virtual ICollection<JobQuotation> JobQuotations { get; } = new List<JobQuotation>();

    public virtual ICollection<ProcessWorkFlowTran> ProcessWorkFlowTrans { get; } = new List<ProcessWorkFlowTran>();

    public virtual ICollection<ProcessWorkFlow> ProcessWorkFlows { get; } = new List<ProcessWorkFlow>();

    public virtual Employee? UpdatedByNavigation { get; set; }
}

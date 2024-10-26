using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class JobStatus
{
    public int Id { get; set; }

    public string JobStatusDescription { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public bool Billable { get; set; }

    public int JobStatusGroupId { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<ClientOrder> ClientOrders { get; } = new List<ClientOrder>();

    public virtual Employee CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<JobStatusExt> JobStatusExts { get; } = new List<JobStatusExt>();

    public virtual ICollection<ProcessWorkFlowTran> ProcessWorkFlowTrans { get; } = new List<ProcessWorkFlowTran>();

    public virtual Employee? UpdatedByNavigation { get; set; }
}

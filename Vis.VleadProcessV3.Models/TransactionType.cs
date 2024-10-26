using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class TransactionType
{
    public int Id { get; set; }

    public int? JobStatusGroupId { get; set; }

    public string Description { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<ProcessWorkFlowMaster> ProcessWorkFlowMasters { get; } = new List<ProcessWorkFlowMaster>();

    public virtual ICollection<Receivable> Receivables { get; } = new List<Receivable>();

    public virtual ICollection<VoucherControl> VoucherControls { get; } = new List<VoucherControl>();
}

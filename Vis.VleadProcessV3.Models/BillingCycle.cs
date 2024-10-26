using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class BillingCycle
{
    public int Id { get; set; }

    public int? ClientId { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public string? BillingCycleType { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<FiscalBillingCycle> FiscalBillingCycles { get; } = new List<FiscalBillingCycle>();

    public virtual ICollection<InvoiceMaster> InvoiceMasters { get; } = new List<InvoiceMaster>();
}

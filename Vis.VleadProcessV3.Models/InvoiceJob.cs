using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class InvoiceJob
{
    public int Id { get; set; }

    public int It { get; set; }

    public string JobId { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int Imid { get; set; }

    public int Jid { get; set; }

    public string? InvoiceCancelNo { get; set; }

    public DateTime? InvoiceCancelDate { get; set; }

    public virtual InvoiceMaster Im { get; set; } = null!;

    public virtual InvoiceTran ItNavigation { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewGetDropInvoiceCancel
{
    public int Id { get; set; }

    public string? InvoiceNo { get; set; }

    public string ShortName { get; set; } = null!;
}

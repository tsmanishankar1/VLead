using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewGetCancelledInvoiceList
{
    public int Id { get; set; }

    public string? InvoiceNo { get; set; }

    public string? OriginalNo { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public int CustomerId { get; set; }

    public string ShortName { get; set; } = null!;

    public decimal? ProductValue { get; set; }

    public decimal Discount { get; set; }

    public decimal InvoiceValue { get; set; }

    public int CreatedBy { get; set; }

    public string EmployeeName { get; set; } = null!;

    public decimal? RoundOff { get; set; }

    public decimal? Waiver { get; set; }

    public decimal? ReceivableValue { get; set; }

    public decimal? TotalInvoiceValue { get; set; }
}

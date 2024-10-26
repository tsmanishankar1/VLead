using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class TblVendorDetail
{
    public int Id { get; set; }

    public int? VendorId { get; set; }

    public int? BankId { get; set; }

    public string VendorName { get; set; } = null!;

    public string InvoiceNumber { get; set; } = null!;

    public DateTime InvoiceDate { get; set; }

    public decimal InvoiceValue { get; set; }

    public decimal PendingAmount { get; set; }

    public decimal AmountPaid { get; set; }

    public bool IsPaid { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

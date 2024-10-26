using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewVoucherControler
{
    public int Id { get; set; }

    public int TransactionId { get; set; }

    public int DepartmentId { get; set; }

    public decimal Cocode { get; set; }

    public string? Prefix { get; set; }

    public string? Suffix { get; set; }

    public bool Autonumber { get; set; }

    public long Voucherno { get; set; }

    public DateTime EffectiveFrom { get; set; }

    public DateTime EffectiveTo { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public bool IsActive { get; set; }
}

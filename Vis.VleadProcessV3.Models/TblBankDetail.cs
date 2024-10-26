using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class TblBankDetail
{
    public int Id { get; set; }

    public int? BankId { get; set; }

    public string BankName { get; set; } = null!;

    public string? Ifsccode { get; set; }

    public string? Branch { get; set; }

    public long? AccountNumber { get; set; }

    public string? AccountName { get; set; }

    public decimal? OpeningBalance { get; set; }

    public DateTime? ClosingDate { get; set; }

    public decimal ClosingBalance { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

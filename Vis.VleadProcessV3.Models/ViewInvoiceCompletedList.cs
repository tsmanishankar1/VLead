using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewInvoiceCompletedList
{
    public int Id { get; set; }

    public int Jid { get; set; }

    public int TransactionId { get; set; }

    public string JobId { get; set; } = null!;

    public DateTime JobDate { get; set; }

    public string? JobDescription { get; set; }

    public int DepartmentId { get; set; }

    public int? EmployeeId { get; set; }

    public int? ClientId { get; set; }

    public int? ScopeId { get; set; }

    public DateTime FileReceivedDate { get; set; }

    public string FileName { get; set; } = null!;

    public DateTime? DateofUpload { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public decimal? SpecialPrice { get; set; }

    public long? StitchCount { get; set; }

    public string? JobStatus { get; set; }

    public string? Department { get; set; }

    public string? Employee { get; set; }

    public string? Customer { get; set; }

    public string? Scope { get; set; }
}

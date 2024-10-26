using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewConvertedClientOrder
{
    public int OrderId { get; set; }

    public int ClientId { get; set; }

    public string FileName { get; set; } = null!;

    public string? Pono { get; set; }

    public string Instruction { get; set; } = null!;

    public DateTime? Podate { get; set; }

    public string? SalesPersonName { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public int? RemovalReasonId { get; set; }

    public int WorkType { get; set; }

    public int JobStatus { get; set; }

    public string? ParentJobId { get; set; }

    public string? JobReferenceId { get; set; }

    public string? ReceiptMode { get; set; }

    public string? FileUploadPath { get; set; }
}

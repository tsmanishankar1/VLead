using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ClientOrder
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

    public bool IsCompleted { get; set; }

    public string? Suggestion { get; set; }

    public int? QuotationStatus { get; set; }

    public bool? IsConverted { get; set; }

    public bool IsFileCopied { get; set; }

    public int FileInwardTypeId { get; set; }

    public int? BatchId { get; set; }

    public string? BatchDate { get; set; }

    public int? Ccid { get; set; }

    public string? CcemailId { get; set; }

    public bool? IsFileUploaded { get; set; }

    public int? FileUploadedCount { get; set; }

    public bool? IsFileTransfer { get; set; }

    public bool IsMailSent { get; set; }

    public virtual CustomerContact? Cc { get; set; }

    public virtual ICollection<ClientOrderExt> ClientOrderExts { get; } = new List<ClientOrderExt>();

    public virtual JobStatus JobStatusNavigation { get; set; } = null!;
}

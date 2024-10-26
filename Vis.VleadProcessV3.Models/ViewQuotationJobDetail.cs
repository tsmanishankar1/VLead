using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewQuotationJobDetail
{
    public int Id { get; set; }

    public string JobId { get; set; } = null!;

    public int? CustomerId { get; set; }

    public string FileName { get; set; } = null!;

    public string? SalesPersonName { get; set; }

    public DateTime FileReceivedDate { get; set; }

    public string CustomerJobType { get; set; } = null!;

    public DateTime? DateofUpload { get; set; }

    public int JobStatusId { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsCompleted { get; set; }

    public long? StitchCount { get; set; }

    public int? ScopeId { get; set; }

    public string? Description { get; set; }

    public int DepartmentId { get; set; }

    public string? Comments { get; set; }

    public string? FileUploadPath { get; set; }

    public DateTime? DateofDelivery { get; set; }

    public DateTime? EstDateofDelivery { get; set; }

    public string Instruction { get; set; } = null!;

    public decimal? SpecialPrice { get; set; }

    public int? EstimatedTime { get; set; }
}

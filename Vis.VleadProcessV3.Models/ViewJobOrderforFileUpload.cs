using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewJobOrderforFileUpload
{
    public int Id { get; set; }

    public string JobId { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public DateTime FileReceivedDate { get; set; }

    public string? FileUploadPath { get; set; }

    public int? ClientOrderId { get; set; }

    public string? ShortName { get; set; }
}

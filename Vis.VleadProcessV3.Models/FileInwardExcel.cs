using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class FileInwardExcel
{
    public int Id { get; set; }

    public string? Department { get; set; }

    public string? ClientStatus { get; set; }

    public DateTime DateofReceived { get; set; }

    public string? ClientName { get; set; }

    public string? ClientJobId { get; set; }

    public string? FileName { get; set; }

    public string? JobStatusDescription { get; set; }

    public string? Username { get; set; }

    public string? SalesPersonName { get; set; }

    public string? CustomerName { get; set; }

    public string? Temp { get; set; }

    public string? Style { get; set; }

    public string? ProjectCode { get; set; }

    public string? TeamCode { get; set; }

    public string? SchoolName { get; set; }

    public string? Ground { get; set; }

    public string? Gender { get; set; }

    public string? FileInwardMode { get; set; }

    public bool? Status { get; set; }

    public string? DateofUpload { get; set; }

    public string? Priority { get; set; }

    public string? ClientSalesPerson { get; set; }

    public string? PoNo { get; set; }

    public string? DateofDelivery { get; set; }

    public string? Division { get; set; }

    public int? UploadedBy { get; set; }
}

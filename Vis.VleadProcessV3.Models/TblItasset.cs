using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class TblItasset
{
    public int Id { get; set; }

    public string BayNumber { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string? PcName { get; set; }

    public int HardwareId { get; set; }

    public string? Monitor { get; set; }

    public string? MonitorSerialNumber { get; set; }

    public string? Keyboard { get; set; }

    public string? KeyboardSerialNumber { get; set; }

    public string? Roll { get; set; }

    public string? Division { get; set; }

    public string? Brand { get; set; }

    public string? Model { get; set; }

    public string? WarantyDetails { get; set; }

    public string? Ram { get; set; }

    public string? Processor { get; set; }

    public string? Graphics { get; set; }

    public string? Hdd { get; set; }

    public string? TagNumber { get; set; }

    public string? MacAddress { get; set; }

    public string? Os { get; set; }

    public string? IpAddress { get; set; }

    public string? ServerType { get; set; }

    public int? ServerTypeId { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public string? InvoiceNumber { get; set; }

    public string? Mouse { get; set; }

    public string? MouseSerialNumber { get; set; }

    public string? WorkingStatus { get; set; }

    public int? WorkingStatusId { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

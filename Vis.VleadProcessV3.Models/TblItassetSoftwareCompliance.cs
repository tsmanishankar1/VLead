using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class TblItassetSoftwareCompliance
{
    public int Id { get; set; }

    public int ItassetId { get; set; }

    public int SoftwareId { get; set; }

    public string? SoftwareStatus { get; set; }

    public int? SoftwareStatusId { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class TblProductionPlan
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int CustomerId { get; set; }

    public int DepartmentId { get; set; }

    public int ProductionPlan { get; set; }

    public int QualityPlan { get; set; }

    public string? ProductionType { get; set; }

    public int? ProductionTypeId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }
}

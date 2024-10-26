using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ProcessWorkFlowStatus
{
    public int Id { get; set; }

    public int? JobId { get; set; }

    public int? DepartmentId { get; set; }

    public bool? IsStatus { get; set; }
}

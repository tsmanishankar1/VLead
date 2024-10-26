using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class Skillset
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual ICollection<EmployeeVsSkillset> EmployeeVsSkillsets { get; } = new List<EmployeeVsSkillset>();
}

using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class Menu
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool HasSubMenu { get; set; }

    public int? ParentId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<Menu> InverseParent { get; } = new List<Menu>();

    public virtual Menu? Parent { get; set; }
}

using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class CustomerDepartment
{
    public int Id { get; set; }

    public int DepartmentId { get; set; }

    public int CustomerId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool? IsActive { get; set; }

    public string? FtpuserName { get; set; }

    public string? Ftppassword { get; set; }

    public string? InFtppath { get; set; }

    public string? OutFtppath { get; set; }

    public string? Host { get; set; }

    public int? Port { get; set; }

    public string? InputType { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Department Department { get; set; } = null!;
}

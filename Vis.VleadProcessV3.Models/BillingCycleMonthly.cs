using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Vis.VleadProcessV3.Models;


public class BillingCycleMonthlyViewModel
{
    public int Id { get; set; }
    public string ClientName { get; set; }
    public string DepartmentName { get; set; }
    public DateTime BillingDate { get; set; }
    public int CustomerId { get; set; }

    public int DepartmentId { get; set; }
}

public partial class BillingCycleMonthly
{
    [JsonIgnore]
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int DepartmentId { get; set; }
    public DateTime BillingDate { get; set; }
    [JsonIgnore]
    public bool IsDelete { get; set; }

    public int CreatedBy { get; set; }
    [JsonIgnore]
    public DateTime CreatedUtc { get; set; }

    public int? UpdateBy { get; set; }
    [JsonIgnore]
    public DateTime? UpadatedUtc { get; set; }
    [JsonIgnore]
    public virtual Customer? Customer { get; set; } = null!;
    [JsonIgnore]
    public virtual Department? Department { get; set; } = null!;


    //public string customerName => Customer?.Name;
    //public string departmentName => Department?.Description;
}

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Vis.VleadProcessV3.Models;

public partial class CustomerContact
{
    public int Id { get; set; }

    public string ContactName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone1 { get; set; }

    public string? CustomerDeptDescription { get; set; }

    public int CustomerId { get; set; }

    public bool? IsDeleted { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public bool? IsAdmin { get; set; }

    public bool? Iscustomer { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public virtual ICollection<ClientOrder> ClientOrders { get; } = new List<ClientOrder>();
    [JsonIgnore]
    public virtual Customer? Customer { get; set; } = null;

    public virtual ICollection<JobOrder> JobOrders { get; } = new List<JobOrder>();
}

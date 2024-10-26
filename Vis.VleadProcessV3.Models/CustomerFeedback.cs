using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class CustomerFeedback
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int CustomerContactId { get; set; }

    public int QualityOfService { get; set; }

    public int OnTimeDelivery { get; set; }

    public int AccountCommunication { get; set; }

    public int RecommendOthers { get; set; }

    public int RushTat { get; set; }

    public string? Suggestions { get; set; }

    public DateTime CreatedUtc { get; set; }
}

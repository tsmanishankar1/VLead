using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Vis.VleadProcessV3.Models;


public partial class ClientCommunication
{
    public int Id { get; set; }
    public string DelayType { get; set; } = null!;
    public string Reason { get; set; } = null!;
    public int CreatedBy { get; set; }
    public DateTime CreatedUtc { get; set; }
    public bool? Agmapproved { get; set; }
    public bool? Agmrejected { get; set; }
    public int? RejectedUpdatedBy { get; set; }
    public DateTime? RejectedUpdatedUtc { get; set; }
    public int? ApprovedBy { get; set; }
    public DateTime? ApprovedUtc { get; set; }
    public string? Attachment { get; set; }
    public ICollection<ClientCommunicationTran> ClientCommunicationTrans { get; set; } = new List<ClientCommunicationTran>();
}
public partial class ClientCommunicationRequest
{
    public string? JobId { get; set; }

    public string DelayType { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public string CustomerId { get; set; }

    public int CreatedBy { get; set; }


}

public partial class ClientCommunicationApproved
{
    public int CcId { get; set; }
    public string DelayType { get; set; } = null!;
    public string Reason { get; set; } = null!;
    public string EmailId { get; set; }
    public string Approved { get; set; }
    public bool Agmapproved { get; set; }
    public int ApprovedBy { get; set; }
    public IEnumerable<ClientCommunicationApproved> SelectedRows { get; set; }
}

public partial class ClientCommunicationRejected
{
    public int JobOrderId { get; set; }
    public string DelayType { get; set; } = null!;
    public string Reason { get; set; } = null!;
    public string EmailId { get; set; }
    public string Rejected { get; set; }
    public bool Agmrejected { get; set; }
    public int RejectedBy { get; set; }
    public IEnumerable<ClientCommunicationRejected> SelectedRows { get; set; }
}

public class ClientCommunicationResponse
{
    public int Id { get; set; }
    public string DelayType { get; set; }
    public string Reason { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedUtc { get; set; }
    public bool Agmapproved { get; set; }
    public bool Agmrejected { get; set; }
    public int? ApprovedBy { get; set; }
    public int? RejectedUpdatedBy { get; set; }
    public DateTime? ApprovedUTC { get; set; }
    public DateTime? RejectedUpdatedUTC { get; set; }
    public string? AttachmentMimeType { get; set; }
    public string? AttachmentFileName { get; set; }
    public byte[]? Attachment { get; set; }
    public List<ClientCommunicationTranResponse> Transactions { get; set; } = new List<ClientCommunicationTranResponse>();
    public IEnumerable<ClientCommunicationResponse> SelectedRows { get; set; }
}

public class ClientCommunicationTranResponse
{
    public int TranId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string Email { get; set; }
    public string? JobId { get; set; }
    public string JobFileName { get; set; }
    public DateTime? JobDate { get; set; }
    public bool IsDeleted { get; set; }
}

public partial class ClientCommunicationJob
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public partial class ClientCommunicationJobResponse
{
    public string JobId { get; set; }
    public int Jid { get; set; }
}

public partial class ClientCommunicationCustomer
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CustomerId { get; set; }
}

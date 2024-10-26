using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class MailNotificationDetail
{
    public int Id { get; set; }

    public string? Task { get; set; }

    public int CompareMailId { get; set; }

    public int CcmailId { get; set; }

    public bool? IsDeleted { get; set; }

    public int ClientId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }
}

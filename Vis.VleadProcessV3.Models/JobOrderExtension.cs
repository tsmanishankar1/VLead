using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class JobOrderExtension
{
    public int Id { get; set; }

    public int Jid { get; set; }

    public DateTime? Date { get; set; }

    public string? Type { get; set; }

    public int? Wftid { get; set; }

    public bool? Flag { get; set; }

    public int? StatusId { get; set; }

    public virtual JobOrder JidNavigation { get; set; } = null!;

    public virtual Status? Status { get; set; }

    public virtual ProcessWorkFlowTran? Wft { get; set; }
}

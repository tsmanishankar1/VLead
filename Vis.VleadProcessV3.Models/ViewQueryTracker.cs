using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ViewQueryTracker
{
    public int Jid { get; set; }

    public DateTime? EstfileReceivedDate { get; set; }

    public int? ClientId { get; set; }

    public int DepartmentId { get; set; }
}

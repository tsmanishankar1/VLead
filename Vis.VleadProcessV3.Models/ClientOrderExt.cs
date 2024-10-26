using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class ClientOrderExt
{
    public int Id { get; set; }

    public int ClientOrderId { get; set; }

    public string? AssociateFileName { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? FileDownloadDate { get; set; }

    public virtual ClientOrder ClientOrder { get; set; } = null!;
}

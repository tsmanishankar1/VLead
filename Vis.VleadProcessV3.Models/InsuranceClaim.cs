using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class InsuranceClaim
{
    public byte RecKey { get; set; }

    public byte PolId { get; set; }

    public string? PolNumber { get; set; }

    public string? PolType { get; set; }

    public DateTime? EffectiveDate { get; set; }

    public short? DocId { get; set; }

    public string? DocName { get; set; }

    public byte? Submitted { get; set; }

    public byte? Outstanding { get; set; }
}

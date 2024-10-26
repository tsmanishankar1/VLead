using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class ArtistInventoryReportDept_Result
    {
        public string? Department { get; set; }

        public int? OverallCapacityFresh { get; set; }

        public int? ReceivedTotalCountFresh { get; set; }

        public int? DeliveryPlan { get; set; }

        public int? Inventory { get; set; }

    }
}
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class ArtDigiOrderEntryInventory_Result
    {
        public string? Division { get; set; }

        public string? Client { get; set; }

        public int? PreviousDayPending { get; set; }

        public int? FreshFilesReceived { get; set; }

        public int? Revision { get; set; }

        public int? TotalCount { get; set; }

        public int? DeliveryPlan { get; set; }

        public int? Inventory { get; set; }

    }
}
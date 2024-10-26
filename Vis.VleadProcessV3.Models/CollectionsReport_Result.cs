using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class CollectionsReport_Result
    {
        public string? ShortName { get; set; }
        public string? Name { get; set; }
        public System.DateTime CollectionDate { get; set; }
        public decimal CollectionAmount { get; set; }
        public string? InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<decimal> AdjustmentAmount { get; set; }
        public Nullable<System.DateTime> EST { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class InvoiceReport_Result
    {
        public Nullable<int> customerId { get; set; }
        public string? shortName { get; set; }
        public Nullable<decimal> artInvoiceAmount { get; set; }
        public Nullable<decimal> digiInvoiceAmount { get; set; }
        public Nullable<decimal> comparedArtInvoiceAmount { get; set; }
        public Nullable<decimal> comparedDigiInvoiceAmount { get; set; }
    }
}

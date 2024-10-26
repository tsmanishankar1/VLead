using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class InvoiceInput
    {
        public string ToInvoiceNumber { get; set; } = null!;

        public int Month { get; set; }

        public int Year { get; set; }
    }
}

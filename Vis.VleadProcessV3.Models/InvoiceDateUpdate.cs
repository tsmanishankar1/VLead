using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class InvoiceDateUpdate
    {
        public int Id { get; set; }

        public string FromInvoiceNumber { get; set; } = null!;

        public string ToInvoiceNumber { get; set; } = null!;

        public int Month { get; set; }

        public int Year { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedUtc { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedUtc { get; set; }

        public bool? IsApproved { get; set; }
        public int? ApprovedBy { get; set; }
    }
}

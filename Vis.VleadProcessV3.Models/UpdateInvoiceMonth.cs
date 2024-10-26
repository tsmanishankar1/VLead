using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class UpdateInvoiceMonth
    {
        public string FromInvoiceNumber { get; set; } = null!;

        public string ToInvoiceNumber { get; set; } = null!;

        public int Month { get; set; }

        public int Year { get; set; }

        public string Approved { get; set; }
        public bool? IsApproved { get; set; }
        public int? ApprovedBy { get; set; }
        public IEnumerable<UpdateInvoiceMonth> SelectedRows { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class CustomerInvoiceRequest
    {
        public string? InvoiceNo { get; set; }
        public string CustomerId { get; set; }

    }
}
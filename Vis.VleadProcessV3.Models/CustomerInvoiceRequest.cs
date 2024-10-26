using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class CustomerInvoiceRequest
    {
        public List<CustomerInvoiceInfo> request { get; set; }

    }
    public class CustomerInvoiceInfo
    {
        public string? InvoiceNo { get; set; }
        public int CustomerId { get; set; }
    }
}

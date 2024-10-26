using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class SubmitInvoiceMonthRequest
    {
        public string fromInvoiceNumber { get; set; }
        public string toInvoiceNumber { get; set; }
        public int year { get; set; }
        public string month { get; set; }
        public int createdBy { get; set; }
    }
}
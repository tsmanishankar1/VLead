using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class ClubInvoicesDTO
    {
        public List<string> InvoiceNumbers { get; set; }
        public int SelectedCustomerId { get; set; }
        public int ClubbedBy { get; set; }
    }
}

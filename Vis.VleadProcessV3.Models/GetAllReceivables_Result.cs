using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public partial class GetAllReceivables_Result
    {
        public int Id { get; set; }
        public string? VoucherNo { get; set; }
        public Nullable<System.DateTime> CollectionDate { get; set; }
        public Nullable<System.DateTime> ReferenceDate { get; set; }
        public decimal CollectionAmount { get; set; }
        public string? ReferenceNo { get; set; }
        public string ?Description { get; set; }
        public bool IsDeleted { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public int TransactionId { get; set; }
    }
}

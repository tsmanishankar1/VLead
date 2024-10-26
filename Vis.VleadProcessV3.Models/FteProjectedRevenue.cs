using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public partial class FteProjectedRevenue
    {
        public int Id { get; set; }

        public int DivisionId { get; set; }

        public int CustomerId { get; set; }

        public int ProjectedRevenueMonth { get; set; }

        public decimal ProjectedRevenue { get; set; }

        public bool IsDeleted { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedUtc { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedUtc { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class ProjectedRevenueRequest
    {
        public int DivisionId { get; set; }

        public int CustomerId { get; set; }

        public int ProjectedRevenueMonth { get; set; }

        public decimal ProjectedRevenue { get; set; }

        public int CreatedBy { get; set; }
    }

    public class ProjectedRevenueResponse
    {
        public string Division { get; set; }

        public string Customer { get; set; }

        public string ProjectedRevenueMonth { get; set; }

        public decimal ProjectedRevenue { get; set; }
    }

}
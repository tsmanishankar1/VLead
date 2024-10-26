using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public partial class CapacityForm
    {
        public int Id { get; set; }

        public int EmpId { get; set; }

        public int DivisionId { get; set; }

        public int CustomerId { get; set; }

        public int? Fresh { get; set; }

        public int? Revision { get; set; }

        public int? QC { get; set; }

        public string? Remarks { get; set; } = null!;

        public int? CreatedBy { get; set; }

        public DateTime? CreatedUtc { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedUtc { get; set; }

        public bool IsActive { get; set; }

        public DateTime? EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

    }
}

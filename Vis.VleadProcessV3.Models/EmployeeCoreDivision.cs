using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public partial class EmployeeCoreDivision
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; } = null!;
        public int CoreDivisionId { get; set; }

        public DateTime EffectiveFromDate { get; set; }

        public DateTime? EffectiveToDate { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedUtc { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedUtc { get; set; }
    }
}
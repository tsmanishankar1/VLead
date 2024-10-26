using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public partial class EmployeeCategoryChange
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string PreviousWorkMode { get; set; } = null!;

        public string NewWorkMode { get; set; } = null!;

        public DateTime EffectiveFromDate { get; set; }

        public DateTime? EffectiveToDate { get; set; }

        public bool IsDeleted { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedUtc { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedUtc { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Employee? CreatedByNavigation { get; set; } = null!;

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Employee? Employee { get; set; } = null!;
    }
}

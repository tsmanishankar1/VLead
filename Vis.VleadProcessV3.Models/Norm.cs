using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public partial class Norm
    {
        public int Id { get; set; }

        public int DivisionId { get; set; }

        public int ClientId { get; set; }

        public int ScopeId { get; set; }

        public int EmployeeId { get; set; }

        public int ProductivityNorm { get; set; }

        public DateTime EffectiveFromDate { get; set; }

        public DateTime? EffectiveToDate { get; set; }

        public string WorkMode { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedUtc { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedUtc { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Customer? Client { get; set; } = null!;

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Employee? CreatedByNavigation { get; set; } = null!;

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Division? Division { get; set; } = null!;

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Employee? Employee { get; set; } = null!;

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Scope? Scope { get; set; } = null!;
    }
}

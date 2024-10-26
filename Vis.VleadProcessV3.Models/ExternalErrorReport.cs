using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public partial class ExternalErrorReport
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedUtc { get; set; }

        public int DivisionId { get; set; }

        public string JobNumber { get; set; } = null!;

        public string Department { get; set; } = null!;

        public string Client { get; set; } = null!;

        public string ClientStatus { get; set; } = null!;

        public string JobStatus { get; set; } = null!;

        public string FileName { get; set; } = null!;

        public string ArtistId { get; set; } = null!;

        public string? ArtistName { get; set; }

        public int? QcId { get; set; }

        public string? QcName { get; set; }

        public string? ClientRevisionComment { get; set; }

        public string? ErrorType { get; set; }

        public DateTime InputMonthYear { get; set; }

        public string Division { get; set; } = null!;

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Employee CreatedByNavigation { get; set; } = null!;

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Division DivisionNavigation { get; set; } = null!;

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Employee Employee { get; set; } = null!;
    }
}

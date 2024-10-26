using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetFTEUtilizationReport_Result
    {
        public string? ClientCode { get; set; }
        public string? department { get; set; }
        public string? EmployeeCode { get; set; }
        public string? ArtistName { get; set; }
        public Nullable<int> ArtistId { get; set; }
        public Nullable<int> ProductionWorkedTime { get; set; }
        public Nullable<int> QualityWorkedTime { get; set; }
        public Nullable<int> BPWorkedTime { get; set; }
        public Nullable<int> SOWorkedTime { get; set; }
        public Nullable<int> EstimationTime { get; set; }
    }
}

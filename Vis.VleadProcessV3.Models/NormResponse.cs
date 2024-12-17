using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class NormResponse
    {
        public int DivisionId { get; set; }
        public string Division { get; set; }
        public int ClientId { get; set; }
        public string Client { get; set; }
        public int? ScopeId { get; set; }
        public string Scope { get; set; }
        public string? Process {  get; set; }
        public int? JobStatusId { get; set; }
        public string? JobStatus { get; set; }
        public int ProductivityNorm { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public string WorkMode { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}

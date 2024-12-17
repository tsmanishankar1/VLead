using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class CreateNormRequest
    {
        public int DivisionId { get; set; }
        public int ClientId { get; set; }
        public int? ScopeId { get; set; }
        public string? Process {  get; set; }
        public int? JobStatusId { get; set; }
        public int? EmployeeId { get; set; }
        public int ProductivityNorm { get; set; }
        public string WorkMode { get; set; }
        public int CreatedBy { get; set; }
    }
}

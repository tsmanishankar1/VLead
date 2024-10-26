using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class EmployeeCategoryChangeRequest
    {
        public int DivisionId { get; set; }
        public int ClientId { get; set; }
        public int ScopeId { get; set; }
        public int EmployeeId { get; set; }
        public string NewWorkMode { get; set; }
        public int NewNorm { get; set; }
        public int CreatedBy { get; set; }
    }
}

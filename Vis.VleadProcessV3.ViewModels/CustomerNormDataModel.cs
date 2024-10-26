using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.ViewModels
{
    public class CustomerNormDataViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerShortName { get; set; }
        public int DepartmentId { get; set; }
        public int ProcessId { get; set; }
        public int JobStatusId { get; set; }
        public int ScopeId { get; set; }
        public int Norms { get; set; }
        public int DivisionId { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedUTC { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
        public bool IsDeleted { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class sp_GetEmployeebyDivision_Result
    {
        public Nullable<int> EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public Nullable<int> DivisionId { get; set; }
        public string? DivisionName { get; set; }
    }
}

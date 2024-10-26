using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class sp_HourlyCCReport_Result
    {
        public Nullable<System.DateTime> Date { get; set; }
        public string? DivisionName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? ClientShortName { get; set; }
        public string? Process { get; set; }
        public string? Type { get; set; }
        public Nullable<int> C0 { get; set; }
        public Nullable<int> C1 { get; set; }
        public Nullable<int> C2 { get; set; }
        public Nullable<int> C3 { get; set; }
        public Nullable<int> C4 { get; set; }
        public Nullable<int> C5 { get; set; }
        public Nullable<int> C6 { get; set; }
        public Nullable<int> C7 { get; set; }
        public Nullable<int> C8 { get; set; }
        public Nullable<int> C9 { get; set; }
        public Nullable<int> C10 { get; set; }
        public Nullable<int> C11 { get; set; }
        public Nullable<int> C12 { get; set; }
        public Nullable<int> C13 { get; set; }
        public Nullable<int> C14 { get; set; }
        public Nullable<int> C15 { get; set; }
        public Nullable<int> C16 { get; set; }
        public Nullable<int> C17 { get; set; }
        public Nullable<int> C18 { get; set; }
        public Nullable<int> C19 { get; set; }
        public Nullable<int> C20 { get; set; }
        public Nullable<int> C21 { get; set; }
        public Nullable<int> C22 { get; set; }
        public Nullable<int> C23 { get; set; }
    }
}

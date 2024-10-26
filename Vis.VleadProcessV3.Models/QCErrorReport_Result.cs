using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class QCErrorReport_Result
    {
        public string? JobId { get; set; }
        public string? FileName { get; set; }
        public int DepartmentId { get; set; }
        public Nullable<int> WFMId { get; set; }
        public string? Description { get; set; }
        public Nullable<int> JId { get; set; }
        public string? QCRemarks { get; set; }  //
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class InventoryForm_Result
    {
        public string? JobId { get; set; }

        public string? DeptName { get; set; }

        public string? ShortName { get; set; }

        public string? ProjectCode { get; set; }

        public string? DivisionName { get; set; }

        public string? FileName { get; set; }
    }
}

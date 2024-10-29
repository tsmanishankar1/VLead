using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public partial class LeaveMaster
    {
        public int Id { get; set; }

        public string Leave { get; set; } = null!;

        public string LeaveType { get; set; } = null!;

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedUtc { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedUtc { get; set; }
    }

    public partial class LeaveMasterRequest
    {
        public string Leave { get; set; } = null!;

        public string LeaveType { get; set; } = null!;

        public int CreatedBy { get; set; }
    }

    public partial class LeaveMasterResponse
    {
        public string Leave { get; set; } = null!;

        public string LeaveType { get; set; } = null!;
    }
}

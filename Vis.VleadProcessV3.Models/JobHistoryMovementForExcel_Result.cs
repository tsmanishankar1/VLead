using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class JobHistoryMovementForExcel_Result
    {
        public string? Department { get; set; }
        public string? JobId { get; set; }
        public System.DateTime JobDate { get; set; }
        public string? Client { get; set; }
        public string? FileName { get; set; }
        public System.DateTime FileReceivedDate { get; set; }
        public string? Status { get; set; }
        public string? ProcessName { get; set; }
        public string? JobStatusDescription { get; set; }
        public string? CustomerJobType { get; set; }
        public string? ArtistName { get; set; }
        public Nullable<int> EstTime { get; set; }
    }
}

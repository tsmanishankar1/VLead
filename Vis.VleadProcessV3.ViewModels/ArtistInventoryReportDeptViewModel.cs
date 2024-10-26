using Microsoft.EntityFrameworkCore;

namespace Vis.VleadProcessV3.ViewModels
{
    [Keyless]
    public class ArtistInventoryReportDeptViewModel
    {
        public DateTime FileReceivedDate { get; set; }

        public int DepartmentId { get; set; }

        public int ClientId { get; set; }

        public int JobStatusId { get; set; }

        public int ClassificationId { get; set; }

    }
}

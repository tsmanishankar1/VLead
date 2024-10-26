using Microsoft.EntityFrameworkCore;

namespace Vis.VleadProcessV3.ViewModels
{
    [Keyless]
    public class ArtistInventoryReportDivViewModel
    {
        public DateTime FileReceivedDate { get; set; }

        public int DivisionId { get; set; }

        public int ClientId { get; set; }

        public int JobStatusId { get; set; }

        public int ClassificationId { get; set; }

    }
}

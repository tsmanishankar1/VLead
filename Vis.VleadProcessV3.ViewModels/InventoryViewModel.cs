
using Microsoft.EntityFrameworkCore;

namespace Vis.VleadProcessV3.ViewModels
{
    [Keyless]
    public class InventoryViewModel
    {
        public int DivisionId { get; set; }

        public int CustomerId { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }
}
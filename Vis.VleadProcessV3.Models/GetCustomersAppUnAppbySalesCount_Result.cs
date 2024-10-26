using Microsoft.EntityFrameworkCore;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetCustomersAppUnAppbySalesCount_Result
    {
        public Nullable<int> UnapprovedcusCount { get; set; }
        public Nullable<int> ApprovedListCount { get; set; }
    }
}

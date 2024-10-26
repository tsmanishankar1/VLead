using Microsoft.EntityFrameworkCore;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetJobStatusByCustId_Result
    {
        public string? JobStatusDescription { get; set; }
        public string? ShortDescription { get; set; }
        public int Id { get; set; }

    }
}

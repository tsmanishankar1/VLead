using Microsoft.EntityFrameworkCore;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetCustomerTATByCustId_Result
    {
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public string? JobStatusDescription { get; set; }
        public decimal TAT { get; set; }
        public int Id { get; set; }
    }
}

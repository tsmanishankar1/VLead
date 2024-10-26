using Microsoft.EntityFrameworkCore;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetCustomerContactCredentials_Result
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Admincontact { get; set; }
        public string? ContactName { get; set; }
    }
}

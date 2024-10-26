using Microsoft.EntityFrameworkCore;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetCustomerRegDetails_Result
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? EmailID { get; set; }
        public string? PhoneNo { get; set; }
        public string? phone1 { get; set; }
        public string? Fax { get; set; }
        public string? Website { get; set; }
        public DateTime CreatedUTC { get; set; }
        public string? CustomerJobType { get; set; }
    }
}

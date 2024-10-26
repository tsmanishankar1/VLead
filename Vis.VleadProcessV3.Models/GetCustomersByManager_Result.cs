using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetCustomersByManager_Result
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public string? SPOC { get; set; }
        public int? ContactPersonId { get; set; }
        public string? ClientContactPerson { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; }
    }
}

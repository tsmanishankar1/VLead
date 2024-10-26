using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class CustomerFollowUpReq
    {
        public string PurposeOfCall { get; set; } = null!;

        public string ModeOfCall { get; set; } = null!;

        public DateTime DateTimeOfCall { get; set; }

        public string? Comments { get; set; }
        public IFormFile? File { get; set; }
    }
}


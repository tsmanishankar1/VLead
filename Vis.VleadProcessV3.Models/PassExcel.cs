using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class PassExcel
    {
        public int createdBy { get; set; }
        public IFormFile file { get; set; }
    }
}

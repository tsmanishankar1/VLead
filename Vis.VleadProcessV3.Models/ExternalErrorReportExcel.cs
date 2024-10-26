using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class ExternalErrorReportExcel
    {
        public int CreatedBy { get; set; }
        public IFormFile file { get; set; }
    }
}

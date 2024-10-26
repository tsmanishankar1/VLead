using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class GetFileCountDivisionWiseByManagerId_Result
    {
        public string Department { get; set; }
        public int FilesCountCurrentMonth { get; set; }
        public int FilesCountPreviousMonth { get; set; }
        public int FileCountDifference { get; set; }
        public string Trend { get; set; }

    }
}
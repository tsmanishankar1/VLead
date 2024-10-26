using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.ViewModels
{
    public class JobTransferViewModel
    {
        public IEnumerable<JobOrder> ConvertDepartment { get; set; }
        public int UpdatedBy { get; set; }
    }
}

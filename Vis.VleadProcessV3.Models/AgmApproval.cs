using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public class AgmApproval
    {
        public string CustomerName { get; set; }
        public string EmployeeCode { get; set; }
        public string CustomerShortName { get; set; }
        public string ApprovedStatus { get; set; }
        public int? ApprovedBy { get; set; }
        public int? RejectedBy { get; set; }
        public IEnumerable<AgmApproval> SelectedRows { get; set; }
    }

    public class ActionResultHelper
    {
        public static IActionResult Ok(string message)
        {
            return new OkObjectResult(message);
        }
    }
}
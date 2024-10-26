using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public partial class DashboardReportFSR_Result
    {
        public Nullable<System.DateTime> FileReceivedDate { get; set; }
        //public Nullable<int> jobstatusId { get; set; }
        public string? Description { get; set; }
        public string? ShortName { get; set; }
        public Nullable<int> ClientId { get; set; }
        public int DepartmentId { get; set; }
        public string? CustomerJobType { get; set; }
        public int ClassificationId { get; set; }
        public string? customerType { get; set; }
        public Nullable<int> Fileasonpereviousday { get; set; }
        public Nullable<int> TodayFilesReceived { get; set; }
    }
}

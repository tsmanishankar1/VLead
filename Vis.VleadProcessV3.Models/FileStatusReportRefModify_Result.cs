using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    //public partial class FileStatusReportRefModify_Result
    //{
    //    public string FileReceivedDate { get; set; }
    //    public int jobstatusId { get; set; }
    //    public string Description { get; set; }
    //    public string ShortName { get; set; }
    //    public Nullable<int> ClientId { get; set; }
    //    public int DepartmentId { get; set; }
    //    public string CustomerJobType { get; set; }
    //    public int ClassificationId { get; set; }
    //    public string customerType { get; set; }
    //    public Nullable<int> Fileasonpereviousday { get; set; }
    //    public Nullable<int> TodayFilesReceived { get; set; }
    //    public Nullable<int> TotalFileReceived { get; set; }
    //    public Nullable<int> QuerySent { get; set; }
    //    public Nullable<int> QueryResponse { get; set; }
    //    public Nullable<int> ClientCoordination { get; set; }
    //    public Nullable<int> ProdnAllocation { get; set; }
    //    public Nullable<int> Production { get; set; }
    //    public Nullable<int> QualityAllocation { get; set; }
    //    public Nullable<int> Quality { get; set; }
    //    public Nullable<int> ProofReadingAllocation { get; set; }
    //    public Nullable<int> ProofReading { get; set; }
    //    public Nullable<int> BuddyProofAllocation { get; set; }
    //    public Nullable<int> BuddyProof { get; set; }
    //    public Nullable<int> sewOutallocation { get; set; }
    //    public Nullable<int> SewOut { get; set; }
    //    public Nullable<int> CancelledJobs { get; set; }
    //    public Nullable<int> FileUploaded { get; set; }
    //}
    [Keyless]
    public partial class FileStatusReportRefModify_Result
    {
        public Nullable<System.DateTime> FileReceivedDate { get; set; }
        public Nullable<int> jobstatusId { get; set; }
        public string? Description { get; set; }
        public string? ShortName { get; set; }
        public Nullable<int> ClientId { get; set; }
        public int DepartmentId { get; set; }
        public string? CustomerJobType { get; set; }
        public int ClassificationId { get; set; }
        public string? customerType { get; set; }
        public Nullable<int> Fileasonpereviousday { get; set; }
        public Nullable<int> FileasReceivedonpereviousday { get; set; }
        public Nullable<int> TodayFilesReceived { get; set; }
        public Nullable<int> TotalFileReceived { get; set; }
        public Nullable<int> QuerySent { get; set; }
        public Nullable<int> QueryResponse { get; set; }
        public Nullable<int> ClientCoordination { get; set; }
        public Nullable<int> ProdnAllocation { get; set; }
        public Nullable<int> Production { get; set; }
        public Nullable<int> QualityAllocation { get; set; }
        public Nullable<int> Quality { get; set; }
        public Nullable<int> ProofReadingAllocation { get; set; }
        public Nullable<int> ProofReading { get; set; }
        public Nullable<int> BuddyProofAllocation { get; set; }
        public Nullable<int> BuddyProof { get; set; }
        public Nullable<int> sewOutallocation { get; set; }
        public Nullable<int> SewOut { get; set; }
        public Nullable<int> CancelledJobs { get; set; }
        public Nullable<int> FileUploaded { get; set; }

    }
}

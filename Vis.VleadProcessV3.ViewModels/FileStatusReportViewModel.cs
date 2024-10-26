using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.ViewModels
{
    public class FileStatusReportViewModel
    {
        public int Id { get; set; }
        public int WFMId { get; set; }
        public int WFTId { get; set; }
        public int JobId { get; set; }
        public int ProcessId { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public Nullable<int> CancelledJobs { get; set; }
        public DateTime FileReceivedDate { get; set; }
        public int ClientId { get; set; }
        public int DepartmentId { get; set; }
        public string CustomerJobType { get; set; }
        public string Description { get; set; }
        public string ProcessName { get; set; }
        public string ShortName { get; set; }
        public Nullable<int> Fileasonpereviousday { get; set; }
        public Nullable<int> FileasReceivedonpereviousday { get; set; }
        public Nullable<int> TotalFileReceived { get; set; }
        public Nullable<int> TodayFilesReceived { get; set; }
        public int ClientCoordination { get; set; }
        public int ProdnAllocation { get; set; }
        public int Production { get; set; }
        public int QualityAllocation { get; set; }
        public int Quality { get; set; }
        public int ProofReadingAllocation { get; set; }
        public int ProofReading { get; set; }
        public int BuddyProof { get; set; }
        public int SewOut { get; set; }
        public Nullable<int> QuerySent { get; set; }
        public Nullable<int> QueryResponse { get; set; }
        public Nullable<int> FileUploaded { get; set; }
        public int TotalPending { get; set; }
        public int OSFilePendingCountpopup { get; set; }
        public Nullable<System.DateTime> DateofUpload { get; set; }
        public Nullable<System.DateTime> JobClosedUTC { get; set; }
        public string JobNo { get; set; }
        public string JobStatusDescription { get; set; }
        public int esttime { get; set; }
        public string AllocatedBy { get; set; }
        public string AllocatedTo { get; set; }
        public int ArtCompleted { get; set; }
        public int QCCompleted { get; set; }
        public int ArtPending { get; set; }
        public int QCPending { get; set; }
        public int Proofpending { get; set; }
        public int ProofCorrection { get; set; }
        public int Supportpending { get; set; }
        public int Hold { get; set; }
        public int BuddyProofAllocation { get; set; }
        public int sewOutallocation { get; set; }
        public int jobstatusId { get; set; }
        //  public int?[] selectedCustomersIds { get; set; }
        public int customerClassficationId { get; set; }
        public string customerType { get; set; }

    }
    public class InitialLoad
    {
        public DateTime FileReceivedDate { get; set; }
        public int? DepartmentId { get; set; }

    }
    public class FileStatusReportModifyViewModel
    {
        public DateTime FileReceivedDate { get; set; }
        public int ClientId { get; set; }
        public int DepartmentId { get; set; }
        public int customerClassficationId { get; set; }
        public int jobstatusId { get; set; }
    }
    public class SewOutReportModel
    {
        public Nullable<int> Id { get; set; }
        public string ShortName { get; set; }
        public string CustomerName { get; set; }
        public Nullable<int> TotalList { get; set; }
        public string customer { get; set; }
        public Nullable<int> Total { get; set; }
        public Nullable<int> ClientId { get; set; }
        public Nullable<int> TotalFiles { get; set; }
        public Nullable<int> SewOutFiles { get; set; }
        public decimal SewOutpercentage { get; set; }
        public Nullable<int> SumTotalfiles { get; set; }
        public Nullable<int> SumTotalsewoutfiles { get; set; }
        public decimal SumTotalsewoutpercentage { get; set; }
    }
}

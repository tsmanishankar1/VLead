using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.ViewModels
{
    public class DailyReportViewModel
    {
        public int ClientId { get; set; }
        public int DepartmentId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string JobId { get; set; }
        public int MasterId { get; set; }
        public DateTime FileReceivedDate { get; set; }
        public string ShortName { get; set; }
        public string CustomerJobType { get; set; }
        public string FileName { get; set; }
        public string Department { get; set; }
        public string JobStatusDescription { get; set; }
        public string CCIntakeBy { get; set; }
        public string FinalStatusofFile { get; set; }
        public string QueryorSPorCancelorDuplicateInitiatedBy { get; set; }
        public string FinalRemark { get; set; }
        public string ProductionAllocatedBy { get; set; }
        public string ArtistName { get; set; }
        public string EstTime { get; set; }
        public string WorkedTime { get; set; }
        public string ArtistCompletedDate { get; set; }
        public string QCAllocatedby { get; set; }
        public string QCName { get; set; }
        public int QCWorkedTime { get; set; }
        public string QCErrorStatus { get; set; }
        public string QCCompletedDate { get; set; }
        public string Scope { get; set; }
        public long? StitchCount { get; set; }
        public string SewOutDoneBy { get; set; }
        public string SewOutErrorStatus { get; set; }
        public int SewOutWorkedTime { get; set; }
        public string SewOutCompletedDate { get; set; }
        public string BuddyProofDoneBy { get; set; }
        public string BuddyProofErrorStatus { get; set; }
        public int BuddyProofWorkedTime { get; set; }
        public string BuddyProofCompletedDate { get; set; }
        public string ProofReadingAllocatedBy { get; set; }
        public string ProofReaderName { get; set; }
        public int ProofReadWorkedTime { get; set; }
        public string ProofReadErrorStatus { get; set; }
        public string ProofReadCompletedDate { get; set; }
        public string RevisedorNotStatus { get; set; }
        public string Uploadedby { get; set; }
        public string UploadedDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public partial class DailyProductionReportRef_Result
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> JobDate { get; set; }
        public string? FileName { get; set; }
        public Nullable<int> ClientId { get; set; }
        public string? ClientShortName { get; set; }
        public string? Department { get; set; }
        public string? JobStatus { get; set; }
        public string? CustomerJobType { get; set; }
        public string? Scope { get; set; }
        public Nullable<int> StitchCount { get; set; }
        public string? CurrentTray { get; set; }
        public string? CurrentStatus { get; set; }
        public string? CCIntakenBy { get; set; }
        public string? ProductionAllocatedBy { get; set; }
        public Nullable<int> ProductionAllocatedToEmpId { get; set; }
        public string? ProductionAllocatedTo { get; set; }
        public Nullable<int> EstimationTime { get; set; }
        public Nullable<int> ProductionWorkedTime { get; set; }
        public Nullable<int> ProductionBreakTime { get; set; }
        public Nullable<System.DateTime> ProductionCompletedDateTime { get; set; }
        public string? QualityAllocatedBy { get; set; }
        public Nullable<int> QualityAllocatedToEmpId { get; set; }
        public string? QualityAllocatedTo { get; set; }
        public Nullable<int> QualityWorkedTime { get; set; }
        public Nullable<int> QualityBreakTime { get; set; }
        public Nullable<System.DateTime> QualityCompletedDateTime { get; set; }
        public string? SOAllocatedBy { get; set; }
        public Nullable<int> SOAllocatedToEmpId { get; set; }
        public string? SOAllocatedTo { get; set; }
        public Nullable<int> SOWorkedTime { get; set; }
        public Nullable<System.DateTime> SOCompletedDateTime { get; set; }
        public string? BPAllocatedBy { get; set; }
        public Nullable<int> BPAllocatedToEmpId { get; set; }
        public string? BPAllocatedTo { get; set; }
        public Nullable<int> BPWorkedTime { get; set; }
        public Nullable<System.DateTime> BPCompletedDateTime { get; set; }
        public string? ProofReadingAllocatedBy { get; set; }
        public Nullable<int> ProofReadingAllocatedToEmpId { get; set; }
        public string? ProofReadingAllocatedTo { get; set; }
        public Nullable<int> ProofreadingWorkedTime { get; set; }
        public Nullable<System.DateTime> ProofReadingCompletedDateTime { get; set; }
        public string? UploadedBy { get; set; }
        public Nullable<System.DateTime> UploadedUTC { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedUTC { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
    }
}

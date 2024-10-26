using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.ViewModels
{
    public class ReportViewModel
    {

    }

    public class QuotationTrackerViewModel
    {
        public string Department { get; set; }
        public DateTime? FileReceivedDate { get; set; }
        public DateTime? JobDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string Client { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public int? EstimatedTimeinMM { get; set; }
        public long? StitchCount { get; set; }
        public decimal? AmountinDollar { get; set; }
        public string RemarksFromVLead { get; set; }
        public string QuotSentbyAllocProductionQuality { get; set; }
        public string QuotationSentbyCC { get; set; }
        public string ApprovedStatus { get; set; }
        public DateTime? ApprovedDate { get; set; }

    }
    public class NewClientReportViewModel
    {
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public String department { get; set; }
    }
    public class TrialClientReportViewModel
    {
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public int[] CustomerId { get; set; }
        public string Department { get; set; }
    }
    //public class TrialClientMisSummaryReportViewModel
    //{
    //    public int DepartmentId { get; set; }
    //    public string Client { get; set; }
    //    public int[] CustomerId { get; set; }
    //    public string Scope { get; set; }
    //    public DateTime? FirstDate { get; set; }
    //    public DateTime? LatestDate { get; set; }
    //    public int NumberOfDaysInTransition { get; set; }
    //    public string PricingGiven { get; set; }
    //    public int TotalFileCountReceivedTillDate { get; set; }
    //    public int FreshFileCount { get; set; }
    //    public decimal FreshAvgStitchCount { get; set; }
    //    public decimal FreshAvgTimeStamp { get; set; }
    //    public int RevisionFileCount { get; set; }
    //    public decimal RevisionAvgStitchCount { get; set; }
    //    public decimal RevisionAvgTimeStamp { get; set; }


    //}
    public class InputForTrialClientSummaryReport
    {
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public int[] CustomerId { get; set; }
        public string department { get; set; }
    }
    public class RevisionCountReportViewModel
    {
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public int[] CustomerId { get; set; }
        public String department { get; set; }
    }
    public class EmployeeErrorReport
    {
        public int Id { get; set; }
        public Nullable<int> ClientId { get; set; }
        public string ClientShortName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int DepartmentId { get; set; }
        public int StatusId { get; set; }
        public string Process { get; set; }
        public string JobId { get; set; }
        public Nullable<int> ProcessId { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public string Department { get; set; }
        public string ProName { get; set; }
        public string PRName { get; set; }
        public string QCName { get; set; }
        public string SewoutName { get; set; }
        public string BuddyName { get; set; }
        public string FileName { get; set; }
        public string Remarks { get; set; }
        public string QCRemarks { get; set; }//
        public string NeedRevise { get; set; }
        public string EmployeeName { get; set; }
        public DateTime JobDate { get; set; }
        public IEnumerable<TrayDetailMovement> TrayDetailmovement { get; set; }
    }
    public class FTEUtilizationReportViewModel
    {
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public String department { get; set; }
    }
    public class FTEUtilizationwithClientWiseReportViewModel
    {
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public int[] CustomerId { get; set; }
        public String department { get; set; }
    }
    public class FteUtilizationFields
    {
        public string ClientCode { get; set; }
        public string department { get; set; }
        public string EmployeeCode { get; set; }
        public string ArtistName { get; set; }
        public Nullable<int> ProductionWorkedTime { get; set; }
        public Nullable<int> QualityWorkedTime { get; set; }
        public Nullable<int> BPWorkedTime { get; set; }
        public Nullable<int> SOWorkedTime { get; set; }
        public Nullable<int> EstimationTime { get; set; }
        public int TotalEstimationTime { get; set; }
        public double FtePercentage { get; set; }
    }
    public class EmployeeWiseReport
    {
        public Nullable<int> Id { get; set; }
        public string JobId { get; set; }
        public DateTime FileReceivedDateFrom { get; set; }
        public DateTime FileReceivedDateTo { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ShortName { get; set; }
        public string Status { get; set; }
        public string JobStatusDescription { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public int ProcessId { get; set; }
        public int statusId { get; set; }
        public int EstimatedTime { get; set; }
        public int TotalTimeTaken { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public int[] SelectedClientId { get; set; }
        public int[] SelectedEmployeeId { get; set; }
        public int[] SelectedProcessId { get; set; }
        public int TotalfilesAllocatedProd { get; set; }
        public IEnumerable<TrayDetailMovement> TrayDetailmovement { get; set; }
    }
    public class EmployeeDetailsReport
    {
        public Nullable<int> Id { get; set; }
        public Nullable<int> ClientId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int DepartmentId { get; set; }
        public string ClientShortName { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Status { get; set; }//
        public string FileName { get; set; }
        public int EmployeeId1 { get; set; }
        public int DepartmentId1 { get; set; }
        public Nullable<int> FilesAllocatedInProd { get; set; }
        public Nullable<int> EstimatedTime { get; set; }
        public Nullable<int> ProdWorkedTime { get; set; }
        public Nullable<int> ProdBreakTime { get; set; }
        public Nullable<int> FilesCompInProd { get; set; }
        public Nullable<int> FilesAllocatedInQuality { get; set; }
        public Nullable<int> QualityWorkedTime { get; set; }
        public Nullable<int> FilesCompInQuality { get; set; }
        //
        public Nullable<int> FilesAllocatedInProof { get; set; }
        public Nullable<int> ProofWorkedTime { get; set; }
        public Nullable<int> FilesCompInProof { get; set; }
        //
        public Nullable<int> FilesAllocatedInBP { get; set; }
        public Nullable<int> BPWorkedTime { get; set; }
        public int FilesCompInBP { get; set; }
        public Nullable<int> FilesAllocatedInSO { get; set; }
        public Nullable<int> SOWorkedTime { get; set; }
        public Nullable<int> FilesCompInSO { get; set; }
        public int FilesAllocatedInPR { get; set; }
        public int PRWorkedTime { get; set; }
        public int FilesCompInPR { get; set; }
        public IEnumerable<TrayDetailMovement> TrayDetailmovement { get; set; }
    }
    public class EmployeeReport
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> JobDate { get; set; }
        public string FileName { get; set; }
        public Nullable<int> ClientId { get; set; }
        public string ClientShortName { get; set; }
        public string Department { get; set; }
        public string JobStatus { get; set; }
        public string CustomerJobType { get; set; }
        public string Scope { get; set; }
        public Nullable<int> StitchCount { get; set; }
        public string CurrentTray { get; set; }
        public string CurrentStatus { get; set; }
        public string CCIntakenBy { get; set; }
        public string ProductionAllocatedBy { get; set; }
        public Nullable<int> ProductionAllocatedToEmpId { get; set; }
        public string ProductionAllocatedTo { get; set; }
        public Nullable<int> EstimationTime { get; set; }
        public Nullable<int> ProductionWorkedTime { get; set; }
        public Nullable<int> ProductionBreakTime { get; set; }
        public Nullable<System.DateTime> ProductionCompletedDateTime { get; set; }
        public string QualityAllocatedBy { get; set; }
        public Nullable<int> QualityAllocatedToEmpId { get; set; }
        public string QualityAllocatedTo { get; set; }
        public Nullable<int> QualityWorkedTime { get; set; }
        public Nullable<int> QualityBreakTime { get; set; }
        public Nullable<System.DateTime> QualityCompletedDateTime { get; set; }
        public string SOAllocatedBy { get; set; }
        public Nullable<int> SOAllocatedToEmpId { get; set; }
        public string SOAllocatedTo { get; set; }
        public Nullable<int> SOWorkedTime { get; set; }
        public Nullable<System.DateTime> SOCompletedDateTime { get; set; }
        public string BPAllocatedBy { get; set; }
        public Nullable<int> BPAllocatedToEmpId { get; set; }
        public string BPAllocatedTo { get; set; }
        public Nullable<int> BPWorkedTime { get; set; }
        public Nullable<System.DateTime> BPCompletedDateTime { get; set; }
        public string ProofReadingAllocatedBy { get; set; }
        public Nullable<int> ProofReadingAllocatedToEmpId { get; set; }
        public string ProofReadingAllocatedTo { get; set; }
        public Nullable<int> ProofreadingWorkedTime { get; set; }
        public Nullable<System.DateTime> ProofReadingCompletedDateTime { get; set; }
        public string UploadedBy { get; set; }
        public Nullable<System.DateTime> UploadedUTC { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedUTC { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
        public string EmployeeName { get; set; }
        public List<EmployeeReport> Employee { get; set; }
    }
    public class HourlyPara
    {
        public DateTime Date { get; set; }
        public Nullable<int> DivisionId { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public string Process { get; set; }
    }
    public class HourlyReportVM
    {
        public Nullable<System.DateTime> Date { get; set; }
        public string DivisionName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string ClientShortName { get; set; }
        public string Process { get; set; }
        public Nullable<int> AC0 { get; set; }
        public Nullable<int> AC1 { get; set; }
        public Nullable<int> AC2 { get; set; }
        public Nullable<int> AC3 { get; set; }
        public Nullable<int> AC4 { get; set; }
        public Nullable<int> AC5 { get; set; }
        public Nullable<int> AC6 { get; set; }
        public Nullable<int> AC7 { get; set; }
        public Nullable<int> AC8 { get; set; }
        public Nullable<int> AC9 { get; set; }
        public Nullable<int> AC10 { get; set; }
        public Nullable<int> AC11 { get; set; }
        public Nullable<int> AC12 { get; set; }
        public Nullable<int> AC13 { get; set; }
        public Nullable<int> AC14 { get; set; }
        public Nullable<int> AC15 { get; set; }
        public Nullable<int> AC16 { get; set; }
        public Nullable<int> AC17 { get; set; }
        public Nullable<int> AC18 { get; set; }
        public Nullable<int> AC19 { get; set; }
        public Nullable<int> AC20 { get; set; }
        public Nullable<int> AC21 { get; set; }
        public Nullable<int> AC22 { get; set; }
        public Nullable<int> AC23 { get; set; }
        public Nullable<int> CC0 { get; set; }
        public Nullable<int> CC1 { get; set; }
        public Nullable<int> CC2 { get; set; }
        public Nullable<int> CC3 { get; set; }
        public Nullable<int> CC4 { get; set; }
        public Nullable<int> CC5 { get; set; }
        public Nullable<int> CC6 { get; set; }
        public Nullable<int> CC7 { get; set; }
        public Nullable<int> CC8 { get; set; }
        public Nullable<int> CC9 { get; set; }
        public Nullable<int> CC10 { get; set; }
        public Nullable<int> CC11 { get; set; }
        public Nullable<int> CC12 { get; set; }
        public Nullable<int> CC13 { get; set; }
        public Nullable<int> CC14 { get; set; }
        public Nullable<int> CC15 { get; set; }
        public Nullable<int> CC16 { get; set; }
        public Nullable<int> CC17 { get; set; }
        public Nullable<int> CC18 { get; set; }
        public Nullable<int> CC19 { get; set; }
        public Nullable<int> CC20 { get; set; }
        public Nullable<int> CC21 { get; set; }
        public Nullable<int> CC22 { get; set; }
        public Nullable<int> CC23 { get; set; }
        public string Type { get; set; }
    }
    public class TATPara
    {
        public DateTime Date { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> ClientId { get; set; }
    }
    public class TATViewModel
    {
        public int JId { get; set; }
        public Nullable<System.DateTime> JobSubmittedDateEST { get; set; }
        public Nullable<System.DateTime> DateofUploadEST { get; set; }
        public string TATExceed { get; set; }
        public string TATinHHMM { get; set; }
        public string JobId { get; set; }
        public string FileName { get; set; }
        public string Scope { get; set; }
        public Nullable<long> StitchCount { get; set; }
        public string Process { get; set; }
        public string Status { get; set; }
    }
}

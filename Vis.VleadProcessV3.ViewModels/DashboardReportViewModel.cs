using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.ViewModels
{
    public class DashboardReportViewModel
    {
        public string ReportName { get; set; }
        public string ComparisonType { get; set; }
        public string FromComparison { get; set; }
        public string ToComparison { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Department { get; set; }
        public int[] CustomerId { get; set; }
        public string DateFormat { get; set; }
        public int?[] ScopeId { get; set; }
    }

    //Dashboard Report sample
    public class GetDashboardPara
    {
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }
        public string Division { get; set; }
        public Nullable<int> DivisionId { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<int> Norms { get; set; }
        public Nullable<int> FreshCount { get; set; }
        public Nullable<int> RevisionCount { get; set; }
        public Nullable<int> DeficitExceed { get; set; }
        public Nullable<int> Achieved { get; set; }
        public Nullable<int> RevenueUSD { get; set; }
        public Nullable<int> RevenueINR { get; set; }

    }

    public class GetDashboardFSRPara
    {
        //public int? ClientId { get; set; }
        //public int? DepartmentId { get; set; }
        public int? DivisionId { get; set; }
        public string Division { get; set; }
        public int? Inventory { get; set; }
        public int? TodayFileReceived { get; set; }
    }

    public class GetDashSummaryPara
    {
        public string Shift { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
    public class GetDashSummaryListPara
    {
        public string Shift { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int DivisionId { get; set; }
    }
    public class GetCurrentDashSummaryListPara
    {
        //public DateTime FromDate { get; set; }
        //public DateTime ToDate { get; set; }
        public DateTime CurrentDatetime { get; set; }
        public int DivisionId { get; set; }
    }

    public class GetDashSummaryViewModel
    {
        public int? DivisionId { get; set; }
        public string Division { get; set; }
        public string TLName { get; set; }
        public double? TotalTeamStrength { get; set; }
        public double? TotalPresent { get; set; }
        public double? TotalAbsent { get; set; }
        public double? TotalCapacity { get; set; }
        public int? TotalCompletedCount { get; set; }
        public double? TotalDeficitExceed { get; set; }
        public decimal? TotalRevenueUSD { get; set; }
        public decimal? TotalRevenueINR { get; set; }
    }
    public class GetCurrentDashSummaryViewModel
    {
        public int? DivisionId { get; set; }
        public string Division { get; set; }
        public string TLName { get; set; }
        public double? TotalTeamStrength { get; set; }
        //public double? TotalPresent { get; set; }
        //public double? TotalAbsent { get; set; }
        //public double? TotalCapacity { get; set; }
        public int Norms { get; set; }
        public int FreshCount { get; set; }
        public int RevisionCount { get; set; }
        public int? TotalCompletedCount { get; set; }
        public double? TotalDeficitExceed { get; set; }
        public decimal? TotalRevenueUSD { get; set; }
        public decimal? TotalRevenueINR { get; set; }
        public DateTime CurrentDatetime { get; set; }
    }
    public class GetDashSummaryListViewModel
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int Norms { get; set; }
        public int FreshCount { get; set; }
        public int BenchCount { get; set; }
        public int QCCompletedCount { get; set; }
        public decimal DeficitExceed { get; set; }
        public int RevisionCount { get; set; }
        public int OverAllArchieved { get; set; }
        public decimal? RevenueUSD { get; set; }
        public decimal? RevenueINR { get; set; }
        public int OutSource { get; set; }
    }
    public class GetCurrentDashSummaryListViewModel
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int Norms { get; set; }
        public int FreshCount { get; set; }
        public int BenchCount { get; set; }
        public int QCCompletedCount { get; set; }
        public decimal DeficitExceed { get; set; }
        public int RevisionCount { get; set; }
        public int OverAllArchieved { get; set; }
        public decimal? RevenueUSD { get; set; }
        public decimal? RevenueINR { get; set; }
        public int OutSource { get; set; }
    }
    public class GetDashSummarySingleViewModel
    {
        public int TotalCount1 { get; set; }
        public int TotalCount2 { get; set; }
        public int TotalCount3 { get; set; }
        public int TotalCount4 { get; set; }
        public int TotalCount5 { get; set; }
        public int TotalCount6 { get; set; }
        public int TotalCount7 { get; set; }
        public int TotalCount8 { get; set; }
        public int TotalCount9 { get; set; }
        public int TotalCount10 { get; set; }
        //
        public double TotalPresent1 { get; set; }
        public double TotalPresent2 { get; set; }
        public double TotalPresent3 { get; set; }
        public double TotalPresent4 { get; set; }
        public double TotalPresent5 { get; set; }
        public double TotalPresent6 { get; set; }
        public double TotalPresent7 { get; set; }
        public double TotalPresent8 { get; set; }
        public double TotalPresent9 { get; set; }
        public double TotalPresent10 { get; set; }
        //
        public int TotalAbsent1 { get; set; }
        public int TotalAbsent2 { get; set; }
        public int TotalAbsent3 { get; set; }
        public int TotalAbsent4 { get; set; }
        public int TotalAbsent5 { get; set; }
        public int TotalAbsent6 { get; set; }
        public int TotalAbsent7 { get; set; }
        public int TotalAbsent8 { get; set; }
        public int TotalAbsent9 { get; set; }
        public int TotalAbsent10 { get; set; }
        //
        public int TotalCompletedCount1 { get; set; }
        public int TotalCompletedCount2 { get; set; }
        public int TotalCompletedCount3 { get; set; }
        public int TotalCompletedCount4 { get; set; }
        public int TotalCompletedCount5 { get; set; }
        public int TotalCompletedCount6 { get; set; }
        public int TotalCompletedCount7 { get; set; }
        public int TotalCompletedCount8 { get; set; }
        public int TotalCompletedCount9 { get; set; }
        public int TotalCompletedCount10 { get; set; }
        //
        public decimal? TotalRevenue1 { get; set; }
        public decimal? TotalRevenue2 { get; set; }
        public decimal? TotalRevenue3 { get; set; }
        public decimal? TotalRevenue4 { get; set; }
        public decimal? TotalRevenue5 { get; set; }
        public decimal? TotalRevenue6 { get; set; }
        public decimal? TotalRevenue7 { get; set; }
        public decimal? TotalRevenue8 { get; set; }
        public decimal? TotalRevenue9 { get; set; }
        public decimal? TotalRevenue10 { get; set; }
    }
    public class GetCurrentDashSummarySingleViewModel
    {
        public int TotalCount1 { get; set; }
        public int TotalCount2 { get; set; }
        public int TotalCount3 { get; set; }
        public int TotalCount4 { get; set; }
        public int TotalCount5 { get; set; }
        public int TotalCount6 { get; set; }
        public int TotalCount7 { get; set; }
        public int TotalCount8 { get; set; }
        public int TotalCount9 { get; set; }
        public int TotalCount10 { get; set; }
        //
        public int TotalFresh1 { get; set; }
        public int TotalFresh2 { get; set; }
        public int TotalFresh3 { get; set; }
        public int TotalFresh4 { get; set; }
        public int TotalFresh5 { get; set; }
        public int TotalFresh6 { get; set; }
        public int TotalFresh7 { get; set; }
        public int TotalFresh8 { get; set; }
        public int TotalFresh9 { get; set; }
        public int TotalFresh10 { get; set; }
        //
        public int TotalRevision1 { get; set; }
        public int TotalRevision2 { get; set; }
        public int TotalRevision3 { get; set; }
        public int TotalRevision4 { get; set; }
        public int TotalRevision5 { get; set; }
        public int TotalRevision6 { get; set; }
        public int TotalRevision7 { get; set; }
        public int TotalRevision8 { get; set; }
        public int TotalRevision9 { get; set; }
        public int TotalRevision10 { get; set; }
        //
        public int TotalCompletedCount1 { get; set; }
        public int TotalCompletedCount2 { get; set; }
        public int TotalCompletedCount3 { get; set; }
        public int TotalCompletedCount4 { get; set; }
        public int TotalCompletedCount5 { get; set; }
        public int TotalCompletedCount6 { get; set; }
        public int TotalCompletedCount7 { get; set; }
        public int TotalCompletedCount8 { get; set; }
        public int TotalCompletedCount9 { get; set; }
        public int TotalCompletedCount10 { get; set; }
        //
        public decimal? TotalRevenue1 { get; set; }
        public decimal? TotalRevenue2 { get; set; }
        public decimal? TotalRevenue3 { get; set; }
        public decimal? TotalRevenue4 { get; set; }
        public decimal? TotalRevenue5 { get; set; }
        public decimal? TotalRevenue6 { get; set; }
        public decimal? TotalRevenue7 { get; set; }
        public decimal? TotalRevenue8 { get; set; }
        public decimal? TotalRevenue9 { get; set; }
        public decimal? TotalRevenue10 { get; set; }
    }
    //
}

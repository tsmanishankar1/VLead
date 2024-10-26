using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Models;
using VisProcess.Models;
using TimeZone = Vis.VleadProcessV3.Models.TimeZone;

public partial class ApplicationDbContext : DbContext
{

    private readonly IConfiguration _configuration;

    public ApplicationDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }
    //adding for single entry service

    public virtual DbSet<AttendanceFileInwardExcel> AttendanceFileInwardExcels { get; set; }

    public virtual DbSet<AttendanceMaster> AttendanceMasters { get; set; }

    public virtual DbSet<AttendanceSheet> AttendanceSheets { get; set; }

    public virtual DbSet<AttendanceTransaction> AttendanceTransactions { get; set; }

    public virtual DbSet<ArtistInventoryReportDept_Result> ArtistInventoryReportDept_Results { get; set; }

    public virtual DbSet<ArtistInventoryReportDiv_Result> ArtistInventoryReportDiv_Results { get; set; }

    public virtual DbSet<ArtDigiOrderEntryInventory_Result> ArtDigiOrderEntryInventory_Results { get; set; }

    public virtual DbSet<ArtDigiFileCount_Result> ArtDigiFileCount_Results { get; set; }

    public virtual DbSet<BankDetail> BankDetails { get; set; }

    public virtual DbSet<BeanchUpload> BeanchUploads { get; set; }

    public virtual DbSet<BenchScheduler> BenchSchedulers { get; set; }

    public virtual DbSet<BenchStatus> BenchStatuses { get; set; }

    public virtual DbSet<BillingCycle> BillingCycles { get; set; }

    public virtual DbSet<BillingCycleMonthly> BillingCycleMonthlies { get; set; }

    public virtual DbSet<CapacityForm> CapacityForm { get; set; }

    public virtual DbSet<Checklist> Checklists { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<City1> Cities1 { get; set; }

    public virtual DbSet<ClientCommunication> ClientCommunications { get; set; }

    public virtual DbSet<ClientOrder> ClientOrders { get; set; }

    public virtual DbSet<ClientOrderExt> ClientOrderExts { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<CustomerCallFollowUp> CustomerCallFollowUps { get; set; }
    public virtual DbSet<CustomerClassification> CustomerClassifications { get; set; }

    public virtual DbSet<CustomerCommunication> CustomerCommunications { get; set; }

    public virtual DbSet<CustomerContact> CustomerContacts { get; set; }

    public virtual DbSet<CustomerDepartment> CustomerDepartments { get; set; }

    public virtual DbSet<CustomerExt> CustomerExts { get; set; }

    public virtual DbSet<CustomerFeedback> CustomerFeedbacks { get; set; }

    public virtual DbSet<CustomerRegistration> CustomerRegistrations { get; set; }

    public virtual DbSet<CustomerTat> CustomerTats { get; set; }

    public virtual DbSet<CustomerVsDivision> CustomerVsDivisions { get; set; }

    public virtual DbSet<CustomerVsEmployee> CustomerVsEmployees { get; set; }

    public virtual DbSet<CustomerVsManager> CustomerVsManagers { get; set; }

    public virtual DbSet<CustomerVsSalesEmployee> CustomerVsSalesEmployees { get; set; }

    public virtual DbSet<CustomerVsScope> CustomerVsScopes { get; set; }

    public virtual DbSet<CustomersNorm> CustomersNorms { get; set; }

    public virtual DbSet<DailyBench> DailyBenches { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Designation> Designations { get; set; }

    public virtual DbSet<Division> Divisions { get; set; }

    public virtual DbSet<DormantClientResult> DormantClientResults { get; set; }
    public virtual DbSet<EmailContactNotification> EmailContactNotifications { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeAssign> EmployeeAssigns { get; set; }

    public virtual DbSet<EmployeeCommunication> EmployeeCommunications { get; set; }

    public virtual DbSet<EmployeeCategoryChange> EmployeeCategoryChanges { get; set; }

    public virtual DbSet<EmployeeCoreDivision> EmployeeCoreDivisions { get; set; }

    public virtual DbSet<EmployeeHierarchy> EmployeeHierarchies { get; set; }

    public virtual DbSet<EmployeeProcess> EmployeeProcesses { get; set; }

    public virtual DbSet<EmployeePerformanceReport> EmployeePerformanceReports { get; set; }

    public virtual DbSet<EmployeeUploadedFile> EmployeeUploadedFiles { get; set; }

    public virtual DbSet<EmployeeVsDivision> EmployeeVsDivisions { get; set; }

    public virtual DbSet<EmployeeVsRole> EmployeeVsRoles { get; set; }

    public virtual DbSet<EmployeeVsSkillset> EmployeeVsSkillsets { get; set; }

    public virtual DbSet<ErrorCategory> ErrorCategories { get; set; }

    public virtual DbSet<ExternalErrorReport> ExternalErrorReports { get; set; }

    public virtual DbSet<FileAttachment> FileAttachments { get; set; }

    public virtual DbSet<FileInwardExcel> FileInwardExcels { get; set; }

    public virtual DbSet<FileInwardType> FileInwardTypes { get; set; }

    public virtual DbSet<FileProcessTray> FileProcessTrays { get; set; }

    public virtual DbSet<FileSummary> FileSummaries { get; set; }

    public virtual DbSet<FiscalBillingCycle> FiscalBillingCycles { get; set; }

    public virtual DbSet<FteProjectedRevenue> FteProjectedRevenues { get; set; }

    public virtual DbSet<Holiday> Holidays { get; set; }

    public virtual DbSet<HolidayMaster> HolidayMasters { get; set; }

    public virtual DbSet<InsuranceClaim> InsuranceClaims { get; set; }

    public virtual DbSet<IntegrationMaster> IntegrationMasters { get; set; }

    public virtual DbSet<IntegrationTran> IntegrationTrans { get; set; }

    public virtual DbSet<InvoiceDateUpdate> InvoiceDateUpdates { get; set; }

    public virtual DbSet<InvoiceJob> InvoiceJobs { get; set; }

    public virtual DbSet<InvoiceMaster> InvoiceMasters { get; set; }

    public virtual DbSet<InvoiceTran> InvoiceTrans { get; set; }

    public virtual DbSet<JobApprovalExt> JobApprovalExts { get; set; }

    public virtual DbSet<JobCategory> JobCategories { get; set; }

    public virtual DbSet<JobFilesLocationMaster> JobFilesLocationMasters { get; set; }

    public virtual DbSet<JobOrder> JobOrders { get; set; }

    public virtual DbSet<JobOrderExtension> JobOrderExtensions { get; set; }

    public virtual DbSet<JobOrderFile> JobOrderFiles { get; set; }

    public virtual DbSet<JobQuery> JobQueries { get; set; }

    public virtual DbSet<JobQuotation> JobQuotations { get; set; }

    public virtual DbSet<JobStatus> JobStatuses { get; set; }

    public virtual DbSet<JobStatusExt> JobStatusExts { get; set; }

    public virtual DbSet<JobSummary> JobSummaries { get; set; }

    public virtual DbSet<Kraapproval> Kraapprovals { get; set; }

    public virtual DbSet<KravsDesignation> KravsDesignations { get; set; }

    public virtual DbSet<LeaveTrack> LeaveTracks { get; set; }

    public virtual DbSet<LeaveRequest> LeaveRequests { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<MailContact> MailContacts { get; set; }

    public virtual DbSet<MailNotificationDetail> MailNotificationDetails { get; set; }

    public virtual DbSet<Mailmstr> Mailmstrs { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<MonthlyRevenueResult> MonthlyRevenueResults { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<NonBillable> NonBillables { get; set; }

    public virtual DbSet<Norm> Norms { get; set; }

    public virtual DbSet<OutsourceCount> OutsourceCounts { get; set; }

    public virtual DbSet<Pricing> Pricings { get; set; }

    public virtual DbSet<PricingFileRange> PricingFileRanges { get; set; }

    public virtual DbSet<PricingScope> PricingScopes { get; set; }

    public virtual DbSet<PricingStaffing> PricingStaffings { get; set; }

    public virtual DbSet<PricingType> PricingTypes { get; set; }

    public virtual DbSet<PricingWithScope> PricingWithScopes { get; set; }

    public virtual DbSet<Process> Processes { get; set; }

    public virtual DbSet<ProcessWorkFlow> ProcessWorkFlows { get; set; }

    public virtual DbSet<ProcessWorkFlowMaster> ProcessWorkFlowMasters { get; set; }

    public virtual DbSet<ProcessWorkFlowStatus> ProcessWorkFlowStatuses { get; set; }

    public virtual DbSet<ProcessWorkFlowTran> ProcessWorkFlowTrans { get; set; }

    public virtual DbSet<Proficiency> Proficiencies { get; set; }

    public virtual DbSet<Receivable> Receivables { get; set; }

    public virtual DbSet<ReceivableAdjustment> ReceivableAdjustments { get; set; }

    public virtual DbSet<ReceivableExt> ReceivableExts { get; set; }

    public virtual DbSet<RemovalReason> RemovalReasons { get; set; }

    public virtual DbSet<ResignReason> ResignReasons { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Scope> Scopes { get; set; }

    public virtual DbSet<Shift> Shifts { get; set; }

    public virtual DbSet<ShiftVsEmployee> ShiftVsEmployees { get; set; }

    public virtual DbSet<Skillset> Skillsets { get; set; }

    public virtual DbSet<StaffingPrice> StaffingPrices { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Statee> Statees { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<StatusMaster> StatusMasters { get; set; }

    public virtual DbSet<TblBankDetail> TblBankDetails { get; set; }

    public virtual DbSet<TblItasset> TblItassets { get; set; }

    public virtual DbSet<TblItassetHardware> TblItassetHardwares { get; set; }

    public virtual DbSet<TblItassetSoftware> TblItassetSoftwares { get; set; }

    public virtual DbSet<TblItassetSoftwareCompliance> TblItassetSoftwareCompliances { get; set; }

    public virtual DbSet<TblItcategory> TblItcategories { get; set; }

    public virtual DbSet<TblItcategoryDetail> TblItcategoryDetails { get; set; }

    public virtual DbSet<TblProductionPlan> TblProductionPlans { get; set; }

    public virtual DbSet<TblVendorDetail> TblVendorDetails { get; set; }

    public virtual DbSet<Temp2> Temp2s { get; set; }

    public virtual DbSet<TimeZone> TimeZones { get; set; }

    public virtual DbSet<TrainingFeedbackForm> TrainingFeedbackForms { get; set; }

    public virtual DbSet<TransactionType> TransactionTypes { get; set; }

    public virtual DbSet<TrayDetailMovement> TrayDetailMovements { get; set; }

    public virtual DbSet<TrayDetailMovementTran> TrayDetailMovementTrans { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<ViewAssignedJob> ViewAssignedJobs { get; set; }

    public virtual DbSet<ViewAutoGetGenerateforInvoice> ViewAutoGetGenerateforInvoices { get; set; }

    public virtual DbSet<ViewBusinessDay> ViewBusinessDays { get; set; }

    public virtual DbSet<ViewCancelledJob> ViewCancelledJobs { get; set; }

    public virtual DbSet<ViewCollectionsReport> ViewCollectionsReports { get; set; }

    public virtual DbSet<ViewConvertedClientOrder> ViewConvertedClientOrders { get; set; }

    public virtual DbSet<ViewCurrentTrayFileStatusReport> ViewCurrentTrayFileStatusReports { get; set; }

    public virtual DbSet<ViewCustomerAddresswithLocation> ViewCustomerAddresswithLocations { get; set; }

    public virtual DbSet<ViewCustomerDetail> ViewCustomerDetails { get; set; }

    public virtual DbSet<ViewCustomerNorm> ViewCustomerNorms { get; set; }

    public virtual DbSet<ViewCustomerStatus> ViewCustomerStatuses { get; set; }

    public virtual DbSet<ViewCustomerVsEmployee> ViewCustomerVsEmployees { get; set; }

    public virtual DbSet<ViewCustomerVsProcess> ViewCustomerVsProcesses { get; set; }

    public virtual DbSet<ViewDailyOperationsReport> ViewDailyOperationsReports { get; set; }

    public virtual DbSet<ViewDailyProductionReport> ViewDailyProductionReports { get; set; }

    public virtual DbSet<ViewDailyReport> ViewDailyReports { get; set; }

    public virtual DbSet<ViewDashBoardStatus> ViewDashBoardStatuses { get; set; }

    public virtual DbSet<ViewDashboardReportFsr> ViewDashboardReportFsrs { get; set; }

    public virtual DbSet<ViewDepartmentBasedCustomer> ViewDepartmentBasedCustomers { get; set; }

    public virtual DbSet<ViewDivisionDetail> ViewDivisionDetails { get; set; }

    public virtual DbSet<ViewEmployeeDetail> ViewEmployeeDetails { get; set; }

    public virtual DbSet<ViewEmployeeDetailsWithAdd> ViewEmployeeDetailsWithAdds { get; set; }

    public virtual DbSet<ViewEmployeeDetailsWithAddress> ViewEmployeeDetailsWithAddresses { get; set; }

    public virtual DbSet<ViewEmployeeDetailsfromEmp> ViewEmployeeDetailsfromEmps { get; set; }

    public virtual DbSet<ViewEmployeeDetailswithShift> ViewEmployeeDetailswithShifts { get; set; }

    public virtual DbSet<ViewEmployeeWiseReport> ViewEmployeeWiseReports { get; set; }

    public virtual DbSet<ViewEmployeeWithShift> ViewEmployeeWithShifts { get; set; }

    public virtual DbSet<ViewFileInwardExcel> ViewFileInwardExcels { get; set; }

    public virtual DbSet<ViewFileStatusReport> ViewFileStatusReports { get; set; }

    public virtual DbSet<ViewFileStatusReportRef> ViewFileStatusReportRefs { get; set; }

    public virtual DbSet<ViewFileStatusReportRefXerox> ViewFileStatusReportRefXeroxes { get; set; }

    public virtual DbSet<ViewGeneratedInvoice> ViewGeneratedInvoices { get; set; }

    public virtual DbSet<ViewGetBirthandJoining> ViewGetBirthandJoinings { get; set; }

    public virtual DbSet<ViewGetCancelledInvoiceList> ViewGetCancelledInvoiceLists { get; set; }

    public virtual DbSet<ViewGetCheckWaiverJob> ViewGetCheckWaiverJobs { get; set; }

    public virtual DbSet<ViewGetConfirmedGeneratedInvoice> ViewGetConfirmedGeneratedInvoices { get; set; }

    public virtual DbSet<ViewGetCustomerforInvoiceCancel> ViewGetCustomerforInvoiceCancels { get; set; }

    public virtual DbSet<ViewGetCustomerforSelection> ViewGetCustomerforSelections { get; set; }

    public virtual DbSet<ViewGetDropInvoiceCancel> ViewGetDropInvoiceCancels { get; set; }

    public virtual DbSet<ViewGetEmployeesForPendingJob> ViewGetEmployeesForPendingJobs { get; set; }

    public virtual DbSet<ViewGetGenerateforInvoice> ViewGetGenerateforInvoices { get; set; }

    public virtual DbSet<ViewGetIntegrationTran> ViewGetIntegrationTrans { get; set; }

    public virtual DbSet<ViewGetInvoiceMaster> ViewGetInvoiceMasters { get; set; }

    public virtual DbSet<ViewGetInvoiceMasterforDayDiff> ViewGetInvoiceMasterforDayDiffs { get; set; }

    public virtual DbSet<ViewGetPricingforInvoice> ViewGetPricingforInvoices { get; set; }

    public virtual DbSet<ViewGetTranMergeInvoiceCancel> ViewGetTranMergeInvoiceCancels { get; set; }

    public virtual DbSet<ViewGetTransactionTypeCancel> ViewGetTransactionTypeCancels { get; set; }

    public virtual DbSet<ViewGetTransactionTypeInvoice> ViewGetTransactionTypeInvoices { get; set; }

    public virtual DbSet<ViewGetUser> ViewGetUsers { get; set; }

    public virtual DbSet<ViewGetWorkflowListforLogin> ViewGetWorkflowListforLogins { get; set; }

    public virtual DbSet<ViewInvoiceCompletedList> ViewInvoiceCompletedLists { get; set; }

    public virtual DbSet<ViewJobAllocation> ViewJobAllocations { get; set; }

    public virtual DbSet<ViewJobAllocationandJobOrder> ViewJobAllocationandJobOrders { get; set; }

    public virtual DbSet<ViewJobDetail> ViewJobDetails { get; set; }

    public virtual DbSet<ViewJobForcompletedJob> ViewJobForcompletedJobs { get; set; }

    public virtual DbSet<ViewJobMovement> ViewJobMovements { get; set; }

    public virtual DbSet<ViewJobOrder> ViewJobOrders { get; set; }

    public virtual DbSet<ViewJobOrderforFileUpload> ViewJobOrderforFileUploads { get; set; }

    public virtual DbSet<ViewJobQueryInCc> ViewJobQueryInCcs { get; set; }

    public virtual DbSet<ViewJobsAllocatedSummary> ViewJobsAllocatedSummaries { get; set; }

    public virtual DbSet<ViewLogtext> ViewLogtexts { get; set; }

    public virtual DbSet<ViewMailContact> ViewMailContacts { get; set; }

    public virtual DbSet<ViewOperationsReport> ViewOperationsReports { get; set; }

    public virtual DbSet<ViewQueryTracker> ViewQueryTrackers { get; set; }

    public virtual DbSet<ViewQuotationDetailsinJobOrder> ViewQuotationDetailsinJobOrders { get; set; }

    public virtual DbSet<ViewQuotationJobDetail> ViewQuotationJobDetails { get; set; }

    public virtual DbSet<ViewQuotationTracker> ViewQuotationTrackers { get; set; }

    public virtual DbSet<ViewShiftVsEmployee> ViewShiftVsEmployees { get; set; }

    public virtual DbSet<ViewShiftVsEmployeeShift> ViewShiftVsEmployeeShifts { get; set; }

    public virtual DbSet<ViewTatreport> ViewTatreports { get; set; }

    public virtual DbSet<ViewVoucherControler> ViewVoucherControlers { get; set; }

    public virtual DbSet<VisClosureResult> VisClosureResults { get; set; }

    public virtual DbSet<VoucherControl> VoucherControls { get; set; }

    public virtual DbSet<WellnessStatus> WellnessStatuses { get; set; }
    //sp
    public virtual DbSet<GetCustomerDetails_Result> GetCustomerDetails { get; set; }
    public virtual DbSet<GetInactiveClients_Result> GetInactiveClients_Result { get; set; }
    public virtual DbSet<JobAllocation_Result> JobAllocation_Results { get; set; }
    public virtual DbSet<ListofBenchWork_Result> ListofBenchWork_Results { get; set; }
    public virtual DbSet<JobHistoryMovement_Result> JobHistoryMovement_Results { get; set; }
    public virtual DbSet<Inventory> Inventory { get; set; }
    public virtual DbSet<InventoryForm_Result> InventoryForm_Results { get; set; }
    public virtual DbSet<ClientOrderCCJobs_Result> ClientOrderCCJobs_Results { get; set; }
    public virtual DbSet<GetCustomerFeedbackReportDetails_Result> GetCustomerFeedbackReportDetails_Results { get; set; }
    public virtual DbSet<GetCustomersWithNullPriceForScope_Result> GetCustomersWithNullPriceForScope_Result { get; set; }
    public virtual DbSet<GetScopeByDeptIdCustId_Result> GetScopeByDeptIdCustId_Results { get; set; }
    public virtual DbSet<GetCustomerTATByCustId_Result> GetCustomerTATByCustId_Results { get; set; }
    public virtual DbSet<GetJobStatusByCustId_Result> GetJobStatusByCustId_Results { get; set; }
    public virtual DbSet<GetAllCustomerSalesEmployee_Result> GetAllCustomerSalesEmployee_Results { get; set; }
    public virtual DbSet<GetCustomersApprovedBySales_Result> GetCustomersApprovedBySales_Results { get; set; }
    public virtual DbSet<GetCustomersUnApprovedBySales_Result> GetCustomersUnApprovedBySales_Results { get; set; }
    public virtual DbSet<GetCustomersAppUnAppbySalesCount_Result> GetCustomersAppUnAppbySalesCount_Results { get; set; }
    public virtual DbSet<GetCustomerRegDetails_Result> GetCustomerRegDetails_Results { get; set; }
    public virtual DbSet<GetCustomerContactCredentials_Result> GetCustomerContactCredentials_Results { get; set; }
    public virtual DbSet<GetMonthwiseRevenuePerHeadReport_Result> GetMonthwiseRevenuePerHeadReport_Results { get; set; }
    public virtual DbSet<GetUnbilledClient_Result> GetUnbilledClient_Results { get; set; }
    public virtual DbSet<GetJobOrdersByPeriod_Result> GetJobOrdersByPeriod_Results { get; set; }
    public virtual DbSet<ScopeforPricing_Result> ScopeforPricing_Results { get; set; }
    public virtual DbSet<DashBoardStatus_Result> DashBoardStatus_Results { get; set; }
    public virtual DbSet<AverageRevenue_Result> AverageRevenue_Results { get; set; }
    public virtual DbSet<TrayWisePendingStatus_Result> TrayWisePendingStatus_Results { get; set; }
    public virtual DbSet<GetDetailedFileCount_Result> GetDetailedFileCount_Results { get; set; }
    public virtual DbSet<JobQueryHistory_Result> JobQueryHistory_Results { get; set; }
    public virtual DbSet<JobProdHistory_Result> JobProdHistory_Results { get; set; }
    public virtual DbSet<sp_NonBillableJobs_Result> Sp_NonBillableJobs_Results { get; set; }
    public virtual DbSet<GetAgeingOutstandingSummary_Result> GetAgeingOutstandingSummary_Results { get; set; }
    public virtual DbSet<GetReceivablesforIntegrationMaster_Result> GetReceivablesforIntegrationMaster_Results { get; set; }
    public virtual DbSet<GetAllReceivables_Result> GetAllReceivables_Results { get; set; }
    public virtual DbSet<GetJobOrderReportDetails_Result> GetJobOrderReportDetails_Results { get; set; }
    public virtual DbSet<InvoiceReport_Result> InvoiceReport_Results { get; set; }
    public virtual DbSet<MonthWiseRevenueReport_Result> MonthWiseRevenueReport_Results { get; set; }
    public virtual DbSet<RevenueAnalysisReport_Result> RevenueAnalysisReport_Results { get; set; }
    public virtual DbSet<TrendReport_Result> TrendReport_Results { get; set; }
    public virtual DbSet<TrendDetail_Result> TrendDetail_Results { get; set; }
    public virtual DbSet<TATReport_Result> TATReport_Results { get; set; }
    public virtual DbSet<TATReport_Vlead_Result> TATReport_Vlead_Results { get; set; }
    public virtual DbSet<FileStatusReportRefModify_Result> FileStatusReportRefModify_Results { get; set; }
    public virtual DbSet<CurrentTrayFileStatusReport_Result> CurrentTrayFileStatusReport_Results { get; set; }
    public virtual DbSet<GetPendingPricingDetailForSales_Result> GetPendingPricingDetailForSales_Result { get; set; }
    public virtual DbSet<GetQuoteTrackJobs_Result> GetQuoteTrackJobs_Results { get; set; }
    public virtual DbSet<GetClientStatus_Result> GetClientStatus_Results { get; set; }
    public virtual DbSet<GetNewClientReport_Result> GetNewClientReport_Results { get; set; }
    public virtual DbSet<GetTrialClientReport_Result> GetTrialClientReport_Results { get; set; }
    public virtual DbSet<GetTrialClientSummaryReport_Result> GetTrialClientSummaryReport_Results { get; set; }
    public virtual DbSet<GetRevisionCount_Result> GetRevisionCount_Results { get; set; }
    public virtual DbSet<ErrorReport_Result> ErrorReport_Results { get; set; }
    public virtual DbSet<QCErrorReport_Result> QCErrorReport_Results { get; set; }
    public virtual DbSet<GetFTEUtilizationReport_Result> GetFTEUtilizationReport_Results { get; set; }
    public virtual DbSet<GetFTEUtilizationWithClientWiseReport_Result> GetFTEUtilizationWithClientWiseReport_Results { get; set; }
    public virtual DbSet<GetCCIntakeAndUploadedReport_Result> GetCCIntakeAndUploadedReport_Results { get; set; }
    public virtual DbSet<CancelledJobReport_Result> CancelledJobReport_Results { get; set; }
    public virtual DbSet<OSFileStatusReportModify_Result> OSFileStatusReportModify_Results { get; set; }
    public virtual DbSet<QueryFileStatusReport_Result> QueryFileStatusReport_Results { get; set; }
    public virtual DbSet<UnbilledRevenueReport_Result> UnbilledRevenueReport_Results { get; set; }
    public virtual DbSet<TotalNofreshfiles_Result> TotalNofreshfiles_Results { get; set; }
    public virtual DbSet<TotalNoSewOutfiles_Result> TotalNoSewOutfiles_Results { get; set; }
    public virtual DbSet<CollectionsReport_Result> CollectionsReport_Results { get; set; }
    public virtual DbSet<GetQueryTrackJobs_Result> GetQueryTrackJobs_Results { get; set; }
    public virtual DbSet<GetQueryTrackJobDetails_Result> GetQueryTrackJobDetails_Results { get; set; }
    public virtual DbSet<GetQueryArtistDetails_Result> GetQueryArtistDetails_Results { get; set; }
    public virtual DbSet<GetEmployeeDetailsReport_Result> GetEmployeeDetailsReport_Results { get; set; }
    public virtual DbSet<GetEmployeeDetailsTotalReport_Result> GetEmployeeDetailsTotalReport_Results { get; set; }
    public virtual DbSet<GetEmployeeGrandDetailsReport_Result> GetEmployeeGrandDetailsReport_Results { get; set; }
    public virtual DbSet<GetEmployeeGrandDetailsTotalReport_Result> GetEmployeeGrandDetailsTotalReport_Results { get; set; }
    public virtual DbSet<GetEmployeeGrandDetailsReport_Hold_Result> GetEmployeeGrandDetailsReport_Hold_Results { get; set; }
    public virtual DbSet<DailyProductionReportRef_Result> DailyProductionReportRef_Results { get; set; }
    public virtual DbSet<QueryTrackerSummaryDetails_Result> QueryTrackerSummaryDetails_Results { get; set; }
    public virtual DbSet<spAttendanceData_Result> spAttendanceData_Results { get; set; }
    public virtual DbSet<DashboardReportFSR_Result> DashboardReportFSR_Results { get; set; }
    public virtual DbSet<sp_GetEmployeebyDivision_Result> sp_GetEmployeebyDivision_Results { get; set; }
    public virtual DbSet<sp_HourlyCCReport_Result> sp_HourlyCCReport_Results { get; set; }
    public virtual DbSet<sp_HourlyACReport_Result> sp_HourlyACReport_Results { get; set; }
    public virtual DbSet<sp_TATReport_Result> sp_TATReport_Results { get; set; }
    public virtual DbSet<TrailJobsCompletedCountbyClientwise_Result> TrailJobsCompletedCountbyClientwise_Results { get; set; }
    public virtual DbSet<Getclientidanddepartmentidforbillingcycle> Getclientidanddepartmentidforbillingcycles { get; set; }

    // view
    public virtual DbSet<JobHistoryMovementForExcel_Result> JobHistoryMovementForExcel_Results { get; set; }
    //unit procedure
    public virtual DbSet<GetGenerateforInvoice_Result> GetGenerateforInvoice_Results { get; set; }
    public virtual DbSet<GeneratedInvoice_Result> GeneratedInvoice_Results { get; set; }
    public virtual DbSet<GetStaffingAmount_Result> GetStaffingAmount_Results { get; set; }
    public virtual DbSet<GetCheckStaffing_Result> GetCheckStaffing_Results { get; set; }
    public virtual DbSet<GetPricingforTandM_Result> GetPricingforTandM_Results { get; set; }
    public virtual DbSet<GetRateforTandM_Result> GetRateforTandM_Results { get; set; }
    public virtual DbSet<GetDropInvoiceCancel_Result> GetDropInvoiceCancel_Results { get; set; }
    public virtual DbSet<GetFileCountDivisionWiseByManagerId_Result> GetFileCountDivisionWiseByManagerId_Result { get; set; }
    public virtual DbSet<GetCustomersByManager_Result> GetCustomersByManager_Result { get; set; }
    public virtual DbSet<GetFileCountGivenRange_Result> GetFileCountGivenRange_Result { get; set; }
    public virtual DbSet<GetInactiveClientsScopeWiseByManagerId_result> GetGetInactiveClientsScopeWiseByManagerId_Results { get; set; }
    public virtual DbSet<GetTranMergeInvoiceCancel_Result> GetTranMergeInvoiceCancel_Results { get; set; }
    public virtual DbSet<GetIntegrationMaster_Result> GetIntegrationMaster_Results { get; set; }
    public virtual DbSet<GetIntegrationTran_Result> GetIntegrationTran_Results { get; set; }
    public virtual DbSet<GetReceiptsIntegrationTran_Result> GetReceiptsIntegrationTran_Results { get; set; }
    public virtual DbSet<WaiverJobforInvoice_Result> WaiverJobforInvoice_Results { get; set; }
    public virtual DbSet<SP_SingleTableDPR_Result> SP_SingleTableDPR_Results { get; set; }

    // resolving single entry mapping from sql query
    public virtual DbSet<SingleEntry1> SingleEntries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DBConnection"), option => option.CommandTimeout(300));

    //protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    //{
    //    base.ConfigureConventions(configurationBuilder);


    //   // configurationBuilder.DefaultTypeMapping<GetCustomerDetails_Result1>();
    //}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_CI_AI");
        // modelBuilder.<GetCustomerDetails_Result1>()

        modelBuilder.Entity<AttendanceFileInwardExcel>(entity =>
        {
            entity.ToTable("AttendanceFileInwardExcel");

            entity.Property(e => e.AttendanceDate).HasColumnType("datetime");
            entity.Property(e => e.AttendanceStatus)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeDepartment)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeShift)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AttendanceMaster>(entity =>
        {
            entity.ToTable("AttendanceMaster");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("date");
        });

        modelBuilder.Entity<AttendanceSheet>(entity =>
        {
            entity.HasKey(e => e.Sno).HasName("PK__Attendan__CA1FE4641462D1B7");

            entity.Property(e => e.Sno).ValueGeneratedNever();
            entity.Property(e => e.AttendanceStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BreakHours)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Date)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Designation)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EarlyEntry)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EarlyExit)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExtraHoursWorked)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InTime)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LateEntry)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.OutTime)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductiveHours)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShiftTime)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalHoursWorked)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AttendanceTransaction>(entity =>
        {
            entity.ToTable("AttendanceTransaction");

            entity.Property(e => e.Eid).HasColumnName("EId");
            entity.Property(e => e.Shift).HasMaxLength(20);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<BankDetail>(entity =>
        {
            entity.Property(e => e.AccountNo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address2).IsUnicode(false);
            entity.Property(e => e.Branch)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.EmailId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HolderName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Ifsccode)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("IFSCCode");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNo1)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.SwiftCode)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.TypeOfAccount)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<BeanchUpload>(entity =>
        {
            entity.ToTable("BeanchUpload");

            entity.Property(e => e.CommentsForInternalTraining)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.DeployedDivisionName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Qc).HasColumnName("QC");
            entity.Property(e => e.Type)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.UpdateUtc).HasColumnType("datetime");

            entity.HasOne(d => d.Division).WithMany(p => p.BeanchUploads)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DivisionId");

            entity.HasOne(d => d.Employee).WithMany(p => p.BeanchUploads)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EmployeeId");
        });

        modelBuilder.Entity<BenchScheduler>(entity =>
        {
            entity.ToTable("BenchScheduler");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.FromDateTime).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ToDateTime).HasColumnType("datetime");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.Wfmid).HasColumnName("WFMId");
            entity.Property(e => e.Wftid).HasColumnName("WFTId");

            entity.HasOne(d => d.BenchType).WithMany(p => p.BenchSchedulerBenchTypes)
                .HasForeignKey(d => d.BenchTypeId)
                .HasConstraintName("FK_BenchScheduler_Scope");

            entity.HasOne(d => d.Client).WithMany(p => p.BenchSchedulers)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_BenchScheduler_Customer");

            entity.HasOne(d => d.Employee).WithMany(p => p.BenchSchedulerEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BenchScheduler_Employee");

            entity.HasOne(d => d.ScheduledByNavigation).WithMany(p => p.BenchSchedulerScheduledByNavigations)
                .HasForeignKey(d => d.ScheduledBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BenchScheduler_Employee1");

            entity.HasOne(d => d.Trainning).WithMany(p => p.BenchSchedulerTrainnings)
                .HasForeignKey(d => d.TrainningId)
                .HasConstraintName("FK_BenchScheduler_Scope1");

            entity.HasOne(d => d.Wfm).WithMany(p => p.BenchSchedulers)
                .HasForeignKey(d => d.Wfmid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BenchScheduler_ProcessWorkFlowMaster");

            entity.HasOne(d => d.Wft).WithMany(p => p.BenchSchedulers)
                .HasForeignKey(d => d.Wftid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BenchScheduler_ProcessWorkFlowTran");
        });

        modelBuilder.Entity<BenchStatus>(entity =>
        {
            entity.ToTable("BenchStatus");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Division)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<BillingCycle>(entity =>
        {
            entity.ToTable("BillingCycle");

            entity.Property(e => e.BillingCycleType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<BillingCycleMonthly>(entity =>
        {
            entity.ToTable("billingCycleMonthly");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BillingDate)
                .HasColumnType("date")
                .HasColumnName("billingDate");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("createdUtc");
            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            //entity.Property(e => e.Customername)
            //    .HasMaxLength(25)
            //    .IsUnicode(false)
            //    .HasColumnName("customername");
            entity.Property(e => e.DepartmentId).HasColumnName("departmentId");
            //entity.Property(e => e.Departmentname)
            //    .HasMaxLength(25)
            //    .IsUnicode(false)
            //    .HasColumnName("departmentname");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.UpadatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("upadatedUtc");
            entity.Property(e => e.UpdateBy).HasColumnName("updateBy");

            entity.HasOne(d => d.Customer).WithMany(p => p.BillingCycleMonthlies)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_billingCycleMonthly_customerId");

            entity.HasOne(d => d.Department).WithMany(p => p.BillingCycleMonthlies)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_billingCycleMonthly_departmentId");
        });

        modelBuilder.Entity<Checklist>(entity =>
        {
            entity.ToTable("Checklist");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Customer).WithMany(p => p.Checklists)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Checklist_Customer");

            entity.HasOne(d => d.Dept).WithMany(p => p.Checklists)
                .HasForeignKey(d => d.DeptId)
                .HasConstraintName("FK_Checklist_Department");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__city__3214EC07CC04A3BB");

            entity.ToTable("cities");

            entity.Property(e => e.CityName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CityShortName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.TimeZone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK__city__CountryId__60FC61CA");

            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK__city__StateId__61F08603");
        });

        modelBuilder.Entity<City1>(entity =>
        {
            entity.ToTable("city");

            entity.Property(e => e.CityName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CityShortName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.TimeZone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Country).WithMany(p => p.City1s)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK__city__CountryId__2FBA0BF1");

            entity.HasOne(d => d.State).WithMany(p => p.City1s)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK__city__StateId__30AE302A");
        });

        modelBuilder.Entity<CustomerVsEmployee>(entity =>
        {
            entity.ToTable("CustomerVsEmployee");

            entity.Property(e => e.ApprovedUtc).HasColumnType("datetime");
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.IsAgmapproved).HasColumnName("IsAGMApproved");
            entity.Property(e => e.IsAgmrejected).HasColumnName("IsAGMRejected");
            entity.Property(e => e.RejectedUtc).HasColumnType("datetime");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CurrentProcess).WithMany(p => p.CustomerVsEmployeeCurrentProcesses)
                .HasForeignKey(d => d.CurrentProcessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerVsEmployee_Process");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerVsEmployees)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerVsEmployee_Customer");

            entity.HasOne(d => d.Employee).WithMany(p => p.CustomerVsEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerVsEmployee_Employee");

            entity.HasOne(d => d.NextProcess).WithMany(p => p.CustomerVsEmployeeNextProcesses)
                .HasForeignKey(d => d.NextProcessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerVsEmployee_Process1");
        });

        modelBuilder.Entity<CustomerVsManager>(entity =>
        {
            entity.ToTable("CustomerVsManager");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");
            entity.Property(e => e.EffectiveTo).HasColumnType("datetime");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerVsManagers)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerVsManager_Customer");

            entity.HasOne(d => d.Manager).WithMany(p => p.CustomerVsManagers)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK_CustomerVsManager_Employee");
        });

        modelBuilder.Entity<ClientOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId);

            entity.ToTable("ClientOrder");

            entity.Property(e => e.BatchDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CcemailId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CCEmailId");
            entity.Property(e => e.Ccid).HasColumnName("CCId");
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.FileInwardTypeId).HasDefaultValueSql("((3))");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileUploadPath).HasMaxLength(200);
            entity.Property(e => e.Instruction).IsUnicode(false);
            entity.Property(e => e.JobReferenceId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ParentJobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Podate)
                .HasColumnType("datetime")
                .HasColumnName("PODate");
            entity.Property(e => e.Pono)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PONo");
            entity.Property(e => e.ReceiptMode)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.SalesPersonName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Suggestion).IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Cc).WithMany(p => p.ClientOrders)
                .HasForeignKey(d => d.Ccid)
                .HasConstraintName("FK__ClientOrde__CCId__1D9B5BB6");

            entity.HasOne(d => d.JobStatusNavigation).WithMany(p => p.ClientOrders)
                .HasForeignKey(d => d.JobStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientOrder_JobStatus");
        });

        modelBuilder.Entity<ClientOrderExt>(entity =>
        {
            entity.ToTable("ClientOrderExt");

            entity.Property(e => e.FileDownloadDate).HasColumnType("datetime");

            entity.HasOne(d => d.ClientOrder).WithMany(p => p.ClientOrderExts)
                .HasForeignKey(d => d.ClientOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientOrderExt_ClientOrder");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Company");

            entity.Property(e => e.Address1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address3)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Cinno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CINNo");
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Cstno)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("CSTNo");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Phone1)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Phone2)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Tinno)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("TINNo");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.WebAddress)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Location).WithMany(p => p.Companies)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Company_Location");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Country__3214EC070755DDB6");

            entity.ToTable("Country");

            entity.Property(e => e.CountryName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CountryShortName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.UpdateUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdateUTC");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.ApprovedDate).HasColumnType("datetime");
            entity.Property(e => e.BillingCycleType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BlacklistedReasons)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.CreditLimit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreditLimitAvailed).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CurrencyMode).HasMaxLength(10);
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Fax)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.InputType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.LiveStartDate).HasColumnType("datetime");
            entity.Property(e => e.LostCustomerStatus)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.ModeofSales)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.OutputType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PaymentMode).HasMaxLength(50);
            entity.Property(e => e.Phone1)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.PrivilegedClient).HasMaxLength(50);
            entity.Property(e => e.ReportTimeZone).HasMaxLength(50);
            entity.Property(e => e.RpttimeZoneDifference).HasColumnName("RPTTimeZoneDifference");
            entity.Property(e => e.ShortName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.State).HasMaxLength(100);
            entity.Property(e => e.Timezone).HasMaxLength(100);
            entity.Property(e => e.TimezoneDescription).HasMaxLength(100);
            entity.Property(e => e.TimezoneType).HasMaxLength(100);
            entity.Property(e => e.TrialStartDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Company).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customer_Company");

            entity.HasOne(d => d.CustomerClassification).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CustomerClassificationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customer_CustomerClassification");

            entity.HasOne(d => d.TimeZone).WithMany(p => p.Customers)
                .HasForeignKey(d => d.TimeZoneId)
                .HasConstraintName("FK__Customer__TimeZo__2EDAF651");
        });

        modelBuilder.Entity<CustomerClassification>(entity =>
        {
            entity.ToTable("CustomerClassification");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<CustomerCommunication>(entity =>
        {
            entity.ToTable("CustomerCommunication");

            entity.Property(e => e.Address1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address3)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EstablishmentType)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Phone1)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Phone2)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.WebAddress)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerCommunications)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerC__Custo__7F2BE32F");

            entity.HasOne(d => d.Location).WithMany(p => p.CustomerCommunications)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerCommunication_Location");
        });

        modelBuilder.Entity<CustomerCallFollowUp>(entity =>
        {
            entity.ToTable("CustomerCallFollowUp");

            entity.Property(e => e.Comments).IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.DateTimeOfCall).HasColumnType("datetime");
            entity.Property(e => e.FilePath).IsUnicode(false);
            entity.Property(e => e.ModeOfCall)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PurposeOfCall)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerCallFollowUps)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerCallFollowUp_Customer");
        });

        modelBuilder.Entity<CustomerContact>(entity =>
        {
            entity.Property(e => e.ContactName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.CustomerDeptDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Iscustomer).HasColumnName("ISCustomer");
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.Phone1)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerContacts)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerC__Custo__3493CFA7");
        });

        modelBuilder.Entity<CustomerDepartment>(entity =>
        {
            entity.ToTable("CustomerDepartment");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Ftppassword)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FTPPassword");
            entity.Property(e => e.FtpuserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FTPUserName");
            entity.Property(e => e.Host).IsUnicode(false);
            entity.Property(e => e.InFtppath)
                .IsUnicode(false)
                .HasColumnName("InFTPPath");
            entity.Property(e => e.InputType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OutFtppath)
                .IsUnicode(false)
                .HasColumnName("OutFTPPath");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerDepartments)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerD__Custo__35A7EF71");

            entity.HasOne(d => d.Department).WithMany(p => p.CustomerDepartments)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerDepartment_Department");
        });

        modelBuilder.Entity<CustomerExt>(entity =>
        {
            entity.ToTable("CustomerExt");

            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.LiveDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerExts)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerExt_Customer");
        });

        modelBuilder.Entity<CustomerFeedback>(entity =>
        {
            entity.ToTable("CustomerFeedback");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.RushTat).HasColumnName("RushTAT");
            entity.Property(e => e.Suggestions)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CustomerRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07C29BD5CF");

            entity.ToTable("CustomerRegistration");

            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.EmailId)
                .HasMaxLength(50)
                .HasColumnName("EmailID");
            entity.Property(e => e.Fax)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.Phone1)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("phone1");
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.UserName).HasMaxLength(50);
            entity.Property(e => e.Website).HasMaxLength(100);
        });

        modelBuilder.Entity<CustomerTat>(entity =>
        {
            entity.ToTable("CustomerTAT");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.CustomerShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Tat)
                .HasColumnType("numeric(8, 2)")
                .HasColumnName("TAT");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<CustomerVsDivision>(entity =>
        {
            entity.ToTable("CustomerVsDivision");

            entity.HasIndex(e => new { e.CustomerId, e.DivisionId, e.DeptId }, "NonClusteredIndex-20191128-175006");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerVsDivisions)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_CustomerVsDivision_Customer");

            entity.HasOne(d => d.Dept).WithMany(p => p.CustomerVsDivisions)
                .HasForeignKey(d => d.DeptId)
                .HasConstraintName("FK_CustomerVsDivision_Department");

            entity.HasOne(d => d.Division).WithMany(p => p.CustomerVsDivisions)
                .HasForeignKey(d => d.DivisionId)
                .HasConstraintName("FK_CustomerVsDivision_Divisions");
        });
        // Added customervsemployee .
        modelBuilder.Entity<CustomerVsEmployee>(entity =>
        {
            entity.ToTable("CustomerVsEmployee");

            entity.Property(e => e.ApprovedUtc).HasColumnType("datetime");
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.IsAgmapproved).HasColumnName("IsAGMApproved");
            entity.Property(e => e.RejectedUtc).HasColumnType("datetime");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CurrentProcess).WithMany(p => p.CustomerVsEmployeeCurrentProcesses)
                .HasForeignKey(d => d.CurrentProcessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerVsEmployee_Process");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerVsEmployees)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerVsEmployee_Customer");

            entity.HasOne(d => d.Employee).WithMany(p => p.CustomerVsEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerVsEmployee_Employee");

            entity.HasOne(d => d.NextProcess).WithMany(p => p.CustomerVsEmployeeNextProcesses)
                .HasForeignKey(d => d.NextProcessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerVsEmployee_Process1");
        });

        modelBuilder.Entity<CustomerVsSalesEmployee>(entity =>
        {
            entity.ToTable("CustomerVsSalesEmployee");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerVsSalesEmployees)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerV__Custo__46D27B73");

            entity.HasOne(d => d.Employee).WithMany(p => p.CustomerVsSalesEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerV__Emplo__47C69FAC");
        });

        modelBuilder.Entity<CustomerVsScope>(entity =>
        {
            entity.ToTable("CustomerVsScope");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DeptName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EffectiveFromDate).HasColumnType("datetime");
            entity.Property(e => e.EffectiveToDate).HasColumnType("datetime");
            entity.Property(e => e.ScopeGroupDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ScopeName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerVsScopes)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerVsScope_Customer");

            entity.HasOne(d => d.Dept).WithMany(p => p.CustomerVsScopes)
                .HasForeignKey(d => d.DeptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerVsScope_Department");

            entity.HasOne(d => d.Scope).WithMany(p => p.CustomerVsScopes)
                .HasForeignKey(d => d.ScopeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerVsScope_Scope");
        });

        modelBuilder.Entity<CustomersNorm>(entity =>
        {
            entity.ToTable("CustomersNorm");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.CustomerShortName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<DailyBench>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DailyBen__3214EC07CFD5F679");

            entity.ToTable("DailyBench");

            entity.Property(e => e.Bench).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BenchInMins).HasMaxLength(50);
            entity.Property(e => e.Client).HasMaxLength(100);
            entity.Property(e => e.CreatedUTC)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.DivisionName).HasMaxLength(100);
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.EmployeeName).HasMaxLength(100);
            entity.Property(e => e.Fresh).HasMaxLength(50);
            entity.Property(e => e.Leave).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Prod).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.QC).HasColumnName("QC");
            entity.Property(e => e.Remarks).HasMaxLength(255);
            entity.Property(e => e.Shift).HasMaxLength(50);
            entity.Property(e => e.UpdatedUTC)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Division).WithMany(p => p.DailyBenches)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyBench_Division");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("Department");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<Designation>(entity =>
        {
            entity.ToTable("Designation");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<Division>(entity =>
        {
            entity.Property(e => e.CreateUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreateUTC");
            entity.Property(e => e.DivisionName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdateUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdateUTC");
        });

        modelBuilder.Entity<DormantClientResult>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<EmailContactNotification>(entity =>
        {
            entity.ToTable("EmailContactNotification");

            entity.Property(e => e.CcEmailId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.ToEmailId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Customer).WithMany(p => p.EmailContactNotifications)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmailContactNotification_CustomerContacts");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK_employee");

            entity.ToTable("Employee");

            entity.Property(e => e.BatchNo)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.Property(e => e.BloodGroup)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.DateOfJoining).HasColumnType("datetime");
            entity.Property(e => e.DateOfResignation).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContactName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContactNo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IsInternetConnection)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsSystem)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.NetWorkType).HasMaxLength(100);
            entity.Property(e => e.PersonalEmail).HasMaxLength(50);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ServiceProvider).HasMaxLength(100);
            entity.Property(e => e.SystemConfig).HasMaxLength(100);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Company).WithMany(p => p.Employees)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employee_Company");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Department");

            entity.HasOne(d => d.Designation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DesignationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employee_Designation");

            entity.HasOne(d => d.Profiency).WithMany(p => p.Employees)
                .HasForeignKey(d => d.ProfiencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employee_Proficiency");

            entity.HasOne(d => d.ResignReasonsNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.ResignReasons)
                .HasConstraintName("FK_employee_ResignReasons");
        });

        modelBuilder.Entity<EmployeeAssign>(entity =>
        {
            entity.ToTable("EmployeeAssign");

            entity.HasIndex(e => e.Wftid, "IDX_EmployeeAssign");

            entity.HasIndex(e => e.EmployeeId, "IX_EA_EmployeeId");

            entity.HasIndex(e => e.Status, "IX_EA_Status").IsDescending();

            entity.HasIndex(e => e.Wftid, "IX_EA_WFTId").IsDescending();

            entity.Property(e => e.AllocattedDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Jid).HasColumnName("JId");
            entity.Property(e => e.Remarks).IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.Wfmid).HasColumnName("WFMId");
            entity.Property(e => e.Wftid).HasColumnName("WFTId");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.EmployeeAssignCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeAssign_Employee1");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeAssignEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeAssign_Employee");

            entity.HasOne(d => d.ErrorCategory).WithMany(p => p.EmployeeAssigns)
                .HasForeignKey(d => d.ErrorCategoryId)
                .HasConstraintName("FK_EmployeeAssign_ErrorCategory");

            entity.HasOne(d => d.Process).WithMany(p => p.EmployeeAssigns)
                .HasForeignKey(d => d.ProcessId)
                .HasConstraintName("FK_EmployeeAssign_Process");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.EmployeeAssignUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_EmployeeAssign_Employee2");

            entity.HasOne(d => d.Wft).WithMany(p => p.EmployeeAssigns)
                .HasForeignKey(d => d.Wftid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeAssign_ProcessWorkFlowMaster");
        });

        modelBuilder.Entity<EmployeeCommunication>(entity =>
        {
            entity.ToTable("EmployeeCommunication");

            entity.Property(e => e.Address1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address3)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AddressType)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeCommunications)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeCommunication_Employee");
        });

        modelBuilder.Entity<EmployeeCategoryChange>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_EmployeeCategory");

            entity.ToTable("EmployeeCategoryChange");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.EffectiveFromDate).HasColumnType("datetime");
            entity.Property(e => e.EffectiveToDate).HasColumnType("datetime");
            entity.Property(e => e.NewWorkMode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PreviousWorkMode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.EmployeeCategoryChangeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CRIDS");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeCategoryChangeEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EMPS");
        });

        modelBuilder.Entity<EmployeeCoreDivision>(entity =>
        {
            entity.ToTable("EmployeeCoreDivision");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.EffectiveFromDate).HasColumnType("datetime");
            entity.Property(e => e.EffectiveToDate).HasColumnType("datetime");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<EmployeeHierarchy>(entity =>
        {
            entity.ToTable("EmployeeHierarchy");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SubEmpCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SubEmpName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<EmployeeProcess>(entity =>
        {
            entity.ToTable("EmployeeProcess");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.EmployeeProcessCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeProcess_Employee1");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeProcessEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeProcess_Employee");

            entity.HasOne(d => d.Process).WithMany(p => p.EmployeeProcesses)
                .HasForeignKey(d => d.ProcessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeProcess_Process");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.EmployeeProcessUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_EmployeeProcess_Employee2");
        });

        modelBuilder.Entity<EmployeeUploadedFile>(entity =>
        {
            entity.ToTable("EmployeeUploadedFile");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(50);
            entity.Property(e => e.EmployeeName)
               .HasMaxLength(50)
               .IsUnicode(false);
            entity.Property(e => e.FilePath).IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");
        });

        modelBuilder.Entity<EmployeeVsDivision>(entity =>
        {
            entity.ToTable("EmployeeVsDivision");



            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<EmployeeVsRole>(entity =>
        {
            entity.HasKey(e => e.Erid);

            entity.ToTable("EmployeeVsRole");

            entity.Property(e => e.Erid).HasColumnName("ERID");
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RoleDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<EmployeeVsSkillset>(entity =>
        {
            entity.ToTable("EmployeeVsSkillset");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProficiencyLevel)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.WorkingStatus)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Skillset).WithMany(p => p.EmployeeVsSkillsets)
                .HasForeignKey(d => d.SkillsetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeVsSkillset_SkillId");
        });

        modelBuilder.Entity<ErrorCategory>(entity =>
        {
            entity.ToTable("ErrorCategory");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ErrorCategoryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ErrorCategory_Employee");

            entity.HasOne(d => d.Department).WithMany(p => p.ErrorCategories)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ErrorCategory_Department");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ErrorCategoryUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_ErrorCategory_Employee1");
        });

        modelBuilder.Entity<ExternalErrorReport>(entity =>
        {
            entity.ToTable("ExternalErrorReport");

            entity.Property(e => e.ArtistId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Artist ID");
            entity.Property(e => e.ArtistName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Artist Name");
            entity.Property(e => e.Client)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ClientRevisionComment)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Client revision comment");
            entity.Property(e => e.ClientStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Client Status");
            entity.Property(e => e.Department)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Division)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ErrorType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Error Type");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("File Name");
            entity.Property(e => e.InputMonthYear)
                .HasColumnType("date")
                .HasColumnName("Input Month-Year");
            entity.Property(e => e.JobNumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Job Number");
            entity.Property(e => e.JobStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Job Status");
            entity.Property(e => e.QcId).HasColumnName("QC ID");
            entity.Property(e => e.QcName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("QC Name");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ExternalErrorReportCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CRBY");

            entity.HasOne(d => d.DivisionNavigation).WithMany(p => p.ExternalErrorReports)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DIV");

            entity.HasOne(d => d.Employee).WithMany(p => p.ExternalErrorReportEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EMP");
        });

        modelBuilder.Entity<FileAttachment>(entity =>
        {
            entity.ToTable("FileAttachment");

            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Fileattachment1).HasColumnName("Fileattachment");
        });

        modelBuilder.Entity<FileInwardExcel>(entity =>
        {
            entity.ToTable("FileInwardExcel");

            entity.Property(e => e.ClientJobId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ClientSalesPerson)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ClientStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UploadedBy).HasMaxLength(50);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateofDelivery)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.DateofReceived).
            HasColumnType("datetime");
            entity.Property(e => e.DateofUpload)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Division)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FileInwardMode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Ground)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PoNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Priority)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SalesPersonName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.SchoolName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Style)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TeamCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Temp)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FileInwardType>(entity =>
        {
            entity.ToTable("FileInwardType");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<FileProcessTray>(entity =>
        {
            entity.ToTable("FileProcessTray");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Fsid).HasColumnName("FSId");
            entity.Property(e => e.Fyfrom)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("FYFrom");
            entity.Property(e => e.Fyto)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("FYTo");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<FileSummary>(entity =>
        {
            entity.ToTable("FileSummary");

            entity.Property(e => e.ClientShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Fyfrom)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("FYFrom");
            entity.Property(e => e.Fyto)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("FYTo");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TrayName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<FiscalBillingCycle>(entity =>
        {
            entity.ToTable("FiscalBillingCycle");

            entity.Property(e => e.MonthEight).HasColumnType("datetime");
            entity.Property(e => e.MonthEleven).HasColumnType("datetime");
            entity.Property(e => e.MonthFive).HasColumnType("datetime");
            entity.Property(e => e.MonthFour).HasColumnType("datetime");
            entity.Property(e => e.MonthNine).HasColumnType("datetime");
            entity.Property(e => e.MonthOne).HasColumnType("datetime");
            entity.Property(e => e.MonthSeven).HasColumnType("datetime");
            entity.Property(e => e.MonthSix).HasColumnType("datetime");
            entity.Property(e => e.MonthTen).HasColumnType("datetime");
            entity.Property(e => e.MonthThree).HasColumnType("datetime");
            entity.Property(e => e.MonthTwelve).HasColumnType("datetime");
            entity.Property(e => e.MonthTwo).HasColumnType("datetime");

            entity.HasOne(d => d.BillingCycle).WithMany(p => p.FiscalBillingCycles)
                .HasForeignKey(d => d.BillingCycleId)
                .HasConstraintName("FK_FiscalBillingCycle_FiscalBillingCycle");
        });

        modelBuilder.Entity<FteProjectedRevenue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ProjectedRevenue");

            entity.ToTable("FteProjectedRevenue");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.ProjectedRevenue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<Holiday>(entity =>
        {
            entity.ToTable("Holiday");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.HolidayDate).HasColumnType("datetime");
            entity.Property(e => e.HolidayDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<HolidayMaster>(entity =>
        {
            entity.ToTable("HolidayMaster");

            entity.Property(e => e.CreatedByUtc).HasColumnType("datetime");
            entity.Property(e => e.HolidayDate).HasColumnType("date");
            entity.Property(e => e.HolidayDescription)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HolidayType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedByUtc).HasColumnType("datetime");
        });

        modelBuilder.Entity<InsuranceClaim>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.DocId).HasColumnName("DocID");
            entity.Property(e => e.DocName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.EffectiveDate)
                .HasColumnType("date")
                .HasColumnName("Effective Date");
            entity.Property(e => e.PolId).HasColumnName("PolID");
            entity.Property(e => e.PolNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PolType)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.RecKey).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<IntegrationMaster>(entity =>
        {
            entity.ToTable("IntegrationMaster");

            entity.Property(e => e.Amount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.BankName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.CurrencyName)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.CurrencySymbol)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.DestinationBank)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DiscountAmount).HasColumnType("numeric(8, 2)");
            entity.Property(e => e.DocumentDate).HasColumnType("datetime");
            entity.Property(e => e.DocumentNumber)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.ExchangeRate).HasColumnType("numeric(14, 4)");
            entity.Property(e => e.FailedReason)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Mode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProductValue).HasColumnType("numeric(14, 4)");
            entity.Property(e => e.Roundoff).HasColumnType("numeric(8, 4)");
            entity.Property(e => e.TotalValue).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.TransactionNumber)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.WaiverAmount).HasColumnType("numeric(18, 4)");
        });

        modelBuilder.Entity<IntegrationTran>(entity =>
        {
            entity.ToTable("IntegrationTran");

            entity.Property(e => e.InvoiceDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceNumber)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.Qty).HasColumnType("numeric(7, 2)");
            entity.Property(e => e.Rate).HasColumnType("numeric(10, 4)");
            entity.Property(e => e.Scope)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Value).HasColumnType("numeric(14, 4)");

            entity.HasOne(d => d.IntegrationMaster).WithMany(p => p.IntegrationTrans)
                .HasForeignKey(d => d.IntegrationMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IntegrationMasterId");
        });

        modelBuilder.Entity<InvoiceDateUpdate>(entity =>
        {
            entity.ToTable("InvoiceDateUpdate");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.FromInvoiceNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsApproved).HasDefaultValueSql("((0))");
            entity.Property(e => e.ToInvoiceNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<InvoiceJob>(entity =>
        {
            entity.HasIndex(e => e.Jid, "UQK_JId").IsUnique();

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Imid).HasColumnName("IMId");
            entity.Property(e => e.InvoiceCancelDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceCancelNo)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.It).HasColumnName("IT");
            entity.Property(e => e.Jid).HasColumnName("JId");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Im).WithMany(p => p.InvoiceJobs)
                .HasForeignKey(d => d.Imid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceJobs_InvoiceMaster");

            entity.HasOne(d => d.ItNavigation).WithMany(p => p.InvoiceJobs)
                .HasForeignKey(d => d.It)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceJobs_InvoiceTran");
        });

        modelBuilder.Entity<InvoiceMaster>(entity =>
        {
            entity.ToTable("InvoiceMaster");

            entity.Property(e => e.ArtDiscountAmount).HasColumnType("numeric(14, 2)");
            entity.Property(e => e.ArtDiscountPercent).HasColumnType("numeric(14, 2)");
            entity.Property(e => e.ArtInvoiceAmount).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.ArtWaiver).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DigiDiscountAmount).HasColumnType("numeric(14, 2)");
            entity.Property(e => e.DigiDiscountPercent).HasColumnType("numeric(14, 2)");
            entity.Property(e => e.DigiInvoiceAmount).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.DigiWaiver).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.Discount).HasColumnType("numeric(8, 2)");
            entity.Property(e => e.ExchangeRate).HasColumnType("numeric(14, 4)");
            entity.Property(e => e.InvoiceDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceValue).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.PrimaryDateTime).HasColumnType("datetime");
            entity.Property(e => e.ProductValue).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.ReceivableValue).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.ReferenceDate).HasColumnType("datetime");
            entity.Property(e => e.ReferenceNo)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.RoundOff).HasColumnType("numeric(8, 4)");
            entity.Property(e => e.SecondaryDateTime).HasColumnType("datetime");
            entity.Property(e => e.TotalInvoiceValue).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.Waiver).HasColumnType("numeric(18, 4)");

            entity.HasOne(d => d.BillingCycle).WithMany(p => p.InvoiceMasters)
                .HasForeignKey(d => d.BillingCycleId)
                .HasConstraintName("FK_InvoiceMaster_BillingCycle");

            entity.HasOne(d => d.Customer).WithMany(p => p.InvoiceMasters)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceMaster_Customer");
        });

        modelBuilder.Entity<InvoiceTran>(entity =>
        {
            entity.ToTable("InvoiceTran");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Imid).HasColumnName("IMId");
            entity.Property(e => e.InvoiceDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.Qty).HasColumnType("numeric(8, 2)");
            entity.Property(e => e.Rate).HasColumnType("numeric(10, 4)");
            entity.Property(e => e.ScopeGroupDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Uom)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("UOM");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.Value).HasColumnType("numeric(14, 4)");

            entity.HasOne(d => d.Im).WithMany(p => p.InvoiceTrans)
                .HasForeignKey(d => d.Imid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceTran_InvoiceMaster");

            entity.HasOne(d => d.PricingType).WithMany(p => p.InvoiceTrans)
                .HasForeignKey(d => d.PricingTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceTran_PricingType");

            entity.HasOne(d => d.Scope).WithMany(p => p.InvoiceTrans)
                .HasForeignKey(d => d.ScopeId)
                .HasConstraintName("FK_InvoiceTran_Scope");
        });

        modelBuilder.Entity<JobApprovalExt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobAppro__3214EC0712898728");

            entity.ToTable("JobApprovalExt");

            entity.Property(e => e.ApprovalType)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CreditLimit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Remarks).IsUnicode(false);

            entity.HasOne(d => d.Client).WithMany(p => p.JobApprovalExts)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__JobApprov__Clien__7FD5EEA5");
        });

        modelBuilder.Entity<JobCategory>(entity =>
        {
            entity.ToTable("JobCategory");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<JobFilesLocationMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobFiles__3214EC07B6E105A0");

            entity.ToTable("JobFilesLocationMaster");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.FtpfilePath)
                .IsUnicode(false)
                .HasColumnName("FTPFilePath");
            entity.Property(e => e.ProcessName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<JobOrder>(entity =>
        {
            entity.ToTable("JobOrder");

            entity.HasIndex(e => e.JobStatusId, "IDX_JobOrder");

            entity.HasIndex(e => e.ClientId, "IX_JobOrder_ClientId");

            entity.HasIndex(e => e.DateofUpload, "IX_JobOrder_DateofUpload").IsDescending();

            entity.HasIndex(e => e.JobId, "IX_JobOrder_JobId").IsDescending();

            entity.HasIndex(e => e.JobId, "JO_JOBID").IsUnique();

            entity.Property(e => e.ApparelLogoLocation)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.BatchDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BatchNo)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.CcemailId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CCEmailId");
            entity.Property(e => e.Ccid).HasColumnName("CCId");
            entity.Property(e => e.ClientJobId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ClientJobIddate).HasColumnType("datetime");
            entity.Property(e => e.ClientSalesPerson)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateofDelivery).HasColumnType("datetime");
            entity.Property(e => e.DateofDownload).HasColumnType("datetime");
            entity.Property(e => e.DateofUpload).HasColumnType("datetime");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.FileUploadPath).HasMaxLength(200);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ImprintColors1)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ImprintColors2)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ImprintColors3)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobClosedUtc)
                .HasColumnType("datetime")
                .HasColumnName("JobClosedUTC");
            entity.Property(e => e.JobDate).HasColumnType("datetime");
            entity.Property(e => e.JobDescription).IsUnicode(false);
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobReferenceId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.LogoDimensionWidth)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.LogoDimensionsLength)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ParentJobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.PoNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Podate)
                .HasColumnType("datetime")
                .HasColumnName("PODate");
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.QueryJobDate).HasColumnType("datetime");
            entity.Property(e => e.Remarks).IsUnicode(false);
            entity.Property(e => e.SalesPersonName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.SchoolName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SpecialPrice).HasColumnType("numeric(10, 4)");
            entity.Property(e => e.Style)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TeamCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Temp)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.Username)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.VirtualProof)
                .HasMaxLength(3)
                .IsUnicode(false);

            entity.HasOne(d => d.Cc).WithMany(p => p.JobOrders)
                .HasForeignKey(d => d.Ccid)
                .HasConstraintName("FK__JobOrder__CCId__1CA7377D");
        });

        modelBuilder.Entity<JobOrderExtension>(entity =>
        {
            entity.ToTable("JobOrderExtension");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Jid).HasColumnName("JId");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Wftid).HasColumnName("WFTId");

            entity.HasOne(d => d.JidNavigation).WithMany(p => p.JobOrderExtensions)
                .HasForeignKey(d => d.Jid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobOrderExtension_JobOrder");

            entity.HasOne(d => d.Status).WithMany(p => p.JobOrderExtensions)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__JobOrderE__Statu__07220AB2");

            entity.HasOne(d => d.Wft).WithMany(p => p.JobOrderExtensions)
                .HasForeignKey(d => d.Wftid)
                .HasConstraintName("FK__JobOrderE__WFTId__04459E07");
        });

        modelBuilder.Entity<JobOrderFile>(entity =>
        {
            entity.ToTable("JobOrderFile");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.FileName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.Wfmid).HasColumnName("WFMId");
            entity.Property(e => e.Wftid).HasColumnName("WFTId");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.JobOrderFiles)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_JobOrderFile_Employee");

            entity.HasOne(d => d.Job).WithMany(p => p.JobOrderFiles)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobOrderFile_JobOrder");

            entity.HasOne(d => d.Process).WithMany(p => p.JobOrderFiles)
                .HasForeignKey(d => d.ProcessId)
                .HasConstraintName("FK_JobOrderFile_Process");

            entity.HasOne(d => d.Wfm).WithMany(p => p.JobOrderFiles)
                .HasForeignKey(d => d.Wfmid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobOrderFile_ProcessWorkFlowMaster");

            entity.HasOne(d => d.Wft).WithMany(p => p.JobOrderFiles)
                .HasForeignKey(d => d.Wftid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobOrderFile_ProcessWorkFlowTran");
        });

        modelBuilder.Entity<JobQuery>(entity =>
        {
            entity.ToTable("JobQuery");

            entity.Property(e => e.Comments).IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.DateofDelivery).HasColumnType("datetime");
            entity.Property(e => e.Jid).HasColumnName("JId");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.QueryDate).HasColumnType("datetime");
            entity.Property(e => e.QueryRaisedOn).HasColumnType("datetime");
            entity.Property(e => e.SpecialPrice).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.Wfmid).HasColumnName("WFMId");
            entity.Property(e => e.Wftid).HasColumnName("WFTId");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.JobQueryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_JobQuery_Employee2");

            entity.HasOne(d => d.JidNavigation).WithMany(p => p.JobQueries)
                .HasForeignKey(d => d.Jid)
                .HasConstraintName("FK__JobQuery__JobOrd__64CCF2AE");

            entity.HasOne(d => d.Process).WithMany(p => p.JobQueries)
                .HasForeignKey(d => d.ProcessId)
                .HasConstraintName("FK_JobQuery_Process");

            entity.HasOne(d => d.QueryRaisedByNavigation).WithMany(p => p.JobQueryQueryRaisedByNavigations)
                .HasForeignKey(d => d.QueryRaisedBy)
                .HasConstraintName("FK_JobQuery_Employee");

            entity.HasOne(d => d.Scope).WithMany(p => p.JobQueries)
                .HasForeignKey(d => d.ScopeId)
                .HasConstraintName("FK_JobQuery_Scope");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.JobQueries)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobQuery_Status");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.JobQueryUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_JobQuery_Employee3");

            entity.HasOne(d => d.Wfm).WithMany(p => p.JobQueries)
                .HasForeignKey(d => d.Wfmid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobQuery_ProcessWorkFlowMaster");

            entity.HasOne(d => d.Wft).WithMany(p => p.JobQueries)
                .HasForeignKey(d => d.Wftid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobQuery_JobQuery");
        });

        modelBuilder.Entity<JobQuotation>(entity =>
        {
            entity.ToTable("JobQuotation");

            entity.Property(e => e.Amount).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Comments).IsUnicode(false);
            entity.Property(e => e.DateofDelivery).HasColumnType("datetime");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.QuotationraisedOn).HasColumnType("datetime");
            entity.Property(e => e.Wfmid).HasColumnName("WFMId");
            entity.Property(e => e.Wftid).HasColumnName("WFTId");

            entity.HasOne(d => d.Process).WithMany(p => p.JobQuotations)
                .HasForeignKey(d => d.ProcessId)
                .HasConstraintName("FK_JobQuotation_Process");

            entity.HasOne(d => d.Scope).WithMany(p => p.JobQuotations)
                .HasForeignKey(d => d.ScopeId)
                .HasConstraintName("FK_JobQuotation_Scope");

            entity.HasOne(d => d.Status).WithMany(p => p.JobQuotations)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobQuotation_Status");

            entity.HasOne(d => d.Wfm).WithMany(p => p.JobQuotations)
                .HasForeignKey(d => d.Wfmid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobQuotation_ProcessWorkFlowMaster");

            entity.HasOne(d => d.Wft).WithMany(p => p.JobQuotations)
                .HasForeignKey(d => d.Wftid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobQuotation_ProcessWorkFlowTran");
        });

        modelBuilder.Entity<JobStatus>(entity =>
        {
            entity.ToTable("JobStatus");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ShortDescription)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.JobStatusCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobStatus_CreatedBy");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.JobStatusUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_JobStatus_UpdatedBy");
        });

        modelBuilder.Entity<JobStatusExt>(entity =>
        {
            entity.ToTable("JobStatusExt");

            entity.Property(e => e.Tat)
                .HasColumnType("numeric(5, 2)")
                .HasColumnName("TAT");

            entity.HasOne(d => d.Customer).WithMany(p => p.JobStatusExts)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobStatusExt_Customer");

            entity.HasOne(d => d.JobStatus).WithMany(p => p.JobStatusExts)
                .HasForeignKey(d => d.JobStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobStatusExt_JobStatus");
        });

        modelBuilder.Entity<JobSummary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TATExceedReport");

            entity.ToTable("JobSummary");

            entity.Property(e => e.MonthYear)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Kraapproval>(entity =>
        {
            entity.ToTable("KRAApproval");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Krayear).HasColumnName("KRAYear");

            entity.HasOne(d => d.Department).WithMany(p => p.Kraapprovals)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_KRAApproval_Department");

            entity.HasOne(d => d.Employee).WithMany(p => p.Kraapprovals)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_KRAApproval_Employee");
        });

        modelBuilder.Entity<KravsDesignation>(entity =>
        {
            entity.ToTable("KRAVsDesignation");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Kradescription)
                .IsUnicode(false)
                .HasColumnName("KRADescription");
            entity.Property(e => e.KraisDeleted).HasColumnName("KRAIsDeleted");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<LeaveRequest>(entity =>
        {
            entity.ToTable("LeaveRequest");

            entity.Property(e => e.Comments)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.Property(e => e.EndDate)
                .HasColumnType("datetime");

            entity.Property(e => e.StartDate)
                .HasColumnType("datetime");

            entity.Property(e => e.LeaveCategoryName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LeaveCategory");
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.ToTable("Location");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.DayLightTimezoneDifference).HasMaxLength(100);
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LocationHeaderDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TimezoneDescription).HasMaxLength(100);
            entity.Property(e => e.TimezoneDifference).HasMaxLength(100);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.Zipcode)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.ToTable("Log");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Module)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MailContact>(entity =>
        {
            entity.ToTable("MailContact");

            entity.Property(e => e.Ccmail)
                .IsUnicode(false)
                .HasColumnName("CCMail");
            entity.Property(e => e.Client)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.FromMail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProcessName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ToMail).IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<MailNotificationDetail>(entity =>
        {
            entity.Property(e => e.CcmailId).HasColumnName("CCMailId");
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Task)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<Mailmstr>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Mailmstr__3214EC07FE0C5386");

            entity.ToTable("Mailmstr");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MailType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Tray)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.ToTable("Menu");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_Menu_Menu");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.Property(e => e.ApprovedUtc)
                .HasColumnType("datetime")
                .HasColumnName("ApprovedUTC");
            entity.Property(e => e.Category)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.NewImagePath).HasMaxLength(200);
            entity.Property(e => e.Title)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.News)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_News_Employee");
        });

        modelBuilder.Entity<NonBillable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NonBilla__3214EC077804F901");

            entity.ToTable("NonBillable");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Norm>(entity =>
        {
            entity.ToTable("Norm");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.EffectiveFromDate).HasColumnType("datetime");
            entity.Property(e => e.EffectiveToDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.WorkMode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Client).WithMany(p => p.Norms)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CI");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.NormCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CREID");

            entity.HasOne(d => d.Division).WithMany(p => p.Norms)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DI");

            entity.HasOne(d => d.Employee).WithMany(p => p.NormEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EMPID");

            entity.HasOne(d => d.Scope).WithMany(p => p.Norms)
                .HasForeignKey(d => d.ScopeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SI");
        });

        modelBuilder.Entity<OutsourceCount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Outsourc__3214EC0784662BC9");
            entity.Property(e => e.CreatedUTC)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.OutSourceFilecount).HasColumnName("OutSourceFilecount").HasColumnType("int");
            entity.Property(e => e.OutsourceAmount).HasColumnName("OutsourceAmount").HasColumnType("int");
            entity.Property(e => e.UpdatedUTC)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.HasOne(d => d.Customer).WithMany(p => p.OutsourceCounts)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customer");
            entity.HasOne(d => d.Division).WithMany(p => p.OutsourceCounts)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Divisions");
        });

        modelBuilder.Entity<Pricing>(entity =>
        {
            entity.ToTable("Pricing");

            entity.Property(e => e.FlatRate).HasColumnType("decimal(8, 4)");
            entity.Property(e => e.MaxPricingLimit).HasColumnType("decimal(8, 4)");
            entity.Property(e => e.MoreStitchCount).HasColumnType("decimal(8, 4)");
            entity.Property(e => e.PriceCharged).HasColumnType("decimal(8, 4)");
            entity.Property(e => e.ThousandStitchCharge).HasColumnType("decimal(8, 4)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Pricings)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pricing_Customer");

            entity.HasOne(d => d.Department).WithMany(p => p.Pricings)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pricing_Department");

            entity.HasOne(d => d.PricingType).WithMany(p => p.Pricings)
                .HasForeignKey(d => d.PricingTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pricing_PricingType");
        });

        modelBuilder.Entity<PricingFileRange>(entity =>
        {
            entity.ToTable("PricingFileRange");

            entity.Property(e => e.ThousandStitchCharge).HasColumnType("decimal(8, 4)");

            entity.HasOne(d => d.Pricing).WithMany(p => p.PricingFileRanges)
                .HasForeignKey(d => d.PricingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingFileRange_Pricing");
        });

        modelBuilder.Entity<PricingScope>(entity =>
        {
            entity.ToTable("PricingScope");

            entity.Property(e => e.EstimatedTime).HasMaxLength(10);
            entity.Property(e => e.MaxPrice).HasColumnType("decimal(8, 0)");
            entity.Property(e => e.ModeOfPricing)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(8, 0)");
            entity.Property(e => e.RatePerHour).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.RateperFile).HasColumnType("decimal(8, 0)");

            entity.HasOne(d => d.Pricing).WithMany(p => p.PricingScopes)
                .HasForeignKey(d => d.PricingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingScope_Pricing");

            entity.HasOne(d => d.Scope).WithMany(p => p.PricingScopes)
                .HasForeignKey(d => d.ScopeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingScope_Scope");
        });

        modelBuilder.Entity<PricingStaffing>(entity =>
        {
            entity.ToTable("PricingStaffing");

            entity.Property(e => e.RatePerMonth).HasColumnType("decimal(8, 0)");

            entity.HasOne(d => d.Designation).WithMany(p => p.PricingStaffings)
                .HasForeignKey(d => d.DesignationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingStaffing_Designation");

            entity.HasOne(d => d.Pricing).WithMany(p => p.PricingStaffings)
                .HasForeignKey(d => d.PricingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingStaffing_Pricing");
        });

        modelBuilder.Entity<PricingType>(entity =>
        {
            entity.ToTable("PricingType");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PricingTypeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingType_Employee");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PricingTypeUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_PricingType_Employee1");
        });

        modelBuilder.Entity<PricingWithScope>(entity =>
        {
            entity.ToTable("PricingWithScope");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Designation)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.EstimationTime).HasMaxLength(10);
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(8, 4)");
            entity.Property(e => e.RatePerHour).HasColumnType("decimal(8, 4)");
            entity.Property(e => e.ScopeTempDesc)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.WefromDate)
                .HasColumnType("datetime")
                .HasColumnName("WEFromDate");
            entity.Property(e => e.WetoDate)
                .HasColumnType("datetime")
                .HasColumnName("WEToDate");

            entity.HasOne(d => d.Customer).WithMany(p => p.PricingWithScopes)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingWithScope_Customer");

            entity.HasOne(d => d.Department).WithMany(p => p.PricingWithScopes)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingWithScope_Department");

            entity.HasOne(d => d.PricingType).WithMany(p => p.PricingWithScopes)
                .HasForeignKey(d => d.PricingTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingWithScope_PricingType");

            entity.HasOne(d => d.Scope).WithMany(p => p.PricingWithScopes)
                .HasForeignKey(d => d.ScopeId)
                .HasConstraintName("FK_PricingWithScope_Scope");
        });

        modelBuilder.Entity<Process>(entity =>
        {
            entity.ToTable("Process");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .IsUnicode(false)
                .HasComment("");
            entity.Property(e => e.ShortName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProcessCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Process_Employee");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ProcessUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_Process_Employee1");
        });

        modelBuilder.Entity<ProcessWorkFlow>(entity =>
        {
            entity.ToTable("ProcessWorkFlow");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.CustomJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProcessWorkFlowCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcessWorkFlow_Employee");

            entity.HasOne(d => d.CurrentProcess).WithMany(p => p.ProcessWorkFlowCurrentProcesses)
                .HasForeignKey(d => d.CurrentProcessId)
                .HasConstraintName("FK_ProcessWorkFlow_Process");

            entity.HasOne(d => d.Customer).WithMany(p => p.ProcessWorkFlows)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_ProcessWorkFlow_Customer");

            entity.HasOne(d => d.Department).WithMany(p => p.ProcessWorkFlows)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcessWorkFlow_Department");

            entity.HasOne(d => d.NextProcess).WithMany(p => p.ProcessWorkFlowNextProcesses)
                .HasForeignKey(d => d.NextProcessId)
                .HasConstraintName("FK_ProcessWorkFlow_Process1");

            entity.HasOne(d => d.Status).WithMany(p => p.ProcessWorkFlows)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcessWorkFlow_Status");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ProcessWorkFlowUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_ProcessWorkFlow_Employee1");
        });

        modelBuilder.Entity<ProcessWorkFlowMaster>(entity =>
        {
            entity.ToTable("ProcessWorkFlowMaster");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Customer).WithMany(p => p.ProcessWorkFlowMasters)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcessWorkFlowMaster_Customer");

            entity.HasOne(d => d.Department).WithMany(p => p.ProcessWorkFlowMasters)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcessWorkFlowMaster_Department");

            entity.HasOne(d => d.JobCategory).WithMany(p => p.ProcessWorkFlowMasters)
                .HasForeignKey(d => d.JobCategoryId)
                .HasConstraintName("FK_ProcessWorkFlowMaster_JobCategory");

            entity.HasOne(d => d.Job).WithMany(p => p.ProcessWorkFlowMasters)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcessWorkFlowMaster_JobOrder");

            entity.HasOne(d => d.Scope).WithMany(p => p.ProcessWorkFlowMasters)
                .HasForeignKey(d => d.ScopeId)
                .HasConstraintName("FK_ProcessWorkFlowMaster_Scope");

            entity.HasOne(d => d.Transaction).WithMany(p => p.ProcessWorkFlowMasters)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcessWorkFlowMaster_TransactionType");
        });

        modelBuilder.Entity<ProcessWorkFlowStatus>(entity =>
        {
            entity.ToTable("ProcessWorkFlowStatus");
        });

        modelBuilder.Entity<ProcessWorkFlowTran>(entity =>
        {
            entity.ToTable("ProcessWorkFlowTran");

            entity.HasIndex(e => e.ProcessId, "IX_PWT_ProcessId");

            entity.HasIndex(e => e.Wfmid, "IX_PWT_WFMId").IsDescending();

            entity.HasIndex(e => e.DepartmentId, "IX_PWT_deptid");

            entity.HasIndex(e => new { e.Id, e.PreviousProcessId, e.EstimatedTime, e.CreatedUtc, e.TimeStamp, e.EmployeeName, e.AllocatedEstimatedTime, e.ArtistName, e.ErrorStatus, e.Jid }, "IX_PWTtable");

            entity.HasIndex(e => e.EmployeeCount, "IX_empcount");

            entity.HasIndex(e => e.IsActive, "IX_isactive");

            entity.HasIndex(e => e.StatusId, "IX_statusid");

            entity.Property(e => e.ArtistName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ErrorStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FileUploadPath).HasMaxLength(200);
            entity.Property(e => e.Jid).HasColumnName("JId");
            entity.Property(e => e.Remarks).IsUnicode(false);
            entity.Property(e => e.TimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.Wfmid).HasColumnName("WFMId");
            entity.Property(e => e.WorkflowTypeDescription)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProcessWorkFlowTranCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_ProcessWorkFlowTran_Employee");

            entity.HasOne(d => d.JidNavigation).WithMany(p => p.ProcessWorkFlowTrans)
                .HasForeignKey(d => d.Jid)
                .HasConstraintName("FK__ProcessWork__JId__62E4AA3C");

            entity.HasOne(d => d.JobStatus).WithMany(p => p.ProcessWorkFlowTrans)
                .HasForeignKey(d => d.JobStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcessWorkFlowTran_JobStatus");

            entity.HasOne(d => d.PreviousProcess).WithMany(p => p.ProcessWorkFlowTranPreviousProcesses)
                .HasForeignKey(d => d.PreviousProcessId)
                .HasConstraintName("FK_ProcessWorkFlowTran_Process1");

            entity.HasOne(d => d.Process).WithMany(p => p.ProcessWorkFlowTranProcesses)
                .HasForeignKey(d => d.ProcessId)
                .HasConstraintName("FK_ProcessWorkFlowTran_Process");

            entity.HasOne(d => d.Status).WithMany(p => p.ProcessWorkFlowTrans)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcessWorkFlowTran_Status");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ProcessWorkFlowTranUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_ProcessWorkFlowTran_Employee1");

            entity.HasOne(d => d.Wfm).WithMany(p => p.ProcessWorkFlowTrans)
                .HasForeignKey(d => d.Wfmid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcessWorkFlowTran_ProcessWorkFlowMaster");
        });

        modelBuilder.Entity<Proficiency>(entity =>
        {
            entity.ToTable("Proficiency");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<Receivable>(entity =>
        {
            entity.ToTable("Receivable");

            entity.Property(e => e.CollectionAmount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.CollectionDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DestinationBank)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExchangeRate).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.ReferenceDate).HasColumnType("datetime");
            entity.Property(e => e.ReferenceNo)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.VoucherNo)
                .HasMaxLength(35)
                .IsUnicode(false);

            entity.HasOne(d => d.Customer).WithMany(p => p.Receivables)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Receivable_Customer");

            entity.HasOne(d => d.RemovalReason).WithMany(p => p.Receivables)
                .HasForeignKey(d => d.RemovalReasonId)
                .HasConstraintName("FK_Receivable_RemovalReasons");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Receivables)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Receivable_TransactionType");
        });

        modelBuilder.Entity<ReceivableAdjustment>(entity =>
        {
            entity.ToTable("ReceivableAdjustment");

            entity.Property(e => e.AdjustmentAmount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.AvailableAdvance).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Crdr)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("CRDR");
            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.ReferenceNo)
                .HasMaxLength(35)
                .IsUnicode(false);

            entity.HasOne(d => d.Invoice).WithMany(p => p.ReceivableAdjustments)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK_ReceivableAdjustment_InvoiceMaster");

            entity.HasOne(d => d.Receivable).WithMany(p => p.ReceivableAdjustments)
                .HasForeignKey(d => d.ReceivableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReceivableAdjustment_Receivable");
        });

        modelBuilder.Entity<ReceivableExt>(entity =>
        {
            entity.ToTable("ReceivableExt");

            entity.Property(e => e.Amount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.BankName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ReceiptMode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ReceivableId).HasColumnName("ReceivableID");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.TransactionNo)
                .HasMaxLength(35)
                .IsUnicode(false);

            entity.HasOne(d => d.Receivable).WithMany(p => p.ReceivableExts)
                .HasForeignKey(d => d.ReceivableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReceivableExt_Receivable");
        });

        modelBuilder.Entity<RemovalReason>(entity =>
        {
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<ResignReason>(entity =>
        {
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<Scope>(entity =>
        {
            entity.ToTable("Scope");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Department).WithMany(p => p.Scopes)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Scope_Department");
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.ToTable("Shift");

            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FromTime).HasMaxLength(50);
            entity.Property(e => e.ShiftType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ToTime).HasMaxLength(50);
        });

        modelBuilder.Entity<ShiftVsEmployee>(entity =>
        {
            entity.ToTable("ShiftVsEmployee");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");
            entity.Property(e => e.EffectiveTo).HasColumnType("datetime");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Employee).WithMany(p => p.ShiftVsEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShiftVsEmployee_employee");

            entity.HasOne(d => d.Shift).WithMany(p => p.ShiftVsEmployees)
                .HasForeignKey(d => d.ShiftId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shift_Employee");
        });

        modelBuilder.Entity<Skillset>(entity =>
        {
            entity.ToTable("Skillset");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<StaffingPrice>(entity =>
        {
            entity.ToTable("StaffingPrice");

            entity.Property(e => e.BillingFromDate).HasColumnType("datetime");
            entity.Property(e => e.BillingToDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.WefromDate)
                .HasColumnType("datetime")
                .HasColumnName("WEFromDate");
            entity.Property(e => e.WetoDate)
                .HasColumnType("datetime")
                .HasColumnName("WEToDate");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.ToTable("State");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.StateName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StateShortName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK__State__CountryId__2DD1C37F");
        });

        modelBuilder.Entity<Statee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__State__3214EC073BF16EF4");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.StateName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StateShortName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Country).WithMany(p => p.Statees)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK__State__CountryId__60083D91");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("Status");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.StatusCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Status_Employee");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.StatusUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_Status_Employee1");
        });

        modelBuilder.Entity<StatusMaster>(entity =>
        {
            entity.ToTable("StatusMaster");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description).HasMaxLength(150);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<TblBankDetail>(entity =>
        {
            entity.ToTable("tbl_BankDetails");

            entity.Property(e => e.AccountName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.BankName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Branch)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ClosingBalance).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.ClosingDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Ifsccode)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("IFSCCode");
            entity.Property(e => e.OpeningBalance).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblItasset>(entity =>
        {
            entity.ToTable("tbl_ITAsset");

            entity.Property(e => e.BayNumber).IsUnicode(false);
            entity.Property(e => e.Brand).IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Division).IsUnicode(false);
            entity.Property(e => e.Graphics).IsUnicode(false);
            entity.Property(e => e.Hdd).IsUnicode(false);
            entity.Property(e => e.InvoiceDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceNumber).IsUnicode(false);
            entity.Property(e => e.IpAddress).IsUnicode(false);
            entity.Property(e => e.Keyboard).IsUnicode(false);
            entity.Property(e => e.KeyboardSerialNumber).IsUnicode(false);
            entity.Property(e => e.Location).IsUnicode(false);
            entity.Property(e => e.MacAddress).IsUnicode(false);
            entity.Property(e => e.Model).IsUnicode(false);
            entity.Property(e => e.Monitor).IsUnicode(false);
            entity.Property(e => e.MonitorSerialNumber).IsUnicode(false);
            entity.Property(e => e.Mouse).IsUnicode(false);
            entity.Property(e => e.MouseSerialNumber).IsUnicode(false);
            entity.Property(e => e.Os).IsUnicode(false);
            entity.Property(e => e.PcName).IsUnicode(false);
            entity.Property(e => e.Processor).IsUnicode(false);
            entity.Property(e => e.Ram).IsUnicode(false);
            entity.Property(e => e.Roll).IsUnicode(false);
            entity.Property(e => e.ServerType).IsUnicode(false);
            entity.Property(e => e.TagNumber).IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.WarantyDetails).IsUnicode(false);
            entity.Property(e => e.WorkingStatus).IsUnicode(false);
        });

        modelBuilder.Entity<TblItassetHardware>(entity =>
        {
            entity.ToTable("tbl_ITAssetHardware");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblItassetSoftware>(entity =>
        {
            entity.ToTable("tbl_ITAssetSoftware");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblItassetSoftwareCompliance>(entity =>
        {
            entity.ToTable("tbl_ITAssetSoftwareCompliance");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ItassetId).HasColumnName("ITAssetId");
            entity.Property(e => e.SoftwareStatus)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblItcategory>(entity =>
        {
            entity.ToTable("tbl_ITCategory");

            entity.Property(e => e.CategoryId).HasColumnName("Category_Id");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblItcategoryDetail>(entity =>
        {
            entity.ToTable("tbl_ITCategoryDetail");

            entity.Property(e => e.CategoryId).HasColumnName("Category_Id");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblProductionPlan>(entity =>
        {
            entity.ToTable("tbl_ProductionPlans");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.ProductionType)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblVendorDetail>(entity =>
        {
            entity.ToTable("tbl_VendorDetails");

            entity.Property(e => e.AmountPaid).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceValue).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.PendingAmount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.VendorName)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Temp2>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("temp2");

            entity.Property(e => e.Fifth).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Firstq).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Fourth).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Second).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Third).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<TimeZone>(entity =>
        {
            entity.ToTable("TimeZone");

            entity.Property(e => e.Description)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.Istdiff)
                .HasMaxLength(6)
                .HasColumnName("ISTDiff");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TimezoneDiff).HasMaxLength(6);
        });

        modelBuilder.Entity<TrainingFeedbackForm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Training__3214EC0725B0C75E");

            entity.Property(e => e.ApprovedUTC)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ApprovedUTC");
            entity.Property(e => e.AtsComments)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AttendanceComments)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AttitudeComments)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CommunicationSkillsComments)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUTC)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.CreativityComments)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.JobKnowledgeComments)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OverallComments)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProductivityComments)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PunctualityComments)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RejectedUTC)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("RejectedUTC");
            entity.Property(e => e.TechnicalSkillsComments)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TrainingEndDate).HasColumnType("date");
            entity.Property(e => e.TrainingStartDate).HasColumnType("date");
            entity.Property(e => e.UpdatedUTC)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.WorkQualityComments)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Employee).WithMany(p => p.TrainingFeedbackForms)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeDetails_Employee");
        });

        modelBuilder.Entity<TransactionType>(entity =>
        {
            entity.ToTable("TransactionType");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<TrayDetailMovement>(entity =>
        {
            entity.ToTable("TrayDetailMovement");

            entity.Property(e => e.BpallocatedBy)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("BPAllocatedBy");
            entity.Property(e => e.BpallocatedDate)
                .HasColumnType("datetime")
                .HasColumnName("BPAllocatedDate");
            entity.Property(e => e.BpallocatedTo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("BPAllocatedTo");
            entity.Property(e => e.BpallocatedToEmpId).HasColumnName("BPAllocatedToEmpId");
            entity.Property(e => e.BpcompletedDateTime)
                .HasColumnType("datetime")
                .HasColumnName("BPCompletedDateTime");
            entity.Property(e => e.BpstartDate)
                .HasColumnType("datetime")
                .HasColumnName("BPStartDate");
            entity.Property(e => e.BpworkedTime).HasColumnName("BPWorkedTime");
            entity.Property(e => e.CcintakenBy)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("CCIntakenBy");
            entity.Property(e => e.CcintakenDate)
                .HasColumnType("datetime")
                .HasColumnName("CCIntakenDate");
            entity.Property(e => e.Classification)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ClientShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.CurrentStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CurrentTray)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Jid).HasColumnName("JId");
            entity.Property(e => e.JobDate).HasColumnType("datetime");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatus)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ProductionAllocatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProductionAllocatedDate).HasColumnType("datetime");
            entity.Property(e => e.ProductionAllocatedTo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProductionCompletedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ProductionStartDate).HasColumnType("datetime");
            entity.Property(e => e.ProofReadingAllocatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProofReadingAllocatedDate).HasColumnType("datetime");
            entity.Property(e => e.ProofReadingAllocatedTo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProofReadingCompletedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ProofReadingStartDate).HasColumnType("datetime");
            entity.Property(e => e.QualityAllocatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.QualityAllocatedDate).HasColumnType("datetime");
            entity.Property(e => e.QualityAllocatedTo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.QualityCompletedDateTime).HasColumnType("datetime");
            entity.Property(e => e.QualityStartDate).HasColumnType("datetime");
            entity.Property(e => e.QueryRaisedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.QueryRaisedDate).HasColumnType("datetime");
            entity.Property(e => e.QueryResponseBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.QueryResponseDate).HasColumnType("datetime");
            entity.Property(e => e.Scope)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SoallocatedBy)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SOAllocatedBy");
            entity.Property(e => e.SoallocatedDate)
                .HasColumnType("datetime")
                .HasColumnName("SOAllocatedDate");
            entity.Property(e => e.SoallocatedTo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SOAllocatedTo");
            entity.Property(e => e.SoallocatedToEmpId).HasColumnName("SOAllocatedToEmpId");
            entity.Property(e => e.SocompletedDateTime)
                .HasColumnType("datetime")
                .HasColumnName("SOCompletedDateTime");
            entity.Property(e => e.SostartDate)
                .HasColumnType("datetime")
                .HasColumnName("SOStartDate");
            entity.Property(e => e.SoworkedTime).HasColumnName("SOWorkedTime");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.UploadedBy)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.UploadedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UploadedUTC");
        });

        modelBuilder.Entity<TrayDetailMovementTran>(entity =>
        {
            entity.ToTable("TrayDetailMovementTran");

            entity.Property(e => e.AllocatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AllocatedDate).HasColumnType("datetime");
            entity.Property(e => e.AllocatedTo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CcintakenBy)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("CCIntakenBy");
            entity.Property(e => e.CcintakenDate)
                .HasColumnType("datetime")
                .HasColumnName("CCIntakenDate");
            entity.Property(e => e.Classification)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ClientShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CompletedDateTime).HasColumnType("datetime");
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.CurrentStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CurrentTray)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Jid).HasColumnName("JId");
            entity.Property(e => e.JobDate).HasColumnType("datetime");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatus)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Process)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.QueryRaisedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.QueryRaisedDate).HasColumnType("datetime");
            entity.Property(e => e.QueryResponseBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.QueryResponseDate).HasColumnType("datetime");
            entity.Property(e => e.Scope)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Tdmid).HasColumnName("TDMId");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.UploadedBy)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.UploadedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UploadedUTC");
            entity.Property(e => e.Wfmid).HasColumnName("WFMId");

            entity.HasOne(d => d.ScopeNavigation).WithMany(p => p.TrayDetailMovementTrans)
                .HasForeignKey(d => d.ScopeId)
                .HasConstraintName("FK__TrayDetai__Scope__3429BB53");

            entity.HasOne(d => d.Tdm).WithMany(p => p.TrayDetailMovementTrans)
                .HasForeignKey(d => d.Tdmid)
                .HasConstraintName("FK_TrayDetailMovementTran_TrayDetailMovement");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Domain)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Roles)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Customer).WithMany(p => p.Users)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_User_Customer");

            entity.HasOne(d => d.Employee).WithMany(p => p.Users)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_User_Employee");
        });

        modelBuilder.Entity<ViewAssignedJob>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_AssignedJobs");

            entity.Property(e => e.CategoryDesc)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.JobDate).HasColumnType("datetime");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewAutoGetGenerateforInvoice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_AutoGetGenerateforInvoice");

            entity.Property(e => e.BillingCycleType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateofUpload)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EstdateofUpload)
                .HasColumnType("datetime")
                .HasColumnName("ESTDateofUpload");
            entity.Property(e => e.EstfileReceivedDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTFileReceivedDate");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.Jid).HasColumnName("JId");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SpecialPrice).HasColumnType("numeric(10, 4)");
        });

        modelBuilder.Entity<ViewBusinessDay>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_BusinessDays");

            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NumberOfDays).HasColumnName("Number_Of_Days");
            entity.Property(e => e.ReceivedDate)
                .HasColumnType("datetime")
                .HasColumnName("Received_date");
            entity.Property(e => e.ShortName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WithHolidays).HasColumnName("With_holidays");
        });

        modelBuilder.Entity<ViewCancelledJob>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_CancelledJobs");

            entity.Property(e => e.ActionDoneBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ActionInitiatedBy)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.ActionInitiatedTray)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.ActionStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ActionTakenDate).HasColumnType("datetime");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.Jid).HasColumnName("JId");
            entity.Property(e => e.JobDate).HasColumnType("datetime");
            entity.Property(e => e.JobScopeStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Remarks).IsUnicode(false);
            entity.Property(e => e.Scope)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.WorkedTimeQc).HasColumnName("WorkedTimeQC");
        });

        modelBuilder.Entity<ViewCollectionsReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_CollectionsReport");

            entity.Property(e => e.AdjustmentAmount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.CollectionAmount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.CollectionDate).HasColumnType("datetime");
            entity.Property(e => e.Est)
                .HasColumnType("datetime")
                .HasColumnName("EST");
            entity.Property(e => e.InvoiceDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewConvertedClientOrder>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_ConvertedClientOrder");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileUploadPath).HasMaxLength(200);
            entity.Property(e => e.Instruction).IsUnicode(false);
            entity.Property(e => e.JobReferenceId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ParentJobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Podate)
                .HasColumnType("datetime")
                .HasColumnName("PODate");
            entity.Property(e => e.Pono)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PONo");
            entity.Property(e => e.ReceiptMode)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.SalesPersonName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<ViewCurrentTrayFileStatusReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_CurrentTrayFileStatusReport");

            entity.Property(e => e.AllocatedBy)
                .HasMaxLength(121)
                .IsUnicode(false);
            entity.Property(e => e.AllocatedTo)
                .HasMaxLength(121)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DateofUpload).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Est)
                .HasColumnType("datetime")
                .HasColumnName("EST");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<ViewCustomerAddresswithLocation>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_CustomerAddresswithLocation");

            entity.Property(e => e.Address1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address3)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BlacklistedReasons)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Classification)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Cstno)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("CSTNo");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Phone1)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Phone2)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Tinno)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("TINNo");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.WebAddress)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewCustomerDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_CustomerDetails");

            entity.Property(e => e.Address1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address3)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BlacklistedReasons)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Classification)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Cstno)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("CSTNo");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Phone1)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.Phone2)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Tinno)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("TINNo");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.WebAddress)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewCustomerNorm>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_CustomerNorms");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.CustomerShortName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Departmentname)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DivisionName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Jobstatus)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Processname)
                .IsUnicode(false)
                .HasColumnName("processname");
            entity.Property(e => e.ScopeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<ViewCustomerStatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_CustomerStatus");

            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DepartmentDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LiveDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ScopeDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ViewCustomerVsEmployee>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_CustomerVsEmployee");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.CustomerClassification)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeNameCode)
                .HasMaxLength(123)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<ViewCustomerVsProcess>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_CustomerVsProcess");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.CurrentProcess).IsUnicode(false);
            entity.Property(e => e.CustomJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NextProcess).IsUnicode(false);
            entity.Property(e => e.Scope)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.StatusDescription)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<ViewDailyOperationsReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_DailyOperationsReport");

            entity.Property(e => e.Ccintake)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CCIntake");
            entity.Property(e => e.Ccupload)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CCUpload");
            entity.Property(e => e.Client)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileInwardType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("date");
            entity.Property(e => e.FileStatus)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.FinalStatus)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.JobDate).HasColumnType("date");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PrerrorStatus)
                .IsUnicode(false)
                .HasColumnName("PRErrorStatus");
            entity.Property(e => e.ProofAccepted)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProofReader)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PrworkedTime).HasColumnName("PRWorkedTime");
            entity.Property(e => e.Qc)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("QC");
            entity.Property(e => e.QcerrorStatus)
                .IsUnicode(false)
                .HasColumnName("QCErrorStatus");
            entity.Property(e => e.QcworkedTime).HasColumnName("QCWorkedTime");
            entity.Property(e => e.Wfmid).HasColumnName("WFMId");
        });

        modelBuilder.Entity<ViewDailyProductionReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_DailyProductionReport");

            entity.Property(e => e.AcceptedProof)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ArtistName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CcteamIntake)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CCTeamIntake");
            entity.Property(e => e.CcteamUpload)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CCTeamUpload");
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateofUpload).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileInwardType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.FinalStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.JobClosedUtc)
                .HasColumnType("datetime")
                .HasColumnName("JobClosedUTC");
            entity.Property(e => e.JobDate).HasColumnType("datetime");
            entity.Property(e => e.JobDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PrerrorStatus)
                .IsUnicode(false)
                .HasColumnName("PRErrorStatus");
            entity.Property(e => e.ProofAccepted)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProofReaderName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Prtime).HasColumnName("PRTime");
            entity.Property(e => e.QcerrorStatus)
                .IsUnicode(false)
                .HasColumnName("QCErrorStatus");
            entity.Property(e => e.Qcname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("QCName");
            entity.Property(e => e.Qctime).HasColumnName("QCTime");
            entity.Property(e => e.SalesPersonName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ScopeDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewDailyReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_DailyReport");

            entity.Property(e => e.CcintakeBy)
                .HasMaxLength(121)
                .IsUnicode(false)
                .HasColumnName("CCIntakeBy");
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DateofUpload).HasColumnType("datetime");
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Est)
                .HasColumnType("datetime")
                .HasColumnName("EST");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ScopeDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UploadedBy)
                .HasMaxLength(121)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewDashBoardStatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_DashBoardStatus");

            entity.Property(e => e.ClientName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("clientName");
            entity.Property(e => e.CustomerClassificationType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DateofUpload).HasColumnType("datetime");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Est)
                .HasColumnType("datetime")
                .HasColumnName("EST");
            entity.Property(e => e.EstjobDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTJobDate");
            entity.Property(e => e.EstqueryDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTQueryDate");
            entity.Property(e => e.EstqueryJobDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTQueryJobDate");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.UpdateJobDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<ViewDashboardReportFsr>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_DashboardReportFSR");

            entity.Property(e => e.ClassificationName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DateofUpload).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Est)
                .HasColumnType("datetime")
                .HasColumnName("EST");
            entity.Property(e => e.EstjobDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTJobDate");
            entity.Property(e => e.EstqueryDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTQueryDate");
            entity.Property(e => e.EstqueryJobDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTQueryJobDate");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.JobDate).HasColumnType("datetime");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<ViewDepartmentBasedCustomer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_DepartmentBasedCustomers");

            entity.Property(e => e.BlacklistedReasons)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewDivisionDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_DivisionDetails");

            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewEmployeeDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_EmployeeDetails");

            entity.Property(e => e.BloodGroup)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.DateOfJoining).HasColumnType("datetime");
            entity.Property(e => e.DateOfResignation).HasColumnType("datetime");
            entity.Property(e => e.DepartmentDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DesigId).HasColumnName("DesigID");
            entity.Property(e => e.DesignationDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContactName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContactNo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.LeaderName1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LeaderName2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ManagerName1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ManagerName2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ProfId).HasColumnName("ProfID");
            entity.Property(e => e.ProfiencyDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewEmployeeDetailsWithAdd>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_EmployeeDetailsWithADD");

            entity.Property(e => e.Address1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address11)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address22)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address3)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address33)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AddressType)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.BloodGroup)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.DateOfJoining).HasColumnType("datetime");
            entity.Property(e => e.DateOfResignation).HasColumnType("datetime");
            entity.Property(e => e.DepartmentDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DesigId).HasColumnName("DesigID");
            entity.Property(e => e.DesignationDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContactName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContactNo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IsInternetConnection)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsSystem)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LeaderName2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ManagerName1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.NetWorkType).HasMaxLength(100);
            entity.Property(e => e.PersonalEmail).HasMaxLength(50);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ProfId).HasColumnName("ProfID");
            entity.Property(e => e.ProfiencyDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ServiceProvider).HasMaxLength(100);
            entity.Property(e => e.SystemConfig).HasMaxLength(100);
        });

        modelBuilder.Entity<ViewEmployeeDetailsWithAddress>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_EmployeeDetailsWithAddress");

            entity.Property(e => e.Address1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address11)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address22)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address3)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address33)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AddressType)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.BloodGroup)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.DateOfJoining).HasColumnType("datetime");
            entity.Property(e => e.DateOfResignation).HasColumnType("datetime");
            entity.Property(e => e.DepartmentDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DesigId).HasColumnName("DesigID");
            entity.Property(e => e.DesignationDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContactName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContactNo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.LeaderName1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LeaderName2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ManagerName1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ManagerName2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ProfId).HasColumnName("ProfID");
            entity.Property(e => e.ProfiencyDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewEmployeeDetailsfromEmp>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_EmployeeDetailsfromEMP");

            entity.Property(e => e.BloodGroup)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.DateOfJoining).HasColumnType("datetime");
            entity.Property(e => e.DateOfResignation).HasColumnType("datetime");
            entity.Property(e => e.DepartmentDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DesigId).HasColumnName("DesigID");
            entity.Property(e => e.DesignationDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");
            entity.Property(e => e.EffectiveTo).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContactName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContactNo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FromTime).HasMaxLength(50);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.LeaderName1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LeaderName2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ManagerName1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ManagerName2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ProfId).HasColumnName("ProfID");
            entity.Property(e => e.ProfiencyDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShiftType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ToTime).HasMaxLength(50);
        });

        modelBuilder.Entity<ViewEmployeeDetailswithShift>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_EmployeeDetailswithShift");

            entity.Property(e => e.DepartmentDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DesignationDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");
            entity.Property(e => e.EffectiveTo).HasColumnType("datetime");
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FromTime).HasMaxLength(50);
            entity.Property(e => e.LeaderName1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LeaderName2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ManagerName1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ManagerName2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProfiencyDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShiftType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ToTime).HasMaxLength(50);
        });

        modelBuilder.Entity<ViewEmployeeWiseReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_EmployeeWiseReport");

            entity.Property(e => e.Est)
                .HasColumnType("datetime")
                .HasColumnName("EST");
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewEmployeeWithShift>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_EmployeeWithShift");

            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeDesc)
                .HasMaxLength(121)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FromTime).HasMaxLength(50);
            entity.Property(e => e.ShiftName).HasMaxLength(163);
            entity.Property(e => e.ShiftType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ToTime).HasMaxLength(50);
        });

        modelBuilder.Entity<ViewFileInwardExcel>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_FileInwardExcel");

            entity.Property(e => e.ClientJobId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ClientStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateofReceived).HasColumnType("datetime");
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Division)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FileInwardMode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Ground)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SalesPersonName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.SchoolName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StatusDesc)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.Style)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TeamCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Temp)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.UploadedBy).HasMaxLength(50);
        });

        modelBuilder.Entity<ViewFileStatusReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_FileStatusReport");

            entity.Property(e => e.ClassificationName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DateofUpload).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Est)
                .HasColumnType("datetime")
                .HasColumnName("EST");
            entity.Property(e => e.EstjobDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTJobDate");
            entity.Property(e => e.EstqueryDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTQueryDate");
            entity.Property(e => e.EstqueryJobDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTQueryJobDate");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.JobDate).HasColumnType("datetime");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<ViewFileStatusReportRef>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_FileStatusReportRef");

            entity.Property(e => e.ClassificationName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DateofUpload).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Est)
                .HasColumnType("datetime")
                .HasColumnName("EST");
            entity.Property(e => e.EstjobDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTJobDate");
            entity.Property(e => e.EstqueryDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTQueryDate");
            entity.Property(e => e.EstqueryJobDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTQueryJobDate");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.JobDate).HasColumnType("datetime");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<ViewFileStatusReportRefXerox>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_FileStatusReportRefXerox");

            entity.Property(e => e.ClassificationName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ClientName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DateofUpload).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Est)
                .HasColumnType("datetime")
                .HasColumnName("EST");
            entity.Property(e => e.EstcreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("ESTCreatedUTC");
            entity.Property(e => e.EstjobDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTJobDate");
            entity.Property(e => e.EstqueryDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTQueryDate");
            entity.Property(e => e.EstqueryJobDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTQueryJobDate");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.JobDate).HasColumnType("datetime");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<ViewGeneratedInvoice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_GeneratedInvoice");

            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Discount).HasColumnType("numeric(8, 2)");
            entity.Property(e => e.EstdateofUpload)
                .HasColumnType("datetime")
                .HasColumnName("ESTDateofUpload");
            entity.Property(e => e.EstfileReceivedDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTFileReceivedDate");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceValue).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Jid).HasColumnName("JId");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ProductValue).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.Qty).HasColumnType("numeric(8, 2)");
            entity.Property(e => e.Rate).HasColumnType("numeric(10, 4)");
            entity.Property(e => e.ScopeDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.SpecialPrice).HasColumnType("numeric(10, 4)");
        });

        modelBuilder.Entity<ViewGetBirthandJoining>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_GetBirthandJoining");

            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.DateOfJoining).HasColumnType("datetime");
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Designation)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Dobd)
                .HasMaxLength(4000)
                .HasColumnName("DOBD");
            entity.Property(e => e.Dobdate)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("DOBDate");
            entity.Property(e => e.Dobm)
                .HasMaxLength(4000)
                .HasColumnName("DOBM");
            entity.Property(e => e.Dobmd)
                .HasMaxLength(4000)
                .HasColumnName("DOBMD");
            entity.Property(e => e.Dojd)
                .HasMaxLength(4000)
                .HasColumnName("DOJD");
            entity.Property(e => e.Dojdate)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("DOJDate");
            entity.Property(e => e.Dojm)
                .HasMaxLength(4000)
                .HasColumnName("DOJM");
            entity.Property(e => e.Dojmd)
                .HasMaxLength(4000)
                .HasColumnName("DOJMD");
            entity.Property(e => e.Dojy)
                .HasMaxLength(4000)
                .HasColumnName("DOJY");
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewGetCancelledInvoiceList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_GetCancelledInvoiceList");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Discount).HasColumnType("numeric(8, 2)");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceValue).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.OriginalNo)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.ProductValue).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.ReceivableValue).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.RoundOff).HasColumnType("numeric(8, 4)");
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TotalInvoiceValue).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Waiver).HasColumnType("numeric(18, 4)");
        });

        modelBuilder.Entity<ViewGetCheckWaiverJob>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_GetCheckWaiverJobs");

            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Imid).HasColumnName("IMId");
            entity.Property(e => e.Jid).HasColumnName("JId");
            entity.Property(e => e.Qty).HasColumnType("numeric(8, 2)");
            entity.Property(e => e.Rate).HasColumnType("numeric(10, 4)");
            entity.Property(e => e.ScopeGroupDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Value).HasColumnType("numeric(14, 4)");
        });

        modelBuilder.Entity<ViewGetConfirmedGeneratedInvoice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_GetConfirmedGeneratedInvoice");

            entity.Property(e => e.ArtDiscountAmount).HasColumnType("numeric(14, 2)");
            entity.Property(e => e.ArtInvoiceAmount).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.ArtWaiver).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.CurrencyMode).HasMaxLength(10);
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DigiDiscountAmount).HasColumnType("numeric(14, 2)");
            entity.Property(e => e.DigiInvoiceAmount).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.DigiWaiver).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.Discount).HasColumnType("numeric(8, 2)");
            entity.Property(e => e.Est)
                .HasColumnType("datetime")
                .HasColumnName("EST");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceValue).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PoNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductValue).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.Qty).HasColumnType("numeric(8, 2)");
            entity.Property(e => e.Rate).HasColumnType("numeric(10, 4)");
            entity.Property(e => e.RoundOff)
                .HasColumnType("numeric(8, 4)")
                .HasColumnName("RoundOFF");
            entity.Property(e => e.SalesPersonName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Scope)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalInvoiceValue).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Value).HasColumnType("numeric(14, 4)");
            entity.Property(e => e.Waiver).HasColumnType("numeric(18, 4)");
        });

        modelBuilder.Entity<ViewGetCustomerforInvoiceCancel>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_GetCustomerforInvoiceCancel");

            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewGetCustomerforSelection>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_GetCustomerforSelection");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewGetDropInvoiceCancel>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_GetDropInvoiceCancel");

            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewGetEmployeesForPendingJob>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_GetEmployeesForPendingJobs");

            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Jid).HasColumnName("JId");
            entity.Property(e => e.Remarks).IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Wfmid).HasColumnName("WFMId");
            entity.Property(e => e.Wftid).HasColumnName("WFTId");
        });

        modelBuilder.Entity<ViewGetGenerateforInvoice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_GetGenerateforInvoice");

            entity.Property(e => e.BillingCycleType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateofUpload)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EstdateofUpload)
                .HasColumnType("datetime")
                .HasColumnName("ESTDateofUpload");
            entity.Property(e => e.EstfileReceivedDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTFileReceivedDate");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.Jid).HasColumnName("JId");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.SpecialPrice).HasColumnType("numeric(10, 4)");
        });

        modelBuilder.Entity<ViewGetIntegrationTran>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_GetIntegrationTran");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Imid).HasColumnName("IMId");
            entity.Property(e => e.InvoiceDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.Qty).HasColumnType("numeric(8, 2)");
            entity.Property(e => e.Rate).HasColumnType("numeric(10, 4)");
            entity.Property(e => e.Uom)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("UOM");
            entity.Property(e => e.Value).HasColumnType("numeric(14, 4)");
        });

        modelBuilder.Entity<ViewGetInvoiceMaster>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_GetInvoiceMaster");

            entity.Property(e => e.ArtDiscountAmount).HasColumnType("numeric(14, 2)");
            entity.Property(e => e.ArtInvoiceAmount).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.ArtPayableAmount).HasColumnType("numeric(20, 4)");
            entity.Property(e => e.ArtWaiver).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DigiDiscountAmount).HasColumnType("numeric(14, 2)");
            entity.Property(e => e.DigiInvoiceAmount).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.DigiPayableAmount).HasColumnType("numeric(20, 4)");
            entity.Property(e => e.DigiWaiver).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.Discount).HasColumnType("numeric(8, 2)");
            entity.Property(e => e.EstinvoiceDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTInvoiceDate");
            entity.Property(e => e.InvoiceDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceValue).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.PaymentMode).HasMaxLength(50);
            entity.Property(e => e.PrimaryDateTime).HasColumnType("datetime");
            entity.Property(e => e.ProductValue).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.ReceivableValue).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.ReferenceDate).HasColumnType("datetime");
            entity.Property(e => e.RoundOff).HasColumnType("numeric(8, 4)");
            entity.Property(e => e.SecondaryDateTime).HasColumnType("datetime");
            entity.Property(e => e.ShortName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalInvoiceValue).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.Waiver).HasColumnType("numeric(18, 4)");
        });

        modelBuilder.Entity<ViewGetInvoiceMasterforDayDiff>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_GetInvoiceMasterforDayDiff");

            entity.Property(e => e.ArtInvoiceAmount).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.ArtWaiver).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DigiInvoiceAmount).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.DigiWaiver).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.Discount).HasColumnType("numeric(8, 2)");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.InvoiceDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceValue).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.PrimaryDateTime).HasColumnType("datetime");
            entity.Property(e => e.ProductValue).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.ReceivableValue).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.ReferenceDate).HasColumnType("datetime");
            entity.Property(e => e.ReferenceNo)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.RoundOff).HasColumnType("numeric(8, 4)");
            entity.Property(e => e.SecondaryDateTime).HasColumnType("datetime");
            entity.Property(e => e.TotalInvoiceValue).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.Waiver).HasColumnType("numeric(18, 4)");
        });

        modelBuilder.Entity<ViewGetPricingforInvoice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_GetPricingforInvoice");

            entity.Property(e => e.BillingCycleType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateofUpload)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EstdateofUpload)
                .HasColumnType("datetime")
                .HasColumnName("ESTDateofUpload");
            entity.Property(e => e.EstfileReceivedDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTFileReceivedDate");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.Jid).HasColumnName("JId");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.SpecialPrice).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Wfmid).HasColumnName("WFMId");
        });

        modelBuilder.Entity<ViewGetTranMergeInvoiceCancel>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_GetTranMergeInvoiceCancel");

            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Imid).HasColumnName("IMId");
            entity.Property(e => e.InvoiceDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.PricingType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Qty).HasColumnType("numeric(8, 2)");
            entity.Property(e => e.Rate).HasColumnType("numeric(10, 4)");
            entity.Property(e => e.Scope)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ScopeGroupDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Value).HasColumnType("numeric(14, 4)");
        });

        modelBuilder.Entity<ViewGetTransactionTypeCancel>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_GetTransactionTypeCancel");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<ViewGetTransactionTypeInvoice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_GetTransactionTypeInvoice");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<ViewGetUser>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_GetUser");

            entity.Property(e => e.Domain)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Roles)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UserType)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.UserTypeDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewGetWorkflowListforLogin>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_GetWorkflowListforLogin");

            entity.Property(e => e.AllocattedDate).HasColumnType("datetime");
            entity.Property(e => e.AssignedFrom)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BalanceTime).HasColumnName("Balance_time");
            entity.Property(e => e.Color)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CustomerClassification)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DateofDelivery).HasColumnType("datetime");
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EstfileReceivedDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTFileReceivedDate");
            entity.Property(e => e.EstjobDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTJobDate");
            entity.Property(e => e.FileInwardType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.Instruction).IsUnicode(false);
            entity.Property(e => e.Jid).HasColumnName("JId");
            entity.Property(e => e.JobCategoryDesc)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.JobDate).HasColumnType("datetime");
            entity.Property(e => e.JobDateEst)
                .HasColumnType("datetime")
                .HasColumnName("JobDateEST");
            entity.Property(e => e.JobDateQueryDate)
                .HasColumnType("datetime")
                .HasColumnName("JobDate_QueryDate");
            entity.Property(e => e.JobDescription).IsUnicode(false);
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JofileUploadPath)
                .HasMaxLength(200)
                .HasColumnName("JOFileUploadPath");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ParentJobFilePath)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.ParentJobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.PoNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProcessName).IsUnicode(false);
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.QueryJobDate).HasColumnType("datetime");
            entity.Property(e => e.ReferenceJobFilePath)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.ReferenceJobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.SalesPersonName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ScopeDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TranFileUploadPath).HasMaxLength(200);
            entity.Property(e => e.Wfmid).HasColumnName("WFMId");
            entity.Property(e => e.Wftid).HasColumnName("WFTId");
            entity.Property(e => e.WorkStatus)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewInvoiceCompletedList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_InvoiceCompletedList");

            entity.Property(e => e.Customer)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DateofUpload).HasColumnType("datetime");
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Employee)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.Jid).HasColumnName("JId");
            entity.Property(e => e.JobDate).HasColumnType("datetime");
            entity.Property(e => e.JobDescription).IsUnicode(false);
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatus)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Scope)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SpecialPrice).HasColumnType("numeric(10, 4)");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<ViewJobAllocation>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_JobAllocation");

            entity.Property(e => e.ArtistName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BalanceTime).HasColumnName("Balance_time");
            entity.Property(e => e.CategoryDesc)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CustomerClassification)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DateofDelivery).HasColumnType("datetime");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EstfileReceivedDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTFileReceivedDate");
            entity.Property(e => e.EstjobDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTJobDate");
            entity.Property(e => e.FileInwardType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Jid).HasColumnName("JId");
            entity.Property(e => e.JobDate).HasColumnType("datetime");
            entity.Property(e => e.JobDateEst)
                .HasColumnType("datetime")
                .HasColumnName("JobDateEST");
            entity.Property(e => e.JobDateQueryDate)
                .HasColumnType("datetime")
                .HasColumnName("JobDate_QueryDate");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.QueryJobDate).HasColumnType("datetime");
            entity.Property(e => e.ScopeDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.WorkStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewJobAllocationandJobOrder>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_JobAllocationandJobOrder");

            entity.Property(e => e.AllocattedDate).HasColumnType("datetime");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Process).IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Wfmid).HasColumnName("WFMId");
            entity.Property(e => e.Wftid).HasColumnName("WFTId");
        });

        modelBuilder.Entity<ViewJobDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_JobDetails");

            entity.Property(e => e.BalanceTime).HasColumnName("Balance_time");
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateofUpload).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EstfileReceivedDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTFileReceivedDate");
            entity.Property(e => e.EstjobDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTJobDate");
            entity.Property(e => e.FileInwardMode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileInwardType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.Jid).HasColumnName("JId");
            entity.Property(e => e.JobClosedUtc)
                .HasColumnType("datetime")
                .HasColumnName("JobClosedUTC");
            entity.Property(e => e.JobDate).HasColumnType("datetime");
            entity.Property(e => e.JobDateEst)
                .HasColumnType("datetime")
                .HasColumnName("JobDateEST");
            entity.Property(e => e.JobDateQueryDate)
                .HasColumnType("datetime")
                .HasColumnName("JobDate_QueryDate");
            entity.Property(e => e.JobDescription).IsUnicode(false);
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SalesPersonName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Username)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewJobForcompletedJob>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_JobForcompletedJobs");

            entity.Property(e => e.BalanceTime).HasColumnName("Balance_time");
            entity.Property(e => e.CreditLimit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CustomerClassification)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CustomerType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DateofUpload).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EstfileReceivedDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTFileReceivedDate");
            entity.Property(e => e.EstjobDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTJobDate");
            entity.Property(e => e.FileInwardMode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileInwardType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.Jid).HasColumnName("JId");
            entity.Property(e => e.JobClosedUtc)
                .HasColumnType("datetime")
                .HasColumnName("JobClosedUTC");
            entity.Property(e => e.JobDate).HasColumnType("datetime");
            entity.Property(e => e.JobDateEst)
                .HasColumnType("datetime")
                .HasColumnName("JobDateEST");
            entity.Property(e => e.JobDateQueryDate)
                .HasColumnType("datetime")
                .HasColumnName("JobDate_QueryDate");
            entity.Property(e => e.JobDescription).IsUnicode(false);
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SalesEmpName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SalesPersonName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Scopetype)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("scopetype");
            entity.Property(e => e.ShortName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Username)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewJobMovement>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_JobMovement");

            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DateofUpload).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.JobDate).HasColumnType("datetime");
            entity.Property(e => e.JobDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProcessName).IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewJobOrder>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_JobOrder");

            entity.Property(e => e.ApparelLogoLocation)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.BatchNo)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ClientJobId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ClientJobIddate).HasColumnType("datetime");
            entity.Property(e => e.Color)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateofUpload).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileInwardType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ImprintColors1)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ImprintColors2)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ImprintColors3)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobClosedUtc)
                .HasColumnType("datetime")
                .HasColumnName("JobClosedUTC");
            entity.Property(e => e.JobDate).HasColumnType("datetime");
            entity.Property(e => e.JobDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobReferenceId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.LogoDimensionWidth)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.LogoDimensionsLength)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ParentJobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.PoNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks).IsUnicode(false);
            entity.Property(e => e.SalesPersonName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.SchoolName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Style)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TeamCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Temp)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.Username)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.VirtualProof)
                .HasMaxLength(3)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewJobOrderforFileUpload>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_JobOrderforFileUpload");

            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.FileUploadPath).HasMaxLength(200);
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewJobQueryInCc>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_JobQueryInCC");

            entity.Property(e => e.BalanceTime).HasColumnName("Balance_time");
            entity.Property(e => e.CcemailId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CCEmailId");
            entity.Property(e => e.Ccid).HasColumnName("CCId");
            entity.Property(e => e.Color)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.CustomerClassification)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CustomerType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EstfileReceivedDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTFileReceivedDate");
            entity.Property(e => e.EstjobDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTJobDate");
            entity.Property(e => e.FileInwardType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.Instruction).IsUnicode(false);
            entity.Property(e => e.Jid).HasColumnName("JId");
            entity.Property(e => e.JobDate).HasColumnType("datetime");
            entity.Property(e => e.JobDateEst)
                .HasColumnType("datetime")
                .HasColumnName("JobDateEST");
            entity.Property(e => e.JobDateQueryDate)
                .HasColumnType("datetime")
                .HasColumnName("JobDate_QueryDate");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JofileUploadPath)
                .HasMaxLength(200)
                .HasColumnName("JOFileUploadPath");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ParentJobFilePath)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.ParentJobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReferenceJobFilePath)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.ReferenceJobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Remarks).IsUnicode(false);
            entity.Property(e => e.SalesPersonName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.TranFileUploadPath).HasMaxLength(200);
        });

        modelBuilder.Entity<ViewJobsAllocatedSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_JobsAllocatedSummary");

            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.JobLoadInMinutes).HasColumnName("Job Load in Minutes");
        });

        modelBuilder.Entity<ViewLogtext>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_logtext");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Module)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewMailContact>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("view_MailContact");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<ViewOperationsReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_OperationsReport");

            entity.Property(e => e.Ccintake)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CCIntake");
            entity.Property(e => e.Ccupload)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CCUpload");
            entity.Property(e => e.Client)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileInwardType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("date");
            entity.Property(e => e.FileStatus)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.FinalStatus)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.JobDate).HasColumnType("date");
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PrerrorStatus)
                .IsUnicode(false)
                .HasColumnName("PRErrorStatus");
            entity.Property(e => e.ProofAccepted)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.QcerrorStatus)
                .IsUnicode(false)
                .HasColumnName("QCErrorStatus");
            entity.Property(e => e.Wfmid).HasColumnName("WFMId");
        });

        modelBuilder.Entity<ViewQueryTracker>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_QueryTracker");

            entity.Property(e => e.EstfileReceivedDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTFileReceivedDate");
            entity.Property(e => e.Jid)
                .ValueGeneratedOnAdd()
                .HasColumnName("JId");
        });

        modelBuilder.Entity<ViewQuotationDetailsinJobOrder>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_QuotationDetailsinJobOrder");

            entity.Property(e => e.Comments).IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Instruction).IsUnicode(false);
            entity.Property(e => e.ParentJobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Remarks).IsUnicode(false);
            entity.Property(e => e.Scope)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SpecialPrice).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Suggestion).IsUnicode(false);
        });

        modelBuilder.Entity<ViewQuotationJobDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_QuotationJobDetails");

            entity.Property(e => e.Comments).IsUnicode(false);
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DateofDelivery).HasColumnType("datetime");
            entity.Property(e => e.DateofUpload).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EstDateofDelivery).HasColumnType("datetime");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.FileUploadPath).HasMaxLength(200);
            entity.Property(e => e.Instruction).IsUnicode(false);
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.SalesPersonName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.SpecialPrice).HasColumnType("decimal(8, 2)");
        });

        modelBuilder.Entity<ViewQuotationTracker>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_QuotationTracker");

            entity.Property(e => e.EstfileReceivedDate)
                .HasColumnType("datetime")
                .HasColumnName("ESTFileReceivedDate");
            entity.Property(e => e.Jid)
                .ValueGeneratedOnAdd()
                .HasColumnName("JId");
        });

        modelBuilder.Entity<ViewShiftVsEmployee>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_ShiftVsEmployee");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");
            entity.Property(e => e.EffectiveTo).HasColumnType("datetime");
            entity.Property(e => e.FromTime).HasMaxLength(50);
            entity.Property(e => e.ShiftType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ToTime).HasMaxLength(50);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<ViewShiftVsEmployeeShift>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_ShiftVsEmployee_Shift");

            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");
            entity.Property(e => e.EffectiveTo).HasColumnType("datetime");
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewTatreport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_TATReport");

            entity.Property(e => e.ClientName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CustomerJobType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DateofUpload).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.HoldStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.JobId)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.JobStatusDescription)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<ViewVoucherControler>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_VoucherControler");

            entity.Property(e => e.Cocode).HasColumnType("numeric(4, 0)");
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");
            entity.Property(e => e.EffectiveTo).HasColumnType("datetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Prefix)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Suffix)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
        });

        modelBuilder.Entity<VisClosureResult>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<VoucherControl>(entity =>
        {
            entity.ToTable("VoucherControl");

            entity.Property(e => e.Cocode).HasColumnType("numeric(4, 0)");
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");
            entity.Property(e => e.EffectiveTo).HasColumnType("datetime");
            entity.Property(e => e.Prefix)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Suffix)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Department).WithMany(p => p.VoucherControls)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VoucherControl_Department");

            entity.HasOne(d => d.Transaction).WithMany(p => p.VoucherControls)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VoucherControl_TransactionType");
        });

        modelBuilder.Entity<WellnessStatus>(entity =>
        {
            entity.ToTable("WellnessStatus");

            entity.Property(e => e.Aarogya).IsUnicode(false);
            entity.Property(e => e.Area).IsUnicode(false);
            entity.Property(e => e.Contact).IsUnicode(false);
            entity.Property(e => e.ContactF).IsUnicode(false);
            entity.Property(e => e.Covid).IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Email)
                .IsUnicode(false)
                .HasColumnName("EMail");
            entity.Property(e => e.Emobile)
                .IsUnicode(false)
                .HasColumnName("EMobile");
            entity.Property(e => e.EmployeeName).IsUnicode(false);
            entity.Property(e => e.Employeecode).IsUnicode(false);
            entity.Property(e => e.Family).IsUnicode(false);
            entity.Property(e => e.Foreigns).IsUnicode(false);
            entity.Property(e => e.Stay).IsUnicode(false);
            entity.Property(e => e.Symptoms).IsUnicode(false);
            entity.Property(e => e.Transport).IsUnicode(false);
            entity.Property(e => e.Uforeign)
                .IsUnicode(false)
                .HasColumnName("UForeign");
        });

        modelBuilder.Entity<MonthlyRevenueResult>().HasNoKey().ToView(null);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}

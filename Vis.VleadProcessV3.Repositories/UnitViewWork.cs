using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.Repositories
{
    public class UnitViewWork : IDisposable
    {
      
        private  ApplicationDbContext context;
        private TallyDbContext Tally;
        public UnitViewWork(ApplicationDbContext dbContext,TallyDbContext tallyDbContext)
        {
            context = dbContext;
            Tally= tallyDbContext;
        }
        

        private IQueryBuilder<ViewAssignedJob> Viewassignedjobs;
        private IQueryBuilder<ViewAutoGetGenerateforInvoice> Viewautogetgenerateforinvoice;
        private IQueryBuilder<View_BenchScheduler> Viewbenchscheduler;
        private IQueryBuilder<ViewBusinessDay> Viewbusinessdays;
        private IQueryBuilder<ViewCancelledJob> Viewcancelledjobs;
        private IQueryBuilder<ViewCollectionsReport> Viewcollectionsreport;
        private IQueryBuilder<ViewConvertedClientOrder> Viewconvertedclientorder;
        private IQueryBuilder<ViewCurrentTrayFileStatusReport> Viewcurrenttrayfilestatusreport;
        private IQueryBuilder<ViewCustomerAddresswithLocation> Viewcustomeraddresswithlocation;
        private IQueryBuilder<ViewCustomerDetail> Viewcustomerdetails;
        private IQueryBuilder<ViewCustomerStatus> Viewcustomerstatus;
        private IQueryBuilder<ViewCustomerVsEmployee> ViewcustomerVsemployee;
        private IQueryBuilder<ViewCustomerVsProcess> ViewcustomerVsprocess;
        private IQueryBuilder<ViewDailyOperationsReport> Viewdailyoperationsreport;
        private IQueryBuilder<ViewDailyProductionReport> Viewdailyproductionreport;
        private IQueryBuilder<ViewDailyReport> Viewdailyreport;
        private IQueryBuilder<ViewDepartmentBasedCustomer> Viewdepartmentbasedcustomers;
        private IQueryBuilder<ViewEmployeeDetail> Viewemployeedetails;
        private IQueryBuilder<ViewEmployeeDetailsfromEmp> ViewemployeedetailsfromEMP;
        private IQueryBuilder<ViewEmployeeDetailsWithAdd> ViewemployeedetailsWithADD;
        private IQueryBuilder<ViewEmployeeDetailsWithAddress> ViewemployeedetailsWithaddress;
        private IQueryBuilder<ViewEmployeeDetailswithShift> Viewemployeedetailswithshift;
        private IQueryBuilder<ViewEmployeeWiseReport> ViewemployeeWisereport;
        private IQueryBuilder<ViewEmployeeWithShift> ViewemployeeWithshift;
        private IQueryBuilder<ViewFileInwardExcel> Viewfileinwardexcel;
        private IQueryBuilder<ViewFileStatusReport> Viewfilestatusreport;
        private IQueryBuilder<ViewFileStatusReportRef> ViewfilestatusreportRef;
        private IQueryBuilder<ViewGeneratedInvoice> Viewgeneratedinvoice;
        private IQueryBuilder<ViewGetCancelledInvoiceList> Viewgetcancelledinvoicelist;
        private IQueryBuilder<ViewGetCheckWaiverJob> Viewgetcheckwaiverjobs;
        private IQueryBuilder<ViewGetConfirmedGeneratedInvoice> Viewgetconfirmedgeneratedinvoice;
        private IQueryBuilder<ViewGetCustomerforInvoiceCancel> Viewgetcustomerforinvoicecancel;
        private IQueryBuilder<ViewGetCustomerforSelection> Viewgetcustomerforselection;
        private IQueryBuilder<ViewGetDropInvoiceCancel> Viewgetdropinvoicecancel;
        private IQueryBuilder<View_GetDropInvoiceDetailTab> Viewgetdropinvoicedetailtab;
        private IQueryBuilder<View_GetDropInvoiceIntegration> Viewgetdropinvoiceintegration;
        private IQueryBuilder<ViewGetEmployeesForPendingJob> ViewgetemployeesForpendingjobs;
        private IQueryBuilder<ViewGetGenerateforInvoice> Viewgetgenerateforinvoice;
        private IQueryBuilder<View_GetGeneratingInvoice> Viewgetgeneratinginvoice;
        private IQueryBuilder<View_GetIntegrationMaster> Viewgetintegrationmaster;
        private IQueryBuilder<ViewGetIntegrationTran> Viewgetintegrationtran;
        private IQueryBuilder<ViewGetInvoiceMaster> Viewgetinvoicemaster;
        private IQueryBuilder<ViewGetInvoiceMasterforDayDiff> Viewgetinvoicemasterfordaydiff;
        private IQueryBuilder<ViewGetPricingforInvoice> Viewgetpricingforinvoice;
        private IQueryBuilder<ViewGetTranMergeInvoiceCancel> Viewgettranmergeinvoicecancel;
        private IQueryBuilder<ViewGetTransactionTypeCancel> Viewgettransactiontypecancel;
        private IQueryBuilder<ViewGetTransactionTypeInvoice> Viewgettransactiontypeinvoice;
        private IQueryBuilder<ViewGetUser> Viewgetuser;
        private IQueryBuilder<ViewGetWorkflowListforLogin> Viewgetworkflowlistforlogin;
        private IQueryBuilder<ViewInvoiceCompletedList> Viewinvoicecompletedlist;
        private IQueryBuilder<View_InvoiceMater> Viewinvoicemater;
        private IQueryBuilder<ViewJobAllocation> Viewjoballocation;
        private IQueryBuilder<ViewJobAllocationandJobOrder> Viewjoballocationandjoborder;
        private IQueryBuilder<ViewJobDetail> Viewjobdetails;
        private IQueryBuilder<ViewJobForcompletedJobs> ViewjobForcompletedjobs;
        private IQueryBuilder<ViewJobMovement> Viewjobmovement;
        private IQueryBuilder<ViewJobOrder> Viewjoborder;
        private IQueryBuilder<ViewJobOrderforFileUpload> Viewjoborderforfileupload;
        private IQueryBuilder<ViewJobQueryInCc> ViewjobqueryInCC;
        private IQueryBuilder<ViewJobsAllocatedSummary> Viewjobsallocatedsummary;
        private IQueryBuilder<ViewMailContact> Viewmailcontact;
        private IQueryBuilder<ViewOperationsReport> ViewoperationsReport;
        private IQueryBuilder<View_OutStandingList> Viewoutstandinglist;
        private IQueryBuilder<ViewQueryTracker> Viewquerytracker;
        private IQueryBuilder<ViewQuotationDetailsinJobOrder> Viewquotationdetailsinjoborder;
        private IQueryBuilder<ViewQuotationJobDetail> Viewquotationjobdetails;
        private IQueryBuilder<ViewQuotationTracker> Viewquotationtracker;
        private IQueryBuilder<ViewShiftVsEmployee> ViewshiftVsemployee;
        private IQueryBuilder<ViewShiftVsEmployeeShift> ViewshiftVsemployee_shift;
        private IQueryBuilder<ViewTatreport> ViewTATreport;
        private IQueryBuilder<ViewVoucherControler> Viewvouchercontroler;
        private IQueryBuilder<ViewGetBirthandJoining> Viewgetbirthandjoining;

        public IQueryBuilder<ViewAssignedJob> ViewAssignedJobs
        {
            get
            {
                if (this.Viewassignedjobs == null)
                {
                    this.Viewassignedjobs = new QueryBuilder<ViewAssignedJob>(context,Tally);
                }
                return Viewassignedjobs;
            }
        }
        public IQueryBuilder<ViewAutoGetGenerateforInvoice> ViewAutoGetGenerateforInvoice
        {
            get
            {
                if (this.Viewautogetgenerateforinvoice == null)
                {
                    this.Viewautogetgenerateforinvoice = new QueryBuilder<ViewAutoGetGenerateforInvoice>(context,Tally);
                }
                return Viewautogetgenerateforinvoice;
            }
        }
        public IQueryBuilder<View_BenchScheduler> ViewBenchScheduler
        {
            get
            {
                if (this.Viewbenchscheduler == null)
                {
                    this.Viewbenchscheduler = new QueryBuilder<View_BenchScheduler>(context,Tally);
                }
                return Viewbenchscheduler;
            }
        }
        public IQueryBuilder<ViewBusinessDay> ViewBusinessDays
        {
            get
            {
                if (this.Viewbusinessdays == null)
                {
                    this.Viewbusinessdays = new QueryBuilder<ViewBusinessDay>(context,Tally);
                }
                return Viewbusinessdays;
            }
        }
        public IQueryBuilder<ViewCancelledJob> ViewCancelledJobs
        {
            get
            {
                if (this.Viewcancelledjobs == null)
                {
                    this.Viewcancelledjobs = new QueryBuilder<ViewCancelledJob>(context,Tally);
                }
                return Viewcancelledjobs;
            }
        }
        public IQueryBuilder<ViewCollectionsReport> ViewCollectionsReport
        {
            get
            {
                if (this.Viewcollectionsreport == null)
                {
                    this.Viewcollectionsreport = new QueryBuilder<ViewCollectionsReport>(context,Tally);
                }
                return Viewcollectionsreport;
            }
        }
        public IQueryBuilder<ViewConvertedClientOrder> ViewConvertedClientOrder
        {
            get
            {
                if (this.Viewconvertedclientorder == null)
                {
                    this.Viewconvertedclientorder = new QueryBuilder<ViewConvertedClientOrder>(context,Tally);
                }
                return Viewconvertedclientorder;
            }
        }
        public IQueryBuilder<ViewCurrentTrayFileStatusReport> ViewCurrentTrayFileStatusReport
        {
            get
            {
                if (this.Viewcurrenttrayfilestatusreport == null)
                {
                    this.Viewcurrenttrayfilestatusreport = new QueryBuilder<ViewCurrentTrayFileStatusReport>(context,Tally);
                }
                return Viewcurrenttrayfilestatusreport;
            }
        }
        public IQueryBuilder<ViewCustomerAddresswithLocation> ViewCustomerAddresswithLocation
        {
            get
            {
                if (this.Viewcustomeraddresswithlocation == null)
                {
                    this.Viewcustomeraddresswithlocation = new QueryBuilder<ViewCustomerAddresswithLocation>(context,Tally);
                }
                return Viewcustomeraddresswithlocation;
            }
        }
        public IQueryBuilder<ViewCustomerDetail> ViewCustomerDetails
        {
            get
            {
                if (this.Viewcustomerdetails == null)
                {
                    this.Viewcustomerdetails = new QueryBuilder<ViewCustomerDetail>(context,Tally);
                }
                return Viewcustomerdetails;
            }
        }
        public IQueryBuilder<ViewCustomerStatus> ViewCustomerStatus
        {
            get
            {
                if (this.Viewcustomerstatus == null)
                {
                    this.Viewcustomerstatus = new QueryBuilder<ViewCustomerStatus>(context,Tally);
                }
                return Viewcustomerstatus;
            }
        }
        public IQueryBuilder<ViewCustomerVsEmployee> ViewCustomerVsEmployee
        {
            get
            {
                if (this.ViewcustomerVsemployee == null)
                {
                    this.ViewcustomerVsemployee = new QueryBuilder<ViewCustomerVsEmployee>(context,Tally);
                }
                return ViewcustomerVsemployee;
            }
        }
        public IQueryBuilder<ViewCustomerVsProcess> ViewCustomerVsProcess
        {
            get
            {
                if (this.ViewcustomerVsprocess == null)
                {
                    this.ViewcustomerVsprocess = new QueryBuilder<ViewCustomerVsProcess>(context,Tally);
                }
                return ViewcustomerVsprocess;
            }
        }
        public IQueryBuilder<ViewDailyOperationsReport> ViewDailyOperationsReport
        {
            get
            {
                if (this.Viewdailyoperationsreport == null)
                {
                    this.Viewdailyoperationsreport = new QueryBuilder<ViewDailyOperationsReport>(context,Tally);
                }
                return Viewdailyoperationsreport;
            }
        }
        public IQueryBuilder<ViewDailyProductionReport> ViewDailyProductionReport
        {
            get
            {
                if (this.Viewdailyproductionreport == null)
                {
                    this.Viewdailyproductionreport = new QueryBuilder<ViewDailyProductionReport>(context,Tally);
                }
                return Viewdailyproductionreport;
            }
        }
        public IQueryBuilder<ViewDailyReport> ViewDailyReport
        {
            get
            {
                if (this.Viewdailyreport == null)
                {
                    this.Viewdailyreport = new QueryBuilder<ViewDailyReport>(context,Tally);
                }
                return Viewdailyreport;
            }
        }
        public IQueryBuilder<ViewDepartmentBasedCustomer> ViewDepartmentBasedCustomers
        {
            get
            {
                if (this.Viewdepartmentbasedcustomers == null)
                {
                    this.Viewdepartmentbasedcustomers = new QueryBuilder<ViewDepartmentBasedCustomer>(context,Tally);
                }
                return Viewdepartmentbasedcustomers;
            }
        }
        public IQueryBuilder<ViewEmployeeDetail> ViewEmployeeDetails
        {
            get
            {
                if (this.Viewemployeedetails == null)
                {
                    this.Viewemployeedetails = new QueryBuilder<ViewEmployeeDetail>(context,Tally);
                }
                return Viewemployeedetails;
            }
        }
        public IQueryBuilder<ViewEmployeeDetailsfromEmp> ViewEmployeeDetailsfromEMP
        {
            get
            {
                if (this.ViewemployeedetailsfromEMP == null)
                {
                    this.ViewemployeedetailsfromEMP = new QueryBuilder<ViewEmployeeDetailsfromEmp>(context,Tally);
                }
                return ViewemployeedetailsfromEMP;
            }
        }
        public IQueryBuilder<ViewEmployeeDetailsWithAdd> ViewEmployeeDetailsWithADD
        {
            get
            {
                if (this.ViewemployeedetailsWithADD == null)
                {
                    this.ViewemployeedetailsWithADD = new QueryBuilder<ViewEmployeeDetailsWithAdd>(context,Tally);
                }
                return ViewemployeedetailsWithADD;
            }
        }
        public IQueryBuilder<ViewEmployeeDetailsWithAddress> ViewEmployeeDetailsWithAddress
        {
            get
            {
                if (this.ViewemployeedetailsWithaddress == null)
                {
                    this.ViewemployeedetailsWithaddress = new QueryBuilder<ViewEmployeeDetailsWithAddress>(context,Tally);
                }
                return ViewemployeedetailsWithaddress;
            }
        }
        public IQueryBuilder<ViewEmployeeDetailswithShift> ViewEmployeeDetailswithShift
        {
            get
            {
                if (this.Viewemployeedetailswithshift == null)
                {
                    this.Viewemployeedetailswithshift = new QueryBuilder<ViewEmployeeDetailswithShift>(context,Tally);
                }
                return Viewemployeedetailswithshift;
            }
        }
        public IQueryBuilder<ViewEmployeeWiseReport> ViewEmployeeWiseReport
        {
            get
            {
                if (this.ViewemployeeWisereport == null)
                {
                    this.ViewemployeeWisereport = new QueryBuilder<ViewEmployeeWiseReport>(context,Tally);
                }
                return ViewemployeeWisereport;
            }
        }
        public IQueryBuilder<ViewEmployeeWithShift> ViewEmployeeWithShift
        {
            get
            {
                if (this.ViewemployeeWithshift == null)
                {
                    this.ViewemployeeWithshift = new QueryBuilder<ViewEmployeeWithShift>(context,Tally);
                }
                return ViewemployeeWithshift;
            }
        }
        public IQueryBuilder<ViewFileInwardExcel> ViewFileInwardExcel
        {
            get
            {
                if (this.Viewfileinwardexcel == null)
                {
                    this.Viewfileinwardexcel = new QueryBuilder<ViewFileInwardExcel>(context,Tally);
                }
                return Viewfileinwardexcel;
            }
        }
        public IQueryBuilder<ViewFileStatusReport> ViewFileStatusReport
        {
            get
            {
                if (this.Viewfilestatusreport == null)
                {
                    this.Viewfilestatusreport = new QueryBuilder<ViewFileStatusReport>(context,Tally);
                }
                return Viewfilestatusreport;
            }
        }
        public IQueryBuilder<ViewFileStatusReportRef> ViewFileStatusReportRef
        {
            get
            {
                if (this.ViewfilestatusreportRef == null)
                {
                    this.ViewfilestatusreportRef = new QueryBuilder<ViewFileStatusReportRef>(context,Tally);
                }
                return ViewfilestatusreportRef;
            }
        }
        public IQueryBuilder<ViewGeneratedInvoice> ViewGeneratedInvoice
        {
            get
            {
                if (this.Viewgeneratedinvoice == null)
                {
                    this.Viewgeneratedinvoice = new QueryBuilder<ViewGeneratedInvoice>(context,Tally);
                }
                return Viewgeneratedinvoice;
            }
        }
        public IQueryBuilder<ViewGetCancelledInvoiceList> ViewGetCancelledInvoiceList
        {
            get
            {
                if (this.Viewgetcancelledinvoicelist == null)
                {
                    this.Viewgetcancelledinvoicelist = new QueryBuilder<ViewGetCancelledInvoiceList>(context,Tally);
                }
                return Viewgetcancelledinvoicelist;
            }
        }
        public IQueryBuilder<ViewGetCheckWaiverJob> ViewGetCheckWaiverJobs
        {
            get
            {
                if (this.Viewgetcheckwaiverjobs == null)
                {
                    this.Viewgetcheckwaiverjobs = new QueryBuilder<ViewGetCheckWaiverJob>(context,Tally);
                }
                return Viewgetcheckwaiverjobs;
            }
        }
        public IQueryBuilder<ViewGetConfirmedGeneratedInvoice> ViewGetConfirmedGeneratedInvoice
        {
            get
            {
                if (this.Viewgetconfirmedgeneratedinvoice == null)
                {
                    this.Viewgetconfirmedgeneratedinvoice = new QueryBuilder<ViewGetConfirmedGeneratedInvoice>(context,Tally);
                }
                return Viewgetconfirmedgeneratedinvoice;
            }
        }
        public IQueryBuilder<ViewGetCustomerforInvoiceCancel> ViewGetCustomerforInvoiceCancel
        {
            get
            {
                if (this.Viewgetcustomerforinvoicecancel == null)
                {
                    this.Viewgetcustomerforinvoicecancel = new QueryBuilder<ViewGetCustomerforInvoiceCancel>(context,Tally);
                }
                return Viewgetcustomerforinvoicecancel;
            }
        }
        public IQueryBuilder<ViewGetCustomerforSelection> ViewGetCustomerforSelection
        {
            get
            {
                if (this.Viewgetcustomerforselection == null)
                {
                    this.Viewgetcustomerforselection = new QueryBuilder<ViewGetCustomerforSelection>(context,Tally);
                }
                return Viewgetcustomerforselection;
            }
        }
        public IQueryBuilder<ViewGetDropInvoiceCancel> ViewGetDropInvoiceCancel
        {
            get
            {
                if (this.Viewgetdropinvoicecancel == null)
                {
                    this.Viewgetdropinvoicecancel = new QueryBuilder<ViewGetDropInvoiceCancel>(context,Tally);
                }
                return Viewgetdropinvoicecancel;
            }
        }
        public IQueryBuilder<View_GetDropInvoiceDetailTab> ViewGetDropInvoiceDetailTab
        {
            get
            {
                if (this.Viewgetdropinvoicedetailtab == null)
                {
                    this.Viewgetdropinvoicedetailtab = new QueryBuilder<View_GetDropInvoiceDetailTab>(context,Tally);
                }
                return Viewgetdropinvoicedetailtab;
            }
        }
        public IQueryBuilder<View_GetDropInvoiceIntegration> ViewGetDropInvoiceIntegration
        {
            get
            {
                if (this.Viewgetdropinvoiceintegration == null)
                {
                    this.Viewgetdropinvoiceintegration = new QueryBuilder<View_GetDropInvoiceIntegration>(context,Tally);
                }
                return Viewgetdropinvoiceintegration;
            }
        }
        public IQueryBuilder<ViewGetEmployeesForPendingJob> ViewGetEmployeesForPendingJobs
        {
            get
            {
                if (this.ViewgetemployeesForpendingjobs == null)
                {
                    this.ViewgetemployeesForpendingjobs = new QueryBuilder<ViewGetEmployeesForPendingJob>(context,Tally);
                }
                return ViewgetemployeesForpendingjobs;
            }
        }
        public IQueryBuilder<ViewGetGenerateforInvoice> ViewGetGenerateforInvoice
        {
            get
            {
                if (this.Viewgetgenerateforinvoice == null)
                {
                    this.Viewgetgenerateforinvoice = new QueryBuilder<ViewGetGenerateforInvoice>(context,Tally);
                }
                return Viewgetgenerateforinvoice;
            }
        }
        public IQueryBuilder<View_GetGeneratingInvoice> ViewGetGeneratingInvoice
        {
            get
            {
                if (this.Viewgetgeneratinginvoice == null)
                {
                    this.Viewgetgeneratinginvoice = new QueryBuilder<View_GetGeneratingInvoice>(context,Tally);
                }
                return Viewgetgeneratinginvoice;
            }
        }
        public IQueryBuilder<View_GetIntegrationMaster> ViewGetIntegrationMaster
        {
            get
            {
                if (this.Viewgetintegrationmaster == null)
                {
                    this.Viewgetintegrationmaster = new QueryBuilder<View_GetIntegrationMaster>(context,Tally);
                }
                return Viewgetintegrationmaster;
            }
        }
        public IQueryBuilder<ViewGetIntegrationTran> ViewGetIntegrationTran
        {
            get
            {
                if (this.Viewgetintegrationtran == null)
                {
                    this.Viewgetintegrationtran = new QueryBuilder<ViewGetIntegrationTran>(context,Tally);
                }
                return Viewgetintegrationtran;
            }
        }
        public IQueryBuilder<ViewGetInvoiceMaster> ViewGetInvoiceMaster
        {
            get
            {
                if (this.Viewgetinvoicemaster == null)
                {
                    this.Viewgetinvoicemaster = new QueryBuilder<ViewGetInvoiceMaster>(context,Tally);
                }
                return Viewgetinvoicemaster;
            }
        }
        public IQueryBuilder<ViewGetInvoiceMasterforDayDiff> ViewGetInvoiceMasterforDayDiff
        {
            get
            {
                if (this.Viewgetinvoicemasterfordaydiff == null)
                {
                    this.Viewgetinvoicemasterfordaydiff = new QueryBuilder<ViewGetInvoiceMasterforDayDiff>(context,Tally);
                }
                return Viewgetinvoicemasterfordaydiff;
            }
        }
        public IQueryBuilder<ViewGetPricingforInvoice> ViewGetPricingforInvoice
        {
            get
            {
                if (this.Viewgetpricingforinvoice == null)
                {
                    this.Viewgetpricingforinvoice = new QueryBuilder<ViewGetPricingforInvoice>(context,Tally);
                }
                return Viewgetpricingforinvoice;
            }
        }
        public IQueryBuilder<ViewGetTranMergeInvoiceCancel> ViewGetTranMergeInvoiceCancel
        {
            get
            {
                if (this.Viewgettranmergeinvoicecancel == null)
                {
                    this.Viewgettranmergeinvoicecancel = new QueryBuilder<ViewGetTranMergeInvoiceCancel>(context,Tally);
                }
                return Viewgettranmergeinvoicecancel;
            }
        }
        public IQueryBuilder<ViewGetTransactionTypeCancel> ViewGetTransactionTypeCancel
        {
            get
            {
                if (this.Viewgettransactiontypecancel == null)
                {
                    this.Viewgettransactiontypecancel = new QueryBuilder<ViewGetTransactionTypeCancel>(context,Tally);
                }
                return Viewgettransactiontypecancel;
            }
        }
        public IQueryBuilder<ViewGetTransactionTypeInvoice> ViewGetTransactionTypeInvoice
        {
            get
            {
                if (this.Viewgettransactiontypeinvoice == null)
                {
                    this.Viewgettransactiontypeinvoice = new QueryBuilder<ViewGetTransactionTypeInvoice>(context,Tally);
                }
                return Viewgettransactiontypeinvoice;
            }
        }
        public IQueryBuilder<ViewGetUser> ViewGetUser
        {
            get
            {
                if (this.Viewgetuser == null)
                {
                    this.Viewgetuser = new QueryBuilder<ViewGetUser>(context,Tally);
                }
                return Viewgetuser;
            }
        }
        public IQueryBuilder<ViewGetWorkflowListforLogin> ViewGetWorkflowListforLogin
        {
            get
            {
                if (this.Viewgetworkflowlistforlogin == null)
                {
                    this.Viewgetworkflowlistforlogin = new QueryBuilder<ViewGetWorkflowListforLogin>(context,Tally);
                }
                return Viewgetworkflowlistforlogin;
            }
        }
        public IQueryBuilder<ViewInvoiceCompletedList> ViewInvoiceCompletedList
        {
            get
            {
                if (this.Viewinvoicecompletedlist == null)
                {
                    this.Viewinvoicecompletedlist = new QueryBuilder<ViewInvoiceCompletedList>(context,Tally);
                }
                return Viewinvoicecompletedlist;
            }
        }
        public IQueryBuilder<View_InvoiceMater> ViewInvoiceMater
        {
            get
            {
                if (this.Viewinvoicemater == null)
                {
                    this.Viewinvoicemater = new QueryBuilder<View_InvoiceMater>(context,Tally);
                }
                return Viewinvoicemater;
            }
        }
        public IQueryBuilder<ViewJobAllocation> ViewJobAllocation
        {
            get
            {
                if (this.Viewjoballocation == null)
                {
                    this.Viewjoballocation = new QueryBuilder<ViewJobAllocation>(context,Tally);
                }
                return Viewjoballocation;
            }
        }
        public IQueryBuilder<ViewJobAllocationandJobOrder> ViewJobAllocationandJobOrder
        {
            get
            {
                if (this.Viewjoballocationandjoborder == null)
                {
                    this.Viewjoballocationandjoborder = new QueryBuilder<ViewJobAllocationandJobOrder>(context,Tally);
                }
                return Viewjoballocationandjoborder;
            }
        }
        public IQueryBuilder<ViewJobDetail> ViewJobDetails
        {
            get
            {
                if (this.Viewjobdetails == null)
                {
                    this.Viewjobdetails = new QueryBuilder<ViewJobDetail>(context,Tally);
                }
                return Viewjobdetails;
            }
        }
        public IQueryBuilder<ViewJobForcompletedJobs> ViewJobForcompletedJobs
        {
            get
            {
                if (this.ViewjobForcompletedjobs == null)
                {
                    this.ViewjobForcompletedjobs = new QueryBuilder<ViewJobForcompletedJobs>(context,Tally);
                }
                return ViewjobForcompletedjobs;
            }
        }
        public IQueryBuilder<ViewJobMovement> ViewJobMovement
        {
            get
            {
                if (this.Viewjobmovement == null)
                {
                    this.Viewjobmovement = new QueryBuilder<ViewJobMovement>(context,Tally);
                }
                return Viewjobmovement;
            }
        }
        public IQueryBuilder<ViewJobOrder> ViewJobOrder
        {
            get
            {
                if (this.Viewjoborder == null)
                {
                    this.Viewjoborder = new QueryBuilder<ViewJobOrder>(context,Tally);
                }
                return Viewjoborder;
            }
        }
        public IQueryBuilder<ViewJobOrderforFileUpload> ViewJobOrderforFileUpload
        {
            get
            {
                if (this.Viewjoborderforfileupload == null)
                {
                    this.Viewjoborderforfileupload = new QueryBuilder<ViewJobOrderforFileUpload>(context,Tally);
                }
                return Viewjoborderforfileupload;
            }
        }
        public IQueryBuilder<ViewJobQueryInCc> ViewJobQueryInCC
        {
            get
            {
                if (this.ViewjobqueryInCC == null)
                {
                    this.ViewjobqueryInCC = new QueryBuilder<ViewJobQueryInCc>(context,Tally);
                }
                return ViewjobqueryInCC;
            }
        }
        public IQueryBuilder<ViewJobsAllocatedSummary> ViewJobsAllocatedSummary
        {
            get
            {
                if (this.Viewjobsallocatedsummary == null)
                {
                    this.Viewjobsallocatedsummary = new QueryBuilder<ViewJobsAllocatedSummary>(context,Tally);
                }
                return Viewjobsallocatedsummary;
            }
        }
        public IQueryBuilder<ViewMailContact> ViewMailContact
        {
            get
            {
                if (this.Viewmailcontact == null)
                {
                    this.Viewmailcontact = new QueryBuilder<ViewMailContact>(context,Tally);
                }
                return Viewmailcontact;
            }
        }
        public IQueryBuilder<ViewOperationsReport> ViewOperationsReport
        {
            get
            {
                if (this.ViewoperationsReport == null)
                {
                    this.ViewoperationsReport = new QueryBuilder<ViewOperationsReport>(context,Tally);
                }
                return ViewoperationsReport;
            }
        }
        public IQueryBuilder<View_OutStandingList> ViewOutStandingList
        {
            get
            {
                if (this.Viewoutstandinglist == null)
                {
                    this.Viewoutstandinglist = new QueryBuilder<View_OutStandingList>(context,Tally);
                }
                return Viewoutstandinglist;
            }
        }
        public IQueryBuilder<ViewQueryTracker> ViewQueryTracker
        {
            get
            {
                if (this.Viewquerytracker == null)
                {
                    this.Viewquerytracker = new QueryBuilder<ViewQueryTracker>(context,Tally);
                }
                return Viewquerytracker;
            }
        }
        public IQueryBuilder<ViewQuotationDetailsinJobOrder> ViewQuotationDetailsinJobOrder
        {
            get
            {
                if (this.Viewquotationdetailsinjoborder == null)
                {
                    this.Viewquotationdetailsinjoborder = new QueryBuilder<ViewQuotationDetailsinJobOrder>(context,Tally);
                }
                return Viewquotationdetailsinjoborder;
            }
        }
        public IQueryBuilder<ViewQuotationJobDetail> ViewQuotationJobDetails
        {
            get
            {
                if (this.Viewquotationjobdetails == null)
                {
                    this.Viewquotationjobdetails = new QueryBuilder<ViewQuotationJobDetail>(context,Tally);
                }
                return Viewquotationjobdetails;
            }
        }
        public IQueryBuilder<ViewQuotationTracker> ViewQuotationTracker
        {
            get
            {
                if (this.Viewquotationtracker == null)
                {
                    this.Viewquotationtracker = new QueryBuilder<ViewQuotationTracker>(context,Tally);
                }
                return Viewquotationtracker;
            }
        }
        public IQueryBuilder<ViewShiftVsEmployee> ViewShiftVsEmployee
        {
            get
            {
                if (this.ViewshiftVsemployee == null)
                {
                    this.ViewshiftVsemployee = new QueryBuilder<ViewShiftVsEmployee>(context,Tally);
                }
                return ViewshiftVsemployee;
            }
        }
        public IQueryBuilder<ViewShiftVsEmployeeShift> ViewShiftVsEmployee_Shift
        {
            get
            {
                if (this.ViewshiftVsemployee_shift == null)
                {
                    this.ViewshiftVsemployee_shift = new QueryBuilder<ViewShiftVsEmployeeShift>(context,Tally);
                }
                return ViewshiftVsemployee_shift;
            }
        }
        public IQueryBuilder<ViewTatreport> ViewTATReport
        {
            get
            {
                if (this.ViewTATreport == null)
                {
                    this.ViewTATreport = new QueryBuilder<ViewTatreport>(context,Tally);
                }
                return ViewTATreport;
            }
        }
        public IQueryBuilder<ViewVoucherControler> ViewVoucherControler
        {
            get
            {
                if (this.Viewvouchercontroler == null)
                {
                    this.Viewvouchercontroler = new QueryBuilder<ViewVoucherControler>(context,Tally);
                }
                return Viewvouchercontroler;
            }
        }
        public IQueryBuilder<ViewGetBirthandJoining> ViewGetBirthandJoining
        {
            get
            {
                if (this.Viewgetbirthandjoining == null)
                {
                    this.Viewgetbirthandjoining = new QueryBuilder<ViewGetBirthandJoining>(context,Tally);
                }
                return Viewgetbirthandjoining;
            }
        }
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            if (disposing)
            {
                this.context.Dispose();
            }

            this.disposed = true;
            this.context = null;
        }
        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}

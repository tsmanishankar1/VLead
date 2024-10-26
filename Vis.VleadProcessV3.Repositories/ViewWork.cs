using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.Repositories
{
  
        public class ViewWork /*: IDisposable*/
        {
      
        private readonly ApplicationDbContext context;
        public ViewWork(ApplicationDbContext dbContext)
        {
            
            context = dbContext;
        }
         

            private IRepository<ViewAssignedJob> view_assignedjobsRepository;
            private IRepository<ViewAutoGetGenerateforInvoice> view_autogetgenerateforinvoiceRepository;
            //private IRepository<ViewBenchScheduler> view_benchschedulerRepository; //me
            private IRepository<ViewBusinessDay> view_businessdaysRepository;
            private IRepository<ViewCancelledJob> view_cancelledjobsRepository;
            private IRepository<ViewCollectionsReport> view_collectionsreportRepository;
            private IRepository<ViewConvertedClientOrder> view_convertedclientorderRepository;
            private IRepository<ViewCurrentTrayFileStatusReport> view_currenttrayfilestatusreportRepository;
            private IRepository<ViewCustomerAddresswithLocation> view_customeraddresswithlocationRepository;
            private IRepository<ViewCustomerDetail> view_customerdetailsRepository;
            private IRepository<ViewCustomerStatus> view_customerstatusRepository;
            private IRepository<ViewCustomerVsEmployee> view_customerVsemployeeRepository;
            //Norms
            private IRepository<ViewCustomerNorm> view_CustomerNormsRepository;
            //Norms ends
            private IRepository<ViewCustomerVsProcess> view_customerVsprocessRepository;
            private IRepository<ViewDailyOperationsReport> view_dailyoperationsreportRepository;
            private IRepository<ViewDailyProductionReport> view_dailyproductionreportRepository;
            private IRepository<ViewDailyReport> view_dailyreportRepository;
            private IRepository<ViewDepartmentBasedCustomer> view_departmentbasedcustomersRepository;
            private IRepository<ViewEmployeeDetail> view_employeedetailsRepository;
            private IRepository<ViewEmployeeDetailsfromEmp> view_employeedetailsfromEMPRepository;
            private IRepository<ViewEmployeeDetailsWithAdd> view_employeedetailsWithADDRepository;
            private IRepository<ViewEmployeeDetailsWithAddress> view_employeedetailsWithaddressRepository;
            private IRepository<ViewEmployeeDetailswithShift> view_employeedetailswithshiftRepository;
            private IRepository<ViewEmployeeWiseReport> view_employeeWisereportRepository;
            private IRepository<ViewEmployeeWithShift> view_employeeWithshiftRepository;
            private IRepository<ViewFileInwardExcel> view_fileinwardexcelRepository;
            private IRepository<ViewFileStatusReport> view_filestatusreportRepository;
            private IRepository<ViewFileStatusReportRef> view_filestatusreportRefRepository;
            private IRepository<ViewGeneratedInvoice> view_generatedinvoiceRepository;
            private IRepository<ViewGetCancelledInvoiceList> view_getcancelledinvoicelistRepository;
            private IRepository<ViewGetCheckWaiverJob> view_getcheckwaiverjobsRepository;
            private IRepository<ViewGetConfirmedGeneratedInvoice> view_getconfirmedgeneratedinvoiceRepository;
            private IRepository<ViewGetCustomerforInvoiceCancel> view_getcustomerforinvoicecancelRepository;
            private IRepository<ViewGetCustomerforSelection> view_getcustomerforselectionRepository;
            private IRepository<ViewGetDropInvoiceCancel> view_getdropinvoicecancelRepository;
            //private IRepository<ViewGetDropInvoiceDetailTab> view_getdropinvoicedetailtabRepository;
           /// private IRepository<ViewGetDropInvoiceIntegration> view_getdropinvoiceintegrationRepository;
            private IRepository<ViewGetEmployeesForPendingJob> view_getemployeesForpendingjobsRepository;
            private IRepository<ViewGetGenerateforInvoice> view_getgenerateforinvoiceRepository;
           // private IRepository<ViewGetGeneratingInvoice> view_getgeneratinginvoiceRepository;
           // private IRepository<ViewGetIntegrationMaster> view_getintegrationmasterRepository;
            private IRepository<ViewGetIntegrationTran> view_getintegrationtranRepository;
            private IRepository<ViewGetInvoiceMaster> view_getinvoicemasterRepository;
            private IRepository<ViewGetInvoiceMasterforDayDiff> view_getinvoicemasterfordaydiffRepository;
            private IRepository<ViewGetPricingforInvoice> view_getpricingforinvoiceRepository;
            private IRepository<ViewGetTranMergeInvoiceCancel> view_gettranmergeinvoicecancelRepository;
            private IRepository<ViewGetTransactionTypeCancel> view_gettransactiontypecancelRepository;
            private IRepository<ViewGetTransactionTypeInvoice> view_gettransactiontypeinvoiceRepository;
            private IRepository<ViewGetUser> view_getuserRepository;
            private IRepository<ViewGetWorkflowListforLogin> view_getworkflowlistforloginRepository;
            private IRepository<ViewInvoiceCompletedList> view_invoicecompletedlistRepository;
           // private IRepository<ViewInvoiceMater> view_invoicematerRepository;
            private IRepository<ViewJobAllocation> view_joballocationRepository;
            private IRepository<ViewJobAllocationandJobOrder> view_joballocationandjoborderRepository;
            private IRepository<ViewJobDetail> view_jobdetailsRepository;
            private IRepository<ViewJobForcompletedJob> view_jobForcompletedjobsRepository;
            private IRepository<ViewJobMovement> view_jobmovementRepository;
            private IRepository<ViewJobOrder> view_joborderRepository;
            private IRepository<ViewJobOrderforFileUpload> view_joborderforfileuploadRepository;
            private IRepository<ViewJobQueryInCc> view_jobqueryInCCRepository;
            private IRepository<ViewJobsAllocatedSummary> view_jobsallocatedsummaryRepository;
            private IRepository<ViewMailContact> view_mailcontactRepository;
            private IRepository<ViewOperationsReport> view_operationsReportRepository;
           // private IRepository<ViewOutStandingList> view_outstandinglistRepository;
            private IRepository<ViewQueryTracker> view_querytrackerRepository;
            private IRepository<ViewQuotationDetailsinJobOrder> view_quotationdetailsinjoborderRepository;
            private IRepository<ViewQuotationJobDetail> view_quotationjobdetailsRepository;
            private IRepository<ViewQuotationTracker> view_quotationtrackerRepository;
            private IRepository<ViewShiftVsEmployee> view_shiftVsemployeeRepository;
            private IRepository<ViewShiftVsEmployeeShift> view_shiftVsemployee_shiftRepository;
            private IRepository<ViewTatreport> view_TATreportRepository;
            private IRepository<ViewVoucherControler> view_vouchercontrolerRepository;
            //private IRepository<ViewDetailOfPricing> view_detailofpricingRepository; //balaji
            private IRepository<View_CustomerVsScope> view_customervsscopeRepository;

            public IRepository<ViewAssignedJob> ViewAssignedJobsRepository
            {
                get
                {
                    if (this.view_assignedjobsRepository == null)
                    {
                        this.view_assignedjobsRepository = new Repository<ViewAssignedJob>(context);
                    }
                    return view_assignedjobsRepository;
                }
            }
            public IRepository<ViewAutoGetGenerateforInvoice> ViewAutoGetGenerateforInvoiceRepository
            {
                get
                {
                    if (this.view_autogetgenerateforinvoiceRepository == null)
                    {
                        this.view_autogetgenerateforinvoiceRepository = new Repository<ViewAutoGetGenerateforInvoice>(context);
                    }
                    return view_autogetgenerateforinvoiceRepository;
                }
            }
            //public IRepository<ViewBenchScheduler> ViewBenchSchedulerRepository
            //{
            //    get
            //    {
            //        if (this.view_benchschedulerRepository == null)
            //        {
            //            this.view_benchschedulerRepository = new Repository<ViewBenchScheduler>(context);
            //        }
            //        return view_benchschedulerRepository;
            //    }
            //}
            public IRepository<ViewBusinessDay> ViewBusinessDaysRepository
            {
                get
                {
                    if (this.view_businessdaysRepository == null)
                    {
                        this.view_businessdaysRepository = new Repository<ViewBusinessDay>(context);
                    }
                    return view_businessdaysRepository;
                }
            }
            public IRepository<ViewCancelledJob> ViewCancelledJobsRepository
            {
                get
                {
                    if (this.view_cancelledjobsRepository == null)
                    {
                        this.view_cancelledjobsRepository = new Repository<ViewCancelledJob>(context);
                    }
                    return view_cancelledjobsRepository;
                }
            }
            public IRepository<ViewCollectionsReport> ViewCollectionsReportRepository
            {
                get
                {
                    if (this.view_collectionsreportRepository == null)
                    {
                        this.view_collectionsreportRepository = new Repository<ViewCollectionsReport>(context);
                    }
                    return view_collectionsreportRepository;
                }
            }
            public IRepository<ViewConvertedClientOrder> ViewConvertedClientOrderRepository
            {
                get
                {
                    if (this.view_convertedclientorderRepository == null)
                    {
                        this.view_convertedclientorderRepository = new Repository<ViewConvertedClientOrder>(context);
                    }
                    return view_convertedclientorderRepository;
                }
            }
            public IRepository<ViewCurrentTrayFileStatusReport> ViewCurrentTrayFileStatusReportRepository
            {
                get
                {
                    if (this.view_currenttrayfilestatusreportRepository == null)
                    {
                        this.view_currenttrayfilestatusreportRepository = new Repository<ViewCurrentTrayFileStatusReport>(context);
                    }
                    return view_currenttrayfilestatusreportRepository;
                }
            }
            public IRepository<ViewCustomerAddresswithLocation> ViewCustomerAddresswithLocationRepository
            {
                get
                {
                    if (this.view_customeraddresswithlocationRepository == null)
                    {
                        this.view_customeraddresswithlocationRepository = new Repository<ViewCustomerAddresswithLocation>(context);
                    }
                    return view_customeraddresswithlocationRepository;
                }
            }
            public IRepository<ViewCustomerDetail> ViewCustomerDetailsRepository
            {
                get
                {
                    if (this.view_customerdetailsRepository == null)
                    {
                        this.view_customerdetailsRepository = new Repository<ViewCustomerDetail>(context);
                    }
                    return view_customerdetailsRepository;
                }
            }
            public IRepository<ViewCustomerStatus> ViewCustomerStatusRepository
            {
                get
                {
                    if (this.view_customerstatusRepository == null)
                    {
                        this.view_customerstatusRepository = new Repository<ViewCustomerStatus>(context);
                    }
                    return view_customerstatusRepository;
                }
            }
            public IRepository<ViewCustomerVsEmployee> ViewCustomerVsEmployeeRepository
            {
                get
                {
                    if (this.view_customerVsemployeeRepository == null)
                    {
                        this.view_customerVsemployeeRepository = new Repository<ViewCustomerVsEmployee>(context);
                    }
                    return view_customerVsemployeeRepository;
                }
            }
            //Norms
            public IRepository<ViewCustomerNorm> ViewCustomerNormsRepository
            {
                get
                {
                    if (this.view_CustomerNormsRepository == null)
                    {
                        this.view_CustomerNormsRepository = new Repository<ViewCustomerNorm>(context);
                    }
                    return view_CustomerNormsRepository;
                }
            }

            //Norms ends



            public IRepository<ViewCustomerVsProcess> ViewCustomerVsProcessRepository
            {
                get
                {
                    if (this.view_customerVsprocessRepository == null)
                    {
                        this.view_customerVsprocessRepository = new Repository<ViewCustomerVsProcess>(context);
                    }
                    return view_customerVsprocessRepository;
                }
            }
            public IRepository<ViewDailyOperationsReport> ViewDailyOperationsReportRepository
            {
                get
                {
                    if (this.view_dailyoperationsreportRepository == null)
                    {
                        this.view_dailyoperationsreportRepository = new Repository<ViewDailyOperationsReport>(context);
                    }
                    return view_dailyoperationsreportRepository;
                }
            }
            public IRepository<ViewDailyProductionReport> ViewDailyProductionReportRepository
            {
                get
                {
                    if (this.view_dailyproductionreportRepository == null)
                    {
                        this.view_dailyproductionreportRepository = new Repository<ViewDailyProductionReport>(context);
                    }
                    return view_dailyproductionreportRepository;
                }
            }
            public IRepository<ViewDailyReport> ViewDailyReportRepository
            {
                get
                {
                    if (this.view_dailyreportRepository == null)
                    {
                        this.view_dailyreportRepository = new Repository<ViewDailyReport>(context);
                    }
                    return view_dailyreportRepository;
                }
            }
            public IRepository<ViewDepartmentBasedCustomer> ViewDepartmentBasedCustomersRepository
            {
                get
                {
                    if (this.view_departmentbasedcustomersRepository == null)
                    {
                        this.view_departmentbasedcustomersRepository = new Repository<ViewDepartmentBasedCustomer>(context);
                    }
                    return view_departmentbasedcustomersRepository;
                }
            }
            public IRepository<ViewEmployeeDetail> ViewEmployeeDetailsRepository
            {
                get
                {
                    if (this.view_employeedetailsRepository == null)
                    {
                        this.view_employeedetailsRepository = new Repository<ViewEmployeeDetail>(context);
                    }
                    return view_employeedetailsRepository;
                }
            }
            public IRepository<ViewEmployeeDetailsfromEmp> ViewEmployeeDetailsfromEMPRepository
            {
                get
                {
                    if (this.view_employeedetailsfromEMPRepository == null)
                    {
                        this.view_employeedetailsfromEMPRepository = new Repository<ViewEmployeeDetailsfromEmp>(context);
                    }
                    return view_employeedetailsfromEMPRepository;
                }
            }
            public IRepository<ViewEmployeeDetailsWithAdd> ViewEmployeeDetailsWithADDRepository
            {
                get
                {
                    if (this.view_employeedetailsWithADDRepository == null)
                    {
                        this.view_employeedetailsWithADDRepository = new Repository<ViewEmployeeDetailsWithAdd>(context);
                    }
                    return view_employeedetailsWithADDRepository;
                }
            }
            public IRepository<ViewEmployeeDetailsWithAddress> ViewEmployeeDetailsWithAddressRepository
            {
                get
                {
                    if (this.view_employeedetailsWithaddressRepository == null)
                    {
                        this.view_employeedetailsWithaddressRepository = new Repository<ViewEmployeeDetailsWithAddress>(context);
                    }
                    return view_employeedetailsWithaddressRepository;
                }
            }
            public IRepository<ViewEmployeeDetailswithShift> ViewEmployeeDetailswithShiftRepository
            {
                get
                {
                    if (this.view_employeedetailswithshiftRepository == null)
                    {
                        this.view_employeedetailswithshiftRepository = new Repository<ViewEmployeeDetailswithShift>(context);
                    }
                    return view_employeedetailswithshiftRepository;
                }
            }
            public IRepository<ViewEmployeeWiseReport> ViewEmployeeWiseReportRepository
            {
                get
                {
                    if (this.view_employeeWisereportRepository == null)
                    {
                        this.view_employeeWisereportRepository = new Repository<ViewEmployeeWiseReport>(context);
                    }
                    return view_employeeWisereportRepository;
                }
            }
            public IRepository<ViewEmployeeWithShift> ViewEmployeeWithShiftRepository
            {
                get
                {
                    if (this.view_employeeWithshiftRepository == null)
                    {
                        this.view_employeeWithshiftRepository = new Repository<ViewEmployeeWithShift>(context);
                    }
                    return view_employeeWithshiftRepository;
                }
            }
            public IRepository<ViewFileInwardExcel> ViewFileInwardExcelRepository
            {
                get
                {
                    if (this.view_fileinwardexcelRepository == null)
                    {
                        this.view_fileinwardexcelRepository = new Repository<ViewFileInwardExcel>(context);
                    }
                    return view_fileinwardexcelRepository;
                }
            }
            public IRepository<ViewFileStatusReport> ViewFileStatusReportRepository
            {
                get
                {
                    if (this.view_filestatusreportRepository == null)
                    {
                        this.view_filestatusreportRepository = new Repository<ViewFileStatusReport>(context);
                    }
                    return view_filestatusreportRepository;
                }
            }
            public IRepository<ViewFileStatusReportRef> ViewFileStatusReportRefRepository
            {
                get
                {
                    if (this.view_filestatusreportRefRepository == null)
                    {
                        this.view_filestatusreportRefRepository = new Repository<ViewFileStatusReportRef>(context);
                    }
                    return view_filestatusreportRefRepository;
                }
            }
            public IRepository<ViewGeneratedInvoice> ViewGeneratedInvoiceRepository
            {
                get
                {
                    if (this.view_generatedinvoiceRepository == null)
                    {
                        this.view_generatedinvoiceRepository = new Repository<ViewGeneratedInvoice>(context);
                    }
                    return view_generatedinvoiceRepository;
                }
            }
            public IRepository<ViewGetCancelledInvoiceList> ViewGetCancelledInvoiceListRepository
            {
                get
                {
                    if (this.view_getcancelledinvoicelistRepository == null)
                    {
                        this.view_getcancelledinvoicelistRepository = new Repository<ViewGetCancelledInvoiceList>(context);
                    }
                    return view_getcancelledinvoicelistRepository;
                }
            }
            public IRepository<ViewGetCheckWaiverJob> ViewGetCheckWaiverJobsRepository
            {
                get
                {
                    if (this.view_getcheckwaiverjobsRepository == null)
                    {
                        this.view_getcheckwaiverjobsRepository = new Repository<ViewGetCheckWaiverJob>(context);
                    }
                    return view_getcheckwaiverjobsRepository;
                }
            }
            public IRepository<ViewGetConfirmedGeneratedInvoice> ViewGetConfirmedGeneratedInvoiceRepository
            {
                get
                {
                    if (this.view_getconfirmedgeneratedinvoiceRepository == null)
                    {
                        this.view_getconfirmedgeneratedinvoiceRepository = new Repository<ViewGetConfirmedGeneratedInvoice>(context);
                    }
                    return view_getconfirmedgeneratedinvoiceRepository;
                }
            }
            public IRepository<ViewGetCustomerforInvoiceCancel> ViewGetCustomerforInvoiceCancelRepository
            {
                get
                {
                    if (this.view_getcustomerforinvoicecancelRepository == null)
                    {
                        this.view_getcustomerforinvoicecancelRepository = new Repository<ViewGetCustomerforInvoiceCancel>(context);
                    }
                    return view_getcustomerforinvoicecancelRepository;
                }
            }
            public IRepository<ViewGetCustomerforSelection> ViewGetCustomerforSelectionRepository
            {
                get
                {
                    if (this.view_getcustomerforselectionRepository == null)
                    {
                        this.view_getcustomerforselectionRepository = new Repository<ViewGetCustomerforSelection>(context);
                    }
                    return view_getcustomerforselectionRepository;
                }
            }
            public IRepository<ViewGetDropInvoiceCancel> ViewGetDropInvoiceCancelRepository
            {
                get
                {
                    if (this.view_getdropinvoicecancelRepository == null)
                    {
                        this.view_getdropinvoicecancelRepository = new Repository<ViewGetDropInvoiceCancel>(context);
                    }
                    return view_getdropinvoicecancelRepository;
                }
            }
            //public IRepository<ViewGetDropInvoiceDetailTab> ViewGetDropInvoiceDetailTabRepository
            //{
            //    get
            //    {
            //        if (this.view_getdropinvoicedetailtabRepository == null)
            //        {
            //            this.view_getdropinvoicedetailtabRepository = new Repository<ViewGetDropInvoiceDetailTab>(context);
            //        }
            //        return view_getdropinvoicedetailtabRepository;
            //    }
            //}
            //public IRepository<ViewGetDropInvoiceIntegration> ViewGetDropInvoiceIntegrationRepository
            //{
            //    get
            //    {
            //        if (this.view_getdropinvoiceintegrationRepository == null)
            //        {
            //            this.view_getdropinvoiceintegrationRepository = new Repository<ViewGetDropInvoiceIntegration>(context);
            //        }
            //        return view_getdropinvoiceintegrationRepository;
            //    }
            //}
            public IRepository<ViewGetEmployeesForPendingJob> ViewGetEmployeesForPendingJobsRepository
            {
                get
                {
                    if (this.view_getemployeesForpendingjobsRepository == null)
                    {
                        this.view_getemployeesForpendingjobsRepository = new Repository<ViewGetEmployeesForPendingJob>(context);
                    }
                    return view_getemployeesForpendingjobsRepository;
                }
            }
            public IRepository<ViewGetGenerateforInvoice> ViewGetGenerateforInvoiceRepository
            {
                get
                {
                    if (this.view_getgenerateforinvoiceRepository == null)
                    {
                        this.view_getgenerateforinvoiceRepository = new Repository<ViewGetGenerateforInvoice>(context);
                    }
                    return view_getgenerateforinvoiceRepository;
                }
            }
            //public IRepository<ViewGetGeneratingInvoice> ViewGetGeneratingInvoiceRepository
            //{
            //    get
            //    {
            //        if (this.view_getgeneratinginvoiceRepository == null)
            //        {
            //            this.view_getgeneratinginvoiceRepository = new Repository<ViewGetGeneratingInvoice>(context);
            //        }
            //        return view_getgeneratinginvoiceRepository;
            //    }
            //}
            //public IRepository<ViewGetIntegrationMaster> ViewGetIntegrationMasterRepository
            //{
            //    get
            //    {
            //        if (this.view_getintegrationmasterRepository == null)
            //        {
            //            this.view_getintegrationmasterRepository = new Repository<ViewGetIntegrationMaster>(context);
            //        }
            //        return view_getintegrationmasterRepository;
            //    }
            //}
            public IRepository<ViewGetIntegrationTran> ViewGetIntegrationTranRepository
            {
                get
                {
                    if (this.view_getintegrationtranRepository == null)
                    {
                        this.view_getintegrationtranRepository = new Repository<ViewGetIntegrationTran>(context);
                    }
                    return view_getintegrationtranRepository;
                }
            }
            public IRepository<ViewGetInvoiceMaster> ViewGetInvoiceMasterRepository
            {
                get
                {
                    if (this.view_getinvoicemasterRepository == null)
                    {
                        this.view_getinvoicemasterRepository = new Repository<ViewGetInvoiceMaster>(context);
                    }
                    return view_getinvoicemasterRepository;
                }
            }
            public IRepository<ViewGetInvoiceMasterforDayDiff> ViewGetInvoiceMasterforDayDiffRepository
            {
                get
                {
                    if (this.view_getinvoicemasterfordaydiffRepository == null)
                    {
                        this.view_getinvoicemasterfordaydiffRepository = new Repository<ViewGetInvoiceMasterforDayDiff>(context);
                    }
                    return view_getinvoicemasterfordaydiffRepository;
                }
            }
            public IRepository<ViewGetPricingforInvoice> ViewGetPricingforInvoiceRepository
            {
                get
                {
                    if (this.view_getpricingforinvoiceRepository == null)
                    {
                        this.view_getpricingforinvoiceRepository = new Repository<ViewGetPricingforInvoice>(context);
                    }
                    return view_getpricingforinvoiceRepository;
                }
            }
            public IRepository<ViewGetTranMergeInvoiceCancel> ViewGetTranMergeInvoiceCancelRepository
            {
                get
                {
                    if (this.view_gettranmergeinvoicecancelRepository == null)
                    {
                        this.view_gettranmergeinvoicecancelRepository = new Repository<ViewGetTranMergeInvoiceCancel>(context);
                    }
                    return view_gettranmergeinvoicecancelRepository;
                }
            }
            public IRepository<ViewGetTransactionTypeCancel> ViewGetTransactionTypeCancelRepository
            {
                get
                {
                    if (this.view_gettransactiontypecancelRepository == null)
                    {
                        this.view_gettransactiontypecancelRepository = new Repository<ViewGetTransactionTypeCancel>(context);
                    }
                    return view_gettransactiontypecancelRepository;
                }
            }
            public IRepository<ViewGetTransactionTypeInvoice> ViewGetTransactionTypeInvoiceRepository
            {
                get
                {
                    if (this.view_gettransactiontypeinvoiceRepository == null)
                    {
                        this.view_gettransactiontypeinvoiceRepository = new Repository<ViewGetTransactionTypeInvoice>(context);
                    }
                    return view_gettransactiontypeinvoiceRepository;
                }
            }
            public IRepository<ViewGetUser> ViewGetUserRepository
            {
                get
                {
                    if (this.view_getuserRepository == null)
                    {
                        this.view_getuserRepository = new Repository<ViewGetUser>(context);
                    }
                    return view_getuserRepository;
                }
            }
            public IRepository<ViewGetWorkflowListforLogin> ViewGetWorkflowListforLoginRepository
            {
                get
                {
                    if (this.view_getworkflowlistforloginRepository == null)
                    {
                        this.view_getworkflowlistforloginRepository = new Repository<ViewGetWorkflowListforLogin>(context);
                    }
                    return view_getworkflowlistforloginRepository;
                }
            }
            public IRepository<ViewInvoiceCompletedList> ViewInvoiceCompletedListRepository
            {
                get
                {
                    if (this.view_invoicecompletedlistRepository == null)
                    {
                        this.view_invoicecompletedlistRepository = new Repository<ViewInvoiceCompletedList>(context);
                    }
                    return view_invoicecompletedlistRepository;
                }
            }
            //public IRepository<ViewInvoiceMater> ViewInvoiceMaterRepository
            //{
            //    get
            //    {
            //        if (this.view_invoicematerRepository == null)
            //        {
            //            this.view_invoicematerRepository = new Repository<ViewInvoiceMater>(context);
            //        }
            //        return view_invoicematerRepository;
            //    }
            //}
            public IRepository<ViewJobAllocation> ViewJobAllocationRepository
            {
                get
                {
                    if (this.view_joballocationRepository == null)
                    {
                        this.view_joballocationRepository = new Repository<ViewJobAllocation>(context);
                    }
                    return view_joballocationRepository;
                }
            }
            public IRepository<ViewJobAllocationandJobOrder> ViewJobAllocationandJobOrderRepository
            {
                get
                {
                    if (this.view_joballocationandjoborderRepository == null)
                    {
                        this.view_joballocationandjoborderRepository = new Repository<ViewJobAllocationandJobOrder>(context);
                    }
                    return view_joballocationandjoborderRepository;
                }
            }
            public IRepository<ViewJobDetail> ViewJobDetailsRepository
            {
                get
                {
                    if (this.view_jobdetailsRepository == null)
                    {
                        this.view_jobdetailsRepository = new Repository<ViewJobDetail>(context);
                    }
                    return view_jobdetailsRepository;
                }
            }
            public IRepository<ViewJobForcompletedJob> ViewJobForcompletedJobsRepository
            {
                get
                {
                    if (this.view_jobForcompletedjobsRepository == null)
                    {
                        this.view_jobForcompletedjobsRepository = new Repository<ViewJobForcompletedJob>(context);
                    }
                    return view_jobForcompletedjobsRepository;
                }
            }
            public IRepository<ViewJobMovement> ViewJobMovementRepository
            {
                get
                {
                    if (this.view_jobmovementRepository == null)
                    {
                        this.view_jobmovementRepository = new Repository<ViewJobMovement>(context);
                    }
                    return view_jobmovementRepository;
                }
            }
            public IRepository<ViewJobOrder> ViewJobOrderRepository
            {
                get
                {
                    if (this.view_joborderRepository == null)
                    {
                        this.view_joborderRepository = new Repository<ViewJobOrder>(context);
                    }
                    return view_joborderRepository;
                }
            }
            public IRepository<ViewJobOrderforFileUpload> ViewJobOrderforFileUploadRepository
            {
                get
                {
                    if (this.view_joborderforfileuploadRepository == null)
                    {
                        this.view_joborderforfileuploadRepository = new Repository<ViewJobOrderforFileUpload>(context);
                    }
                    return view_joborderforfileuploadRepository;
                }
            }
            public IRepository<ViewJobQueryInCc> ViewJobQueryInCCRepository
            {
                get
                {
                    if (this.view_jobqueryInCCRepository == null)
                    {
                        this.view_jobqueryInCCRepository = new Repository<ViewJobQueryInCc>(context);
                    }
                    return view_jobqueryInCCRepository;
                }
            }
            public IRepository<ViewJobsAllocatedSummary> ViewJobsAllocatedSummaryRepository
            {
                get
                {
                    if (this.view_jobsallocatedsummaryRepository == null)
                    {
                        this.view_jobsallocatedsummaryRepository = new Repository<ViewJobsAllocatedSummary>(context);
                    }
                    return view_jobsallocatedsummaryRepository;
                }
            }
        public IRepository<ViewMailContact> view_MailContactRepository
        {
            get
            {
                if (this.view_mailcontactRepository == null)
                {
                    this.view_mailcontactRepository = new Repository<ViewMailContact>(context);
                }
                return view_mailcontactRepository;
            }
        }
        public IRepository<ViewOperationsReport> ViewOperationsReportRepository
            {
                get
                {
                    if (this.view_operationsReportRepository == null)
                    {
                        this.view_operationsReportRepository = new Repository<ViewOperationsReport>(context);
                    }
                    return view_operationsReportRepository;
                }
            }
            //public IRepository<ViewOutStandingList> ViewOutStandingListRepository
            //{
            //    get
            //    {
            //        if (this.view_outstandinglistRepository == null)
            //        {
            //            this.view_outstandinglistRepository = new Repository<ViewOutStandingList>(context);
            //        }
            //        return view_outstandinglistRepository;
            //    }
            //}
            public IRepository<ViewQueryTracker> ViewQueryTrackerRepository
            {
                get
                {
                    if (this.view_querytrackerRepository == null)
                    {
                        this.view_querytrackerRepository = new Repository<ViewQueryTracker>(context);
                    }
                    return view_querytrackerRepository;
                }
            }
            public IRepository<ViewQuotationDetailsinJobOrder> ViewQuotationDetailsinJobOrderRepository
            {
                get
                {
                    if (this.view_quotationdetailsinjoborderRepository == null)
                    {
                        this.view_quotationdetailsinjoborderRepository = new Repository<ViewQuotationDetailsinJobOrder>(context);
                    }
                    return view_quotationdetailsinjoborderRepository;
                }
            }
            public IRepository<ViewQuotationJobDetail> ViewQuotationJobDetailsRepository
            {
                get
                {
                    if (this.view_quotationjobdetailsRepository == null)
                    {
                        this.view_quotationjobdetailsRepository = new Repository<ViewQuotationJobDetail>(context);
                    }
                    return view_quotationjobdetailsRepository;
                }
            }
            public IRepository<ViewQuotationTracker> ViewQuotationTrackerRepository
            {
                get
                {
                    if (this.view_quotationtrackerRepository == null)
                    {
                        this.view_quotationtrackerRepository = new Repository<ViewQuotationTracker>(context);
                    }
                    return view_quotationtrackerRepository;
                }
            }
            public IRepository<ViewShiftVsEmployee> ViewShiftVsEmployeeRepository
            {
                get
                {
                    if (this.view_shiftVsemployeeRepository == null)
                    {
                        this.view_shiftVsemployeeRepository = new Repository<ViewShiftVsEmployee>(context);
                    }
                    return view_shiftVsemployeeRepository;
                }
            }
            public IRepository<ViewShiftVsEmployeeShift> ViewShiftVsEmployee_ShiftRepository
            {
                get
                {
                    if (this.view_shiftVsemployee_shiftRepository == null)
                    {
                        this.view_shiftVsemployee_shiftRepository = new Repository<ViewShiftVsEmployeeShift>(context);
                    }
                    return view_shiftVsemployee_shiftRepository;
                }
            }
            public IRepository<ViewTatreport> ViewTATReportRepository
            {
                get
                {
                    if (this.view_TATreportRepository == null)
                    {
                        this.view_TATreportRepository = new Repository<ViewTatreport>(context);
                    }
                    return view_TATreportRepository;
                }
            }
            public IRepository<ViewVoucherControler> ViewVoucherControlerRepository
            {
                get
                {
                    if (this.view_vouchercontrolerRepository == null)
                    {
                        this.view_vouchercontrolerRepository = new Repository<ViewVoucherControler>(context);
                    }
                    return view_vouchercontrolerRepository;
                }
            }
        //public IRepository<ViewDetailOfPricing> ViewDetailOfPricingRepository
        //{
        //    get
        //    {
        //        if (this.view_detailofpricingRepository == null)
        //        {
        //            this.view_detailofpricingRepository = new Repository<ViewDetailOfPricing>(context);
        //        }
        //        return view_detailofpricingRepository;
        //    }
        //}
        public IRepository<View_CustomerVsScope> ViewCustomerVsScopeRepository
        {
            get
            {
                if (this.view_customervsscopeRepository == null)
                {
                    this.view_customervsscopeRepository = new Repository<View_CustomerVsScope>(context);
                }
                return view_customervsscopeRepository;
            }
        }


        public void Save()
            {
                context.SaveChanges();
            }

            private bool disposed = false;

            public virtual void Dispose(bool disposing)
            {
                if (!this.disposed)
                {
                    if (disposing)
                    {
                        context.Dispose();
                    }
                }
                this.disposed = true;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            //public void lazy()
            //{
            //    context.Configuration.LazyLoadingEnabled = true;
            //}

            public void Close()
            {
                context.Dispose();
            }
        }
    }



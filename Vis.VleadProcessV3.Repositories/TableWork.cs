using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Models;


namespace Vis.VleadProcessV3.Repositories
{

    public class TableWork : IDisposable
    {
        private readonly ApplicationDbContext context;
        public TableWork(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

            private IRepository<BankDetail> bankdetailRepository;
            private IRepository<BenchScheduler> benchschedulerRepository;
            private IRepository<BenchStatus> benchstatusRepository;
            private IRepository<BillingCycle> billingcycleRepository;
            private IRepository<BillingCycleMonthly> billingcyclemonthlyRepository;
            private IRepository<EmailContactNotification> emailContactNotificationRepository;
            private IRepository<CapacityForm> capacityformRepository;
            private IRepository<City1> cityRepository;
            private IRepository<ClientOrder> clientorderRepository;
            private IRepository<ClientOrderExt> clientorderExtRepository;
            private IRepository<Company> companyRepository;
            private IRepository<Country> countryRepository;
            private IRepository<Customer> customerRepository;
            private IRepository<CustomerClassification> customerclassificationRepository;
            private IRepository<CustomerCommunication> customercommunicationRepository;
            private IRepository<CustomerContact> customercontactRepository;
            private IRepository<CustomerDepartment> customerdepartmentRepository;
            private IRepository<CustomerExt> customerExtRepository;
            private IRepository<CustomerRegistration> customerregistrationRepository;
            //private IRepository<CustomerTAT> customerTATRepository;
            private IRepository<CustomerVsEmployee> customerVsemployeeRepository;
            private IRepository<CustomersNorm> customersNormsRepository;//Norms
            private IRepository<CustomerVsSalesEmployee> customerVssalesemployeeRepository;
            private IRepository<CustomerVsScope> customerVsscopeRepository;
            private IRepository<Department> departmentRepository;
            private IRepository<Designation> designationRepository;
            private IRepository<Employee> employeeRepository;
            private IRepository<EmployeeAssign> employeeassignRepository;
            private IRepository<EmployeeCommunication> employeecommunicationRepository;
            private IRepository<EmployeeHierarchy> employeehierarchyRepository;
            private IRepository<EmployeeProcess> employeeprocessRepository;
            private IRepository<EmployeeVsRole> employeeVsroleRepository;
            private IRepository<ErrorCategory> errorcategoryRepository;
            private IRepository<FileAttachment> fileattachmentRepository;
            private IRepository<FileInwardExcel> fileinwardexcelRepository;
            private IRepository<AttendanceFileInwardExcel> attendanceFileInwardExcelRepository;//Attendance
            private IRepository<FileInwardType> fileinwardtypeRepository;
            private IRepository<FileProcessTray> fileprocesstrayRepository;
            private IRepository<FileSummary> filesummaryRepository;
            private IRepository<FiscalBillingCycle> fiscalbillingcycleRepository;
            private IRepository<IntegrationMaster> integrationmasterRepository;
            private IRepository<IntegrationTran> integrationtranRepository;
            private IRepository<Inventory> inventoryFormSubmitRepository;
            private IRepository<InvoiceJob> invoicejobRepository;
            private IRepository<InvoiceMaster> invoicemasterRepository;
            private IRepository<InvoiceTran> invoicetranRepository;
            private IRepository<JobApprovalExt> jobapprovalExtRepository;
            private IRepository<JobCategory> jobcategoryRepository;
            private IRepository<JobFilesLocationMaster> jobfileslocationmasterRepository;
            private IRepository<JobOrder> joborderRepository;
            private IRepository<JobOrderExtension> joborderextensionRepository;
            private IRepository<JobOrderFile> joborderfileRepository;
            private IRepository<JobQuery> jobqueryRepository;
            private IRepository<JobQuotation> jobquotationRepository;
            private IRepository<JobStatus> jobstatusRepository;
            private IRepository<JobStatusExt> jobstatusExtRepository;
            private IRepository<JobSummary> jobsummaryRepository;
            private IRepository<Location> locationRepository;
            private IRepository<Log> logRepository;
            private IRepository<MailContact> mailcontactRepository;
           // private IRepository<MailMaster> mailmasterRepository;
            private IRepository<MailNotificationDetail> mailnotificationdetailRepository;
            private IRepository<Menu> menuRepository;
            private IRepository<News> newsRepository;
            //private IRepository<Podio> podioRepository;
            //private IRepository<PodioMapping> podiomappingRepository;
            private IRepository<Pricing> pricingRepository;
            private IRepository<PricingFileRange> pricingfilerangeRepository;
            private IRepository<PricingScope> pricingscopeRepository;
            private IRepository<PricingStaffing> pricingstaffingRepository;
            private IRepository<PricingType> pricingtypeRepository;
            private IRepository<PricingWithScope> pricingwithscopeRepository;
            private IRepository<Process> processRepository;
            private IRepository<ProcessWorkFlow> processworkflowRepository;
            private IRepository<ProcessWorkFlowMaster> processworkflowmasterRepository;
            private IRepository<ProcessWorkFlowStatus> processworkflowstatusRepository;
            private IRepository<ProcessWorkFlowTran> processworkflowtranRepository;
            private IRepository<Proficiency> proficiencyRepository;
            private IRepository<Receivable> receivableRepository;
            private IRepository<ReceivableAdjustment> receivableadjustmentRepository;
            private IRepository<ReceivableExt> receivableExtRepository;
            private IRepository<RemovalReason> removalreasonRepository;
            private IRepository<ResignReason> resignreasonRepository;
            private IRepository<Role> roleRepository;
            private IRepository<Scope> scopeRepository;
            private IRepository<Shift> shiftRepository;
            private IRepository<ShiftVsEmployee> shiftVsemployeeRepository;
            private IRepository<StaffingPrice> staffingpriceRepository;
            private IRepository<State> stateRepository;
            private IRepository<Status> statusRepository;
            private IRepository<Vis.VleadProcessV3.Models.TimeZone> timezoneRepository;
            private IRepository<TransactionType> transactiontypeRepository;
            private IRepository<TrayDetailMovement> traydetailmovementRepository;
            private IRepository<TrayDetailMovementTran> traydetailmovementtranRepository;
            private IRepository<User> userRepository;
            private IRepository<VoucherControl> vouchercontrolRepository;
            private IRepository<Holiday> holidayRepository;
            private IRepository<Division> divisionRepository;
            private IRepository<CustomerVsDivision> customerVsdivisionRepository;
            private IRepository<EmployeeVsDivision> employeeVsdivisionRepository;
            private IRepository<Checklist> checklistRepository;//------------------------------------------------final CL------------------------------
            private IRepository<TblItassetHardware> Tbl_ITAssetHardwareRepository;//----------------------slowness bugs fixed-----------------------------
            private IRepository<TblItassetSoftware> Tbl_ITAssetSoftwareRepository;//----------------------slowness bugs fixed-----------------------------
            private IRepository<Skillset> skillsetRepository;//-----------------------------------------ESs---------------------
            private IRepository<EmployeeVsSkillset> employeeskillsetRepository;//---------------------ESs----------------------
        private IRepository<EmployeeCoreDivision> employeecoredivisionRepository;

        public IRepository<BankDetail> BankDetailRepository
            {
                get
                {
                    if (this.bankdetailRepository == null)
                    {
                        this.bankdetailRepository = new Repository<BankDetail>(context);
                    }
                    return bankdetailRepository;
                }
            }
            public IRepository<BenchScheduler> BenchSchedulerRepository
            {
                get
                {
                    if (this.benchschedulerRepository == null)
                    {
                        this.benchschedulerRepository = new Repository<BenchScheduler>(context);
                    }
                    return benchschedulerRepository;
                }
            }
            public IRepository<BenchStatus> BenchStatusRepository
            {
                get
                {
                    if (this.benchstatusRepository == null)
                    {
                        this.benchstatusRepository = new Repository<BenchStatus>(context);
                    }
                    return benchstatusRepository;
                }
            }
            public IRepository<BillingCycle> BillingCycleRepository
            {
                get
                {
                    if (this.billingcycleRepository == null)
                    {
                        this.billingcycleRepository = new Repository<BillingCycle>(context);
                    }
                    return billingcycleRepository;
                }
            }
            public IRepository<BillingCycleMonthly> BillingCycleMonthlyRepository
            {
                get
                {
                    if (this.billingcyclemonthlyRepository == null)
                    {
                        this.billingcyclemonthlyRepository = new Repository<BillingCycleMonthly>(context);
                    }
                    return billingcyclemonthlyRepository;
                }

            }

        public IRepository<EmailContactNotification> EmailContactNotificationRepository
        {
            get
            {
                if (this.emailContactNotificationRepository == null)
                {
                    this.emailContactNotificationRepository = new Repository<EmailContactNotification>(context);
                }
                return emailContactNotificationRepository;

            }

        }
        public IRepository<City1> CityRepository
            {
                get
                {
                    if (this.cityRepository == null)
                    {
                        this.cityRepository = new Repository<City1>(context);
                    }
                    return cityRepository;
                }
            }
            public IRepository<ClientOrder> ClientOrderRepository
            {
                get
                {
                    if (this.clientorderRepository == null)
                    {
                        this.clientorderRepository = new Repository<ClientOrder>(context);
                    }
                    return clientorderRepository;
                }
            }
            public IRepository<ClientOrderExt> ClientOrderExtRepository
            {
                get
                {
                    if (this.clientorderExtRepository == null)
                    {
                        this.clientorderExtRepository = new Repository<ClientOrderExt>(context);
                    }
                    return clientorderExtRepository;
                }
            }
            public IRepository<Company> CompanyRepository
            {
                get
                {
                    if (this.companyRepository == null)
                    {
                        this.companyRepository = new Repository<Company>(context);
                    }
                    return companyRepository;
                }
            }
            public IRepository<Country> CountryRepository
            {
                get
                {
                    if (this.countryRepository == null)
                    {
                        this.countryRepository = new Repository<Country>(context);
                    }
                    return countryRepository;
                }
            }
            public IRepository<Customer> CustomerRepository
            {
                get
                {
                    if (this.customerRepository == null)
                    {
                        this.customerRepository = new Repository<Customer>(context);
                    }
                    return customerRepository;
                }
            }
            public IRepository<CustomerClassification> CustomerClassificationRepository
            {
                get
                {
                    if (this.customerclassificationRepository == null)
                    {
                        this.customerclassificationRepository = new Repository<CustomerClassification>(context);
                    }
                    return customerclassificationRepository;
                }
            }
            public IRepository<CustomerCommunication> CustomerCommunicationRepository
            {
                get
                {
                    if (this.customercommunicationRepository == null)
                    {
                        this.customercommunicationRepository = new Repository<CustomerCommunication>(context);
                    }
                    return customercommunicationRepository;
                }
            }
            public IRepository<CustomerContact> CustomerContactRepository
            {
                get
                {
                    if (this.customercontactRepository == null)
                    {
                        this.customercontactRepository = new Repository<CustomerContact>(context);
                    }
                    return customercontactRepository;
                }
            }
            public IRepository<CustomerDepartment> CustomerDepartmentRepository
            {
                get
                {
                    if (this.customerdepartmentRepository == null)
                    {
                        this.customerdepartmentRepository = new Repository<CustomerDepartment>(context);
                    }
                    return customerdepartmentRepository;
                }
            }
            public IRepository<CustomerExt> CustomerExtRepository
            {
                get
                {
                    if (this.customerExtRepository == null)
                    {
                        this.customerExtRepository = new Repository<CustomerExt>(context);
                    }
                    return customerExtRepository;
                }
            }
            public IRepository<CapacityForm> CapacityFormRepository
            {
                get
                {
                    if (this.capacityformRepository == null)
                    {
                        this.capacityformRepository = new Repository<CapacityForm>(context);
                    }
                    return capacityformRepository;
                }
            }
        public IRepository<CustomerRegistration> CustomerRegistrationRepository
            {
                get
                {
                    if (this.customerregistrationRepository == null)
                    {
                        this.customerregistrationRepository = new Repository<CustomerRegistration>(context);
                    }
                    return customerregistrationRepository;
                }
            }
            //public IRepository<CustomerTAT> CustomerTATRepository
            //{
            //    get
            //    {
            //        if (this.customerTATRepository == null)
            //        {
            //            this.customerTATRepository = new Repository<CustomerTAT>(context);
            //        }
            //        return customerTATRepository;
            //    }
            //}
            public IRepository<CustomerVsEmployee> CustomerVsEmployeeRepository
            {
                get
                {
                    if (this.customerVsemployeeRepository == null)
                    {
                        this.customerVsemployeeRepository = new Repository<CustomerVsEmployee>(context);
                    }
                    return customerVsemployeeRepository;
                }
            }

            //Norms

            public IRepository<CustomersNorm> CustomersNormsRepository
            {
                get
                {
                    if (this.customersNormsRepository == null)
                    {
                        this.customersNormsRepository = new Repository<CustomersNorm>(context);
                    }
                    return customersNormsRepository;
                }
            }
            //Norms
            public IRepository<CustomerVsSalesEmployee> CustomerVsSalesEmployeeRepository
            {
                get
                {
                    if (this.customerVssalesemployeeRepository == null)
                    {
                        this.customerVssalesemployeeRepository = new Repository<CustomerVsSalesEmployee>(context);
                    }
                    return customerVssalesemployeeRepository;
                }
            }
            public IRepository<CustomerVsScope> CustomerVsScopeRepository
            {
                get
                {
                    if (this.customerVsscopeRepository == null)
                    {
                        this.customerVsscopeRepository = new Repository<CustomerVsScope>(context);
                    }
                    return customerVsscopeRepository;
                }
            }
            public IRepository<Department> DepartmentRepository
            {
                get
                {
                    if (this.departmentRepository == null)
                    {
                        this.departmentRepository = new Repository<Department>(context);
                    }
                    return departmentRepository;
                }
            }
            public IRepository<Designation> DesignationRepository
            {
                get
                {
                    if (this.designationRepository == null)
                    {
                        this.designationRepository = new Repository<Designation>(context);
                    }
                    return designationRepository;
                }
            }
            public IRepository<Employee> EmployeeRepository
            {
                get
                {
                    if (this.employeeRepository == null)
                    {
                        this.employeeRepository = new Repository<Employee>(context);
                    }
                    return employeeRepository;
                }
            }
            public IRepository<EmployeeAssign> EmployeeAssignRepository
            {
                get
                {
                    if (this.employeeassignRepository == null)
                    {
                        this.employeeassignRepository = new Repository<EmployeeAssign>(context);
                    }
                    return employeeassignRepository;
                }
            }
            public IRepository<EmployeeCommunication> EmployeeCommunicationRepository
            {
                get
                {
                    if (this.employeecommunicationRepository == null)
                    {
                        this.employeecommunicationRepository = new Repository<EmployeeCommunication>(context);
                    }
                    return employeecommunicationRepository;
                }
            }
        public IRepository<EmployeeCoreDivision> EmployeeCoreDivisionRepository
        {
            get
            {
                if (this.employeecoredivisionRepository == null)
                {
                    this.employeecoredivisionRepository = new Repository<EmployeeCoreDivision>(context);
                }
                return employeecoredivisionRepository;
            }
        }
        public IRepository<EmployeeHierarchy> EmployeeHierarchyRepository
            {
                get
                {
                    if (this.employeehierarchyRepository == null)
                    {
                        this.employeehierarchyRepository = new Repository<EmployeeHierarchy>(context);
                    }
                    return employeehierarchyRepository;
                }
            }
            public IRepository<EmployeeProcess> EmployeeProcessRepository
            {
                get
                {
                    if (this.employeeprocessRepository == null)
                    {
                        this.employeeprocessRepository = new Repository<EmployeeProcess>(context);
                    }
                    return employeeprocessRepository;
                }
            }
            public IRepository<EmployeeVsRole> EmployeeVsRoleRepository
            {
                get
                {
                    if (this.employeeVsroleRepository == null)
                    {
                        this.employeeVsroleRepository = new Repository<EmployeeVsRole>(context);
                    }
                    return employeeVsroleRepository;
                }
            }
            public IRepository<ErrorCategory> ErrorCategoryRepository
            {
                get
                {
                    if (this.errorcategoryRepository == null)
                    {
                        this.errorcategoryRepository = new Repository<ErrorCategory>(context);
                    }
                    return errorcategoryRepository;
                }
            }
            public IRepository<FileAttachment> FileAttachmentRepository
            {
                get
                {
                    if (this.fileattachmentRepository == null)
                    {
                        this.fileattachmentRepository = new Repository<FileAttachment>(context);
                    }
                    return fileattachmentRepository;
                }
            }
            public IRepository<FileInwardExcel> FileInwardExcelRepository
            {
                get
                {
                    if (this.fileinwardexcelRepository == null)
                    {
                        this.fileinwardexcelRepository = new Repository<FileInwardExcel>(context);
                    }
                    return fileinwardexcelRepository;
                }
            }
            //Attendance

            public IRepository<AttendanceFileInwardExcel> AttendanceFileInwardExcelRepository
            {
                get
                {
                    if (this.attendanceFileInwardExcelRepository == null)
                    {
                        this.attendanceFileInwardExcelRepository = new Repository<AttendanceFileInwardExcel>(context);
                    }
                    return attendanceFileInwardExcelRepository;
                }
            }

            //Attendance end
            public IRepository<FileInwardType> FileInwardTypeRepository
            {
                get
                {
                    if (this.fileinwardtypeRepository == null)
                    {
                        this.fileinwardtypeRepository = new Repository<FileInwardType>(context);
                    }
                    return fileinwardtypeRepository;
                }
            }
            public IRepository<FileProcessTray> FileProcessTrayRepository
            {
                get
                {
                    if (this.fileprocesstrayRepository == null)
                    {
                        this.fileprocesstrayRepository = new Repository<FileProcessTray>(context);
                    }
                    return fileprocesstrayRepository;
                }
            }
            public IRepository<FileSummary> FileSummaryRepository
            {
                get
                {
                    if (this.filesummaryRepository == null)
                    {
                        this.filesummaryRepository = new Repository<FileSummary>(context);
                    }
                    return filesummaryRepository;
                }
            }
            public IRepository<FiscalBillingCycle> FiscalBillingCycleRepository
            {
                get
                {
                    if (this.fiscalbillingcycleRepository == null)
                    {
                        this.fiscalbillingcycleRepository = new Repository<FiscalBillingCycle>(context);
                    }
                    return fiscalbillingcycleRepository;
                }
            }
            public IRepository<IntegrationMaster> IntegrationMasterRepository
            {
                get
                {
                    if (this.integrationmasterRepository == null)
                    {
                        this.integrationmasterRepository = new Repository<IntegrationMaster>(context);
                    }
                    return integrationmasterRepository;
                }
            }
            public IRepository<IntegrationTran> IntegrationTranRepository
            {
                get
                {
                    if (this.integrationtranRepository == null)
                    {
                        this.integrationtranRepository = new Repository<IntegrationTran>(context);
                    }
                    return integrationtranRepository;
                }
            }
            public IRepository<InvoiceJob> InvoiceJobRepository
            {
                get
                {
                    if (this.invoicejobRepository == null)
                    {
                        this.invoicejobRepository = new Repository<InvoiceJob>(context);
                    }
                    return invoicejobRepository;
                }
            }
            public IRepository<Inventory> InventoryFormSubmitRepository
            {
                get
                {
                    if (this.inventoryFormSubmitRepository == null)
                    {
                        this.inventoryFormSubmitRepository = new Repository<Inventory>(context);
                    }
                    return inventoryFormSubmitRepository;
                }
            }
            public IRepository<InvoiceMaster> InvoiceMasterRepository
                {
                get
                {
                    if (this.invoicemasterRepository == null)
                    {
                        this.invoicemasterRepository = new Repository<InvoiceMaster>(context);
                    }
                    return invoicemasterRepository;
                }
            }
            public IRepository<InvoiceTran> InvoiceTranRepository
            {
                get
                {
                    if (this.invoicetranRepository == null)
                    {
                        this.invoicetranRepository = new Repository<InvoiceTran>(context);
                    }
                    return invoicetranRepository;
                }
            }
            public IRepository<JobApprovalExt> JobApprovalExtRepository
            {
                get
                {
                    if (this.jobapprovalExtRepository == null)
                    {
                        this.jobapprovalExtRepository = new Repository<JobApprovalExt>(context);
                    }
                    return jobapprovalExtRepository;
                }
            }
            public IRepository<JobCategory> JobCategoryRepository
            {
                get
                {
                    if (this.jobcategoryRepository == null)
                    {
                        this.jobcategoryRepository = new Repository<JobCategory>(context);
                    }
                    return jobcategoryRepository;
                }
            }
            public IRepository<JobFilesLocationMaster> JobFilesLocationMasterRepository
            {
                get
                {
                    if (this.jobfileslocationmasterRepository == null)
                    {
                        this.jobfileslocationmasterRepository = new Repository<JobFilesLocationMaster>(context);
                    }
                    return jobfileslocationmasterRepository;
                }
            }
            public IRepository<JobOrder> JobOrderRepository
            {
                get
                {
                    if (this.joborderRepository == null)
                    {
                        this.joborderRepository = new Repository<JobOrder>(context);
                    }
                    return joborderRepository;
                }
            }
            public IRepository<JobOrderExtension> JobOrderExtensionRepository
            {
                get
                {
                    if (this.joborderextensionRepository == null)
                    {
                        this.joborderextensionRepository = new Repository<JobOrderExtension>(context);
                    }
                    return joborderextensionRepository;
                }
            }
            public IRepository<JobOrderFile> JobOrderFileRepository
            {
                get
                {
                    if (this.joborderfileRepository == null)
                    {
                        this.joborderfileRepository = new Repository<JobOrderFile>(context);
                    }
                    return joborderfileRepository;
                }
            }
            public IRepository<JobQuery> JobQueryRepository
            {
                get
                {
                    if (this.jobqueryRepository == null)
                    {
                        this.jobqueryRepository = new Repository<JobQuery>(context);
                    }
                    return jobqueryRepository;
                }
            }
            public IRepository<JobQuotation> JobQuotationRepository
            {
                get
                {
                    if (this.jobquotationRepository == null)
                    {
                        this.jobquotationRepository = new Repository<JobQuotation>(context);
                    }
                    return jobquotationRepository;
                }
            }
            public IRepository<JobStatus> JobStatusRepository
            {
                get
                {
                    if (this.jobstatusRepository == null)
                    {
                        this.jobstatusRepository = new Repository<JobStatus>(context);
                    }
                    return jobstatusRepository;
                }
            }
            public IRepository<JobStatusExt> JobStatusExtRepository
            {
                get
                {
                    if (this.jobstatusExtRepository == null)
                    {
                        this.jobstatusExtRepository = new Repository<JobStatusExt>(context);
                    }
                    return jobstatusExtRepository;
                }
            }
            public IRepository<JobSummary> JobSummaryRepository
            {
                get
                {
                    if (this.jobsummaryRepository == null)
                    {
                        this.jobsummaryRepository = new Repository<JobSummary>(context);
                    }
                    return jobsummaryRepository;
                }
            }
            public IRepository<Location> LocationRepository
            {
                get
                {
                    if (this.locationRepository == null)
                    {
                        this.locationRepository = new Repository<Location>(context);
                    }
                    return locationRepository;
                }
            }
            public IRepository<Log> LogRepository
            {
                get
                {
                    if (this.logRepository == null)
                    {
                        this.logRepository = new Repository<Log>(context);
                    }
                    return logRepository;
                }
            }
            public IRepository<MailContact> MailContactRepository
            {
                get
                {
                    if (this.mailcontactRepository == null)
                    {
                        this.mailcontactRepository = new Repository<MailContact>(context);
                    }
                    return mailcontactRepository;
                }
            }
            //public IRepository<MailMaster> MailMasterRepository
            //{
            //    get
            //    {
            //        if (this.mailmasterRepository == null)
            //        {
            //            this.mailmasterRepository = new Repository<MailMaster>(context);
            //        }
            //        return mailmasterRepository;
            //    }
            //}
            public IRepository<MailNotificationDetail> MailNotificationDetailRepository
            {
                get
                {
                    if (this.mailnotificationdetailRepository == null)
                    {
                        this.mailnotificationdetailRepository = new Repository<MailNotificationDetail>(context);
                    }
                    return mailnotificationdetailRepository;
                }
            }
            public IRepository<Menu> MenuRepository
            {
                get
                {
                    if (this.menuRepository == null)
                    {
                        this.menuRepository = new Repository<Menu>(context);
                    }
                    return menuRepository;
                }
            }
            public IRepository<News> NewsRepository
            {
                get
                {
                    if (this.newsRepository == null)
                    {
                        this.newsRepository = new Repository<News>(context);
                    }
                    return newsRepository;
                }
            }
            //public IRepository<Podio> PodioRepository
            //{
            //    get
            //    {
            //        if (this.podioRepository == null)
            //        {
            //            this.podioRepository = new Repository<Podio>(context);
            //        }
            //        return podioRepository;
            //    }
            //}
            //public IRepository<PodioMapping> PodioMappingRepository
            //{
            //    get
            //    {
            //        if (this.podiomappingRepository == null)
            //        {
            //            this.podiomappingRepository = new Repository<PodioMapping>(context);
            //        }
            //        return podiomappingRepository;
            //    }
            //}
            public IRepository<Pricing> PricingRepository
            {
                get
                {
                    if (this.pricingRepository == null)
                    {
                        this.pricingRepository = new Repository<Pricing>(context);
                    }
                    return pricingRepository;
                }
            }
            public IRepository<PricingFileRange> PricingFileRangeRepository
            {
                get
                {
                    if (this.pricingfilerangeRepository == null)
                    {
                        this.pricingfilerangeRepository = new Repository<PricingFileRange>(context);
                    }
                    return pricingfilerangeRepository;
                }
            }
            public IRepository<PricingScope> PricingScopeRepository
            {
                get
                {
                    if (this.pricingscopeRepository == null)
                    {
                        this.pricingscopeRepository = new Repository<PricingScope>(context);
                    }
                    return pricingscopeRepository;
                }
            }
            public IRepository<PricingStaffing> PricingStaffingRepository
            {
                get
                {
                    if (this.pricingstaffingRepository == null)
                    {
                        this.pricingstaffingRepository = new Repository<PricingStaffing>(context);
                    }
                    return pricingstaffingRepository;
                }
            }
            public IRepository<PricingType> PricingTypeRepository
            {
                get
                {
                    if (this.pricingtypeRepository == null)
                    {
                        this.pricingtypeRepository = new Repository<PricingType>(context);
                    }
                    return pricingtypeRepository;
                }
            }
            public IRepository<PricingWithScope> PricingWithScopeRepository
            {
                get
                {
                    if (this.pricingwithscopeRepository == null)
                    {
                        this.pricingwithscopeRepository = new Repository<PricingWithScope>(context);
                    }
                    return pricingwithscopeRepository;
                }
            }
            public IRepository<Process> ProcessRepository
            {
                get
                {
                    if (this.processRepository == null)
                    {
                        this.processRepository = new Repository<Process>(context);
                    }
                    return processRepository;
                }
            }
            public IRepository<ProcessWorkFlow> ProcessWorkFlowRepository
            {
                get
                {
                    if (this.processworkflowRepository == null)
                    {
                        this.processworkflowRepository = new Repository<ProcessWorkFlow>(context);
                    }
                    return processworkflowRepository;
                }
            }
            public IRepository<ProcessWorkFlowMaster> ProcessWorkFlowMasterRepository
            {
                get
                {
                    if (this.processworkflowmasterRepository == null)
                    {
                        this.processworkflowmasterRepository = new Repository<ProcessWorkFlowMaster>(context);
                    }
                    return processworkflowmasterRepository;
                }
            }
            public IRepository<ProcessWorkFlowStatus> ProcessWorkFlowStatusRepository
            {
                get
                {
                    if (this.processworkflowstatusRepository == null)
                    {
                        this.processworkflowstatusRepository = new Repository<ProcessWorkFlowStatus>(context);
                    }
                    return processworkflowstatusRepository;
                }
            }
            public IRepository<ProcessWorkFlowTran> ProcessWorkFlowTranRepository
            {
                get
                {
                    if (this.processworkflowtranRepository == null)
                    {
                        this.processworkflowtranRepository = new Repository<ProcessWorkFlowTran>(context);
                    }
                    return processworkflowtranRepository;
                }
            }
            public IRepository<Proficiency> ProficiencyRepository
            {
                get
                {
                    if (this.proficiencyRepository == null)
                    {
                        this.proficiencyRepository = new Repository<Proficiency>(context);
                    }
                    return proficiencyRepository;
                }
            }
            public IRepository<Receivable> ReceivableRepository
            {
                get
                {
                    if (this.receivableRepository == null)
                    {
                        this.receivableRepository = new Repository<Receivable>(context);
                    }
                    return receivableRepository;
                }
            }
            public IRepository<ReceivableAdjustment> ReceivableAdjustmentRepository
            {
                get
                {
                    if (this.receivableadjustmentRepository == null)
                    {
                        this.receivableadjustmentRepository = new Repository<ReceivableAdjustment>(context);
                    }
                    return receivableadjustmentRepository;
                }
            }
            public IRepository<ReceivableExt> ReceivableExtRepository
            {
                get
                {
                    if (this.receivableExtRepository == null)
                    {
                        this.receivableExtRepository = new Repository<ReceivableExt>(context);
                    }
                    return receivableExtRepository;
                }
            }
            public IRepository<RemovalReason> RemovalReasonRepository
            {
                get
                {
                    if (this.removalreasonRepository == null)
                    {
                        this.removalreasonRepository = new Repository<RemovalReason>(context);
                    }
                    return removalreasonRepository;
                }
            }
            public IRepository<ResignReason> ResignReasonRepository
            {
                get
                {
                    if (this.resignreasonRepository == null)
                    {
                        this.resignreasonRepository = new Repository<ResignReason>(context);
                    }
                    return resignreasonRepository;
                }
            }
            public IRepository<Role> RoleRepository
            {
                get
                {
                    if (this.roleRepository == null)
                    {
                        this.roleRepository = new Repository<Role>(context);
                    }
                    return roleRepository;
                }
            }
            public IRepository<Scope> ScopeRepository
            {
                get
                {
                    if (this.scopeRepository == null)
                    {
                        this.scopeRepository = new Repository<Scope>(context);
                    }
                    return scopeRepository;
                }
            }
            public IRepository<Shift> ShiftRepository
            {
                get
                {
                    if (this.shiftRepository == null)
                    {
                        this.shiftRepository = new Repository<Shift>(context);
                    }
                    return shiftRepository;
                }
            }
            public IRepository<ShiftVsEmployee> ShiftVsEmployeeRepository
            {
                get
                {
                    if (this.shiftVsemployeeRepository == null)
                    {
                        this.shiftVsemployeeRepository = new Repository<ShiftVsEmployee>(context);
                    }
                    return shiftVsemployeeRepository;
                }
            }
            public IRepository<StaffingPrice> StaffingPriceRepository
            {
                get
                {
                    if (this.staffingpriceRepository == null)
                    {
                        this.staffingpriceRepository = new Repository<StaffingPrice>(context);
                    }
                    return staffingpriceRepository;
                }
            }
            public IRepository<State> StateRepository
            {
                get
                {
                    if (this.stateRepository == null)
                    {
                        this.stateRepository = new Repository<State>(context);
                    }
                    return stateRepository;
                }
            }
            public IRepository<Status> StatusRepository
            {
                get
                {
                    if (this.statusRepository == null)
                    {
                        this.statusRepository = new Repository<Status>(context);
                    }
                    return statusRepository;
                }
            }
        public IRepository<Vis.VleadProcessV3.Models.TimeZone> TimeZoneRepository
        {
            get
            {
                if (this.timezoneRepository == null)
                {
                    this.timezoneRepository = new Repository<Vis.VleadProcessV3.Models.TimeZone>(context);
                }
                return timezoneRepository;
            }
        }
        public IRepository<TransactionType> TransactionTypeRepository
            {
                get
                {
                    if (this.transactiontypeRepository == null)
                    {
                        this.transactiontypeRepository = new Repository<TransactionType>(context);
                    }
                    return transactiontypeRepository;
                }
            }
            public IRepository<TrayDetailMovement> TrayDetailMovementRepository
            {
                get
                {
                    if (this.traydetailmovementRepository == null)
                    {
                        this.traydetailmovementRepository = new Repository<TrayDetailMovement>(context);
                    }
                    return traydetailmovementRepository;
                }
            }
            public IRepository<TrayDetailMovementTran> TrayDetailMovementTranRepository
            {
                get
                {
                    if (this.traydetailmovementtranRepository == null)
                    {
                        this.traydetailmovementtranRepository = new Repository<TrayDetailMovementTran>(context);
                    }
                    return traydetailmovementtranRepository;
                }
            }
            public IRepository<User> UserRepository
            {
                get
                {
                    if (this.userRepository == null)
                    {
                        this.userRepository = new Repository<User>(context);
                    }
                    return userRepository;
                }
            }
            public IRepository<VoucherControl> VoucherControlRepository
            {
                get
                {
                    if (this.vouchercontrolRepository == null)
                    {
                        this.vouchercontrolRepository = new Repository<VoucherControl>(context);
                    }
                    return vouchercontrolRepository;
                }
            }
            public IRepository<Holiday> HolidayRepository
            {
                get
                {
                    if (this.holidayRepository == null)
                    {
                        this.holidayRepository = new Repository<Holiday>(context);
                    }
                    return holidayRepository;
                }
            }
            public IRepository<Division> DivisionRepository
            {
                get
                {
                    if (this.divisionRepository == null)
                    {
                        this.divisionRepository = new Repository<Division>(context);
                    }
                    return divisionRepository;
                }
            }
            public IRepository<CustomerVsDivision> CustomerVsDivisionRepository
            {
                get
                {
                    if (this.customerVsdivisionRepository == null)
                    {
                        this.customerVsdivisionRepository = new Repository<CustomerVsDivision>(context);
                    }
                    return customerVsdivisionRepository;
                }
            }
            public IRepository<EmployeeVsDivision> EmployeeVsDivisionRepository
            {
                get
                {
                    if (this.employeeVsdivisionRepository == null)
                    {
                        this.employeeVsdivisionRepository = new Repository<EmployeeVsDivision>(context);
                    }
                    return employeeVsdivisionRepository;
                }
            }
            //------------------------------------------final CL---------------------------------------------------------------------
            public IRepository<Checklist> ChecklistRepository
            {
                get
                {
                    if (this.checklistRepository == null)
                    {
                        this.checklistRepository = new Repository<Checklist>(context);
                    }
                    return checklistRepository;
                }
            }
        //------------------------------------------final CL---------------------------------------------------------------------
        //-----------------------------------------slowness bugs fixed--------------------------------------------------------------------------
        public IRepository<TblItassetHardware> tbl_ITAssetHardwareRepository
        {
            get
            {
                if (this.Tbl_ITAssetHardwareRepository == null)
                {
                    this.Tbl_ITAssetHardwareRepository = new Repository<TblItassetHardware>(context);
                }
                return Tbl_ITAssetHardwareRepository;
            }
        }
        public IRepository<TblItassetSoftware> tbl_ITAssetSoftwareRepository
        {
            get
            {
                if (this.Tbl_ITAssetSoftwareRepository == null)
                {
                    this.Tbl_ITAssetSoftwareRepository = new Repository<TblItassetSoftware>(context);
                }
                return Tbl_ITAssetSoftwareRepository;
            }
        }
        ////-----------------------------------------slowness bugs fixed--------------------------------------------------------------------------
        ////---------------------------------ESs--------------------------------
        public IRepository<Skillset> SkillsetRepository
        {
            get
            {
                if (this.skillsetRepository == null)
                {
                    this.skillsetRepository = new Repository<Skillset>(context);
                }
                return skillsetRepository;
            }
        }
        public IRepository<EmployeeVsSkillset> EmployeeskillsetRepository
        {
            get
            {
                if (this.employeeskillsetRepository == null)
                {
                    this.employeeskillsetRepository = new Repository<EmployeeVsSkillset>(context);
                }
                return employeeskillsetRepository;
            }
        }
        ////-----------------------------------ESs---------------------------

        public void Save()
            {
                context.SaveChanges();
            }
            public int SaveChanges()
            {
                return context.SaveChanges();
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


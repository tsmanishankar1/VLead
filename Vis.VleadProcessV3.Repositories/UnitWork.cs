using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.Repositories
{
    public class UnitWork /*: IDisposable*/
    {
        private  ApplicationDbContext context;
        private  TallyDbContext Tally;


       
        public UnitWork(ApplicationDbContext context, TallyDbContext tally)
        {
            Tally = tally;
            this.context = context;
        }
        


        private IQueryBuilder<BankDetail> bankdetail;
        private IQueryBuilder<BenchScheduler> benchscheduler;
        private IQueryBuilder<BenchStatus> benchstatus;
        private IQueryBuilder<BillingCycle> billingcycle;
        private IQueryBuilder<City> city;
        private IQueryBuilder<ClientOrder> clientorder;
        private IQueryBuilder<ClientOrderExt> clientorderExt;
        private IQueryBuilder<Company> company;
        private IQueryBuilder<Country> country;
        private IQueryBuilder<Customer> customer;
        private IQueryBuilder<CustomerClassification> customerclassification;
        private IQueryBuilder<CustomerCommunication> customercommunication;
        private IQueryBuilder<CustomerContact> customercontact;
        private IQueryBuilder<CustomerDepartment> customerdepartment;
        private IQueryBuilder<CustomerExt> customerExt;
        private IQueryBuilder<CustomerRegistration> customerregistration;
        private IQueryBuilder<CustomerTat> customerTAT;
        private IQueryBuilder<CustomerVsEmployee> customerVsemployee;
        private IQueryBuilder<CustomersNorm> customersnorm;//--------------------------------Norms--------
        private IQueryBuilder<CustomerVsSalesEmployee> customerVssalesemployee;
        private IQueryBuilder<CustomerVsScope> customerVsscope;
        private IQueryBuilder<Department> department;
        private IQueryBuilder<Designation> designation;
        private IQueryBuilder<Employee> employee;
        private IQueryBuilder<EmployeeAssign> employeeassign;
        private IQueryBuilder<EmployeeCommunication> employeecommunication;
        private IQueryBuilder<EmployeeHierarchy> employeehierarchy;
        private IQueryBuilder<EmployeeProcess> employeeprocess;
        private IQueryBuilder<EmployeeVsRole> employeeVsrole;
        private IQueryBuilder<ErrorCategory> errorcategory;
        private IQueryBuilder<FileAttachment> fileattachment;
        private IQueryBuilder<FileInwardExcel> fileinwardexcel;
        private IQueryBuilder<FileInwardType> fileinwardtype;
        private IQueryBuilder<FileProcessTray> fileprocesstray;
        private IQueryBuilder<FileSummary> filesummary;
        private IQueryBuilder<FiscalBillingCycle> fiscalbillingcycle;
        private IQueryBuilder<IntegrationMaster> integrationmaster;
        private IQueryBuilder<IntegrationTran> integrationtran;
        private IQueryBuilder<InvoiceJob> invoicejob;
        private IQueryBuilder<InvoiceMaster> invoicemaster;
        private IQueryBuilder<InvoiceTran> invoicetran;
        private IQueryBuilder<JobApprovalExt> jobapprovalExt;
        private IQueryBuilder<JobCategory> jobcategory;
        private IQueryBuilder<JobFilesLocationMaster> jobfileslocationmaster;
        private IQueryBuilder<JobOrder> joborder;
        private IQueryBuilder<JobOrderExtension> joborderextension;
        private IQueryBuilder<JobOrderFile> joborderfile;
        private IQueryBuilder<JobQuery> jobquery;
        private IQueryBuilder<JobQuotation> jobquotation;
        private IQueryBuilder<JobStatus> jobstatus;
        private IQueryBuilder<JobStatusExt> jobstatusExt;
        private IQueryBuilder<JobSummary> jobsummary;
        private IQueryBuilder<Location> location;
        private IQueryBuilder<Log> log;
        private IQueryBuilder<MailContact> mailcontact;
        private IQueryBuilder<Mailmstr> mailmaster;
        private IQueryBuilder<MailNotificationDetail> mailnotificationdetail;
        private IQueryBuilder<Menu> menu;
        private IQueryBuilder<News> news;
        private IQueryBuilder<NonBillable> nonbillable;
        //private IQueryBuilder<Podio> podio; //balaji classes not present
        //private IQueryBuilder<PodioMapping> podiomapping;
        private IQueryBuilder<Pricing> pricing;
        private IQueryBuilder<PricingFileRange> pricingfilerange;
        private IQueryBuilder<PricingScope> pricingscope;
        private IQueryBuilder<PricingStaffing> pricingstaffing;
        private IQueryBuilder<PricingType> pricingtype;
        private IQueryBuilder<PricingWithScope> pricingwithscope;
        private IQueryBuilder<Process> process;
        private IQueryBuilder<ProcessWorkFlow> processworkflow;
        private IQueryBuilder<ProcessWorkFlowMaster> processworkflowmaster;
        private IQueryBuilder<ProcessWorkFlowStatus> processworkflowstatus;
        private IQueryBuilder<ProcessWorkFlowTran> processworkflowtran;
        private IQueryBuilder<Proficiency> proficiency;
        private IQueryBuilder<Receivable> receivable;
        private IQueryBuilder<ReceivableAdjustment> receivableadjustment;
        private IQueryBuilder<ReceivableExt> receivableExt;
        private IQueryBuilder<RemovalReason> removalreason;
        private IQueryBuilder<ResignReason> resignreason;
        private IQueryBuilder<Role> role;
        private IQueryBuilder<Scope> scope;
        private IQueryBuilder<Shift> shift;
        private IQueryBuilder<ShiftVsEmployee> shiftVsemployee;
        private IQueryBuilder<StaffingPrice> staffingprice;
        private IQueryBuilder<State> state;
        private IQueryBuilder<Status> status;
        private IQueryBuilder<Vis.VleadProcessV3.Models.TimeZone> timezone;
        private IQueryBuilder<TransactionType> transactiontype;
        private IQueryBuilder<TrayDetailMovement> traydetailmovement;
        private IQueryBuilder<TrayDetailMovementTran> traydetailmovementtran;
        private IQueryBuilder<User> user;
        private IQueryBuilder<VoucherControl> vouchercontrol;
        private IQueryBuilder<Holiday> holiday;
        private IQueryBuilder<Division> division;
        private IQueryBuilder<CustomerVsDivision> customerVsdivision;
        private IQueryBuilder<EmployeeVsDivision> employeeVsdivision;
        private IQueryBuilder<Checklist> checklist;//-----------------------final CL----------------------------------------

        public IQueryBuilder<BankDetail> BankDetail
        {
            get
            {
                if (this.bankdetail == null)
                {
                    this.bankdetail = new QueryBuilder<BankDetail>(context,Tally);
                }
                return bankdetail;
            }
        }
        public IQueryBuilder<BenchScheduler> BenchScheduler
        {
            get
            {
                if (this.benchscheduler == null)
                {
                    this.benchscheduler = new QueryBuilder<BenchScheduler>(context,Tally);
                }
                return benchscheduler;
            }
        }
        public IQueryBuilder<BenchStatus> BenchStatus
        {
            get
            {
                if (this.benchstatus == null)
                {
                    this.benchstatus = new QueryBuilder<BenchStatus>(context,Tally);
                }
                return benchstatus;
            }
        }
        public IQueryBuilder<BillingCycle> BillingCycle
        {
            get
            {
                if (this.billingcycle == null)
                {
                    this.billingcycle = new QueryBuilder<BillingCycle>(context,Tally);
                }
                return billingcycle;
            }
        }
        public IQueryBuilder<City> City
        {
            get
            {
                if (this.city == null)
                {
                    this.city = new QueryBuilder<City>(context,Tally);
                }
                return city;
            }
        }
        public IQueryBuilder<ClientOrder> ClientOrder
        {
            get
            {
                if (this.clientorder == null)
                {
                    this.clientorder = new QueryBuilder<ClientOrder>(context,Tally);
                }
                return clientorder;
            }
        }
        public IQueryBuilder<ClientOrderExt> ClientOrderExt
        {
            get
            {
                if (this.clientorderExt == null)
                {
                    this.clientorderExt = new QueryBuilder<ClientOrderExt>(context,Tally);
                }
                return clientorderExt;
            }
        }
        public IQueryBuilder<Company> Company
        {
            get
            {
                if (this.company == null)
                {
                    this.company = new QueryBuilder<Company>(context,Tally);
                }
                return company;
            }
        }
        public IQueryBuilder<Country> Country
        {
            get
            {
                if (this.country == null)
                {
                    this.country = new QueryBuilder<Country>(context,Tally);
                }
                return country;
            }
        }
        public IQueryBuilder<Customer> Customer
        {
            get
            {
                if (this.customer == null)
                {
                    this.customer = new QueryBuilder<Customer>(context,Tally);
                }
                return customer;
            }
        }
        public IQueryBuilder<CustomerClassification> CustomerClassification
        {
            get
            {
                if (this.customerclassification == null)
                {
                    this.customerclassification = new QueryBuilder<CustomerClassification>(context,Tally);
                }
                return customerclassification;
            }
        }
        public IQueryBuilder<CustomerCommunication> CustomerCommunication
        {
            get
            {
                if (this.customercommunication == null)
                {
                    this.customercommunication = new QueryBuilder<CustomerCommunication>(context,Tally);
                }
                return customercommunication;
            }
        }
        public IQueryBuilder<CustomerContact> CustomerContact
        {
            get
            {
                if (this.customercontact == null)
                {
                    this.customercontact = new QueryBuilder<CustomerContact>(context,Tally);
                }
                return customercontact;
            }
        }
        public IQueryBuilder<CustomerDepartment> CustomerDepartment
        {
            get
            {
                if (this.customerdepartment == null)
                {
                    this.customerdepartment = new QueryBuilder<CustomerDepartment>(context,Tally);
                }
                return customerdepartment;
            }
        }
        public IQueryBuilder<CustomerExt> CustomerExt
        {
            get
            {
                if (this.customerExt == null)
                {
                    this.customerExt = new QueryBuilder<CustomerExt>(context,Tally);
                }
                return customerExt;
            }
        }
        public IQueryBuilder<CustomerRegistration> CustomerRegistration
        {
            get
            {
                if (this.customerregistration == null)
                {
                    this.customerregistration = new QueryBuilder<CustomerRegistration>(context,Tally);
                }
                return customerregistration;
            }
        }
        public IQueryBuilder<CustomerTat> CustomerTAT
        {
            get
            {
                if (this.customerTAT == null)
                {
                    this.customerTAT = new QueryBuilder<CustomerTat>(context,Tally);
                }
                return customerTAT;
            }
        }
        public IQueryBuilder<CustomerVsEmployee> CustomerVsEmployee
        {
            get
            {
                if (this.customerVsemployee == null)
                {
                    this.customerVsemployee = new QueryBuilder<CustomerVsEmployee>(context,Tally);
                }
                return customerVsemployee;
            }
        }

        //Norms
        public IQueryBuilder<CustomersNorm> CustomersNorm
        {
            get
            {
                if (this.customersnorm == null)
                {
                    this.customersnorm = new QueryBuilder<CustomersNorm>(context,Tally);
                }
                return customersnorm;
            }
        }
        //Norms
        public IQueryBuilder<CustomerVsSalesEmployee> CustomerVsSalesEmployee
        {
            get
            {
                if (this.customerVssalesemployee == null)
                {
                    this.customerVssalesemployee = new QueryBuilder<CustomerVsSalesEmployee>(context,Tally);
                }
                return customerVssalesemployee;
            }
        }
        public IQueryBuilder<CustomerVsScope> CustomerVsScope
        {
            get
            {
                if (this.customerVsscope == null)
                {
                    this.customerVsscope = new QueryBuilder<CustomerVsScope>(context,Tally);
                }
                return customerVsscope;
            }
        }
        public IQueryBuilder<Department> Department
        {
            get
            {
                if (this.department == null)
                {
                    this.department = new QueryBuilder<Department>(context,Tally);
                }
                return department;
            }
        }
        public IQueryBuilder<Designation> Designation
        {
            get
            {
                if (this.designation == null)
                {
                    this.designation = new QueryBuilder<Designation>(context,Tally);
                }
                return designation;
            }
        }
        public IQueryBuilder<Employee> Employee
        {
            get
            {
                if (this.employee == null)
                {
                    this.employee = new QueryBuilder<Employee>(context,Tally);
                }
                return employee;
            }
        }
        public IQueryBuilder<EmployeeAssign> EmployeeAssign
        {
            get
            {
                if (this.employeeassign == null)
                {
                    this.employeeassign = new QueryBuilder<EmployeeAssign>(context,Tally);
                }
                return employeeassign;
            }
        }
        public IQueryBuilder<EmployeeCommunication> EmployeeCommunication
        {
            get
            {
                if (this.employeecommunication == null)
                {
                    this.employeecommunication = new QueryBuilder<EmployeeCommunication>(context,Tally);
                }
                return employeecommunication;
            }
        }
        public IQueryBuilder<EmployeeHierarchy> EmployeeHierarchy
        {
            get
            {
                if (this.employeehierarchy == null)
                {
                    this.employeehierarchy = new QueryBuilder<EmployeeHierarchy>(context,Tally);
                }
                return employeehierarchy;
            }
        }
        public IQueryBuilder<EmployeeProcess> EmployeeProcess
        {
            get
            {
                if (this.employeeprocess == null)
                {
                    this.employeeprocess = new QueryBuilder<EmployeeProcess>(context,Tally);
                }
                return employeeprocess;
            }
        }
        public IQueryBuilder<EmployeeVsRole> EmployeeVsRole
        {
            get
            {
                if (this.employeeVsrole == null)
                {
                    this.employeeVsrole = new QueryBuilder<EmployeeVsRole>(context,Tally);
                }
                return employeeVsrole;
            }
        }
        public IQueryBuilder<ErrorCategory> ErrorCategory
        {
            get
            {
                if (this.errorcategory == null)
                {
                    this.errorcategory = new QueryBuilder<ErrorCategory>(context,Tally);
                }
                return errorcategory;
            }
        }
        public IQueryBuilder<FileAttachment> FileAttachment
        {
            get
            {
                if (this.fileattachment == null)
                {
                    this.fileattachment = new QueryBuilder<FileAttachment>(context,Tally);
                }
                return fileattachment;
            }
        }
        public IQueryBuilder<FileInwardExcel> FileInwardExcel
        {
            get
            {
                if (this.fileinwardexcel == null)
                {
                    this.fileinwardexcel = new QueryBuilder<FileInwardExcel>(context,Tally);
                }
                return fileinwardexcel;
            }
        }
        public IQueryBuilder<FileInwardType> FileInwardType
        {
            get
            {
                if (this.fileinwardtype == null)
                {
                    this.fileinwardtype = new QueryBuilder<FileInwardType>(context,Tally);
                }
                return fileinwardtype;
            }
        }
        public IQueryBuilder<FileProcessTray> FileProcessTray
        {
            get
            {
                if (this.fileprocesstray == null)
                {
                    this.fileprocesstray = new QueryBuilder<FileProcessTray>(context,Tally);
                }
                return fileprocesstray;
            }
        }
        public IQueryBuilder<FileSummary> FileSummary
        {
            get
            {
                if (this.filesummary == null)
                {
                    this.filesummary = new QueryBuilder<FileSummary>(context,Tally);
                }
                return filesummary;
            }
        }
        public IQueryBuilder<FiscalBillingCycle> FiscalBillingCycle
        {
            get
            {
                if (this.fiscalbillingcycle == null)
                {
                    this.fiscalbillingcycle = new QueryBuilder<FiscalBillingCycle>(context,Tally);
                }
                return fiscalbillingcycle;
            }
        }
        public IQueryBuilder<IntegrationMaster> IntegrationMaster
        {
            get
            {
                if (this.integrationmaster == null)
                {
                    this.integrationmaster = new QueryBuilder<IntegrationMaster>(context,Tally);
                }
                return integrationmaster;
            }
        }
        public IQueryBuilder<IntegrationTran> IntegrationTran
        {
            get
            {
                if (this.integrationtran == null)
                {
                    this.integrationtran = new QueryBuilder<IntegrationTran>(context,Tally);
                }
                return integrationtran;
            }
        }
        public IQueryBuilder<InvoiceJob> InvoiceJob
        {
            get
            {
                if (this.invoicejob == null)
                {
                    this.invoicejob = new QueryBuilder<InvoiceJob>(context,Tally);
                }
                return invoicejob;
            }
        }
        public IQueryBuilder<InvoiceMaster> InvoiceMaster
        {
            get
            {
                if (this.invoicemaster == null)
                {
                    this.invoicemaster = new QueryBuilder<InvoiceMaster>(context,Tally);
                }
                return invoicemaster;
            }
        }
        public IQueryBuilder<InvoiceTran> InvoiceTran
        {
            get
            {
                if (this.invoicetran == null)
                {
                    this.invoicetran = new QueryBuilder<InvoiceTran>(context,Tally);
                }
                return invoicetran;
            }
        }
        public IQueryBuilder<JobApprovalExt> JobApprovalExt
        {
            get
            {
                if (this.jobapprovalExt == null)
                {
                    this.jobapprovalExt = new QueryBuilder<JobApprovalExt>(context,Tally);
                }
                return jobapprovalExt;
            }
        }
        public IQueryBuilder<JobCategory> JobCategory
        {
            get
            {
                if (this.jobcategory == null)
                {
                    this.jobcategory = new QueryBuilder<JobCategory>(context,Tally);
                }
                return jobcategory;
            }
        }
        public IQueryBuilder<JobFilesLocationMaster> JobFilesLocationMaster
        {
            get
            {
                if (this.jobfileslocationmaster == null)
                {
                    this.jobfileslocationmaster = new QueryBuilder<JobFilesLocationMaster>(context,Tally);
                }
                return jobfileslocationmaster;
            }
        }
        public IQueryBuilder<JobOrder> JobOrder
        {
            get
            {
                if (this.joborder == null)
                {
                    this.joborder = new QueryBuilder<JobOrder>(context,Tally);
                }
                return joborder;
            }
        }
        public IQueryBuilder<JobOrderExtension> JobOrderExtension
        {
            get
            {
                if (this.joborderextension == null)
                {
                    this.joborderextension = new QueryBuilder<JobOrderExtension>(context,Tally);
                }
                return joborderextension;
            }
        }
        public IQueryBuilder<JobOrderFile> JobOrderFile
        {
            get
            {
                if (this.joborderfile == null)
                {
                    this.joborderfile = new QueryBuilder<JobOrderFile>(context,Tally);
                }
                return joborderfile;
            }
        }
        public IQueryBuilder<JobQuery> JobQuery
        {
            get
            {
                if (this.jobquery == null)
                {
                    this.jobquery = new QueryBuilder<JobQuery>(context,Tally);
                }
                return jobquery;
            }
        }
        public IQueryBuilder<JobQuotation> JobQuotation
        {
            get
            {
                if (this.jobquotation == null)
                {
                    this.jobquotation = new QueryBuilder<JobQuotation>(context,Tally);
                }
                return jobquotation;
            }
        }
        public IQueryBuilder<JobStatus> JobStatus
        {
            get
            {
                if (this.jobstatus == null)
                {
                    this.jobstatus = new QueryBuilder<JobStatus>(context,Tally);
                }
                return jobstatus;
            }
        }
        public IQueryBuilder<JobStatusExt> JobStatusExt
        {
            get
            {
                if (this.jobstatusExt == null)
                {
                    this.jobstatusExt = new QueryBuilder<JobStatusExt>(context,Tally);
                }
                return jobstatusExt;
            }
        }
        public IQueryBuilder<JobSummary> JobSummary
        {
            get
            {
                if (this.jobsummary == null)
                {
                    this.jobsummary = new QueryBuilder<JobSummary>(context,Tally);
                }
                return jobsummary;
            }
        }
        public IQueryBuilder<Location> Location
        {
            get
            {
                if (this.location == null)
                {
                    this.location = new QueryBuilder<Location>(context,Tally);
                }
                return location;
            }
        }
        public IQueryBuilder<Log> Log
        {
            get
            {
                if (this.log == null)
                {
                    this.log = new QueryBuilder<Log>(context,Tally);
                }
                return log;
            }
        }
        public IQueryBuilder<MailContact> MailContact
        {
            get
            {
                if (this.mailcontact == null)
                {
                    this.mailcontact = new QueryBuilder<MailContact>(context,Tally);
                }
                return mailcontact;
            }
        }
        public IQueryBuilder<Mailmstr> MailMaster
        {
            get
            {
                if (this.mailmaster == null)
                {
                    this.mailmaster = new QueryBuilder<Mailmstr>(context,Tally);
                }
                return mailmaster;
            }
        }
        public IQueryBuilder<MailNotificationDetail> MailNotificationDetail
        {
            get
            {
                if (this.mailnotificationdetail == null)
                {
                    this.mailnotificationdetail = new QueryBuilder<MailNotificationDetail>(context,Tally);
                }
                return mailnotificationdetail;
            }
        }
        public IQueryBuilder<Menu> Menu
        {
            get
            {
                if (this.menu == null)
                {
                    this.menu = new QueryBuilder<Menu>(context,Tally);
                }
                return menu;
            }
        }
        public IQueryBuilder<News> News
        {
            get
            {
                if (this.news == null)
                {
                    this.news = new QueryBuilder<News>(context,Tally);
                }
                return news;
            }
        }
        public IQueryBuilder<NonBillable> NonBillable
        {
            get
            {
                if (this.nonbillable == null)
                {
                    this.nonbillable = new QueryBuilder<NonBillable>(context,Tally);
                }
                return nonbillable;
            }
        }
        //public IQueryBuilder<Podio> Podio
        //{
        //    get
        //    {
        //        if (this.podio == null)
        //        {
        //            this.podio = new QueryBuilder<Podio>(context,Tally);
        //        }
        //        return podio;
        //    }
        //}
        //public IQueryBuilder<PodioMapping> PodioMapping
        //{
        //    get
        //    {
        //        if (this.podiomapping == null)
        //        {
        //            this.podiomapping = new QueryBuilder<PodioMapping>(context,Tally);
        //        }
        //        return podiomapping;
        //    }
        //}
        public IQueryBuilder<Pricing> Pricing
        {
            get
            {
                if (this.pricing == null)
                {
                    this.pricing = new QueryBuilder<Pricing>(context,Tally);
                }
                return pricing;
            }
        }
        public IQueryBuilder<PricingFileRange> PricingFileRange
        {
            get
            {
                if (this.pricingfilerange == null)
                {
                    this.pricingfilerange = new QueryBuilder<PricingFileRange>(context,Tally);
                }
                return pricingfilerange;
            }
        }
        public IQueryBuilder<PricingScope> PricingScope
        {
            get
            {
                if (this.pricingscope == null)
                {
                    this.pricingscope = new QueryBuilder<PricingScope>(context,Tally);
                }
                return pricingscope;
            }
        }
        public IQueryBuilder<PricingStaffing> PricingStaffing
        {
            get
            {
                if (this.pricingstaffing == null)
                {
                    this.pricingstaffing = new QueryBuilder<PricingStaffing>(context,Tally);
                }
                return pricingstaffing;
            }
        }
        public IQueryBuilder<PricingType> PricingType
        {
            get
            {
                if (this.pricingtype == null)
                {
                    this.pricingtype = new QueryBuilder<PricingType>(context,Tally);
                }
                return pricingtype;
            }
        }
        public IQueryBuilder<PricingWithScope> PricingWithScope
        {
            get
            {
                if (this.pricingwithscope == null)
                {
                    this.pricingwithscope = new QueryBuilder<PricingWithScope>(context,Tally);
                }
                return pricingwithscope;
            }
        }
        public IQueryBuilder<Process> Process
        {
            get
            {
                if (this.process == null)
                {
                    this.process = new QueryBuilder<Process>(context,Tally);
                }
                return process;
            }
        }
        public IQueryBuilder<ProcessWorkFlow> ProcessWorkFlow
        {
            get
            {
                if (this.processworkflow == null)
                {
                    this.processworkflow = new QueryBuilder<ProcessWorkFlow>(context,Tally);
                }
                return processworkflow;
            }
        }
        public IQueryBuilder<ProcessWorkFlowMaster> ProcessWorkFlowMaster
        {
            get
            {
                if (this.processworkflowmaster == null)
                {
                    this.processworkflowmaster = new QueryBuilder<ProcessWorkFlowMaster>(context,Tally);
                }
                return processworkflowmaster;
            }
        }
        public IQueryBuilder<ProcessWorkFlowStatus> ProcessWorkFlowStatus
        {
            get
            {
                if (this.processworkflowstatus == null)
                {
                    this.processworkflowstatus = new QueryBuilder<ProcessWorkFlowStatus>(context,Tally);
                }
                return processworkflowstatus;
            }
        }
        public IQueryBuilder<ProcessWorkFlowTran> ProcessWorkFlowTran
        {
            get
            {
                if (this.processworkflowtran == null)
                {
                    this.processworkflowtran = new QueryBuilder<ProcessWorkFlowTran>(context,Tally);
                }
                return processworkflowtran;
            }
        }
        public IQueryBuilder<Proficiency> Proficiency
        {
            get
            {
                if (this.proficiency == null)
                {
                    this.proficiency = new QueryBuilder<Proficiency>(context,Tally);
                }
                return proficiency;
            }
        }
        public IQueryBuilder<Receivable> Receivable
        {
            get
            {
                if (this.receivable == null)
                {
                    this.receivable = new QueryBuilder<Receivable>(context,Tally);
                }
                return receivable;
            }
        }
        public IQueryBuilder<ReceivableAdjustment> ReceivableAdjustment
        {
            get
            {
                if (this.receivableadjustment == null)
                {
                    this.receivableadjustment = new QueryBuilder<ReceivableAdjustment>(context,Tally);
                }
                return receivableadjustment;
            }
        }
        public IQueryBuilder<ReceivableExt> ReceivableExt
        {
            get
            {
                if (this.receivableExt == null)
                {
                    this.receivableExt = new QueryBuilder<ReceivableExt>(context,Tally);
                }
                return receivableExt;
            }
        }
        public IQueryBuilder<RemovalReason> RemovalReason
        {
            get
            {
                if (this.removalreason == null)
                {
                    this.removalreason = new QueryBuilder<RemovalReason>(context,Tally);
                }
                return removalreason;
            }
        }
        public IQueryBuilder<ResignReason> ResignReason
        {
            get
            {
                if (this.resignreason == null)
                {
                    this.resignreason = new QueryBuilder<ResignReason>(context,Tally);
                }
                return resignreason;
            }
        }
        public IQueryBuilder<Role> Role
        {
            get
            {
                if (this.role == null)
                {
                    this.role = new QueryBuilder<Role>(context,Tally);
                }
                return role;
            }
        }
        public IQueryBuilder<Scope> Scope
        {
            get
            {
                if (this.scope == null)
                {
                    this.scope = new QueryBuilder<Scope>(context,Tally);
                }
                return scope;
            }
        }
        public IQueryBuilder<Shift> Shift
        {
            get
            {
                if (this.shift == null)
                {
                    this.shift = new QueryBuilder<Shift>(context,Tally);
                }
                return shift;
            }
        }
        public IQueryBuilder<ShiftVsEmployee> ShiftVsEmployee
        {
            get
            {
                if (this.shiftVsemployee == null)
                {
                    this.shiftVsemployee = new QueryBuilder<ShiftVsEmployee>(context,Tally);
                }
                return shiftVsemployee;
            }
        }
        public IQueryBuilder<StaffingPrice> StaffingPrice
        {
            get
            {
                if (this.staffingprice == null)
                {
                    this.staffingprice = new QueryBuilder<StaffingPrice>(context,Tally);
                }
                return staffingprice;
            }
        }
        public IQueryBuilder<State> State
        {
            get
            {
                if (this.state == null)
                {
                    this.state = new QueryBuilder<State>(context,Tally);
                }
                return state;
            }
        }
        public IQueryBuilder<Status> Status
        {
            get
            {
                if (this.status == null)
                {
                    this.status = new QueryBuilder<Status>(context,Tally);
                }
                return status;
            }
        }
        public IQueryBuilder<Vis.VleadProcessV3.Models.TimeZone> TimeZone
        {
            get
            {
                if (this.timezone == null)
                {
                    this.timezone = new QueryBuilder<Vis.VleadProcessV3.Models.TimeZone>(context,Tally);
                }
                return timezone;
            }
        }
        public IQueryBuilder<TransactionType> TransactionType
        {
            get
            {
                if (this.transactiontype == null)
                {
                    this.transactiontype = new QueryBuilder<TransactionType>(context,Tally);
                }
                return transactiontype;
            }
        }
        public IQueryBuilder<TrayDetailMovement> TrayDetailMovement
        {
            get
            {
                if (this.traydetailmovement == null)
                {
                    this.traydetailmovement = new QueryBuilder<TrayDetailMovement>(context,Tally);
                }
                return traydetailmovement;
            }
        }
        public IQueryBuilder<TrayDetailMovementTran> TrayDetailMovementTran
        {
            get
            {
                if (this.traydetailmovementtran == null)
                {
                    this.traydetailmovementtran = new QueryBuilder<TrayDetailMovementTran>(context,Tally);
                }
                return traydetailmovementtran;
            }
        }
        public IQueryBuilder<User> User
        {
            get
            {
                if (this.user == null)
                {
                    this.user = new QueryBuilder<User>(context,Tally);
                }
                return user;
            }
        }
        public IQueryBuilder<VoucherControl> VoucherControl
        {
            get
            {
                if (this.vouchercontrol == null)
                {
                    this.vouchercontrol = new QueryBuilder<VoucherControl>(context,Tally);
                }
                return vouchercontrol;
            }
        }
        public IQueryBuilder<Holiday> Holiday
        {
            get
            {
                if (this.holiday == null)
                {
                    this.holiday = new QueryBuilder<Holiday>(context,Tally);
                }
                return holiday;
            }
        }
        public IQueryBuilder<Division> Division
        {
            get
            {
                if (this.division == null)
                {
                    this.division = new QueryBuilder<Division>(context,Tally);
                }
                return division;
            }
        }
        public IQueryBuilder<CustomerVsDivision> CustomerVsDivision
        {
            get
            {
                if (this.customerVsdivision == null)
                {
                    this.customerVsdivision = new QueryBuilder<CustomerVsDivision>(context,Tally);
                }
                return customerVsdivision;
            }
        }
        public IQueryBuilder<EmployeeVsDivision> EmployeeVsDivision
        {
            get
            {
                if (this.employeeVsdivision == null)
                {
                    this.employeeVsdivision = new QueryBuilder<EmployeeVsDivision>(context,Tally);
                }
                return employeeVsdivision;
            }
        }
        //------------------------------------------------final CL----------------------------------------------
        public IQueryBuilder<Checklist> Checklist
        {
            get
            {
                if (this.checklist == null)
                {
                    this.checklist = new QueryBuilder<Checklist>(context,Tally);
                }
                return checklist;

            }
        }
        //---------------------------------------------------final CL---------------------------------------------------------
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
        public int Save()
        {
            return context.SaveChanges();
        }
        public void SaveTally()
        {
            Tally.SaveChanges();
        }
    }
}

using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.ViewModels
{
    public class AddCustomer
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int CustomerClassificationId { get; set; }
        public short? CreditDays { get; set; }
        public bool IsBlacklisted { get; set; }
        public bool IsApproved { get; set; }
        public string BlacklistedReasons { get; set; }
        public int[] Department { get; set; }
        public decimal? CreditLimit { get; set; }
        public decimal? CreditLimitAvailed { get; set; }
        // public string JobType { get; set; }
        public string TimeZone { get; set; }
        public string ReportTimeZone { get; set; }
        public string DropdownTimeZone { get; set; }
        public int DepartmentId { get; set; }
        public string EstablishmentType { get; set; }
        public string BillingCycleType { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int LocationId { get; set; }
        public string EmailAddress { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string WebAddress { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string CustomerDepartmentName { get; set; }
        public DateTime CreatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedUTC { get; set; }
        public int UpdatedBy { get; set; }
        public IEnumerable<Department> SelectedDepartments { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public string PhoneNo { get; set; }
        public Nullable<bool> Active { get; set; }
        public string VerifyCode { get; set; }
        // public string CountryName { get; set; }      
        // public string StateName { get; set; }
        // public string CityName { get; set; }     
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string CustomerJobType { get; set; }
        public string InputType { get; set; }
        public string OutputType { get; set; }
        public string PrivilegedClient { get; set; }
        public string PaymentMode { get; set; }
        public bool? IsBulk { get; set; }
        public bool? Checklist { get; set; }//----------------------------------------final CL-----------------------------------------------
        public bool? IsRush { get; set; }
        public bool BunchMail { get; set; }
        public bool? IsEstimatedTime { get; set; }
        public bool? IsManualUpload { get; set; }
        //public bool? IsRushApproved { get; set; }
        public Nullable<int> RPTTimeZoneDifference { get; set; }
        public Nullable<DateTime> TrialStartDate { get; set; }
        public Nullable<DateTime> LiveStartDate { get; set; }
        public string ModeofSales { get; set; }
        public string CurrencyMode { get; set; }
    }
    public class AddCountry
    {
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string TimeZone { get; set; }
    }

    public class AddCustomerReg
    {
        public string UserName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string EmailID { get; set; }
        public string PhoneNo { get; set; }
        public string Fax { get; set; }
    }
    public class AddMailContact
    {
        public int IdVal { get; set; }
        public string ProcessName { get; set; }
        public string Client { get; set; }
        public string FromMail { get; set; }
        public List<string> ToMail { get; set; }
        public List<string> CCMail { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedUTC { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public int DepartmentId { get; set; }
        public IEnumerable<AddMailContact> AddContacts { get; set; }

    }
}

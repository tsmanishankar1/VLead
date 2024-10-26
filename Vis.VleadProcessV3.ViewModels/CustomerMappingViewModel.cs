namespace Vis.VleadProcessV3.ViewModels
{
    public class CustomerVsSalesViewModel
    {
        public int Id { get; set; }
        public int[]? CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public string CustomerName { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedUTC { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
        public int CustomerType { get; set; }

    }


    public class customerVsSalesMapping
    {
        public int CreatedBy { get; set; }
        public IEnumerable<CustomerVsSalesViewModel> selectedCustomers { get; set; }
        public IEnumerable<CustomerVsSalesViewModel> selectedEmployee { get; set; }


    }

    public class CustomerVsSalesEmpViewModel
    {
        public int Id { get; set; }
        public int? CustId { get; set; }
        public int? EmpID { get; set; }
        public string CustomerName { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNameCode { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public System.DateTime CreatedUTC { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }

    }

    public class CustomerVsScopeViewModel
    {
        public int Id { get; set; }
        //
        public string ShortName { get; set; }
        public string Name { get; set; }
        //
        public int CustId { get; set; }
        public int ScopeId { get; set; }
        public int DeptId { get; set; }
        public string CustName { get; set; }
        public string Description { get; set; }
        public string ScopeName { get; set; }
        public string ScopeGroupDescription { get; set; }
        public int ScopeGroupId { get; set; }
        public string DeptName { get; set; }
        public string CustJobType { get; set; }
        public bool IsEstimatedTime { get; set; }
        public int? EstimatedTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedUTC { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
        //public IEnumerable<CustomerVsScopeViewModel> Customers { get; set; }
        //public IEnumerable<CustomerVsScopeViewModel> Departments { get; set; }
        //public IEnumerable<CustomerVsScopeViewModel> Scopes { get; set; }
        // public IEnumerable<CustomerVsScopeViewModel> CustomerJobypes { get; set; }  
    }

    public class CustomerTATViewModel
    {

        public int JobStatusId { get; set; }
        public int CustomerId { get; set; }
        public decimal TAT { get; set; }
        public string CustomerShortName { get; set; }
        public string JobStatusDescription { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedUTC { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedUTC { get; set; }
        public bool IsActive { get; set; }
    }
    public class selectedEmployee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }

    }
    public class SelectedCustomersList
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
    }
    public class SelectedDepartmentList
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
    public class SelectedScopeList
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }

    public class scopegroupdesc
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ScopeId { get; set; }
        public int DeptId { get; set; }
        public string CustomerName { get; set; }
        public string ScopeName { get; set; }
        public string DeptName { get; set; }
        public string CustomerJobType { get; set; }
        public string ScopeGroupDescription { get; set; }
    }

    public class ScopeChangeVM
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int DepartmentId { get; set; }
        public int ClientId { get; set; }
        public int ScopeId { get; set; }
        public string JobId { get; set; }
        public int JId { get; set; }
        public IEnumerable<ScopeChangeVM> ChangeScope { get; set; }
    }
    public class StitchCountChangeVM
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int DepartmentId { get; set; }
        public int ClientId { get; set; }
        public long StitchCount { get; set; }
        public string JobId { get; set; }
        public int JId { get; set; }
        public IEnumerable<StitchCountChangeVM> ChangeStitchCount { get; set; }
    }
    public class SetSpecialPricingVM
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int DepartmentId { get; set; }
        public int ClientId { get; set; }
        public long SpecialPrice { get; set; }
        public string JobId { get; set; }
        public int JId { get; set; }
        public IEnumerable<SetSpecialPricingVM> SpecialPricing { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.ViewModels
{
    public class GetPricingVM
    {
        public IEnumerable<Department> Departments { get; set; }
        public Department SelectedDepartment { get; set; }
        public IEnumerable<PricingType> PricingTypes { get; set; }
        public PricingType SelectedPricingType { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
        public Customer SelectedCustomer { get; set; }
        public IEnumerable<Scope> Scopes { get; set; }
        public IEnumerable<StaffingPrice> StaffingPrices { get; set; }
        public IEnumerable<Pricing> Pricings { get; set; }
    }
    public class PricingWithScopeViewModel
    {
        public IEnumerable<Department> Departments { get; set; }
        public Department? SelectedDepartment { get; set; } = null;
        public IEnumerable<PricingType> PricingTypes { get; set; }
        public PricingType? SelectedPricingType { get; set; } = null;
        public IEnumerable<Customer> Customers { get; set; }
        public Customer? SelectedCustomer { get; set; } = null;
        public IEnumerable<Pricing> Pricings { get; set; }
        public Scope? SelectedScope { get; set; } = null;
        public string ScopeTempDesc { get; set; }
        public string EstimatedTime { get; set; }
        public decimal RatePerHour { get; set; }
        public int FromRange { get; set; }
        public int ToRange { get; set; }
        public decimal Price { get; set; }
        public int CreatedBy { get; set; }
        public int CustomerId { get; set; }
        public int DepartmentId { get; set; }
        public int ScopeId { get; set; }
        public int JobStatusId { get; set; }
        public int PricingTypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Designation { get; set; }
        public string CusShortName { get; set; }
        public int NumberofArtist { get; set; }
        public DateTime WEFromDate { get; set; }
        public DateTime WEToDate { get; set; }
        public int Id { get; set; }
        public IEnumerable<PricingWithScopeViewModel> AddCountDatas { get; set; }
    }

    public class PriceApproveVM
    {
        public int Id { get; set; }
        public int UId { get; set; }
        public int DepartmentId { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public int PricingTypeId { get; set; }
        public int Approve { get; set; }
        public IEnumerable<PriceApproveVM> GetCollection { get; set; }
    }
}

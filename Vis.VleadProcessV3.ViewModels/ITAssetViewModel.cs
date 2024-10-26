using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.ViewModels
{
    public class ITAssetViewModel
    { }

    public class tbl_ITAssetPara
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int ITAssetId { get; set; }
        public string BayNumber { get; set; }
        public string Location { get; set; }
        public string PcName { get; set; }
        public int HardwareId { get; set; }
        public string Monitor { get; set; }
        public string MonitorSerialNumber { get; set; }
        public string Keyboard { get; set; }
        public string KeyboardSerialNumber { get; set; }
        public string Roll { get; set; }
        public string Division { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string WarantyDetails { get; set; }
        public string Ram { get; set; }
        public string Processor { get; set; }
        public string Graphics { get; set; }
        public string Hdd { get; set; }
        public string TagNumber { get; set; }
        public string MacAddress { get; set; }
        public string Os { get; set; }
        public string IpAddress { get; set; }
        public string ServerType { get; set; }
        public Nullable<int> ServerTypeId { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string Mouse { get; set; }
        public string MouseSerialNumber { get; set; }
        public string WorkingStatus { get; set; }
        public Nullable<int> WorkingStatusId { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public int[] SoftwareId { get; set; }
        public int SoftwareStatusId { get; set; }
    }

    public class tbl_VendorDetailsPara
    {
        public int Id { get; set; }
        public string VendorName { get; set; }
        public string InvoiceNumber { get; set; }
        public System.DateTime InvoiceDate { get; set; }
        public decimal InvoiceValue { get; set; }
        public decimal PendingAmount { get; set; }
        public decimal AmountbePaid { get; set; }
        public decimal AmountPaid { get; set; }
        public int EmployeeId { get; set; }
    }

    public class tbl_BankDetailsPara
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public Nullable<System.DateTime> ClosingDate { get; set; }
        public decimal ClosingBalance { get; set; }
        public int EmployeeId { get; set; }
    }
}

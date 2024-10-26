using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.ViewModels
{
    public class InwardExcel
    {
        public int Id { get; set; }
        public DateTime? DateofReceived { get; set; }
        public string ClientName { get; set; }
        public string ClientJobId { get; set; }
        public string FileName { get; set; }
        public string JobStatusDescription { get; set; }
        public string Username { get; set; }
        public string SalesPersonName { get; set; }
        public string ClientSalesPerson { get; set; } //
        public string CustomerName { get; set; }
        public string Temp { get; set; }
        public string Style { get; set; }
        public string ProjectCode { get; set; }
        public string TeamCode { get; set; }
        public string SchoolName { get; set; }
        public string Ground { get; set; }
        public string Gender { get; set; }
        public string FileInwardMode { get; set; }
        public Nullable<bool> Status { get; set; }
        public DateTime FileReceivedDate { get; set; }
        public string JobDescription { get; set; }
        public int JobStatusId { get; set; }
        public int DepartmentId { get; set; }
        public int DivisionId { get; set; }//------------------------------------division id Job Order--------------------------------
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        //public string ClientJobId { get; set; }
        public string Remarks { get; set; }
        public string PoNo { get; set; }
        public int FileInwardTypeId { get; set; }
        public string Color { get; set; }
        public string LogoDimensionWidth { get; set; }
        public string LogoDimensionsLength { get; set; }
        public string ApparelLogoLocation { get; set; }
        public string ImprintColors1 { get; set; }
        public string ImprintColors2 { get; set; }
        public string ImprintColors3 { get; set; }
        public string VirtualProof { get; set; }
        public DateTime? DateofUpload { get; set; }
        public DateTime? DateofClose { get; set; }
        public string CustomerJobType { get; set; }
        public DateTime JobDate { get; set; }
        public int ClientOrderId { get; set; }
        //public IEnumerable<ClientOrderExt> UpdateJobtoClientOrder { get; set; }
        public IEnumerable<FileInwardExcel> ViewDatas { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? PODate { get; set; }
        public Nullable<int> CCId { get; set; }
        public string CCEmailId { get; set; }
        public Nullable<DateTime> DateofDelivery { get; set; }
        public IEnumerable<InwardExcel> GetAllValues { get; set; }
    }
    public class GetEmployee
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int EstTime { get; set; }
        public string Status { get; set; }
        public string EmployeenameWithCode { get; set; }
        public string ShiftName { get; set; }
        public int? WFMId { get; set; }
        public int? JId { get; set; }
    }
    public class GetCustomerCollectionVM
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}

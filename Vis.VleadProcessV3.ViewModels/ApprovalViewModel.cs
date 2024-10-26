namespace Vis.VleadProcessV3.ViewModels
{
    public partial class ApprovalViewModel
    {
        public Nullable<short> CreditDays { get; set; }
        public string Remarks { get; set; }
        public string ApprovalType { get; set; }
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }
        public Nullable<decimal> CreditLimit { get; set; }
    }
    public partial class CustomerFeedbackViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> CustomerContactId { get; set; }
    }
}

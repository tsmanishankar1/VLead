namespace Vis.VleadProcessV3.ViewModels
{
    public class JobMovementViewModel
    {
        public int ClientId { get; set; }
        public int DepartmentId { get; set; }
        public int TransactionId { get; set; }
        public DateTime? JobClosedUTC { get; set; } = null;
        public DateTime? DateofUpload { get; set; } = null;
        public string FileName { get; set; }
    }
}

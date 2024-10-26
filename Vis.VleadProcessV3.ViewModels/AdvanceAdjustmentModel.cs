namespace Vis.VleadProcessV3.ViewModels
{
    public class AdvanceAdjustmentModel
    {
        public IEnumerable<ReceivableAdjustViewModel> ReceivableAdjustments { get; set; }
        public int AdvanceId { get; set; }
        public int CreatedBy { get; set; }
        public IEnumerable<AlreadyAdjustedModel> AlreadyAdjusted { get; set; }
    }
    public class AdvanceAdjustmentModel1
    {
        public IEnumerable<ReceivableAdjustViewModel1> ReceivableAdjustments { get; set; }
        public int AdvanceId { get; set; }
      
    }
}

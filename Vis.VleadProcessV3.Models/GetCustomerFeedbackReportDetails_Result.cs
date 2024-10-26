using Microsoft.EntityFrameworkCore;

namespace Vis.VleadProcessV3.Models
{
    [Keyless]
    public class GetCustomerFeedbackReportDetails_Result
    {
        
        public string? CustomerName { get; set; }
        public string? ShortName { get; set; }
        public string? ContactName { get; set; }
        public int QualityOfService { get; set; }
        public int OnTimeDelivery { get; set; }
        public int AccountCommunication { get; set; }
        public int RecommendOthers { get; set; }
        public int RUSHTAT { get; set; }
        public string? Suggestions { get; set; }
        public DateTime CreatedUTC { get; set; }
    }
}

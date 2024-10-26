using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Vis.VleadProcessV3.Models
{
    public partial class TrainingFeedbackForm
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public DateTime? TrainingStartDate { get; set; }

        public DateTime? TrainingEndDate { get; set; }

        public int JobKnowledgeRating { get; set; }

        public string? JobKnowledgeComments { get; set; }

        public int ProductivityRating { get; set; }

        public string? ProductivityComments { get; set; }

        public int WorkQualityRating { get; set; }

        public string? WorkQualityComments { get; set; }

        public int TechnicalSkillsRating { get; set; }

        public string? TechnicalSkillsComments { get; set; }

        public int AttitudeRating { get; set; }

        public string? AttitudeComments { get; set; }

        public int CreativityRating { get; set; }

        public string? CreativityComments { get; set; }

        public int PunctualityRating { get; set; }

        public string? PunctualityComments { get; set; }

        public int AttendanceRating { get; set; }

        public string? AttendanceComments { get; set; }

        public int CommunicationSkillsRating { get; set; }

        public string? CommunicationSkillsComments { get; set; }

        public int OverallRating { get; set; }

        public string? OverallComments { get; set; }

        public int TotalWorkingDays { get; set; }

        public int InformedLeave { get; set; }

        public int UnInformedLeave { get; set; }

        public int TotalLeave { get; set; }

        public string? AtsComments { get; set; }

        public bool? EmployeeSignature { get; set; }

        public bool? ReviewersSignature { get; set; }

        public DateTime? Date { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? CreatedUTC { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime? UpdatedUTC { get; set; }

        public bool IsDeleted { get; set; }

        public bool? IsApproved { get; set; }

        public int ApprovedBy { get; set; }

        public DateTime? ApprovedUTC { get; set; }

        public bool? IsRejected { get; set; }

        public int RejectedBy { get; set; }

        public DateTime? RejectedUTC { get; set; }

        [JsonIgnore]
        public virtual Employee? Employee { get; set; }
    }

    public class ApproveFeedbackForm
    {
        public int FormId { get; set; }
        public bool? IsApproved { get; set; }
        public int ApprovedBy { get; set; }
    }
    public class RejectFeedbackForm
    {
        public int FormId { get; set; }
        public bool? IsRejected { get; set; }
        public int RejectedBy { get; set; }
    }
}
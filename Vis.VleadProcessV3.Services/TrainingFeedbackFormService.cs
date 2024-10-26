using System;
using System.Collections.Generic;
using System.Data.Entity;
using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.Services
{
    public class TrainingFeedbackFormService
    {
        private readonly ApplicationDbContext _context;
        public TrainingFeedbackFormService(ApplicationDbContext context)
        {
            _context = context;
        }
        public object GetEmployeeDetails(int employeeId)
        {
            var employeeDetails = _context.Employees
                                          .Where(e => e.EmployeeId == employeeId && e.IsDeleted == false)
                                          .Select(x => new
                                          {
                                              EmployeeId = x.EmployeeId,
                                              BatchNo = x.BatchNo,
                                              EmployeeName = x.EmployeeName,
                                              DateOfJoining = x.DateOfJoining,
                                              EmployeeCode = x.EmployeeCode
                                          })
                                          .FirstOrDefault();
            if (employeeDetails == null)
            {
                return new
                {
                    success = false,
                    message = "Employee Not Found"
                };
            }
            return new
            {
                success = true,
                result = employeeDetails
            };
        }
        public object GetTrainingEmployee()
        {
            var trainingEmployee = _context.Employees
                                           .Where(x => x.IsDeleted == false &&
                                                       (x.DateOfResignation == null || x.ResignReasons == null) &&
                                                       x.DepartmentId == 12)
                                           .OrderByDescending(x => x.EmployeeId)
                                           .Select(x => new
                                           {
                                               EmployeeId = x.EmployeeId,
                                               BatchNo = x.BatchNo,
                                               EmployeeName = x.EmployeeName,
                                               EmployeeCode = x.EmployeeCode,
                                               DateOfJoining = x.DateOfJoining
                                           })
                                           .ToList();

            return trainingEmployee;
        }
        public object GetTrainingDetails(int employeeId)
        {
            var employee = _context.Employees
                .Where(e => e.EmployeeId == employeeId && !e.IsDeleted)
                .Select(e => new
                {
                    e.BatchNo,
                    e.EmployeeName,
                    e.EmployeeCode,
                    e.DateOfJoining,
                    TrainingFeedback = _context.TrainingFeedbackForms
                        .Where(tf => tf.EmployeeId == employeeId && !tf.IsDeleted)
                        .Select(tf => new
                        {
                            tf.Id,
                            tf.EmployeeId,
                            tf.TrainingStartDate,
                            tf.TrainingEndDate,
                            tf.JobKnowledgeRating,
                            tf.JobKnowledgeComments,
                            tf.ProductivityRating,
                            tf.ProductivityComments,
                            tf.WorkQualityRating,
                            tf.WorkQualityComments,
                            tf.TechnicalSkillsRating,
                            tf.TechnicalSkillsComments,
                            tf.AttitudeRating,
                            tf.AttitudeComments,
                            tf.CreativityRating,
                            tf.CreativityComments,
                            tf.PunctualityRating,
                            tf.PunctualityComments,
                            tf.AttendanceRating,
                            tf.AttendanceComments,
                            tf.CommunicationSkillsRating,
                            tf.CommunicationSkillsComments,
                            tf.OverallRating,
                            tf.OverallComments,
                            tf.TotalWorkingDays,
                            tf.InformedLeave,
                            tf.UnInformedLeave,
                            tf.TotalLeave,
                            tf.AtsComments,
                            tf.EmployeeSignature,
                            tf.ReviewersSignature,
                            tf.Date,
                            tf.CreatedBy,
                            tf.CreatedUTC,
                            tf.UpdatedBy,
                            tf.UpdatedUTC,
                            tf.IsApproved,
                            tf.ApprovedUTC,
                            tf.IsRejected,
                            tf.RejectedUTC,
                            tf.IsDeleted
                        })
                        .ToList()
                })
                .FirstOrDefault();

            if (employee == null)
            {
                return new { success = false, message = "Employee Not Found" };
            }

            return new { success = true, result = employee };
        }
        public object CreateTrainingDetails(TrainingFeedbackForm trainingDetails)
        {
            var employee = _context.Employees
                                   .Where(e => e.EmployeeId == trainingDetails.EmployeeId && !e.IsDeleted)
                                   .FirstOrDefault();
            if (employee == null)
            {
                throw new ArgumentException("Employee not found.");
            }
            trainingDetails.CreatedUTC = DateTime.UtcNow;
            trainingDetails.UpdatedUTC = DateTime.UtcNow;
            trainingDetails.IsDeleted = false;
            _context.TrainingFeedbackForms.Add(trainingDetails);
            _context.SaveChanges();
            return new
            {
                success = true,
                message = "Training Details Created Successfully"
            };
        }
        public object ApproveTrainingFeedbackForm(ApproveFeedbackForm approveFeedbackForm)
        {
            var feedbackForm = _context.TrainingFeedbackForms
                                           .FirstOrDefault(f => f.Id == approveFeedbackForm.FormId && !f.IsDeleted);
            if (feedbackForm == null)
            {
                return new
                {
                    sucess = false,
                    message = "Training feedback form not found or already deleted."
                };
            }
            if (feedbackForm.IsApproved == true)
            {
                return new
                {
                    success = false,
                    message = "Training feedback form is already approved."
                };
            }
            if (feedbackForm.IsRejected == true)
            {
                return new
                {
                    success = false,
                    message = "Training feedback form is already rejected."
                };
            }
            feedbackForm.IsApproved = approveFeedbackForm.IsApproved ?? true;
            feedbackForm.ApprovedBy = approveFeedbackForm.ApprovedBy;
            feedbackForm.ApprovedUTC = DateTime.UtcNow;
            feedbackForm.UpdatedUTC = DateTime.UtcNow;
            feedbackForm.UpdatedBy = approveFeedbackForm.ApprovedBy;

            _context.TrainingFeedbackForms.Update(feedbackForm);
            _context.SaveChanges();

            return new
            {
                success = true,
                message = "Training feedback form approved successfully."
            };
        }
        public object RejectTrainingFeedbackForm(RejectFeedbackForm rejectFeedbackForm)
        {
            var feedbackForm = _context.TrainingFeedbackForms
                                       .FirstOrDefault(f => f.Id == rejectFeedbackForm.FormId && !f.IsDeleted);

            if (feedbackForm == null)
            {
                return new
                {
                    sucess = false,
                    message = "Training feedback form not found or already deleted."
                };
            }

            if (feedbackForm.IsRejected == true)
            {
                return new
                {
                    success = false,
                    message = "Training feedback form is already rejected"
                };
            }

            if (feedbackForm.IsApproved == true)
            {
                return new
                {
                    success = false,
                    message = "Training feedback form is already approved."
                };
            }

            feedbackForm.IsRejected = rejectFeedbackForm.IsRejected ?? true;
            feedbackForm.RejectedBy = rejectFeedbackForm.RejectedBy;
            feedbackForm.RejectedUTC = DateTime.UtcNow;
            feedbackForm.UpdatedUTC = DateTime.UtcNow;
            feedbackForm.UpdatedBy = rejectFeedbackForm.RejectedBy;

            _context.TrainingFeedbackForms.Update(feedbackForm);
            _context.SaveChanges();
            return new
            {
                success = true,
                message = "Training feedback form rejected successfully."
            };
        }
        public TrainingFeedbackForm UpdateTrainingDetails(TrainingFeedbackForm updatedDetails)
        {
            var existingDetails = _context.TrainingFeedbackForms
                                          .FirstOrDefault(ed => ed.Id == updatedDetails.Id && ed.IsDeleted == false);

            if (existingDetails == null)
            {
                throw new ArgumentException("Employee details not found or inactive.");
            }
            existingDetails.AtsComments = updatedDetails.AtsComments;
            existingDetails.AttendanceComments = updatedDetails.AttendanceComments;
            existingDetails.UnInformedLeave = updatedDetails.UnInformedLeave;
            existingDetails.InformedLeave = updatedDetails.InformedLeave;
            existingDetails.TotalLeave = updatedDetails.TotalLeave;
            existingDetails.AttendanceRating = updatedDetails.AttendanceRating;
            existingDetails.CommunicationSkillsComments = updatedDetails.CommunicationSkillsComments;
            existingDetails.CommunicationSkillsRating = updatedDetails.CommunicationSkillsRating;
            existingDetails.CreativityComments = updatedDetails.CreativityComments;
            existingDetails.CreativityRating = updatedDetails.CreativityRating;
            existingDetails.Date = updatedDetails.Date;
            existingDetails.TrainingEndDate = updatedDetails.TrainingEndDate;
            existingDetails.TrainingStartDate = updatedDetails.TrainingStartDate;
            existingDetails.JobKnowledgeComments = updatedDetails.JobKnowledgeComments;
            existingDetails.JobKnowledgeRating = updatedDetails.JobKnowledgeRating;
            existingDetails.OverallComments = updatedDetails.OverallComments;
            existingDetails.OverallRating = updatedDetails.OverallRating;
            existingDetails.ProductivityComments = updatedDetails.ProductivityComments;
            existingDetails.ProductivityRating = updatedDetails.ProductivityRating;
            existingDetails.PunctualityComments = updatedDetails.PunctualityComments;
            existingDetails.PunctualityRating = updatedDetails.PunctualityRating;
            existingDetails.ReviewersSignature = updatedDetails.ReviewersSignature;
            existingDetails.TechnicalSkillsComments = updatedDetails.TechnicalSkillsComments;
            existingDetails.TechnicalSkillsRating = updatedDetails.TechnicalSkillsRating;
            existingDetails.EmployeeSignature = updatedDetails.EmployeeSignature;
            existingDetails.TotalLeave = updatedDetails.TotalLeave;
            existingDetails.TotalWorkingDays = updatedDetails.TotalWorkingDays;
            existingDetails.WorkQualityComments = updatedDetails.WorkQualityComments;
            existingDetails.WorkQualityRating = updatedDetails.WorkQualityRating;
            existingDetails.JobKnowledgeRating = updatedDetails.JobKnowledgeRating;
            existingDetails.ProductivityRating = updatedDetails.ProductivityRating;
            existingDetails.UpdatedUTC = DateTime.UtcNow;
            existingDetails.UpdatedBy = updatedDetails.UpdatedBy;

            _context.SaveChanges();
            return existingDetails;
        }
        public void DeleteTrainingDetails(int Id)
        {
            var employeeDetail = _context.TrainingFeedbackForms
                                         .FirstOrDefault(ed => ed.Id == Id);

            if (employeeDetail == null)
            {
                throw new ArgumentException("Employee details not found.");
            }

            if (employeeDetail.IsDeleted)
            {
                throw new InvalidOperationException("Employee details already deleted.");
            }

            employeeDetail.IsDeleted = true;
            employeeDetail.UpdatedUTC = DateTime.UtcNow;
            _context.TrainingFeedbackForms.Update(employeeDetail);
            _context.SaveChanges();
        }
    }
}

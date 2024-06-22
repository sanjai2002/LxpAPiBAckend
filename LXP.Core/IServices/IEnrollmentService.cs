﻿using LXP.Common.ViewModels;

namespace LXP.Core.IServices
{
    public interface IEnrollmentService
    {
        Task<bool> Addenroll(EnrollmentViewModel enrollment);

        object GetCourseandTopicsByLearnerId(Guid learnerId);

        public IEnumerable<EnrollmentReportViewModel> GetEnrollmentsReport();
        public IEnumerable<EnrolledUserViewModel> GetEnrolledUsers(Guid courseId);
        public IEnumerable<EnrollmentReportViewModel> GetEnrolledInprogressLearnerbyCourseId(
            Guid courseId
        );
        public IEnumerable<EnrollmentReportViewModel> GetEnrolledCompletedLearnerbyCourseId(
            Guid courseId
        );

        Task<bool> DeleteEnrollment(Guid enrollmentId);

        object GetCourseandTopicsByCourseId(Guid courseId, Guid learnerId); //2106
    }
}

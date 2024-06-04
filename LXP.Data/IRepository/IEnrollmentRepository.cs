using LXP.Common.Entities;
using LXP.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.IRepository
{
    public interface IEnrollmentRepository
    {
        Task Addenroll(Enrollment enrollment);

        bool AnyEnrollmentByLearnerAndCourse(Guid learnerId, Guid courseId);

        object GetCourseandTopicsByLearnerId(Guid learnerId);
        public IEnumerable<EnrollmentReportViewModel> GetEnrollmentReport();
        public IEnumerable<EnrolledUserViewModel> GetEnrolledUser(Guid courseId);
        public IEnumerable<EnrollmentReportViewModel> GetEnrolledInprogressLearnerbyCourseId(Guid courseId);
        public IEnumerable<EnrollmentReportViewModel> GetEnrolledCompletedLearnerbyCourseId(Guid courseId);

    }
}
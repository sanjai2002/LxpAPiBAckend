using LXP.Common.Entities;

namespace LXP.Data.IRepository
{
    public interface IEnrollmentRepository
    {
        Task Addenroll(Enrollment enrollment);

        bool AnyEnrollmentByLearnerAndCourse(Guid learnerId, Guid courseId);

        object GetCourseandTopicsByLearnerId(Guid learnerId);
    }
}
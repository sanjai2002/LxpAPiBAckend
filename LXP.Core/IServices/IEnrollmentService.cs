using LXP.Common.ViewModels;

namespace LXP.Core.IServices
{
    public interface IEnrollmentService
    {
        Task<bool> Addenroll(EnrollmentViewModel enrollment);

        object GetCourseandTopicsByLearnerId(Guid learnerId);


    }
}
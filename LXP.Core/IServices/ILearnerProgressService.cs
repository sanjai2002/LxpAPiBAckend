using LXP.Common.Entities;
using LXP.Common.ViewModels;

namespace LXP.Core.IServices
{
    public interface ILearnerProgressService
    {
        Task<bool> Progress(LearnerProgressViewModel learnerProgress);
        Task<bool> materialCompletion(Guid learnerId, Guid materialId);
        Task<bool> UpdateProgress(Guid learnerId, Guid materialId, TimeOnly watchtime);

        //Task<double> materialCompletionPercentage(Guid learnerId, Guid materialId);

        Task<double> TopicTotalTime(Guid topicId);
        Task<double> CourseTotalTime(Guid courseId);

        // Task<LearnerProgressViewModel> GetProgressById(Guid learnerProgressId);

        Task<double> TopicWatchTime(Guid topicId, Guid learnerId);
        Task<double> CourseWatchTime(Guid courseId, Guid learnerId);

        //Task CalculateAndUpdateCourseCompletionAsync(Guid learnerId);

        Task<decimal?> GetCourseCompletionPercentageAsync(Guid learnerId, Guid enrollmentId);
        Task<LearnerProgress> GetLearnerProgressByLearnerIdAndMaterialId(
            string LearnerId,
            string MaterialId
        );

        Task<bool> LearnerProgress(ProgressViewModel learnerProgress);
    }
    //public interface ILearnerProgressService
    //{
    //    Task Progress(LearnerProgressViewModel learnerProgress);
    //    Task<double> materialCompletion(Guid learnerId, Guid materialId);
    //    Task<double> materialWatchTime(Guid learnerId, Guid materialId, TimeOnly watchTime);
    //    Task<double> materialCompletionPercentage(Guid learnerId, Guid materialId);

    //    Task<double> TopicTotalTime(Guid topicId);
    //    Task<double> CourseTotalTime(Guid courseId);
    //    // Task<LearnerProgressViewModel> GetProgressById(Guid learnerProgressId);

    //    Task<double> TopicWatchTime(Guid topicId, Guid learnerId);
    //    Task<double> CourseWatchTime(Guid courseId, Guid learnerId);
    //}
}

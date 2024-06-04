using LXP.Common.Entities;
using LXP.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.IServices
{
    public interface ILearnerProgressService
    {
        Task Progress(LearnerProgressViewModel learnerProgress);
        Task<double> materialCompletion(Guid learnerId,Guid materialId);
        Task<double> materialWatchTime(Guid learnerId,Guid materialId,TimeOnly watchTime);
        Task<double> materialCompletionPercentage(Guid learnerId, Guid materialId);

        Task<double> TopicTotalTime(Guid topicId);
        Task<double> CourseTotalTime(Guid courseId);
        // Task<LearnerProgressViewModel> GetProgressById(Guid learnerProgressId);

        Task<double> TopicWatchTime(Guid topicId, Guid learnerId);
        Task<double> CourseWatchTime(Guid courseId, Guid learnerId);
    }
}

using LXP.Common.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.IRepository
{
    public interface ILearnerProgressRepository
    {
        Task LearnerProgress(LearnerProgress learnerProgress);

        Task<LearnerProgress> GetLearnerProgressById(Guid learnerId, Guid courseId);
        Task<LearnerProgress> GetLearnerProgressByMaterialId(Guid learnerId, Guid materialId);

        void UpdateLearnerProgress(LearnerProgress progress);
        Task<LearnerProgress> GetLearnerProgressByLearnerIdAndMaterialId(Guid LearnerId, Guid MaterialId);

        Task<List<LearnerProgress>> GetMaterialByTopic(Guid topicId, Guid learnerId);
        Task CalculateAndUpdateCourseCompletionAsync(Guid learnerId);

        Task<Enrollment> GetEnrollmentByIdAsync(Guid learnerId, Guid enrollmentId);

        Task<bool> AnyLearnerProgressByLearnerIdAndMaterialId(Guid LearnerId, Guid MaterialId);
    }
}
using LXP.Common.Entities;
using LXP.Data.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
//using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.Repository
{
    public class LearnerProgressRepository : ILearnerProgressRepository
    {
        private readonly LXPDbContext _context;
       public LearnerProgressRepository(LXPDbContext context)
        {
            _context = context;
        }

        public async Task LearnerProgress(LearnerProgress learnerProgress)
        {
          await _context.LearnerProgresses.AddAsync(learnerProgress);
          await _context.SaveChangesAsync();
        }

       public async Task<LearnerProgress> GetLearnerProgressById(Guid learnerId, Guid courseId)
        {
            return await _context.LearnerProgresses.FirstOrDefaultAsync(progress=>progress.LearnerId==learnerId && progress.CourseId==courseId);

        }
        public async Task<LearnerProgress> GetLearnerProgressByMaterialId(Guid learnerId, Guid materialId)
        {
            return await _context.LearnerProgresses.FirstOrDefaultAsync(progress => progress.LearnerId == learnerId && progress.MaterialId == materialId);

        }

        public void UpdateLearnerProgress(LearnerProgress progress)
        {
             _context.LearnerProgresses.Update(progress);
             _context.SaveChanges();
        }
       public async Task<List<LearnerProgress>> GetMaterialByTopic(Guid topicId, Guid learnerId)
        {
            return await _context.LearnerProgresses.Where(material=>material.TopicId==topicId && material.LearnerId== learnerId).ToListAsync();
        }
    }
}

using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.Repository
{
    public class LearnerDashboardRepository :  ILearnerDashboardRepository
    {
        private readonly LXPDbContext _lXPDbContext;
        

        public LearnerDashboardRepository(LXPDbContext lXPDbContext)
        {
            _lXPDbContext = lXPDbContext;
        }

        public List<Enrollment> GetLearnerCompletedCount(Guid learnerId)
        {
            return _lXPDbContext.Enrollments.Where(e => e.LearnerId == learnerId && e.CompletedStatus == 1).ToList();
        }

        public List<Enrollment> GetLearnerenrolledCourseCount(Guid learnerId)
        {
            return _lXPDbContext.Enrollments.Where(e => e.LearnerId == learnerId).ToList();
        }

        public List<Enrollment> GetLearnerDashboardInProgressCount(Guid learnerId)
        {
  

            return _lXPDbContext.Enrollments.Where(e => e.LearnerId == learnerId && e.CompletedStatus == 0).ToList();
        }

      
    }
}

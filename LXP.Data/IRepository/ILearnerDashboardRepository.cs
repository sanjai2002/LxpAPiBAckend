﻿using LXP.Common.Entities;

namespace LXP.Data.IRepository
{
    public interface ILearnerDashboardRepository
    {
        public List<Enrollment> GetLearnerenrolledCourseCount(Guid learnerId);
        public List<Enrollment> GetLearnerCompletedCount(Guid learnerId);
        public List<Enrollment> GetLearnerDashboardInProgressCount(Guid learnerId);
    }
}

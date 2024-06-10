using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using LXP.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.Services
{
    public class LearnerDashboardService:ILearnerDashboardService
    {
        private readonly ILearnerDashboardRepository _learnerDashboardRepository;



        public LearnerDashboardService(ILearnerDashboardRepository learnerDashboardRepository)
        {
            _learnerDashboardRepository = learnerDashboardRepository;
        }

        public LearnerDashboardCourseCountViewModel GetLearnerDashboardDetails(Guid learnerId)
        {
            var dashboarddetails = new LearnerDashboardCourseCountViewModel
            {
                CompletedCount = _learnerDashboardRepository.GetLearnerCompletedCount(learnerId).Count(),
                EnrolledCourseCount = _learnerDashboardRepository.GetLearnerenrolledCourseCount(learnerId).Count(),
                InProgressCount = _learnerDashboardRepository.GetLearnerDashboardInProgressCount(learnerId).Count(),

            };

            return dashboarddetails;
        }
    }
}

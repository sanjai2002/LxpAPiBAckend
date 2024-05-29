using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Data.DBContexts;
using LXP.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.Repository
{
    public class UserReportRepository : IUserReportRepository
    {
        private readonly LXPDbContext _lXPDbContext;
        
        public UserReportRepository(LXPDbContext lXPDbContext)
        {
            _lXPDbContext = lXPDbContext;
        }
        public IEnumerable<UserReportViewModel> GetUserReport()
        {
            //var learnerId = _lXPDbContext.Enrollments
            //   .GroupBy(e => e.LearnerId)
            //   .Select(g => g.Key)
            //   .ToList();


            var query = _lXPDbContext.Enrollments
                .GroupBy(e => e.LearnerId)
                    .Select(grouped => new UserReportViewModel
                    {
                       
                        EnrolledCourse = grouped.Count(),
                        CompletedCourse = grouped.Count(x => x.CompletedStatus == 1),


                    }); ;

            var userReports = query.ToList();
            return userReports;

        }
    }
}

using LXP.Data.DBContexts;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.Repository
{
    public  class LearnerRepository:ILearnerRepository
    {
        private readonly LXPDbContext _lXPDbContext;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;
        public LearnerRepository(LXPDbContext lXPDbContext, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _lXPDbContext = lXPDbContext;
            _environment = environment;
            _contextAccessor = httpContextAccessor;
        }

        public object GetAllLearnerDetailsByLearnerId(Guid learnerid)
        {
            var result = from learner in _lXPDbContext.Learners
                         where learner.LearnerId == learnerid
                         select new
                         {
                             LearnerEmail = learner.Email,
                             LearnerLastlogin=learner.UserLastLogin,
                             learnerprofile = (from learnerprofile in _lXPDbContext.LearnerProfiles
                                       where learnerprofile.LearnerId == learner.LearnerId 
                                       select new
                                       {
                                           LearnerFirstName = learnerprofile.FirstName,
                                           LearnerLastName = learnerprofile.LastName,
                                           LearnerDob = learnerprofile.Dob,
                                           LearnerGender = learnerprofile.Gender,
                                           LearnerContactNumber = learnerprofile.ContactNumber,
                                           LearnerStream = learnerprofile.Stream,
                                           Learnerprofile = String.Format("{0}://{1}{2}/wwwroot/LearnerProfileImages/{3}",
                                                            _contextAccessor.HttpContext.Request.Scheme,
                                                            _contextAccessor.HttpContext.Request.Host,
                                                            _contextAccessor.HttpContext.Request.PathBase,
                                                            learnerprofile.ProfilePhoto)
                                       }).ToList()
                         };
            return result;
        }

        //GetLearnerEntrolledcourseByLearnerId

        public object GetLearnerEnrolledcourseByLearnerId(Guid learnerid)
        {
            var result = from enrollment in _lXPDbContext.Enrollments
                         where enrollment.LearnerId == learnerid
                         orderby enrollment.EnrollmentDate descending
                         select new
                         {
                             Enrollmentid = enrollment.EnrollmentId,
                             Enrolledcourse = enrollment.Course.Title,
                             EnrolledCourseCategory = enrollment.Course.Category.Category,
                             EnrolledCourselevels = enrollment.Course.Level.Level,
                             Enrollmentdate = enrollment.EnrollmentDate,
                         };
            return result;
        }


      

    }
}

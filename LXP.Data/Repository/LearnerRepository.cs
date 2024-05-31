using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LXP.Data.IRepository;
using System.Threading.Tasks;
using LXP.Common.Entities;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using LXP.Common.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

namespace LXP.Data.Repository
{
    public class LearnerRepository : ILearnerRepository
    {
        private readonly LXPDbContext _lXPDbContext;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;
        public LearnerRepository(LXPDbContext context, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor) {

            _lXPDbContext = context;
            _environment = environment;
            _contextAccessor = httpContextAccessor;
        }

        public void AddLearner(Learner learner) {

            _lXPDbContext.Learners.Add(learner);


            _lXPDbContext.SaveChanges();


        }
        //public Task<bool> AnyLearnerByEmail(string email)
        //{
        //    return _lXPDbContext.Learners.AnyAsync(learner=>learner.Email==email);
        //}



        public async Task<bool> AnyLearnerByEmail(string email)
        {
            return  _lXPDbContext.Learners.Any(l => l.Email == email);
        }


        public Learner GetLearnerByLearnerEmail(string email)
        {
            return _lXPDbContext.Learners.FirstOrDefault(learner => learner.Email == email);
        }

        public async Task<List<Learner>> GetAllLearner()
        {
            return _lXPDbContext.Learners.ToList();
        }

        public Learner GetLearnerDetailsByLearnerId(Guid LearnerId)

        {

            return _lXPDbContext.Learners.Find(LearnerId);


        }

        public async Task UpdateLearner(Learner learner)
        {
            _lXPDbContext.Learners.Update(learner);
            await _lXPDbContext.SaveChangesAsync();
        }

        public IEnumerable<AllLearnersViewModel> GetLearners()
        {
            return _lXPDbContext.LearnerProfiles
                           .Select(c => new AllLearnersViewModel
                           {

                               LearnerID = c.LearnerId,
                               LearnerName = c.FirstName + " " + c.LastName,
                               Email = c.Learner.Email,
                               LastLogin = c.Learner.UserLastLogin,
                           }

           ).ToList();
        }

   
        public object GetAllLearnerDetailsByLearnerId(Guid learnerid)
        {
            var result = from learner in _lXPDbContext.Learners
                         where learner.LearnerId == learnerid
                         select new
                         {
                             LearnerEmail = learner.Email,
                             LearnerLastlogin = learner.UserLastLogin,
                             LearnerFirstName = learner.LearnerProfiles.First().FirstName,
                             LearnerLastName = learner.LearnerProfiles.First().LastName,
                             LearnerDob = learner.LearnerProfiles.First().Dob,
                             LearnerGender = learner.LearnerProfiles.First().Gender,
                             LearnerContactNumber = learner.LearnerProfiles.First().ContactNumber,
                             LearnerStream = learner.LearnerProfiles.First().Stream,
                             Learnerprofile = String.Format("{0}://{1}{2}/wwwroot/LearnerProfileImages/{3}",
                                                                                _contextAccessor.HttpContext!.Request.Scheme,
                                                                                _contextAccessor.HttpContext.Request.Host,
                                                                                _contextAccessor.HttpContext.Request.PathBase,
                                                                                learner.LearnerProfiles.First().ProfilePhoto)


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


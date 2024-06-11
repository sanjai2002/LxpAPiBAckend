using LXP.Common.Entities;
using LXP.Data.IRepository;
using System;
using System.Collections.Generic;                                                                      
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using LXP.Common.ViewModels;

namespace LXP.Data.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly LXPDbContext _lXPDbContext;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;
        public CourseRepository(LXPDbContext lXPDbContext, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _lXPDbContext = lXPDbContext;
            _environment = environment;
            _contextAccessor = httpContextAccessor;
        }
        public Course GetCourseDetailsByCourseName(string courseName)
        {
            return _lXPDbContext.Courses.Include(course => course.Level).Include(course => course.Category).FirstOrDefault(course => course.Title == courseName);
        }
        public void AddCourse(Course course)
        {
            _lXPDbContext.Courses.Add(course);
            _lXPDbContext.SaveChanges();
        }
        public bool AnyCourseByCourseTitle(string courseTitle)
        {
            return _lXPDbContext.Courses.Any(course => course.Title == courseTitle);
        }
        public Course GetCourseDetailsByCourseId(Guid CourseId)
        {
            return _lXPDbContext.Courses.Include(course => course.Level).Include(course => course.Category).FirstOrDefault(course => course.CourseId == CourseId);
        }

        public Course FindCourseid(Guid courseid)
        {
            return _lXPDbContext.Courses.Find(courseid);

        }

        public Enrollment FindEntrollmentcourse(Guid Courseid)
        {
            return _lXPDbContext.Enrollments.FirstOrDefault(Course => Course.CourseId == Courseid);
        }

        public async Task Deletecourse(Course course)
        {
            _lXPDbContext.Courses.Remove(course);
            await _lXPDbContext.SaveChangesAsync();
        }

        public async Task Changecoursestatus(Course course)
        {
            _lXPDbContext.Courses.Update(course);
            await _lXPDbContext.SaveChangesAsync();
        }


        public async Task Updatecourse(Course course)
        {
            _lXPDbContext.Courses.Update(course);
            await _lXPDbContext.SaveChangesAsync();
        }


        public IEnumerable<CourseDetailsViewModel> GetAllCourse()
        {
            return _lXPDbContext.Courses
                      .Select(c => new CourseDetailsViewModel
                      {
                          CourseId = c.CourseId,
                          Status = c.IsAvailable,
                          Title = c.Title,
                          Level = c.Level.Level,
                          Category = c.Category.Category,
                          Duration = c.Duration,
                          Description = c.Description,
                          CreatedAt = c.CreatedAt,
                          CategoryId = c.CategoryId,
                          LevelId = c.LevelId,
                          Thumbnailimage = String.Format("{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",

                                           _contextAccessor.HttpContext.Request.Scheme,
                                           _contextAccessor.HttpContext.Request.Host,
                                           _contextAccessor.HttpContext.Request.PathBase, c.Thumbnail),
                          ModifiedAt = c.ModifiedAt.ToString(),
                      })
                      .ToList();

        }

        public IEnumerable<CourseDetailsViewModel> GetLimitedCourse()
        {
            return _lXPDbContext.Courses
              .OrderByDescending(c => c.CreatedAt)
              .Select(c => new CourseDetailsViewModel
              {
                  CourseId = c.CourseId,
                  Title = c.Title,
                  Level = c.Level.Level,
                  Category = c.Category.Category,
                  Duration = c.Duration,
                  Thumbnailimage = String.Format("{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                             _contextAccessor.HttpContext.Request.Scheme,
                             _contextAccessor.HttpContext.Request.Host,
                             _contextAccessor.HttpContext.Request.PathBase,
                             c.Thumbnail),
                  CreatedAt = c.CreatedAt,
              })
              .Take(9)
              .ToList();
        }

        public IEnumerable<CourseListViewModel> GetAllCourseDetails()
        {
            return _lXPDbContext.Courses
               .Select(c => new CourseListViewModel
               {
                   CourseId = c.CourseId,
                   Title = c.Title,
                   Description = c.Description,
                   Level = c.Level.Level,
                   Category = c.Category.Category,
                   Duration = c.Duration,
                   CreatedAt = c.CreatedAt,
                   CreatedBy = c.CreatedBy,
                   Thumbnailimage = String.Format("{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                                                    _contextAccessor.HttpContext.Request.Scheme,
                                                    _contextAccessor.HttpContext.Request.Host,
                                                    _contextAccessor.HttpContext.Request.PathBase,
                                                    c.Thumbnail),


               })
             .ToList();


        }


        public async Task<dynamic> GetAllCourseDetailsByLearnerId(Guid learnerId)
        {

            var query = from course in _lXPDbContext.Courses
                        join enrollment in _lXPDbContext.Enrollments
                        on new { course.CourseId, LearnerId = learnerId } equals new { enrollment.CourseId, enrollment.LearnerId }
                        into enrollments
                        from enrollment in enrollments.DefaultIfEmpty()
                        where course.IsAvailable && course.IsActive
                        orderby course.CourseId, enrollment.EnrollmentId
                        select new
                        {

                            CourseId = course.CourseId,
                            Catagory = course.Category.Category,
                            Level = course.Level.Level,
                            Title = course.Title,
                            Description = course.Description,
                            Duration = course.Duration,
                            Thumbnailimage = String.Format("{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",  //name changed
                                             _contextAccessor.HttpContext.Request.Scheme,
                                             _contextAccessor.HttpContext.Request.Host,
                                             _contextAccessor.HttpContext.Request.PathBase,
                                             course.Thumbnail),
                            CreatedBy = "Admin",
                            CreatedAt = new DateTime(),
                            IsActive = true,
                            IsAvailable = true,
                            ModifiedAt = new DateTime(),
                            ModifiedBy = "Admin",


                            EnrollStatus = enrollment.EnrollStatus == null ? false : enrollment.EnrollStatus,

                        };

            //var query = from course in _lXPDbContext.Courses
            //            join enrollment in _lXPDbContext.Enrollments
            //            on new { course.CourseId, LearnerId = learnerId } equals new { enrollment.CourseId, enrollment.LearnerId }
            //            into enrollments
            //            from enrollment in enrollments.DefaultIfEmpty()
            //            where course.IsAvailable && course.IsActive
            //            orderby course.CourseId, enrollment?.EnrollmentId ?? 0 // Handle nullable EnrollmentId
            //            select new
            //            {
            //                Course = course,
            //                EnrollmentId = enrollment?.EnrollmentId ?? 0, // Handle nullable EnrollmentId
            //                LearnerId = enrollment.LearnerId,
            //                EnrollmentDate = enrollment.EnrollmentDate,
            //                EnrollStatus = enrollment.EnrollStatus,
            //                EnrollRequestStatus = enrollment.EnrollRequestStatus,
            //                EnrollmentCreatedBy = enrollment.CreatedBy,
            //                EnrollmentCreatedAt = enrollment.CreatedAt,
            //                EnrollmentModifiedBy = enrollment.ModifiedBy,
            //                EnrollmentModifiedAt = enrollment.ModifiedAt                                          // Other fields...
            //            };

            //return query.ToList();


            // Execute the query or further manipulate the results as needed
            return query.ToList();

        }

    }

}

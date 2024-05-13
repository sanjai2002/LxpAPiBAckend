using LXP.Common;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Data.Entity;
using Microsoft.AspNetCore.Http;

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
        public Course GetCourseDetailsByCourseId(Guid CourseId)
        {
            return _lXPDbContext.Courses.Find(CourseId);
        }


        //public void Update(Course course)
        //{
        //    _lXPDbContext.Entry(course).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //    _lXPDbContext.SaveChanges();
        //}

        //   public void Update(CourseUpdateModel courseupdate)
        //   {
        //    Course course = _lXPDbContext.Courses.Find(courseupdate.CourseId);
        //    var uniqueFileName = $"{Guid.NewGuid()}_{courseupdate.Thumbnailimage.FileName}";
        //    var uploadsFolder = Path.Combine(_environment.WebRootPath, "CourseThumbnailImages"); // Use WebRootPath
        //    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        courseupdate.Thumbnailimage.CopyTo(stream); // Use await
        //    }

        //    course!.Title = courseupdate.Title;
        //    course.Description = courseupdate.Description;
        //    course.Duration = courseupdate.Duration;
        //    course.Thumbnail = uniqueFileName;
        //    course.ModifiedBy = courseupdate.ModifiedBy;
        //    course.ModifiedAt=DateTime.Now;

        //    _lXPDbContext.Courses.Update(course);
        //    _lXPDbContext.SaveChangesAsync();
        //}



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

        public IEnumerable<CourseViewModel> GetAllCourse()
        {
            return _lXPDbContext.Courses
                      .Select(c => new CourseViewModel
                      {
                          CourseId = c.CourseId,
                          Title = c.Title,
                          Level = c.Level.Level,
                          Category = c.Catagory.Category,
                          Duration = c.Duration,
                          CreatedAt = c.CreatedAt,
                      })

                      .ToList();

        }

        public IEnumerable<CourseViewModel> GetLimitedCourse()
        {
            return _lXPDbContext.Courses
              .OrderByDescending(c => c.CreatedAt)
              .Select(c => new CourseViewModel
              {
                  CourseId = c.CourseId,
                  Title = c.Title,
                  Level = c.Level.Level,
                  Category = c.Catagory.Category,
                  Duration = c.Duration,
                  Thumbnailimage = String.Format("{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                             _contextAccessor.HttpContext.Request.Scheme,
                             _contextAccessor.HttpContext.Request.Host,
                             _contextAccessor.HttpContext.Request.PathBase,
                             c.Thumbnail)

              })
              .Take(9)
              .ToList();
        }




        //public IEnumerable<CourseUpdateModel> GetIndividualCourse(Guid id)
        //{
        //    return _lXPDbContext.Courses
        //                  .Select(x => new CourseUpdateModel
        //                  {
        //                      CourseId = id,
        //                      Title = x.Title,
        //                  })
        //                  .ToList();
        //}

        //public CourseUpdateModel GetById(Guid courseId)
        //{
        //    return _lXPDbContext.Courses.
        //        Find(courseId)!;
        //}

        //public async Task<int> UpdateCourse(CourseUpdateModel courseupdate)
        //{
        //    Course course = _lXPDbContext.Courses.Find(courseupdate.CourseId);
        //    course.Title = courseupdate.Title;
        //    _lXPDbContext.Courses.Update(course);
        //    return await _lXPDbContext.SaveChangesAsync();
        //}

        //public Course FindCourseid(Guid course)
        //{
        //     return _lXPDbContext.Courses.Find(course);
        //}


    }
}

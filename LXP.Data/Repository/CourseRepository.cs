using LXP.Common;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Entity;

namespace LXP.Data.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly LXPDbContext _lXPDbContext;
        private readonly IWebHostEnvironment _environment;
        public CourseRepository(LXPDbContext lXPDbContext, IWebHostEnvironment environment)
        {
            _lXPDbContext = lXPDbContext;
            _environment = environment;
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

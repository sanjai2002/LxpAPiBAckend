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




       

    }
}

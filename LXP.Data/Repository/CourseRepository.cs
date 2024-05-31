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
            this._lXPDbContext = lXPDbContext;
            _environment = environment;
            _contextAccessor = httpContextAccessor;
        }
        public Course GetCourseDetailsByCourseId(Guid CourseId)
        {
            return _lXPDbContext.Courses.Find(CourseId);


        }
        public async Task AddCourse(Course course)
        {
            await _lXPDbContext.Courses.AddAsync(course);
            await _lXPDbContext.SaveChangesAsync();
        }
        public bool AnyCourseByCourseTitle(string CourseTitle)
        {
            return _lXPDbContext.Courses.Any(course => course.Title == CourseTitle);
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

    }

}

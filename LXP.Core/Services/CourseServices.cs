using LXP.Common;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using LXP.Data.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using static Mysqlx.Notice.Warning.Types;

namespace LXP.Core.Services
{

    public class CourseServices : ICourseServices
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;
        public CourseServices(ICourseRepository courseRepository, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _courseRepository = courseRepository; ;
            _environment = environment;
            _contextAccessor = httpContextAccessor;

        }

        public Course GetCourseByCourseId(Guid courseId)
        {
            var course = _courseRepository.GetCourseDetailsByCourseId(courseId);
            var courseView = new Course
            {
                CourseId = courseId,
                LevelId = course.LevelId,
                CatagoryId = course.CatagoryId,
                Title = course.Title,
                Description = course.Description,
                Duration = course.Duration,
                Thumbnail = String.Format("{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                                             _contextAccessor.HttpContext.Request.Scheme,
                                             _contextAccessor.HttpContext.Request.Host,
                                             _contextAccessor.HttpContext.Request.PathBase,
                                             course.Thumbnail)
            };

            return courseView;

        }


        //public void UpdateCourseusingid(CourseUpdateModel courseupdate)
        //{

        //    if (courseupdate == null)
        //    {
        //        throw new ArgumentNullException(nameof(courseupdate));
        //    }
        //    //var uniqueFileName = $"{Guid.NewGuid()}_{courseupdate.Thumbnailimage.FileName}";
        //    ////    // Save the image to a designated folder (e.g., wwwroot/images)
        //    //var uploadsFolder = Path.Combine(_environment.WebRootPath, "CourseThumbnailImages"); // Use WebRootPath
        //    //var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //    //using (var stream = new FileStream(filePath, FileMode.Create))
        //    //{
        //    //    courseupdate.Thumbnailimage.CopyTo(stream); // Use await
        //    //}
        //    //CourseUpdateModel updatecourse = new CourseUpdateModel
        //    //{
        //    //    CourseId = courseupdate.CourseId,
        //    //    CatagoryId = courseupdate.CatagoryId,
        //    //    LevelId = courseupdate.LevelId,
        //    //    Title = courseupdate.Title,
        //    //    Description = courseupdate.Description,
        //    //    Duration = courseupdate.Duration,
        //    //    ModifiedBy = courseupdate.ModifiedBy
        //    //};
        //    _courseRepository.Update(courseupdate);
        //}


        public async Task <bool> Deletecourse(Guid courseid)
        {
            var Course = _courseRepository.FindCourseid(courseid);
            if (Course != null)
            {
                var Enrollentcourse = _courseRepository.FindEntrollmentcourse(courseid);
                if (Enrollentcourse == null)
                {
                    _courseRepository.Deletecourse(Course);
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> Changecoursestatus(Coursestatus courseStatus)
        {
            var course =  _courseRepository.FindCourseid(courseStatus.CourseId);
            if (course != null)
            {
                course.IsAvailable = courseStatus.IsAvailable;
                await _courseRepository.Changecoursestatus(course);
                return true;
            }
            return false;
        }

        
        public async Task<bool> Updatecourse(CourseUpdateModel courseupdate)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{courseupdate.Thumbnailimage.FileName}";
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "CourseThumbnailImages"); // Use WebRootPath
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                courseupdate.Thumbnailimage.CopyTo(stream); 
            }

            var course = _courseRepository.FindCourseid(courseupdate.CourseId);
            if (course != null)
            {

                course!.Title = courseupdate.Title;
                course.Description = courseupdate.Description;
                course.Duration = courseupdate.Duration;
                course.Thumbnail = uniqueFileName;
                course.ModifiedBy = courseupdate.ModifiedBy;
                course.ModifiedAt = DateTime.Now;
                await _courseRepository.Updatecourse(course);
                return true;
            }
            return false;
        }






    }
}

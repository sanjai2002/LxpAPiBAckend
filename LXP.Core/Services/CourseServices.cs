using LXP.Common.ViewModels;
using LXP.Common.Entities;
using LXP.Core.IServices;

using LXP.Data.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using LXP.Data.Repository;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LXP.Core.Services
{
    public class CourseServices : ICourseServices
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICourseLevelRepository _courseLevelRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;
        

        public CourseServices(ICourseRepository courseRepository,ICategoryRepository categoryRepository,ICourseLevelRepository courseLevelRepository,IWebHostEnvironment environment,IHttpContextAccessor httpContextAccessor) 
        {
            _courseRepository = courseRepository;
            _courseLevelRepository = courseLevelRepository;
            _categoryRepository = categoryRepository;
            _environment = environment;
            _contextAccessor = httpContextAccessor;

        }
        public CourseListViewModel AddCourse(CourseViewModel course)
        {
            bool isCourseExists = _courseRepository.AnyCourseByCourseTitle(course.Title);

            if (!isCourseExists)
            {
                

                Guid levelId= Guid.Parse(course.Level);
                CourseLevel level = _courseLevelRepository.GetCourseLevelByCourseLevelId(levelId);
                Guid categoryId = Guid.Parse(course.Catagory);
                CourseCategory category = _categoryRepository.GetCategoryByCategoryId(categoryId);

                // Generate a unique file name
                var uniqueFileName = $"{Guid.NewGuid()}_{course.Thumbnailimage.FileName}";

                // Save the image to a designated folder (e.g., wwwroot/images)
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "CourseThumbnailImages"); // Use WebRootPath
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    course.Thumbnailimage.CopyTo(stream); // Use await
                }

                Course newCourse = new Course
                {
                    CourseId = Guid.NewGuid(),
                    Catagory = category,
                    Level = level,
                    Title = course.Title,
                    Description = course.Description,
                    Duration = course.Duration,
                    Thumbnail = uniqueFileName,
                    CreatedBy = course.CreatedBy,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    IsAvailable = true,
                    ModifiedAt = null,
                    ModifiedBy = null


                };
                _courseRepository.AddCourse(newCourse);

                return GetCourseDetailsByCourseName(newCourse.Title);
            }
            else
            {
                return null;
            }
        }

        public async Task<CourseListViewModel> GetCourseDetailsByCourseId(string courseId)
        {
            Course course = _courseRepository.GetCourseDetailsByCourseId(Guid.Parse(courseId));
             
            CourseListViewModel courseDetails = new CourseListViewModel()
            {
               CourseId = course.CourseId,
               Title = course.Title,
               Description = course.Description,
               Catagory=course.Catagory.Category,
               Level=course.Level.Level,
               Duration = course.Duration,
               Thumbnail= String.Format("{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                                             _contextAccessor.HttpContext.Request.Scheme,
                                             _contextAccessor.HttpContext.Request.Host,
                                             _contextAccessor.HttpContext.Request.PathBase,
                                             course.Thumbnail),
               CreatedAt=course.CreatedAt,
               IsActive=course.IsActive,
               IsAvailable=course.IsAvailable, 
             ModifiedAt=course.ModifiedAt,
               CreatedBy=course.CreatedBy,
              ModifiedBy=course.ModifiedBy,
           
            };
            
            return courseDetails;
        }
        public CourseListViewModel GetCourseDetailsByCourseName(string courseName)
        {
            var course = _courseRepository.GetCourseDetailsByCourseName(courseName);
            var courseDetails = new CourseListViewModel
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                Catagory = course.Catagory.Category,
                Level = course.Level.Level,
                Duration = course.Duration,
                Thumbnail = String.Format("{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                                             _contextAccessor.HttpContext.Request.Scheme,
                                             _contextAccessor.HttpContext.Request.Host,
                                             _contextAccessor.HttpContext.Request.PathBase,
                                             course.Thumbnail),
                CreatedAt = course.CreatedAt,
                IsActive = course.IsActive,
                IsAvailable = course.IsAvailable,
                ModifiedAt = course.ModifiedAt,
                CreatedBy = course.CreatedBy,
                ModifiedBy = course.ModifiedBy,


            };
            return courseDetails;



        }
    }
}

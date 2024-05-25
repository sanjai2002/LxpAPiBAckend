using AutoMapper;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace LXP.Core.Services
{
    public class CourseServices : ICourseServices
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICourseLevelRepository _courseLevelRepository;

        private Mapper _courseMapper;
        public CourseServices(ICourseRepository courseRepository, ICategoryRepository categoryRepository, ICourseLevelRepository courseLevelRepository, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _courseRepository = courseRepository; ;
            _environment = environment;
            _courseLevelRepository = courseLevelRepository;
            _categoryRepository = categoryRepository;

            _contextAccessor = httpContextAccessor;
        }
        public CourseListViewModel AddCourse(CourseViewModel course)
        {
            bool isCourseExists = _courseRepository.AnyCourseByCourseTitle(course.Title);

            if (!isCourseExists)
            {


                Guid levelId = Guid.Parse(course.Level);
                CourseLevel level = _courseLevelRepository.GetCourseLevelByCourseLevelId(levelId);
                Guid categoryId = Guid.Parse(course.Category);
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
                    Category = category,
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
                Catagory = course.Category.Category,
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
        public CourseListViewModel GetCourseDetailsByCourseName(string courseName)
        {
            var course = _courseRepository.GetCourseDetailsByCourseName(courseName);
            var courseDetails = new CourseListViewModel
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                Catagory = course.Category.Category,
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


        public Course GetCourseByCourseId(Guid courseId)
        {
            var course = _courseRepository.GetCourseDetailsByCourseId(courseId);

            var courseView = new Course
            {
                CourseId = courseId,
                LevelId = course.LevelId,
                CategoryId = course.CategoryId,
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


        public async Task<bool> Deletecourse(Guid courseid)
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
            var course = _courseRepository.FindCourseid(courseStatus.CourseId);
            if (course != null)
            {
                course.IsAvailable = courseStatus.IsAvailable;
                course.ModifiedAt = DateTime.Now;
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
                course.CategoryId = courseupdate.CategoryId;
                course.LevelId = courseupdate.LevelId;
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

        public IEnumerable<CourseDetailsViewModel> GetAllCourse()
        {
            return _courseRepository.GetAllCourse();

        }

        public IEnumerable<CourseDetailsViewModel> GetLimitedCourse()
        {
            return _courseRepository.GetLimitedCourse();
        }


    }
}

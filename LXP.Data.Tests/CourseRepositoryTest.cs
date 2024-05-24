//using NUnit.Framework;
//using Microsoft.EntityFrameworkCore;
//using LXP.Data.Repository;

//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using LXP.Common.Entities;
//using LXP.Common.ViewModels;
//using LXP.Common;

//namespace LXP.Data.Tests
//{
//    [TestFixture]
//    public class CourseRepositoryTests
//    {
//        private DbContextOptions<LXPDbContext> _options;
//        private LXPDbContext _context;

//        [SetUp]
//        public void Setup()
//        {
//            _options = new DbContextOptionsBuilder<LXPDbContext>()
//                .UseInMemoryDatabase(databaseName: "lxp")
//                .Options;

//            _context = new LXPDbContext(_options);
//            SeedData(_context);
//        }

//        [TearDown]
//        public void TearDown()
//        {
//            _context.Dispose();
//        }

//        private void SeedData(LXPDbContext context)
//        {
//            // Seed some test data for Courses and Enrollments
//            var course1 = new Course {
//                CourseId = Guid.NewGuid(),
//                Title = "Course 1",
//                Description = "Description",
//                Thumbnail = "image",
//                CreatedBy = "Laevi",
//                CreatedAt = DateTime.Now,
//                ModifiedBy = "Sanjai",
//                ModifiedAt = DateTime.Now
//            };
//            var course2 = new Course { 
//                CourseId = Guid.NewGuid(),
//                Title = "Course 2", 
//                Description = "Description",
//                Thumbnail = "image",
//                CreatedBy = "kavin", 
//                CreatedAt = DateTime.Now,
//                ModifiedBy = "sanjai",
//                ModifiedAt = DateTime.Now };
//            var enrollment1 = new Enrollment { EnrollmentId = Guid.NewGuid(), CourseId = course1.CourseId, LearnerId = Guid.NewGuid(),
//                CreatedBy = "kavin",
//                CreatedAt = DateTime.Now,
//                ModifiedBy = "sanjai",
//                ModifiedAt = DateTime.Now
//            };
//            var enrollment2 = new Enrollment { EnrollmentId = Guid.NewGuid(), CourseId = course2.CourseId, LearnerId = Guid.NewGuid(),
//                CreatedBy = "sanjai",
//                CreatedAt = DateTime.Now,
//                ModifiedBy = "kavin",
//                ModifiedAt = DateTime.Now
//            };

//            context.Courses.AddRange(course1, course2);
//            context.Enrollments.AddRange(enrollment1, enrollment2);
//            context.SaveChanges();
//        }

//        [Test]
//        public void GetCourseDetailsByCourseId_ValidCourseId_ReturnsCourse()
//        {
//            // Arrange
//            var repository = new CourseRepository(_context, null);
//            var courseId = _context.Courses.First().CourseId;

//            // Act
//            var result = repository.GetCourseDetailsByCourseId(courseId);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(courseId, result.CourseId);
//        }

//        [Test]
//        public void FindCourseid_ValidCourseId_ReturnsCourse()
//        {
//            // Arrange
//            var repository = new CourseRepository(_context, null);
//            var courseId = _context.Courses.First().CourseId;

//            // Act
//            var result = repository.FindCourseid(courseId);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(courseId, result.CourseId);
//        }

//        [Test]
//        public void FindEntrollmentcourse_ValidCourseId_ReturnsEnrollment()
//        {
//            // Arrange
//            var repository = new CourseRepository(_context, null);
//            var courseId = _context.Courses.First().CourseId;

//            // Act
//            var result = repository.FindEntrollmentcourse(courseId);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(courseId, result.CourseId);
//        }

//        [Test]
//        public async Task Deletecourse_ValidCourse_DeletesCourse()
//        {
//            // Arrange
//            var repository = new CourseRepository(_context, null);
//            var course = _context.Courses.First();

//            // Act
//            await repository.Deletecourse(course);
//            var result = _context.Courses.Find(course.CourseId);

//            // Assert
//            Assert.IsNull(result);
//        }

//        [Test]
//        public async Task Changecoursestatus_ValidCourse_UpdatesCourseStatus()
//        {
//            // Arrange
//            var repository = new CourseRepository(_context, null);
//            var course = _context.Courses.First();

//            // Act
//            course.IsAvailable = course.IsAvailable;
//            await repository.Changecoursestatus(course);
//            var result = _context.Courses.Find(course.CourseId);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(course.IsAvailable, result.IsAvailable);
//        }

//        [Test]
//        public async Task Updatecourse_ValidCourse_UpdatesCourse()
//        {
//            // Arrange
//            var repository = new CourseRepository(_context, null);
//            var course = _context.Courses.First();
//            var originalName = course.Title;
//            var newName = "New Course Name";

//            // Act
//            course.Title = newName;
//            await repository.Updatecourse(course);
//            var result = _context.Courses.Find(course.CourseId);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(newName, result.Title);
//            Assert.AreNotEqual(originalName, result.Title);
//        }
//    }
//}
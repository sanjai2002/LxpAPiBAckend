using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LXP.Common.Entities;
using LXP.Data.DBContexts;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LXP.Data.Repository.Tests
{
    [TestFixture]
    public class CourseRepositoryTests
    {
        private DbContextOptions<LXPDbContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<LXPDbContext>()
                .UseInMemoryDatabase(databaseName: "lxp")
                .Options;

            using (var context = new LXPDbContext(_options))
            {
                context.Courses.AddRange(
                    new Course
                    {
                        CourseId = Guid.NewGuid(),
                        Title = "Course 1",
                        Description = "Description",
                        Thumbnail = "image",
                        CreatedBy = "Karni",
                        CreatedAt = DateTime.Now,
                        ModifiedBy = "karikalan",
                        ModifiedAt = DateTime.Now
                    },



                    new Course
                    {
                        CourseId = Guid.NewGuid(),
                        Title = "Course 2",
                        CreatedBy = "Karni",
                        Description = "Description",
                        Thumbnail = "image",
                        CreatedAt = DateTime.Now,
                        ModifiedBy = "karikalan",
                        ModifiedAt = DateTime.Now
                    }
                );
                context.SaveChanges();
            }
        }

        [Test]
        public async Task AddCourse_ValidCourse_AddsCourseToContextAndSavesChanges()
        {
            // Arrange
            using (var context = new LXPDbContext(_options))
            {
                var repository = new CourseRepository(context);
                var courseToAdd = new Course
                {
                    CourseId = Guid.NewGuid(),
                    Title = "Course 1",
                    Description = "Description",
                    Thumbnail = "image",
                    CreatedBy = "Karni",
                    CreatedAt = DateTime.Now,
                    ModifiedBy = "karikalan",
                    ModifiedAt = DateTime.Now
                };

                // Act
                repository.AddCourse(courseToAdd);

                // Assert
                var addedCourse = await context.Courses.FindAsync(courseToAdd.CourseId);
                Assert.IsNotNull(addedCourse);
                Assert.AreEqual(courseToAdd.Title, addedCourse.Title);
            }
        }

        [Test]
        public void GetCourseDetailsByCourseId_ExistingCourseId_ReturnsCourse()
        {
            // Arrange
            using (var context = new LXPDbContext(_options))
            {
                var repository = new CourseRepository(context);
                var courseId = context.Courses.First().CourseId;

                // Act
                var result = repository.GetCourseDetailsByCourseId(courseId);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(courseId, result.CourseId);
            }
        }

        [Test]
        public void GetCourseDetailsByCourseId_NonExistingCourseId_ReturnsNull()
        {
            // Arrange
            using (var context = new LXPDbContext(_options))
            {
                var repository = new CourseRepository(context);
                var nonExistingCourseId = Guid.NewGuid();

                // Act
                var result = repository.GetCourseDetailsByCourseId(nonExistingCourseId);

                // Assert
                Assert.IsNull(result);
            }
        }

        [Test]
        public void GetCourseDetailsByCourseName_ExistingCourseName_ReturnsCourse()
        {
            // Arrange
            using (var context = new LXPDbContext(_options))
            {
                var repository = new CourseRepository(context);

                // Act
                var result = repository.GetCourseDetailsByCourseName("Course 1");

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("Course 1", result.Title);
            }
        }

        [Test]
        public void GetCourseDetailsByCourseName_NonExistingCourseName_ReturnsNull()
        {
            // Arrange
            using (var context = new LXPDbContext(_options))
            {
                var repository = new CourseRepository(context);

                // Act
                var result = repository.GetCourseDetailsByCourseName("Non-existing Course");

                // Assert
                Assert.IsNull(result);
            }
        }

        [Test]
        public void AnyCourseByCourseTitle_ExistingCourseTitle_ReturnsTrue()
        {
            // Arrange
            using (var context = new LXPDbContext(_options))
            {
                var repository = new CourseRepository(context);

                // Act
                var result = repository.AnyCourseByCourseTitle("Course 1");

                // Assert
                Assert.IsTrue(result);
            }
        }

        [Test]
        public void AnyCourseByCourseTitle_NonExistingCourseTitle_ReturnsFalse()
        {
            // Arrange
            using (var context = new LXPDbContext(_options))
            {
                var repository = new CourseRepository(context);

                // Act
                var result = repository.AnyCourseByCourseTitle("Non-existing Course");

                // Assert
                Assert.IsFalse(result);
            }
        }
    }
}
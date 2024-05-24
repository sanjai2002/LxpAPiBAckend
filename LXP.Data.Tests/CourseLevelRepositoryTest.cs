using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LXP.Common.Entities;
using LXP.Data.DBContexts;
using LXP.Data.IRepository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LXP.Data.Repository.Tests
{
    [TestFixture]
    public class CourseLevelRepositoryTests
    {
        private DbContextOptions<LXPDbContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<LXPDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new LXPDbContext(_options))
            {
                context.CourseLevels.AddRange(
                    new CourseLevel
                    {
                        LevelId = Guid.NewGuid(),
                        Level = "Level 1",
                        CreatedBy = "Karni",
                        CreatedAt = DateTime.Now,
                        ModifiedBy = "kani",
                        ModifiedAt = DateTime.Now
                    },
                    new CourseLevel
                    {
                        LevelId = Guid.NewGuid(),
                        Level = "Level 2",
                        CreatedBy = "Karni",
                        CreatedAt = DateTime.Now,
                        ModifiedBy = "kani",
                        ModifiedAt = DateTime.Now
                    }
                );
                context.SaveChanges();
            }
        }

        [Test]
        public async Task AddCourseLevel_ValidLevel_AddsLevelToContextAndSavesChanges()
        {
            // Arrange
            using (var context = new LXPDbContext(_options))
            {
                var repository = new CourseLevelRepository(context);
                var levelToAdd = new CourseLevel
                {
                    LevelId = Guid.NewGuid(),
                    Level = "Test Level",
                    CreatedBy = "Karni",
                    CreatedAt = DateTime.Now,
                    ModifiedBy = "kani",
                    ModifiedAt = DateTime.Now
                };

                // Act
                await repository.AddCourseLevel(levelToAdd);

                // Assert
                var addedLevel = await context.CourseLevels.FindAsync(levelToAdd.LevelId);
                Assert.IsNotNull(addedLevel);
                Assert.AreEqual(levelToAdd.Level, addedLevel.Level);
            }
        }

        //[Test]
        //public void GetAllCourseLevel_ReturnsAllLevelsFromContext()
        //{
        //    // Arrange
        //    using (var context = new LXPDbContext(_options))
        //    {
        //        var repository = new CourseLevelRepository(context);

        //        // Act
        //        var result = repository.GetAllCourseLevel();

        //        // Assert
        //        Assert.AreEqual(2, result);
        //    }
        //}

        [Test]
        public void GetCourseLevelByCourseLevelId_ReturnsCorrectLevel()
        {
            // Arrange
            using (var context = new LXPDbContext(_options))
            {
                var repository = new CourseLevelRepository(context);
                var levelId = context.CourseLevels.First().LevelId;

                // Act
                var result = repository.GetCourseLevelByCourseLevelId(levelId);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(levelId, result.LevelId);
            }
        }
    }
}
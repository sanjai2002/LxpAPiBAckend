using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.Services;
using LXP.Data.IRepository;
using Moq;
using NUnit.Framework;

namespace LXP.Core.Services.Tests
{
    [TestFixture]
    public class CourseLevelServicesTests
    {
        private Mock<ICourseLevelRepository> _mockRepository;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<ICourseLevelRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CourseLevel, CourseLevelListViewModel>().ReverseMap();
            });

            _mapper = config.CreateMapper();
        }

        //RETRY

        //[Test]
        //public async Task GetAllCourseLevel_EmptyCourseLevels_ReturnsDefaultLevelsAndCallsAddCourseLevel()
        //{
        //    // Arrange
        //    var emptyCourseLevels = new List<CourseLevel>();
        //    _mockRepository.Setup(r => r.GetAllCourseLevel()).ReturnsAsync(emptyCourseLevels);

        //    var services = new CourseLevelServices(_mockRepository.Object);

        //    // Act
        //    var result = await services.GetAllCourseLevel("admin");

        //    // Assert
        //    _mockRepository.Verify(r => r.AddCourseLevel(It.IsAny<CourseLevel>()), Times.Exactly(3));
        //    result = await services.GetAllCourseLevel("admin");
        //    Assert.AreEqual(3, result.Count);
        //    Assert.AreEqual("Beginner", result[0].Level); // Corrected index
        //    Assert.AreEqual("Intermediate", result[1].Level); // Corrected index
        //    Assert.AreEqual("Advanced", result[2].Level); // Corrected index
        //}


        [Test]
        public async Task GetAllCourseLevel_NonEmptyCourseLevels_ReturnsMappedViewModels()
        {
            // Arrange
            var courseLevels = new List<CourseLevel>
            {
                new CourseLevel { LevelId = Guid.NewGuid(), Level = "Beginner" },
                new CourseLevel { LevelId = Guid.NewGuid(), Level = "Advanced" }
            };
            _mockRepository.Setup(r => r.GetAllCourseLevel()).ReturnsAsync(courseLevels);

            var services = new CourseLevelServices(_mockRepository.Object);

            // Act
            var result = await services.GetAllCourseLevel("admin");

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Beginner", result[0].Level);
            Assert.AreEqual("Advanced", result[1].Level);
        }

        [Test]
        public async Task AddCourseLevel_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var services = new CourseLevelServices(_mockRepository.Object);

            // Act
            await services.AddCourseLevel("TestLevel", "admin");

            // Assert
            _mockRepository.Verify(r => r.AddCourseLevel(It.Is<CourseLevel>(c => c.Level == "TestLevel" && c.CreatedBy == "admin")), Times.Once);
        }
    }
}
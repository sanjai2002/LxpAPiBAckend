using NUnit.Framework;
using Moq;
using System;
using LXP.Common.ViewModels;
using LXP.Common.Entities;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using LXP.Core.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace LXP.Core.Tests.Services
{
    [TestFixture]
    public class CourseServicesTests
    {
        private Mock<ICourseRepository> _courseRepositoryMock;
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private Mock<ICourseLevelRepository> _courseLevelRepositoryMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;

        [SetUp]
        public void Setup()
        {
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _courseLevelRepositoryMock = new Mock<ICourseLevelRepository>();
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        }

        //[Test]
        //public void AddCourse_CourseDoesNotExist_ReturnsTrue()
        //{
        //    // Arrange
        //    var courseServices = new CourseServices(
        //        _courseRepositoryMock.Object,
        //        _categoryRepositoryMock.Object,
        //        _courseLevelRepositoryMock.Object,
        //        _webHostEnvironmentMock.Object,
        //        _httpContextAccessorMock.Object);

        //    var courseViewModel = new CourseViewModel
        //    {
        //        Title = "Course Title",
        //        Level = "LevelId",
        //        Catagory = "CategoryId",
        //        Description = "Course Description",
        //        Duration = 10,
        //        Thumbnailimage = null,

        //        // Mock the file here if needed

        //    };

        //    _courseRepositoryMock.Setup(x => x.AnyCourseByCourseTitle(It.IsAny<string>())).Returns(false);
        //    _courseLevelRepositoryMock.Setup(x => x.GetCourseLevelByCourseLevelId(It.IsAny<Guid>())).Returns(new CourseLevel());
        //    _categoryRepositoryMock.Setup(x => x.GetCategoryByCategoryId(It.IsAny<Guid>())).Returns(new CourseCategory());

        //    // Act
        //    var result = courseServices.AddCourse(courseViewModel);

        //    // Assert
        //    Assert.IsTrue(result);
        //    _courseRepositoryMock.Verify(x => x.AddCourse(It.IsAny<Course>()), Times.Once);
        //}

        [Test]
        public void AddCourse_CourseExists_ReturnsFalse()
        {
            // Arrange
            var courseServices = new CourseServices(
                _courseRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _courseLevelRepositoryMock.Object,
                _webHostEnvironmentMock.Object,
                _httpContextAccessorMock.Object);

            var courseViewModel = new CourseViewModel
            {
                Title = "Existing Course Title",
                Level = "LevelId",
                Catagory = "CategoryId",
                Description = "Course Description",
                Duration = 10,
                Thumbnailimage = null // Mock the file here if needed
            };

            _courseRepositoryMock.Setup(x => x.AnyCourseByCourseTitle(It.IsAny<string>())).Returns(true);

            // Act
            
            var result = courseServices.AddCourse(courseViewModel);

            // Assert
            Assert.IsFalse(result);
            _courseRepositoryMock.Verify(x => x.AddCourse(It.IsAny<Course>()), Times.Never);
        }
    }
}
using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LXP.Common.Constants;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using System.Net;
using LXP.Api.Controllers;

namespace LXP.Api.Tests
{
    [TestFixture]
    public class CourseControllerTests
    {
        private Mock<ICourseServices> _courseServicesMock;
        private CourseController _courseController;

        [SetUp]
        public void Setup()
        {
            _courseServicesMock = new Mock<ICourseServices>();
            _courseController = new CourseController(_courseServicesMock.Object);
        }

        [Test]
        public async Task AddCourseDetails_ValidCourse_ReturnsOkResultWithInsertedCourseDetails()
        {
            // Arrange
            var courseViewModel = new CourseViewModel
            {
                Title = "New Course",
                Level = "LevelId",
                Catagory = "CategoryId",
                Description = "Course Description",
                Duration = 10,
                Thumbnailimage = null // Mock the file here if needed
            };

            var insertedCourse = new CourseListViewModel
            {
                CourseId = Guid.NewGuid(),
                Title = "New Course",
                Level = "Beginner",
                Catagory ="Technical",
                Description = "Course Description",
                Duration = 10,
                Thumbnail = "thumbnail.jpg",
                CreatedBy = "Admin",
                CreatedAt = DateTime.Now,
                IsActive = true,
                IsAvailable = true,
                ModifiedAt = DateTime.Now,
                ModifiedBy = "Admin"
            };

            _courseServicesMock.Setup(x => x.AddCourse(courseViewModel)).Returns(true);
            _courseServicesMock.Setup(x => x.GetCourseDetailsByCourseName(courseViewModel.Title)).Returns(insertedCourse);

            // Act
            var result = _courseController.AddCourseDetails(courseViewModel);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            APIResponse response = (APIResponse)okResult.Value;
            Assert.AreEqual((int)HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(insertedCourse, response.Data);
        }

        [Test]
        public async Task AddCourseDetails_InvalidModelState_ReturnsBadRequestResult()
        {
            // Arrange
            _courseController.ModelState.AddModelError("error", "model state error");
            var courseViewModel = new CourseViewModel();

            // Act
            var result = _courseController.AddCourseDetails(courseViewModel);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task AddCourseDetails_ExistingCourse_ReturnsOkResultWithFailureMessage()
        {
            // Arrange
            var courseViewModel = new CourseViewModel
            {
                Title = "Existing Course"
            };

            _courseServicesMock.Setup(x => x.AddCourse(courseViewModel)).Returns(false);

            // Act
            var result = _courseController.AddCourseDetails(courseViewModel);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            APIResponse response = (APIResponse)okResult.Value;
            Assert.AreEqual((int)HttpStatusCode.PreconditionFailed, response.StatusCode);
            Assert.AreEqual(MessageConstants.MsgAlreadyExists, response.Message);
        }

        [Test]
        public async Task GetCourseDetailsByCourseId_ValidId_ReturnsOkResultWithCourseDetails()
        {
            // Arrange
            var courseId = "1";
            var course = new CourseListViewModel
            {
                CourseId = Guid.NewGuid(),
                Title = "Course 1",
                Level = "Beginner",
                Catagory = "Technical",
                Description = "Course Description",
                Duration = 10,
                Thumbnail = "thumbnail.jpg",
                CreatedBy = "Admin",
                CreatedAt = DateTime.Now,
                IsActive = true,
                IsAvailable = true,
                ModifiedAt = DateTime.Now,
                ModifiedBy = "Admin"
            };

            _courseServicesMock.Setup(x => x.GetCourseDetailsByCourseId(courseId)).Returns(course);

            // Act
            var result = await _courseController.GetCourseDetailsByCourseId(courseId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            APIResponse response= (APIResponse)okResult.Value;
            Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(course, response.Data);
        }
    }
}
using LXP.Api.Controllers;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Api.Tests
{
    [TestFixture]
    public class CourseTopicControllerTests
    {
        private Mock<ICourseTopicServices> _mockCourseTopicServices;
        private CourseTopicController _controller;

        [SetUp]
        public void Setup()
        {
            _mockCourseTopicServices = new Mock<ICourseTopicServices>();
            _controller = new CourseTopicController(_mockCourseTopicServices.Object);
        }

        //retry
        //[Test]
        //public async Task AddCourseTopic_NewTopic_ReturnsTrue()
        //{
        //    // Arrange
        //    var courseTopic = new CourseTopicViewModel
        //    {
        //        Name = "New Topic",
        //        Description = "Description",
        //        CourseId = Guid.NewGuid().ToString(),
        //        CreatedBy = "User"
        //    };

        //    _mockCourseTopicRepository.Setup(x => x.AnyTopicByTopicName(It.IsAny<string>())).Returns(false);
        //    _mockCourseRepository.Setup(x => x.GetCourseDetailsByCourseId(It.IsAny<Guid>())).Returns(new Course());

        //    // Act
        //    var result = await _courseTopicServices.AddCourseTopic(courseTopic);

        //    // Assert
        //    Assert.IsTrue(result);
        //}

        //retry

        //[Test]
        //public async Task AddCourseTopic_TopicNotCreated_ReturnsBadRequest()
        //{
        //    // Arrange
        //    var courseTopic = new CourseTopicViewModel();
        //    _mockCourseTopicServices.Setup(x => x.AddCourseTopic(courseTopic)).ReturnsAsync(false);

        //    // Act
        //    var result = await _controller.AddCourseTopic(courseTopic) as ObjectResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //    APIResponse response = (APIResponse)result.Value;
        //    Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        //}

        [Test]
        public async Task GetAllCourseTopicByCourseId_ReturnsOk()
        {
            // Arrange
            var id = "courseId";
            _mockCourseTopicServices.Setup(x => x.GetAllTopicDetailsByCourseId(id)).Returns(new object());

            // Act
            var result = _controller.GetAllCourseTopicByCourseId(id) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task UpdateCourseTopic_TopicUpdated_ReturnsOk()
        {
            // Arrange
            var courseTopic = new CourseTopicUpdateModel();
            _mockCourseTopicServices.Setup(x => x.UpdateCourseTopic(courseTopic)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateCourseTopic(courseTopic) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task UpdateCourseTopic_TopicNotUpdated_ReturnsBadRequest()
        {
            // Arrange
            var courseTopic = new CourseTopicUpdateModel();
            _mockCourseTopicServices.Setup(x => x.UpdateCourseTopic(courseTopic)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateCourseTopic(courseTopic) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            APIResponse response = (APIResponse) result.Value;
            Assert.AreEqual((int)HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }

        
    }
}

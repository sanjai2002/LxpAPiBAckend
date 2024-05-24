using NUnit.Framework;
using Moq;
using LXP.Api.Controllers;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using LXP.Common.ViewModels;

namespace LXP.Api.Tests
{
    [TestFixture]
    public class CourseLevelControllerTests
    {
        private CourseLevelController _courseLevelController;
        private Mock<ICourseLevelServices> _courseLevelServicesMock;

        [SetUp]
        public void Setup()
        {
            _courseLevelServicesMock = new Mock<ICourseLevelServices>();
            _courseLevelController = new CourseLevelController(_courseLevelServicesMock.Object);
        }

        [Test]
        public async Task GetAllCourseLevel_ValidAccessedBy_ReturnsOk()
        {
            // Arrange
            var accessedBy = "TestUser";
            var courseLevels = new List<CourseLevelListViewModel>
            {
                new CourseLevelListViewModel { LevelId = Guid.Parse("4da6ee07-e71a-43f9-b975-a4538b1f5385"), Level = "Beginner" },
                new CourseLevelListViewModel { LevelId = Guid.Parse("51e8cf2d-1b86-492a-b97e-be546b2f6aaf"), Level = "Intermediate" },
                new CourseLevelListViewModel { LevelId = Guid.Parse("701cd139-a70a-4068-b216-6c907ced3afd"), Level = "Advanced" }
            };

            _courseLevelServicesMock.Setup(service => service.GetAllCourseLevel(accessedBy))
                                     .ReturnsAsync(courseLevels);

            // Act
            var result = await _courseLevelController.GetAllCourseLevel(accessedBy);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            APIResponse response = (APIResponse)okResult.Value;
            Assert.AreEqual((int)System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(courseLevels, response.Data);
        }
    }
}
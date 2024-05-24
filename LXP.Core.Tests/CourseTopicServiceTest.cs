using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using LXP.Core.Services;
using LXP.Data.IRepository;
using LXP.Common.Entities;
using LXP.Common.ViewModels;


namespace LXP.Core.Tests.Services
{
    [TestFixture]
    public class CourseTopicServicesTest
    {
        private Mock<ICourseTopicRepository> _mockCourseTopicRepository;
        private Mock<ICourseRepository> _mockCourseRepository;
        private CourseTopicServices _courseTopicServices;

        [SetUp]
        public void Setup()
        {
            _mockCourseTopicRepository = new Mock<ICourseTopicRepository>();
            _mockCourseRepository = new Mock<ICourseRepository>();
            _courseTopicServices = new CourseTopicServices(_mockCourseTopicRepository.Object, _mockCourseRepository.Object);
        }

        [Test]
        public async Task AddCourseTopic_NewTopic_ReturnsTrue()
        {
            // Arrange
            var courseTopic = new CourseTopicViewModel
            {
                Name = "New Topic",
                Description = "Description",
                CourseId = Guid.NewGuid().ToString(),
                CreatedBy = "User"
            };

            _mockCourseTopicRepository.Setup(x => x.AnyTopicByTopicName(It.IsAny<string>())).Returns(false);
            _mockCourseRepository.Setup(x => x.GetCourseDetailsByCourseId(It.IsAny<Guid>())).Returns(new Course());

            // Act
            var result = await _courseTopicServices.AddCourseTopic(courseTopic);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddCourseTopic_ExistingTopic_ReturnsFalse()
        {
            // Arrange
            var courseTopic = new CourseTopicViewModel
            {
                Name = "Existing Topic",
                Description = "Description",
                CourseId = Guid.NewGuid().ToString(),
                CreatedBy = "User"
            };

            _mockCourseTopicRepository.Setup(x => x.AnyTopicByTopicName(It.IsAny<string>())).Returns(true);

            // Act
            var result = await _courseTopicServices.AddCourseTopic(courseTopic);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task UpdateCourseTopic_ExistingTopic_ReturnsTrue()
        {
            // Arrange
            var courseTopic = new CourseTopicUpdateModel
            {
                TopicId = Guid.NewGuid().ToString(),
                Name = "Updated Topic",
                Description = "Updated Description",
                ModifiedBy = "User"
            };

            var existingTopic = new Topic
            {
                TopicId = Guid.Parse(courseTopic.TopicId),
                Name = "Existing Topic",
                Description = "Existing Description",
                ModifiedBy = "Previous User",
                ModifiedAt = DateTime.Now.AddDays(-1) // Set to a previous date to ensure modification
            };

            _mockCourseTopicRepository.Setup(x => x.GetTopicByTopicId(It.IsAny<Guid>())).ReturnsAsync(existingTopic);
            _mockCourseTopicRepository.Setup(x => x.UpdateCourseTopic(It.IsAny<Topic>())).ReturnsAsync(1);

            // Act
            var result = await _courseTopicServices.UpdateCourseTopic(courseTopic);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("Updated Topic", existingTopic.Name);
            Assert.AreEqual("Updated Description", existingTopic.Description);
            Assert.AreEqual("User", existingTopic.ModifiedBy);
            Assert.IsTrue((DateTime.Now - existingTopic.ModifiedAt.Value).TotalSeconds < 1); // Check if ModifiedAt is within 1 second of current time
        }

        [Test]
        public void GetTopicDetails_ReturnsTopicDetails()
        {
            // Arrange
            var courseId = "courseId";
            var expectedTopicDetails = new Topic(); // Assuming TopicDetails is the return type of _courseTopicRepository.GetTopicDetails(courseId)

            _mockCourseTopicRepository.Setup(x => x.GetAllTopicDetailsByCourseId(courseId)).Returns(expectedTopicDetails);

            // Act
            var result = _courseTopicServices.GetAllTopicDetailsByCourseId(courseId);

            // Assert
            Assert.AreEqual(expectedTopicDetails, result);
        }
    }
}
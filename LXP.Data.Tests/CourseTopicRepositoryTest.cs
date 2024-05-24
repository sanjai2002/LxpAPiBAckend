using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using LXP.Common.Entities;
using Microsoft.EntityFrameworkCore;
using LXP.Data.DBContexts;
using LXP.Data.Repository;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LXP.Data.Tests
{
    public class CourseTopicRepositoryTest
    {
        private LXPDbContext _dbContext;
        private ICourseTopicRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<LXPDbContext>()
                .UseInMemoryDatabase(databaseName: "lxp")
                .Options;

            _dbContext = new LXPDbContext(options);
            _repository = new CourseTopicRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        [Test]
        public async Task AddCourseTopic_Should_AddTopicToDatabase()
        {
            // Arrange
            var topic = new Topic
            {
                TopicId = Guid.NewGuid(),
                Name = "Test Topic",
                Description = "Test Description",
                IsActive = true,
                CourseId = Guid.NewGuid(),
                CreatedBy = "Karni",
                CreatedAt = DateTime.Now,
                ModifiedBy = "karikalan",
                ModifiedAt = DateTime.Now
            };

            // Act
            await _repository.AddCourseTopic(topic);

            // Assert
            var addedTopic = await _dbContext.Topics.FindAsync(topic.TopicId);
            Assert.NotNull(addedTopic);
            Assert.AreEqual(topic.Name, addedTopic.Name);
            Assert.AreEqual(topic.Description, addedTopic.Description);
            Assert.AreEqual(topic.IsActive, addedTopic.IsActive);
            Assert.AreEqual(topic.CourseId, addedTopic.CourseId);
        }

        [Test]
        public async Task GetTopicByTopicId_Should_ReturnTopic()
        {
            // Arrange
            var topicId = Guid.NewGuid();
            var topic = new Topic
            {
                TopicId = topicId,
                Name = "Test Topic",
                Description = "Test Description",
                IsActive = true,
                CourseId = Guid.NewGuid(),
                CreatedBy = "Karni",
                CreatedAt = DateTime.Now,
                ModifiedBy = "karikalan",
                ModifiedAt = DateTime.Now
            };

            _dbContext.Topics.Add(topic);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetTopicByTopicId(topicId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(topicId, result.TopicId);
        }

        //[Test]
        //public async Task GetTopicDetails_Should_ReturnTopicDetails()
        //{
        //    // Arrange
        //    var courseId = Guid.NewGuid().ToString();
        //    var course = new Course
        //    {
        //        CourseId = Guid.Parse(courseId),
        //        Title = "Test Course",
        //        IsActive = true

        //    };

        //    var topicId = Guid.NewGuid();
        //    var topic = new Topic
        //    {
        //        TopicId = topicId,
        //        Name = "Test Topic",
        //        Description = "Test Description",
        //        IsActive = true,
        //        CourseId = course.CourseId
        //    };
        //    var materialTypeId=Guid.NewGuid();
        //    var materialType = new MaterialType
        //    {
        //        MaterialTypeId= materialTypeId,
        //        Type="Paithiyam"
        //    };
        //    var material = new Material
        //    {
        //        MaterialId = Guid.NewGuid(),
        //        Name = "Test Material",
        //        Duration = 60,
        //        MaterialTypeId = materialType.MaterialTypeId,
        //        TopicId = topic.TopicId
        //    };

        //    _dbContext.Courses.Add(course);
        //    _dbContext.Topics.Add(topic);
        //    _dbContext.Materials.Add(material);
        //    await _dbContext.SaveChangesAsync();

        //    // Act
        //    var result = _repository.GetTopicDetails(courseId);

        //    // Assert
        //    Assert.NotNull(result);
        //    var topicDetails = result;
        //    Assert.NotNull(topicDetails);
        //    Assert.AreEqual(course.CourseId, topicDetails.CourseId);
        //    Assert.AreEqual(course.Title, topicDetails.CourseTitle);
        //    Assert.AreEqual(course.IsActive, topicDetails.CourseIsActive);
        //    Assert.AreEqual(1, topicDetails.Topics.Count);
        //    var topicDetail = topicDetails.Topics.FirstOrDefault();
        //    Assert.NotNull(topicDetail);
        //    Assert.AreEqual(topic.Name, topicDetail.TopicName);
        //    Assert.AreEqual(topic.Description, topicDetail.TopicDescription);
        //    Assert.AreEqual(topic.IsActive, topicDetail.TopicIsActive);
        //    Assert.AreEqual(1, topicDetail.Materials.Count);
        //    var materialDetail = topicDetail.Materials.FirstOrDefault();
        //    Assert.NotNull(materialDetail);
        //    Assert.AreEqual(material.MaterialId, materialDetail.MaterialId);
        //    Assert.AreEqual(material.Name, materialDetail.MaterialName);
        //    Assert.AreEqual(materialType.Type, materialDetail.MaterialType); // Assuming MaterialType with id 1 is "Type1"
        //    Assert.AreEqual(material.Duration, materialDetail.MaterialDuration);
        //}

        [Test]
        public void AnyTopicByTopicName_Should_ReturnTrueIfTopicExists()
        {
            // Arrange
            var topicName = "Test Topic";
            var topic = new Topic
            {
                TopicId = Guid.NewGuid(),
                Name = topicName,
                Description = "Test Description",
                IsActive = true,
                CourseId = Guid.NewGuid(),
                CreatedBy = "Karni",
                CreatedAt = DateTime.Now,
                ModifiedBy = "karikalan",
                ModifiedAt = DateTime.Now
            };

            _dbContext.Topics.Add(topic);
            _dbContext.SaveChanges();

            // Act
            var result = _repository.AnyTopicByTopicName(topicName);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void AnyTopicByTopicName_Should_ReturnFalseIfTopicDoesNotExist()
        {
            // Arrange
            var topicName = "Non-existent Topic";

            // Act
            var result = _repository.AnyTopicByTopicName(topicName);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]

       public async Task UpdateCourseTopic_Should_UpdateTopic()
        {
            // Arrange
            var topicId = Guid.NewGuid();
            var topic = new Topic
            {
                TopicId = topicId,
                Name = "Test Topic",
                Description = "Test Description",
                IsActive = true,
                CourseId = Guid.NewGuid(),
                CreatedBy="Karni",
                CreatedAt= DateTime.Now,
                ModifiedBy ="kani",
                ModifiedAt= DateTime.Now

            };

            _dbContext.Topics.Add(topic);
            await _dbContext.SaveChangesAsync();

            // Modify the topic
            topic.Name = "Updated Topic";

            // Act
            await _repository.UpdateCourseTopic(topic);

            // Assert
            var updatedTopic = await _dbContext.Topics.FindAsync(topicId);
            Assert.NotNull(updatedTopic);
            Assert.AreEqual(topic.Name, updatedTopic.Name);
        }
    }
}
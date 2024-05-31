using LXP.Common.Entities;
using LXP.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.Repository
{
    public class CourseTopicRepository : ICourseTopicRepository
    {
        private readonly LXPDbContext _lXPDbContext;
        public CourseTopicRepository(LXPDbContext lXPDbContext)
        {
            this._lXPDbContext = lXPDbContext;
        }
        public async Task AddCourseTopic(Topic topic)
        {
            await _lXPDbContext.Topics.AddAsync(topic);
            await _lXPDbContext.SaveChangesAsync();
        }
        public object GetTopicDetails(string courseId)
        {
            var result = from course in _lXPDbContext.Courses
                         where course.CourseId == Guid.Parse(courseId)
                         select new
                         {
                             CourseId = course.CourseId,
                             CourseTitle = course.Title,
                             CourseIsActive = course.IsActive,
                             Topics = (from topic in _lXPDbContext.Topics
                                       where topic.CourseId == course.CourseId
                                       select new
                                       {
                                           TopicName = topic.Name,
                                           TopicDescription = topic.Description,
                                           TopicId = topic.TopicId,
                                           TopicIsActive = topic.IsActive,
                                           Materials = (from material in _lXPDbContext.Materials
                                                        join materialType in _lXPDbContext.MaterialTypes on material.MaterialTypeId equals materialType.MaterialTypeId
                                                        where material.TopicId == topic.TopicId
                                                        select new
                                                        {
                                                            MaterialId = material.MaterialId,
                                                            MaterialName = material.Name,
                                                            MaterialType = materialType.Type,
                                                            MaterialDuration = material.Duration
                                                        }).ToList()
                                       }).ToList()
                         };



            return result;
        }
        public bool AnyTopicByTopicName(string topicName)
        {
            return _lXPDbContext.Topics.Any(topic => topic.Name == topicName);
        }

        public async Task<Topic> GetTopicByTopicId(Guid topicId)
        {
            return await _lXPDbContext.Topics.FindAsync(topicId);
        }
    }
}
using LXP.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.IRepository
{
    public interface ICourseTopicRepository
    {
        object GetTopicDetails(string courseId);
        Task AddCourseTopic(Topic topic);
        bool AnyTopicByTopicName(string topicName);
        Task<Topic> GetTopicDetailsByTopicNameAndCourse(string topicName, Guid courseId);

        Task<int> UpdateCourseTopic(Topic topic);

        Task<Topic> GetTopicByTopicId(Guid topicId);
        Task<List<Topic>> GetTopicsbycouresId(Guid courseId);
        Task<List<LearnerProgress>> GetTopicsbyLearnerId(Guid courseId, Guid LearnerId);

    }
}
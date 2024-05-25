using LXP.Common.Entities;

namespace LXP.Data.IRepository
{
    public interface ICourseTopicRepository
    {
        object GetAllTopicDetailsByCourseId(string courseId);
        void AddCourseTopic(Topic topic);
        bool AnyTopicByTopicName(string topicName);
        Task<Topic> GetTopicDetailsByTopicNameAndCourse(string topicName, Guid courseId);

        Task<int> UpdateCourseTopic(Topic topic);
        Task<Topic> GetTopicByTopicId(Guid topicId);


    }
}

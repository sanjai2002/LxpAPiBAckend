using LXP.Common.ViewModels;
namespace LXP.Core.IServices
{
    public interface ICourseTopicServices
    {
        object GetAllTopicDetailsByCourseId(string courseId);
        Task<CourseTopicListViewModel> AddCourseTopic(CourseTopicViewModel courseTopic);
        Task<bool> UpdateCourseTopic(CourseTopicUpdateModel courseTopic);
        Task<bool> SoftDeleteTopic(string topicId);
        Task<CourseTopicListViewModel> GetTopicDetailsByTopicNameAndCourseId(string topicName, string courseId);
        Task<CourseTopicListViewModel> GetTopicDetailsByTopicId(string topicId);

    }
}

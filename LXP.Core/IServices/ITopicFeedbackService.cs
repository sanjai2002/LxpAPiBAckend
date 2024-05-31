
using LXP.Common.ViewModels.TopicFeedbackQuestionViemModel;


namespace LXP.Core.IServices
{
    public interface ITopicFeedbackService
    {
        IEnumerable<TopicFeedbackQuestionNoViewModel> GetAllFeedbackQuestions();
        TopicFeedbackQuestionNoViewModel GetFeedbackQuestionById(Guid id);
        
        bool AddFeedbackQuestion(TopicFeedbackQuestionViewModel question, List<FeedbackOptionDTO> options);
        bool UpdateFeedbackQuestion(Guid id, TopicFeedbackQuestionViewModel question, List<FeedbackOptionDTO> options);
        bool DeleteFeedbackQuestion(Guid id);
        IEnumerable<TopicFeedbackQuestionNoViewModel> GetFeedbackQuestionsByTopicId(Guid topicId);
    }
}


using LXP.Common.Entities;
using LXP.Common.ViewModels.TopicFeedbackQuestionViemModel;

namespace LXP.Core.IServices
{
    public interface ITopicFeedbackService
    {
        Guid AddFeedbackQuestion(TopicFeedbackQuestionViewModel topicFeedbackQuestion, List<TopicFeedbackQuestionsOptionViewModel> options);
        List<TopicFeedbackQuestionNoViewModel> GetAllFeedbackQuestions();
        TopicFeedbackQuestionNoViewModel GetFeedbackQuestionById(Guid topicFeedbackQuestionId);
        bool UpdateFeedbackQuestion(Guid topicFeedbackQuestionId, TopicFeedbackQuestionViewModel topicFeedbackQuestion, List<TopicFeedbackQuestionsOptionViewModel> options);
        bool DeleteFeedbackQuestion(Guid topicFeedbackQuestionId);
        List<TopicFeedbackQuestionNoViewModel> GetFeedbackQuestionsByTopicId(Guid topicId);
        bool DeleteFeedbackQuestionsByTopicId(Guid topicId);
    }
}
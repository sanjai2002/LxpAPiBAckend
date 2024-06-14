using LXP.Common.ViewModels.FeedbackResponseViewModel;

namespace LXP.Services.IServices
{
    public interface IFeedbackResponseDetailsService
    {
        List<QuizFeedbackResponseDetailsViewModel> GetQuizFeedbackResponses(Guid quizId);
        List<TopicFeedbackResponseDetailsViewModel> GetTopicFeedbackResponses(Guid topicId);
        List<QuizFeedbackResponseDetailsViewModel> GetQuizFeedbackResponsesByLearner(Guid quizId, Guid learnerId);
        List<TopicFeedbackResponseDetailsViewModel> GetTopicFeedbackResponsesByLearner(Guid topicId, Guid learnerId);
    }
}
using LXP.Common.ViewModels.FeedbackResponseViewModel;

namespace LXP.Services.IServices
{
    public interface IFeedbackResponseService
    {
        void SubmitFeedbackResponse(QuizFeedbackResponseViewModel feedbackResponse);
        void SubmitFeedbackResponse(TopicFeedbackResponseViewModel feedbackResponse);
        void SubmitFeedbackResponses(IEnumerable<QuizFeedbackResponseViewModel> feedbackResponses);
        void SubmitFeedbackResponses(IEnumerable<TopicFeedbackResponseViewModel> feedbackResponses);

        //new
        LearnerFeedbackStatusViewModel GetQuizFeedbackStatus(Guid learnerId, Guid quizId);
        LearnerFeedbackStatusViewModel GetTopicFeedbackStatus(Guid learnerId, Guid topicId);
    }
}

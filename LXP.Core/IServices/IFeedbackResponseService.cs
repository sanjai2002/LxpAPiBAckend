using LXP.Common.ViewModels.FeedbackResponseViewModel;

namespace LXP.Services.IServices
{
    public interface IFeedbackResponseService
    {
        void SubmitFeedbackResponse(QuizFeedbackResponseViewModel feedbackResponse);
        void SubmitFeedbackResponse(TopicFeedbackResponseViewModel feedbackResponse);
    }
}
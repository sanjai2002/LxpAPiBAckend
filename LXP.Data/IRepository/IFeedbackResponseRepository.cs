using LXP.Common.ViewModels.FeedbackResponseViewModel;

namespace LXP.Data.IRepository
{
    public interface IFeedbackResponseRepository
    {
        void SubmitFeedbackResponse(QuizFeedbackResponseViewModel feedbackResponse);
        void SubmitFeedbackResponse(TopicFeedbackResponseViewModel feedbackResponse);
    }
}
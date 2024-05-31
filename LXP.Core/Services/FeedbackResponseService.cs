using LXP.Common.ViewModels.FeedbackResponseViewModel;
using LXP.Data.IRepository;
using LXP.Services.IServices;

namespace LXP.Services
{
    public class FeedbackResponseService : IFeedbackResponseService
    {
        private readonly IFeedbackResponseRepository _feedbackResponseRepository;

        public FeedbackResponseService(IFeedbackResponseRepository feedbackResponseRepository)
        {
            _feedbackResponseRepository = feedbackResponseRepository;
        }

        public void SubmitFeedbackResponse(QuizFeedbackResponseViewModel feedbackResponse)
        {
            _feedbackResponseRepository.SubmitFeedbackResponse(feedbackResponse);
        }

        public void SubmitFeedbackResponse(TopicFeedbackResponseViewModel feedbackResponse)
        {
            _feedbackResponseRepository.SubmitFeedbackResponse(feedbackResponse);
        }
    }
}
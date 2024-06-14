using LXP.Common.ViewModels.FeedbackResponseViewModel;
using LXP.Data.IRepository;
using LXP.Services.IServices;

namespace LXP.Services
{
    public class FeedbackResponseDetailsService : IFeedbackResponseDetailsService
    {
        private readonly IFeedbackResponseRepository _feedbackResponseRepository;

        public FeedbackResponseDetailsService(IFeedbackResponseRepository feedbackResponseRepository)
        {
            _feedbackResponseRepository = feedbackResponseRepository;
        }

        public List<QuizFeedbackResponseDetailsViewModel> GetQuizFeedbackResponses(Guid quizId)
        {
            return _feedbackResponseRepository.GetQuizFeedbackResponses(quizId);
        }

        public List<TopicFeedbackResponseDetailsViewModel> GetTopicFeedbackResponses(Guid topicId)
        {
            return _feedbackResponseRepository.GetTopicFeedbackResponses(topicId);
        }

        public List<QuizFeedbackResponseDetailsViewModel> GetQuizFeedbackResponsesByLearner(Guid quizId, Guid learnerId)
        {
            return _feedbackResponseRepository.GetQuizFeedbackResponsesByLearner(quizId, learnerId);
        }

        public List<TopicFeedbackResponseDetailsViewModel> GetTopicFeedbackResponsesByLearner(Guid topicId, Guid learnerId)
        {
            return _feedbackResponseRepository.GetTopicFeedbackResponsesByLearner(topicId, learnerId);
        }
    }
}
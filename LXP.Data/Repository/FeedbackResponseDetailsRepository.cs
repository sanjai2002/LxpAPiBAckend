using LXP.Common.Entities;
using LXP.Common.ViewModels.FeedbackResponseViewModel;
using LXP.Data.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LXP.Data.Repository
{
    public class FeedbackResponseDetailsRepository : IFeedbackResponseDetailsRepository
    {
        private readonly LXPDbContext _context;

        public FeedbackResponseDetailsRepository(LXPDbContext context)
        {
            _context = context;
        }

        public List<QuizFeedbackResponseDetailsViewModel> GetQuizFeedbackResponses(Guid quizId)
        {
            var responses = _context.Feedbackresponses
                .Include(r => r.QuizFeedbackQuestion)
                .Include(r => r.Option)
                .Where(r => r.QuizFeedbackQuestion.QuizId == quizId)
                .Select(r => new QuizFeedbackResponseDetailsViewModel
                {
                    QuizFeedbackQuestionId = r.QuizFeedbackQuestionId,
                    LearnerId = r.LearnerId,
                    Question = r.QuizFeedbackQuestion.Question,
                    QuestionType = r.QuizFeedbackQuestion.QuestionType,
                    Response = r.Response,
                    OptionText = r.Option != null ? r.Option.OptionText : null
                })
                .ToList();

            return responses;
        }

        public List<TopicFeedbackResponseDetailsViewModel> GetTopicFeedbackResponses(Guid topicId)
        {
            var responses = _context.Feedbackresponses
                .Include(r => r.TopicFeedbackQuestion)
                .Include(r => r.Option)
                .Where(r => r.TopicFeedbackQuestion.TopicId == topicId)
                .Select(r => new TopicFeedbackResponseDetailsViewModel
                {
                    TopicFeedbackQuestionId = r.TopicFeedbackQuestionId,
                    LearnerId = r.LearnerId,
                    Question = r.TopicFeedbackQuestion.Question,
                    QuestionType = r.TopicFeedbackQuestion.QuestionType,
                    Response = r.Response,
                    OptionText = r.Option != null ? r.Option.OptionText : null
                })
                .ToList();

            return responses;
        }

        public List<QuizFeedbackResponseDetailsViewModel> GetQuizFeedbackResponsesByLearner(Guid quizId, Guid learnerId)
        {
            var responses = _context.Feedbackresponses
                .Include(r => r.QuizFeedbackQuestion)
                .Include(r => r.Option)
                .Where(r => r.QuizFeedbackQuestion.QuizId == quizId && r.LearnerId == learnerId)
                .Select(r => new QuizFeedbackResponseDetailsViewModel
                {
                    QuizFeedbackQuestionId = r.QuizFeedbackQuestionId,
                    LearnerId = r.LearnerId,
                    Question = r.QuizFeedbackQuestion.Question,
                    QuestionType = r.QuizFeedbackQuestion.QuestionType,
                    Response = r.Response,
                    OptionText = r.Option != null ? r.Option.OptionText : null
                })
                .ToList();

            return responses;
        }

        public List<TopicFeedbackResponseDetailsViewModel> GetTopicFeedbackResponsesByLearner(Guid topicId, Guid learnerId)
        {
            var responses = _context.Feedbackresponses
                .Include(r => r.TopicFeedbackQuestion)
                .Include(r => r.Option)
                .Where(r => r.TopicFeedbackQuestion.TopicId == topicId && r.LearnerId == learnerId)
                .Select(r => new TopicFeedbackResponseDetailsViewModel
                {
                    TopicFeedbackQuestionId = r.TopicFeedbackQuestionId,
                    LearnerId = r.LearnerId,
                    Question = r.TopicFeedbackQuestion.Question,
                    QuestionType = r.TopicFeedbackQuestion.QuestionType,
                    Response = r.Response,
                    OptionText = r.Option != null ? r.Option.OptionText : null
                })
                .ToList();

            return responses;
        }
    }
}

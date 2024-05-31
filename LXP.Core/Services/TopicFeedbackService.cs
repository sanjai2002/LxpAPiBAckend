
using LXP.Common.ViewModels.TopicFeedbackQuestionViemModel;
using LXP.Core.IServices;
using LXP.Data.IRepository;

namespace LXP.Core.Services
{
    public class TopicFeedbackService : ITopicFeedbackService
    {
        private readonly ITopicFeedbackRepository _repository;

        public TopicFeedbackService(ITopicFeedbackRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<TopicFeedbackQuestionNoViewModel> GetAllFeedbackQuestions()
        {
            return _repository.GetAllFeedbackQuestions();
        }

        public TopicFeedbackQuestionNoViewModel GetFeedbackQuestionById(Guid id)
        {
            return _repository.GetFeedbackQuestionById(id);
        }


        public bool AddFeedbackQuestion(TopicFeedbackQuestionViewModel question, List<FeedbackOptionDTO> options)
        {
            return _repository.AddFeedbackQuestion(question, options);
        }

        public bool UpdateFeedbackQuestion(Guid id, TopicFeedbackQuestionViewModel question, List<FeedbackOptionDTO> options)
        {
            if (question == null)
                return false;

            if (string.IsNullOrEmpty(question.Question))
                return false;

            if (question.QuestionType == "MCQ" && (question.Options == null || question.Options.Count < 2 || question.Options.Count > 5))
                return false;

            return _repository.UpdateFeedbackQuestion(id, question, options);
        }

        public bool DeleteFeedbackQuestion(Guid id)
        {
            return _repository.DeleteFeedbackQuestion(id);
        }

        public IEnumerable<TopicFeedbackQuestionNoViewModel> GetFeedbackQuestionsByTopicId(Guid topicId)
        {
            return _repository.GetFeedbackQuestionsByTopicId(topicId);
        }
    }
}


using LXP.Common.ViewModels.QuizFeedbackQuestionViewModel;
using LXP.Core.IServices;
using LXP.Data.IRepository;


namespace LXP.Core.Services
{
    public class QuizFeedbackService : IQuizFeedbackService
    {
        private readonly IQuizFeedbackRepository _quizFeedbackRepository;

        public QuizFeedbackService(IQuizFeedbackRepository quizFeedbackRepository)
        {
            _quizFeedbackRepository = quizFeedbackRepository;
        }

        public Guid AddFeedbackQuestion(QuizfeedbackquestionViewModel quizfeedbackquestionDto, List<QuizFeedbackQuestionsOptionViewModel> options)
        {            
            return _quizFeedbackRepository.AddFeedbackQuestion(quizfeedbackquestionDto, options);
        }

        public List<QuizfeedbackquestionNoViewModel> GetAllFeedbackQuestions()
        {           
            return _quizFeedbackRepository.GetAllFeedbackQuestions();
        }

        
        

        public QuizfeedbackquestionNoViewModel GetFeedbackQuestionById(Guid quizFeedbackQuestionId)
        {          
            return _quizFeedbackRepository.GetFeedbackQuestionById(quizFeedbackQuestionId);
        }

        public bool UpdateFeedbackQuestion(Guid quizFeedbackQuestionId, QuizfeedbackquestionViewModel quizfeedbackquestionDto, List<QuizFeedbackQuestionsOptionViewModel> options)
        {        
            return _quizFeedbackRepository.UpdateFeedbackQuestion(quizFeedbackQuestionId, quizfeedbackquestionDto, options);
        }

        public bool DeleteFeedbackQuestion(Guid quizFeedbackQuestionId)
        {
            return _quizFeedbackRepository.DeleteFeedbackQuestion(quizFeedbackQuestionId);
        }
        public List<QuizfeedbackquestionNoViewModel> GetFeedbackQuestionsByQuizId(Guid quizId)
        {
            return _quizFeedbackRepository.GetFeedbackQuestionsByQuizId(quizId);
        }

        public bool DeleteFeedbackQuestionsByQuizId(Guid quizId) 
        {
            try
            {
                var questions = _quizFeedbackRepository.GetFeedbackQuestionsByQuizId(quizId);
                if (questions == null || questions.Count == 0)
                    return false;

                foreach (var question in questions)
                {
                    _quizFeedbackRepository.DeleteFeedbackQuestion(question.QuizFeedbackQuestionId);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}


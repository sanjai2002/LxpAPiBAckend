using LXP.Common.DTO;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using System;
using System.Collections.Generic;

namespace LXP.Core.Services
{
    public class QuizFeedbackService : IQuizFeedbackService
    {
        private readonly IQuizFeedbackRepository _quizFeedbackRepository;

        public QuizFeedbackService(IQuizFeedbackRepository quizFeedbackRepository)
        {
            _quizFeedbackRepository = quizFeedbackRepository;
        }

        public Guid AddFeedbackQuestion(QuizFeedbackQuestionDto quizfeedbackquestionDto, List<QuizFeedbackQuestionsOptionDto> options)
        {            
            return _quizFeedbackRepository.AddFeedbackQuestion(quizfeedbackquestionDto, options);
        }

        public List<QuizFeedbackQuestionNoDto> GetAllFeedbackQuestions()
        {           
            return _quizFeedbackRepository.GetAllFeedbackQuestions();
        }

        public QuizFeedbackQuestionNoDto GetFeedbackQuestionById(Guid quizFeedbackQuestionId)
        {          
            return _quizFeedbackRepository.GetFeedbackQuestionById(quizFeedbackQuestionId);
        }

        public bool UpdateFeedbackQuestion(Guid quizFeedbackQuestionId, QuizFeedbackQuestionDto quizfeedbackquestionDto, List<QuizFeedbackQuestionsOptionDto> options)
        {        
            return _quizFeedbackRepository.UpdateFeedbackQuestion(quizFeedbackQuestionId, quizfeedbackquestionDto, options);
        }

        public bool DeleteFeedbackQuestion(Guid quizFeedbackQuestionId)
        {
            return _quizFeedbackRepository.DeleteFeedbackQuestion(quizFeedbackQuestionId);
        }
        public List<QuizFeedbackQuestionNoDto> GetFeedbackQuestionsByQuizId(Guid quizId)
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

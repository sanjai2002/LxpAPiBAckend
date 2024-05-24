using LXP.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.IServices
{
    public interface IQuizFeedbackService
    {
        Guid AddFeedbackQuestion(QuizFeedbackQuestionDto quizfeedbackquestionDto, List<QuizFeedbackQuestionsOptionDto> options);
       
        List<QuizFeedbackQuestionNoDto> GetAllFeedbackQuestions();

        QuizFeedbackQuestionNoDto GetFeedbackQuestionById(Guid QuizFeedbackQuestionId);
    
        bool UpdateFeedbackQuestion(Guid quizFeedbackQuestionId, QuizFeedbackQuestionDto quizfeedbackquestionDto, List<QuizFeedbackQuestionsOptionDto> options);
    
        bool DeleteFeedbackQuestion(Guid quizFeedbackQuestionId);
        List<QuizFeedbackQuestionNoDto> GetFeedbackQuestionsByQuizId(Guid quizId);
        bool DeleteFeedbackQuestionsByQuizId(Guid quizId);
    }
}

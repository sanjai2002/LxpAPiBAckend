using System;
using System.Collections.Generic;
using LXP.Common.ViewModels;

namespace LXP.Data.IRepository
{
    public interface IQuizFeedbackRepository
    {
        Guid AddFeedbackQuestion(QuizFeedbackQuestionDto quizfeedbackquestionDto, List<QuizFeedbackQuestionsOptionDto> options);
        List<QuizFeedbackQuestionNoDto> GetAllFeedbackQuestions();
        
        int GetNextFeedbackQuestionNo(Guid quizId);
        Guid AddFeedbackQuestionOption(QuizFeedbackQuestionsOptionDto feedbackquestionsoptionDto, Guid QuizFeedbackQuestionId);
        List<QuizFeedbackQuestionsOptionDto> GetFeedbackQuestionOptionsById(Guid QuizFeedbackQuestionId);
        QuizFeedbackQuestionNoDto GetFeedbackQuestionById(Guid QuizFeedbackQuestionId);
        bool ValidateOptionsByFeedbackQuestionType(string questionType, List<QuizFeedbackQuestionsOptionDto> options);
        bool UpdateFeedbackQuestion(Guid QuizFeedbackQuestionId, QuizFeedbackQuestionDto quizfeedbackquestionDto, List<QuizFeedbackQuestionsOptionDto> options);
        bool DeleteFeedbackQuestion(Guid QuizFeedbackQuestionId);
        List<QuizFeedbackQuestionNoDto> GetFeedbackQuestionsByQuizId(Guid quizId);

        bool DeleteFeedbackQuestionsByQuizId(Guid quizId);

    }
}


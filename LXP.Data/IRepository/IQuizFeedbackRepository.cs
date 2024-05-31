using System;
using System.Collections.Generic;

using LXP.Common.ViewModels.QuizFeedbackQuestionViewModel;

namespace LXP.Data.IRepository
{
    public interface IQuizFeedbackRepository
    {
        Guid AddFeedbackQuestion(QuizfeedbackquestionViewModel quizfeedbackquestionDto, List<QuizFeedbackQuestionsOptionViewModel> options);
        List<QuizfeedbackquestionNoViewModel> GetAllFeedbackQuestions();
        int GetNextFeedbackQuestionNo(Guid quizId);
        Guid AddFeedbackQuestionOption(QuizFeedbackQuestionsOptionViewModel feedbackquestionsoptionDto, Guid QuizFeedbackQuestionId);
        List<QuizFeedbackQuestionsOptionViewModel> GetFeedbackQuestionOptionsById(Guid QuizFeedbackQuestionId);
        QuizfeedbackquestionNoViewModel GetFeedbackQuestionById(Guid QuizFeedbackQuestionId);
        bool ValidateOptionsByFeedbackQuestionType(string questionType, List<QuizFeedbackQuestionsOptionViewModel> options);
        bool UpdateFeedbackQuestion(Guid QuizFeedbackQuestionId, QuizfeedbackquestionViewModel quizfeedbackquestionDto, List<QuizFeedbackQuestionsOptionViewModel> options);
        bool DeleteFeedbackQuestion(Guid QuizFeedbackQuestionId);
        List<QuizfeedbackquestionNoViewModel> GetFeedbackQuestionsByQuizId(Guid quizId);
        bool DeleteFeedbackQuestionsByQuizId(Guid quizId);
        

    }
}


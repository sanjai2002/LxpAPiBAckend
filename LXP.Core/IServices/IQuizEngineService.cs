using LXP.Common.ViewModels.QuizEngineViewModel;
using System;
using System.Collections.Generic;

public interface IQuizEngineService
{
  
    ViewQuizDetailsViewModel GetQuizById(Guid quizId);
    IEnumerable<QEQuizQuestionViewModel> GetQuestionsForQuiz(Guid quizId);
    Guid StartQuizAttempt(Guid learnerId, Guid quizId);
    void SubmitAnswer(AnswerSubmissionModel answerSubmissionModel);
    LearnerAttemptDetailsViewModel GetQuizAttemptDetails(Guid attemptId);
    Guid RetakeQuiz(Guid learnerId, Guid quizId);

    ViewQuizDetailsViewModel GetQuizDetailsByTopicId(Guid topicId);
}

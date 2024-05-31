using LXP.Common.ViewModels.QuizEngineViewModel;

public interface IQuizEngineRepository
{
    
    ViewQuizDetailsViewModel GetQuizById(Guid quizId);
    IEnumerable<QEQuizQuestionViewModel> GetQuestionsForQuiz(Guid quizId);
    bool IsQuestionOptionCorrect(Guid quizQuestionId, Guid questionOptionId);
    string GetQuestionTypeById(Guid quizQuestionId);
    IEnumerable<string> GetCorrectOptionsForQuestion(Guid quizQuestionId);
    LearnerAttemptViewModel CreateLearnerAttempt(Guid learnerId, Guid quizId);
    LearnerAttemptDetailsViewModel GetLearnerAttemptDetailsById(Guid attemptId);
    LearnerAttemptViewModel GetLearnerAttemptById(Guid attemptId);
    void UpdateLearnerAttempt(LearnerAttemptViewModel attempt);
    bool IsAllowedToAttemptQuiz(Guid learnerId, Guid quizId);
    LearnerAnswerDTO CreateLearnerAnswer(AnswerSubmissionModel answerSubmissionModel);

    public Guid GetOptionIdByText(Guid quizQuestionId, string optionText);//new line

    ViewQuizDetailsViewModel GetQuizDetailsByTopicId(Guid topicId);


    //Guid GetQuestionOptionIdByOption(Guid quizQuestionId, string option);//new line 
}

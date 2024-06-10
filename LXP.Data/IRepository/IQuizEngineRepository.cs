using LXP.Common.ViewModels.QuizEngineViewModel;
namespace LXP.Data.IRepository
{
    public interface IQuizEngineRepository
    {
        

        Task<ViewQuizDetailsViewModel> GetQuizByIdAsync(Guid quizId);
        Task<IEnumerable<QuizEngineQuestionViewModel>> GetQuestionsForQuizAsync(Guid quizId);
        Task<bool> IsQuestionOptionCorrectAsync(Guid quizQuestionId, Guid questionOptionId);
        Task<string> GetQuestionTypeByIdAsync(Guid quizQuestionId);
        Task<IEnumerable<string>> GetCorrectOptionsForQuestionAsync(Guid quizQuestionId);
        Task<LearnerAttemptViewModel> CreateLearnerAttemptAsync(Guid learnerId, Guid quizId, DateTime startTime);
        Task<LearnerAttemptViewModel> GetLearnerAttemptByIdAsync(Guid attemptId);
        Task UpdateLearnerAttemptAsync(LearnerAttemptViewModel attempt);
        Task<bool> IsAllowedToAttemptQuizAsync(Guid learnerId, Guid quizId);
        Task<Guid> GetOptionIdByTextAsync(Guid quizQuestionId, string optionText);
        Task<ViewQuizDetailsViewModel> GetQuizDetailsByTopicIdAsync(Guid topicId);
        Task<IEnumerable<LearnerAttemptViewModel>> GetLearnerAttemptsForQuizAsync(Guid learnerId, Guid quizId);
        Task<IEnumerable<LearnerAnswerViewModel>> GetLearnerAnswersForAttemptAsync(Guid attemptId);
        Task UpdateLearnerAnswerAsync(Guid learnerAnswerId, Guid questionOptionId);
        Task<string> GetOptionTextByIdAsync(Guid optionId);
        Task<IEnumerable<string>> GetQuestionOptionsAsync(Guid quizQuestionId);
        Task ClearLearnerAnswersAsync(Guid attemptId, Guid quizQuestionId);//newly added
        Task<IEnumerable<LearnerAnswerViewModel>> GetLearnerAnswersByAttemptAndQuestionAsync(Guid attemptId, Guid quizQuestionId);//newly added
        Task CreateLearnerAnswerAsync(Guid learnerAttemptId, Guid quizQuestionId, Guid questionOptionId);//NEW


        //
        Task<LearnerQuizAttemptViewModel> GetLearnerQuizAttemptAsync(Guid attemptId);
        Task<LearnerQuizAttemptResultViewModel> GetLearnerQuizAttemptResultAsync(Guid attemptId);

        // new batch

         Task SaveLearnerAnswerAsync(LearnerAnswerViewModel learnerAnswer);

        Task<QuizEngineQuestionViewModel> GetQuizQuestionByIdAsync(Guid quizQuestionId);

        // cache

        Task SaveCachedAnswersAsync(Guid learnerAttemptId, Dictionary<Guid, List<string>> questionAnswers);
        Task SubmitAnswerAsync(AnswerSubmissionModel answerSubmissionModel);
    }
}



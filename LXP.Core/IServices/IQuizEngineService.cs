using LXP.Common.ViewModels.QuizEngineViewModel;

namespace LXP.Core.IServices
{
    public interface IQuizEngineService
    {
        
        Task<ViewQuizDetailsViewModel> GetQuizByIdAsync(Guid quizId);
        Task<IEnumerable<QuizEngineQuestionViewModel>> GetQuestionsForQuizAsync(Guid quizId);
        Task<Guid> StartQuizAttemptAsync(Guid learnerId, Guid quizId);
        Task SubmitAnswerAsync(AnswerSubmissionModel answerSubmissionModel);
        Task<Guid> RetakeQuizAsync(Guid learnerId, Guid quizId);
        Task<ViewQuizDetailsViewModel> GetQuizDetailsByTopicIdAsync(Guid topicId);
        Task SubmitQuizAttemptAsync(Guid attemptId);


        //
        Task<LearnerQuizAttemptViewModel> GetLearnerQuizAttemptAsync(Guid attemptId);
        Task<LearnerQuizAttemptResultViewModel> GetLearnerQuizAttemptResultAsync(Guid attemptId);
        //new batch
        Task SubmitAnswerBatchAsync(AnswerSubmissionBatchModel model);
        //cache
       Task CacheAnswersAsync(CachedAnswerSubmissionModel model);
       Task SubmitCachedAnswersAsync(Guid learnerAttemptId);
    }


}
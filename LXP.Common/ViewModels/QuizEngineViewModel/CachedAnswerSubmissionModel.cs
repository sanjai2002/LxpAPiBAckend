namespace LXP.Common.ViewModels.QuizEngineViewModel
{
    public class CachedAnswerSubmissionModel
    {
        public Guid LearnerAttemptId { get; set; }
        public Dictionary<Guid, List<string>> QuestionAnswers { get; set; } = new Dictionary<Guid, List<string>>();
    }
}

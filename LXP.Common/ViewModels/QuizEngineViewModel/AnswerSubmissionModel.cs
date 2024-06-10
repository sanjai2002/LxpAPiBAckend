namespace LXP.Common.ViewModels.QuizEngineViewModel
{
    public class AnswerSubmissionModel
    {
        public Guid LearnerAttemptId { get; set; }
        public Guid QuizQuestionId { get; set; }
        public List<string> SelectedOptions { get; set; } = new List<string>();
    }
}
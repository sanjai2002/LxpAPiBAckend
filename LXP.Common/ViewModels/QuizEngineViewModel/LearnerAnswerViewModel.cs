namespace LXP.Common.ViewModels.QuizEngineViewModel
{
    public class LearnerAnswerViewModel
    {
        public Guid LearnerAnswerId { get; set; }
        public Guid LearnerAttemptId { get; set; }
        public Guid QuizQuestionId { get; set; }
        public Guid QuestionOptionId { get; set; }
    }
}
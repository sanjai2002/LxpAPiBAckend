namespace LXP.Common.ViewModels.QuizEngineViewModel
{
    public class LearnerQuizAttemptViewModel
    {
        public Guid QuizId { get; set; }
        public Guid TopicId { get; set; }

        public Guid LearnerAttemptId { get; set; }
        public List<QuestionResponseViewModel> QuestionResponses { get; set; } = new List<QuestionResponseViewModel>();
    }
}
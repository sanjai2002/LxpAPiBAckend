namespace LXP.Common.ViewModels.QuizEngineViewModel
{
    public class LearnerAttemptViewModel
    {
        public Guid LearnerAttemptId { get; set; }
        public Guid LearnerId { get; set; }
        public Guid QuizId { get; set; }
        public int AttemptCount { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public float Score { get; set; }
    }
}
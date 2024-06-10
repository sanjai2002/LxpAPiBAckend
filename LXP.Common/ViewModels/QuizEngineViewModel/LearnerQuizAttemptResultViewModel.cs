

namespace LXP.Common.ViewModels.QuizEngineViewModel
{
    public class LearnerQuizAttemptResultViewModel
    {
        public Guid QuizId { get; set; }
        public Guid TopicId { get; set; }
        public Guid LearnerAttemptId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        // public TimeSpan TimeTaken { get; set; }
        public double TimeTaken { get; set; }
        public int CurrentAttempt { get; set; }
        public int AttemptsRemaining { get; set; }
        public float Score { get; set; }
        public bool IsPassed { get; set; }
    }
}
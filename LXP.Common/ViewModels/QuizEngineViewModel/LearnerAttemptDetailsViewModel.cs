namespace LXP.Common.ViewModels.QuizEngineViewModel
{

public class LearnerAttemptDetailsViewModel
{
    public Guid LearnerAttemptId { get; set; }
    public Guid LearnerId { get; set; }
    public Guid QuizId { get; set; }
    public float Score { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public List<QuestionAndAnswerDetails> QuestionsAndAnswers { get; set; } = new List<QuestionAndAnswerDetails>();
}

}

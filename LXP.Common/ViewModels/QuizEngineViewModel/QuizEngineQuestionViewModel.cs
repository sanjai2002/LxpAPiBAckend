namespace LXP.Common.ViewModels.QuizEngineViewModel

{
    public class QuizEngineQuestionViewModel
    {
        public Guid QuizQuestionId { get; set; }
        public string? Question { get; set; }
        public string? QuestionType { get; set; }
        public int QuestionNo { get; set; }
        public List<QuizEngineOptionViewModel>? Options { get; set; }
    }
}
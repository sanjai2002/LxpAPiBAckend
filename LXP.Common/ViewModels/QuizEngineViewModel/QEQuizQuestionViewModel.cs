namespace LXP.Common.ViewModels.QuizEngineViewModel

{
    public class QEQuizQuestionViewModel
    {
        public Guid QuizQuestionId { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public int QuestionNo { get; set; }
        public List<QEQuestionOptionViewModel> Options { get; set; }
    }
}


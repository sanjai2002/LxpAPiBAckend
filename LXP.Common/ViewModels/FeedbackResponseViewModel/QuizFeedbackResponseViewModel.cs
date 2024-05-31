
namespace LXP.Common.ViewModels.FeedbackResponseViewModel
{
    public class QuizFeedbackResponseViewModel
    {
        public Guid QuizFeedbackQuestionId { get; set; }
        public Guid LearnerId { get; set; }
        public string? Response { get; set; }
        public string? OptionText { get; set; }
    }
}


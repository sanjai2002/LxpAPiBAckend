namespace LXP.Common.ViewModels.FeedbackResponseViewModel
{
    public class TopicFeedbackResponseDetailsViewModel
    {
        public Guid TopicFeedbackQuestionId { get; set; }
        public Guid LearnerId { get; set; }
        public string? Question { get; set; }
        public string? QuestionType { get; set; }
        public string? Response { get; set; }
        public string? OptionText { get; set; }
    }
}
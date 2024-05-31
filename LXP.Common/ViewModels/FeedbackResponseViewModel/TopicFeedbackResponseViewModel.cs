
namespace LXP.Common.ViewModels.FeedbackResponseViewModel
{
    public class TopicFeedbackResponseViewModel
    {
        public Guid TopicFeedbackQuestionId { get; set; }
        public Guid LearnerId { get; set; }
        public string? Response { get; set; }
        public string? OptionText { get; set; }
    }
}


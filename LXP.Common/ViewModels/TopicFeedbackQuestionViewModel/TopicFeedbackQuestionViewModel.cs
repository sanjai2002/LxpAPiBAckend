namespace LXP.Common.ViewModels.TopicFeedbackQuestionViemModel
{
    public class TopicFeedbackQuestionViewModel
    {
        public Guid TopicId { get; set; }
        public string? Question { get; set; }
        public string? QuestionType { get; set; }
        public List<FeedbackOptionDTO>? Options { get; set; }
    }
}

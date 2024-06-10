

namespace LXP.Common.ViewModels.TopicFeedbackQuestionViemModel
{

    public class TopicFeedbackQuestionNoViewModel
    {
        public Guid TopicFeedbackQuestionId { get; set; }
        public Guid TopicId { get; set; }
        public int QuestionNo { get; set; }
        public string Question { get; set; } = null!;
        public string QuestionType { get; set; } = null!;
        public List<TopicFeedbackQuestionsOptionViewModel> Options { get; set; } = new List<TopicFeedbackQuestionsOptionViewModel>();
    }
}

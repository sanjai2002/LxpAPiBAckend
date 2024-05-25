using System;
using System.Collections.Generic;

namespace LXP.Common.Entities;

public partial class Feedbackquestionsoption
{
    public Guid FeedbackQuestionOptionId { get; set; }

    public Guid? QuizFeedbackQuestionId { get; set; }

    public Guid? TopicFeedbackQuestionId { get; set; }

    public string OptionText { get; set; } = null!;

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual Quizfeedbackquestion? QuizFeedbackQuestion { get; set; }

    public virtual Topicfeedbackquestion? TopicFeedbackQuestion { get; set; }
}

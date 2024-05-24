using System;
using System.Collections.Generic;

namespace LXP.Common.Entities;

public partial class Topicfeedbackquestion
{
    public Guid TopicFeedbackQuestionId { get; set; }

    public Guid TopicId { get; set; }

    public int QuestionNo { get; set; }

    public string Question { get; set; } = null!;

    public string QuestionType { get; set; } = null!;

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<Feedbackquestionsoption> Feedbackquestionsoptions { get; set; } = new List<Feedbackquestionsoption>();

    public virtual ICollection<Feedbackresponse> Feedbackresponses { get; set; } = new List<Feedbackresponse>();

    public virtual Topic Topic { get; set; } = null!;
}

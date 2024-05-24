using System;
using System.Collections.Generic;

namespace LXP.Common.Entities;

public partial class FeedbackQuestion
{
    public Guid FeedbackQuestionId { get; set; }

    public Guid TopicId { get; set; }

    public int QuestionNo { get; set; }

    public string Question { get; set; } = null!;

    public string QuestionType { get; set; } = null!;

    public string FeedbackType { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<FeedbackQuestionOption> FeedbackQuestionOptions { get; set; } = new List<FeedbackQuestionOption>();

    public virtual ICollection<FeedbackResponse> FeedbackResponses { get; set; } = new List<FeedbackResponse>();

    public virtual Topic Topic { get; set; } = null!;
}

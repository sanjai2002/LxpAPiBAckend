using System;
using System.Collections.Generic;

namespace LXP.Common.Entities;

public partial class FeedbackQuestionOption
{
    public Guid OptionId { get; set; }

    public Guid FeedbackQuestionId { get; set; }

    public string Option { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual FeedbackQuestion FeedbackQuestion { get; set; } = null!;

    public virtual ICollection<FeedbackResponse> FeedbackResponses { get; set; } = new List<FeedbackResponse>();
}

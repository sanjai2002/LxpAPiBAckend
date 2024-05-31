using System;
using System.Collections.Generic;
namespace LXP.Common.Entities;
public partial class LearnerAnswer
{
    public Guid LearnerAnswerId { get; set; }

    public Guid LearnerAttemptId { get; set; }

    public Guid QuizQuestionId { get; set; }

    public Guid QuestionOptionId { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual LearnerAttempt LearnerAttempt { get; set; } = null!;

    public virtual QuestionOption QuestionOption { get; set; } = null!;

    public virtual QuizQuestion QuizQuestion { get; set; } = null!;
}

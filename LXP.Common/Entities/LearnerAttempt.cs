using System;
using System.Collections.Generic;

namespace LXP.Common.Entities;

public partial class LearnerAttempt
{
    public Guid LearnerAttemptId { get; set; }

    public Guid LearnerId { get; set; }

    public Guid QuizId { get; set; }

    public int AttemptCount { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public float Score { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual Learner Learner { get; set; } = null!;

    public virtual ICollection<LearnerAnswer> LearnerAnswers { get; set; } = new List<LearnerAnswer>();

    public virtual Quiz Quiz { get; set; } = null!;
}

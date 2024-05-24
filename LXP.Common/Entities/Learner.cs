using System;
using System.Collections.Generic;

namespace LXP.Common.Entities;

public partial class Learner
{
    public Guid LearnerId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public bool UnblockRequest { get; set; }

    public bool AccountStatus { get; set; }

    public DateTime UserLastLogin { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual ICollection<FeedbackResponse> FeedbackResponses { get; set; } = new List<FeedbackResponse>();

    public virtual ICollection<LearnerAttempt> LearnerAttempts { get; set; } = new List<LearnerAttempt>();

    public virtual ICollection<LearnerProfile> LearnerProfiles { get; set; } = new List<LearnerProfile>();

    public virtual ICollection<LearnerProgress> LearnerProgresses { get; set; } = new List<LearnerProgress>();

    public virtual ICollection<PasswordHistory> PasswordHistories { get; set; } = new List<PasswordHistory>();
}

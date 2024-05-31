using System;
using System.Collections.Generic;

namespace LXP.Common.Entities;

public partial class LearnerProfile
{
    public Guid ProfileId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly Dob { get; set; }

    public string Gender { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public string Stream { get; set; } = null!;

    public string? ProfilePhoto { get; set; }

    public Guid LearnerId { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual Learner Learner { get; set; } = null!;
}

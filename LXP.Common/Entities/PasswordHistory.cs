using System;
using System.Collections.Generic;
namespace LXP.Common.Entities;

public partial class PasswordHistory
{
    public Guid PasswordId { get; set; }

    public Guid LearnerId { get; set; }

    public string? OldPassword { get; set; }

    public string NewPassword { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual Learner Learner { get; set; } = null!;
}

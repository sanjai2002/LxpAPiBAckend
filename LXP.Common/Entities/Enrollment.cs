using System;
using System.Collections.Generic;

namespace LXP.Common.Entities;

public partial class Enrollment
{
    public Guid EnrollmentId { get; set; }

    public Guid LearnerId { get; set; }

    public Guid CourseId { get; set; }

    public DateTime EnrollmentDate { get; set; }

    public bool EnrollStatus { get; set; }

    public bool EnrollRequestStatus { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Learner Learner { get; set; } = null!;
}

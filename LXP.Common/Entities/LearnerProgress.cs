using System;
using System.Collections.Generic;

namespace LXP.Common.Entities;

public partial class LearnerProgress
{
    public Guid LearnerProgressId { get; set; }

    public Guid CourseId { get; set; }

    public Guid TopicId { get; set; }

    public Guid MaterialId { get; set; }

    public Guid LearnerId { get; set; }

    public TimeOnly WatchTime { get; set; }

    public TimeOnly TotalTime { get; set; }

    public bool IsWatched { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Learner Learner { get; set; } = null!;

    public virtual Material Material { get; set; } = null!;

    public virtual Topic Topic { get; set; } = null!;
}

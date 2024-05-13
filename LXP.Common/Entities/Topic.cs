using System;
using System.Collections.Generic;

namespace LXP.Common.Entities;

public partial class Topic
{
    public Guid TopicId { get; set; }

    public Guid CourseId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<FeedbackQuestion> FeedbackQuestions { get; set; } = new List<FeedbackQuestion>();

    public virtual ICollection<LearnerProgress> LearnerProgresses { get; set; } = new List<LearnerProgress>();

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();

    public virtual Quiz? Quiz { get; set; }
}

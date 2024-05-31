using System;
using System.Collections.Generic;
namespace LXP.Common.Entities;

public partial class Material
{
    public Guid MaterialId { get; set; }

    public Guid TopicId { get; set; }

    public Guid MaterialTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public decimal Duration { get; set; }

    public bool IsActive { get; set; }

    public bool IsAvailable { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<LearnerProgress> LearnerProgresses { get; set; } = new List<LearnerProgress>();

    public virtual MaterialType MaterialType { get; set; } = null!;

    public virtual Topic Topic { get; set; } = null!;
}

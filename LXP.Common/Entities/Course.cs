using System;
using System.Collections.Generic;
namespace LXP.Common.Entities;

public partial class Course
{
    public Guid CourseId { get; set; }

    public Guid LevelId { get; set; }

    public Guid CategoryId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Duration { get; set; }

    public string Thumbnail { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsAvailable { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual CourseCategory Category { get; set; } = null!;

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual ICollection<LearnerProgress> LearnerProgresses { get; set; } = new List<LearnerProgress>();

    public virtual CourseLevel Level { get; set; } = null!;

    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
}

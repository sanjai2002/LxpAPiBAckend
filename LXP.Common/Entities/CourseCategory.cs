using System;
using System.Collections.Generic;

namespace LXP.Common.Entities;

public partial class CourseCategory
{
    public Guid CatagoryId { get; set; }

    public string Category { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}

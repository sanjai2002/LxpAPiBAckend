using System;
using System.Collections.Generic;

namespace LXP.Common.Entities;

public partial class QuestionOption
{
    public Guid QuestionOptionId { get; set; }

    public Guid QuizQuestionId { get; set; }

    public string Option { get; set; } = null!;

    public bool IsCorrect { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<LearnerAnswer> LearnerAnswers { get; set; } = new List<LearnerAnswer>();

    public virtual QuizQuestion QuizQuestion { get; set; } = null!;
}

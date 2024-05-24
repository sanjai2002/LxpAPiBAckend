using System;
using System.Collections.Generic;

namespace LXP.Common.Entities;

public partial class QuizQuestion
{
    public Guid QuizQuestionId { get; set; }

    public Guid QuizId { get; set; }

    public int QuestionNo { get; set; }

    public string Question { get; set; } = null!;

    public string QuestionType { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<LearnerAnswer> LearnerAnswers { get; set; } = new List<LearnerAnswer>();

    public virtual ICollection<QuestionOption> QuestionOptions { get; set; } = new List<QuestionOption>();

    public virtual Quiz Quiz { get; set; } = null!;
}

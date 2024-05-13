using System;
using System.Collections.Generic;

namespace LXP.Common.Entities;

public partial class Quiz
{
    public Guid QuizId { get; set; }

    public Guid CourseId { get; set; }

    public Guid TopicId { get; set; }

    public string NameOfQuiz { get; set; } = null!;

    public int Duration { get; set; }

    public int PassMark { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<LearnerAttempt> LearnerAttempts { get; set; } = new List<LearnerAttempt>();

    public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();

    public virtual Topic Topic { get; set; } = null!;
}

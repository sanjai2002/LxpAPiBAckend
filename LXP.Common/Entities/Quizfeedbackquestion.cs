using System;
using System.Collections.Generic;

namespace LXP.Common.Entities;

public partial class Quizfeedbackquestion
{
    public Guid QuizFeedbackQuestionId { get; set; }

    public Guid QuizId { get; set; }

    public int QuestionNo { get; set; }

    public string Question { get; set; } = null!;

    public string QuestionType { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<Feedbackquestionsoption> Feedbackquestionsoptions { get; set; } = new List<Feedbackquestionsoption>();

    public virtual ICollection<Feedbackresponse> Feedbackresponses { get; set; } = new List<Feedbackresponse>();

    public virtual Quiz Quiz { get; set; } = null!;
}

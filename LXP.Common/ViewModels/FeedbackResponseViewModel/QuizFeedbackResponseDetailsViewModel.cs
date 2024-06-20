﻿namespace LXP.Common.ViewModels.FeedbackResponseViewModel
{
    public class QuizFeedbackResponseDetailsViewModel
    {
        public Guid QuizFeedbackQuestionId { get; set; }
        public Guid LearnerId { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public string Response { get; set; }
        public string OptionText { get; set; }
        public string LearnerName { get; set; } // Added property
        public string QuizName { get; set; }
    }
}


//public class QuizFeedbackResponseDetailsViewModel
//{
//    public Guid QuizFeedbackQuestionId { get; set; }
//    public Guid LearnerId { get; set; }
//    public string Question { get; set; }
//    public string QuestionType { get; set; }
//    public string Response { get; set; }
//    public string OptionText { get; set; }
//}

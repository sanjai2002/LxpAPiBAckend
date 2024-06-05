using LXP.Common.Entities;

namespace LXP.Common.ViewModels
{
    public class QuizReportViewModel
    {
        public string CourseName { get; set; }
        public string TopicName { get; set; }
        public string QuizName { get; set; }
        public int NoOfPassedUsers { get; set; }
        public int NoOfFailedUsers { get;set; }
        public float AverageScore { get; set; }

        public Guid QuizId { get; set; }
    }
}

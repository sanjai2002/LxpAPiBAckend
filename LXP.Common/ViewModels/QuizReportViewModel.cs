using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class QuizScorelearnerViewModel
    {
        public Guid LearnerId { get; set; }
        public string? LearnerName { get; set; }
        public string ? Profilephoto {  get; set; }
        public int TotalNoofQuizAttempts { get; set; }
        public int LearnerAttempts { get; set; }
        public float Score { get; set; }


    }
}

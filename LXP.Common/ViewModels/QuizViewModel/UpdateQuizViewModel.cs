using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels.QuizViewModel
{
    public class UpdateQuizViewModel
    {
        public string NameOfQuiz { get; set; } = null!;
        public int Duration { get; set; }
        public int? AttemptsAllowed { get; set; } 
        public int PassMark { get; set; }
    }
}
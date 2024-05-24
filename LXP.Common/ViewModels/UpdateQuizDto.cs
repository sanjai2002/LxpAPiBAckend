using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class UpdateQuizDto
    {
        public string NameOfQuiz { get; set; } = null!;
        public int Duration { get; set; }
        public int? AttemptsAllowed { get; set; } // Added
        public int PassMark { get; set; }
    }


}
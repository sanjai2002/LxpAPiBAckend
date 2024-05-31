using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels.QuizQuestionViewModel
{
    public class QuizQuestionNoViewModel
    {
        public Guid QuizQuestionId { get; set; } 
        public Guid QuizId { get; set; }
        public int QuestionNo { get; set; }
        public string Question { get; set; } = null!;
        public string QuestionType { get; set; } = null!; 
        public List<QuestionOptionViewModel> Options { get; set; } = new List<QuestionOptionViewModel>();
    }
}












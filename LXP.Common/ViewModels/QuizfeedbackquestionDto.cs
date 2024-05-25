using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class QuizFeedbackQuestionDto
    {
        public Guid QuizId { get; set; }



        public string Question { get; set; } = null!;

        public string QuestionType { get; set; } = null!;

        public List<QuizFeedbackQuestionsOptionDto> Options { get; set; } = new List<QuizFeedbackQuestionsOptionDto>();
    }
}

//public int QuestionNo { get; set; }
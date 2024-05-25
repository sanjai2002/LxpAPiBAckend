using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.DTO
{
    public class QuizFeedbackResponseViewModel
    {

        public Guid QuizFeedbackQuestionId { get; set; }

        public string? Response { get; set; }

        //public int? OptionId { get; set; }


    }
}

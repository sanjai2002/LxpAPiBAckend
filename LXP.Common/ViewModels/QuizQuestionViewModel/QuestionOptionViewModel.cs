using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels.QuizQuestionViewModel
{ 
    public class QuestionOptionViewModel
    {  
        public string Option { get; set; } = null!;
        public bool IsCorrect { get; set; }
       
    }
   
}


// public Guid QuestionOptionId { get; set; } = new Guid();
// public Guid QuizQuestionId { get; set; }
// public string CreatedBy { get; set; } = null!;
// public DateTime CreatedAt { get; set; }
// public string? ModifiedBy { get; set; }
// public DateTime? ModifiedAt { get; set; }

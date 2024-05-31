using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels.QuizViewModel
{

  public class CreateQuizViewModel
  {
    public string? NameOfQuiz { get; set; }
    public int Duration { get; set; }
    public int PassMark { get; set; }
    public int? AttemptsAllowed { get; set; }
    public Guid TopicId { get; set; } 
    }

}






//using System;

//namespace LXP.Common.DTO
//{
//    public class CreateQuizDto
//    {
//        public string NameOfQuiz { get; set; } = null!;
//        public int Duration { get; set; }
//        public int? AttemptsAllowed { get; set; }
//        public int PassMark { get; set; }
//        public Guid CourseId { get; set; }  // Added
//        public Guid TopicId { get; set; }   // Added
//    }
//}
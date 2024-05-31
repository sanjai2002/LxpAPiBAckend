



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels.QuizViewModel
{
    public class QuizViewModel
    {
        public Guid QuizId { get; set; }
        public Guid CourseId { get; set; }
        public Guid TopicId { get; set; }
        public string NameOfQuiz { get; set; } = null!;
        public int Duration { get; set; }
        public int? AttemptsAllowed { get; set; }
        public int PassMark { get; set; }
        public string CreatedBy { get; set; } = "System";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
     

    }
}


// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;


// namespace LXP.Common.DTO
// {
//     public class QuizDto
//     {
//         public Guid QuizId { get; set; }
//         //public Guid TopicId { get; set; } = Guid.Parse("e3a895e4-1b3f-45b8-9c0a-98f9c0fa4996");
//         //public Guid CourseId { get; set; } = Guid.Parse("ce753ccb-408c-4d8c-8acd-cbc8c5adcbb8");
//         public Guid CourseId { get; set; }
//         public Guid TopicId { get; set; }

//         public string NameOfQuiz { get; set; } = null!;
//         public int Duration { get; set; }
//         public int PassMark { get; set; }
//         public string CreatedBy { get; set; } = null!;
//         public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
//         public string? ModifiedBy { get; set; }
//         public DateTime? ModifiedAt { get; set; }
//     }
// }
// public class QuizDto
// {
//     public Guid QuizId { get; set; }
//     public int Duration { get; set; }
//     public int PassMark { get; set; }
// }
//  public string ModifiedBy { get; set; } = null!;
//  public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
using LXP.Common.Entities;
using LXP.Common.ViewModels.QuizViewModel;
using LXP.Core.IServices;
using LXP.Data.IRepository;

namespace LXP.Core.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepository;

        public QuizService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public void CreateQuiz(QuizViewModel quiz, Guid topicId)
        {
            var topic = _quizRepository.GetTopicById(topicId);
            if (topic == null)
                throw new Exception($"Topic with id {topicId} not found.");

            var courseId = topic.CourseId;

            
            var existingQuiz = _quizRepository.GetQuizByTopicId(topicId);
            if (existingQuiz != null)
                throw new Exception($"A quiz already exists for the topic with id {topicId}.");

            ValidateQuiz(quiz);

            var quizEntity = new Quiz
            {
                QuizId = quiz.QuizId,
                CourseId = courseId,
                TopicId = topicId,
                NameOfQuiz = quiz.NameOfQuiz,
                Duration = quiz.Duration,
                PassMark = quiz.PassMark,
                AttemptsAllowed = quiz.AttemptsAllowed,
                CreatedBy = quiz.CreatedBy,
                CreatedAt = quiz.CreatedAt
            };

            _quizRepository.AddQuiz(quizEntity);
        }

        public void UpdateQuiz(QuizViewModel quiz)
        {
            ValidateQuiz(quiz);

            var quizEntity = _quizRepository.GetQuizById(quiz.QuizId);
            if (quizEntity != null)
            {
                quizEntity.NameOfQuiz = quiz.NameOfQuiz;
                quizEntity.Duration = quiz.Duration;
                quizEntity.PassMark = quiz.PassMark;
                quizEntity.AttemptsAllowed = quiz.AttemptsAllowed;

                _quizRepository.UpdateQuiz(quizEntity);
            }
        }

        public void DeleteQuiz(Guid quizId)
        {
            var quizEntity = _quizRepository.GetQuizById(quizId);
            if (quizEntity != null)
            {
                _quizRepository.DeleteQuiz(quizEntity);
            }
        }

        public IEnumerable<QuizViewModel> GetAllQuizzes()
        {
            return _quizRepository.GetAllQuizzes()
                .Select(q => new QuizViewModel
                {
                    QuizId = q.QuizId,
                    NameOfQuiz = q.NameOfQuiz,
                    Duration = q.Duration,
                    PassMark = q.PassMark,
                    AttemptsAllowed = q.AttemptsAllowed
                })
                .ToList();
        }

        public QuizViewModel GetQuizById(Guid quizId)
        {
            var quiz = _quizRepository.GetQuizById(quizId);
            if (quiz == null)
                return null;

            return new QuizViewModel
            {
                QuizId = quiz.QuizId,
                NameOfQuiz = quiz.NameOfQuiz,
                Duration = quiz.Duration,
                PassMark = quiz.PassMark,
                AttemptsAllowed = quiz.AttemptsAllowed
            };
        }

        public Guid? GetQuizIdByTopicId(Guid topicId)
        {
            var quiz = _quizRepository.GetQuizByTopicId(topicId);
            return quiz?.QuizId;
        }

        private void ValidateQuiz(QuizViewModel quiz)
        {
            if (string.IsNullOrWhiteSpace(quiz.NameOfQuiz))
                throw new Exception("NameOfQuiz cannot be null or empty.");
            if (quiz.Duration <= 0)
                throw new Exception("Duration must be a positive integer.");
            if (quiz.PassMark <= 0)
                throw new Exception("PassMark must be a positive integer.");
            if (quiz.AttemptsAllowed.HasValue && quiz.AttemptsAllowed <= 0)
                throw new Exception("AttemptsAllowed must be null or a positive integer.");
        }
    }
}

// using LXP.Common.ViewModels.QuizViewModel;
// using LXP.Core.IServices;
// using LXP.Data.IRepository;
// using System;
// using System.Collections.Generic;

// namespace LXP.Core.Services
// {
//     public class QuizService : IQuizService
//     {
//         private readonly IQuizRepository _quizRepository;

//         public QuizService(IQuizRepository quizRepository)
//         {
//             _quizRepository = quizRepository;
//         }

//         public void CreateQuiz(QuizViewModel quiz, Guid TopicId)
//         {
//             _quizRepository.CreateQuiz(quiz, TopicId);
//         }

//         public void UpdateQuiz(QuizViewModel quiz)
//         {
//             _quizRepository.UpdateQuiz(quiz);
//         }

//         public void DeleteQuiz(Guid quizId)
//         {
//             _quizRepository.DeleteQuiz(quizId);
//         }

//         public IEnumerable<QuizViewModel> GetAllQuizzes()
//         {
//             return _quizRepository.GetAllQuizzes();
//         }

//         public QuizViewModel GetQuizById(Guid quizId)
//         {
//             return _quizRepository.GetQuizById(quizId);
//         }

//         public Guid? GetQuizIdByTopicId(Guid topicId)
//         {
//             return _quizRepository.GetQuizIdByTopicId(topicId);
//         }
//     }
// }
// //using LXP.Common.DTO;
//using LXP.Core.IServices;
//using LXP.Data.IRepository;
//using System;
//using System.Collections.Generic;

//namespace LXP.Core.Services
//{
//    public class QuizService : IQuizService
//    {
//        private readonly IQuizRepository _quizRepository;

//        public QuizService(IQuizRepository quizRepository)
//        {
//            _quizRepository = quizRepository;
//        }

//        public void CreateQuiz(QuizDto quiz, Guid TopicId)
//        {
//            _quizRepository.CreateQuiz(quiz, TopicId);
//        }

//        public void UpdateQuiz(QuizDto quiz)
//        {
//            _quizRepository.UpdateQuiz(quiz);
//        }

//        public void DeleteQuiz(Guid quizId)
//        {
//            _quizRepository.DeleteQuiz(quizId);
//        }

//        public IEnumerable<QuizDto> GetAllQuizzes()
//        {
//            return _quizRepository.GetAllQuizzes();
//        }

//        public QuizDto GetQuizById(Guid quizId)
//        {
//            return _quizRepository.GetQuizById(quizId);
//        }
//    }
//}


////using LXP.Common.DTO;
////using LXP.Core.IServices;
////using LXP.Data.IRepository;
////using System;
////using System.Collections.Generic;
////using System.Threading.Tasks;

////namespace LXP.Core.Services
////{
////    public class QuizService : IQuizService
////    {
////        private readonly IQuizRepository _quizRepository;

////        public QuizService(IQuizRepository quizRepository)
////        {
////            _quizRepository = quizRepository;
////        }

////        public async Task<QuizDto> GetQuizByIdAsync(Guid quizId)
////        {
////            return await _quizRepository.GetQuizByIdAsync(quizId);
////        }

////        public async Task<IEnumerable<QuizDto>> GetAllQuizzesAsync()
////        {
////            return await _quizRepository.GetAllQuizzesAsync();
////        }

////        public async Task UpdateQuizAsync(QuizDto quiz)
////        {
////            await _quizRepository.UpdateQuizAsync(quiz);
////        }

////        public async Task DeleteQuizAsync(Guid quizId)
////        {
////            await _quizRepository.DeleteQuizAsync(quizId);
////        }

////        public async Task CreateQuizAsync(QuizDto quiz, Guid TopicId)
////        {
////            await _quizRepository.CreateQuizAsync(quiz, TopicId);
////        }
////    }
////}


//////using LXP.Common.DTO;
//////using LXP.Core.IServices;
//////using LXP.Data.IRepository;
//////using System;
//////using System.Collections.Generic;
//////using System.Linq;
//////using System.Text;
//////using System.Threading.Tasks;

//////namespace LXP.Core.Services
//////{
//////    public class QuizService : IQuizService
//////    {
//////        private readonly IQuizRepository _quizRepository;

//////        public QuizService(IQuizRepository quizRepository)
//////        {
//////            _quizRepository = quizRepository;
//////        }



//////        public void UpdateQuiz(QuizDto quiz)
//////        {
//////            _quizRepository.UpdateQuiz(quiz);
//////        }


//////        public void DeleteQuiz(Guid quizId)
//////        {
//////            _quizRepository.DeleteQuiz(quizId);
//////        }

//////        public IEnumerable<QuizDto> GetAllQuizzes()
//////        {
//////            return _quizRepository.GetAllQuizzes();
//////        }

//////        public QuizDto GetQuizById(Guid quizId)
//////        {
//////            return _quizRepository.GetQuizById(quizId);
//////        }
//////        public void CreateQuiz(QuizDto quiz, Guid TopicId)
//////        {
//////            _quizRepository.CreateQuiz(quiz, TopicId);
//////        }


//////    }
//////}

////////public void CreateQuiz(QuizDto quiz)
////////{
////////    _quizRepository.CreateQuiz(quiz);
////////}

///////*
////// * 
////// * using LXP.Common.DTO;
//////using LXP.Core.IServices;
//////using LXP.Data.IRepository;
//////using System;
//////using System.Collections.Generic;

//////namespace LXP.Core.Services
//////{
//////    public class QuizService : IQuizService
//////    {
//////        private readonly IQuizRepository _quizRepository;

//////        public QuizService(IQuizRepository quizRepository)
//////        {
//////            _quizRepository = quizRepository;
//////        }

//////        public void CreateQuiz(QuizDto quiz)
//////        {
//////            _quizRepository.CreateQuiz(quiz);
//////        }

//////        public void UpdateQuiz(QuizDto quiz)
//////        {
//////            _quizRepository.UpdateQuiz(quiz);
//////        }

//////        public void DeleteQuiz(Guid quizId)
//////        {
//////            _quizRepository.DeleteQuiz(quizId);
//////        }

//////        public IEnumerable<QuizDto> GetAllQuizzes()
//////        {
//////            return _quizRepository.GetAllQuizzes();
//////        }

//////        public QuizDto GetQuizById(Guid quizId)
//////        {
//////            return _quizRepository.GetQuizById(quizId);
//////        }
//////    }
//////}
//////*/



////////public void CreateQuiz(Guid quizId, Guid courseId, Guid topicId, string nameOfQuiz, int duration, int passMark, string createdBy, DateTime createdAt)
////////{
////////    _quizRepository.CreateQuiz(quizId, courseId, topicId, nameOfQuiz, duration, passMark, createdBy, createdAt);
////////}



////////public void CreateQuiz(Guid quizId, Guid courseId, Guid topicId, string nameOfQuiz, int duration, int passMark, string createdBy, DateTime createdAt)
////////{
////////    _quizRepository.CreateQuiz(quizId, courseId, topicId, nameOfQuiz, duration, passMark, createdBy, createdAt);
////////}

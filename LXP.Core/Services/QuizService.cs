
using LXP.Common.ViewModels.QuizViewModel;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using System;
using System.Collections.Generic;

namespace LXP.Core.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepository;

        public QuizService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public void CreateQuiz(QuizViewModel quiz, Guid TopicId)
        {
            _quizRepository.CreateQuiz(quiz, TopicId);
        }

        public void UpdateQuiz(QuizViewModel quiz)
        {
            _quizRepository.UpdateQuiz(quiz);
        }

        public void DeleteQuiz(Guid quizId)
        {
            _quizRepository.DeleteQuiz(quizId);
        }

        public IEnumerable<QuizViewModel> GetAllQuizzes()
        {
            return _quizRepository.GetAllQuizzes();
        }

        public QuizViewModel GetQuizById(Guid quizId)
        {
            return _quizRepository.GetQuizById(quizId);
        }

        public Guid? GetQuizIdByTopicId(Guid topicId)
        {
            return _quizRepository.GetQuizIdByTopicId(topicId);
        }
    }
}
//using LXP.Common.DTO;
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

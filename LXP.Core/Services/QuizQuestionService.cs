using LXP.Common.ViewModels.QuizQuestionViewModel;
using LXP.Core.IServices;
using LXP.Data.IRepository;

public class QuizQuestionService : IQuizQuestionService
{
    private readonly IQuizQuestionRepository _quizQuestionRepository;

    public QuizQuestionService(IQuizQuestionRepository quizQuestionRepository)
    {
        _quizQuestionRepository = quizQuestionRepository;
    }

    public async Task<Guid> AddQuestionAsync(QuizQuestionViewModel quizQuestionDto, List<QuestionOptionViewModel> options)
    {
        return await _quizQuestionRepository.AddQuestionAsync(quizQuestionDto, options);
    }

    public async Task<bool> UpdateQuestionAsync(Guid quizQuestionId, QuizQuestionViewModel quizQuestionDto, List<QuestionOptionViewModel> options)
    {
        return await _quizQuestionRepository.UpdateQuestionAsync(quizQuestionId, quizQuestionDto, options);
    }

    public async Task<bool> DeleteQuestionAsync(Guid quizQuestionId)
    {
        return await _quizQuestionRepository.DeleteQuestionAsync(quizQuestionId);
    }

    public async Task<List<QuizQuestionNoViewModel>> GetAllQuestionsAsync()
    {
        return await _quizQuestionRepository.GetAllQuestionsAsync();
    }

    public async Task<List<QuizQuestionNoViewModel>> GetAllQuestionsByQuizIdAsync(Guid quizId)
    {
        return await _quizQuestionRepository.GetAllQuestionsByQuizIdAsync(quizId);
    }

    public async Task<QuizQuestionNoViewModel> GetQuestionByIdAsync(Guid quizQuestionId)
    {
        return await _quizQuestionRepository.GetQuestionByIdAsync(quizQuestionId);
    }
}


//without async

//using LXP.Common.ViewModels.QuizQuestionViewModel;
//using LXP.Core.IServices;
//using LXP.Data.IRepository;
//using System;
//using System.Collections.Generic;

//namespace LXP.Core.Services
//{
//    public class QuizQuestionService : IQuizQuestionService
//    {
//        private readonly IQuizQuestionRepository _quizQuestionRepository;

//        public QuizQuestionService(IQuizQuestionRepository quizQuestionRepository)
//        {
//            _quizQuestionRepository = quizQuestionRepository;
//        }

//        public Guid AddQuestion(QuizQuestionViewModel quizQuestionDto, List<QuestionOptionViewModel> options)
//        {
//            return _quizQuestionRepository.AddQuestion(quizQuestionDto, options);
//        }

//        public bool UpdateQuestion(Guid quizQuestionId, QuizQuestionViewModel quizQuestionDto, List<QuestionOptionViewModel> options)
//        {
//            return _quizQuestionRepository.UpdateQuestion(quizQuestionId, quizQuestionDto, options);
//        }

//        public bool DeleteQuestion(Guid quizQuestionId)
//        {
//            return _quizQuestionRepository.DeleteQuestion(quizQuestionId);
//        }

//        public List<QuizQuestionNoViewModel> GetAllQuestions()
//        {
//            return _quizQuestionRepository.GetAllQuestions();
//        }

//        public List<QuizQuestionNoViewModel> GetAllQuestionsByQuizId(Guid quizId)
//        {
//            return _quizQuestionRepository.GetAllQuestionsByQuizId(quizId);
//        }



//        public QuizQuestionNoViewModel GetQuestionById(Guid quizQuestionId)
//        {
//            return _quizQuestionRepository.GetQuestionById(quizQuestionId);
//        }
//    }
//}


//using LXP.Common.DTO;
//using LXP.Core.IServices;
//using LXP.Data.IRepository;
//using System;
//using System.Collections.Generic;

//namespace LXP.Core.Services
//{
//    public class QuizQuestionService : IQuizQuestionService
//    {
//        private readonly IQuizQuestionRepository _quizQuestionRepository;

//        public QuizQuestionService(IQuizQuestionRepository quizQuestionRepository)
//        {
//            _quizQuestionRepository = quizQuestionRepository;
//        }

//        public Guid AddQuestion(QuizQuestionDto quizQuestionDto, List<QuestionOptionDto> options)
//        {
//            return _quizQuestionRepository.AddQuestion(quizQuestionDto, options);
//        }

//        public bool UpdateQuestion(Guid quizQuestionId, QuizQuestionDto quizQuestionDto, List<QuestionOptionDto> options)
//        {
//            return _quizQuestionRepository.UpdateQuestion(quizQuestionId, quizQuestionDto, options);
//        }


//        public bool DeleteQuestion(Guid quizQuestionId)
//        {
//            return _quizQuestionRepository.DeleteQuestion(quizQuestionId);
//        }

//        public List<QuizQuestionNoDto> GetAllQuestions()
//        {
//            return _quizQuestionRepository.GetAllQuestions();
//        }
//    }
//}
//////////using System;
//////////using System.Collections.Generic;
//////////using System.Linq;
//////////using System.Text;
//////////using System.Threading.Tasks;

//////////namespace LXP.Core.Services
//////////{
//////////    internal class Class1
//////////    {
//////////    }
//////////}
//////////using System;
//////////using System.Collections.Generic;
//////////using System.Linq;
//////////using System.Threading.Tasks;
//////////using LXP.Core.IServices;
//////////using LXP.Data.IRepository;

//////////namespace LXP.Core.Services
//////////{
//////////    public class QuizQuestionService : IQuizQuestionService
//////////    {
//////////        private readonly IQuizQuestionRepository _quizQuestionRepository;

//////////        public QuizQuestionService(IQuizQuestionRepository quizQuestionRepository)
//////////        {
//////////            _quizQuestionRepository = quizQuestionRepository;
//////////        }

//////////        /// <summary>
//////////        /// Adds a new quiz question to the database.
//////////        /// </summary>
//////////        /// <param name="question">The quiz question details.</param>
//////////        /// <returns>The newly created quiz question information.</returns>
//////////        public async Task<QuizQuestionDto> AddQuestionAsync(QuizQuestionDto question)
//////////        {
//////////            ValidateQuestion(question); // Add validation logic for question properties

//////////            question.QuizQuestionId = Guid.NewGuid();
//////////            question.CreatedAt = DateTime.UtcNow;
//////////            question.CreatedBy = "System"; // Replace with appropriate user identity

//////////            var options = question.Options?.ToList(); // Handle null options
//////////            if (options != null)
//////////            {
//////////                foreach (var option in options)
//////////                {
//////////                    option.QuestionOptionId = Guid.NewGuid();
//////////                    option.QuizQuestionId = question.QuizQuestionId;
//////////                    option.CreatedAt = DateTime.UtcNow;
//////////                    option.CreatedBy = "System"; // Replace with appropriate user identity
//////////                }
//////////            }

//////////            await _quizQuestionRepository.AddQuestionAsync(question, options);
//////////            return question;
//////////        }

//////////        /// <summary>
//////////        /// Deletes a quiz question by its ID.
//////////        /// </summary>
//////////        /// <param name="id">The ID of the quiz question to delete.</param>
//////////        /// <returns></returns>
//////////        public async Task DeleteQuestionAsync(Guid id)
//////////        {
//////////            await _quizQuestionRepository.DeleteQuestionAsync(id);
//////////        }

//////////        /// <summary>
//////////        /// Updates an existing quiz question in the database.
//////////        /// </summary>
//////////        /// <param name="question">The updated quiz question details.</param>
//////////        /// <returns></returns>
//////////        public async Task UpdateQuestionAsync(QuizQuestionDto question)
//////////        {
//////////            ValidateQuestion(question); // Add validation logic for question properties

//////////            question.ModifiedAt = DateTime.UtcNow; // Update modified date

//////////            var options = question.Options?.ToList(); // Handle null options
//////////            if (options != null)
//////////            {
//////////                foreach (var option in options)
//////////                {
//////////                    option.ModifiedAt = DateTime.UtcNow; // Update modified date for options
//////////                }
//////////            }

//////////            await _quizQuestionRepository.UpdateQuestionAsync(question, options);
//////////        }

//////////        /// <summary>
//////////        /// Gets all quiz questions from the database.
//////////        /// </summary>
//////////        /// <returns>A list of all quiz questions.</returns>
//////////        public async Task<IEnumerable<QuizQuestionDto>> GetQuestionsAsync()
//////////        {
//////////            return await _quizQuestionRepository.GetQuestionsAsync();
//////////        }

//////////        private void ValidateQuestion(QuizQuestionDto question)
//////////        {
//////////            // Implement validation logic here
//////////            // - Check for required fields (Question, QuestionType, etc.)
//////////            // - Validate option count and correctness for different question types (MSQ, MCQ, T/F)
//////////        }
//////////    }
//////////}
////////using LXP.Common.DTO;
////////using LXP.Data.IRepository;
////////using System;
////////using System.Collections.Generic;
////////using LXP.Core.IServices;
////////using LXP.Data.IRepository;

////////namespace LXP.Core.Services
////////{
////////    public class QuizQuestionService : IQuizQuestionService
////////    {
////////        private readonly IQuizQuestionRepository _quizQuestionRepository;

////////        public QuizQuestionService(IQuizQuestionRepository quizQuestionRepository)
////////        {
////////            _quizQuestionRepository = quizQuestionRepository;
////////        }

////////        public Guid AddQuestion(QuizQuestionDto quizQuestionDto)
////////        {
////////            // Add implementation to add a new question
////////            return _quizQuestionRepository.AddQuestion(quizQuestionDto);
////////        }

////////        public bool UpdateQuestion(QuizQuestionDto quizQuestionDto)
////////        {
////////            // Add implementation to update an existing question
////////            return _quizQuestionRepository.UpdateQuestion(quizQuestionDto);
////////        }

////////        public bool DeleteQuestion(Guid quizQuestionId)
////////        {
////////            // Add implementation to delete a question
////////            return _quizQuestionRepository.DeleteQuestion(quizQuestionId);
////////        }

////////        public List<QuizQuestionDto> GetAllQuestions()
////////        {
////////            // Add implementation to get all questions
////////            return _quizQuestionRepository.GetAllQuestions();
////////        }

////////        public Guid AddOption(QuestionOptionDto questionOptionDto)
////////        {
////////            // Add implementation to add a new option
////////            return _quizQuestionRepository.AddOption(questionOptionDto);
////////        }
////////    }
////////}
//////using LXP.Common.DTO;
//////using LXP.Core.IServices;
//////using LXP.Data.IRepository;
//////using System;
//////using System.Collections.Generic;

//////namespace LXP.Core.Services
//////{
//////    public class QuizQuestionService : IQuizQuestionService
//////    {
//////        private readonly IQuizQuestionRepository _quizQuestionRepository;

//////        public QuizQuestionService(IQuizQuestionRepository quizQuestionRepository)
//////        {
//////            _quizQuestionRepository = quizQuestionRepository;
//////        }

//////        public Guid AddQuestion(QuizQuestionDto quizQuestionDto)
//////        {
//////            // Generate a new QuestionNo
//////            quizQuestionDto.QuestionNo = _quizQuestionRepository.GetNextQuestionNo();

//////            return _quizQuestionRepository.AddQuestion(quizQuestionDto);
//////        }

//////        public bool UpdateQuestion(QuizQuestionDto quizQuestionDto)
//////        {
//////            return _quizQuestionRepository.UpdateQuestion(quizQuestionDto);
//////        }

//////        public bool DeleteQuestion(Guid quizQuestionId)
//////        {
//////            var result = _quizQuestionRepository.DeleteQuestion(quizQuestionId);
//////            if (result)
//////            {

//////                _quizQuestionRepository.DecrementQuestionNos(quizQuestionId);
//////            }
//////            return result;
//////        }

//////        public List<QuizQuestionDto> GetAllQuestions()
//////        {
//////            return _quizQuestionRepository.GetAllQuestions();
//////        }

//////        public Guid AddOption(QuestionOptionDto questionOptionDto)
//////        {
//////            return _quizQuestionRepository.AddOption(questionOptionDto);
//////        }
//////    }
//////}
////using LXP.Common.DTO;
////using LXP.Core.IServices;
////using LXP.Data.IRepository;
////using System;
////using System.Collections.Generic;

////namespace LXP.Core.Services
////{
////    public class QuizQuestionService : IQuizQuestionService
////    {
////        private readonly IQuizQuestionRepository _quizQuestionRepository;

////        public QuizQuestionService(IQuizQuestionRepository quizQuestionRepository)
////        {
////            _quizQuestionRepository = quizQuestionRepository;
////        }

////        public Guid AddQuestion(QuizQuestionDto quizQuestionDto)
////        {
////            // Generate a new QuestionNo
////            quizQuestionDto.QuestionNo = _quizQuestionRepository.GetNextQuestionNo();

////            return _quizQuestionRepository.AddQuestion(quizQuestionDto);
////        }

////        public bool UpdateQuestion(QuizQuestionDto quizQuestionDto)
////        {
////            return _quizQuestionRepository.UpdateQuestion(quizQuestionDto);
////        }

////        public bool DeleteQuestion(Guid quizQuestionId)
////        {
////            var result = _quizQuestionRepository.DeleteQuestion(quizQuestionId);
////            if (result)
////            {
////                _quizQuestionRepository.DecrementQuestionNos(quizQuestionId);
////            }
////            return result;
////        }

////        public List<QuizQuestionDto> GetAllQuestions()
////        {
////            return _quizQuestionRepository.GetAllQuestions();
////        }

////        public Guid AddOption(QuestionOptionDto questionOptionDto)
////        {
////            var question = _quizQuestionRepository.GetAllQuestions().FirstOrDefault(q => q.QuizQuestionId == questionOptionDto.QuizQuestionId);
////            if (question != null)
////            {
////                var options = _quizQuestionRepository.GetOptionsByQuestionId(questionOptionDto.QuizQuestionId);
////                options.Add(questionOptionDto);

////                if (_quizQuestionRepository.ValidateOptionsByQuestionType(question.QuestionType, options))
////                {
////                    return _quizQuestionRepository.AddOption(questionOptionDto);
////                }
////            }

////            return Guid.Empty;
////        }
////    }
////}



//using LXP.Common.DTO;
//using LXP.Core.IServices;
//using LXP.Data.IRepository;
//using LXP.Data.Repository;
//using System;
//using System.Collections.Generic;

//namespace LXP.Core.Services
//{
//    public class QuizQuestionService : IQuizQuestionService
//    {

//        private readonly IQuizQuestionRepository _quizQuestionRepository;

//        public QuizQuestionService(IQuizQuestionRepository quizQuestionRepository)
//        {
//            _quizQuestionRepository = quizQuestionRepository;
//        }

//        public Guid AddQuestion(QuizQuestionDto quizQuestionDto)
//        {
//            // Generate a new QuestionNo based on the QuizId
//            quizQuestionDto.QuestionNo = _quizQuestionRepository.GetNextQuestionNo(quizQuestionDto.QuizId);

//            return _quizQuestionRepository.AddQuestion(quizQuestionDto);
//        }

//        public bool UpdateQuestion(QuizQuestionDto quizQuestionDto)
//        {
//            return _quizQuestionRepository.UpdateQuestion(quizQuestionDto);
//        }

//        public bool DeleteQuestion(Guid quizQuestionId)
//        {
//            var result = _quizQuestionRepository.DeleteQuestion(quizQuestionId);
//            if (result)
//            {
//                _quizQuestionRepository.DecrementQuestionNos(quizQuestionId);
//            }
//            return result;
//        }

//        public List<QuizQuestionDto> GetAllQuestions()
//        {
//            return _quizQuestionRepository.GetAllQuestions();
//        }

//        public Guid AddOption(QuestionOptionDto questionOptionDto)
//        {
//            var question = _quizQuestionRepository.GetAllQuestions().FirstOrDefault(q => q.QuizQuestionId == questionOptionDto.QuizQuestionId);
//            if (question != null)
//            {
//                var options = _quizQuestionRepository.GetOptionsByQuestionId(questionOptionDto.QuizQuestionId);
//                options.Add(questionOptionDto);

//                if (_quizQuestionRepository.ValidateOptionsByQuestionType(question.QuestionType, options))
//                {
//                    return _quizQuestionRepository.AddOption(questionOptionDto);
//                }
//            }

//            return Guid.Empty;
//        }
//    }
//}
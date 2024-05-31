
using LXP.Data.IRepository;
using Microsoft.EntityFrameworkCore;
using LXP.Common.ViewModels.QuizQuestionViewModel;
using LXP.Common.Entities;


namespace LXP.Data.Repository
{
    public static class QuestionTypes
    {
        public const string MultiSelectQuestion = "MSQ";
        public const string MultiChoiceQuestion = "MCQ";
        public const string TrueFalseQuestion = "T/F";
    }

    public class QuizQuestionRepository : IQuizQuestionRepository
    {
        private readonly LXPDbContext _LXPDbContext;

        public QuizQuestionRepository(LXPDbContext dbContext)
        {
            _LXPDbContext =
                dbContext
                ?? throw new ArgumentNullException(nameof(dbContext), "DB context cannot be null.");
        }
        public async Task<Guid> AddQuestionAsync(QuizQuestionViewModel quizQuestionDto, List<QuestionOptionViewModel> options)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(quizQuestionDto.Question))
                    throw new ArgumentException("Question cannot be null or empty.", nameof(quizQuestionDto.Question));

                if (string.IsNullOrWhiteSpace(quizQuestionDto.QuestionType))
                    throw new ArgumentException("QuestionType cannot be null or empty.", nameof(quizQuestionDto.QuestionType));

                quizQuestionDto.QuestionType = quizQuestionDto.QuestionType.ToUpper();

                if (!IsValidQuestionType(quizQuestionDto.QuestionType))
                    throw new ArgumentException("Invalid question type.", nameof(quizQuestionDto.QuestionType));

                if (!ValidateOptionsByQuestionType(quizQuestionDto.QuestionType, options))
                    throw new ArgumentException("Invalid options for the given question type.", nameof(options));

                if (!ValidateOptions(quizQuestionDto.QuestionType, options))
                    throw new ArgumentException("Invalid options for the given question type.", nameof(options));

                var quizQuestionEntity = new QuizQuestion
                {
                    QuizId = quizQuestionDto.QuizId,
                    Question = quizQuestionDto.Question,
                    QuestionType = quizQuestionDto.QuestionType,
                    QuestionNo = await GetNextQuestionNoAsync(quizQuestionDto.QuizId),
                    CreatedBy = "SystemUser",
                    CreatedAt = DateTime.UtcNow
                };

                await _LXPDbContext.QuizQuestions.AddAsync(quizQuestionEntity);
                await _LXPDbContext.SaveChangesAsync();

                foreach (var option in options)
                {
                    var questionOptionEntity = new QuestionOption
                    {
                        QuizQuestionId = quizQuestionEntity.QuizQuestionId,
                        Option = option.Option,
                        IsCorrect = option.IsCorrect,
                        CreatedBy = "SystemUser",
                        CreatedAt = DateTime.UtcNow
                    };

                    await _LXPDbContext.QuestionOptions.AddAsync(questionOptionEntity);
                }

                await _LXPDbContext.SaveChangesAsync();

                return quizQuestionEntity.QuizQuestionId;
            }
            catch (ArgumentException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while adding the quiz question.", ex);
            }
        }


        

        private bool IsValidQuestionType(string questionType)
        {
            return questionType == QuestionTypes.MultiSelectQuestion
                || questionType == QuestionTypes.MultiChoiceQuestion
                || questionType == QuestionTypes.TrueFalseQuestion;
        }


        
        public async Task<bool> UpdateQuestionAsync(Guid quizQuestionId, QuizQuestionViewModel quizQuestionDto, List<QuestionOptionViewModel> options)
        {
            try
            {
                var quizQuestionEntity = await _LXPDbContext.QuizQuestions.FindAsync(quizQuestionId);
                if (quizQuestionEntity == null)
                    return false;

                if (quizQuestionDto.QuestionType.ToUpper() != quizQuestionEntity.QuestionType)
                {
                    throw new InvalidOperationException("Question type cannot be updated.");
                }

                // Update the question
                quizQuestionEntity.Question = quizQuestionDto.Question;

                // Remove existing options
                var existingOptions = _LXPDbContext.QuestionOptions.Where(o => o.QuizQuestionId == quizQuestionId).ToList();
                _LXPDbContext.QuestionOptions.RemoveRange(existingOptions);

                // Add new options
                foreach (var option in options)
                {
                    var questionOptionEntity = new QuestionOption
                    {
                        QuizQuestionId = quizQuestionEntity.QuizQuestionId,
                        Option = option.Option,
                        IsCorrect = option.IsCorrect,
                        CreatedBy = quizQuestionEntity.CreatedBy,
                        CreatedAt = quizQuestionEntity.CreatedAt
                    };

                    await _LXPDbContext.QuestionOptions.AddAsync(questionOptionEntity);
                }

                // Validate options based on the existing question type
                if (!ValidateOptionsByQuestionType(quizQuestionEntity.QuestionType, options))
                    throw new ArgumentException("Invalid options for the given question type.", nameof(options));

                await _LXPDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while updating the quiz question.", ex);
            }
        }
        public async Task<bool> DeleteQuestionAsync(Guid quizQuestionId)
        {
            try
            {
                var quizQuestionEntity = await _LXPDbContext.QuizQuestions.FindAsync(quizQuestionId);
                if (quizQuestionEntity == null)
                    return false;

                _LXPDbContext.QuestionOptions.RemoveRange(_LXPDbContext.QuestionOptions.Where(o => o.QuizQuestionId == quizQuestionId));
                _LXPDbContext.QuizQuestions.Remove(quizQuestionEntity);
                await _LXPDbContext.SaveChangesAsync();

                ReorderQuestionNos(quizQuestionEntity.QuizId, quizQuestionEntity.QuestionNo);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while deleting the quiz question.", ex);
            }
        }



        
        private void ReorderQuestionNos(Guid quizId, int deletedQuestionNo)
        {
            var subsequentQuestions = _LXPDbContext.QuizQuestions
                .Where(q => q.QuizId == quizId && q.QuestionNo > deletedQuestionNo)
                .ToList();
            foreach (var question in subsequentQuestions)
            {
                question.QuestionNo--;
            }
            _LXPDbContext.SaveChanges();
        }


        public List<QuizQuestionNoViewModel> GetAllQuestions()
        {
            try
            {
                return _LXPDbContext.QuizQuestions
                    .Select(
                        q =>
                            new QuizQuestionNoViewModel
                            {
                                QuizId = q.QuizId,
                                QuizQuestionId = q.QuizQuestionId,
                                Question = q.Question,
                                QuestionType = q.QuestionType,
                                QuestionNo = q.QuestionNo,

                                Options = _LXPDbContext.QuestionOptions
                                    .Where(o => o.QuizQuestionId == q.QuizQuestionId)
                                    .Select(
                                        o =>
                                            new QuestionOptionViewModel
                                            {
                                                Option = o.Option,
                                                IsCorrect = o.IsCorrect
                                            }
                                    )
                                    .ToList()
                            }
                    )
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "An error occurred while retrieving all quiz questions.",
                    ex
                );
            }
        }
        

        public async Task<List<QuizQuestionNoViewModel>> GetAllQuestionsByQuizIdAsync(Guid quizId)
        {
            try
            {
                return await _LXPDbContext.QuizQuestions
                    .Where(q => q.QuizId == quizId)
                    .Select(q => new QuizQuestionNoViewModel
                    {
                        QuizId = q.QuizId,
                        QuizQuestionId = q.QuizQuestionId,
                        Question = q.Question,
                        QuestionType = q.QuestionType,
                        QuestionNo = q.QuestionNo,
                        Options = _LXPDbContext.QuestionOptions
                            .Where(o => o.QuizQuestionId == q.QuizQuestionId)
                            .Select(o => new QuestionOptionViewModel { Option = o.Option, IsCorrect = o.IsCorrect })
                            .ToList()
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving all quiz questions by quiz ID.", ex);
            }
        }



        public async Task<List<QuizQuestionNoViewModel>> GetAllQuestionsAsync()
        {
            try
            {
                return await _LXPDbContext.QuizQuestions
                    .Select(q => new QuizQuestionNoViewModel
                    {
                        QuizId = q.QuizId,
                        QuizQuestionId = q.QuizQuestionId,
                        Question = q.Question,
                        QuestionType = q.QuestionType,
                        QuestionNo = q.QuestionNo,
                        Options = _LXPDbContext.QuestionOptions
                            .Where(o => o.QuizQuestionId == q.QuizQuestionId)
                            .Select(o => new QuestionOptionViewModel { Option = o.Option, IsCorrect = o.IsCorrect })
                            .ToList()
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving all quiz questions.", ex);
            }
        }


        public async Task<QuizQuestionNoViewModel> GetQuestionByIdAsync(Guid quizQuestionId)
        {
            try
            {
                var quizQuestion = await _LXPDbContext.QuizQuestions
                    .Where(q => q.QuizQuestionId == quizQuestionId)
                    .Select(q => new
                    {
                        q.QuizId,
                        q.QuizQuestionId,
                        q.Question,
                        q.QuestionType,
                        q.QuestionNo,
                        Options = _LXPDbContext.QuestionOptions
                            .Where(o => o.QuizQuestionId == q.QuizQuestionId)
                            .Select(o => new QuestionOptionViewModel { Option = o.Option, IsCorrect = o.IsCorrect })
                            .ToList()
                    })
                    .FirstOrDefaultAsync();

                if (quizQuestion == null)
                {
                    return null;
                }

                return new QuizQuestionNoViewModel
                {
                    QuizId = quizQuestion.QuizId,
                    QuizQuestionId = quizQuestion.QuizQuestionId,
                    Question = quizQuestion.Question,
                    QuestionType = quizQuestion.QuestionType,
                    QuestionNo = quizQuestion.QuestionNo,
                    Options = quizQuestion.Options ?? new List<QuestionOptionViewModel>()
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving the quiz question by ID.", ex);
            }
        }


        public int GetNextQuestionNo(Guid quizId)
        {
            try
            {
                return _LXPDbContext.QuizQuestions.Where(q => q.QuizId == quizId).Count() + 1;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "An error occurred while retrieving the next question number.",
                    ex
                );
            }
        }


        public void DecrementQuestionNos(Guid deletedQuestionId)
        {
            try
            {
                var deletedQuestion = _LXPDbContext.QuizQuestions.Find(deletedQuestionId);
                if (deletedQuestion == null)
                    return;

                var subsequentQuestions = _LXPDbContext.QuizQuestions
                    .Where(
                        q =>
                            q.QuizId == deletedQuestion.QuizId
                            && q.QuestionNo > deletedQuestion.QuestionNo
                    )
                    .ToList();
                foreach (var question in subsequentQuestions)
                {
                    question.QuestionNo--;
                }
                _LXPDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "An error occurred while decrementing question numbers.",
                    ex
                );
            }
        }

        public Guid AddQuestionOption(QuestionOptionViewModel questionOptionDto, Guid quizQuestionId)
        {
            try
            {
                var questionOptionEntity = new QuestionOption
                {
                    QuizQuestionId = quizQuestionId,
                    Option = questionOptionDto.Option,
                    IsCorrect = questionOptionDto.IsCorrect,
                    CreatedBy = "SystemUser",
                    CreatedAt = DateTime.UtcNow
                };

                _LXPDbContext.QuestionOptions.Add(questionOptionEntity);
                _LXPDbContext.SaveChanges();

                return questionOptionEntity.QuestionOptionId;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "An error occurred while adding a question option.",
                    ex
                );
            }
        }

        public List<QuestionOptionViewModel> GetQuestionOptionsById(Guid quizQuestionId)
        {
            try
            {
                return _LXPDbContext.QuestionOptions
                    .Where(o => o.QuizQuestionId == quizQuestionId)
                    .Select(
                        o => new QuestionOptionViewModel { Option = o.Option, IsCorrect = o.IsCorrect }
                    )
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "An error occurred while retrieving question options by ID.",
                    ex
                );
            }
        }

        public async Task<int> GetNextQuestionNoAsync(Guid quizId)
        {
            try
            {
                int count = await _LXPDbContext.QuizQuestions
                    .Where(q => q.QuizId == quizId)
                    .CountAsync();

                return count + 1;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "An error occurred while retrieving the next question number asynchronously.",
                    ex
                );
            }
        }


        public bool ValidateOptionsByQuestionType(
            string questionType,
            List<QuestionOptionViewModel> options
        )
        {
            switch (questionType)
            {
                case QuestionTypes.MultiSelectQuestion:
                    return options.Count >= 5
                        && options.Count <= 8
                        && options.Count(o => o.IsCorrect) >= 2
                        && options.Count(o => o.IsCorrect) <= 3;
                case QuestionTypes.MultiChoiceQuestion:
                    return options.Count == 4 && options.Count(o => o.IsCorrect) == 1;
                case QuestionTypes.TrueFalseQuestion:
                    return options.Count == 2 && options.Count(o => o.IsCorrect) == 1;
                default:
                    return false;
            }
        }
        public bool ValidateOptions(string questionType, List<QuestionOptionViewModel> options)
        {
            if (questionType == QuestionTypes.TrueFalseQuestion)
            {
                return ValidateTrueFalseOptions(options);
            }
            else
            {
                return ValidateUniqueOptions(options)
                    && ValidateOptionsByQuestionType(questionType, options);
            }
        }

        private bool ValidateTrueFalseOptions(List<QuestionOptionViewModel> options)
        {
            // Check if there are exactly two options
            if (options.Count != 2)
                return false;

            // Check if one option is true and the other is false (case-insensitive)
            var trueOption = options.Any(o => o.Option.ToLower() == "true");
            var falseOption = options.Any(o => o.Option.ToLower() == "false");

            return trueOption && falseOption;
        }

        private bool ValidateUniqueOptions(List<QuestionOptionViewModel> options)
        {
            // Check if all options are unique (case-insensitive)
            var distinctOptions = options.Select(o => o.Option.ToLower()).Distinct().Count();
            return distinctOptions == options.Count;
        }
    }
}


//public List<QuizQuestionNoViewModel> GetAllQuestionsByQuizId(Guid quizId)
//{
//    try
//    {
//        return _LXPDbContext.QuizQuestions
//            .Where(q => q.QuizId == quizId)
//            .Select(
//                q =>
//                    new QuizQuestionNoViewModel
//                    {
//                        QuizId = q.QuizId,
//                        QuizQuestionId = q.QuizQuestionId,
//                        Question = q.Question,
//                        QuestionType = q.QuestionType,
//                        QuestionNo = q.QuestionNo,

//                        Options = _LXPDbContext.QuestionOptions // Assuming the DbSet name is QuestionOptions
//                            .Where(o => o.QuizQuestionId == q.QuizQuestionId)
//                            .Select(
//                                o =>
//                                    new QuestionOptionViewModel
//                                    {
//                                        Option = o.Option,
//                                        IsCorrect = o.IsCorrect
//                                    }
//                            )
//                            .ToList()
//                    }
//            )
//            .ToList();
//    }
//    catch (Exception ex)
//    {
//        throw new InvalidOperationException(
//            "An error occurred while retrieving all quiz questions by quiz ID.",
//            ex
//        );
//    }
//}


//public QuizQuestionNoViewModel GetQuestionById(Guid quizQuestionId)
//{
//    try
//    {
//        var quizQuestion = _LXPDbContext.QuizQuestions
//            .Where(q => q.QuizQuestionId == quizQuestionId)
//            .Select(q => new
//            {
//                q.QuizId,
//                q.QuizQuestionId,
//                q.Question,
//                q.QuestionType,
//                q.QuestionNo,
//                Options = _LXPDbContext.QuestionOptions
//                    .Where(o => o.QuizQuestionId == q.QuizQuestionId)
//                    .Select(o => new QuestionOptionViewModel
//                    {
//                        Option = o.Option,
//                        IsCorrect = o.IsCorrect
//                    })
//                    .ToList()
//            })
//            .FirstOrDefault();

//        if (quizQuestion == null)
//        {
//            return null;
//        }

//        return new QuizQuestionNoViewModel
//        {
//            QuizId = quizQuestion.QuizId,
//            QuizQuestionId = quizQuestion.QuizQuestionId,
//            Question = quizQuestion.Question,
//            QuestionType = quizQuestion.QuestionType,
//            QuestionNo = quizQuestion.QuestionNo,
//            Options = quizQuestion.Options ?? new List<QuestionOptionViewModel>()
//        };
//    }
//    catch (Exception ex)
//    {
//        throw new InvalidOperationException(
//            "An error occurred while retrieving the quiz question by ID.",
//            ex
//        );
//    }
//}


//public bool DeleteQuestion(Guid quizQuestionId)
//{
//    try
//    {
//        var quizQuestionEntity = _LXPDbContext.QuizQuestions.Find(quizQuestionId);
//        if (quizQuestionEntity == null)
//            return false;

//        _LXPDbContext.QuestionOptions.RemoveRange(
//            _LXPDbContext.QuestionOptions.Where(o => o.QuizQuestionId == quizQuestionId)
//        );
//        _LXPDbContext.QuizQuestions.Remove(quizQuestionEntity);
//        _LXPDbContext.SaveChanges();

//        ReorderQuestionNos(quizQuestionEntity.QuizId, quizQuestionEntity.QuestionNo);
//        return true;
//    }
//    catch (Exception ex)
//    {
//        throw new InvalidOperationException(
//            "An error occurred while deleting the quiz question.",
//            ex
//        );
//    }
//}



//public bool UpdateQuestion(Guid quizQuestionId, QuizQuestionViewModel quizQuestionDto, List<QuestionOptionViewModel> options)
//{
//    try
//    {
//        var quizQuestionEntity = _LXPDbContext.QuizQuestions.Find(quizQuestionId);
//        if (quizQuestionEntity == null)
//            return false;

//        if (quizQuestionDto.QuestionType.ToUpper() != quizQuestionEntity.QuestionType)
//        {
//            throw new InvalidOperationException("Question type cannot be updated.");
//        }


//        // Update the question and question type
//        quizQuestionEntity.Question = quizQuestionDto.Question;

//        _LXPDbContext.SaveChanges();

//        // Remove existing options
//        var existingOptions = _LXPDbContext.QuestionOptions
//            .Where(o => o.QuizQuestionId == quizQuestionId)
//            .ToList();
//        _LXPDbContext.QuestionOptions.RemoveRange(existingOptions);

//        // Add new options
//        foreach (var option in options)
//        {
//            var questionOptionEntity = new QuestionOption
//            {
//                QuizQuestionId = quizQuestionEntity.QuizQuestionId,
//                Option = option.Option,
//                IsCorrect = option.IsCorrect,
//                CreatedBy = quizQuestionEntity.CreatedBy,
//                CreatedAt = quizQuestionEntity.CreatedAt
//            };

//            _LXPDbContext.QuestionOptions.Add(questionOptionEntity);
//        }

//        // Validate options based on the existing question type
//        if (!ValidateOptionsByQuestionType(quizQuestionEntity.QuestionType, options))
//            throw new ArgumentException(
//                "Invalid options for the given question type.",
//                nameof(options)
//            );

//        _LXPDbContext.SaveChanges();

//        return true;
//    }
//    catch (Exception ex)
//    {
//        throw new InvalidOperationException(
//            "An error occurred while updating the quiz question.",
//            ex
//        );
//    }
//}



//public Guid AddQuestion(QuizQuestionViewModel quizQuestionDto, List<QuestionOptionViewModel> options)
//{
//    try
//    {
//        if (string.IsNullOrWhiteSpace(quizQuestionDto.Question))
//            throw new ArgumentException(
//                "Question cannot be null or empty.",
//                nameof(quizQuestionDto.Question)
//            );

//        if (string.IsNullOrWhiteSpace(quizQuestionDto.QuestionType))
//            throw new ArgumentException(
//                "QuestionType cannot be null or empty.",
//                nameof(quizQuestionDto.QuestionType)
//            );

//        quizQuestionDto.QuestionType = quizQuestionDto.QuestionType.ToUpper();

//        if (!IsValidQuestionType(quizQuestionDto.QuestionType))
//            throw new ArgumentException(
//                "Invalid question type.",
//                nameof(quizQuestionDto.QuestionType)
//            );

//        if (!ValidateOptionsByQuestionType(quizQuestionDto.QuestionType, options))
//            throw new ArgumentException(
//                "Invalid options for the given question type.",
//                nameof(options)
//            );
//        if (!ValidateOptions(quizQuestionDto.QuestionType, options))
//            throw new ArgumentException(
//                "Invalid options for the given question type.",
//                nameof(options)
//            );

//        var quizQuestionEntity = new QuizQuestion
//        {
//            QuizId = quizQuestionDto.QuizId,
//            //QuizId = Guid.Parse("4db699e3-6867-47f9-9bf6-841c221038a3"),
//            Question = quizQuestionDto.Question,
//            QuestionType = quizQuestionDto.QuestionType,
//            QuestionNo = GetNextQuestionNo(quizQuestionDto.QuizId),
//            CreatedBy = "SystemUser",
//            CreatedAt = DateTime.UtcNow
//        };

//        _LXPDbContext.QuizQuestions.Add(quizQuestionEntity);
//        _LXPDbContext.SaveChanges();

//        foreach (var option in options)
//        {
//            var questionOptionEntity = new QuestionOption
//            {
//                QuizQuestionId = quizQuestionEntity.QuizQuestionId,
//                Option = option.Option,
//                IsCorrect = option.IsCorrect,
//                CreatedBy = "SystemUser",
//                CreatedAt = DateTime.UtcNow
//            };

//            _LXPDbContext.QuestionOptions.Add(questionOptionEntity);
//        }

//        _LXPDbContext.SaveChanges();

//        return quizQuestionEntity.QuizQuestionId;
//    }
//    catch (ArgumentException ex)
//    {
//        throw;
//    }
//    catch (Exception ex)
//    {
//        throw new InvalidOperationException(
//            "An error occurred while adding the quiz question.",
//            ex
//        );
//    }
//}

//public QuizQuestionNoDto GetQuestionById(Guid quizQuestionId)
//{
//    try
//    {
//        return _LXPDbContext.QuizQuestions
//            .Where(q => q.QuizQuestionId == quizQuestionId)
//            .Select(q =>
//                new QuizQuestionNoDto
//                {
//                    QuizId = q.QuizId,
//                    QuizQuestionId = q.QuizQuestionId,
//                    Question = q.Question,
//                    QuestionType = q.QuestionType,
//                    QuestionNo = q.QuestionNo,

//                    Options = _LXPDbContext.QuizFeedbackQuestionOptions
//                        .Where(o => o.QuizQuestionId == q.QuizQuestionId)
//                        .Select(o =>
//                            new QuestionOptionDto
//                            {
//                                Option = o.Option,
//                                IsCorrect = o.IsCorrect
//                            }
//                        )
//                        .ToList()
//                }
//            )
//            .FirstOrDefault();
//    }
//    catch (Exception ex)
//    {
//        throw new InvalidOperationException(
//            "An error occurred while retrieving the quiz question by ID.",
//            ex
//        );
//    }
//}
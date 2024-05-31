
using LXP.Common.Entities;
using LXP.Common.ViewModels.QuizFeedbackQuestionViewModel;

using LXP.Data.IRepository;

namespace LXP.Data.Repository
{
    public static class FeedbackQuestionTypes
    {
        public const string MultiChoiceQuestion = "MCQ";
        public const string DescriptiveQuestion = "Descriptive";
    }
    public class QuizFeedbackRepository : IQuizFeedbackRepository
    {
        private readonly LXPDbContext _dbContext;

        public QuizFeedbackRepository(LXPDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public Guid AddFeedbackQuestion(QuizfeedbackquestionViewModel quizfeedbackquestionDto, List<QuizFeedbackQuestionsOptionViewModel> options)
        {
            try
            {
                // Normalize question type to uppercase
                var normalizedQuestionType = quizfeedbackquestionDto.QuestionType.ToUpper();

                // Ensure no options are saved for descriptive questions
                if (normalizedQuestionType == FeedbackQuestionTypes.DescriptiveQuestion.ToUpper())
                {
                    options = null;
                }

                if (!ValidateOptionsByFeedbackQuestionType(quizfeedbackquestionDto.QuestionType, options))
                    throw new ArgumentException(
                        "Invalid options for the given question type.",
                        nameof(options)
                    );

                // Create and save the feedback question entity
                var questionEntity = new Quizfeedbackquestion
                {
                    QuizId = quizfeedbackquestionDto.QuizId,
                    QuestionNo = GetNextFeedbackQuestionNo(quizfeedbackquestionDto.QuizId),
                    Question = quizfeedbackquestionDto.Question,
                    QuestionType = normalizedQuestionType,
                    CreatedBy = "Admin",
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.Quizfeedbackquestions.Add(questionEntity);
                _dbContext.SaveChanges();

                // Save the options only if the question type is MCQ
                if (normalizedQuestionType == FeedbackQuestionTypes.MultiChoiceQuestion.ToUpper())
                {
                    if (options != null && options.Count > 0)
                    {
                        foreach (var option in options)
                        {
                            var optionEntity = new Feedbackquestionsoption
                            {
                                QuizFeedbackQuestionId = questionEntity.QuizFeedbackQuestionId,
                                OptionText = option.OptionText,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = "Admin"
                            };
                            _dbContext.Feedbackquestionsoptions.Add(optionEntity);
                        }
                        _dbContext.SaveChanges();
                    }
                }

                return questionEntity.QuizFeedbackQuestionId;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if necessary
                throw new InvalidOperationException("An error occurred while adding the feedback question.", ex);
            }
        }

        public List<QuizfeedbackquestionNoViewModel> GetAllFeedbackQuestions()
        {
            return _dbContext.Quizfeedbackquestions
                .Select(q => new QuizfeedbackquestionNoViewModel
                {
                    QuizFeedbackQuestionId = q.QuizFeedbackQuestionId,
                    QuizId = q.QuizId,
                    QuestionNo = q.QuestionNo,
                    Question = q.Question,
                    QuestionType = q.QuestionType,
                    Options = _dbContext.Feedbackquestionsoptions
                                    .Where(o => o.QuizFeedbackQuestionId == q.QuizFeedbackQuestionId)
                                    .Select(
                                        o =>
                                            new QuizFeedbackQuestionsOptionViewModel
                                            {
                                                OptionText = o.OptionText,
                                              
                                            }
                                    )
                                    .ToList()
                }).ToList();
        }
        public List<QuizfeedbackquestionNoViewModel> GetFeedbackQuestionsByQuizId(Guid quizId)
        {
            return _dbContext.Quizfeedbackquestions
                .Where(q => q.QuizId == quizId)
                .Select(q => new QuizfeedbackquestionNoViewModel
                {
                    QuizFeedbackQuestionId = q.QuizFeedbackQuestionId,
                    QuizId = q.QuizId,
                    QuestionNo = q.QuestionNo,
                    Question = q.Question,
                    QuestionType = q.QuestionType,
                    Options = _dbContext.Feedbackquestionsoptions
                                        .Where(o => o.QuizFeedbackQuestionId == q.QuizFeedbackQuestionId)
                                        .Select(o => new QuizFeedbackQuestionsOptionViewModel
                                        {
                                            OptionText = o.OptionText
                                        }).ToList()
                }).ToList();
        }


        public int GetNextFeedbackQuestionNo(Guid quizId)
        {
            var lastQuestion = _dbContext.Quizfeedbackquestions
                .Where(q => q.QuizId == quizId)
                .OrderByDescending(q => q.QuestionNo)
                .FirstOrDefault();
            return lastQuestion != null ? lastQuestion.QuestionNo + 1 : 1;
        }






        public Guid AddFeedbackQuestionOption(QuizFeedbackQuestionsOptionViewModel feedbackquestionsoptionDto, Guid quizFeedbackQuestionId)
        {
            var optionEntity = new Feedbackquestionsoption
            {
                QuizFeedbackQuestionId = quizFeedbackQuestionId,
                OptionText = feedbackquestionsoptionDto.OptionText,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "Admin"
            };
            _dbContext.Feedbackquestionsoptions.Add(optionEntity);
            _dbContext.SaveChanges();

            return optionEntity.FeedbackQuestionOptionId;
        }

        public List<QuizFeedbackQuestionsOptionViewModel> GetFeedbackQuestionOptionsById(Guid quizFeedbackQuestionId)
        {
            return _dbContext.Feedbackquestionsoptions
                .Where(o => o.QuizFeedbackQuestionId == quizFeedbackQuestionId)
                .Select(o => new QuizFeedbackQuestionsOptionViewModel
                {
                    //FeedbackQuestionOptionId = o.FeedbackQuestionOptionId,
                    OptionText = o.OptionText
                }).ToList();
        }

      




        public QuizfeedbackquestionNoViewModel GetFeedbackQuestionById(Guid quizFeedbackQuestionId)
        {
            var feedbackQuestion = _dbContext.Quizfeedbackquestions
                .Where(q => q.QuizFeedbackQuestionId == quizFeedbackQuestionId)
                .Select(q => new
                {
                    q.QuizFeedbackQuestionId,
                    q.QuizId,
                    q.QuestionNo,
                    q.Question,
                    q.QuestionType,
                    Options = _dbContext.Feedbackquestionsoptions
                                .Where(o => o.QuizFeedbackQuestionId == q.QuizFeedbackQuestionId)
                                .Select(o => new QuizFeedbackQuestionsOptionViewModel
                                {
                                    OptionText = o.OptionText
                                })
                                .ToList()
                })
                .FirstOrDefault();

            if (feedbackQuestion == null)
            {
                return null;
            }

            return new QuizfeedbackquestionNoViewModel
            {
                QuizFeedbackQuestionId = feedbackQuestion.QuizFeedbackQuestionId,
                QuizId = feedbackQuestion.QuizId,
                QuestionNo = feedbackQuestion.QuestionNo,
                Question = feedbackQuestion.Question,
                QuestionType = feedbackQuestion.QuestionType,
                Options = feedbackQuestion.Options ?? new List<QuizFeedbackQuestionsOptionViewModel>()
            };
        }


        public bool ValidateOptionsByFeedbackQuestionType(string questionType, List<QuizFeedbackQuestionsOptionViewModel> options)
        {
            questionType = questionType.ToUpper();

            if (questionType == FeedbackQuestionTypes.MultiChoiceQuestion.ToUpper())
            {
                return options != null && options.Count >= 2 && options.Count <= 5;
            }
            return options == null || options.Count == 0;
        }


        public bool UpdateFeedbackQuestion(Guid quizFeedbackQuestionId, QuizfeedbackquestionViewModel quizfeedbackquestionDto, List<QuizFeedbackQuestionsOptionViewModel> options)
        {
            try
            {
                var existingQuestion = _dbContext.Quizfeedbackquestions.FirstOrDefault(q => q.QuizFeedbackQuestionId == quizFeedbackQuestionId);
                if (existingQuestion != null)
                {
                    // Check if the question type is being modified
                    if (!existingQuestion.QuestionType.Equals(quizfeedbackquestionDto.QuestionType, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException("Question type cannot be modified.");
                    }

                    // Validate options based on question type
                    if (!ValidateOptionsByFeedbackQuestionType(existingQuestion.QuestionType, options))
                    {
                        throw new ArgumentException("Invalid options for the given question type.");
                    }

                    // Update the question details
                    existingQuestion.Question = quizfeedbackquestionDto.Question;
                    existingQuestion.ModifiedAt = DateTime.UtcNow;
                    existingQuestion.ModifiedBy = "Admin";
                    _dbContext.SaveChanges();

                    // Remove existing options if question type is MultiChoiceQuestion
                    if (existingQuestion.QuestionType == FeedbackQuestionTypes.MultiChoiceQuestion.ToUpper())
                    {
                        var existingOptions = _dbContext.Feedbackquestionsoptions.Where(o => o.QuizFeedbackQuestionId == quizFeedbackQuestionId).ToList();
                        _dbContext.Feedbackquestionsoptions.RemoveRange(existingOptions);
                        _dbContext.SaveChanges();

                        // Add new options
                        // Add
                        if (options != null && options.Count > 0)
                        {
                            foreach (var option in options)
                            {
                                var optionEntity = new Feedbackquestionsoption
                                {
                                    QuizFeedbackQuestionId = quizFeedbackQuestionId,
                                    OptionText = option.OptionText,
                                    CreatedAt = DateTime.UtcNow,
                                    CreatedBy = "Admin"
                                };
                                _dbContext.Feedbackquestionsoptions.Add(optionEntity);
                            }
                            _dbContext.SaveChanges();
                        }
                    }

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if necessary
                throw new InvalidOperationException("An error occurred while updating the feedback question.", ex);
            }
        }




        public bool DeleteFeedbackQuestion(Guid quizFeedbackQuestionId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var existingQuestion = _dbContext.Quizfeedbackquestions.FirstOrDefault(q => q.QuizFeedbackQuestionId == quizFeedbackQuestionId);
                if (existingQuestion != null)
                {
                    var relatedOptions = _dbContext.Feedbackquestionsoptions
                                                   .Where(o => o.QuizFeedbackQuestionId == quizFeedbackQuestionId)
                                                   .ToList();

                    if (relatedOptions.Any())
                    {
                        _dbContext.Feedbackquestionsoptions.RemoveRange(relatedOptions);
                    }

                    _dbContext.Quizfeedbackquestions.Remove(existingQuestion);
                    _dbContext.SaveChanges();

                    ReorderQuestionNos(existingQuestion.QuizId, existingQuestion.QuestionNo);

                    transaction.Commit();
                    return true;
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            return false;
        }

        public bool DeleteFeedbackQuestionsByQuizId(Guid quizId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var feedbackQuestions = _dbContext.Quizfeedbackquestions.Where(q => q.QuizId == quizId).ToList();
                if (feedbackQuestions.Count == 0)
                {
                    return false;
                }

                foreach (var question in feedbackQuestions)
                {
                    var relatedOptions = _dbContext.Feedbackquestionsoptions
                                                   .Where(o => o.QuizFeedbackQuestionId == question.QuizFeedbackQuestionId)
                                                   .ToList();

                    if (relatedOptions.Any())
                    {
                        _dbContext.Feedbackquestionsoptions.RemoveRange(relatedOptions);
                    }

                    _dbContext.Quizfeedbackquestions.Remove(question);
                }

                _dbContext.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public void ReorderQuestionNos(Guid quizId, int deletedQuestionNo)
        {
            var questionsToUpdate = _dbContext.Quizfeedbackquestions
                                              .Where(q => q.QuizId == quizId && q.QuestionNo > deletedQuestionNo)
                                              .OrderBy(q => q.QuestionNo)
                                              .ToList();

            foreach (var question in questionsToUpdate)
            {
                question.QuestionNo--;
            }
            _dbContext.SaveChanges();
        }

    }
}





//private Guid? GetOptionIdByText(Guid questionId, string optionText)
//{
//    var option = _dbContext.Feedbackquestionsoptions
//        .FirstOrDefault(o => o.QuizFeedbackQuestionId == questionId && o.OptionText.ToLower() == optionText.ToLower());

//    return option?.FeedbackQuestionOptionId;
//}




//private void ValidateFeedbackQuestion(QuizfeedbackquestionViewModel quizfeedbackquestionDto, List<QuizFeedbackQuestionsOptionViewModel> options)
//{
//    if (quizfeedbackquestionDto.QuestionType == FeedbackQuestionTypes.MultiChoiceQuestion)
//    {
//        if (options == null || options.Count == 0)
//        {
//            throw new ArgumentException("MCQ questions must have at least one option.");
//        }
//    }
//    else if (quizfeedbackquestionDto.QuestionType == FeedbackQuestionTypes.DescriptiveQuestion)
//    {
//        if (options != null && options.Count > 0)
//        {
//            throw new ArgumentException("Descriptive questions should not have options.");
//        }
//    }
//    else
//    {
//        throw new ArgumentException("Invalid question type.");
//    }
//}




















































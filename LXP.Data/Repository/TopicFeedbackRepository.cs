
using LXP.Common.Entities;
using LXP.Common.ViewModels.TopicFeedbackQuestionViemModel;
using LXP.Data.IRepository;

namespace LXP.Data.Repository
{
    public static class TopicFeedbackQuestionTypes
    {
        public const string MultiChoiceQuestion = "MCQ";
        public const string DescriptiveQuestion = "Descriptive";
    }

    public class TopicFeedbackRepository : ITopicFeedbackRepository
    {
        private readonly LXPDbContext _context;

        public TopicFeedbackRepository(LXPDbContext context)
        {
            _context = context;
        }

        public bool AddFeedbackQuestion(TopicFeedbackQuestionViewModel topicfeedbackquestionDto, List<FeedbackOptionDTO> options)
        {
            try
            {
                if (topicfeedbackquestionDto == null)
                    throw new ArgumentNullException(nameof(topicfeedbackquestionDto));

                // Normalize question type to uppercase
                var normalizedQuestionType = topicfeedbackquestionDto.QuestionType.ToUpper();

                // Ensure no options are saved for descriptive questions
                if (normalizedQuestionType == TopicFeedbackQuestionTypes.DescriptiveQuestion.ToUpper())
                {
                    options = null;
                }

                if (!ValidateOptionsByFeedbackQuestionType(normalizedQuestionType, options))
                    throw new ArgumentException("Invalid options for the given question type.", nameof(options));

                int questionNo = GetNextFeedbackQuestionNo(topicfeedbackquestionDto.TopicId);

                var feedbackQuestion = new Topicfeedbackquestion
                {
                    TopicId = topicfeedbackquestionDto.TopicId,
                    QuestionNo = questionNo,
                    Question = topicfeedbackquestionDto.Question,
                    QuestionType = normalizedQuestionType,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "Admin"
                };

                _context.Topicfeedbackquestions.Add(feedbackQuestion);
                _context.SaveChanges();

                // Save the options only if the question type is MCQ
                if (normalizedQuestionType == TopicFeedbackQuestionTypes.MultiChoiceQuestion.ToUpper() && options != null)
                {
                    foreach (var option in options)
                    {
                        var optionEntity = new Feedbackquestionsoption
                        {
                            TopicFeedbackQuestionId = feedbackQuestion.TopicFeedbackQuestionId,
                            OptionText = option.OptionText,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = "Admin"
                        };
                        _context.Feedbackquestionsoptions.Add(optionEntity);
                    }
                    _context.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while adding the feedback question.", ex);
            }
        }

        public IEnumerable<TopicFeedbackQuestionNoViewModel> GetAllFeedbackQuestions()
        {
            return _context.Topicfeedbackquestions
                .Select(q => new TopicFeedbackQuestionNoViewModel
                {
                    TopicFeedbackId = q.TopicFeedbackQuestionId,
                    TopicId = q.TopicId,
                    QuestionNo = q.QuestionNo,
                    Question = q.Question,
                    QuestionType = q.QuestionType,
                    Options = _context.Feedbackquestionsoptions
                        .Where(o => o.TopicFeedbackQuestionId == q.TopicFeedbackQuestionId)
                        .Select(o => new FeedbackOptionDTO
                        {
                            OptionText = o.OptionText
                        })
                        .ToList()
                }).ToList();
        }
        public IEnumerable<TopicFeedbackQuestionNoViewModel> GetFeedbackQuestionsByTopicId(Guid topicId)
        {
            return _context.Topicfeedbackquestions
                .Where(q => q.TopicId == topicId)
                .Select(q => new TopicFeedbackQuestionNoViewModel
                {
                    TopicFeedbackId = q.TopicFeedbackQuestionId,
                    TopicId = q.TopicId,
                    QuestionNo = q.QuestionNo,
                    Question = q.Question,
                    QuestionType = q.QuestionType,
                    Options = _context.Feedbackquestionsoptions
                        .Where(o => o.TopicFeedbackQuestionId == q.TopicFeedbackQuestionId)
                        .Select(o => new FeedbackOptionDTO
                        {
                            OptionText = o.OptionText
                        })
                        .ToList()
                })
                .ToList();
        }

        public TopicFeedbackQuestionNoViewModel GetFeedbackQuestionById(Guid id)
        {
            var question = _context.Topicfeedbackquestions
                .Where(q => q.TopicFeedbackQuestionId == id)
                .Select(q => new TopicFeedbackQuestionNoViewModel
                {
                    TopicFeedbackId = q.TopicFeedbackQuestionId,
                    TopicId = q.TopicId,
                    QuestionNo = q.QuestionNo,
                    Question = q.Question,
                    QuestionType = q.QuestionType,
                    Options = _context.Feedbackquestionsoptions
                        .Where(o => o.TopicFeedbackQuestionId == q.TopicFeedbackQuestionId)
                        .Select(o => new FeedbackOptionDTO
                        {
                            OptionText = o.OptionText
                        })
                        .ToList()
                })
                .FirstOrDefault();

            return question;
        }

        public bool UpdateFeedbackQuestion(Guid id, TopicFeedbackQuestionViewModel question, List<FeedbackOptionDTO> options)
        {
            try
            {
                var existingQuestion = _context.Topicfeedbackquestions.FirstOrDefault(q => q.TopicFeedbackQuestionId == id);
                if (existingQuestion != null)
                {
                    // Check if the question type is being modified
                    if (!existingQuestion.QuestionType.Equals(question.QuestionType, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException("Question type cannot be modified.");
                    }

                    // Validate options based on question type
                    if (!ValidateOptionsByFeedbackQuestionType(existingQuestion.QuestionType, options))
                    {
                        throw new ArgumentException("Invalid options for the given question type.");
                    }

                    // Update the question details
                    existingQuestion.Question = question.Question;
                    existingQuestion.ModifiedAt = DateTime.UtcNow;
                    existingQuestion.ModifiedBy = "Admin";
                    _context.SaveChanges();

                    // Remove existing options if question type is MultiChoiceQuestion
                    if (existingQuestion.QuestionType == TopicFeedbackQuestionTypes.MultiChoiceQuestion.ToUpper())
                    {
                        var existingOptions = _context.Feedbackquestionsoptions.Where(o => o.TopicFeedbackQuestionId == id).ToList();
                        _context.Feedbackquestionsoptions.RemoveRange(existingOptions);
                        _context.SaveChanges();

                        // Add new options
                        if (options != null && options.Count > 0)
                        {
                            foreach (var option in options)
                            {
                                var optionEntity = new Feedbackquestionsoption
                                {
                                    TopicFeedbackQuestionId = id,
                                    OptionText = option.OptionText,
                                    CreatedAt = DateTime.UtcNow,
                                    CreatedBy = "Admin"
                                };
                                _context.Feedbackquestionsoptions.Add(optionEntity);
                            }
                            _context.SaveChanges();
                        }
                    }

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while updating the feedback question.", ex);
            }
        }

        public bool DeleteFeedbackQuestion(Guid id)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var existingQuestion = _context.Topicfeedbackquestions.FirstOrDefault(q => q.TopicFeedbackQuestionId == id);
                if (existingQuestion != null)
                {
                    var relatedOptions = _context.Feedbackquestionsoptions
                        .Where(o => o.TopicFeedbackQuestionId == id)
                        .ToList();

                    if (relatedOptions.Any())
                    {
                        _context.Feedbackquestionsoptions.RemoveRange(relatedOptions);
                    }

                    _context.Topicfeedbackquestions.Remove(existingQuestion);
                    _context.SaveChanges();

                    ReorderQuestionNo(existingQuestion.TopicId, existingQuestion.QuestionNo);

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

        private int GetNextFeedbackQuestionNo(Guid topicId)
        {
            var lastQuestion = _context.Topicfeedbackquestions
                .Where(q => q.TopicId == topicId)
                .OrderByDescending(q => q.QuestionNo)
                .FirstOrDefault();
            return lastQuestion != null ? lastQuestion.QuestionNo + 1 : 1;
        }

        private void ReorderQuestionNo(Guid topicId, int deletedQuestionNo)
        {
            var subsequentQuestions = _context.Topicfeedbackquestions
                .Where(q => q.TopicId == topicId && q.QuestionNo > deletedQuestionNo)
                .ToList();
            foreach (var question in subsequentQuestions)
            {
                question.QuestionNo--;
            }
            _context.SaveChanges();
        }

        private bool ValidateOptionsByFeedbackQuestionType(string questionType, List<FeedbackOptionDTO> options)
        {
            questionType = questionType.ToUpper();

            if (questionType == TopicFeedbackQuestionTypes.MultiChoiceQuestion.ToUpper())
            {
                return options != null && options.Count >= 2 && options.Count <= 5;
            }
            return options == null || options.Count == 0;
        }
    }
}





//private Guid? GetOptionIdByText(Guid questionId, string optionText)
//{
//    var option = _context.Feedbackquestionsoptions
//        .FirstOrDefault(o => o.TopicFeedbackQuestionId == questionId && o.OptionText.ToLower() == optionText.ToLower());

//    return option?.FeedbackQuestionOptionId;
//}



//public void AddFeedbackResponse(TopicFeedbackResponseViewModel feedbackResponse)
//{
//    if (feedbackResponse == null)
//        throw new ArgumentNullException(nameof(feedbackResponse));

//    var question = _context.Topicfeedbackquestions
//        .FirstOrDefault(q => q.TopicFeedbackQuestionId == feedbackResponse.TopicFeedbackQuestionId);

//    if (question == null)
//        throw new ArgumentException("Invalid question ID.", nameof(feedbackResponse.TopicFeedbackQuestionId));

//    // Validate the response based on question type
//    if (question.QuestionType == TopicFeedbackQuestionTypes.MultiChoiceQuestion && feedbackResponse.OptionId == null)
//        throw new ArgumentException("OptionId must be provided for MCQ responses.");

//    if (question.QuestionType == TopicFeedbackQuestionTypes.DescriptiveQuestion && string.IsNullOrEmpty(feedbackResponse.Response))
//        throw new ArgumentException("Response must be provided for descriptive questions.");

//    var response = new Feedbackresponse
//    {
//        TopicFeedbackQuestionId = feedbackResponse.TopicFeedbackQuestionId,
//        LearnerId = feedbackResponse.LearnerId,
//        Response = question.QuestionType == TopicFeedbackQuestionTypes.DescriptiveQuestion ? feedbackResponse.Response : null,
//        OptionId = question.QuestionType == TopicFeedbackQuestionTypes.MultiChoiceQuestion ? feedbackResponse.OptionId : null,
//        GeneratedAt = DateTime.UtcNow,
//        GeneratedBy = "User" 
//    };

//    _context.Feedbackresponses.Add(response);
//    _context.SaveChanges();
//}

/*using LXP.Common.DTO;
using LXP.Data.DBContexts;
using LXP.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LXP.Data.Repository
{
    public class TopicFeedbackRepository : ITopicFeedbackRepository
    {
        private readonly LXPDbContext _context;

        public TopicFeedbackRepository(LXPDbContext context)
        {
            _context = context;
        }

        public bool AddFeedbackQuestion(TopicFeedbackQuestionDTO question, List<FeedbackOptionDTO> options)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            int questionCount = _context.Topicfeedbackquestions.Count(q => q.TopicId == question.TopicId);
            int questionNo = questionCount + 1;

            var feedbackQuestion = new Topicfeedbackquestion
            {
                TopicId = Guid.Parse("e3a895e4-1b3f-45b8-9c0a-98f9c0fa4996"),
                QuestionNo = questionNo,
                Question = question.Question,
                QuestionType = question.QuestionType,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "Admin"
            };

            if (question.QuestionType == "MCQ" && options != null)
            {
                foreach (var option in options)
                {
                    feedbackQuestion.Feedbackquestionsoptions.Add(new Feedbackquestionsoption
                    {
                        OptionText = option.OptionText,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "Admin"
                    });
                }
            }

            _context.Topicfeedbackquestions.Add(feedbackQuestion);
            _context.SaveChanges();

            return true;
        }

        public IEnumerable<TopicFeedbackQuestionNoDTO> GetAllFeedbackQuestions()
        {
            return _context.Topicfeedbackquestions
                .Select(q => new TopicFeedbackQuestionNoDTO
                {
                    TopicFeedbackId = q.TopicFeedbackQuestionId,
                    TopicId = q.TopicId,
                    QuestionNo = q.QuestionNo,
                    Question = q.Question,
                    QuestionType = q.QuestionType
                })
                .ToList();
        }

        public TopicFeedbackQuestionNoDTO GetFeedbackQuestionById(Guid id)
        {
            var question = _context.Topicfeedbackquestions.FirstOrDefault(q => q.TopicFeedbackQuestionId == id);
            if (question == null)
                return null;

            return new TopicFeedbackQuestionNoDTO
            {
                TopicFeedbackId = question.TopicFeedbackQuestionId,
                TopicId = question.TopicId,
                QuestionNo = question.QuestionNo,
                Question = question.Question,
                QuestionType = question.QuestionType
            };
        }

        public void AddFeedbackResponse(TopicFeedbackResponseDTO feedbackResponse)
        {
            var response = new Feedbackresponse
            {
                TopicFeedbackQuestionId = feedbackResponse.TopicFeedbackQuestionId,
                LearnerId = feedbackResponse.LearnerId,
                Response = feedbackResponse.Response,
                OptionId = feedbackResponse.OptionId
            };

            _context.Feedbackresponses.Add(response);
            _context.SaveChanges();
        }

        public bool UpdateFeedbackQuestion(Guid id, TopicFeedbackQuestionDTO question, List<FeedbackOptionDTO> options)
        {
            var existingQuestion = _context.Topicfeedbackquestions.FirstOrDefault(q => q.TopicFeedbackQuestionId == id);
            if (existingQuestion != null)
            {
                existingQuestion.Question = question.Question;
                existingQuestion.QuestionType = question.QuestionType;
                existingQuestion.ModifiedAt = DateTime.UtcNow;
                existingQuestion.ModifiedBy = "Admin";
                _context.SaveChanges();

                var existingOptions = _context.Feedbackquestionsoptions.Where(o => o.TopicFeedbackQuestionId == id).ToList();
                _context.Feedbackquestionsoptions.RemoveRange(existingOptions);
                _context.SaveChanges();

                if (options != null && options.Count > 0)
                {
                    foreach (var option in options)
                    {
                        var optionEntity = new Feedbackquestionsoption
                        {
                            TopicFeedbackQuestionId = id,
                            OptionText = option.OptionText,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = "Admin"
                        };
                        _context.Feedbackquestionsoptions.Add(optionEntity);
                    }
                    _context.SaveChanges();
                }

                return true;
            }
            return false;
        }

        public bool DeleteFeedbackQuestion(Guid id)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var existingQuestion = _context.Topicfeedbackquestions.FirstOrDefault(q => q.TopicFeedbackQuestionId == id);
                if (existingQuestion != null)
                {
                    var relatedOptions = _context.Feedbackquestionsoptions.Where(o => o.TopicFeedbackQuestionId == id).ToList();
                    if (relatedOptions.Any())
                    {
                        _context.Feedbackquestionsoptions.RemoveRange(relatedOptions);
                    }

                    _context.Topicfeedbackquestions.Remove(existingQuestion);
                    _context.SaveChanges();

                    ReorderQuestionNo(existingQuestion.TopicId, existingQuestion.QuestionNo);

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

        private void ReorderQuestionNo(Guid topicId, int deletedQuestionNo)
        {
            var subsequentQuestions = _context.Topicfeedbackquestions
                .Where(q => q.TopicId == topicId && q.QuestionNo > deletedQuestionNo)
                .ToList();

            foreach (var question in subsequentQuestions)
            {
                question.QuestionNo--;
            }
            _context.SaveChanges();
        }
    }
}*/

//using LXP.Common.DTO;
//using LXP.Data.DBContexts;
//using LXP.Data.IRepository;

//namespace LXP.Data.Repository
//{
//    public class TopicFeedbackRepository : ITopicFeedbackRepository
//    {
//        private readonly LXPDbContext _context;

//        public TopicFeedbackRepository(LXPDbContext context)
//        {
//            _context = context;
//        }

//        public bool AddFeedbackQuestion(TopicFeedbackQuestionDTO question, List<FeedbackOptionDTO> options)
//        {
//            if (question == null)
//                throw new ArgumentNullException(nameof(question));



//            int questionCount = _context.Topicfeedbackquestions.Count(q => q.TopicId == question.TopicId);

//            int questionNo = questionCount + 1;

//            var feedbackQuestion = new Topicfeedbackquestion
//            {
//                TopicId = Guid.Parse("e3a895e4-1b3f-45b8-9c0a-98f9c0fa4996"),
//                QuestionNo = questionNo,
//                Question = question.Question,
//                QuestionType = question.QuestionType,
//                CreatedAt = DateTime.UtcNow,
//                CreatedBy = "Admin"
//            };

//            if (question.QuestionType == "MCQ" && question.Options != null)
//            {
//                foreach (var option in options)
//                {
//                    feedbackQuestion.Feedbackquestionsoptions.Add(new Feedbackquestionsoption
//                    {
//                        OptionText = option.OptionText
//                    });
//                }
//            }

//            _context.Topicfeedbackquestions.Add(feedbackQuestion);
//            _context.SaveChanges();

//            return true;
//        }


//        public IEnumerable<TopicFeedbackQuestionNoDTO> GetAllFeedbackQuestions()
//        {
//            return _context.Topicfeedbackquestions
//                .Select(q => new TopicFeedbackQuestionNoDTO
//                {
//                    TopicFeedbackId = q.TopicFeedbackQuestionId,
//                    TopicId = q.TopicId,
//                    QuestionNo = q.QuestionNo,
//                    Question = q.Question,
//                    QuestionType = q.QuestionType
//                })
//                .ToList();
//        }

//        public TopicFeedbackQuestionNoDTO GetFeedbackQuestionById(Guid id)
//        {
//            var question = _context.Topicfeedbackquestions.FirstOrDefault(q => q.TopicFeedbackQuestionId == id);
//            if (question == null)
//                return null;

//            return new TopicFeedbackQuestionNoDTO
//            {
//                TopicFeedbackId = question.TopicFeedbackQuestionId,
//                TopicId = question.TopicId,
//                QuestionNo = question.QuestionNo,
//                Question = question.Question,
//                QuestionType = question.QuestionType
//            };
//        }

//        public void AddFeedbackResponse(TopicFeedbackResponseDTO feedbackResponse)
//        {
//            var response = new Feedbackresponse
//            {
//                TopicFeedbackQuestionId = feedbackResponse.TopicFeedbackQuestionId,
//                LearnerId = feedbackResponse.LearnerId,
//                Response = feedbackResponse.Response,
//                OptionId = feedbackResponse.OptionId,

//            };

//            _context.Feedbackresponses.Add(response);
//            _context.SaveChanges();
//        }

//        public bool UpdateFeedbackQuestion(Guid id, TopicFeedbackQuestionDTO question, List<FeedbackOptionDTO> options)
//        {
//            var existingQuestion = _context.Topicfeedbackquestions.FirstOrDefault(q => q.TopicFeedbackQuestionId == id);
//            if (existingQuestion != null)
//            {
//                existingQuestion.Question = question.Question;
//                existingQuestion.QuestionType = question.QuestionType;
//                existingQuestion.ModifiedAt = DateTime.UtcNow;
//                existingQuestion.ModifiedBy = "Admin";
//                _context.SaveChanges();

//                var existingOptions = _context.Feedbackquestionsoptions.Where(o => o.TopicFeedbackQuestionId == id).ToList();
//                _context.Feedbackquestionsoptions.RemoveRange(existingOptions);
//                _context.SaveChanges();

//                if (options != null && options.Count > 0)
//                {
//                    foreach (var option in options)
//                    {
//                        var optionEntity = new Feedbackquestionsoption
//                        {
//                            TopicFeedbackQuestionId = id,
//                            OptionText = option.OptionText,
//                            CreatedAt = DateTime.UtcNow,
//                            CreatedBy = "Admin"
//                        };
//                        _context.Feedbackquestionsoptions.Add(optionEntity);
//                    }
//                    _context.SaveChanges();
//                }

//                return true;
//            }
//            return false;

//        }

//        public bool DeleteFeedbackQuestion(Guid id)
//        {
//            using var transaction = _context.Database.BeginTransaction();
//            try
//            {
//                var existingQuestion = _context.Topicfeedbackquestions.FirstOrDefault(q => q.TopicFeedbackQuestionId == id);
//                if (existingQuestion != null)
//                {

//                    var relatedOptions = _context.Feedbackquestionsoptions
//                                                    .Where(o => o.TopicFeedbackQuestionId == id)
//                                                    .ToList();
//                    if (relatedOptions.Any())
//                    {
//                        _context.Feedbackquestionsoptions.RemoveRange(relatedOptions);
//                    }

//                    _context.Topicfeedbackquestions.Remove(existingQuestion);
//                    _context.SaveChanges();

//                    ReorderQuestionNo(existingQuestion.TopicId, existingQuestion.QuestionNo);

//                    transaction.Commit();
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                transaction.Rollback();
//                throw;
//            }
//            return false;

//        }


//        private void ReorderQuestionNo(Guid topicId, int deletedQuestionNo)
//        {
//            var subsequentQuestions = _context.Topicfeedbackquestions.Where(q => q.TopicId == topicId && q.QuestionNo > deletedQuestionNo).ToList();
//            foreach(var question in subsequentQuestions)
//            {
//                question.QuestionNo--;
//            }
//            _context.SaveChanges();
//        }

//    }
//}


//private void DecrementQuestionNos(Guid deletedQuestionId)
//{
//    var deletedQuestion = _context.Topicfeedbackquestions.FirstOrDefault(q => q.TopicFeedbackQuestionId == deletedQuestionId);
//    if (deletedQuestion != null)
//    {
//        var questionsToUpdate = _context.Topicfeedbackquestions
//            .Where(q => q.TopicId == deletedQuestion.TopicId && q.QuestionNo > deletedQuestion.QuestionNo)
//            .OrderBy(q => q.QuestionNo)
//            .ToList();
//        _context.SaveChanges();


//    }
//}
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using LXP.Common.DTO;
//using LXP.Data.DBContexts;
//using LXP.Data.IRepository;

//namespace LXP.Data.Repository
//{
//    public static class TopicFeedbackQuestionTypes
//    {
//        public const string MultiChoiceQuestion = "MCQ";
//        public const string DescriptiveQuestion = "Descriptive";
//    }

//    public class TopicFeedbackRepository : ITopicFeedbackRepository
//    {
//        private readonly LXPDbContext _context;

//        public TopicFeedbackRepository(LXPDbContext context)
//        {
//            _context = context;
//        }

//        //public bool AddFeedbackQuestion(TopicFeedbackQuestionDTO question, List<FeedbackOptionDTO> options)
//        //{
//        //    try
//        //    {
//        //        if (question == null)
//        //            throw new ArgumentNullException(nameof(question));

//        //        // Normalize question type to uppercase
//        //        var normalizedQuestionType = question.QuestionType.ToUpper();

//        //        // Ensure no options are saved for descriptive questions
//        //        if (normalizedQuestionType == TopicFeedbackQuestionTypes.DescriptiveQuestion.ToUpper())
//        //        {
//        //            options = null;
//        //        }

//        //        if (!ValidateOptionsByFeedbackQuestionType(normalizedQuestionType, options))
//        //            throw new ArgumentException("Invalid options for the given question type.", nameof(options));

//        //        int questionNo = GetNextFeedbackQuestionNo(question.TopicId);

//        //        var feedbackQuestion = new Topicfeedbackquestion
//        //        {
//        //            TopicId = Guid.Parse("e3a895e4-1b3f-45b8-9c0a-98f9c0fa4996"),
//        //            QuestionNo = questionNo,
//        //            Question = question.Question,
//        //            QuestionType = normalizedQuestionType,
//        //            CreatedAt = DateTime.UtcNow,
//        //            CreatedBy = "Admin"
//        //        };

//        //        _context.Topicfeedbackquestions.Add(feedbackQuestion);
//        //        _context.SaveChanges();

//        //        // Save the options only if the question type is MCQ
//        //        if (normalizedQuestionType == TopicFeedbackQuestionTypes.MultiChoiceQuestion.ToUpper() && options != null)
//        //        {
//        //            foreach (var option in options)
//        //            {
//        //                var optionEntity = new Feedbackquestionsoption
//        //                {
//        //                    TopicFeedbackQuestionId = feedbackQuestion.TopicFeedbackQuestionId,
//        //                    OptionText = option.OptionText,
//        //                    CreatedAt = DateTime.UtcNow,
//        //                    CreatedBy = "Admin"
//        //                };
//        //                _context.Feedbackquestionsoptions.Add(optionEntity);
//        //            }
//        //            _context.SaveChanges();
//        //        }

//        //        return true;
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        throw new InvalidOperationException("An error occurred while adding the feedback question.", ex);
//        //    }
//        //}
//        public bool AddFeedbackQuestion(TopicFeedbackQuestionDTO question, List<FeedbackOptionDTO> options, Guid topicId)
//        {
//            try
//            {
//                if (question == null)
//                    throw new ArgumentNullException(nameof(question));

//                // Normalize question type to uppercase
//                var normalizedQuestionType = question.QuestionType.ToUpper();

//                // Ensure no options are saved for descriptive questions
//                if (normalizedQuestionType == TopicFeedbackQuestionTypes.DescriptiveQuestion.ToUpper())
//                {
//                    options = null;
//                }

//                if (!ValidateOptionsByFeedbackQuestionType(normalizedQuestionType, options))
//                    throw new ArgumentException("Invalid options for the given question type.", nameof(options));

//                int questionNo = GetNextFeedbackQuestionNo(topicId);

//                var feedbackQuestion = new Topicfeedbackquestion
//                {
//                    TopicId = topicId,
//                    QuestionNo = questionNo,
//                    Question = question.Question,
//                    QuestionType = normalizedQuestionType,
//                    CreatedAt = DateTime.UtcNow,
//                    CreatedBy = "Admin"
//                };

//                _context.Topicfeedbackquestions.Add(feedbackQuestion);
//                _context.SaveChanges();

//                // Save the options only if the question type is MCQ
//                if (normalizedQuestionType == TopicFeedbackQuestionTypes.MultiChoiceQuestion.ToUpper() && options != null)
//                {
//                    foreach (var option in options)
//                    {
//                        var optionEntity = new Feedbackquestionsoption
//                        {
//                            TopicFeedbackQuestionId = feedbackQuestion.TopicFeedbackQuestionId,
//                            OptionText = option.OptionText,
//                            CreatedAt = DateTime.UtcNow,
//                            CreatedBy = "Admin"
//                        };
//                        _context.Feedbackquestionsoptions.Add(optionEntity);
//                    }
//                    _context.SaveChanges();
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                throw new InvalidOperationException("An error occurred while adding the feedback question.", ex);
//            }
//        }

//        public IEnumerable<TopicFeedbackQuestionNoDTO> GetAllFeedbackQuestions()
//        {
//            return _context.Topicfeedbackquestions
//                .Select(q => new TopicFeedbackQuestionNoDTO
//                {
//                    TopicFeedbackId = q.TopicFeedbackQuestionId,
//                    TopicId = q.TopicId,
//                    QuestionNo = q.QuestionNo,
//                    Question = q.Question,
//                    QuestionType = q.QuestionType,
//                    Options = _context.Feedbackquestionsoptions
//                        .Where(o => o.TopicFeedbackQuestionId == q.TopicFeedbackQuestionId)
//                        .Select(o => new FeedbackOptionDTO
//                        {
//                            OptionText = o.OptionText
//                        })
//                        .ToList()
//                }).ToList();
//        }
//        public IEnumerable<TopicFeedbackQuestionNoDTO> GetFeedbackQuestionsByTopicId(Guid topicId)
//        {
//            return _context.Topicfeedbackquestions
//                .Where(q => q.TopicId == topicId)
//                .Select(q => new TopicFeedbackQuestionNoDTO
//                {
//                    TopicFeedbackId = q.TopicFeedbackQuestionId,
//                    TopicId = q.TopicId,
//                    QuestionNo = q.QuestionNo,
//                    Question = q.Question,
//                    QuestionType = q.QuestionType,
//                    Options = _context.Feedbackquestionsoptions
//                        .Where(o => o.TopicFeedbackQuestionId == q.TopicFeedbackQuestionId)
//                        .Select(o => new FeedbackOptionDTO
//                        {
//                            OptionText = o.OptionText
//                        })
//                        .ToList()
//                })
//                .ToList();
//        }

//        public TopicFeedbackQuestionNoDTO GetFeedbackQuestionById(Guid id)
//        {
//            var question = _context.Topicfeedbackquestions
//                .Where(q => q.TopicFeedbackQuestionId == id)
//                .Select(q => new TopicFeedbackQuestionNoDTO
//                {
//                    TopicFeedbackId = q.TopicFeedbackQuestionId,
//                    TopicId = q.TopicId,
//                    QuestionNo = q.QuestionNo,
//                    Question = q.Question,
//                    QuestionType = q.QuestionType,
//                    Options = _context.Feedbackquestionsoptions
//                        .Where(o => o.TopicFeedbackQuestionId == q.TopicFeedbackQuestionId)
//                        .Select(o => new FeedbackOptionDTO
//                        {
//                            OptionText = o.OptionText
//                        })
//                        .ToList()
//                })
//                .FirstOrDefault();

//            return question;
//        }

//        public void AddFeedbackResponse(TopicFeedbackResponseDTO feedbackResponse)
//        {
//            var response = new Feedbackresponse
//            {
//                TopicFeedbackQuestionId = feedbackResponse.TopicFeedbackQuestionId,
//                LearnerId = feedbackResponse.LearnerId,
//                Response = feedbackResponse.Response,
//                OptionId = feedbackResponse.OptionId,
//            };

//            _context.Feedbackresponses.Add(response);
//            _context.SaveChanges();
//        }

//        public bool UpdateFeedbackQuestion(Guid id, TopicFeedbackQuestionDTO question, List<FeedbackOptionDTO> options)
//        {
//            try
//            {
//                var existingQuestion = _context.Topicfeedbackquestions.FirstOrDefault(q => q.TopicFeedbackQuestionId == id);
//                if (existingQuestion != null)
//                {
//                    // Check if the question type is being modified
//                    if (!existingQuestion.QuestionType.Equals(question.QuestionType, StringComparison.OrdinalIgnoreCase))
//                    {
//                        throw new InvalidOperationException("Question type cannot be modified.");
//                    }

//                    // Validate options based on question type
//                    if (!ValidateOptionsByFeedbackQuestionType(existingQuestion.QuestionType, options))
//                    {
//                        throw new ArgumentException("Invalid options for the given question type.");
//                    }

//                    // Update the question details
//                    existingQuestion.Question = question.Question;
//                    existingQuestion.ModifiedAt = DateTime.UtcNow;
//                    existingQuestion.ModifiedBy = "Admin";
//                    _context.SaveChanges();

//                    // Remove existing options if question type is MultiChoiceQuestion
//                    if (existingQuestion.QuestionType == TopicFeedbackQuestionTypes.MultiChoiceQuestion.ToUpper())
//                    {
//                        var existingOptions = _context.Feedbackquestionsoptions.Where(o => o.TopicFeedbackQuestionId == id).ToList();
//                        _context.Feedbackquestionsoptions.RemoveRange(existingOptions);
//                        _context.SaveChanges();

//                        // Add new options
//                        if (options != null && options.Count > 0)
//                        {
//                            foreach (var option in options)
//                            {
//                                var optionEntity = new Feedbackquestionsoption
//                                {
//                                    TopicFeedbackQuestionId = id,
//                                    OptionText = option.OptionText,
//                                    CreatedAt = DateTime.UtcNow,
//                                    CreatedBy = "Admin"
//                                };
//                                _context.Feedbackquestionsoptions.Add(optionEntity);
//                            }
//                            _context.SaveChanges();
//                        }
//                    }

//                    return true;
//                }
//                return false;
//            }
//            catch (Exception ex)
//            {
//                throw new InvalidOperationException("An error occurred while updating the feedback question.", ex);
//            }
//        }

//        public bool DeleteFeedbackQuestion(Guid id)
//        {
//            using var transaction = _context.Database.BeginTransaction();
//            try
//            {
//                var existingQuestion = _context.Topicfeedbackquestions.FirstOrDefault(q => q.TopicFeedbackQuestionId == id);
//                if (existingQuestion != null)
//                {
//                    var relatedOptions = _context.Feedbackquestionsoptions
//                        .Where(o => o.TopicFeedbackQuestionId == id)
//                        .ToList();

//                    if (relatedOptions.Any())
//                    {
//                        _context.Feedbackquestionsoptions.RemoveRange(relatedOptions);
//                    }

//                    _context.Topicfeedbackquestions.Remove(existingQuestion);
//                    _context.SaveChanges();

//                    ReorderQuestionNo(existingQuestion.TopicId, existingQuestion.QuestionNo);

//                    transaction.Commit();
//                    return true;
//                }
//            }
//            catch (Exception)
//            {
//                transaction.Rollback();
//                throw;
//            }
//            return false;
//        }

//        private int GetNextFeedbackQuestionNo(Guid topicId)
//        {
//            var lastQuestion = _context.Topicfeedbackquestions
//                .Where(q => q.TopicId == topicId)
//                .OrderByDescending(q => q.QuestionNo)
//                .FirstOrDefault();
//            return lastQuestion != null ? lastQuestion.QuestionNo + 1 : 1;
//        }

//        private void ReorderQuestionNo(Guid topicId, int deletedQuestionNo)
//        {
//            var subsequentQuestions = _context.Topicfeedbackquestions
//                .Where(q => q.TopicId == topicId && q.QuestionNo > deletedQuestionNo)
//                .ToList();
//            foreach (var question in subsequentQuestions)
//            {
//                question.QuestionNo--;
//            }
//            _context.SaveChanges();
//        }

//        private bool ValidateOptionsByFeedbackQuestionType(string questionType, List<FeedbackOptionDTO> options)
//        {
//            questionType = questionType.ToUpper();

//            if (questionType == TopicFeedbackQuestionTypes.MultiChoiceQuestion.ToUpper())
//            {
//                return options != null && options.Count >= 2 && options.Count <= 5;
//            }
//            return options == null || options.Count == 0;
//        }
//    }
//}


///*using LXP.Common.DTO;
//using LXP.Data.DBContexts;
//using LXP.Data.IRepository;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace LXP.Data.Repository
//{
//    public class TopicFeedbackRepository : ITopicFeedbackRepository
//    {
//        private readonly LXPDbContext _context;

//        public TopicFeedbackRepository(LXPDbContext context)
//        {
//            _context = context;
//        }

//        public bool AddFeedbackQuestion(TopicFeedbackQuestionDTO question, List<FeedbackOptionDTO> options)
//        {
//            if (question == null)
//                throw new ArgumentNullException(nameof(question));

//            int questionCount = _context.Topicfeedbackquestions.Count(q => q.TopicId == question.TopicId);
//            int questionNo = questionCount + 1;

//            var feedbackQuestion = new Topicfeedbackquestion
//            {
//                TopicId = Guid.Parse("e3a895e4-1b3f-45b8-9c0a-98f9c0fa4996"),
//                QuestionNo = questionNo,
//                Question = question.Question,
//                QuestionType = question.QuestionType,
//                CreatedAt = DateTime.UtcNow,
//                CreatedBy = "Admin"
//            };

//            if (question.QuestionType == "MCQ" && options != null)
//            {
//                foreach (var option in options)
//                {
//                    feedbackQuestion.Feedbackquestionsoptions.Add(new Feedbackquestionsoption
//                    {
//                        OptionText = option.OptionText,
//                        CreatedAt = DateTime.UtcNow,
//                        CreatedBy = "Admin"
//                    });
//                }
//            }

//            _context.Topicfeedbackquestions.Add(feedbackQuestion);
//            _context.SaveChanges();

//            return true;
//        }

//        public IEnumerable<TopicFeedbackQuestionNoDTO> GetAllFeedbackQuestions()
//        {
//            return _context.Topicfeedbackquestions
//                .Select(q => new TopicFeedbackQuestionNoDTO
//                {
//                    TopicFeedbackId = q.TopicFeedbackQuestionId,
//                    TopicId = q.TopicId,
//                    QuestionNo = q.QuestionNo,
//                    Question = q.Question,
//                    QuestionType = q.QuestionType
//                })
//                .ToList();
//        }

//        public TopicFeedbackQuestionNoDTO GetFeedbackQuestionById(Guid id)
//        {
//            var question = _context.Topicfeedbackquestions.FirstOrDefault(q => q.TopicFeedbackQuestionId == id);
//            if (question == null)
//                return null;

//            return new TopicFeedbackQuestionNoDTO
//            {
//                TopicFeedbackId = question.TopicFeedbackQuestionId,
//                TopicId = question.TopicId,
//                QuestionNo = question.QuestionNo,
//                Question = question.Question,
//                QuestionType = question.QuestionType
//            };
//        }

//        public void AddFeedbackResponse(TopicFeedbackResponseDTO feedbackResponse)
//        {
//            var response = new Feedbackresponse
//            {
//                TopicFeedbackQuestionId = feedbackResponse.TopicFeedbackQuestionId,
//                LearnerId = feedbackResponse.LearnerId,
//                Response = feedbackResponse.Response,
//                OptionId = feedbackResponse.OptionId
//            };

//            _context.Feedbackresponses.Add(response);
//            _context.SaveChanges();
//        }

//        public bool UpdateFeedbackQuestion(Guid id, TopicFeedbackQuestionDTO question, List<FeedbackOptionDTO> options)
//        {
//            var existingQuestion = _context.Topicfeedbackquestions.FirstOrDefault(q => q.TopicFeedbackQuestionId == id);
//            if (existingQuestion != null)
//            {
//                existingQuestion.Question = question.Question;
//                existingQuestion.QuestionType = question.QuestionType;
//                existingQuestion.ModifiedAt = DateTime.UtcNow;
//                existingQuestion.ModifiedBy = "Admin";
//                _context.SaveChanges();

//                var existingOptions = _context.Feedbackquestionsoptions.Where(o => o.TopicFeedbackQuestionId == id).ToList();
//                _context.Feedbackquestionsoptions.RemoveRange(existingOptions);
//                _context.SaveChanges();

//                if (options != null && options.Count > 0)
//                {
//                    foreach (var option in options)
//                    {
//                        var optionEntity = new Feedbackquestionsoption
//                        {
//                            TopicFeedbackQuestionId = id,
//                            OptionText = option.OptionText,
//                            CreatedAt = DateTime.UtcNow,
//                            CreatedBy = "Admin"
//                        };
//                        _context.Feedbackquestionsoptions.Add(optionEntity);
//                    }
//                    _context.SaveChanges();
//                }

//                return true;
//            }
//            return false;
//        }

//        public bool DeleteFeedbackQuestion(Guid id)
//        {
//            using var transaction = _context.Database.BeginTransaction();
//            try
//            {
//                var existingQuestion = _context.Topicfeedbackquestions.FirstOrDefault(q => q.TopicFeedbackQuestionId == id);
//                if (existingQuestion != null)
//                {
//                    var relatedOptions = _context.Feedbackquestionsoptions.Where(o => o.TopicFeedbackQuestionId == id).ToList();
//                    if (relatedOptions.Any())
//                    {
//                        _context.Feedbackquestionsoptions.RemoveRange(relatedOptions);
//                    }

//                    _context.Topicfeedbackquestions.Remove(existingQuestion);
//                    _context.SaveChanges();

//                    ReorderQuestionNo(existingQuestion.TopicId, existingQuestion.QuestionNo);

//                    transaction.Commit();
//                    return true;
//                }
//            }
//            catch (Exception)
//            {
//                transaction.Rollback();
//                throw;
//            }
//            return false;
//        }

//        private void ReorderQuestionNo(Guid topicId, int deletedQuestionNo)
//        {
//            var subsequentQuestions = _context.Topicfeedbackquestions
//                .Where(q => q.TopicId == topicId && q.QuestionNo > deletedQuestionNo)
//                .ToList();

//            foreach (var question in subsequentQuestions)
//            {
//                question.QuestionNo--;
//            }
//            _context.SaveChanges();
//        }
//    }
//}*/

////using LXP.Common.DTO;
////using LXP.Data.DBContexts;
////using LXP.Data.IRepository;

////namespace LXP.Data.Repository
////{
////    public class TopicFeedbackRepository : ITopicFeedbackRepository
////    {
////        private readonly LXPDbContext _context;

////        public TopicFeedbackRepository(LXPDbContext context)
////        {
////            _context = context;
////        }

////        public bool AddFeedbackQuestion(TopicFeedbackQuestionDTO question, List<FeedbackOptionDTO> options)
////        {
////            if (question == null)
////                throw new ArgumentNullException(nameof(question));



////            int questionCount = _context.Topicfeedbackquestions.Count(q => q.TopicId == question.TopicId);

////            int questionNo = questionCount + 1;

////            var feedbackQuestion = new Topicfeedbackquestion
////            {
////                TopicId = Guid.Parse("e3a895e4-1b3f-45b8-9c0a-98f9c0fa4996"),
////                QuestionNo = questionNo,
////                Question = question.Question,
////                QuestionType = question.QuestionType,
////                CreatedAt = DateTime.UtcNow,
////                CreatedBy = "Admin"
////            };

////            if (question.QuestionType == "MCQ" && question.Options != null)
////            {
////                foreach (var option in options)
////                {
////                    feedbackQuestion.Feedbackquestionsoptions.Add(new Feedbackquestionsoption
////                    {
////                        OptionText = option.OptionText
////                    });
////                }
////            }

////            _context.Topicfeedbackquestions.Add(feedbackQuestion);
////            _context.SaveChanges();

////            return true;
////        }


////        public IEnumerable<TopicFeedbackQuestionNoDTO> GetAllFeedbackQuestions()
////        {
////            return _context.Topicfeedbackquestions
////                .Select(q => new TopicFeedbackQuestionNoDTO
////                {
////                    TopicFeedbackId = q.TopicFeedbackQuestionId,
////                    TopicId = q.TopicId,
////                    QuestionNo = q.QuestionNo,
////                    Question = q.Question,
////                    QuestionType = q.QuestionType
////                })
////                .ToList();
////        }

////        public TopicFeedbackQuestionNoDTO GetFeedbackQuestionById(Guid id)
////        {
////            var question = _context.Topicfeedbackquestions.FirstOrDefault(q => q.TopicFeedbackQuestionId == id);
////            if (question == null)
////                return null;

////            return new TopicFeedbackQuestionNoDTO
////            {
////                TopicFeedbackId = question.TopicFeedbackQuestionId,
////                TopicId = question.TopicId,
////                QuestionNo = question.QuestionNo,
////                Question = question.Question,
////                QuestionType = question.QuestionType
////            };
////        }

////        public void AddFeedbackResponse(TopicFeedbackResponseDTO feedbackResponse)
////        {
////            var response = new Feedbackresponse
////            {
////                TopicFeedbackQuestionId = feedbackResponse.TopicFeedbackQuestionId,
////                LearnerId = feedbackResponse.LearnerId,
////                Response = feedbackResponse.Response,
////                OptionId = feedbackResponse.OptionId,

////            };

////            _context.Feedbackresponses.Add(response);
////            _context.SaveChanges();
////        }

////        public bool UpdateFeedbackQuestion(Guid id, TopicFeedbackQuestionDTO question, List<FeedbackOptionDTO> options)
////        {
////            var existingQuestion = _context.Topicfeedbackquestions.FirstOrDefault(q => q.TopicFeedbackQuestionId == id);
////            if (existingQuestion != null)
////            {
////                existingQuestion.Question = question.Question;
////                existingQuestion.QuestionType = question.QuestionType;
////                existingQuestion.ModifiedAt = DateTime.UtcNow;
////                existingQuestion.ModifiedBy = "Admin";
////                _context.SaveChanges();

////                var existingOptions = _context.Feedbackquestionsoptions.Where(o => o.TopicFeedbackQuestionId == id).ToList();
////                _context.Feedbackquestionsoptions.RemoveRange(existingOptions);
////                _context.SaveChanges();

////                if (options != null && options.Count > 0)
////                {
////                    foreach (var option in options)
////                    {
////                        var optionEntity = new Feedbackquestionsoption
////                        {
////                            TopicFeedbackQuestionId = id,
////                            OptionText = option.OptionText,
////                            CreatedAt = DateTime.UtcNow,
////                            CreatedBy = "Admin"
////                        };
////                        _context.Feedbackquestionsoptions.Add(optionEntity);
////                    }
////                    _context.SaveChanges();
////                }

////                return true;
////            }
////            return false;

////        }

////        public bool DeleteFeedbackQuestion(Guid id)
////        {
////            using var transaction = _context.Database.BeginTransaction();
////            try
////            {
////                var existingQuestion = _context.Topicfeedbackquestions.FirstOrDefault(q => q.TopicFeedbackQuestionId == id);
////                if (existingQuestion != null)
////                {

////                    var relatedOptions = _context.Feedbackquestionsoptions
////                                                    .Where(o => o.TopicFeedbackQuestionId == id)
////                                                    .ToList();
////                    if (relatedOptions.Any())
////                    {
////                        _context.Feedbackquestionsoptions.RemoveRange(relatedOptions);
////                    }

////                    _context.Topicfeedbackquestions.Remove(existingQuestion);
////                    _context.SaveChanges();

////                    ReorderQuestionNo(existingQuestion.TopicId, existingQuestion.QuestionNo);

////                    transaction.Commit();
////                    return true;
////                }
////            }
////            catch (Exception ex)
////            {
////                transaction.Rollback();
////                throw;
////            }
////            return false;

////        }


////        private void ReorderQuestionNo(Guid topicId, int deletedQuestionNo)
////        {
////            var subsequentQuestions = _context.Topicfeedbackquestions.Where(q => q.TopicId == topicId && q.QuestionNo > deletedQuestionNo).ToList();
////            foreach(var question in subsequentQuestions)
////            {
////                question.QuestionNo--;
////            }
////            _context.SaveChanges();
////        }

////    }
////}


////private void DecrementQuestionNos(Guid deletedQuestionId)
////{
////    var deletedQuestion = _context.Topicfeedbackquestions.FirstOrDefault(q => q.TopicFeedbackQuestionId == deletedQuestionId);
////    if (deletedQuestion != null)
////    {
////        var questionsToUpdate = _context.Topicfeedbackquestions
////            .Where(q => q.TopicId == deletedQuestion.TopicId && q.QuestionNo > deletedQuestion.QuestionNo)
////            .OrderBy(q => q.QuestionNo)
////            .ToList();
////        _context.SaveChanges();


////    }
////}
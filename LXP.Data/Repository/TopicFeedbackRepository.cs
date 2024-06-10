
using LXP.Common.Entities;
using LXP.Data.IRepository;
using LXP.Common.ViewModels.TopicFeedbackQuestionViemModel;

namespace LXP.Data.Repository
{
    public class TopicFeedbackRepository : ITopicFeedbackRepository
    {
        private readonly LXPDbContext _dbContext;

        public TopicFeedbackRepository(LXPDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddFeedbackQuestion(Topicfeedbackquestion questionEntity)
        {
            _dbContext.Topicfeedbackquestions.Add(questionEntity);
            _dbContext.SaveChanges();
        }

        public void AddFeedbackQuestionOptions(List<Feedbackquestionsoption> options)
        {
            _dbContext.Feedbackquestionsoptions.AddRange(options);
            _dbContext.SaveChanges();
        }

        public List<TopicFeedbackQuestionNoViewModel> GetAllFeedbackQuestions()
        {
            return _dbContext.Topicfeedbackquestions
                .Select(q => new TopicFeedbackQuestionNoViewModel
                {
                    TopicFeedbackQuestionId = q.TopicFeedbackQuestionId,
                    TopicId = q.TopicId,
                    QuestionNo = q.QuestionNo,
                    Question = q.Question,
                    QuestionType = q.QuestionType,
                    Options = _dbContext.Feedbackquestionsoptions
                                        .Where(o => o.TopicFeedbackQuestionId == q.TopicFeedbackQuestionId)
                                        .Select(o => new TopicFeedbackQuestionsOptionViewModel
                                        {
                                            OptionText = o.OptionText
                                        }).ToList()
                }).ToList();
        }

        public List<TopicFeedbackQuestionNoViewModel> GetFeedbackQuestionsByTopicId(Guid topicId)


        {
            return _dbContext.Topicfeedbackquestions
                .Where(q => q.TopicId == topicId)
                .Select(q => new TopicFeedbackQuestionNoViewModel
                {
                    TopicFeedbackQuestionId = q.TopicFeedbackQuestionId,
                    TopicId = q.TopicId,
                    QuestionNo = q.QuestionNo,
                    Question = q.Question,
                    QuestionType = q.QuestionType,
                    Options = _dbContext.Feedbackquestionsoptions
                                        .Where(o => o.TopicFeedbackQuestionId == q.TopicFeedbackQuestionId)
                                        .Select(o => new TopicFeedbackQuestionsOptionViewModel
                                        {
                                            OptionText = o.OptionText
                                        }).ToList()
                }).ToList();
        }

        public int GetNextFeedbackQuestionNo(Guid topicId)
        {
            var lastQuestion = _dbContext.Topicfeedbackquestions
                .Where(q => q.TopicId == topicId)
                .OrderByDescending(q => q.QuestionNo)
                .FirstOrDefault();
            return lastQuestion != null ? lastQuestion.QuestionNo + 1 : 1;
        }

        public TopicFeedbackQuestionNoViewModel GetFeedbackQuestionById(Guid topicFeedbackQuestionId)
        {
            var feedbackQuestion = _dbContext.Topicfeedbackquestions
                .Where(q => q.TopicFeedbackQuestionId == topicFeedbackQuestionId)
                .Select(q => new
                {
                    q.TopicFeedbackQuestionId,
                    q.TopicId,
                    q.QuestionNo,
                    q.Question,
                    q.QuestionType,
                    Options = _dbContext.Feedbackquestionsoptions
                                .Where(o => o.TopicFeedbackQuestionId == q.TopicFeedbackQuestionId)
                                .Select(o => new TopicFeedbackQuestionsOptionViewModel
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

            return new TopicFeedbackQuestionNoViewModel
            {
                TopicFeedbackQuestionId = feedbackQuestion.TopicFeedbackQuestionId,
                TopicId = feedbackQuestion.TopicId,
                QuestionNo = feedbackQuestion.QuestionNo,
                Question = feedbackQuestion.Question,
                QuestionType = feedbackQuestion.QuestionType,
                Options = feedbackQuestion.Options ?? new List<TopicFeedbackQuestionsOptionViewModel>()
            };
        }

        public void UpdateFeedbackQuestion(Topicfeedbackquestion questionEntity)
        {
            _dbContext.SaveChanges();
        }

        public void RemoveFeedbackQuestion(Topicfeedbackquestion questionEntity)
        {
            _dbContext.Topicfeedbackquestions.Remove(questionEntity);
            _dbContext.SaveChanges();
        }

        public void RemoveFeedbackQuestionOptions(List<Feedbackquestionsoption> options)
        {
            _dbContext.Feedbackquestionsoptions.RemoveRange(options);
            _dbContext.SaveChanges();
        }

        public void ReorderQuestionNos(Guid topicId, int deletedQuestionNo)
        {
            var questionsToUpdate = _dbContext.Topicfeedbackquestions
                                              .Where(q => q.TopicId == topicId && q.QuestionNo > deletedQuestionNo)
                                              .OrderBy(q => q.QuestionNo)
                                              .ToList();

            foreach (var question in questionsToUpdate)
            {
                question.QuestionNo--;
            }
            _dbContext.SaveChanges();
        }

        public List<Feedbackquestionsoption> GetFeedbackQuestionOptionsById(Guid topicFeedbackQuestionId)
        {
            return _dbContext.Feedbackquestionsoptions
                .Where(o => o.TopicFeedbackQuestionId == topicFeedbackQuestionId)
                .ToList();
        }

        public Topicfeedbackquestion GetTopicFeedbackQuestionEntityById(Guid topicFeedbackQuestionId)
        {
            return _dbContext.Topicfeedbackquestions.FirstOrDefault(q => q.TopicFeedbackQuestionId == topicFeedbackQuestionId);
        }
    }
}
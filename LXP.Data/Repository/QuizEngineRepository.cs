using LXP.Common.Entities;
using LXP.Common.ViewModels.QuizEngineViewModel;
using LXP.Data.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LXP.Data.Repository
{
    public class QuizEngineRepository : IQuizEngineRepository
    {
        private readonly LXPDbContext _dbContext;

        public QuizEngineRepository(LXPDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsQuestionOptionCorrectAsync(
            Guid quizQuestionId,
            Guid questionOptionId
        )
        {
            return await _dbContext.QuestionOptions.AnyAsync(o =>
                o.QuizQuestionId == quizQuestionId
                && o.QuestionOptionId == questionOptionId
                && o.IsCorrect
            );
        }

        public async Task<string> GetQuestionTypeByIdAsync(Guid quizQuestionId)
        {
            return await _dbContext
                    .QuizQuestions.Where(q => q.QuizQuestionId == quizQuestionId)
                    .Select(q => q.QuestionType)
                    .FirstOrDefaultAsync() ?? string.Empty;
        }

        public async Task<IEnumerable<string>> GetCorrectOptionsForQuestionAsync(
            Guid quizQuestionId
        )
        {
            return await _dbContext
                .QuestionOptions.Where(o => o.QuizQuestionId == quizQuestionId && o.IsCorrect)
                .Select(o => o.Option)
                .ToListAsync();
        }

        public async Task<LearnerAttemptViewModel?> CreateLearnerAttemptAsync(
            Guid learnerId,
            Guid quizId,
            DateTime startTime
        )
        {
            var quiz = await _dbContext.Quizzes.FindAsync(quizId);
            if (quiz == null)
                return null; // or throw an exception if you prefer

            var existingAttempts = await _dbContext.LearnerAttempts.CountAsync(a =>
                a.LearnerId == learnerId && a.QuizId == quizId
            );

            if (quiz.AttemptsAllowed.HasValue && existingAttempts >= quiz.AttemptsAllowed)
                return null; // Return null to indicate maximum attempts reached

            var attempt = new LearnerAttempt
            {
                LearnerId = learnerId,
                QuizId = quizId,
                AttemptCount = existingAttempts + 1,
                StartTime = startTime,
                EndTime = startTime.AddMinutes(quiz.Duration),
                Score = 0,
                CreatedBy = "Learner"
            };

            _dbContext.LearnerAttempts.Add(attempt);
            await _dbContext.SaveChangesAsync();

            return new LearnerAttemptViewModel
            {
                LearnerAttemptId = attempt.LearnerAttemptId,
                LearnerId = attempt.LearnerId,
                QuizId = attempt.QuizId,
                AttemptCount = attempt.AttemptCount,
                StartTime = attempt.StartTime,
                EndTime = attempt.EndTime,
                Score = attempt.Score
            };
        }

        public async Task CreateLearnerAnswerAsync(
            Guid learnerAttemptId,
            Guid quizQuestionId,
            Guid questionOptionId
        )
        {
            var learnerAnswer = new LearnerAnswer
            {
                LearnerAttemptId = learnerAttemptId,
                QuizQuestionId = quizQuestionId,
                QuestionOptionId = questionOptionId,
                CreatedBy = "Learner",
                CreatedAt = DateTime.Now
            };

            _dbContext.LearnerAnswers.Add(learnerAnswer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<LearnerAttemptViewModel> GetLearnerAttemptByIdAsync(Guid attemptId)
        {
            return await _dbContext
                    .LearnerAttempts.Where(a => a.LearnerAttemptId == attemptId)
                    .Select(a => new LearnerAttemptViewModel
                    {
                        LearnerAttemptId = a.LearnerAttemptId,
                        LearnerId = a.LearnerId,
                        QuizId = a.QuizId,
                        AttemptCount = a.AttemptCount,
                        StartTime = a.StartTime,
                        EndTime = a.EndTime,
                        Score = a.Score
                    })
                    .FirstOrDefaultAsync() ?? new LearnerAttemptViewModel();
        }

        public async Task<IEnumerable<string>> GetQuestionOptionsAsync(Guid quizQuestionId)
        {
            return await _dbContext
                .QuestionOptions.Where(o => o.QuizQuestionId == quizQuestionId)
                .Select(o => o.Option)
                .ToListAsync();
        }

        public async Task<bool> IsAllowedToAttemptQuizAsync(Guid learnerId, Guid quizId)
        {
            var quiz = await _dbContext.Quizzes.FindAsync(quizId);
            if (quiz == null)
                return false;

            var existingAttempts = await _dbContext
                .LearnerAttempts.Where(a => a.LearnerId == learnerId && a.QuizId == quizId)
                .ToListAsync();

            var passMark = quiz.PassMark;
            var hasPassedQuiz = existingAttempts.Any(a => a.Score >= passMark);
            if (hasPassedQuiz)
                return false; // User has already passed the quiz

            var attemptsAllowed = quiz.AttemptsAllowed;
            if (attemptsAllowed.HasValue && existingAttempts.Count >= attemptsAllowed)
                return false; // User has exceeded the maximum number of attempts

            return true; // User is allowed to attempt the quiz
        }

        public async Task ClearLearnerAnswersAsync(Guid attemptId, Guid quizQuestionId) //newly added
        {
            var existingAnswers = await _dbContext
                .LearnerAnswers.Where(a =>
                    a.LearnerAttemptId == attemptId && a.QuizQuestionId == quizQuestionId
                )
                .ToListAsync();

            _dbContext.LearnerAnswers.RemoveRange(existingAnswers);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<
            IEnumerable<LearnerAnswerViewModel>
        > GetLearnerAnswersByAttemptAndQuestionAsync(Guid attemptId, Guid quizQuestionId) // newly added
        {
            return await _dbContext
                .LearnerAnswers.Where(a =>
                    a.LearnerAttemptId == attemptId && a.QuizQuestionId == quizQuestionId
                )
                .Select(a => new LearnerAnswerViewModel
                {
                    LearnerAnswerId = a.LearnerAnswerId,
                    LearnerAttemptId = a.LearnerAttemptId,
                    QuizQuestionId = a.QuizQuestionId,
                    QuestionOptionId = a.QuestionOptionId
                })
                .ToListAsync();
        }

        // public async Task<IEnumerable<QuizEngineQuestionViewModel>> GetQuestionsForQuizAsync(
        //     Guid quizId
        // )
        // {
        //     return await _dbContext
        //         .QuizQuestions.Where(q => q.QuizId == quizId)
        //         .Select(q => new QuizEngineQuestionViewModel
        //         {
        //             QuizQuestionId = q.QuizQuestionId,
        //             Question = q.Question,
        //             QuestionType = q.QuestionType,
        //             QuestionNo = q.QuestionNo,
        //             Options = _dbContext
        //                 .QuestionOptions.Where(o => o.QuizQuestionId == q.QuizQuestionId)
        //                 .Select(o => new QuizEngineOptionViewModel { Option = o.Option })
        //                 .ToList()
        //         })
        //         .ToListAsync();
        // }
        public async Task<IEnumerable<QuizEngineQuestionViewModel>> GetQuestionsForQuizAsync(
            Guid quizId
        )
        {
            var questions = await _dbContext
                .QuizQuestions.Where(q => q.QuizId == quizId)
                .Select(q => new QuizEngineQuestionViewModel
                {
                    QuizQuestionId = q.QuizQuestionId,
                    Question = q.Question,
                    QuestionType = q.QuestionType,
                    Options = _dbContext
                        .QuestionOptions.Where(o => o.QuizQuestionId == q.QuizQuestionId)
                        .Select(o => new QuizEngineOptionViewModel { Option = o.Option })
                        .ToList()
                })
                .ToListAsync();

            // Shuffle the questions and assign new question numbers
            var shuffledQuestions = questions
                .OrderBy(q => Guid.NewGuid())
                .Select(
                    (q, index) =>
                        new QuizEngineQuestionViewModel
                        {
                            QuizQuestionId = q.QuizQuestionId,
                            Question = q.Question,
                            QuestionType = q.QuestionType,
                            QuestionNo = index + 1, // Assign new question number based on shuffled order
                            Options = q.Options.OrderBy(o => Guid.NewGuid()).ToList()
                        }
                )
                .ToList();

            return shuffledQuestions;
        }

        public async Task<LearnerQuizStatusViewModel> GetLearnerQuizStatusAsync(
            Guid learnerId,
            Guid quizId
        )
        {
            var quiz = await _dbContext.Quizzes.FindAsync(quizId);
            if (quiz == null)
                return new LearnerQuizStatusViewModel { IsPassed = false, IsAbleToAttempt = false };

            var existingAttempts = await _dbContext
                .LearnerAttempts.Where(a => a.LearnerId == learnerId && a.QuizId == quizId)
                .ToListAsync();

            var passMark = quiz.PassMark;
            var hasPassedQuiz = existingAttempts.Any(a => a.Score >= passMark);

            var attemptsAllowed = quiz.AttemptsAllowed ?? int.MaxValue;
            var attemptsRemaining = attemptsAllowed - existingAttempts.Count;

            return new LearnerQuizStatusViewModel
            {
                IsPassed = hasPassedQuiz,
                IsAbleToAttempt = attemptsRemaining > 0
            };
        } //2206

        public async Task<IEnumerable<LearnerAttemptViewModel>> GetLearnerAttemptsForQuizAsync(
            Guid learnerId,
            Guid quizId
        )
        {
            return await _dbContext
                .LearnerAttempts.Where(a => a.LearnerId == learnerId && a.QuizId == quizId)
                .Select(a => new LearnerAttemptViewModel
                {
                    LearnerAttemptId = a.LearnerAttemptId,
                    LearnerId = a.LearnerId,
                    QuizId = a.QuizId,
                    AttemptCount = a.AttemptCount,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Score = a.Score
                })
                .ToListAsync();
        }

        public async Task UpdateLearnerAnswerAsync(Guid learnerAnswerId, Guid questionOptionId)
        {
            var existingAnswer = await _dbContext.LearnerAnswers.FindAsync(learnerAnswerId);
            if (existingAnswer != null)
            {
                existingAnswer.QuestionOptionId = questionOptionId;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<string> GetOptionTextByIdAsync(Guid optionId)
        {
            return await _dbContext
                .QuestionOptions.Where(o => o.QuestionOptionId == optionId)
                .Select(o => o.Option)
                .FirstOrDefaultAsync();
        }

        public async Task<Guid> GetOptionIdByTextAsync(Guid quizQuestionId, string optionText)
        {
            return await _dbContext
                .QuestionOptions.Where(o =>
                    o.QuizQuestionId == quizQuestionId && o.Option == optionText
                )
                .Select(o => o.QuestionOptionId)
                .FirstOrDefaultAsync();
        }

        public async Task<ViewQuizDetailsViewModel> GetQuizByIdAsync(Guid quizId)
        {
            return await _dbContext
                    .Quizzes.Where(q => q.QuizId == quizId)
                    .Select(q => new ViewQuizDetailsViewModel
                    {
                        QuizId = q.QuizId,
                        TopicId = q.TopicId,
                        CourseId = q.CourseId,
                        NameOfQuiz = q.NameOfQuiz,
                        Duration = q.Duration,
                        PassMark = q.PassMark,
                        AttemptsAllowed = q.AttemptsAllowed
                    })
                    .FirstOrDefaultAsync() ?? new ViewQuizDetailsViewModel();
        }

        public async Task<IEnumerable<LearnerAnswerViewModel>> GetLearnerAnswersForAttemptAsync(
            Guid attemptId
        )
        {
            return await _dbContext
                .LearnerAnswers.Where(a => a.LearnerAttemptId == attemptId)
                .Select(a => new LearnerAnswerViewModel
                {
                    LearnerAnswerId = a.LearnerAnswerId,
                    LearnerAttemptId = a.LearnerAttemptId,
                    QuizQuestionId = a.QuizQuestionId,
                    QuestionOptionId = a.QuestionOptionId
                })
                .ToListAsync();
        }

        public async Task UpdateLearnerAttemptAsync(LearnerAttemptViewModel attempt)
        {
            var existingAttempt = await _dbContext.LearnerAttempts.FindAsync(
                attempt.LearnerAttemptId
            );
            if (existingAttempt != null)
            {
                existingAttempt.Score = attempt.Score;
                existingAttempt.EndTime = attempt.EndTime;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<ViewQuizDetailsViewModel> GetQuizDetailsByTopicIdAsync(Guid topicId)
        {
            return await _dbContext
                    .Quizzes.Where(q => q.TopicId == topicId)
                    .Select(q => new ViewQuizDetailsViewModel
                    {
                        QuizId = q.QuizId,
                        TopicId = q.TopicId,
                        CourseId = q.CourseId,
                        NameOfQuiz = q.NameOfQuiz,
                        Duration = q.Duration,
                        PassMark = q.PassMark,
                        AttemptsAllowed = q.AttemptsAllowed
                    })
                    .FirstOrDefaultAsync() ?? new ViewQuizDetailsViewModel();
        }

        public async Task<LearnerQuizAttemptViewModel> GetLearnerQuizAttemptAsync(Guid attemptId)
        {
            var attempt = await _dbContext
                .LearnerAttempts.Include(a => a.Quiz)
                .FirstOrDefaultAsync(a => a.LearnerAttemptId == attemptId);

            if (attempt == null)
                throw new KeyNotFoundException($"Learner attempt with ID {attemptId} not found.");

            var questionResponses = await (
                from la in _dbContext.LearnerAnswers
                join qq in _dbContext.QuizQuestions on la.QuizQuestionId equals qq.QuizQuestionId
                join qo in _dbContext.QuestionOptions
                    on la.QuestionOptionId equals qo.QuestionOptionId
                where la.LearnerAttemptId == attemptId
                group new
                {
                    qq.QuizQuestionId,
                    qq.Question,
                    qq.QuestionType,
                    qo.Option,
                    Options = _dbContext
                        .QuestionOptions.Where(o => o.QuizQuestionId == qq.QuizQuestionId)
                        .Select(o => new QuizEngineOptionViewModel { Option = o.Option })
                        .ToList()
                } by new
                {
                    qq.QuizQuestionId,
                    qq.Question,
                    qq.QuestionType
                } into g
                select new QuestionResponseViewModel
                {
                    QuizQuestionId = g.Key.QuizQuestionId,
                    Question = g.Key.Question,
                    QuestionType = g.Key.QuestionType,
                    SelectedOptions = g.Select(x => x.Option).ToList(),
                    Options = g.First().Options
                }
            ).ToListAsync();

            return new LearnerQuizAttemptViewModel
            {
                QuizId = attempt.QuizId,
                TopicId = attempt.Quiz.TopicId,
                LearnerAttemptId = attempt.LearnerAttemptId,
                QuestionResponses = questionResponses
            };
        }

        public async Task<LearnerQuizAttemptResultViewModel> GetLearnerQuizAttemptResultAsync(
            Guid attemptId
        )
        {
            var attempt = await _dbContext
                .LearnerAttempts.Include(a => a.Quiz)
                .FirstOrDefaultAsync(a => a.LearnerAttemptId == attemptId);

            if (attempt == null)
                return null;

            var quiz = await _dbContext.Quizzes.FindAsync(attempt.QuizId);

            if (quiz == null)
                return null;

            var totalAttemptsAllowed = quiz.AttemptsAllowed ?? int.MaxValue;
            var attemptsRemaining = totalAttemptsAllowed - attempt.AttemptCount;

            var timeTaken = (attempt.EndTime - attempt.StartTime).TotalSeconds;

            return new LearnerQuizAttemptResultViewModel
            {
                QuizId = attempt.QuizId,
                TopicId = quiz.TopicId,
                LearnerAttemptId = attempt.LearnerAttemptId,
                StartTime = attempt.StartTime,
                EndTime = attempt.EndTime,
                TimeTaken = timeTaken,
                CurrentAttempt = attempt.AttemptCount,
                AttemptsRemaining = attemptsRemaining,
                Score = attempt.Score,
                IsPassed = attempt.Score >= quiz.PassMark
            };
        }

        public async Task SaveLearnerAnswerAsync(LearnerAnswerViewModel learnerAnswer)
        {
            var entity = new LearnerAnswer
            {
                LearnerAnswerId = learnerAnswer.LearnerAnswerId,
                LearnerAttemptId = learnerAnswer.LearnerAttemptId,
                QuizQuestionId = learnerAnswer.QuizQuestionId,
                QuestionOptionId = learnerAnswer.QuestionOptionId
            };

            _dbContext.LearnerAnswers.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<QuizEngineQuestionViewModel> GetQuizQuestionByIdAsync(Guid quizQuestionId)
        {
            return await _dbContext
                .QuizQuestions.Where(q => q.QuizQuestionId == quizQuestionId)
                .Select(q => new QuizEngineQuestionViewModel
                {
                    QuizQuestionId = q.QuizQuestionId,
                    Question = q.Question,
                    QuestionType = q.QuestionType,
                    QuestionNo = q.QuestionNo,
                    Options = _dbContext
                        .QuestionOptions.Where(o => o.QuizQuestionId == q.QuizQuestionId)
                        .Select(o => new QuizEngineOptionViewModel { Option = o.Option })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}

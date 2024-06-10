
using LXP.Common.ViewModels.QuizEngineViewModel;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using Microsoft.Extensions.Caching.Memory;


namespace LXP.Core.Services
{

    public class QuizEngineService : IQuizEngineService
    {
        private readonly IQuizEngineRepository _quizEngineRepository;
        private readonly IMemoryCache _memoryCache;

        public QuizEngineService(IQuizEngineRepository quizEngineRepository, IMemoryCache memoryCache)
        {
            _quizEngineRepository = quizEngineRepository;
            _memoryCache = memoryCache;
        }

        

        public async Task<ViewQuizDetailsViewModel> GetQuizByIdAsync(Guid quizId)
        {
            return await _quizEngineRepository.GetQuizByIdAsync(quizId);
        }

        public async Task<IEnumerable<QuizEngineQuestionViewModel>> GetQuestionsForQuizAsync(Guid quizId)
        {
            return await _quizEngineRepository.GetQuestionsForQuizAsync(quizId);
        }

        public async Task<ViewQuizDetailsViewModel> GetQuizDetailsByTopicIdAsync(Guid topicId)
        {
            return await _quizEngineRepository.GetQuizDetailsByTopicIdAsync(topicId);
        }


        //public async Task<Guid> StartQuizAttemptAsync(Guid learnerId, Guid quizId)
        //{
        //    var quiz = await _quizEngineRepository.GetQuizByIdAsync(quizId);
        //    if (quiz == null)
        //        throw new KeyNotFoundException($"Quiz with ID {quizId} not found.");

        //    var isAllowedToAttempt = await _quizEngineRepository.IsAllowedToAttemptQuizAsync(learnerId, quizId);
        //    if (!isAllowedToAttempt)
        //    {
        //        var existingAttempts = await _quizEngineRepository.GetLearnerAttemptsForQuizAsync(learnerId, quizId);
        //        var passMark = quiz.PassMark;
        //        var hasPassedQuiz = existingAttempts.Any(a => a.Score >= passMark);

        //        if (hasPassedQuiz)
        //            throw new InvalidOperationException("You have already passed this quiz in a previous attempt.");
        //        else
        //            throw new InvalidOperationException("You have exceeded the maximum number of attempts for this quiz.");
        //    }

        //    var startTime = DateTime.UtcNow;
        //    var attempt = await _quizEngineRepository.CreateLearnerAttemptAsync(learnerId, quizId, startTime);
        //    return attempt.LearnerAttemptId;
        //}
        public async Task<Guid> StartQuizAttemptAsync(Guid learnerId, Guid quizId)
        {
            var quiz = await _quizEngineRepository.GetQuizByIdAsync(quizId);
            if (quiz == null)
                throw new KeyNotFoundException($"Quiz with ID {quizId} not found.");

            var isAllowedToAttempt = await _quizEngineRepository.IsAllowedToAttemptQuizAsync(learnerId, quizId);
            if (!isAllowedToAttempt)
            {
                var existingAttempts = await _quizEngineRepository.GetLearnerAttemptsForQuizAsync(learnerId, quizId);
                var passMark = quiz.PassMark;
                var hasPassedQuiz = existingAttempts.Any(a => a.Score >= passMark);

                if (hasPassedQuiz)
                    throw new InvalidOperationException("You have already passed this quiz in a previous attempt.");
                else
                    throw new InvalidOperationException("You have exceeded the maximum number of attempts for this quiz.");
            }

            var startTime = DateTime.UtcNow;
            var attempt = await _quizEngineRepository.CreateLearnerAttemptAsync(learnerId, quizId, startTime);
            if (attempt == null)
                throw new InvalidOperationException("You have exceeded the maximum number of attempts for this quiz.");

            return attempt.LearnerAttemptId;
        }


        public async Task SubmitAnswerAsync(AnswerSubmissionModel answerSubmissionModel)
        {
            var attempt = await _quizEngineRepository.GetLearnerAttemptByIdAsync(answerSubmissionModel.LearnerAttemptId);
            if (attempt == null)
                throw new KeyNotFoundException($"Learner attempt with ID {answerSubmissionModel.LearnerAttemptId} not found.");
            if (DateTime.UtcNow > attempt.EndTime)
                throw new InvalidOperationException("Time limit for submitting the quiz has expired.");

            await _quizEngineRepository.ClearLearnerAnswersAsync(answerSubmissionModel.LearnerAttemptId, answerSubmissionModel.QuizQuestionId);

            var availableOptions = await _quizEngineRepository.GetQuestionOptionsAsync(answerSubmissionModel.QuizQuestionId);
            var availableOptionsIgnoreCase = availableOptions.Select(o => o.ToLower()).ToList();
            var questionType = await _quizEngineRepository.GetQuestionTypeByIdAsync(answerSubmissionModel.QuizQuestionId);

            switch (questionType)
            {
                case "MCQ":
                case "T/F":
                    if (answerSubmissionModel.SelectedOptions.Count > 1)
                        throw new InvalidOperationException("Only one option is allowed for this question type.");
                    break;
                case "MSQ":
                    if (answerSubmissionModel.SelectedOptions.Count < 2 || answerSubmissionModel.SelectedOptions.Count > 3)
                        throw new InvalidOperationException("You must select between 2 and 3 options for this question type.");
                    break;
            }

            foreach (var selectedOption in answerSubmissionModel.SelectedOptions)
            {
                var optionText = selectedOption.ToString();
                var optionTextLower = optionText.ToLower();

                if (!availableOptionsIgnoreCase.Contains(optionTextLower))
                {
                    throw new InvalidOperationException($"The selected option '{optionText}' is not a valid option for the given question.");
                }

                var optionId = await _quizEngineRepository.GetOptionIdByTextAsync(answerSubmissionModel.QuizQuestionId, optionTextLower);
                await _quizEngineRepository.CreateLearnerAnswerAsync(answerSubmissionModel.LearnerAttemptId, answerSubmissionModel.QuizQuestionId, optionId);
            }
        }

        public async Task SubmitQuizAttemptAsync(Guid attemptId)
        {
            var attempt = await _quizEngineRepository.GetLearnerAttemptByIdAsync(attemptId);
            if (attempt == null)
                throw new KeyNotFoundException($"Learner attempt with ID {attemptId} not found.");
            var quiz = await _quizEngineRepository.GetQuizByIdAsync(attempt.QuizId);
            if (quiz == null)
                throw new KeyNotFoundException($"Quiz with ID {attempt.QuizId} not found.");
            var totalQuestions = (await _quizEngineRepository.GetQuestionsForQuizAsync(quiz.QuizId)).Count();
            var existingAnswers = await _quizEngineRepository.GetLearnerAnswersForAttemptAsync(attemptId);
            if (existingAnswers.Select(a => a.QuizQuestionId).Distinct().Count() != totalQuestions)
                throw new InvalidOperationException("You need to answer all the questions in the quiz before submitting the quiz attempt.");
            var individualQuestionMarks = 100 / totalQuestions;
            float finalScore = 0;
            foreach (var answer in existingAnswers)
            {
                var isAnswerCorrect = await _quizEngineRepository.IsQuestionOptionCorrectAsync(answer.QuizQuestionId, answer.QuestionOptionId);
                var questionScore = await CalculateQuestionScore(answer.QuizQuestionId, isAnswerCorrect, individualQuestionMarks, new AnswerSubmissionModel
                {
                    LearnerAttemptId = attemptId,
                    QuizQuestionId = answer.QuizQuestionId,
                    SelectedOptions = new List<string> { await _quizEngineRepository.GetOptionTextByIdAsync(answer.QuestionOptionId) }
                });
                finalScore += questionScore;
            }
            attempt.Score = (float)Math.Round(finalScore);
            attempt.EndTime = DateTime.UtcNow;
            await _quizEngineRepository.UpdateLearnerAttemptAsync(attempt);
        }


        public async Task<Guid> RetakeQuizAsync(Guid learnerId, Guid quizId)
        {
            var quiz = await _quizEngineRepository.GetQuizByIdAsync(quizId);
            if (quiz == null)
                throw new KeyNotFoundException($"Quiz with ID {quizId} not found.");

            var isAllowedToAttempt = await _quizEngineRepository.IsAllowedToAttemptQuizAsync(learnerId, quizId);
            if (!isAllowedToAttempt)
            {
                var existingAttempts = await _quizEngineRepository.GetLearnerAttemptsForQuizAsync(learnerId, quizId);
                var passMark = quiz.PassMark;
                var hasPassedQuiz = existingAttempts.Any(a => a.Score >= passMark);

                if (hasPassedQuiz)
                    throw new InvalidOperationException("You have already passed this quiz in a previous attempt and cannot retake it.");
                else
                    throw new InvalidOperationException("You have exceeded the maximum number of attempts for this quiz.");
            }

            var startTime = DateTime.UtcNow;
            var attempt = await _quizEngineRepository.CreateLearnerAttemptAsync(learnerId, quizId, startTime);
            return attempt.LearnerAttemptId;
        }


        private async Task<float> CalculateQuestionScore(Guid quizQuestionId, bool isAnswerCorrect, float individualQuestionMarks, AnswerSubmissionModel answerSubmissionModel)
        {
            var questionType = await _quizEngineRepository.GetQuestionTypeByIdAsync(quizQuestionId);
            switch (questionType)
            {
                case "MCQ":
                case "T/F":
                    return isAnswerCorrect ? individualQuestionMarks : 0;

                case "MSQ":
                    var correctOptions = await _quizEngineRepository.GetCorrectOptionsForQuestionAsync(quizQuestionId);
                    var correctOptionCount = correctOptions.Count();
                    var selectedOptions = answerSubmissionModel.SelectedOptions.Select(o => o.ToString()).ToList();
                    var correctlySelectedOptions = selectedOptions.Intersect(correctOptions).Count();

                    if (correctlySelectedOptions == correctOptionCount)
                    {
                        return individualQuestionMarks; // All correct options selected
                    }
                    else if (correctlySelectedOptions > 0)
                    {
                        var partialMark = (individualQuestionMarks / correctOptionCount) * correctlySelectedOptions;
                        return partialMark; // Partial marks for partially correct answer
                    }
                    else
                    {
                        return 0; // No marks for incorrect answer
                    }

                default:
                    return 0;
            }
        }


        public async Task<LearnerQuizAttemptViewModel> GetLearnerQuizAttemptAsync(Guid attemptId)
        {
            return await _quizEngineRepository.GetLearnerQuizAttemptAsync(attemptId);
        }
        public async Task<LearnerQuizAttemptResultViewModel> GetLearnerQuizAttemptResultAsync(Guid attemptId)
        {
            return await _quizEngineRepository.GetLearnerQuizAttemptResultAsync(attemptId);
        }


        // new batch


        public async Task SubmitAnswerBatchAsync(AnswerSubmissionBatchModel model)
        {
            var validationErrors = new List<string>();

            foreach (var submission in model.AnswerSubmissions)
            {
                // Validate each submission
                var errors = await ValidateSubmissionAsync(submission);
                if (errors.Count > 0)
                {
                    validationErrors.AddRange(errors);
                    continue; // Skip this submission if validation fails
                }

                foreach (var option in submission.SelectedOptions)
                {
                    var learnerAnswer = new LearnerAnswerViewModel
                    {
                        LearnerAnswerId = Guid.NewGuid(),
                        LearnerAttemptId = submission.LearnerAttemptId,
                        QuizQuestionId = submission.QuizQuestionId,
                        QuestionOptionId = new Guid(option)
                    };

                    await _quizEngineRepository.SaveLearnerAnswerAsync(learnerAnswer);
                }
            }

            if (validationErrors.Count > 0)
            {
                throw new Exception(string.Join("; ", validationErrors));
            }
        }

        private async Task<List<string>> ValidateSubmissionAsync(AnswerSubmissionModel submission)
        {
            var errors = new List<string>();

            // Example validation: Check if LearnerAttemptId is valid
            if (submission.LearnerAttemptId == Guid.Empty)
            {
                errors.Add("LearnerAttemptId is invalid.");
            }

            // Example validation: Check if QuizQuestionId is valid
            if (submission.QuizQuestionId == Guid.Empty)
            {
                errors.Add("QuizQuestionId is invalid.");
            }

            // Example validation: Check if there are any selected options
            if (submission.SelectedOptions == null || submission.SelectedOptions.Count == 0)
            {
                errors.Add("No options selected.");
            }

            // Fetch the question type
            var question = await _quizEngineRepository.GetQuizQuestionByIdAsync(submission.QuizQuestionId);
            if (question != null)
            {
                if (question.QuestionType == "MSQ" && (submission.SelectedOptions.Count < 2 || submission.SelectedOptions.Count > 3))
                {
                    errors.Add("MSQ type question must have 2 or 3 options selected.");
                }
            }
            else
            {
                errors.Add("Quiz question not found.");
            }

            // Add more validations as needed

            return errors;
        }
       public async Task CacheAnswersAsync(CachedAnswerSubmissionModel model)
        {
            // Store the cached answers in memory
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30));
            _memoryCache.Set($"CachedAnswers_{model.LearnerAttemptId}", model.QuestionAnswers, cacheEntryOptions);

            // Save the cached answers to the repository
            await _quizEngineRepository.SaveCachedAnswersAsync(model.LearnerAttemptId, model.QuestionAnswers);

            // TODO: Implement any additional logic, such as validation or processing
        }
        public async Task SubmitCachedAnswersAsync(Guid learnerAttemptId)
        {
            // Retrieve the cached answers from memory
            if (_memoryCache.TryGetValue($"CachedAnswers_{learnerAttemptId}", out Dictionary<Guid, List<string>> questionAnswers))
            {
                // Convert the cached answers to the format expected by the SubmitAnswer endpoint
                var answerSubmissionModels = questionAnswers.Select(kvp => new AnswerSubmissionModel
                {
                    LearnerAttemptId = learnerAttemptId,
                    QuizQuestionId = kvp.Key,
                    SelectedOptions = kvp.Value
                }).ToList();

                // Submit the answers to the repository using the SubmitAnswer endpoint
                foreach (var answerSubmissionModel in answerSubmissionModels)
                {
                    await _quizEngineRepository.SubmitAnswerAsync(answerSubmissionModel);
                }

                // Remove the cached answers from memory
                _memoryCache.Remove($"CachedAnswers_{learnerAttemptId}");
            }
            else
            {
                throw new InvalidOperationException("No cached answers found for the specified learner attempt.");
            }
        }
    }
}








using LXP.Common.ViewModels.QuizEngineViewModel;
using LXP.Core.IServices;
using LXP.Data.IRepository;

namespace LXP.Core.Services
{
    public class QuizEngineService : IQuizEngineService
    {
        private readonly IQuizEngineRepository _quizEngineRepository;

        public QuizEngineService(IQuizEngineRepository quizEngineRepository)
        {
            _quizEngineRepository = quizEngineRepository;
        }

        public async Task<ViewQuizDetailsViewModel> GetQuizByIdAsync(Guid quizId)
        {
            return await _quizEngineRepository.GetQuizByIdAsync(quizId);
        }

        public async Task<IEnumerable<QuizEngineQuestionViewModel>> GetQuestionsForQuizAsync(
            Guid quizId
        )
        {
            return await _quizEngineRepository.GetQuestionsForQuizAsync(quizId);
        }

        public async Task<ViewQuizDetailsViewModel> GetQuizDetailsByTopicIdAsync(Guid topicId)
        {
            return await _quizEngineRepository.GetQuizDetailsByTopicIdAsync(topicId);
        }

        public async Task<Guid> StartQuizAttemptAsync(Guid learnerId, Guid quizId)
        {
            var quiz = await _quizEngineRepository.GetQuizByIdAsync(quizId);
            if (quiz == null)
                throw new KeyNotFoundException($"Quiz with ID {quizId} not found.");

            var isAllowedToAttempt = await _quizEngineRepository.IsAllowedToAttemptQuizAsync(
                learnerId,
                quizId
            );
            if (!isAllowedToAttempt)
            {
                var existingAttempts = await _quizEngineRepository.GetLearnerAttemptsForQuizAsync(
                    learnerId,
                    quizId
                );
                var passMark = quiz.PassMark;
                var hasPassedQuiz = existingAttempts.Any(a => a.Score >= passMark);

                if (hasPassedQuiz)
                    throw new InvalidOperationException(
                        "You have already passed this quiz in a previous attempt."
                    );
                else
                    throw new InvalidOperationException(
                        "You have exceeded the maximum number of attempts for this quiz."
                    );
            }

            var startTime = DateTime.Now;
            var attempt = await _quizEngineRepository.CreateLearnerAttemptAsync(
                learnerId,
                quizId,
                startTime
            );
            if (attempt == null)
                throw new InvalidOperationException(
                    "You have exceeded the maximum number of attempts for this quiz."
                );

            return attempt.LearnerAttemptId;
        }

        public async Task SubmitAnswerAsync(AnswerSubmissionModel answerSubmissionModel)
        {
            var attempt = await _quizEngineRepository.GetLearnerAttemptByIdAsync(
                answerSubmissionModel.LearnerAttemptId
            );
            if (attempt == null)
                throw new KeyNotFoundException(
                    $"Learner attempt with ID {answerSubmissionModel.LearnerAttemptId} not found."
                );
            if (DateTime.Now > attempt.EndTime)
                throw new InvalidOperationException(
                    "Time limit for submitting the quiz has expired."
                );

            await _quizEngineRepository.ClearLearnerAnswersAsync(
                answerSubmissionModel.LearnerAttemptId,
                answerSubmissionModel.QuizQuestionId
            );

            var availableOptions = await _quizEngineRepository.GetQuestionOptionsAsync(
                answerSubmissionModel.QuizQuestionId
            );
            var availableOptionsIgnoreCase = availableOptions.Select(o => o.ToLower()).ToList();
            var questionType = await _quizEngineRepository.GetQuestionTypeByIdAsync(
                answerSubmissionModel.QuizQuestionId
            );

            switch (questionType)
            {
                case "MCQ":
                case "T/F":
                    if (answerSubmissionModel.SelectedOptions.Count > 1)
                        throw new InvalidOperationException(
                            "Only one option is allowed for this question type."
                        );
                    break;
                case "MSQ":
                    if (
                        answerSubmissionModel.SelectedOptions.Count < 2
                        || answerSubmissionModel.SelectedOptions.Count > 3
                    )
                        throw new InvalidOperationException(
                            "You must select between 2 and 3 options for this question type."
                        );
                    break;
            }

            foreach (var selectedOption in answerSubmissionModel.SelectedOptions)
            {
                var optionText = selectedOption.ToString();
                var optionTextLower = optionText.ToLower();

                if (!availableOptionsIgnoreCase.Contains(optionTextLower))
                {
                    throw new InvalidOperationException(
                        $"The selected option '{optionText}' is not a valid option for the given question."
                    );
                }

                var optionId = await _quizEngineRepository.GetOptionIdByTextAsync(
                    answerSubmissionModel.QuizQuestionId,
                    optionTextLower
                );
                await _quizEngineRepository.CreateLearnerAnswerAsync(
                    answerSubmissionModel.LearnerAttemptId,
                    answerSubmissionModel.QuizQuestionId,
                    optionId
                );
            }
        }

        public async Task<LearnerPassStatusViewModel> CheckLearnerPassStatusAsync(
            Guid learnerAttemptId
        )
        {
            var attempt = await _quizEngineRepository.GetLearnerAttemptByIdAsync(learnerAttemptId);
            if (attempt == null)
                throw new KeyNotFoundException(
                    $"Learner attempt with ID {learnerAttemptId} not found."
                );

            var quiz = await _quizEngineRepository.GetQuizByIdAsync(attempt.QuizId);
            if (quiz == null)
                throw new KeyNotFoundException($"Quiz with ID {attempt.QuizId} not found.");

            return new LearnerPassStatusViewModel { IsPassed = attempt.Score >= quiz.PassMark };
        }

        //public async Task SubmitQuizAttemptAsync(Guid attemptId)
        //{
        //    var attempt = await _quizEngineRepository.GetLearnerAttemptByIdAsync(attemptId);
        //    if (attempt == null)
        //        throw new KeyNotFoundException($"Learner attempt with ID {attemptId} not found.");
        //    var quiz = await _quizEngineRepository.GetQuizByIdAsync(attempt.QuizId);
        //    if (quiz == null)
        //        throw new KeyNotFoundException($"Quiz with ID {attempt.QuizId} not found.");
        //    var totalQuestions = (
        //        await _quizEngineRepository.GetQuestionsForQuizAsync(quiz.QuizId)
        //    ).Count();
        //    var existingAnswers = await _quizEngineRepository.GetLearnerAnswersForAttemptAsync(
        //        attemptId
        //    );
        //    if (existingAnswers.Select(a => a.QuizQuestionId).Distinct().Count() != totalQuestions)
        //        throw new InvalidOperationException(
        //            "You need to answer all the questions in the quiz before submitting the quiz attempt."
        //        );
        //    var individualQuestionMarks = 100.0f / totalQuestions; //newly added
        //    float finalScore = 0;
        //    foreach (var answer in existingAnswers)
        //    {
        //        var isAnswerCorrect = await _quizEngineRepository.IsQuestionOptionCorrectAsync(
        //            answer.QuizQuestionId,
        //            answer.QuestionOptionId
        //        );
        //        var questionScore = await CalculateQuestionScore(
        //            answer.QuizQuestionId,
        //            isAnswerCorrect,
        //            individualQuestionMarks,
        //            new AnswerSubmissionModel
        //            {
        //                LearnerAttemptId = attemptId,
        //                QuizQuestionId = answer.QuizQuestionId,
        //                SelectedOptions = new List<string>
        //                {
        //                    await _quizEngineRepository.GetOptionTextByIdAsync(
        //                        answer.QuestionOptionId
        //                    )
        //                }
        //            }
        //        );
        //        finalScore += questionScore;
        //    }
        //    attempt.Score = (float)Math.Round(finalScore);
        //    attempt.EndTime = DateTime.Now;
        //    await _quizEngineRepository.UpdateLearnerAttemptAsync(attempt);
        //}   28062024

        public async Task SubmitQuizAttemptAsync(Guid attemptId)
        {
            var attempt = await _quizEngineRepository.GetLearnerAttemptByIdAsync(attemptId);
            if (attempt == null)
                throw new KeyNotFoundException($"Learner attempt with ID {attemptId} not found.");
            var quiz = await _quizEngineRepository.GetQuizByIdAsync(attempt.QuizId);
            if (quiz == null)
                throw new KeyNotFoundException($"Quiz with ID {attempt.QuizId} not found.");
            var totalQuestions = (
                await _quizEngineRepository.GetQuestionsForQuizAsync(quiz.QuizId)
            ).Count();
            var existingAnswers = await _quizEngineRepository.GetLearnerAnswersForAttemptAsync(
                attemptId
            );
            if (
                existingAnswers == null
                || existingAnswers.Select(a => a.QuizQuestionId).Distinct().Count()
                    != totalQuestions
            )
                throw new InvalidOperationException(
                    "You need to answer all the questions in the quiz before submitting the quiz attempt."
                );
            var individualQuestionMarks = 100.0f / totalQuestions;
            float finalScore = 0;
            foreach (var answer in existingAnswers)
            {
                var isAnswerCorrect = await _quizEngineRepository.IsQuestionOptionCorrectAsync(
                    answer.QuizQuestionId,
                    answer.QuestionOptionId
                );
                var questionScore = await CalculateQuestionScore(
                    answer.QuizQuestionId,
                    isAnswerCorrect,
                    individualQuestionMarks,
                    new AnswerSubmissionModel
                    {
                        LearnerAttemptId = attemptId,
                        QuizQuestionId = answer.QuizQuestionId,
                        SelectedOptions = new List<string>
                        {
                            await _quizEngineRepository.GetOptionTextByIdAsync(
                                answer.QuestionOptionId
                            )
                        }
                    }
                );
                finalScore += questionScore;
            }
            attempt.Score = (float)Math.Round(finalScore, 2);
            attempt.EndTime = DateTime.Now;
            await _quizEngineRepository.UpdateLearnerAttemptAsync(attempt);
        }

        public async Task<Guid> RetakeQuizAsync(Guid learnerId, Guid quizId)
        {
            var quiz = await _quizEngineRepository.GetQuizByIdAsync(quizId);
            if (quiz == null)
                throw new KeyNotFoundException($"Quiz with ID {quizId} not found.");

            var isAllowedToAttempt = await _quizEngineRepository.IsAllowedToAttemptQuizAsync(
                learnerId,
                quizId
            );
            if (!isAllowedToAttempt)
            {
                var existingAttempts = await _quizEngineRepository.GetLearnerAttemptsForQuizAsync(
                    learnerId,
                    quizId
                );
                var passMark = quiz.PassMark;
                var hasPassedQuiz = existingAttempts.Any(a => a.Score >= passMark);

                if (hasPassedQuiz)
                    throw new InvalidOperationException(
                        "You have already passed this quiz in a previous attempt and cannot retake it."
                    );
                else
                    throw new InvalidOperationException(
                        "You have exceeded the maximum number of attempts for this quiz."
                    );
            }

            var startTime = DateTime.Now;
            var attempt = await _quizEngineRepository.CreateLearnerAttemptAsync(
                learnerId,
                quizId,
                startTime
            );
            return attempt.LearnerAttemptId;
        }

        //private async Task<float> CalculateQuestionScore(
        //    Guid quizQuestionId,
        //    bool isAnswerCorrect,
        //    float individualQuestionMarks,
        //    AnswerSubmissionModel answerSubmissionModel
        //)
        //{
        //    var questionType = await _quizEngineRepository.GetQuestionTypeByIdAsync(quizQuestionId);
        //    switch (questionType)
        //    {
        //        case "MCQ":
        //        case "T/F":
        //            return isAnswerCorrect ? individualQuestionMarks : 0;

        //        case "MSQ":
        //            var correctOptions =
        //                await _quizEngineRepository.GetCorrectOptionsForQuestionAsync(
        //                    quizQuestionId
        //                );
        //            var correctOptionCount = correctOptions.Count();
        //            var selectedOptions = answerSubmissionModel
        //                .SelectedOptions.Select(o => o.ToString())
        //                .ToList();
        //            var correctlySelectedOptions = selectedOptions
        //                .Intersect(correctOptions)
        //                .Count();
        //            var incorrectlySelectedOptions = selectedOptions.Except(correctOptions).Count();

        //            if (
        //                correctlySelectedOptions == correctOptionCount
        //                && incorrectlySelectedOptions == 0
        //            )
        //            {
        //                return individualQuestionMarks; // All correct options selected and no incorrect options
        //            }
        //            else if (correctlySelectedOptions > 0 && incorrectlySelectedOptions == 0)
        //            {
        //                var partialMark =
        //                    (individualQuestionMarks / correctOptionCount)
        //                    * correctlySelectedOptions;
        //                return partialMark; // Partial marks for partially correct answer and no incorrect options
        //            }
        //            else
        //            {
        //                return 0; // No marks for incorrect answer or selecting more options than correct options
        //            }

        //        default:
        //            return 0;
        //    }
        //}28062024

        private async Task<float> CalculateQuestionScore(
            Guid quizQuestionId,
            bool isAnswerCorrect,
            float individualQuestionMarks,
            AnswerSubmissionModel answerSubmissionModel
        )
        {
            var questionType = await _quizEngineRepository.GetQuestionTypeByIdAsync(quizQuestionId);
            switch (questionType)
            {
                case "MCQ":
                case "T/F":
                    return isAnswerCorrect ? individualQuestionMarks : 0;

                case "MSQ":
                    var correctOptions =
                        await _quizEngineRepository.GetCorrectOptionsForQuestionAsync(
                            quizQuestionId
                        );
                    var correctOptionCount = correctOptions.Count();
                    var selectedOptions = answerSubmissionModel
                        .SelectedOptions.Select(o => o.ToString())
                        .ToList();
                    var correctlySelectedOptions = selectedOptions
                        .Intersect(correctOptions)
                        .Count();
                    var incorrectlySelectedOptions = selectedOptions.Except(correctOptions).Count();

                    if (
                        correctlySelectedOptions == correctOptionCount
                        && incorrectlySelectedOptions == 0
                    )
                    {
                        return individualQuestionMarks; // All correct options selected and no incorrect options
                    }
                    else if (correctlySelectedOptions > 0 && incorrectlySelectedOptions == 0)
                    {
                        var partialMark =
                            (individualQuestionMarks / correctOptionCount)
                            * correctlySelectedOptions;
                        return partialMark; // Partial marks for partially correct answer and no incorrect options
                    }
                    else
                    {
                        return individualQuestionMarks
                            * (correctlySelectedOptions - incorrectlySelectedOptions)
                            / correctOptionCount; // Adjusted for incorrect options
                    }

                default:
                    return 0;
            }
        }

        public async Task<LearnerQuizAttemptViewModel> GetLearnerQuizAttemptAsync(Guid attemptId)
        {
            return await _quizEngineRepository.GetLearnerQuizAttemptAsync(attemptId);
        }

        public async Task<LearnerQuizAttemptResultViewModel> GetLearnerQuizAttemptResultAsync(
            Guid attemptId
        )
        {
            return await _quizEngineRepository.GetLearnerQuizAttemptResultAsync(attemptId);
        }

        public async Task<LearnerQuizStatusViewModel> GetLearnerQuizStatusAsync(
            Guid learnerId,
            Guid quizId
        )
        {
            return await _quizEngineRepository.GetLearnerQuizStatusAsync(learnerId, quizId);
        }
    }
}

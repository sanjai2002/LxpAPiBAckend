using LXP.Common.ViewModels.QuizEngineViewModel;
using LXP.Data;
using System;
using System.Collections.Generic;

public class QuizEngineService : IQuizEngineService
{
    private readonly IQuizEngineRepository _quizEngineRepository;

    public QuizEngineService(IQuizEngineRepository quizEngineRepository)
    {
        _quizEngineRepository = quizEngineRepository;
    }
    public ViewQuizDetailsViewModel GetQuizById(Guid quizId)
    {
        return _quizEngineRepository.GetQuizById(quizId);
    }

    public IEnumerable<QEQuizQuestionViewModel> GetQuestionsForQuiz(Guid quizId)
    {
        return _quizEngineRepository.GetQuestionsForQuiz(quizId);
    }
    public ViewQuizDetailsViewModel GetQuizDetailsByTopicId(Guid topicId)
    {
        return _quizEngineRepository.GetQuizDetailsByTopicId(topicId);
    }


    public Guid StartQuizAttempt(Guid learnerId, Guid quizId)
    {
        var quiz = _quizEngineRepository.GetQuizById(quizId);
        if (quiz == null)
            throw new KeyNotFoundException($"Quiz with ID {quizId} not found.");

        var isAllowedToAttempt = _quizEngineRepository.IsAllowedToAttemptQuiz(learnerId, quizId);
        if (!isAllowedToAttempt)
            throw new InvalidOperationException("You have exceeded the maximum number of attempts for this quiz.");

        var attempt = _quizEngineRepository.CreateLearnerAttempt(learnerId, quizId);
        return attempt.LearnerAttemptId;
    }

    
    public void SubmitAnswer(AnswerSubmissionModel answerSubmissionModel)
    {
        var attempt = _quizEngineRepository.GetLearnerAttemptById(answerSubmissionModel.LearnerAttemptId);
        if (attempt == null)
            throw new KeyNotFoundException($"Learner attempt with ID {answerSubmissionModel.LearnerAttemptId} not found.");

        foreach (var selectedOption in answerSubmissionModel.SelectedOptions)
        {
            var optionId = _quizEngineRepository.GetOptionIdByText(answerSubmissionModel.QuizQuestionId, selectedOption);
            var isAnswerCorrect = _quizEngineRepository.IsQuestionOptionCorrect(answerSubmissionModel.QuizQuestionId, optionId);
            var learnerAnswer = _quizEngineRepository.CreateLearnerAnswer(new AnswerSubmissionModel
            {
                LearnerAttemptId = answerSubmissionModel.LearnerAttemptId,
                QuizQuestionId = answerSubmissionModel.QuizQuestionId,
                SelectedOptions = new List<string> { selectedOption }
            });

            var questionScore = CalculateQuestionScore(answerSubmissionModel.QuizQuestionId, isAnswerCorrect);
            attempt.Score += questionScore;
        }

        _quizEngineRepository.UpdateLearnerAttempt(attempt);
    }

    public LearnerAttemptDetailsViewModel GetQuizAttemptDetails(Guid attemptId)
    {
        var attempt = _quizEngineRepository.GetLearnerAttemptDetailsById(attemptId);
        if (attempt == null)
            throw new KeyNotFoundException($"Learner attempt with ID {attemptId} not found.");

        return attempt;
    }

    public Guid RetakeQuiz(Guid learnerId, Guid quizId)
    {
        var quiz = _quizEngineRepository.GetQuizById(quizId);
        if (quiz == null)
            throw new KeyNotFoundException($"Quiz with ID {quizId} not found.");

        var isAllowedToAttempt = _quizEngineRepository.IsAllowedToAttemptQuiz(learnerId, quizId);
        if (!isAllowedToAttempt)
            throw new InvalidOperationException("You have exceeded the maximum number of attempts for this quiz.");

        var attempt = _quizEngineRepository.CreateLearnerAttempt(learnerId, quizId);
        return attempt.LearnerAttemptId;
    }

    
    private float CalculateQuestionScore(Guid quizQuestionId, bool isAnswerCorrect)
    {
        var questionType = _quizEngineRepository.GetQuestionTypeById(quizQuestionId);
        switch (questionType)
        {
            case "MCQ":
            case "T/F":
                return isAnswerCorrect ? 1 : 0;
            case "MSQ":
                var options = _quizEngineRepository.GetCorrectOptionsForQuestion(quizQuestionId);
                var correctOptionCount = options.Count();
                return isAnswerCorrect ? 1 : (1 / (float)(2 * correctOptionCount));
            default:
                return 0;
        }
    }

    
}

//private bool CheckAnswerCorrectness(List<string> selectedOptions, IEnumerable<string> correctOptions, string questionType)
//{
//    switch (questionType)
//    {
//        case "MCQ":
//        case "T/F":
//            return selectedOptions.Count == 1 && correctOptions.Contains(selectedOptions[0]);
//        case "MSQ":
//            var correctOptionsSet = new HashSet<string>(correctOptions);
//            var selectedOptionsSet = new HashSet<string>(selectedOptions);
//            return correctOptionsSet.SetEquals(selectedOptionsSet);
//        default:
//            return false;
//    }
//}

//private float CalculateQuestionScore(Guid quizQuestionId, bool isAnswerCorrect)
//{
//    var questionType = _quizEngineRepository.GetQuestionTypeById(quizQuestionId);
//    switch (questionType)
//    {
//        case "MCQ":
//        case "T/F":
//            return isAnswerCorrect ? 1 : 0;
//        case "MSQ":
//            var options = _quizEngineRepository.GetCorrectOptionsForQuestion(quizQuestionId);
//            var correctOptionCount = options.Count();
//            return isAnswerCorrect ? 1 : (1 / (float)(2 * correctOptionCount));
//        default:
//            return 0;
//    }
//}

//public void SubmitAnswer(AnswerSubmissionModel answerSubmissionModel)
//{
//    var attempt = _quizEngineRepository.GetLearnerAttemptById(answerSubmissionModel.LearnerAttemptId);
//    if (attempt == null)
//        throw new KeyNotFoundException($"Learner attempt with ID {answerSubmissionModel.LearnerAttemptId} not found.");

//    var questionType = _quizEngineRepository.GetQuestionTypeById(answerSubmissionModel.QuizQuestionId);
//    var correctOptions = _quizEngineRepository.GetCorrectOptionsForQuestion(answerSubmissionModel.QuizQuestionId);
//    var isAnswerCorrect = CheckAnswerCorrectness(answerSubmissionModel.SelectedOptions, correctOptions, questionType);

//    var learnerAnswers = answerSubmissionModel.SelectedOptions.Select(option => new LearnerAnswer
//    {
//        LearnerAttemptId = answerSubmissionModel.LearnerAttemptId,
//        QuizQuestionId = answerSubmissionModel.QuizQuestionId,
//        QuestionOptionId = _quizEngineRepository.GetQuestionOptionIdByOption(answerSubmissionModel.QuizQuestionId, option),
//        CreatedBy = "System"
//    }).ToList();

//    _quizEngineRepository.CreateLearnerAnswers(learnerAnswers);

//    var questionScore = CalculateQuestionScore(answerSubmissionModel.QuizQuestionId, isAnswerCorrect);
//    attempt.Score += questionScore;

//    _quizEngineRepository.UpdateLearnerAttempt(attempt);
//}

//public void SubmitAnswer(AnswerSubmissionModel answerSubmissionModel)
//{
//    var attempt = _quizEngineRepository.GetLearnerAttemptById(answerSubmissionModel.LearnerAttemptId);
//    if (attempt == null)
//        throw new KeyNotFoundException($"Learner attempt with ID {answerSubmissionModel.LearnerAttemptId} not found.");

//    var isAnswerCorrect = _quizEngineRepository.IsQuestionOptionCorrect(answerSubmissionModel.QuizQuestionId, answerSubmissionModel.QuestionOptionId);
//    var learnerAnswer = _quizEngineRepository.CreateLearnerAnswer(answerSubmissionModel);

//    var questionScore = CalculateQuestionScore(answerSubmissionModel.QuizQuestionId, isAnswerCorrect);
//    attempt.Score += questionScore;

//    _quizEngineRepository.UpdateLearnerAttempt(attempt);
//}
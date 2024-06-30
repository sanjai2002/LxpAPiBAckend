using System.Transactions;
using LXP.Common.Constants;
using LXP.Common.Entities;
using LXP.Common.ViewModels.QuizQuestionViewModel;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace LXP.Core.Services
{
    public class ExcelToJsonService : IExcelToJsonService
    {
        private readonly IQuizQuestionJsonRepository _quizQuestionRepository;

        public ExcelToJsonService(IQuizQuestionJsonRepository quizQuestionRepository)
        {
            _quizQuestionRepository = quizQuestionRepository;
        }

        public async Task<List<QuizQuestionJsonViewModel>> ConvertExcelToJsonAsync(IFormFile file)
        {
            var quizQuestions = new List<QuizQuestionJsonViewModel>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                        throw new ArgumentException("Worksheet not found.");

                    for (int row = 3; row <= worksheet.Dimension.End.Row; row++)
                    {
                        var questionType = worksheet.Cells[row, 2].Value?.ToString();
                        var question = worksheet.Cells[row, 3].Value?.ToString();
                        question = question.Replace("\n", " ").Replace("\r", "");//added this to remove unwanted lines in questions 
                        if (string.IsNullOrEmpty(questionType) || string.IsNullOrEmpty(question))
                            continue;

                        var quizQuestion = new QuizQuestionJsonViewModel
                        {
                            QuestionNumber = row - 2,
                            QuestionType = questionType,
                            Question = question,
                            Options = ExtractOptions(worksheet, row, 4, 8, questionType),
                            CorrectOptions = ExtractOptions(worksheet, row, 12, 3, questionType)
                        };

                        quizQuestions.Add(quizQuestion);
                    }
                }
            }

            return quizQuestions;
        }

        public List<QuizQuestionJsonViewModel> ValidateQuizData(
            List<QuizQuestionJsonViewModel> quizData
        )
        {
            var validQuizData = new List<QuizQuestionJsonViewModel>();

            foreach (var question in quizData)
            {
                if (question.QuestionType == QuizQuestionTypes.MultiChoiceQuestion)
                {
                    if (
                        question.Options.Length != 4
                        || question.Options.Distinct().Count() != 4
                        || question.CorrectOptions.Length != 1
                        || !question.Options.Contains(question.CorrectOptions.First())
                    )
                    {
                        continue;
                    }
                }
                else if (question.QuestionType == QuizQuestionTypes.TrueFalseQuestion)
                {
                    if (
                        question.Options.Length != 2
                        || !(
                            question.Options.Contains("True", StringComparer.OrdinalIgnoreCase)
                            || question.Options.Contains("1")
                        )
                        || !(
                            question.Options.Contains("False", StringComparer.OrdinalIgnoreCase)
                            || question.Options.Contains("0")
                        )
                        || question.CorrectOptions.Length != 1
                        || (
                            question.CorrectOptions.First().ToLower() != "true"
                            && question.CorrectOptions.First().ToLower() != "false"
                            && question.CorrectOptions.First() != "1"
                            && question.CorrectOptions.First() != "0"
                        )
                    )
                    {
                        continue;
                    }
                }
                else if (question.QuestionType == QuizQuestionTypes.MultiSelectQuestion)
                {
                    if (
                        question.Options.Length < 5
                        || question.Options.Length > 8
                        || question.Options.Distinct().Count() != question.Options.Length
                        || question.CorrectOptions.Length < 2
                        || question.CorrectOptions.Length > 3
                        || !question.CorrectOptions.All(co => question.Options.Contains(co))
                    )
                    {
                        continue;
                    }
                }
                validQuizData.Add(question);
            }

            return validQuizData;
        }

        public async Task SaveQuizDataAsync(
            List<QuizQuestionJsonViewModel> quizQuestions,
            Guid quizId
        )
        {
            foreach (var quizQuestion in quizQuestions)
            {
                using (
                    var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)
                )
                {
                    int nextQuestionNo = await _quizQuestionRepository.GetNextQuestionNoAsync(
                        quizId
                    );

                    var questionEntity = new QuizQuestion
                    {
                        QuizId = quizId,
                        QuestionNo = nextQuestionNo,
                        QuestionType = quizQuestion.QuestionType,
                        Question = quizQuestion.Question,
                        CreatedBy = "Admin",
                        CreatedAt = DateTime.Now
                    };

                    await _quizQuestionRepository.AddQuestionsAsync(
                        new List<QuizQuestion> { questionEntity }
                    );

                    var optionEntities = quizQuestion
                        .Options.Select(
                            (option, index) =>
                                new QuestionOption
                                {
                                    QuizQuestionId = questionEntity.QuizQuestionId,
                                    Option = option,
                                    IsCorrect = quizQuestion.CorrectOptions.Contains(option),
                                    CreatedAt = DateTime.Now,
                                    CreatedBy = "Admin",
                                    ModifiedBy = "Admin"
                                }
                        )
                        .ToList();

                    await _quizQuestionRepository.AddOptionsAsync(
                        optionEntities,
                        questionEntity.QuizQuestionId
                    );

                    transaction.Complete();
                }
            }
        }

        private string[] ExtractOptions(
            ExcelWorksheet worksheet,
            int row,
            int startColumn,
            int count,
            string questionType
        )
        {
            var options = new List<string>();
            for (int i = 0; i < count; i++)
            {
                var option = worksheet.Cells[row, startColumn + i].Value?.ToString();
                if (!string.IsNullOrEmpty(option))
                {
                    if (questionType == QuizQuestionTypes.TrueFalseQuestion)
                    {
                        if (option == "1")
                            option = "True";
                        else if (option == "0")
                            option = "False";
                    }
                    options.Add(option);
                }
            }
            return options.ToArray();
        }
    }
}



/*
 *
 * //var quizQuestion = new QuizQuestionJsonViewModel
                        //{
                        //    QuestionNumber = row - 2,
                        //    QuestionType = questionType,
                        //    Question = question,
                        //    Options = ExtractOptions(worksheet, row, 4, 8),
                        //    CorrectOptions = ExtractOptions(worksheet, row, 12, 3)
                        //};
 * //else if (question.QuestionType == QuizQuestionTypes.TrueFalseQuestion)
                //{
                //    if (
                //        question.Options.Length != 2
                //        || !question.Options.Contains("True", StringComparer.OrdinalIgnoreCase)
                //        || !question.Options.Contains("False", StringComparer.OrdinalIgnoreCase)
                //        || question.CorrectOptions.Length != 1
                //        || (
                //            question.CorrectOptions.First().ToLower() != "true"
                //            && question.CorrectOptions.First().ToLower() != "false"
                //        )
                //    )
                //    {
                //        continue;
                //    }
                //}
 *
 *
 * private string[] ExtractOptions(
            ExcelWorksheet worksheet,
            int row,
            int startColumn,
            int count
        )
        {
            var options = new List<string>();
            for (int i = 0; i < count; i++)
            {
                var option = worksheet.Cells[row, startColumn + i].Value?.ToString();
                if (!string.IsNullOrEmpty(option))
                    options.Add(option);
            }
            return options.ToArray();
        }
 *
 *
 *
 * private string[] ExtractOptions(
    ExcelWorksheet worksheet,
    int row,
    int startColumn,
    int count,
    string questionType
)
{
    var options = new List<string>();
    for (int i = 0; i < count; i++)
    {
        var option = worksheet.Cells[row, startColumn + i].Value?.ToString();
        if (!string.IsNullOrEmpty(option))
        {
            if (questionType == QuizQuestionTypes.TrueFalseQuestion)
            {
                if (option == "1") option = "True";
                else if (option == "0") option = "False";
            }
            options.Add(option);
        }
    }
    return options.ToArray();
}
*/



//using System.Transactions;
//using LXP.Common.Entities;
//using LXP.Common.ViewModels.QuizQuestionViewModel;
//using LXP.Core.IServices;
//using LXP.Data.IRepository;
//using Microsoft.AspNetCore.Http;
//using OfficeOpenXml;

//namespace LXP.Core.Services
//{
//    public class ExcelToJsonService : IExcelToJsonService
//    {
//        private readonly IQuizQuestionJsonRepository _quizQuestionRepository;

//        public ExcelToJsonService(IQuizQuestionJsonRepository quizQuestionRepository)
//        {
//            _quizQuestionRepository = quizQuestionRepository;
//        }

//        public async Task<List<QuizQuestionJsonViewModel>> ConvertExcelToJsonAsync(IFormFile file)
//        {
//            var quizQuestions = new List<QuizQuestionJsonViewModel>();

//            using (var stream = new MemoryStream())
//            {
//                await file.CopyToAsync(stream);
//                stream.Position = 0;

//                using (var package = new ExcelPackage(stream))
//                {
//                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
//                    if (worksheet == null)
//                        throw new ArgumentException("Worksheet not found.");

//                    for (int row = 3; row <= worksheet.Dimension.End.Row; row++)
//                    {
//                        var questionType = worksheet.Cells[row, 2].Value?.ToString();
//                        var question = worksheet.Cells[row, 3].Value?.ToString();
//                        if (string.IsNullOrEmpty(questionType) || string.IsNullOrEmpty(question))
//                            continue;

//                        var quizQuestion = new QuizQuestionJsonViewModel
//                        {
//                            QuestionNumber = row - 2,
//                            QuestionType = questionType,
//                            Question = question,
//                            Options = ExtractOptions(worksheet, row, 4, 8),
//                            CorrectOptions = ExtractOptions(worksheet, row, 12, 3)
//                        };

//                        quizQuestions.Add(quizQuestion);
//                    }
//                }
//            }

//            return quizQuestions;
//        }

//        public List<QuizQuestionJsonViewModel> ValidateQuizData(
//            List<QuizQuestionJsonViewModel> quizData
//        )
//        {
//            var validQuizData = new List<QuizQuestionJsonViewModel>();

//            foreach (var question in quizData)
//            {
//                if (question.QuestionType == "MCQ")
//                {
//                    if (
//                        question.Options.Length != 4
//                        || question.Options.Distinct().Count() != 4
//                        || question.CorrectOptions.Length != 1
//                        || !question.Options.Contains(question.CorrectOptions.First())
//                    )
//                    {
//                        continue;
//                    }
//                }
//                else if (question.QuestionType == "T/F")
//                //else if (question.QuestionType == "TF")
//                {
//                    if (
//                        question.Options.Length != 2
//                        || !question.Options.Contains("True", StringComparer.OrdinalIgnoreCase)
//                        || !question.Options.Contains("False", StringComparer.OrdinalIgnoreCase)
//                        || question.CorrectOptions.Length != 1
//                        || (
//                            question.CorrectOptions.First().ToLower() != "true"
//                            && question.CorrectOptions.First().ToLower() != "false"
//                        )
//                    )
//                    {
//                        continue;
//                    }
//                }
//                else if (question.QuestionType == "MSQ")
//                {
//                    if (
//                        question.Options.Length < 5
//                        || question.Options.Length > 8
//                        || question.Options.Distinct().Count() != question.Options.Length
//                        || question.CorrectOptions.Length < 2
//                        || question.CorrectOptions.Length > 3
//                        || !question.CorrectOptions.All(co => question.Options.Contains(co))
//                    )
//                    {
//                        continue;
//                    }
//                }
//                validQuizData.Add(question);
//            }

//            return validQuizData;
//        }

//        public async Task SaveQuizDataAsync(
//            List<QuizQuestionJsonViewModel> quizQuestions,
//            Guid quizId
//        )
//        {
//            foreach (var quizQuestion in quizQuestions)
//            {
//                using (
//                    var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)
//                )
//                {
//                    int nextQuestionNo = await _quizQuestionRepository.GetNextQuestionNoAsync(
//                        quizId
//                    );

//                    var questionEntity = new QuizQuestion
//                    {
//                        QuizId = quizId,
//                        QuestionNo = nextQuestionNo,
//                        QuestionType = quizQuestion.QuestionType,
//                        Question = quizQuestion.Question,
//                        CreatedBy = "Admin",
//                        CreatedAt = DateTime.Now
//                    };

//                    await _quizQuestionRepository.AddQuestionsAsync(
//                        new List<QuizQuestion> { questionEntity }
//                    );

//                    var optionEntities = quizQuestion
//                        .Options.Select(
//                            (option, index) =>
//                                new QuestionOption
//                                {
//                                    QuizQuestionId = questionEntity.QuizQuestionId,
//                                    Option = option,
//                                    IsCorrect = quizQuestion.CorrectOptions.Contains(option),
//                                    CreatedAt = DateTime.Now,
//                                    CreatedBy = "Admin",
//                                    ModifiedBy = "Admin"
//                                }
//                        )
//                        .ToList();

//                    await _quizQuestionRepository.AddOptionsAsync(
//                        optionEntities,
//                        questionEntity.QuizQuestionId
//                    );

//                    transaction.Complete();
//                }
//            }
//        }

//        private string[] ExtractOptions(
//            ExcelWorksheet worksheet,
//            int row,
//            int startColumn,
//            int count
//        )
//        {
//            var options = new List<string>();
//            for (int i = 0; i < count; i++)
//            {
//                var option = worksheet.Cells[row, startColumn + i].Value?.ToString();
//                if (!string.IsNullOrEmpty(option))
//                    options.Add(option);
//            }
//            return options.ToArray();
//        }
//    }
//}

using FluentValidation;

using OfficeOpenXml;
using LXP.Data.IRepository;

using LXP.Core.IServices;
using LXP.Data;
using Microsoft.AspNetCore.Http;
using LXP.Common.Validators;
using LXP.Common.ViewModels.QuizQuestionViewModel;
using LXP.Common.Entities;

namespace LXP.Core.Services
{
    public class BulkQuestionService : IBulkQuestionService
    {
        private readonly IBulkQuestionRepository _bulkQuestionRepository;
        private readonly IQuizQuestionRepository _quizQuestionRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly BulkQuizQuestionViewModelValidator _validator;

        public BulkQuestionService(IBulkQuestionRepository bulkQuestionRepository,
                                   IQuizQuestionRepository quizQuestionRepository,
                                   IQuizRepository quizRepository,
                                   BulkQuizQuestionViewModelValidator validator)
        {
            _bulkQuestionRepository = bulkQuestionRepository;
            _quizQuestionRepository = quizQuestionRepository;
            _quizRepository = quizRepository;
            _validator = validator;
        }

        public async Task<object> ImportQuizDataAsync(IFormFile file, Guid quizId)
        {
            if (file == null || file.Length <= 0)
                throw new ArgumentException("File is empty.");

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                        throw new ArgumentException("Worksheet not found.");

                    List<BulkQuizQuestionViewModel> quizQuestions = new List<BulkQuizQuestionViewModel>();

                    // Loop through each row in the worksheet
                    for (int row = 3; row <= worksheet.Dimension.End.Row; row++)
                    {
                        string type = worksheet.Cells[row, 2].Value?.ToString();

                        if (type == "MCQ" || type == "TF" || type == "MSQ")
                        {
                            BulkQuizQuestionViewModel quizQuestion = new BulkQuizQuestionViewModel
                            {
                                QuestionType = type,
                                Question = worksheet.Cells[row, 3].Value.ToString(),
                            };

                            // Validate question type
                            if (!ValidateQuestionType(quizQuestion))
                                continue;

                            // Extract options based on question type
                            if (type == "MCQ")
                            {
                                quizQuestion.Options = ExtractOptions(worksheet, row, 4, 4);
                                quizQuestion.CorrectOptions = ExtractOptions(worksheet, row, 12, 1);
                                // Validate MCQ options
                                if (!ValidateMCQOptions(quizQuestion))
                                    continue;
                            }
                            else if (type == "TF")
                            {
                                quizQuestion.Options = ExtractOptions(worksheet, row, 4, 2);
                                quizQuestion.CorrectOptions = ExtractOptions(worksheet, row, 12, 1);
                                // Validate T/F options
                                if (!ValidateTFOptions(quizQuestion))
                                    continue;
                            }
                            else if (type == "MSQ")
                            {
                                int optionCount = GetMSQOptionCount(worksheet, row);
                                quizQuestion.Options = ExtractOptions(worksheet, row, 4, optionCount);
                                quizQuestion.CorrectOptions = ExtractOptions(worksheet, row, 12, GetMSQCorrectOptionCount(optionCount));
                                // Validate MSQ options
                                if (!ValidateMSQOptions(quizQuestion))
                                    continue;
                            }

                            quizQuestions.Add(quizQuestion);
                        }
                    }

                    // Loop through each question and add to repository
                    foreach (var quizQuestion in quizQuestions)
                    {
                        // Validate using FluentValidation
                        var validationResult = _validator.Validate(quizQuestion);
                        if (!validationResult.IsValid)
                            throw new ArgumentException(string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage)));

                        // Get the next available question number
                        int nextQuestionNo = await _quizQuestionRepository.GetNextQuestionNoAsync(quizId);

                        // Add question to the repository
                        QuizQuestion questionEntity = new QuizQuestion
                        {
                            QuizId = quizId,
                            QuestionNo = nextQuestionNo,
                            QuestionType = quizQuestion.QuestionType,
                            Question = quizQuestion.Question,
                            CreatedBy = "Admin",
                            CreatedAt = DateTime.UtcNow
                        };

                        // Save the question to get the QuizQuestionId
                        await _bulkQuestionRepository.AddQuestionsAsync(new List<QuizQuestion> { questionEntity });

                        // Add options associated with the question
                        List<QuestionOption> optionEntities = new List<QuestionOption>();
                        for (int i = 0; i < quizQuestion.Options.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(quizQuestion.Options[i]))
                            {
                                QuestionOption optionEntity = new QuestionOption
                                {
                                    QuizQuestionId = questionEntity.QuizQuestionId,
                                    Option = quizQuestion.Options[i],
                                    IsCorrect = quizQuestion.CorrectOptions.Contains(quizQuestion.Options[i]),
                                    CreatedAt = DateTime.UtcNow,
                                    CreatedBy = "Admin",
                                    ModifiedBy = "Admin"
                                };

                                optionEntities.Add(optionEntity);
                            }
                        }

                        await _bulkQuestionRepository.AddOptionsAsync(optionEntities, questionEntity.QuizQuestionId);
                    }
                    return quizQuestions;
                }
            }
        }

        // Validate question type
        private bool ValidateQuestionType(BulkQuizQuestionViewModel quizQuestion)
        {
            return quizQuestion.QuestionType == "MCQ" || quizQuestion.QuestionType == "TF" || quizQuestion.QuestionType == "MSQ";
        }

        // Validate T/F options
        private bool ValidateTFOptions(BulkQuizQuestionViewModel quizQuestion)
        {
            return quizQuestion.Options.Length == 2 &&
                   !string.IsNullOrEmpty(quizQuestion.Options[0]) &&
                   !string.IsNullOrEmpty(quizQuestion.Options[1]) &&
                   !quizQuestion.Options[0].Equals(quizQuestion.Options[1], StringComparison.OrdinalIgnoreCase) &&
                   (quizQuestion.CorrectOptions.Length == 1 && (quizQuestion.CorrectOptions[0].Equals("true", StringComparison.OrdinalIgnoreCase) || quizQuestion.CorrectOptions[0].Equals("false", StringComparison.OrdinalIgnoreCase)));
        }

        // Validate MCQ options
        private bool ValidateMCQOptions(BulkQuizQuestionViewModel quizQuestion)
        {
            return quizQuestion.Options.Length == 4 &&
                   quizQuestion.Options.Distinct().Count() == 4 &&
                   quizQuestion.CorrectOptions.Length == 1 &&
                   quizQuestion.Options.Contains(quizQuestion.CorrectOptions[0]);
        }

        // Validate MSQ options
        private bool ValidateMSQOptions(BulkQuizQuestionViewModel quizQuestion)
        {
            int optionCount = quizQuestion.Options.Length;
            int correctOptionCount = quizQuestion.CorrectOptions.Length;

            return (optionCount >= 4 && optionCount <= 11) &&
                   quizQuestion.Options.Distinct().Count() == optionCount &&
                   (correctOptionCount >= 2 && correctOptionCount <= 3) &&
                   quizQuestion.CorrectOptions.All(opt => quizQuestion.Options.Contains(opt));
        }

        // Get the count of MSQ options based on the filled rows
        private int GetMSQOptionCount(ExcelWorksheet worksheet, int row)
        {
            int count = 0;
            for (int i = 4; i <= 11; i++)
            {
                if (!string.IsNullOrEmpty(worksheet.Cells[row, i].Value?.ToString()))
                    count++;
                else
                    break;
            }
            return count;
        }

        // Get the count of MSQ correct options based on the total option count
        private int GetMSQCorrectOptionCount(int optionCount)
        {
            if (optionCount >= 5 && optionCount <= 8)
                return 2;
            else if (optionCount >= 9 && optionCount <= 11)
                return 3;
            else
                return 0;
        }

        // Extract options from Excel worksheet
        private string[] ExtractOptions(ExcelWorksheet worksheet, int row, int startColumn, int count)
        {
            string[] options = new string[count];
            for (int i = 0; i < count; i++)
            {
                string option = worksheet.Cells[row, startColumn + i].Value?.ToString() ?? "";
                options[i] = option;
            }
            return options;
        }
    }
}


//using LXP.Common.ViewModels;
//using LXP.Core.IServices;
//using Microsoft.AspNetCore.Http;
//using OfficeOpenXml;
//using LXP.Data.IRepository;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using LXP.Data;

//namespace LXP.Core.Services
//{
//    public class BulkQuestionService : IBulkQuestionService
//    {
//        private readonly IBulkQuestionRepository _bulkQuestionRepository;
//        private readonly IQuizQuestionRepository _quizQuestionRepository;
//        private readonly IQuizRepository _quizRepository;

//        public BulkQuestionService(IBulkQuestionRepository bulkQuestionRepository,
//                                   IQuizQuestionRepository quizQuestionRepository,
//                                   IQuizRepository quizRepository)
//        {
//            _bulkQuestionRepository = bulkQuestionRepository;
//            _quizQuestionRepository = quizQuestionRepository;
//            _quizRepository = quizRepository;
//        }

//        public async Task<object> ImportQuizDataAsync(IFormFile file, Guid quizId)
//        {
//            try
//            {
//                if (file == null || file.Length <= 0)
//                    throw new ArgumentException("File is empty.");

//                using (var stream = new MemoryStream())
//                {
//                    await file.CopyToAsync(stream);
//                    stream.Position = 0;

//                    using (ExcelPackage package = new ExcelPackage(stream))
//                    {
//                        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
//                        if (worksheet == null)
//                            throw new ArgumentException("Worksheet not found.");

//                        List<BulkQuizQuestionViewModel> quizQuestions = new List<BulkQuizQuestionViewModel>();

//                        // Loop through each row in the worksheet
//                        for (int row = 3; row <= worksheet.Dimension.End.Row; row++)
//                        {
//                            string type = worksheet.Cells[row, 2].Value?.ToString();

//                            if (type == "MCQ" || type == "TF" || type == "MSQ")
//                            {
//                                BulkQuizQuestionViewModel quizQuestion = new BulkQuizQuestionViewModel
//                                {
//                                    QuestionType = type,
//                                    Question = worksheet.Cells[row, 3].Value.ToString(),
//                                };

//                                // Validate question type
//                                if (!ValidateQuestionType(quizQuestion))
//                                    continue;

//                                // Extract options based on question type
//                                if (type == "MCQ")
//                                {
//                                    quizQuestion.Options = ExtractOptions(worksheet, row, 4, 4);
//                                    quizQuestion.CorrectOptions = ExtractOptions(worksheet, row, 12, 1);
//                                    // Validate MCQ options
//                                    if (!ValidateMCQOptions(quizQuestion))
//                                        continue;
//                                }
//                                else if (type == "TF")
//                                {
//                                    quizQuestion.Options = ExtractOptions(worksheet, row, 4, 2);
//                                    quizQuestion.CorrectOptions = ExtractOptions(worksheet, row, 12, 1);
//                                    // Validate T/F options
//                                    if (!ValidateTFOptions(quizQuestion))
//                                        continue;
//                                }
//                                else if (type == "MSQ")
//                                {
//                                    int optionCount = GetMSQOptionCount(worksheet, row);
//                                    quizQuestion.Options = ExtractOptions(worksheet, row, 4, optionCount);
//                                    quizQuestion.CorrectOptions = ExtractOptions(worksheet, row, 12, GetMSQCorrectOptionCount(optionCount));
//                                    // Validate MSQ options
//                                    if (!ValidateMSQOptions(quizQuestion))
//                                        continue;
//                                }

//                                quizQuestions.Add(quizQuestion);
//                            }
//                        }

//                        // Loop through each question and add to repository
//                        foreach (var quizQuestion in quizQuestions)
//                        {
//                            // Get the next available question number
//                            int nextQuestionNo = await _quizQuestionRepository.GetNextQuestionNoAsync(quizId);

//                            // Add question to the repository
//                            QuizQuestion questionEntity = new QuizQuestion
//                            {
//                                QuizId = quizId,
//                                QuestionNo = nextQuestionNo,
//                                QuestionType = quizQuestion.QuestionType,
//                                Question = quizQuestion.Question,
//                                CreatedBy = "Admin",
//                                CreatedAt = DateTime.UtcNow
//                            };

//                            // Save the question to get the QuizQuestionId
//                            await _bulkQuestionRepository.AddQuestionsAsync(new List<QuizQuestion> { questionEntity });

//                            // Add options associated with the question
//                            List<QuestionOption> optionEntities = new List<QuestionOption>();
//                            for (int i = 0; i < quizQuestion.Options.Length; i++)
//                            {
//                                if (!string.IsNullOrEmpty(quizQuestion.Options[i]))
//                                {
//                                    QuestionOption optionEntity = new QuestionOption
//                                    {
//                                        QuizQuestionId = questionEntity.QuizQuestionId,
//                                        Option = quizQuestion.Options[i],
//                                        IsCorrect = quizQuestion.CorrectOptions.Contains(quizQuestion.Options[i]),
//                                        CreatedAt = DateTime.UtcNow,
//                                        CreatedBy = "Admin",
//                                        ModifiedBy = "Admin"
//                                    };

//                                    optionEntities.Add(optionEntity);
//                                }
//                            }

//                            await _bulkQuestionRepository.AddOptionsAsync(optionEntities, questionEntity.QuizQuestionId);
//                        }
//                        return quizQuestions;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"An error occurred while importing quiz data: {ex.Message}", ex);
//            }

//        }

//        // Validate question type
//        private bool ValidateQuestionType(BulkQuizQuestionViewModel quizQuestion)
//        {
//            return quizQuestion.QuestionType == "MCQ" || quizQuestion.QuestionType == "TF" || quizQuestion.QuestionType == "MSQ";
//        }

//        // Validate T/F options
//        private bool ValidateTFOptions(BulkQuizQuestionViewModel quizQuestion)
//        {
//            return quizQuestion.Options.Length == 2 &&
//                   !string.IsNullOrEmpty(quizQuestion.Options[0]) &&
//                   !string.IsNullOrEmpty(quizQuestion.Options[1]) &&
//                   !quizQuestion.Options[0].Equals(quizQuestion.Options[1], StringComparison.OrdinalIgnoreCase) &&
//                   (quizQuestion.CorrectOptions.Length == 1 && (quizQuestion.CorrectOptions[0].Equals("true", StringComparison.OrdinalIgnoreCase) || quizQuestion.CorrectOptions[0].Equals("false", StringComparison.OrdinalIgnoreCase)));
//        }

//        // Validate MCQ options
//        private bool ValidateMCQOptions(BulkQuizQuestionViewModel quizQuestion)
//        {
//            return quizQuestion.Options.Length == 4 &&
//                   quizQuestion.Options.Distinct().Count() == 4 &&
//                   quizQuestion.CorrectOptions.Length == 1 &&
//                   quizQuestion.Options.Contains(quizQuestion.CorrectOptions[0]);
//        }

//        // Validate MSQ options
//        private bool ValidateMSQOptions(BulkQuizQuestionViewModel quizQuestion)
//        {
//            int optionCount = quizQuestion.Options.Length;
//            int correctOptionCount = quizQuestion.CorrectOptions.Length;

//            return (optionCount >= 4 && optionCount <= 11) &&
//                   quizQuestion.Options.Distinct().Count() == optionCount &&
//                   (correctOptionCount >= 2 && correctOptionCount <= 3) &&
//                   quizQuestion.CorrectOptions.All(opt => quizQuestion.Options.Contains(opt));
//        }

//        // Get the count of MSQ options based on the filled rows
//        private int GetMSQOptionCount(ExcelWorksheet worksheet, int row)
//        {
//            int count = 0;
//            for (int i = 4; i <= 11; i++)
//            {
//                if (!string.IsNullOrEmpty(worksheet.Cells[row, i].Value?.ToString()))
//                    count++;
//                else
//                    break;
//            }
//            return count;
//        }

//        // Get the count of MSQ correct options based on the total option count
//        private int GetMSQCorrectOptionCount(int optionCount)
//        {
//            if (optionCount >= 5 && optionCount <= 8)
//                return 2;
//            else if (optionCount >= 9 && optionCount <= 11)
//                return 3;
//            else
//                return 0;
//        }

//        // Extract options from Excel worksheet
//        private string[] ExtractOptions(ExcelWorksheet worksheet, int row, int startColumn, int count)
//        {
//            string[] options = new string[count];
//            for (int i = 0; i < count; i++)
//            {
//                string option = worksheet.Cells[row, startColumn + i].Value?.ToString() ?? "";
//                options[i] = option;
//            }
//            return options;
//        }
//    }
//}



//using LXP.Common.ViewModels;
//using LXP.Core.IServices;
//using Microsoft.AspNetCore.Http;
//using OfficeOpenXml;
//using LXP.Data.IRepository;
//using LXP.Data;


//namespace LXP.Core.Services
//{
//    public class BulkQuestionService : IBulkQuestionService
//    {
//        private readonly IBulkQuestionRepository _bulkQuestionRepository;

//        public BulkQuestionService(IBulkQuestionRepository bulkQuestionRepository)
//        {
//            _bulkQuestionRepository = bulkQuestionRepository;
//        }

//        public object ImportQuizData(IFormFile file)
//        {
//            try
//            {
//                if (file == null || file.Length <= 0)
//                    throw new ArgumentException("File is empty.");

//                using (var stream = new MemoryStream())
//                {
//                    file.CopyTo(stream);
//                    stream.Position = 0;

//                    using (ExcelPackage package = new ExcelPackage(stream))
//                    {
//                        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
//                        if (worksheet == null)
//                            throw new ArgumentException("Worksheet not found.");

//                        List<QuizQuestionViewModel> quizQuestions = new List<QuizQuestionViewModel>();

//                        // Loop through each row in the worksheet
//                        for (int row = 3; row <= worksheet.Dimension.End.Row; row++)
//                        {
//                            string type = worksheet.Cells[row, 2].Value?.ToString();

//                            if (type == "MCQ" || type == "TF" || type == "MSQ")
//                            {
//                                QuizQuestionViewModel quizQuestion = new QuizQuestionViewModel
//                                {
//                                    QuestionType = type,
//                                    QuestionNumber = Convert.ToInt32(worksheet.Cells[row, 1].Value),
//                                    Question = worksheet.Cells[row, 3].Value.ToString(),
//                                };

//                                // Validate question type
//                                if (!ValidateQuestionType(quizQuestion))
//                                    continue;

//                                // Extract options based on question type
//                                if (type == "MCQ")
//                                {
//                                    quizQuestion.Options = ExtractOptions(worksheet, row, 4, 4);
//                                    quizQuestion.CorrectOptions = ExtractOptions(worksheet, row, 12, 1);
//                                    // Validate MCQ options
//                                    if (!ValidateMCQOptions(quizQuestion))
//                                        continue;
//                                }
//                                else if (type == "TF")
//                                {
//                                    quizQuestion.Options = ExtractOptions(worksheet, row, 4, 2);
//                                    quizQuestion.CorrectOptions = ExtractOptions(worksheet, row, 12, 1);
//                                    // Validate T/F options
//                                    if (!ValidateTFOptions(quizQuestion))
//                                        continue;
//                                }
//                                else if (type == "MSQ")
//                                {
//                                    int optionCount = GetMSQOptionCount(worksheet, row);
//                                    quizQuestion.Options = ExtractOptions(worksheet, row, 4, optionCount);
//                                    quizQuestion.CorrectOptions = ExtractOptions(worksheet, row, 12, GetMSQCorrectOptionCount(optionCount));
//                                    // Validate MSQ options
//                                    if (!ValidateMSQOptions(quizQuestion))
//                                        continue;
//                                }

//                                quizQuestions.Add(quizQuestion);
//                            }
//                        }

//                        // Loop through each question and add to repository
//                        foreach (var quizQuestion in quizQuestions)
//                        {
//                            // Add question to the repository
//                            QuizQuestion questionEntity = new QuizQuestion
//                            {

//                                QuizId = Guid.Parse("98984911-1862-4745-92ba-570bff6bcf05"),
//                                QuestionNo = quizQuestion.QuestionNumber,
//                                QuestionType = quizQuestion.QuestionType,
//                                Question = quizQuestion.Question,
//                                CreatedBy = "Admin",
//                                CreatedAt = DateTime.UtcNow
//                            };

//                            // Save the question to get the QuizQuestionId
//                            _bulkQuestionRepository.AddQuestions(new List<QuizQuestion> { questionEntity });

//                            // Add options associated with the question
//                            List<QuestionOption> optionEntities = new List<QuestionOption>();
//                            for (int i = 0; i < quizQuestion.Options.Length; i++)
//                            {
//                                if (!string.IsNullOrEmpty(quizQuestion.Options[i]))
//                                {
//                                    QuestionOption optionEntity = new QuestionOption
//                                    {
//                                        QuizQuestionId = questionEntity.QuizQuestionId,
//                                        Option = quizQuestion.Options[i],
//                                        IsCorrect = quizQuestion.CorrectOptions.Contains(quizQuestion.Options[i]),
//                                        CreatedAt = DateTime.UtcNow,
//                                        CreatedBy = "Admin",
//                                        ModifiedBy = "Admin2"
//                                    };

//                                    optionEntities.Add(optionEntity);
//                                }
//                            }


//                            _bulkQuestionRepository.AddOptions(optionEntities, questionEntity.QuizQuestionId);
//                        }
//                        return quizQuestions;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"An error occurred while importing quiz data: {ex.Message}", ex);
//            }
//        }

//        // Validate question type
//        private bool ValidateQuestionType(QuizQuestionViewModel quizQuestion)
//        {
//            return quizQuestion.QuestionType == "MCQ" || quizQuestion.QuestionType == "TF" || quizQuestion.QuestionType == "MSQ";
//        }

//        // Validate T/F options
//        private bool ValidateTFOptions(QuizQuestionViewModel quizQuestion)
//        {
//            return quizQuestion.Options.Length == 2 &&
//                   !string.IsNullOrEmpty(quizQuestion.Options[0]) &&
//                   !string.IsNullOrEmpty(quizQuestion.Options[1]) &&
//                   !quizQuestion.Options[0].Equals(quizQuestion.Options[1], StringComparison.OrdinalIgnoreCase) &&
//                   (quizQuestion.CorrectOptions.Length == 1 && (quizQuestion.CorrectOptions[0].Equals("true", StringComparison.OrdinalIgnoreCase) || quizQuestion.CorrectOptions[0].Equals("false", StringComparison.OrdinalIgnoreCase)));
//        }

//        // Validate MCQ options
//        private bool ValidateMCQOptions(QuizQuestionViewModel quizQuestion)
//        {
//            return quizQuestion.Options.Length == 4 &&
//                   quizQuestion.Options.Distinct().Count() == 4 &&
//                   quizQuestion.CorrectOptions.Length == 1 &&
//                   quizQuestion.Options.Contains(quizQuestion.CorrectOptions[0]);
//        }

//        // Validate MSQ options
//        private bool ValidateMSQOptions(QuizQuestionViewModel quizQuestion)
//        {
//            int optionCount = quizQuestion.Options.Length;
//            int correctOptionCount = quizQuestion.CorrectOptions.Length;

//            return (optionCount >= 4 && optionCount <= 11) &&
//                   quizQuestion.Options.Distinct().Count() == optionCount &&
//                   (correctOptionCount >= 2 && correctOptionCount <= 3) &&
//                   quizQuestion.CorrectOptions.All(opt => quizQuestion.Options.Contains(opt));
//        }

//        // Get the count of MSQ options based on the filled rows
//        private int GetMSQOptionCount(ExcelWorksheet worksheet, int row)
//        {
//            int count = 0;
//            for (int i = 4; i <= 11; i++)
//            {
//                if (!string.IsNullOrEmpty(worksheet.Cells[row, i].Value?.ToString()))
//                    count++;
//                else
//                    break;
//            }
//            return count;
//        }

//        // Get the count of MSQ correct options based on the total option count
//        private int GetMSQCorrectOptionCount(int optionCount)
//        {
//            if (optionCount >= 5 && optionCount <= 8)
//                return 2;
//            else if (optionCount >= 9 && optionCount <= 11)
//                return 3;
//            else
//                return 0;
//        }

//        // Extract options from Excel worksheet
//        private string[] ExtractOptions(ExcelWorksheet worksheet, int row, int startColumn, int count)
//        {
//            string[] options = new string[count];
//            for (int i = 0; i < count; i++)
//            {
//                string option = worksheet.Cells[row, startColumn + i].Value?.ToString() ?? "";
//                options[i] = option;
//            }
//            return options;
//        }
//    }
//}
/*
 * using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using LXP.Data.IRepository;
using LXP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace LXP.Core.Services
{
    public class BulkQuestionService : IBulkQuestionService
    {
        private readonly IQuizRepository _quizRepository; // Add IQuizRepository to get QuizId dynamically
        private readonly IBulkQuestionRepository _bulkQuestionRepository;

        public BulkQuestionService(IQuizRepository quizRepository, IBulkQuestionRepository bulkQuestionRepository)
        {
            _quizRepository = quizRepository;
            _bulkQuestionRepository = bulkQuestionRepository;
        }

        public object ImportQuizData(IFormFile file, string quizName) // Accept quizName as a parameter
        {
            try
            {
                if (file == null || file.Length <= 0)
                    throw new ArgumentException("File is empty.");

                // Retrieve the QuizId from the database based on the quizName
                var quiz = _quizRepository.GetQuizByName(quizName);
                if (quiz == null)
                    throw new ArgumentException($"Quiz with name {quizName} not found.");

                Guid quizId = quiz.QuizId;

                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    stream.Position = 0;

                    using (ExcelPackage package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                            throw new ArgumentException("Worksheet not found.");

                        List<QuizQuestionViewModel> quizQuestions = new List<QuizQuestionViewModel>();

                        // Loop through each row in the worksheet
                        for (int row = 3; row <= worksheet.Dimension.End.Row; row++)
                        {
                            string type = worksheet.Cells[row, 2].Value?.ToString();

                            if (type == "MCQ" || type == "TF" || type == "MSQ")
                            {
                                QuizQuestionViewModel quizQuestion = new QuizQuestionViewModel
                                {
                                    QuestionType = type,
                                    QuestionNumber = Convert.ToInt32(worksheet.Cells[row, 1].Value),
                                    Question = worksheet.Cells[row, 3].Value.ToString(),
                                };

                                // Validate question type
                                if (!ValidateQuestionType(quizQuestion))
                                    continue;

                                // Extract options based on question type
                                if (type == "MCQ")
                                {
                                    quizQuestion.Options = ExtractOptions(worksheet, row, 4, 4);
                                    quizQuestion.CorrectOptions = ExtractOptions(worksheet, row, 12, 1);
                                    // Validate MCQ options
                                    if (!ValidateMCQOptions(quizQuestion))
                                        continue;
                                }
                                else if (type == "TF")
                                {
                                    quizQuestion.Options = ExtractOptions(worksheet, row, 4, 2);
                                    quizQuestion.CorrectOptions = ExtractOptions(worksheet, row, 12, 1);
                                    // Validate T/F options
                                    if (!ValidateTFOptions(quizQuestion))
                                        continue;
                                }
                                else if (type == "MSQ")
                                {
                                    int optionCount = GetMSQOptionCount(worksheet, row);
                                    quizQuestion.Options = ExtractOptions(worksheet, row, 4, optionCount);
                                    quizQuestion.CorrectOptions = ExtractOptions(worksheet, row, 12, GetMSQCorrectOptionCount(optionCount));
                                    // Validate MSQ options
                                    if (!ValidateMSQOptions(quizQuestion))
                                        continue;
                                }

                                quizQuestions.Add(quizQuestion);
                            }
                        }

                        // Loop through each question and add to repository
                        foreach (var quizQuestion in quizQuestions)
                        {
                            // Add question to the repository
                            QuizQuestion questionEntity = new QuizQuestion
                            {
                                QuizId = quizId, // Use dynamic QuizId here
                                QuestionNo = quizQuestion.QuestionNumber,
                                QuestionType = quizQuestion.QuestionType,
                                Question = quizQuestion.Question,
                                CreatedBy = "Admin",
                                CreatedAt = DateTime.UtcNow
                            };

                            // Save the question to get the QuizQuestionId
                            _bulkQuestionRepository.AddQuestions(new List<QuizQuestion> { questionEntity });

                            // Add options associated with the question
                            List<QuestionOption> optionEntities = new List<QuestionOption>();
                            for (int i = 0; i < quizQuestion.Options.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(quizQuestion.Options[i]))
                                {
                                    QuestionOption optionEntity = new QuestionOption
                                    {
                                        QuizQuestionId = questionEntity.QuizQuestionId,
                                        Option = quizQuestion.Options[i],
                                        IsCorrect = quizQuestion.CorrectOptions.Contains(quizQuestion.Options[i]),
                                        CreatedAt = DateTime.UtcNow,
                                        CreatedBy = "Admin",
                                        ModifiedBy = "Admin2"
                                    };

                                    optionEntities.Add(optionEntity);
                                }
                            }

                            _bulkQuestionRepository.AddOptions(optionEntities, questionEntity.QuizQuestionId);
                        }
                        return quizQuestions;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while importing quiz data: {ex.Message}", ex);
            }
        }

        // Validate question type
        private bool ValidateQuestionType(QuizQuestionViewModel quizQuestion)
        {
            return quizQuestion.QuestionType == "MCQ" || quizQuestion.QuestionType == "TF" || quizQuestion.QuestionType == "MSQ";
        }

        // Validate T/F options
        private bool ValidateTFOptions(QuizQuestionViewModel quizQuestion)
        {
            return quizQuestion.Options.Length == 2 &&
                   !string.IsNullOrEmpty(quizQuestion.Options[0]) &&
                   !string.IsNullOrEmpty(quizQuestion.Options[1]) &&
                   !quizQuestion.Options[0].Equals(quizQuestion.Options[1], StringComparison.OrdinalIgnoreCase) &&
                   (quizQuestion.CorrectOptions.Length == 1 && (quizQuestion.CorrectOptions[0].Equals("true", StringComparison.OrdinalIgnoreCase) || quizQuestion.CorrectOptions[0].Equals("false", StringComparison.OrdinalIgnoreCase)));
        }

        // Validate MCQ options
        private bool ValidateMCQOptions(QuizQuestionViewModel quizQuestion)
        {
            return quizQuestion.Options.Length == 4 &&
                   quizQuestion.Options.Distinct().Count() == 4 &&
                   quizQuestion.CorrectOptions.Length == 1 &&
                   quizQuestion.Options.Contains(quizQuestion.CorrectOptions[0]);
        }

        // Validate MSQ options
        private bool ValidateMSQOptions(QuizQuestionViewModel quizQuestion)
        {
            int optionCount = quizQuestion.Options.Length;
            int correctOptionCount = quizQuestion.CorrectOptions.Length;

            return (optionCount >= 4 && optionCount <= 11) &&
                   quizQuestion.Options.Distinct().Count() == optionCount &&
                   (correctOptionCount >= 2 && correctOptionCount <= 3) &&
                   quizQuestion.CorrectOptions.All(opt => quizQuestion.Options.Contains(opt));
        }

        // Get the count of MSQ options based on the filled rows
        private int GetMSQOptionCount(ExcelWorksheet worksheet, int row)
        {
            int count = 0;
            for (int i = 4; i <= 11; i++)
            {
                if (!string.IsNullOrEmpty(worksheet.Cells[row, i].Value?.ToString()))
                    count++;
                else
                    break;
            }
            return count;
        }

        // Get the count of MSQ correct options based on the total option count
        private int GetMSQCorrectOptionCount(int optionCount)
        {
            if (optionCount >= 5 && optionCount <= 8)
                return 2;
            else if (optionCount >= 9 && optionCount <= 11)
                return 3;
            else
                return 0;
        }

        // Extract options from Excel worksheet
        private string[] ExtractOptions(ExcelWorksheet worksheet, int row, int startColumn, int count)
        {
            string[] options = new string[count];
            for (int i = 0; i < count; i++)
            {
                string option = worksheet.Cells[row, startColumn + i].Value?.ToString() ?? "";
                options[i] = option;
            }
            return options;
        }
    }
}
*/





//using LXP.Common.Entities;
//using LXP.Common.ViewModels;
//using LXP.Core.IServices;
//using Microsoft.AspNetCore.Http;
//using OfficeOpenXml;
//using LXP.Data.IRepository;
//using LXP.Core.Repositories;

//namespace LXP.Core.Services
//{
//    public class BulkQuestionService : IBulkQuestionService
//    {
//        private readonly IBulkQuestionRepository _bulkQuestionRepository;

//        public BulkQuestionService(IBulkQuestionRepository bulkQuestionRepository)
//        {
//            _bulkQuestionRepository = bulkQuestionRepository;
//        }
//        public object ImportQuizData(IFormFile file)
//        {
//            try
//            {
//                if (file == null || file.Length <= 0)
//                    throw new ArgumentException("File is empty.");

//                using (var stream = new MemoryStream())
//                {
//                    file.CopyTo(stream);
//                    stream.Position = 0;

//                    using (ExcelPackage package = new ExcelPackage(stream))
//                    {
//                        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
//                        if (worksheet == null)
//                            throw new ArgumentException("Worksheet not found.");

//                        List<QuizQuestionViewModel> quizQuestions = new List<QuizQuestionViewModel>();

//                        // Loop through each row in the worksheet
//                        for (int row = 3; row <= worksheet.Dimension.End.Row; row++)
//                        {
//                            string type = worksheet.Cells[row, 2].Value?.ToString();

//                            if (type == "MCQ" || type == "TF" || type == "MSQ")
//                            {
//                                QuizQuestionViewModel quizQuestion = new QuizQuestionViewModel
//                                {
//                                    QuestionType = type,
//                                    QuestionNumber = Convert.ToInt32(worksheet.Cells[row, 1].Value),
//                                    Question = worksheet.Cells[row, 3].Value.ToString(),
//                                };

//                                // Extract options based on question type
//                                if (type == "MCQ")
//                                {
//                                    quizQuestion.Options = ExtractOptions(worksheet, row, 4, 4);
//                                    quizQuestion.CorrectOptions = ExtractOptions(worksheet, row, 12, 1);
//                                }
//                                else if (type == "TF")
//                                {
//                                    quizQuestion.Options = ExtractOptions(worksheet, row, 4, 2);
//                                    quizQuestion.CorrectOptions = ExtractOptions(worksheet, row, 12, 1);
//                                }
//                                else if (type == "MSQ")
//                                {
//                                    quizQuestion.Options = ExtractOptions(worksheet, row, 4, 8);
//                                    quizQuestion.CorrectOptions = ExtractOptions(worksheet, row, 12, 3);
//                                }

//                                quizQuestions.Add(quizQuestion);
//                            }
//                        }

//                        // Loop through each question and add to repository
//                        foreach (var quizQuestion in quizQuestions)

//                        {

//                            // Add question to the repository
//                            QuizQuestion questionEntity = new QuizQuestion
//                            {
//                                QuizId = Guid.Parse("98984911-1862-4745-92ba-570bff6bcf05"),
//                                QuestionNo = quizQuestion.QuestionNumber,
//                                QuestionType = quizQuestion.QuestionType,
//                                Question = quizQuestion.Question,
//                                CreatedBy = "Admin",
//                                CreatedAt = DateTime.UtcNow
//                            };

//                            // Save the question to get the QuizQuestionId
//                            _bulkQuestionRepository.AddQuestions(new List<QuizQuestion> { questionEntity });

//                            // Add options associated with the question
//                            List<QuestionOption> optionEntities = new List<QuestionOption>();
//                            for (int i = 0; i < quizQuestion.Options.Length; i++)
//                            {
//                                if (!string.IsNullOrEmpty(quizQuestion.Options[i]))
//                                {
//                                    QuestionOption optionEntity = new QuestionOption
//                                    {
//                                        QuizQuestionId = questionEntity.QuizQuestionId,
//                                        Option = quizQuestion.Options[i],
//                                        IsCorrect = quizQuestion.CorrectOptions.Contains(quizQuestion.Options[i]),
//                                        CreatedAt = DateTime.UtcNow,
//                                        CreatedBy = "Admin",
//                                        ModifiedBy = "Admin2"
//                                    };

//                                    optionEntities.Add(optionEntity);
//                                }
//                            }

//                            // Save options to the repository
//                            _bulkQuestionRepository.AddOptions(optionEntities, questionEntity.QuizQuestionId);
//                        }
//                        return quizQuestions;
//                    }


//                }

//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"An error occurred while importing quiz data: {ex.Message}", ex);
//            }
//        }

//        private string[] ExtractOptions(ExcelWorksheet worksheet, int row, int startColumn, int count)
//        {
//            string[] options = new string[count];
//            for (int i = 0; i < count; i++)
//            {
//                string option = worksheet.Cells[row, startColumn + i].Value?.ToString() ?? "";
//                options[i] = option;
//            }
//            return options;
//        }
//    }
//}

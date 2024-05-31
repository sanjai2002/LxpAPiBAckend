using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels.QuizQuestionViewModel

{
	    public class BulkQuizQuestionViewModel
    {
			public int QuestionNumber { get; set; }
			public string? QuestionType { get; set; }
			public string? Question { get; set; }
			public string[]? Options { get; set; }
			public string[]? CorrectOptions { get; set; }
		}
}


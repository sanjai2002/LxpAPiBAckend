using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.DTO
{
    public class TopicFeedbackQuestionViewModel
    {
        public Guid TopicId { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public List<FeedbackOptionDTO> Options { get; set; }
    }
}

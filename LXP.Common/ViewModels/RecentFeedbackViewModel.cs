using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class RecentFeedbackViewModel
    {
        public string? Coursename { get; set; }
        public string? TopicName {  get; set; }
        public Guid Learnerid { get; set; }
        public string ?LearnerName { get; set; }
        public string? Profilephoto {  get; set; }
        public Guid FeedbackresponseId {  get; set; }
        public string? Topicfeedbackquestions { get; set; }
        public string? Feedbackresponse {  get; set; }
        public DateTime DateoftheResponse {  get; set; }
    }
}

namespace LXP.Common.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int NoOfLearners { get; set; }

        public int NoOfCourse { get; set; }

        public int NoOfActiveLearners { get; set; }

        public List<string> HighestEnrolledCourse { get; set; }

        public List<string> GetTopLearners { get; set; }
        public List<string> GetTopFeedback {  get; set; }

    }
}

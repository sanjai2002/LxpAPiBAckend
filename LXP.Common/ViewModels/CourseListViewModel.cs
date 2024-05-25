namespace LXP.Common.ViewModels
{
    public class CourseListViewModel
    {
        public Guid CourseId { get; set; }

        public string Level { get; set; }

        public string Catagory { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Duration { get; set; }

        public string Thumbnail { get; set; }

        public bool IsActive { get; set; }

        public bool IsAvailable { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }



    }
}

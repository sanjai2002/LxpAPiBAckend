namespace LXP.Common.ViewModels
{
    public class CourseTopicListViewModel
    {
        public Guid TopicId { get; set; }

        public Guid CourseId { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; } = null!;

    }
}

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace LXP.Common.ViewModels
{
    public class CourseUpdateModel
    {
        ///<Summary>
        ///CourseId
        ///</Summary>
        ///<example>20355af6-81db-47fa-b41a-805ae3049da9</example>
        public Guid CourseId { get; set; }

        ///<Summary>
        ///CourseTitle
        ///</Summary>
        ///<example>Html</example>
        public string Title { get; set; }

        ///<Summary>
        ///Course Level
        ///</Summary>
        ///<example>Beginner</example>

        public Guid LevelId { get; set; }
        ///<Summary>
        ///Course Category
        ///</Summary>
        ///<example>Technical</example>
        public Guid CategoryId { get; set; }

        ///<Summary>
        ///Course Description
        ///</Summary>
        ///<example>This course contains the detailed explanation about the Html structure</example>

        public string Description { get; set; }

        ///<Summary>
        ///Course Duration
        ///</Summary>
        ///<example>10.00</example>

        public decimal Duration { get; set; }

        ///<Summary>
        ///Course Thumbnail
        ///</Summary>
        ///<example>Image with filesize less than 250kb and file extension jpeg or png</example>

        [NotMapped]
        public IFormFile Thumbnailimage { get; set; }
        ///<Summary>
        ///Course ModifiedBy
        ///</Summary>
        ///<example>Admin</example>
        public string? ModifiedBy { get; set; }

    }
}

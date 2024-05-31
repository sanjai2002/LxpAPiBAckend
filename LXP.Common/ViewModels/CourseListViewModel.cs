using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class CourseListViewModel
    {
        ///<summary>
        ///course id
        ///</summary>

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

        public string Level { get; set; }

        ///<Summary>
        ///Course Category
        ///</Summary>
        ///<example>Technical</example>

        public string? Category { get; set; }

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
        public string Thumbnailimage { get; set; }

        public bool IsActive { get; set; }

        public bool IsAvailable { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? ModifiedBy { get; set; } = null!;

        public DateTime? ModifiedAt { get; set; } = null!;
    }
}

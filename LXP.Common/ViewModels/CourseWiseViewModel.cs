using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace LXP.Common.ViewModels
{
    public class CourseWiseViewModel
    {
        public int Count { get; set; }
        public string CourseName { get; set; }
        public Guid CourseId { get; set; }
    }
}

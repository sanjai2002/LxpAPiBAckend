using LXP.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LXP.Common.ViewModels;
namespace LXP.Core.IServices
{
    public interface ICourseTopicServices
    {
        object GetTopicDetails(string courseId);
        Task<bool> AddCourseTopic(CourseTopicViewModel topic);
    }
}
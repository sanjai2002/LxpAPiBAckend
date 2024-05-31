using LXP.Common.Entities;
using LXP.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.IRepository
{
    public interface ICourseRepository
    {
        Course GetCourseDetailsByCourseId(Guid CourseId);
        Task AddCourse(Course course);
        bool AnyCourseByCourseTitle(string CourseTitle);

        IEnumerable<CourseListViewModel> GetAllCourseDetails();
    }
}
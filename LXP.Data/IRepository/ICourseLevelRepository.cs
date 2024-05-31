using LXP.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.IRepository
{
    public interface ICourseLevelRepository
    {
        Task AddCourseLevel(CourseLevel level);

        Task<List<CourseLevel>> GetAllCourseLevel();
        CourseLevel GetCourseLevelByCourseLevelId(Guid courseLevelId);
    }
}
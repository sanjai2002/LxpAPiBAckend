
using LXP.Common.ViewModels;
using LXP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LXP.Common.Entities;

namespace LXP.Core.IServices
{
    public interface ICourseServices
    {
        // check simi
        Task<CourseListViewModel> GetCourseDetailsByCourseId(string courseId);
        Course GetCourseByCourseId(Guid courseId);


        CourseListViewModel GetCourseDetailsByCourseName(string courseName);
        CourseListViewModel AddCourse(CourseViewModel course);

        //Course GetCourseByCourseId(Guid courseId);

        Task<bool> Deletecourse(Guid courseId);

        Task<bool> Changecoursestatus(Coursestatus status);

        Task<bool> Updatecourse(CourseUpdateModel course);

        IEnumerable<CourseDetailsViewModel> GetAllCourse();
        IEnumerable<CourseDetailsViewModel> GetLimitedCourse();


    }
}

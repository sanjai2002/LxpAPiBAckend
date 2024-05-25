
using LXP.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LXP.Common.Entities;
namespace LXP.Data.IRepository
{
    public interface ICourseRepository
    {
        void AddCourse(Course course);
        bool AnyCourseByCourseTitle(string courseTitle);
        Course GetCourseDetailsByCourseName(string courseName);




        Course GetCourseDetailsByCourseId(Guid CourseId);
     
        Course FindCourseid(Guid courseid);

        public Enrollment FindEntrollmentcourse(Guid Courseid);
       
        Task Deletecourse(Course course);

        Task Changecoursestatus(Course course);

        Task Updatecourse(Course course);

        IEnumerable<CourseDetailsViewModel> GetAllCourse();
        IEnumerable<CourseDetailsViewModel> GetLimitedCourse();


    }
}

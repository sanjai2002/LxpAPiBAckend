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
        //Task<int> UpdateCourse(Course course);
        //void Update(CourseUpdateModel course);

        Course FindCourseid(Guid courseid);

        public Enrollment FindEntrollmentcourse(Guid Courseid);
       
        Task Deletecourse(Course course);

        Task Changecoursestatus(Course course);


        Task Updatecourse(Course course);

        IEnumerable<CourseViewModel> GetAllCourse();
        IEnumerable<CourseViewModel> GetLimitedCourse();


    }
}

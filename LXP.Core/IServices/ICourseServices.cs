using LXP.Common.Entities;
using LXP.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.IServices
{
    public interface ICourseServices
    {
        Course GetCourseByCourseId(Guid courseId);

        //bool UpdateCourse(CourseUpdateModel course);
        //void UpdateCourseusingid(CourseUpdateModel course);
        //IEnumerable<CourseUpdateModel> GetIndividualCourse(Guid id);
        
        bool Deletecourse(Guid courseId);

        Task<bool> Changecoursestatus(Coursestatus status);

        Task<bool> Updatecourse(CourseUpdateModel course);

        IEnumerable<CourseViewModel> GetAllCourse();
        IEnumerable<CourseViewModel> GetLimitedCourse();


    }
}

using LXP.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LXP.Common.Entities;

namespace LXP.Core.IServices
{
    public interface ICourseLevelServices
    {
        Task<List<CourseLevelListViewModel>> GetAllCourseLevel(string CreatedBy);
        Task AddCourseLevel(string Level, string CreatedBy);
    }
}
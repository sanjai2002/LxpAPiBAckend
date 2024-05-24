using AutoMapper;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.Services
{
    public class CourseLevelServices:ICourseLevelServices
    {
        private readonly ICourseLevelRepository _courseLevelRepository;
        private Mapper _levelMapper;

        public CourseLevelServices(ICourseLevelRepository courseLevelRepository) 
        {
          this._courseLevelRepository = courseLevelRepository;
            var _configLevel = new MapperConfiguration(cfg => cfg.CreateMap<CourseLevel, CourseLevelListViewModel>().ReverseMap());
            _levelMapper = new Mapper(_configLevel);
        }

        public async Task<List<CourseLevelListViewModel>> GetAllCourseLevel(string accessedBy)
        {
            List<CourseLevel> CourseLevel = await _courseLevelRepository.GetAllCourseLevel();
            if (CourseLevel.Count == 0)
            {
                await AddCourseLevel("Beginner", accessedBy);
                await AddCourseLevel("Advanced", accessedBy);
                await AddCourseLevel("Intermediate", accessedBy);
            }
            return _levelMapper.Map<List<CourseLevel>, List<CourseLevelListViewModel>>(await _courseLevelRepository.GetAllCourseLevel());
        }
        public async Task AddCourseLevel(string level,string createdBy)
        {
            CourseLevel course = new CourseLevel()
            {
                LevelId = Guid.NewGuid(),
                Level = level,
                CreatedAt = DateTime.Now,
                CreatedBy = createdBy
            };
            await _courseLevelRepository.AddCourseLevel(course);
        }

    }
}

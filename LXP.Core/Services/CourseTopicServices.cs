using AutoMapper;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;

namespace LXP.Core.Services
{
    public class CourseTopicServices : ICourseTopicServices
    {
        private readonly ICourseTopicRepository _courseTopicRepository;
        private readonly ICourseRepository _courseRepository;
        private Mapper _courseTopicMapper;

        public CourseTopicServices(ICourseTopicRepository courseTopicRepository, ICourseRepository courseRepository)
        {
            var _configCourseTopic = new MapperConfiguration(cfg => cfg.CreateMap<Topic, CourseTopicListViewModel>().ReverseMap());
            _courseTopicMapper = new Mapper(_configCourseTopic);
            _courseTopicRepository = courseTopicRepository;
            _courseRepository = courseRepository;
        }
        public object GetAllTopicDetailsByCourseId(string courseId)
        {
            return _courseTopicRepository.GetAllTopicDetailsByCourseId(courseId);
        }
        public async Task<CourseTopicListViewModel> GetTopicDetailsByTopicNameAndCourseId(string topicName, string courseId)
        {
            //Course course = _courseRepository.GetCourseDetailsByCourseId(Guid.Parse(courseId));
            Topic topic = await _courseTopicRepository.GetTopicDetailsByTopicNameAndCourse(topicName, Guid.Parse(courseId));
            CourseTopicListViewModel courseTopic = new CourseTopicListViewModel()
            {
                TopicId = topic.TopicId,
                CourseId = topic.CourseId,
                Name = topic.Name,
                Description = topic.Description,
                IsActive = topic.IsActive,
                CreatedAt = topic.CreatedAt,
                CreatedBy = topic.CreatedBy,
                ModifiedAt = topic.ModifiedAt,
                ModifiedBy = topic.ModifiedBy
            };
            return courseTopic;
        }
        public async Task<CourseTopicListViewModel> GetTopicDetailsByTopicId(string topicId)
        {
            Topic topic = await _courseTopicRepository.GetTopicByTopicId(Guid.Parse(topicId));
            CourseTopicListViewModel courseTopic = new CourseTopicListViewModel()
            {
                TopicId = topic.TopicId,
                CourseId = topic.CourseId,
                Name = topic.Name,
                Description = topic.Description,
                IsActive = topic.IsActive,
                CreatedAt = topic.CreatedAt,
                CreatedBy = topic.CreatedBy,
                ModifiedAt = topic.ModifiedAt,
                ModifiedBy = topic.ModifiedBy
            };
            return courseTopic;
        }
        public async Task<CourseTopicListViewModel> AddCourseTopic(CourseTopicViewModel courseTopic)
        {
            bool isTopicExists = _courseTopicRepository.AnyTopicByTopicName(courseTopic.Name);
            Guid courseId = Guid.Parse((courseTopic.CourseId));
            Course course = _courseRepository.GetCourseDetailsByCourseId(courseId);
            if (!isTopicExists)
            {
                Topic topic = new Topic()
                {
                    TopicId = Guid.NewGuid(),
                    Name = courseTopic.Name,
                    Description = courseTopic.Description,
                    CourseId = course.CourseId,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    CreatedBy = courseTopic.CreatedBy,
                    ModifiedAt = null,
                    ModifiedBy = null
                };
                _courseTopicRepository.AddCourseTopic(topic);

                return _courseTopicMapper.Map<Topic, CourseTopicListViewModel>(topic); ;
            }
            else
            {
                return null;
            }



        }
        public async Task<bool> UpdateCourseTopic(CourseTopicUpdateModel courseTopic)
        {

            Topic topic = await _courseTopicRepository.GetTopicByTopicId(Guid.Parse(courseTopic.TopicId));

            topic.Name = courseTopic.Name;
            topic.Description = courseTopic.Description;
            topic.ModifiedBy = courseTopic.ModifiedBy;
            topic.ModifiedAt = DateTime.Now;

            bool isTopicUpdated = await _courseTopicRepository.UpdateCourseTopic(topic) > 0 ? true : false;
            if (isTopicUpdated)
            {
                return true;
            }
            return false;

        }
        public async Task<bool> SoftDeleteTopic(string topicId)
        {

            Topic topic = await _courseTopicRepository.GetTopicByTopicId(Guid.Parse(topicId));
            topic.IsActive = false;
            bool isTopicDeleted = await _courseTopicRepository.UpdateCourseTopic(topic) > 0 ? true : false;
            if (isTopicDeleted)
            {
                return true;
            }
            return false;

        }


    }
}

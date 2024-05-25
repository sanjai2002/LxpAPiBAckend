using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace LXP.Core.Services
{
    public class LearnerServices : ILearnerServices
    {
        private readonly ILearnerRepository _LearnerRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;

        public LearnerServices(ILearnerRepository courseRepository, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccess)
        {
            _LearnerRepository = courseRepository;
            _environment = environment;
            _contextAccessor = httpContextAccess;

        }

        public IEnumerable<AllLearnersViewModel> GetLearners()
        {

            var result = _LearnerRepository.GetLearners();
            return result;
        }

        public object GetAllLearnerDetailsByLearnerId(Guid LearnerId)
        {
            return _LearnerRepository.GetAllLearnerDetailsByLearnerId(LearnerId);

        }

        public object GetLearnerEnrolledcourseByLearnerId(Guid LearnerId)
        {
            return _LearnerRepository.GetLearnerEnrolledcourseByLearnerId(LearnerId);

        }










    }
}

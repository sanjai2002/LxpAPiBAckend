using AutoMapper;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using LXP.Data.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.Services
{
    public  class LearnerServices:ILearnerServices
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

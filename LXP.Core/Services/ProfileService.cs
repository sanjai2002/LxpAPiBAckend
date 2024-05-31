//using AutoMapper;
//using LXP.Common.Entities;
//using LXP.Common.ViewModels;
//using LXP.Core.IServices;
//using LXP.Data.IRepository;
//using LXP.Data.Repository;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LXP.Core.Services
//{
//    public  class ProfileService:IProfileService
//    {
//        private readonly IProfileRepository _profileRepository;
//        private Mapper _learnerProfileMapper;

//        public ProfileService( IProfileRepository profileRepository)
//        {
            
//            this._profileRepository = profileRepository;
//            var _configCategory = new MapperConfiguration(cfg => cfg.CreateMap<LearnerProfile, GetProfileViewModel>().ReverseMap());
//            _learnerProfileMapper = new Mapper(_configCategory);

//        }

//        public async Task<List<GetProfileViewModel>> GetAllLearnerProfile()
//        {
//            List<GetProfileViewModel> learnerProfile = _learnerProfileMapper.Map<List<LearnerProfile>, List<GetProfileViewModel>>(await _profileRepository.GetAllLearnerProfile());
//            return learnerProfile;
//        }

//        public LearnerProfile GetLearnerProfileById(string id) {

//            return _profileRepository.GetLearnerprofileDetailsByLearnerprofileId(Guid.Parse(id));
        
//        }

//    }
//}










using AutoMapper;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using LXP.Data.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private Mapper _learnerProfileMapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;
        public ProfileService(IProfileRepository profileRepository, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {

            this._profileRepository = profileRepository;
            var _configCategory = new MapperConfiguration(cfg => cfg.CreateMap<LearnerProfile, GetProfileViewModel>().ReverseMap());
            _learnerProfileMapper = new Mapper(_configCategory);
            _environment = environment;
            _contextAccessor = httpContextAccessor;

        }

        public async Task<List<GetProfileViewModel>> GetAllLearnerProfile()
        {
            List<GetProfileViewModel> learnerProfile = _learnerProfileMapper.Map<List<LearnerProfile>, List<GetProfileViewModel>>(await _profileRepository.GetAllLearnerProfile());
            return learnerProfile;
        }

        public LearnerProfile GetLearnerProfileById(string id)
        {

            //return _profileRepository.GetLearnerprofileDetailsByLearnerprofileId(Guid.Parse(id));

            var profile = _profileRepository.GetLearnerprofileDetailsByLearnerprofileId(Guid.Parse(id));
            var profileIndividual = new LearnerProfile
            {
                ProfileId = profile.ProfileId,
                //ProfilePhoto = profile.ProfilePhoto,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Dob = profile.Dob,
                Gender = profile.Gender,
                Stream = profile.Stream,
                ContactNumber = profile.ContactNumber,
                ProfilePhoto = String.Format("{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                                             _contextAccessor.HttpContext.Request.Scheme,
                                             _contextAccessor.HttpContext.Request.Host,
                                             _contextAccessor.HttpContext.Request.PathBase,
                                             profile.ProfilePhoto)
            };
            return profileIndividual;
        }


        public async Task UpdateProfile(UpdateProfileViewModel model)
        {
            LearnerProfile learnerProfile = _profileRepository.GetLearnerprofileDetailsByLearnerprofileId(Guid.Parse(model.ProfileId));

            if (model.ProfilePhoto != null)
            {
                var uniqueFileName = $"{Guid.NewGuid()}_{model.ProfilePhoto.FileName}";
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "Images"); // Use WebRootPath
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePhoto.CopyToAsync(stream);
                }

                learnerProfile.ProfilePhoto = uniqueFileName;
            }

            learnerProfile.FirstName = model.FirstName;
            learnerProfile.LastName = model.LastName;
            learnerProfile.ModifiedBy = $"{model.FirstName} {model.LastName}";
            learnerProfile.ModifiedAt = DateTime.Now;
            learnerProfile.ContactNumber = model.ContactNumber;
            learnerProfile.Dob = DateOnly.ParseExact(model.Dob, "yyyy-MM-dd", null);
            learnerProfile.Gender = model.Gender;
            learnerProfile.Stream = model.Stream;

            await _profileRepository.UpdateProfile(learnerProfile);
        }


    }
}
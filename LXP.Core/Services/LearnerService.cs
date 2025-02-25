﻿using AutoMapper;
using LXP.Common.Entities;
using LXP.Common.Utils;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;

namespace LXP.Core.Services
{
    public class LearnerService : ILearnerService
    {
        private readonly ILearnerRepository _learnerRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly IProfilePasswordHistoryRepository _profilePasswordHistoryRepository;
        private Mapper _learnerMapper;

        public LearnerService(
            ILearnerRepository learnerRepository,
            IProfileRepository profileRepository,
            IProfilePasswordHistoryRepository profilePasswordHistoryRepository
        )
        {
            this._learnerRepository = learnerRepository;
            this._profileRepository = profileRepository;
            this._profilePasswordHistoryRepository = profilePasswordHistoryRepository;
            var _configCategory = new MapperConfiguration(cfg =>
                cfg.CreateMap<Learner, GetLearnerViewModel>().ReverseMap()
            );
            _learnerMapper = new Mapper(_configCategory);
            _profilePasswordHistoryRepository = profilePasswordHistoryRepository;
        }

        public async Task<bool> LearnerRegistration(RegisterUserViewModel registerUserViewModel)
        {
            bool isLearnerExists = await _learnerRepository.AnyLearnerByEmail(
                registerUserViewModel.email
            );
            if (!isLearnerExists)
            {
                using (var transaction = _learnerRepository.BeginTransaction())
                {
                    try
                    {
                        Learner newlearner = new Learner()
                        {
                            LearnerId = Guid.NewGuid(),
                            Email = registerUserViewModel.email,
                            Password = SHA256Encrypt.ComputePasswordToSha256Hash(
                                registerUserViewModel.password
                            ),
                            Role = registerUserViewModel.role,
                            UnblockRequest = false,
                            AccountStatus = true,
                            UserLastLogin = DateTime.Now,
                            CreatedAt = DateTime.Now,
                            CreatedBy =
                                $"{registerUserViewModel.firstName} {registerUserViewModel.lastName}",
                            ModifiedAt = DateTime.Now,
                            ModifiedBy =
                                $"{registerUserViewModel.firstName} {registerUserViewModel.lastName}"
                        };
                        _learnerRepository.AddLearner(newlearner);
                        Learner learner = _learnerRepository.GetLearnerByLearnerEmail(
                            newlearner.Email
                        );
                        LearnerProfile profile = new LearnerProfile()
                        {
                            ProfileId = Guid.NewGuid(),
                            FirstName = registerUserViewModel.firstName,
                            LastName = registerUserViewModel.lastName,
                            Dob = DateOnly.ParseExact(
                                registerUserViewModel.dob,
                                "yyyy-MM-dd",
                                null
                            ),
                            Gender = registerUserViewModel.gender,
                            ContactNumber = registerUserViewModel.contactNumber,
                            Stream = registerUserViewModel.stream,
                            CreatedAt = DateTime.Now,
                            CreatedBy =
                                $"{registerUserViewModel.firstName} {registerUserViewModel.lastName}",
                            LearnerId = learner.LearnerId,
                            ModifiedBy =
                                $"{registerUserViewModel.firstName} {registerUserViewModel.lastName}",
                        };

                        PasswordHistory passwordHistory = new PasswordHistory()
                        {
                            PasswordId = Guid.NewGuid(),
                            LearnerId = learner.LearnerId,
                            NewPassword = learner.Password,
                            CreatedBy =
                                $"{registerUserViewModel.firstName} {registerUserViewModel.lastName}",
                            CreatedAt = DateTime.Now,
                            ModifiedAt = DateTime.Now,
                            ModifiedBy =
                                $"{registerUserViewModel.firstName} {registerUserViewModel.lastName}"
                        };

                        _profilePasswordHistoryRepository.AddPasswordHistory1(passwordHistory);
                        _profileRepository.AddProfile(profile);

                        // Commit the transaction
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        // An error occurred, roll back the transaction
                        transaction.Rollback();
                        throw; // Re-throw the exception
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<List<GetLearnerViewModel>> GetAllLearner()
        {
            List<GetLearnerViewModel> learner = _learnerMapper.Map<
                List<Learner>,
                List<GetLearnerViewModel>
            >(await _learnerRepository.GetAllLearner());
            return learner;
        }

        public Learner GetLearnerById(string id)
        {
            return _learnerRepository.GetLearnerDetailsByLearnerId(Guid.Parse(id));
        }

        public async Task<LearnerAndProfileViewModel> LearnerGetLearnerById(string id)
        {
            var learnerId = Guid.Parse(id);
            var learner = _learnerRepository.GetLearnerDetailsByLearnerId(learnerId);
            var profile = await _profileRepository.GetProfileByLearnerId(learnerId);

            var result = new LearnerAndProfileViewModel
            {
                Email = learner.Email,
                Role = learner.Role,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Dob = profile.Dob.ToString(),
                Gender = profile.Gender,
                ContactNumber = profile.ContactNumber,
                Stream = profile.Stream,
            };

            return result;
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using AutoMapper;
//using LXP.Common;
//using LXP.Common.Entities;
//using LXP.Common.Utils;
//using LXP.Common.ViewModels;
//using LXP.Core.IServices;
//using LXP.Data;
//using LXP.Data.IRepository;
//using LXP.Data.Repository;
//using Microsoft.EntityFrameworkCore;

//namespace LXP.Core.Services
//{
//    public class LearnerService : ILearnerService
//    {
//        private readonly ILearnerRepository _learnerRepository;
//        private readonly IProfileRepository _profileRepository;
//        private readonly IProfilePasswordHistoryRepository _profilePasswordHistoryRepository;

//        // private readonly IPasswordHistoryRepository _passwordHistoryRepository;
//        private Mapper _learnerMapper; //Mapper1

//        public LearnerService(
//            ILearnerRepository learnerRepository,
//            IProfileRepository profileRepository,
//            IProfilePasswordHistoryRepository profilePasswordHistoryRepository
//        )
//        {
//            this._learnerRepository = learnerRepository;
//            this._profileRepository = profileRepository;
//            this._profilePasswordHistoryRepository = profilePasswordHistoryRepository;
//            var _configCategory = new MapperConfiguration(cfg =>
//                cfg.CreateMap<Learner, GetLearnerViewModel>().ReverseMap()
//            ); //mapper 2
//            _learnerMapper = new Mapper(_configCategory); // mapper 3
//            _profilePasswordHistoryRepository = profilePasswordHistoryRepository;
//        }

//        public async Task<bool> LearnerRegistration(RegisterUserViewModel registerUserViewModel)
//        {
//            bool isLearnerExists = await _learnerRepository.AnyLearnerByEmail(
//                registerUserViewModel.email
//            );
//            if (!isLearnerExists)
//            {
//                Learner newlearner = new Learner()
//                {
//                    LearnerId = Guid.NewGuid(),
//                    Email = registerUserViewModel.email,
//                    Password = SHA256Encrypt.ComputePasswordToSha256Hash(
//                        registerUserViewModel.password
//                    ),
//                    Role = registerUserViewModel.role,
//                    UnblockRequest = false,
//                    AccountStatus = true,
//                    UserLastLogin = DateTime.Now,
//                    CreatedAt = DateTime.Now,
//                    CreatedBy =
//                        $"{registerUserViewModel.firstName} {registerUserViewModel.lastName}",
//                    ModifiedAt = DateTime.Now,
//                    ModifiedBy =
//                        $"{registerUserViewModel.firstName} {registerUserViewModel.lastName}"
//                };
//                _learnerRepository.AddLearner(newlearner);
//                Learner learner = _learnerRepository.GetLearnerByLearnerEmail(newlearner.Email);
//                LearnerProfile profile = new LearnerProfile()
//                {
//                    ProfileId = Guid.NewGuid(),
//                    FirstName = registerUserViewModel.firstName,
//                    LastName = registerUserViewModel.lastName,
//                    Dob = DateOnly.ParseExact(registerUserViewModel.dob, "yyyy-MM-dd", null),
//                    Gender = registerUserViewModel.gender,
//                    ContactNumber = registerUserViewModel.contactNumber,
//                    Stream = registerUserViewModel.stream,
//                    CreatedAt = DateTime.Now,
//                    CreatedBy =
//                        $"{registerUserViewModel.firstName} {registerUserViewModel.lastName}",
//                    LearnerId = learner.LearnerId,
//                    ModifiedBy =
//                        $"{registerUserViewModel.firstName} {registerUserViewModel.lastName}",
//                };

//                PasswordHistory passwordHistory = new PasswordHistory()
//                {
//                    PasswordId = Guid.NewGuid(),

//                    LearnerId = learner.LearnerId,

//                    NewPassword = learner.Password,

//                    CreatedBy =
//                        $"{registerUserViewModel.firstName} {registerUserViewModel.lastName}",

//                    CreatedAt = DateTime.Now,

//                    ModifiedAt = DateTime.Now,

//                    ModifiedBy =
//                        $"{registerUserViewModel.firstName} {registerUserViewModel.lastName}"
//                };

//                _profilePasswordHistoryRepository.AddPasswordHistory1(passwordHistory);
//                _profileRepository.AddProfile(profile);
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public async Task<List<GetLearnerViewModel>> GetAllLearner()
//        {
//            List<GetLearnerViewModel> learner = _learnerMapper.Map<
//                List<Learner>,
//                List<GetLearnerViewModel>
//            >(await _learnerRepository.GetAllLearner()); //mapper 4
//            return learner;
//        }

//        //public void UpdateAllLearner(Learner learner)
//        //{
//        //    if (learner == null)
//        //    {
//        //        throw new ArgumentNullException(nameof(learner));
//        //    }

//        //    _learnerRepository.UpdateAllLearner(learner);
//        //}


//        public Learner GetLearnerById(string id)
//        {
//            return _learnerRepository.GetLearnerDetailsByLearnerId(Guid.Parse(id));
//        }

//        public async Task<LearnerAndProfileViewModel> LearnerGetLearnerById(string id)
//        {
//            var learnerId = Guid.Parse(id);
//            var learner = _learnerRepository.GetLearnerDetailsByLearnerId(learnerId);
//            var profile = await _profileRepository.GetProfileByLearnerId(learnerId);

//            var result = new LearnerAndProfileViewModel
//            {
//                Email = learner.Email,
//                Role = learner.Role,

//                FirstName = profile.FirstName,
//                LastName = profile.LastName,
//                Dob = profile.Dob.ToString(),
//                Gender = profile.Gender,
//                ContactNumber = profile.ContactNumber,
//                Stream = profile.Stream,
//            };

//            return result;
//        }
//    }
//}

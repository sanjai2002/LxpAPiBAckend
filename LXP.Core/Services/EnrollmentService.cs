using System;
using LXP.Common.ViewModels;
using LXP.Common.Entities;
using LXP.Core.IServices;
using Microsoft.Extensions.Hosting;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using LXP.Data.Repository;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Reflection.Metadata.Ecma335;
//using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly ILearnerRepository _learnerRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private Mapper _enrollMapper;//Mapper1

        public EnrollmentService(IEnrollmentRepository enrollmentRepository, ILearnerRepository learnerRepository, ICourseRepository courseRepository, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _enrollmentRepository = enrollmentRepository;
            _learnerRepository = learnerRepository;
            _courseRepository = courseRepository;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            //var _configEnrollment = new MapperConfiguration(cfg => cfg.CreateMap<Enrollment, EnrollmentViewModel>().ReverseMap());//mapper 2
            //_enrollMapper = new Mapper(_configEnrollment); //mapper 3
        }

        public async Task<bool> Addenroll(EnrollmentViewModel enrollment)
        {
            bool isEnrolledExists = _enrollmentRepository.AnyEnrollmentByLearnerAndCourse(enrollment.LearnerId, enrollment.CourseId);


            if (!isEnrolledExists)
            {
                Learner learner = _learnerRepository.GetLearnerDetailsByLearnerId(enrollment.LearnerId);

                Course course = _courseRepository.GetCourseDetailsByCourseId(enrollment.CourseId);




                Enrollment newEnrollment = new Enrollment
                {
                    EnrollmentId = Guid.NewGuid(),
                    LearnerId = enrollment.LearnerId,
                    CourseId = enrollment.CourseId,
                    EnrollmentDate = DateTime.Now,
                    EnrollStatus = true,
                    CompletedStatus = 0,
                    CreatedBy = "Admin",
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    ModifiedBy = "Admin"
                };

                _enrollmentRepository.Addenroll(newEnrollment);

                return true;

            }
            else
            {
                return false;
            }

        }

        public object GetCourseandTopicsByLearnerId(Guid learnerId)
        {

            return _enrollmentRepository.GetCourseandTopicsByLearnerId(learnerId);

        }
    }


}
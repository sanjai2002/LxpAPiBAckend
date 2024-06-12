using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data.Entity;
using LXP.Data.Repository;
using Microsoft.AspNetCore.Localization;

namespace LXP.Core.Services
{
    public class LearnerProgressService : ILearnerProgressService
    {
        private readonly ILearnerProgressRepository _learnerProgressRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly ICourseTopicRepository _courseTopicRepository;

        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly ILearnerAttemptRepository _repository;
        private readonly ICourseRepository _courseRepository;

        public LearnerProgressService(ILearnerProgressRepository learnerProgressRepository, IMaterialRepository materialRepository, ICourseTopicRepository courseTopicRepository, IEnrollmentRepository enrollmentRepository, ICourseRepository courseRepository)
        {
            _learnerProgressRepository = learnerProgressRepository;
            _materialRepository = materialRepository;
            _courseTopicRepository = courseTopicRepository;
            _enrollmentRepository = enrollmentRepository;
            _courseRepository = courseRepository;
        }
        public async Task<bool> LearnerProgress(ProgressViewModel learnerProgress)
        {
            var material = await _materialRepository.GetMaterialById(learnerProgress.MaterialId);
            var topic = await _courseTopicRepository.GetTopicByTopicId(material.TopicId);
            var course = _courseRepository.GetCourseDetailsByCourseId(topic.CourseId);
            LearnerProgressViewModel learnerProgressViewModel = new LearnerProgressViewModel()
            {
                MaterialId = material.MaterialId,
                TopicId = topic.TopicId,
                CourseId = course.CourseId,
                WatchTime = learnerProgress.WatchTime,
                LearnerId = learnerProgress.LearnerId
            };
            if (!await _learnerProgressRepository.AnyLearnerProgressByLearnerIdAndMaterialId(learnerProgress.LearnerId, material.MaterialId))
            {
                return await Progress(learnerProgressViewModel);
            }
            return await UpdateProgress(learnerProgress.LearnerId, material.MaterialId, learnerProgress.WatchTime);
        }
        public async Task<LearnerProgress> GetLearnerProgressByLearnerIdAndMaterialId(string LearnerId, string MaterialId)
        {
            return await _learnerProgressRepository.GetLearnerProgressByLearnerIdAndMaterialId(Guid.Parse(LearnerId), Guid.Parse(MaterialId));
        }
        public async Task<bool> Progress(LearnerProgressViewModel learnerProgress)
        {
            var material = await _materialRepository.GetMaterialById(learnerProgress.MaterialId);
            double totalCoursetime = await CourseTotalTime(learnerProgress.CourseId);
            int Coursehours = (int)totalCoursetime;
            int CourseMinutes = (int)((totalCoursetime - Coursehours) * 60);
            TimeOnly CourseDuration = new TimeOnly(Coursehours, CourseMinutes);
            double totalCourseWatchtime = await CourseWatchTime(learnerProgress.CourseId, learnerProgress.LearnerId);
            int CourseWatchhours = (int)totalCourseWatchtime;
            int CourseWatchMinutes = (int)((totalCourseWatchtime - CourseWatchhours) * 60);
            TimeOnly CourseWatchDuration = new TimeOnly(CourseWatchhours, CourseWatchMinutes);
            LearnerProgress progress = new LearnerProgress()
            {
                LearnerProgressId = new Guid(),
                CourseId = learnerProgress.CourseId,
                TopicId = learnerProgress.TopicId,
                MaterialId = learnerProgress.MaterialId,
                LearnerId = learnerProgress.LearnerId,
                WatchTime = learnerProgress.WatchTime,
                TotalTime = CourseDuration,
                IsWatched = 0,
                CourseWatchtime = CourseWatchDuration
            };
            await _learnerProgressRepository.LearnerProgress(progress);
            return true;
        }
        public async Task<bool> materialCompletion(Guid learnerId, Guid courseId)
        {
            var learnerProgress = await _learnerProgressRepository.GetLearnerProgressById(learnerId, courseId);
            double totalCourseWatchtime = await CourseWatchTime(learnerProgress.CourseId, learnerProgress.LearnerId);
            int CourseWatchhours = (int)totalCourseWatchtime;
            int CourseWatchMinutes = (int)((totalCourseWatchtime - CourseWatchhours) * 60);
            TimeOnly CourseWatchDuration = new TimeOnly(CourseWatchhours, CourseWatchMinutes);
            if (learnerProgress.CourseWatchtime == learnerProgress.TotalTime)
            {
                learnerProgress.IsWatched = 1;
            }
            else
            {
                learnerProgress.CourseWatchtime = CourseWatchDuration;
                if (learnerProgress.CourseWatchtime == learnerProgress.TotalTime)
                {
                    learnerProgress.IsWatched = 1;
                }
            }
            _learnerProgressRepository.UpdateLearnerProgress(learnerProgress);
            return true;
        }
        public async Task<bool> UpdateProgress(Guid learnerId, Guid materialId, TimeOnly watchtime)
        {
            var learnermaterial = await _learnerProgressRepository.GetLearnerProgressByMaterialId(learnerId, materialId);
            learnermaterial.WatchTime = watchtime;
            _learnerProgressRepository.UpdateLearnerProgress(learnermaterial);
            await materialCompletion(learnerId, learnermaterial.CourseId);
            return true;
        }

        public async Task<double> TopicTotalTime(Guid topicId)
        {
            var material = await _materialRepository.GetMaterialsByTopic(topicId);
            double totalDuration = material.Sum(m => m.Duration.ToTimeSpan().TotalHours);
            return totalDuration;
        }
        public async Task<double> CourseTotalTime(Guid courseId)
        {
            var topic = await _courseTopicRepository.GetTopicsbycouresId(courseId);
            double courseTotalDuration = 0;
            foreach (var topics in topic)
            {
                Guid topicId = topics.TopicId;
                double topicDuration = await TopicTotalTime(topicId);
                courseTotalDuration += topicDuration;
            }
            return courseTotalDuration;
        }

        public async Task<double> CourseWatchTime(Guid courseId, Guid learnerId)
        {
            var topic = await _courseTopicRepository.GetTopicsbyLearnerId(courseId, learnerId);
            double courseWatchDuration = 0;
            courseWatchDuration = topic.Sum(x => x.WatchTime.ToTimeSpan().TotalHours);
            //Console.WriteLine(courseWatchDuration);
            return courseWatchDuration;
        }



      

        //public async Task<double> materialCompletionPercentage(Guid learnerId, Guid courseId)
        //{
        //    var learnerProgress = await _learnerProgressRepository.GetLearnerProgressById(learnerId, courseId);
        //    TimeSpan timeSpan_total = learnerProgress.TotalTime.ToTimeSpan();
        //    double totaltime = timeSpan_total.TotalHours;

        //    TimeSpan timeSpan_watch = learnerProgress.CourseWatchtime.ToTimeSpan();
        //    double watchtime = timeSpan_watch.TotalHours;

        //    double percentage = (watchtime / totaltime) * 100;
        //    return percentage;


        //}

        
        public async Task<double> TopicWatchTime(Guid topicId, Guid learnerId)
        {
            var materials = await _learnerProgressRepository.GetMaterialByTopic(topicId, learnerId);
            Console.WriteLine(materials);
            double watchDuration = 0;
            foreach (var material in materials)
            {
                Guid materialId = material.MaterialId;
                var materialdetail = await _learnerProgressRepository.GetLearnerProgressByMaterialId(learnerId, materialId);
                double duration = materialdetail.WatchTime.ToTimeSpan().TotalHours;
                watchDuration += duration;

            }
            Console.WriteLine(watchDuration);
            return watchDuration;

        }
       
        //public async  Task<LearnerProgressViewModel> GetProgressById(Guid learnerProgressId)
        // {
        //    LearnerProgressViewModel  progress = await _learnerProgressRepository.GetLearnerProgressById(learnerProgressId);
        //     return progress;
        // }

        public async Task CalculateAndUpdateCourseCompletionAsync(Guid learnerId)
        {
            await _learnerProgressRepository.CalculateAndUpdateCourseCompletionAsync(learnerId);
        }

        public async Task<decimal?> GetCourseCompletionPercentageAsync(Guid learnerId, Guid enrollmentId)
        {
            var enrollment = await _learnerProgressRepository.GetEnrollmentByIdAsync(learnerId, enrollmentId);
            return enrollment?.CourseCompletionPercentage;
        }

        
    }
}
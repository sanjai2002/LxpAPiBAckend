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

namespace LXP.Core.Services
{
    public class LearnerProgressService : ILearnerProgressService
    {
        private readonly ILearnerProgressRepository _learnerProgressRepository;
        private readonly IMaterialRepository _materialRepository;

        public LearnerProgressService(ILearnerProgressRepository learnerProgressRepository,IMaterialRepository materialRepository)
        {
            _learnerProgressRepository = learnerProgressRepository;
            _materialRepository = materialRepository;
        }
       

      public async Task Progress(LearnerProgressViewModel learnerProgress)
        {
            var material = await _materialRepository.GetMaterialById(learnerProgress.MaterialId);


            LearnerProgress progress = new LearnerProgress()
            {
                LearnerProgressId = new Guid(),
                CourseId = learnerProgress.CourseId,
                TopicId = learnerProgress.TopicId,
                MaterialId = learnerProgress.MaterialId,
                LearnerId = learnerProgress.LearnerId,
                WatchTime = learnerProgress.WatchTime,
                TotalTime = material.Duration,
                IsWatched = false

            };
            

            await _learnerProgressRepository.LearnerProgress(progress);
            
            
            
            
            //double watchTimeInMinutes = progress.WatchTime.Hour * 60 + progress.WatchTime.Minute;
            //double totalTimeInMinutes = progress.TotalTime.Hour * 60 + progress.TotalTime.Minute;

            //double percentage = (watchTimeInMinutes / totalTimeInMinutes) * 100;
            //return percentage;

        }

       public async Task materialCompletion(Guid learnerId, Guid materialId)
        {
            var learnerProgress = await _learnerProgressRepository.GetLearnerProgressById(learnerId,materialId);

            if(learnerProgress.WatchTime==learnerProgress.TotalTime)
            {


                learnerProgress.IsWatched = true;
               
                
            }
            _learnerProgressRepository.UpdateLearnerProgress(learnerProgress);

        }

       //public async  Task<LearnerProgressViewModel> GetProgressById(Guid learnerProgressId)
       // {
       //    LearnerProgressViewModel  progress = await _learnerProgressRepository.GetLearnerProgressById(learnerProgressId);
       //     return progress;
       // }
    }
}

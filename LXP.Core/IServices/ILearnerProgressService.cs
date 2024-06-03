using LXP.Common.Entities;
using LXP.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.IServices
{
    public interface ILearnerProgressService
    {
        Task Progress(LearnerProgressViewModel learnerProgress);
        Task materialCompletion(Guid learnerId,Guid materialId);
       // Task<LearnerProgressViewModel> GetProgressById(Guid learnerProgressId);
    }
}

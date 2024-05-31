using LXP.Common.Entities;
using LXP.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.IServices
{
    public interface ILearnerService
    {
        Task<bool> LearnerRegistration(RegisterUserViewModel registerUserViewModel);

      

        Task<List<GetLearnerViewModel>> GetAllLearner();

        //Task<List<Learner>>Updateall

        Learner GetLearnerById(string id);
    }
}









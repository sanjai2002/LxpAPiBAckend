using LXP.Common.Entities;
using LXP.Common.ViewModels;

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









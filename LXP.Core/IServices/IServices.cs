using LXP.Common.ViewModels;

namespace LXP.Core.IServices
{
    public interface IService
    {
        public Task<LoginRole> CheckLearner(LoginModel loginmodel);


        Task<bool> ForgetPassword(string Email);


        Task<ResultUpdatePassword> UpdatePassword(UpdatePassword updatePassword);

    }
}







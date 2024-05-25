using LXP.Common.ViewModels;

namespace LXP.Core.IServices
{
    public interface ILoginService
    {
        public Task<LoginRole> LoginLearner(LoginModel loginmodel);


        //Task<bool> ForgetPassword(string Email);


        //Task<ResultUpdatePassword> UpdatePassword(UpdatePassword updatePassword);

    }
}







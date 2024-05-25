using LXP.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.IServices
{
     public  interface IService
    {
        public Task<LoginRole> CheckLearner(LoginModel loginmodel);


        Task<bool> ForgetPassword(string Email);


        Task<ResultUpdatePassword> UpdatePassword(UpdatePassword updatePassword);

    }
}







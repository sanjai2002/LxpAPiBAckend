using LXP.Common.ViewModels;

namespace LXP.Core.IServices
{
    public interface IUpdatePasswordService
    {
        Task<ResultUpdatePassword> UpdatePassword(UpdatePassword updatePassword);
    }
}
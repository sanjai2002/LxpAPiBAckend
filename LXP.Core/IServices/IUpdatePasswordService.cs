using LXP.Common.ViewModels;

namespace LXP.Core.IServices
{
    public interface IUpdatePasswordService
    {
        Task<bool> UpdatePassword(UpdatePassword updatePassword);
    }
}

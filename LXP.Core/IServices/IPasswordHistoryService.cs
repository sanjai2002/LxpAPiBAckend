namespace LXP.Core.IServices
{
    public interface IPasswordHistoryService
    {
        Task<bool> UpdatePassword(string learnerId, string oldPassword, string newPassword);
    }
}

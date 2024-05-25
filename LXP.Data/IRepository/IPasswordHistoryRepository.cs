using LXP.Common.Entities;

namespace LXP.Data.IRepository
{
    public interface IPasswordHistoryRepository
    {
        Task<PasswordHistory> GetPasswordHistory(Guid learnerId);
        Task UpdatePasswordHistory(PasswordHistory passwordHistory);
    }
}

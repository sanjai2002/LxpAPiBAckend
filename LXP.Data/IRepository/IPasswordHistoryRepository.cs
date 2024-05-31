using LXP.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.IRepository
{
    public interface IPasswordHistoryRepository
    {
        Task<PasswordHistory> GetPasswordHistory(Guid learnerId);
        Task UpdatePasswordHistory(PasswordHistory passwordHistory);
    }
}

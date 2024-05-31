using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.IServices
{
    public interface IPasswordHistoryService
    {
        Task<bool> UpdatePassword(string learnerId, string oldPassword, string newPassword);
    }
}

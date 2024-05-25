using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.IServices
{
    public interface IForgetService
    {
        bool ForgetPassword(string Email);

    }
}

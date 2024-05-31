//using LXP.Common.Entities;
//using LXP.Common.ViewModels;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LXP.Core.IServices
//{
//    public interface IProfileService
//    {

//        Task<List<GetProfileViewModel>> GetAllLearnerProfile();
//        LearnerProfile GetLearnerProfileById(string id);
//    }
//}


using LXP.Common.Entities;
using LXP.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.IServices
{
    public interface IProfileService
    {

        Task<List<GetProfileViewModel>> GetAllLearnerProfile();
        LearnerProfile GetLearnerProfileById(string id);

        Task UpdateProfile(UpdateProfileViewModel model);
    }
}
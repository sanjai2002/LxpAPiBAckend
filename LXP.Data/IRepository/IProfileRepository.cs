//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using LXP.Common.Entities;

//namespace LXP.Data.IRepository
//{
//    public interface IProfileRepository
//    {
//        public void AddProfile(LearnerProfile learnerprofile);

//        Task<List<LearnerProfile>> GetAllLearnerProfile();

        
//        LearnerProfile GetLearnerprofileDetailsByLearnerprofileId(Guid ProfileId);
//    }
//}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LXP.Common.Entities;

namespace LXP.Data.IRepository
{
    public interface IProfileRepository
    {
        public void AddProfile(LearnerProfile learnerprofile);

        Task<List<LearnerProfile>> GetAllLearnerProfile();
        LearnerProfile GetLearnerprofileDetailsByLearnerprofileId(Guid ProfileId);
        Task UpdateProfile(LearnerProfile learnerProfile);
    }
}

//using LXP.Data.DBContexts;
//using LXP.Data.IRepository;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using LXP.Common.Entities;
//using System.Runtime.InteropServices;

//namespace LXP.Data.Repository
//{
//    public class ProfileRepository : IProfileRepository
//    {
//        private readonly LXPDbContext _LXPDbContext;
//        public ProfileRepository(LXPDbContext context)
//        {
//            _LXPDbContext = context;
//        }
//        public void AddProfile(LearnerProfile learnerprofile)
//        {
            
            
//            _LXPDbContext.LearnerProfiles.Add(learnerprofile);
//            _LXPDbContext.SaveChanges();
//        }

//        public async Task<List<LearnerProfile>> GetAllLearnerProfile()
//        {
//            return _LXPDbContext.LearnerProfiles.ToList();
//        }
//        public async Task UpdateAllLearnerProfile(LearnerProfile learnerProfile)
//        {
//            _LXPDbContext.Entry(learnerProfile).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
//            await _LXPDbContext.SaveChangesAsync();
//        }



//public LearnerProfile GetLearnerprofileDetailsByLearnerprofileId(Guid ProfileId)

//        {

//            return _LXPDbContext.LearnerProfiles.Find(ProfileId);


//        }


//    }
//}






using LXP.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LXP.Common.Entities;
using System.Runtime.InteropServices;

namespace LXP.Data.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly LXPDbContext _LXPDbContext;
        public ProfileRepository(LXPDbContext context)
        {
            _LXPDbContext = context;
        }
        public void AddProfile(LearnerProfile learnerprofile)
        {


            _LXPDbContext.LearnerProfiles.Add(learnerprofile);
            _LXPDbContext.SaveChanges();
        }

        public async Task<List<LearnerProfile>> GetAllLearnerProfile()
        {
            return _LXPDbContext.LearnerProfiles.ToList();
        }
        public async Task UpdateAllLearnerProfile(LearnerProfile learnerProfile)
        {
            _LXPDbContext.Entry(learnerProfile).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _LXPDbContext.SaveChangesAsync();
        }



        public LearnerProfile GetLearnerprofileDetailsByLearnerprofileId(Guid ProfileId)

        {

            return _LXPDbContext.LearnerProfiles.Find(ProfileId);


        }


        public async Task UpdateProfile(LearnerProfile learnerProfile)
        {
            _LXPDbContext.Entry(learnerProfile).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _LXPDbContext.SaveChangesAsync();
        }


    }
}
using LXP.Common;
using LXP.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LXP.Data.DBContexts;
namespace LXP.Data.Repository
{
    public class UpdatePasswordRepository:IUpdatePasswordRepository
    {

        private readonly LXPDbContext _dbcontext;


        public UpdatePasswordRepository(LXPDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }





        public async Task<Learner> LearnerByEmailAndPassword(string Email, string Password)

        {
            return _dbcontext.Learners.FirstOrDefault(learner => learner.Email == Email && learner.Password == Password);
        }


        public void UpdatePassword(Learner learner)
        {
            _dbcontext.Learners.Update(learner);

             _dbcontext.SaveChangesAsync();
        }



    }
}

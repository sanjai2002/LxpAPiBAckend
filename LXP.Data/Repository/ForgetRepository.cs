using LXP.Common;
using LXP.Common.Entities;
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
    public class ForgetRepository: IForgetRepository
    {
        private readonly LXPDbContext _dbcontext;


        public ForgetRepository(LXPDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }


        public bool AnyUserByEmail(string loginmodel)
        {
            return  _dbcontext.Learners.Any(learner => learner.Email == loginmodel);
        }
        //public async Task<bool> AnyLearnerByEmailAndPassword(string Email, string Password)
        //{
        //    return await _dbcontext.Learners.AnyAsync(learner => learner.Email == Email && learner.Password == Password);
        //}
        public  Learner GetLearnerByEmail(string Email)
        {
            return  _dbcontext.Learners.FirstOrDefault(learner => learner.Email == Email);
        }


        public  void UpdateLearnerPassword(string Email, string Password)
        {
            Learner learner =  GetLearnerByEmail(Email);
            learner.Password = Password;
            _dbcontext.Learners.Update(learner);
             _dbcontext.SaveChangesAsync();
        }


        //public async Task UpdatePassword(Learner learner)
        //{
        //    _dbcontext.Learners.Update(learner);

        //    await _dbcontext.SaveChangesAsync();
        //}



        //public async Task<Learner> LearnerByEmailAndPassword(string Email, string Password)

        //{
        //    return await _dbcontext.Learners.FirstOrDefaultAsync(learner => learner.Email == Email && learner.Password == Password);
        //}
    }
}

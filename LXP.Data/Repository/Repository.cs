using LXP.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace LXP.Data.Repository
{
    public class Repository : IRepository.IRepository
    {

        private readonly LXPDbContext _dbcontext;


        public Repository(LXPDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }


        public async Task CheckLearner(Learner learner)
        {

            var db = new LXPDbContext();

            await db.Learners.AddAsync(learner);

            await db.SaveChangesAsync();

        }

        public async Task<bool> AnyUserByEmail(string loginmodel)
        {
            return _dbcontext.Learners.Any(learner => learner.Email == loginmodel);
        }
        public async Task<bool> AnyLearnerByEmailAndPassword(string Email, string Password)
        {
            return await _dbcontext.Learners.AnyAsync(learner => learner.Email == Email && learner.Password == Password);
        }
        public async Task<Learner> GetLearnerByEmail(string Email)
        {
            return await _dbcontext.Learners.FirstOrDefaultAsync(learner => learner.Email == Email);
        }


        public async Task UpdateLearnerPassword(string Email, string Password)
        {
            Learner learner = await GetLearnerByEmail(Email);
            learner.Password = Password;
            _dbcontext.Learners.Update(learner);
            await _dbcontext.SaveChangesAsync();
        }


        public async Task UpdatePassword(Learner learner)
        {
            _dbcontext.Learners.Update(learner);

            await _dbcontext.SaveChangesAsync();
        }



        public async Task<Learner> LearnerByEmailAndPassword(string Email, string Password)

        {
            return await _dbcontext.Learners.FirstOrDefaultAsync(learner => learner.Email == Email && learner.Password == Password);
        }


    }
}


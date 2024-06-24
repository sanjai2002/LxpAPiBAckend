using LXP.Common.Entities;
using LXP.Common.Entities;
using LXP.Data.IRepository;
using LXP.Data.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LXP.Data.Repository
{
    public class UpdatePasswordRepository : IUpdatePasswordRepository
    {
        private readonly LXPDbContext _dbcontext;

        public UpdatePasswordRepository(LXPDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Learner> LearnerByEmailAndPasswordAsync(string email, string password)
        {
            return await _dbcontext.Learners.FirstOrDefaultAsync(learner =>
                learner.Email == email && learner.Password == password
            );
        }

        public async Task UpdatePasswordAsync(Learner learner)
        {
            _dbcontext.Learners.Update(learner);
            await _dbcontext.SaveChangesAsync();
        }
    }
}

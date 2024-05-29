using LXP.Common.Entities;
using LXP.Data.IRepository;
namespace LXP.Data.Repository
{
    public class UpdatePasswordRepository : IUpdatePasswordRepository
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

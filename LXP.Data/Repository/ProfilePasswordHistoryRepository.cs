using LXP.Common.Entities;
using LXP.Data.IRepository;

namespace LXP.Data.Repository
{
    public class ProfilePasswordHistoryRepository : IProfilePasswordHistoryRepository
    {

        private readonly LXPDbContext _lXPDbContext;
        public ProfilePasswordHistoryRepository(LXPDbContext context)
        {

            _lXPDbContext = context;
        }

        public void AddPasswordHistory1(PasswordHistory passwordHistory)
        {

            _lXPDbContext.PasswordHistories.Add(passwordHistory);


            _lXPDbContext.SaveChanges();



        }
    }
}
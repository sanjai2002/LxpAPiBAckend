using LXP.Common.Entities;
using LXP.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
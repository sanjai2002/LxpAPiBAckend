
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.IRepository
{
    public interface IForgetRepository
    {
        //public Task CheckLearner(Learner loginmodel);

        public bool AnyUserByEmail(string loginmodel);


        //public Task<bool> AnyLearnerByEmailAndPassword(string Email, string Password);

        public Learner GetLearnerByEmail(string Email);


        public void UpdateLearnerPassword(string Email, string Password);

        //public Task UpdatePassword(Learner learner);


        //public Task<Learner> LearnerByEmailAndPassword(string Email, string Password);
    }
}

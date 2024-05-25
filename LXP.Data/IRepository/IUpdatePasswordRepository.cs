using LXP.Common.Entities;

namespace LXP.Data.IRepository
{
    public interface IUpdatePasswordRepository
    {


        //public Task<bool> AnyUserByEmail(string loginmodel);


        //public Task<bool> AnyLearnerByEmailAndPassword(string Email, string Password);

        //public Task<Learner> GetLearnerByEmail(string Email);


        //public Task UpdateLearnerPassword(string Email, string Password);

        public void UpdatePassword(Learner learner);


        Task<Learner> LearnerByEmailAndPassword(string Email, string Password);
    }
}

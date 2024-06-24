using LXP.Common.Entities;
using LXP.Common.Entities;

namespace LXP.Data.IRepository
{
    public interface IUpdatePasswordRepository
    {
        //public Task<bool> AnyUserByEmail(string loginmodel);


        //public Task<bool> AnyLearnerByEmailAndPassword(string Email, string Password);

        //public Task<Learner> GetLearnerByEmail(string Email);


        //public Task UpdateLearnerPassword(string Email, string Password);
        //  Task<bool> Changecoursestatus(Coursestatus status);

        public Task UpdatePasswordAsync(Learner learner);
        Task<Learner> LearnerByEmailAndPasswordAsync(string email, string password);
    }

    // public void LearnerByEmailAndPasswordAsync(Learner learner);

    // Learner UpdatePasswordAsync(string Email, string Password);
}


using LXP.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LXP.Common.Entities;

namespace LXP.Data.IRepository
{
    public  interface IRepository
    {

       public Task CheckLearner(Learner loginmodel);

       public  Task<bool> AnyUserByEmail(string loginmodel);


       public Task<bool> AnyLearnerByEmailAndPassword(string Email, string Password);

       public  Task<Learner> GetLearnerByEmail(string Email);


        public Task UpdateLearnerPassword(string Email, string Password);

        public  Task UpdatePassword(Learner learner);


        public Task<Learner> LearnerByEmailAndPassword(string Email, string Password);



    }
}

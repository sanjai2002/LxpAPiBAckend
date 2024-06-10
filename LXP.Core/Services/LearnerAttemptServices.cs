using LXP.Core.IServices;
using LXP.Data.IRepository;
using LXP.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.Services
{
    public  class LearnerAttemptServices : ILearnerAttemptServices
    {
        private readonly ILearnerAttemptRepository _repository;


        public LearnerAttemptServices(ILearnerAttemptRepository repository)
        {
            _repository = repository;
        }


       public  object GetScoreByTopicIdAndLernerId (string topicId, string LearnerId)
        {
            return _repository.GetScoreByTopicIdAndLernerId(Guid.Parse(topicId), Guid.Parse(LearnerId));


        }


        public object GetScoreByLearnerId(string LearnerId)
        {
            return _repository.GetScoreByLearnerId(Guid.Parse(LearnerId));
        }
    }


   
}

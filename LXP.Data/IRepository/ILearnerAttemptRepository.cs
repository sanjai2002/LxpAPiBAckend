using LXP.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.IRepository
{
    public interface  ILearnerAttemptRepository
    {
        object GetScoreByTopicIdAndLernerId (Guid topicId, Guid LearnerId);

        object GetScoreByLearnerId(Guid LearnerId);
    }

}

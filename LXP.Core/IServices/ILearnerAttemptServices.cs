using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.IServices
{
    public interface  ILearnerAttemptServices
    {
        object GetScoreByTopicIdAndLernerId(string topicId, string LearnerId);

        object GetScoreByLearnerId(string LearnerId);
    }
}

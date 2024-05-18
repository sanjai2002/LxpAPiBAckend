using LXP.Common.ViewModels;
using LXP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.IServices
{
    public interface ILearnerServices
    {
        object GetAllLearnerDetailsByLearnerId(Guid learnerid);

        object GetLearnerEnrolledcourseByLearnerId(Guid learnerid);

     



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.IRepository
{
    public  interface ILearnerRepository
    {

        object GetAllLearnerDetailsByLearnerId(Guid learnerId);

        object GetLearnerEnrolledcourseByLearnerId(Guid learnerId);



    }
}

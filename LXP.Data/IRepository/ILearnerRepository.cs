using LXP.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.IRepository
{
    public  interface ILearnerRepository
    {

        public IEnumerable<AllLearnersViewModel> GetLearners();

        object GetAllLearnerDetailsByLearnerId(Guid learnerId);

        object GetLearnerEnrolledcourseByLearnerId(Guid learnerId);

    }
}

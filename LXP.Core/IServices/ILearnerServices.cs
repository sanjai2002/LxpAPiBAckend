using LXP.Common.ViewModels;

namespace LXP.Core.IServices
{
    public interface ILearnerServices
    {
        public IEnumerable<AllLearnersViewModel> GetLearners();
        object GetAllLearnerDetailsByLearnerId(Guid learnerid);

        object GetLearnerEnrolledcourseByLearnerId(Guid learnerid);







    }
}

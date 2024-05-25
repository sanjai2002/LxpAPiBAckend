using LXP.Common.Entities;
using LXP.Common.ViewModels;

namespace LXP.Data.IRepository
{
    public interface ILearnerRepository
    {
        Task  AddLearner(Learner learner);
        Task<bool> AnyLearnerByEmail(string email);
        Learner GetLearnerByLearnerEmail(string email);

        Task<List<Learner>> GetAllLearner();

        //Task UpdateAllLearner(Learner learner);

        Learner GetLearnerDetailsByLearnerId(Guid LearnerId);

        Task UpdateLearner(Learner learner);

        public IEnumerable<AllLearnersViewModel> GetLearners();

        object GetAllLearnerDetailsByLearnerId(Guid learnerId);

        object GetLearnerEnrolledcourseByLearnerId(Guid learnerId);

    }
}

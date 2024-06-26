using LXP.Common.Entities;
using LXP.Common.ViewModels;

namespace LXP.Core.IServices
{
    public interface IProfileService
    {
        Task<List<GetProfileViewModel>> GetAllLearnerProfile();
        LearnerProfile GetLearnerProfileById(string id);

        Task UpdateProfile(UpdateProfileViewModel model);

        Guid GetprofileId(Guid learnerId);
    }
}

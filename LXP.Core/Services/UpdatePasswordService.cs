using LXP.Common.Utils;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;

namespace LXP.Core.Services
{
    public class UpdatePasswordService : IUpdatePasswordService
    {
        private readonly IUpdatePasswordRepository _repository;

        public UpdatePasswordService(IUpdatePasswordRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> UpdatePassword(UpdatePassword updatePassword)
        {
            var learner = await _repository.LearnerByEmailAndPasswordAsync(
                updatePassword.Email,
                Encryption.ComputePasswordToSha256Hash(updatePassword.OldPassword)
            );

            if (learner == null)
            {
                return false;
            }

            string encryptNewPassword = Encryption.ComputePasswordToSha256Hash(
                updatePassword.NewPassword
            );
            learner.Password = encryptNewPassword;

            await _repository.UpdatePasswordAsync(learner);
            return true;
        }
    }
}

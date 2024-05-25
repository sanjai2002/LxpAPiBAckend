using LXP.Common.Entities;
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

        public async Task<ResultUpdatePassword> UpdatePassword(UpdatePassword updatePassword)

        {
            Learner learner = await _repository.LearnerByEmailAndPassword(updatePassword.Email, Encryption.ComputePasswordToSha256Hash(updatePassword.OldPassword));
            var result = new ResultUpdatePassword();

            if (learner.Password == Encryption.ComputePasswordToSha256Hash(updatePassword.OldPassword))
            {
                string encryptNewPassword = Encryption.ComputePasswordToSha256Hash(updatePassword.NewPassword);
                learner.Password = encryptNewPassword;
                _repository.UpdatePassword(learner);
                result.success = true;
                return result;
            }

            else
            {
                return result;

            }

        }



    }
}

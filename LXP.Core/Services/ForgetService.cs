using LXP.Common.Utils;
using LXP.Core.IServices;
using LXP.Data.IRepository;

namespace LXP.Core.Services
{
    public class ForgetService : IForgetService


    {
        private readonly IForgetRepository _repository;


        public ForgetService(IForgetRepository repository)
        {
            _repository = repository;

        }

        public bool ForgetPassword(string Email)

        {
            var getleareremail = _repository.AnyUserByEmail(Email);

            if (getleareremail != null)
            {

                string password = RandomPassword.Randompasswordgenerator();
                string encryptPassword = Encryption.ComputePasswordToSha256Hash(password);

                _repository.UpdateLearnerPassword(Email, encryptPassword);

                EmailGenerator.Sendpassword(password, Email);

                return true;
            }

            else
            {
                return false;
            }


        }

    }
}

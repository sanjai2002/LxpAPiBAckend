using AutoMapper;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using System.Security.Cryptography;
using System.Text;
namespace LXP.Core.Services
{
    public class LoginService : ILoginService
    {

        private readonly ILoginRepository _repository;

        private Mapper _moviemapper;

        public LoginService(ILoginRepository repository)
        {
            _repository = repository;

            var _configlogin = new MapperConfiguration(cfg => cfg.CreateMap<Learner, LoginModel>());

            _moviemapper = new Mapper(_configlogin);
        }



        public async Task<LoginRole> LoginLearner(LoginModel loginmodel)

        {
            LoginRole loginRole;

            LoginRole message = new LoginRole();


            var getlearners = await _repository.GetLearnerByEmail(loginmodel.Email);

            var user = await _repository.AnyUserByEmail(loginmodel.Email);


            if (user == true)
            {


                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] inputHashPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(loginmodel.Password));

                    StringBuilder stringBuilder = new StringBuilder();

                    for (int i = 0; i < inputHashPassword.Length; i++)

                    {

                        stringBuilder.Append(inputHashPassword[i].ToString("x2"));

                    }

                    string inputPasswordHashed = stringBuilder.ToString();


                    bool checkpassword = await _repository.AnyLearnerByEmailAndPassword(loginmodel.Email, inputPasswordHashed);

                    message.Role = getlearners.Role;

                    message.AccountStatus = getlearners.AccountStatus;

                    if (checkpassword)


                    {

                        await _repository.UpdateLearnerLastLogin(loginmodel.Email);


                        loginRole = new LoginRole();

                        {
                            loginRole.Email = true;

                            loginRole.Password = true;

                            loginRole.Role = message.Role;

                            loginRole.AccountStatus = message.AccountStatus;

                            loginRole.LastLogin = DateTime.Now;



                        }

                        return loginRole;
                    }

                    else
                    {
                        loginRole = new LoginRole();

                        {
                            loginRole.Email = true;

                            loginRole.Password = false;
                        }
                        return loginRole;
                    }

                }

            }

            else
            {
                loginRole = new LoginRole();

                {
                    loginRole.Email = false;

                    loginRole.Password = false;

                }
                return loginRole;


            }


        }


        //public async Task<bool> ForgetPassword(string Email)

        //{


        //    var getleareremail = await _repository.AnyUserByEmail(Email);


        //    if (getleareremail == true)
        //    {

        //        string password = RandomPassword.Randompasswordgenerator();
        //        string encryptPassword = Encryption.ComputePasswordToSha256Hash(password);
        //        _repository.UpdateLearnerPassword(Email, encryptPassword);
        //        EmailGenerator.Sendpassword(password, Email);
        //        return true;
        //    }


        //    else
        //    {
        //        return false;
        //    }


        //}




        //public async Task<ResultUpdatePassword> UpdatePassword(UpdatePassword updatePassword)
        //{
        //    var learner = await _repository.LearnerByEmailAndPassword(updatePassword.Email, Encryption.ComputePasswordToSha256Hash(updatePassword.OldPassword));
        //    var result = new ResultUpdatePassword();
        //    if (learner.Password== Encryption.ComputePasswordToSha256Hash(updatePassword.OldPassword))
        //    {
        //        string encryptNewPassword = Encryption.ComputePasswordToSha256Hash(updatePassword.NewPassword);
        //        learner.Password = encryptNewPassword; 
        //        await _repository.UpdatePassword(learner);
        //        result.success= true;
        //        return result;
        //    }

        //    else
        //    {
        //        return result;

        //    }

        //}


    }

}
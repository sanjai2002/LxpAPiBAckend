using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Collections.Concurrent;
using System.Collections;
using LXP.Common.Constants;

namespace LXP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : BaseController
    {
        private readonly ILearnerService _learnerServices;
        private readonly IProfileService _profileService;
        private readonly IPasswordHistoryService _passwordHistoryService;
        private static DateTime _currentTIme;//Raj
        public readonly Hashtable _otpTable = new Hashtable();
        private static ConcurrentDictionary<string, string> emailOtpMap = new ConcurrentDictionary<string, string>();//Raj


        public RegistrationController(ILearnerService learnerServices, IProfileService profileService, IPasswordHistoryService passwordHistoryService)
        {
            _learnerServices=learnerServices;
            _profileService=profileService;
            _passwordHistoryService=passwordHistoryService;
        }

        ///<summary>
        ///Post the learner and profile details 
        ///</summary>
        ///
        [HttpPost("/lxp/learner/registration")]
        public  async Task<IActionResult> Registration( [FromBody] RegisterUserViewModel learner)
        {
            bool learnerservices= await _learnerServices.LearnerRegistration(learner);
            if (learnerservices)
            {
                return Ok(CreateSuccessResponse(MessageConstants.MsgLearnerRegistrationSuccess));
            }
            else
            {
                return  Ok(CreateFailureResponse(MessageConstants.MsgLearnerAlreadyExists, 400));
            }
        }
        ///<summary>
        ///Fetch all the learner details 
        ///</summary>
        ///
        [HttpGet("/lxp/view/learner")]
        public async Task<IActionResult> GetAllCategory()
        {
            List<GetLearnerViewModel> categories = await _learnerServices.GetAllLearner();
            return Ok(CreateSuccessResponse(categories));
        }

        ///<summary>
        ///Fetch all the learner profle details
        ///</summary>
        [HttpGet("/lxp/view/learnerProfile")]
        public async Task<IActionResult> GetAllLearnerProfile()
        {
            List<GetProfileViewModel> LearnerProfileone = await _profileService.GetAllLearnerProfile();
            return Ok(CreateSuccessResponse(LearnerProfileone));
        }

        ///<summary>
        ///Fetching particular Learner profile details using Id
        ///</summary>
        [HttpGet("/lxp/view/learnerProfile/{id}")]
        public async Task<IActionResult> GetLearnerProfileById(string id)
        {
            LearnerProfile LearnerProfileone = _profileService.GetLearnerProfileById(id);
            return Ok(CreateSuccessResponse(LearnerProfileone));
        }

        ///<summary>
        ///Fetching particular Learner details using Learner Id
        ///</summary>
        [HttpGet("/lxp/view/learner/{id}")]
        public async Task<IActionResult> GetLearnerById(string id)
        {
            Learner LearnerProfileone = _learnerServices.GetLearnerById(id);
            return Ok(CreateSuccessResponse(LearnerProfileone));
        }






        //Raj   Controller

        ///<summary>
        ///Generating email to the repected mail they entered
        ///</summary>
        [HttpPost("EmailVerification")]
        public IActionResult GenerateOTP([FromQuery] string email)
        {
            // Generate a random OTP
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string sOTP = "";
            Random rand = new Random();

            for (int i = 0; i < 6; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sOTP += saAllowedCharacters[p];
            }

            // Store the OTP data in the Hashtable
            //var otpData = new OtpData
            //{
            //    Otp = sOTP,
            //    Timestamp = DateTime.Now,
            //    Email = email
            //};
            emailOtpMap[email] = sOTP;

            // Configure email settings
            string sender = "rajkumarprofo@gmail.com"; // Replace with your Gmail address
            string senderPass = "mdjc ubpu wnse bjno"; // Replace with your Gmail password
            string subject = "LXP Email Verification";
            string body = $"The OTP to Verify Your Email is: {sOTP}";

            // Create and send the email
            using (var mail = new MailMessage(sender, email))
            {
                mail.Subject = subject;
                mail.Body = body;

                using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential(sender, senderPass);
                    smtpClient.EnableSsl = true;

                    try
                    {
                        smtpClient.Send(mail);
                        _currentTIme = DateTime.Now;
                        Console.WriteLine("Email Sent Successfully");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error sending email: {e.Message}");
                    }
                }
            }

            return Ok(emailOtpMap);
        }








        //[HttpGet("VerifyOTP")]
        //public IActionResult VerifyOTP([FromQuery] string email, [FromQuery] string userOTP)
        //{
        //    return Ok(emailOtpMap[email]);
        //    if (emailOtpMap.ContainsKey(email))
        //    {
        //        var otpData = emailOtpMap[email];
        //        //var storedTimestamp = otpData.Timestamp;
        //        //var currentTimestamp = DateTime.Now;

        //        // Check if the OTP is still valid (within 2 minutes)
        //        //var timeDifference = currentTimestamp - storedTimestamp;

        //        if (otpData == userOTP) {
        //            Console.WriteLine($"OTP verified successfully for email: {email}");
        //            return Ok("OTP verified successfully!");
        //        }
        //        //if (timeDifference.TotalMinutes <= 2 && otpData.Otp == userOTP)
        //        //{
        //        //    // Valid OTP
        //        //    /*_otpTable.Remove(email);*/ // Remove the used OTP data
        //        //    Console.WriteLine($"OTP verified successfully for email: {email}");
        //        //    return Ok("OTP verified successfully!");
        //        //}
        //        else
        //        {
        //            // Expired or invalid OTP
        //            Console.WriteLine($"Invalid OTP provided or OTP has expired for email: {email}");
        //            return BadRequest("Invalid OTP provided or OTP has expired.");
        //        }
        //    }
        //    else
        //    {
        //        // OTP data not found for the provided email
        //        Console.WriteLine($"No OTP data found for the provided email: {email}");
        //        return BadRequest("No OTP data found for the provided email.");
        //    }
        //}

        static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTimeStamp).ToLocalTime();
        }

        ///<summary>
        ///verifying the OTP by entering the email Id
        ///</summary>
        [HttpGet("VerifyOTP")]
        public IActionResult VerifyOTP([FromQuery] string email, [FromQuery] string userOTP)
        {
            //return Ok(emailOtpMap["rajkumar08102001@gmail.com"]);
            if (emailOtpMap.ContainsKey(email))
            {
                var otpData = emailOtpMap[email];
                DateTime storedTimestamp = _currentTIme;
                DateTime currentTimestamp = DateTime.Now;
                // Check if the OTP is still valid (within 2 minutes)
                TimeSpan timeDifference = currentTimestamp - storedTimestamp;


                //if (otpData == userOTP)
                //{
                //    Console.WriteLine($"OTP verified successfully for email: {email}");
                //    return Ok("OTP verified successfully!");
                //
                double num = timeDifference.TotalMinutes;
                if (timeDifference.TotalMinutes < 2)
                {
                    if (otpData == userOTP)
                    {
                        string removeEmail = "";
                        emailOtpMap.Remove(email, out removeEmail);
                        Console.WriteLine($"OTP verified successfully for email: {email}");

                        return Ok("OTP verified successfully!");
                    }
                    else
                    {
                        // Expired or invalid OTP
                        Console.WriteLine($"Invalid OTP provided for email: {email}");
                        return BadRequest("Invalid OTP provided.");
                    }

                }
                else
                {
                    // Expired or invalid OTP
                    string removeEmail = "";
                    emailOtpMap.Remove(email, out removeEmail);
                    Console.WriteLine($"OTP has expired for email: {email}");
                    return BadRequest("OTP has expired.");
                }
            }
            else
            {
                // OTP data not found for the provided email
                Console.WriteLine($"No OTP data found for the provided email: {email}");
                return BadRequest("No OTP data found for the provided email.");
            }
        }



        //[HttpGet("VerifyOTP")]
        //public IActionResult VerifyOTP([FromQuery] string email, [FromQuery] string userOTP)
        //{
        //    if (_otpTable.ContainsKey(email))
        //    {
        //        var otpData = (OtpData)_otpTable[email];
        //        var storedTimestamp = otpData.Timestamp;
        //        var currentTimestamp = DateTime.Now;

        //        // Check if the OTP is still valid (within 2 minutes)
        //        var timeDifference = currentTimestamp - storedTimestamp;
        //        if (timeDifference.TotalMinutes <= 2 && otpData.Otp == userOTP)
        //        {
        //            // Valid OTP
        //            _otpTable.Remove(email); // Remove the used OTP data
        //            return Ok("OTP verified successfully!");
        //        }
        //        else
        //        {
        //            // Expired or invalid OTP
        //            return BadRequest("Invalid OTP provided or OTP has expired.");
        //        }
        //    }

        //    // OTP data not found
        //    return BadRequest("No OTP data found for the provided email.");
        //}


















        //[HttpPost("Email Verification")]
        //public IActionResult GenerateOTP([FromQuery] string email)
        //{
        //    string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
        //    string sOTP = String.Empty;

        //    string sTempChars = String.Empty;

        //    Random rand = new Random();

        //    for (int i = 0; i < 6; i++)

        //    {

        //        int p = rand.Next(0, saAllowedCharacters.Length);

        //        sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

        //        sOTP += sTempChars;

        //    }

        //    string sender = "rajkumarprofo@gmail.com";
        //    string senderPass = "mdjc ubpu wnse bjno";
        //    string recieve = email;

        //    MailMessage mail = new MailMessage(sender, recieve);
        //    mail.Subject = "LXP Email Verification";
        //    mail.Body = $"The OTP to Verify Your Email is: {sOTP}";

        //    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
        //    smtpClient.Port = 587;
        //    smtpClient.Credentials = new NetworkCredential(sender, senderPass);
        //    smtpClient.EnableSsl = true;

        //    try
        //    {
        //        smtpClient.Send(mail);
        //        Console.WriteLine("Sent Successfully");
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine($"Error: {e.Message}");
        //    }

        //    return Ok(new { sOTP });
        //}

        [HttpPut("/lxp/learner/updateProfile")]
        public async Task<IActionResult> UpdateProfile( [FromForm] UpdateProfileViewModel model)
        {
            await _profileService.UpdateProfile(model);

            return Ok(CreateSuccessResponse(200));
        }



              [HttpPut("/lxp/learner/updatePassword")]
        public async Task<IActionResult> UpdatePassword(string learnerId, string oldPassword, string newPassword)
        {
            var result = await _passwordHistoryService.UpdatePassword(learnerId, oldPassword, newPassword);

            if (!result)
            {
                return BadRequest("Old password is incorrect");
            }

            return Ok("Password updated successfully");
        }
    }
}


    


using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace LXP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private static ConcurrentDictionary<string, string> emailOtpMap = new ConcurrentDictionary<string, string>();
        private static DateTime _currentTIme;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        ///<summary>
        ///Generating email to the repected mail they entered
        ///</summary>
        [HttpPost("EmailVerification")]
        public async Task<IActionResult> GenerateOTP([FromBody] string email)
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

            // Store the OTP data
            emailOtpMap[email] = sOTP;

            // Configure email settings
            string subject = "LXP Email Verification";
            string body = $"The OTP to Verify Your Email is: {sOTP}";

            // Send the email

            bool emailSent = await _emailService.SendEmailAsync(email, subject, body);

            if (emailSent)
            {
                _currentTIme = DateTime.Now;
                // Return the generated OTP along with a success message
                return Ok(new { Message = "Email sent successfully", Email = email, OTP = sOTP });
            }
            else
            {
                // Handle failure (e.g., return an error response)
                return BadRequest(new { Message = "Error sending email" });
            }


        }


        ///<summary>
        ///Validating the OTP
        ///</summary>
        [HttpPost("VerifyOTP")]
        public IActionResult VerifyOTP([FromBody] OTPVerificationViewModel otpverify)
        {
            // return Ok(emailOtpMap);
            //return Ok(emailOtpMap["rajkumar08102001@gmail.com"]);
            if (emailOtpMap.ContainsKey(otpverify.Email))
            {
                var otpData = emailOtpMap[otpverify.Email];
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
                    if (otpData == otpverify.OTP)
                    {
                        string removeEmail = "";
                        emailOtpMap.Remove(otpverify.Email, out removeEmail);
                        Console.WriteLine($"OTP verified successfully for email: {otpverify.Email}");

                        return Ok("OTP verified successfully!");
                    }
                    else
                    {
                        // Expired or invalid OTP
                        Console.WriteLine($"Invalid OTP provided for email: {otpverify.Email}");
                        return BadRequest("Invalid OTP provided.");
                    }

                }
                else
                {
                    // Expired or invalid OTP
                    string removeEmail = "";
                    emailOtpMap.Remove(otpverify.Email, out removeEmail);
                    Console.WriteLine($"OTP has expired for email: {otpverify.Email}");
                    return BadRequest("OTP has expired." + storedTimestamp + "#####" + _currentTIme);
                }
            }
            else
            {
                // OTP data not found for the provided email
                Console.WriteLine($"No OTP data found for the provided email: {otpverify.Email}");
                return BadRequest("No OTP data found for the provided email.");
            }
        }

    }
}



//using LXP.Core.IServices;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Concurrent;
//using LXP.Common.ViewModels;

//namespace LXP.Api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EmailController : ControllerBase
//    {
//        private readonly IEmailService _emailService;
//        private static ConcurrentDictionary<string, string> emailOtpMap = new ConcurrentDictionary<string, string>();
//        private static DateTime _currentTIme;

//        public EmailController(IEmailService emailService)
//        {
//            _emailService = emailService;
//        }

//        ///<summary>
//        ///Generating email to the repected mail they entered
//        ///</summary>
//        [HttpPost("EmailVerification")]
//        public async Task<IActionResult> GenerateOTP([FromBody] string email)
//        {
//            // Generate a random OTP
//            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
//            string sOTP = "";
//            Random rand = new Random();

//            for (int i = 0; i < 6; i++)
//            {
//                int p = rand.Next(0, saAllowedCharacters.Length);
//                sOTP += saAllowedCharacters[p];
//            }

//            // Store the OTP data
//            emailOtpMap[email] = sOTP;

//            // Configure email settings
//            string subject = "LXP Email Verification";
//            string body = $"The OTP to Verify Your Email is: {sOTP}";

//            // Send the email

//            bool emailSent = await _emailService.SendEmailAsync(email, subject, body);

//            if (emailSent)
//            {
//                _currentTIme = DateTime.Now;
//                // Return the generated OTP along with a success message
//                return Ok(new { Message = "Email sent successfully", Email = email, OTP = sOTP });
//            }
//            else
//            {
//                // Handle failure (e.g., return an error response)
//                return BadRequest(new { Message = "Error sending email" });
//            }


//        }


//        ///<summary>
//        ///Validating the OTP
//        ///</summary>
//        [HttpGet("VerifyOTP")]
//        public IActionResult VerifyOTP([FromBody] string email, [FromBody] string userOTP)

//        {
//            // return Ok(emailOtpMap);
//            //return Ok(emailOtpMap["rajkumar08102001@gmail.com"]);

//            string email = model.Email;
//            string userOTP = model.UserOTP;

//            if (emailOtpMap.ContainsKey(email))
//            {
//                var otpData = emailOtpMap[email];
//                DateTime storedTimestamp = _currentTIme;
//                DateTime currentTimestamp = DateTime.Now;
//                // Check if the OTP is still valid (within 2 minutes)
//                TimeSpan timeDifference = currentTimestamp - storedTimestamp;


//                //if (otpData == userOTP)
//                //{
//                //    Console.WriteLine($"OTP verified successfully for email: {email}");
//                //    return Ok("OTP verified successfully!");
//                //
//                double num = timeDifference.TotalMinutes;
//                if (timeDifference.TotalMinutes < 2)
//                {
//                    if (otpData == userOTP)
//                    {
//                        string removeEmail = "";
//                        emailOtpMap.Remove(email, out removeEmail);
//                        Console.WriteLine($"OTP verified successfully for email: {email}");

//                        return Ok("OTP verified successfully!");
//                    }
//                    else
//                    {
//                        // Expired or invalid OTP
//                        Console.WriteLine($"Invalid OTP provided for email: {email}");
//                        return BadRequest("Invalid OTP provided.");
//                    }

//                }
//                else
//                {
//                    // Expired or invalid OTP
//                    string removeEmail = "";
//                    emailOtpMap.Remove(email, out removeEmail);
//                    Console.WriteLine($"OTP has expired for email: {email}");
//                    return BadRequest("OTP has expired." + storedTimestamp + "#####" + _currentTIme);
//                }
//            }
//            else
//            {
//                // OTP data not found for the provided email
//                Console.WriteLine($"No OTP data found for the provided email: {email}");
//                return BadRequest("No OTP data found for the provided email.");
//            }
//        }

//    }
//}

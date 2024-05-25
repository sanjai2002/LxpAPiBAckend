using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace LXP.Core.Services
{
    public class EmailService : IEmailService
    {


        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }


        public async Task<bool> SendEmailAsync(string recipientEmail, string subject, string body)
        {


            try
            {
                using (var mail = new MailMessage(_emailSettings.SenderEmail, recipientEmail))
                {
                    mail.Subject = subject;
                    mail.Body = body;

                    using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                    {
                        smtpClient.Port = 587;
                        smtpClient.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
                        smtpClient.EnableSsl = true;

                        await smtpClient.SendMailAsync(mail);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending email: {e.Message}");
                return false;
            }
        }

    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace LXP.Common.Utils
{
    public class EmailGenerator
    {

        public static void Sendpassword(string password, string Email)
        {
            string fromMail = "sanjairock85@gmail.com";
            string senderPass = "vmrc sKxx ihyK jscu";
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.To.Add(Email);
            message.Subject = $"Confidential! New Password for Your Accounts";
            message.Body = $"Dear Learner \r\n\r\nThis is a notification from the management. Your new password for  accounts has been generated. Please take a moment to update your password.\r\n\r\nNew Password:{password}\r\n\r\nRemember to change your password promptly for security reasons.\r\n\r\nThank you, Management Team";
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(fromMail, senderPass);
            smtpClient.EnableSsl = true;
           
            smtpClient.Send(message);
               
          
            
        }
    }

}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Static_and_Login.Services.Models
    {
        public class EmailService
        {
            public async Task SendOtpEmailAsync(string toEmail, string otpCode)
            {
                string fromEmail = ConfigurationManager.AppSettings["SmtpEmail"];
                string appPassword = ConfigurationManager.AppSettings["SmtpAppPassword"];

                var message = new MailMessage
                {
                    From = new MailAddress(fromEmail, "Static and Login"),
                    Subject = "Your Password Reset Code",
                    Body = $"Your OTP code is: {otpCode}\n\nThis code expires in 10 minutes.\n\nIf you did not request this, please ignore this email.",
                    IsBodyHtml = false
                };
                message.To.Add(toEmail);

                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(fromEmail, appPassword);
                    await client.SendMailAsync(message);
                }
            }
        }
}


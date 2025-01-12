using System.Net;
using System.Net.Mail;

namespace API
{

    public class EmailService
    {
        public EmailService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void SendEmail(string toEmail, string subject, string body)
        {
            var fromEmail = Configuration.GetSection("Constants:FromEmail").Value ?? string.Empty;
            var fromEmailPassword = Configuration.GetSection("Constants:EmailAccountPassword").Value ?? string.Empty;

            if (!IsValidEmail(fromEmail) || !IsValidEmail(toEmail))
            {
                throw new FormatException("Invalid email address format.");
            }

            var message = new MailMessage()
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(toEmail);

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, fromEmailPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

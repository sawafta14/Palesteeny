using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Palesteeny_Project.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _fromEmail;
        private readonly string _password;

        public SmtpEmailSender(string host, int port, string fromEmail, string password)
        {
            _host = host;
            _port = port;
            _fromEmail = fromEmail;
            _password = password;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            using var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_fromEmail, _password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage(_fromEmail, toEmail, subject, message)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(mailMessage);
        }
    }
}

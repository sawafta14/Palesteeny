using System.Threading.Tasks;

namespace Palesteeny_Project.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}

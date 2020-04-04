using System.Threading.Tasks;

namespace TortoogaApp.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
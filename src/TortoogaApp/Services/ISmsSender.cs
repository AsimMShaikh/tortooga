using System.Threading.Tasks;

namespace TortoogaApp.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
using System.Net.Http;
using System.Threading.Tasks;

namespace Epc.API.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);

    }
}

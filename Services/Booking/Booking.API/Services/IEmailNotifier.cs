using System.Threading.Tasks;
using Booking.API.Model;

namespace Booking.API.Services
{
    public interface IEmailNotifier
    {
        Task SendEmailAsync(EmailModel emailModel);
    }
}
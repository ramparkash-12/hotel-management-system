using System;
using System.Threading.Tasks;

namespace Booking.API.Idempotency
{
    public interface IEventRequestManager
    {
        Task<bool> ExistAsync(Guid id);

        Task SaveEventRequest<T>(Guid id);
    }
}
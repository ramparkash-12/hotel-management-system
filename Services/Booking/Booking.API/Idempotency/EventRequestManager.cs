using System;
using System.Threading.Tasks;
using Booking.API.Data;
using Microsoft.Extensions.Logging;

namespace Booking.API.Idempotency
{
    public class EventRequestManager : IEventRequestManager
    {
        private readonly BookingContext _context;
        private readonly ILoggerFactory _logger;

        public EventRequestManager(BookingContext context, ILoggerFactory logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task<bool> ExistAsync(Guid id)
        {
            // ** Check if event is not idempotent
            _logger.CreateLogger<EventRequestManager>().LogInformation("----- Checking idempotency of integration event: {IntegrationEventId} at {AppName}", id, Program.AppName);

            var request = await _context.
                FindAsync<EventRequest>(id);

            if (request != null)
            {
                // ** Check if event is not idempotent
                _logger.CreateLogger<EventRequestManager>().LogTrace("----- Event already exists: {IntegrationEventId} at {AppName}", id, Program.AppName);
                return false;
            }
            return request != null;
        }

        public async Task SaveEventRequest<T>(Guid id)
        { 
            // ** saving event...
            _logger.CreateLogger<EventRequestManager>().LogInformation("----- Saving event for idempotency of integration event: {IntegrationEventId} at {AppName}", id, Program.AppName);

            var request = new EventRequest()
                {
                    Id = id,
                    Name = typeof(T).Name,
                    Time = DateTime.UtcNow
                };

            _context.Add(request);

            await _context.SaveChangesAsync();
            //throw new Exception($"Request with {id} already exists");
        } 
    }
}
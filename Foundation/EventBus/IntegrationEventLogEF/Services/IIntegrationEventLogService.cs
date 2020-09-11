using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation.EventBus.Events;
using Foundation.EventBus.IntegrationEventLogEF;
using Microsoft.EntityFrameworkCore.Storage;

namespace Foundation.EventBus.IntegrationEventLogEF.Services
{
    public interface IIntegrationEventLogService
    {
        Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId);
        Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction);
        Task MarkEventAsPublishedAsync(Guid eventId);
        Task MarkEventAsInProgressAsync(Guid eventId);
        Task MarkEventAsFailedAsync(Guid eventId);
    }
}
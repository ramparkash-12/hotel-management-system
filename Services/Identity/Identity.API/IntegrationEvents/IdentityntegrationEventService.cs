using System;
using System.Data.Common;
using System.Threading.Tasks;
using Foundation.EventBus.Abstractions;
using Foundation.EventBus.Events;
using Foundation.EventBus.IntegrationEventLogEF.Services;
using Identity.API.Data;
using IntegrationEventLogEF.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Identity.API.IntegrationEvents
{
  public class IdentityntegrationEventService : IIdentityntegrationEventService
  {
    private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
    private readonly IEventBus _eventBus;
    private readonly ApplicationDbContext _Context;
    private readonly IIntegrationEventLogService _eventLogService;
    private readonly ILogger<IdentityntegrationEventService> _logger;

    public IdentityntegrationEventService(
        ILogger<IdentityntegrationEventService> logger,
        IEventBus eventBus,
        ApplicationDbContext Context,
        Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _Context = Context ?? throw new ArgumentNullException(nameof(Context));
      _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
      _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
      _eventLogService = _integrationEventLogServiceFactory(_Context.Database.GetDbConnection());
    }
    
    public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
    {
      try
      {
        _logger.LogInformation("----- Publishing integration event: {IntegrationEventId_published} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);

        await _eventLogService.MarkEventAsInProgressAsync(evt.Id);

        _eventBus.Publish(evt);

        await _eventLogService.MarkEventAsPublishedAsync(evt.Id);

      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);
        await _eventLogService.MarkEventAsFailedAsync(evt.Id);
      }
    }

    public async Task SaveEventAndIdentityContextChangesAsync(IntegrationEvent evt)
    {
      _logger.LogInformation("----- IdentityntegrationEventService - Saving changes and integrationEvent: {IntegrationEventId}", evt.Id);

      //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
      //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency            
      await ResilientTransaction.New(_Context).ExecuteAsync(async () =>
      {
          // Achieving atomicity between original identity database operation and the IntegrationEventLog with a local transaction
          await _Context.SaveChangesAsync();
        await _eventLogService.SaveEventAsync(evt, _Context.Database.CurrentTransaction);
      });
    }

  }
}
using System;
using System.Threading.Tasks;
using Foundation.EventBus.Abstractions;
using Foundation.EventBus.Events;

namespace Identity.API.IntegrationEvents
{
  public class IdentityntegrationEventService : IIdentityntegrationEventService
  {
    private readonly IEventBus _eventBus;

    public IdentityntegrationEventService(IEventBus eventBus)
    {
      _eventBus = eventBus;
    }
    public void PublishThroughEventBus(IntegrationEvent evt)
    {
      try
      {
        _eventBus.Publish(evt);
      }
      catch (Exception)
      { }
    }

  }
}
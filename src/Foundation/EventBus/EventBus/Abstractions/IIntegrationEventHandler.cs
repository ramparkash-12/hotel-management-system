using System.Threading.Tasks;
using Foundation.EventBus.Events;

namespace Foundation.EventBus.Abstractions
{
  public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
      where TIntegrationEvent : IntegrationEvent
  {
    Task Handle(TIntegrationEvent @event);
  }

  public interface IIntegrationEventHandler
  {
  }
}
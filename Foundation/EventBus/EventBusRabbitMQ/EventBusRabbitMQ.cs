using System;
using Foundation.EventBus.Abstractions;
using Foundation.EventBus.Events;
using RabbitMQ.Client;

namespace Foundation.EventBusRabbitMQ
{
  public class EventBusRabbitMQ : IEventBus
  {
    public void Publish(IntegrationEvent @event)
    {
      throw new System.NotImplementedException();
    }

    public void Subscribe<T, TH>()
      where T : IntegrationEvent
      where TH : IIntegrationEventHandler<T>
    {
      throw new System.NotImplementedException();
    }
  }
}
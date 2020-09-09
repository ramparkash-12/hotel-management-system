using System;
using RabbitMQ.Client;

namespace Foundation.EventBusRabbitMQ
{
    public interface IRabbitMQPersistentConnection : IDisposable
    {
         bool IsConnected { get; }
         bool TryConnect();
         IModel CreateModel();
    }
}
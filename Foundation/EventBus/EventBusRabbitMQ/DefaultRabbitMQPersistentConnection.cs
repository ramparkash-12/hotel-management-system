using System;
using System.IO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Foundation.EventBusRabbitMQ
{
  public class DefaultRabbitMQPersistentConnection : IRabbitMQPersistentConnection
  {
    private readonly IConnectionFactory _connectionFactory;
    private readonly DefaultRabbitMQPersistentConnection _logger;
    private readonly int _retryCount;
    IConnection _connection;
    bool _disposed;
    object sync_root = new object();

    public DefaultRabbitMQPersistentConnection(IConnectionFactory connectionFactory, DefaultRabbitMQPersistentConnection logger, int retryCount)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _retryCount = retryCount;
    }

    public bool IsConnected
    {
        get
        {
            return _connection != null && _connection.IsOpen && !_disposed;
        }
    }

    public IModel CreateModel()
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
        }

        return _connection.CreateModel();
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;
        try
        {
            _connection.Dispose();
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException(ex.ToString());
        }
    }

    public bool TryConnect()
    {
            lock (sync_root)
            {
               _connection = _connectionFactory
                          .CreateConnection();

                if (IsConnected)
                {
                    _connection.ConnectionShutdown += OnConnectionShutdown;
                    _connection.CallbackException += OnCallbackException;
                    _connection.ConnectionBlocked += OnConnectionBlocked;

                    return true;
                }
                else
                {
                    return false;
                }
            }
    }

    private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            //_logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

            TryConnect();
        }

        void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            //_logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

            TryConnect();
        }

        void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;

            //_logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            TryConnect();
        }
  }
}
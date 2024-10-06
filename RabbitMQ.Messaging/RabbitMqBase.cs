using RabbitMQ.Client;

namespace RabbitMQ.Messaging;

public class RabbitMqBase : IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqBase(ConnectionFactory factory, string queueName)
    {
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
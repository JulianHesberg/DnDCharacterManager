using System.Text;
using System.Text.Json;
using MessageBroker.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageBroker.Implementations;

public class RabbitAMPQ : IMessageBroker
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly Dictionary<string, string> _consumerTags = new Dictionary<string, string>();

    public RabbitAMPQ(string hostName, string userName, string password)
    {
        var factory = new ConnectionFactory
        {
            HostName = hostName,
            UserName = userName,
            Password = password
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public Task Publish<T>(string queueName, T message, CancellationToken cancellationToken = default)
    {
        _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var messageBody = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(messageBody);

        _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        return Task.CompletedTask;
    }

    public Task Subscribe(string queueName, Action<IMessage> onMessageReceived, CancellationToken cancellationToken = default)
    {
        _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var messageJson = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<IMessage>(messageJson);
            if (message != null)
            {
                onMessageReceived(message);
            }
        };

        var consumerTag = _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        _consumerTags[queueName] = consumerTag;

        return Task.CompletedTask;
    }

    public Task Unsubscribe(string queueName, CancellationToken cancellationToken = default)
    {
        if (_consumerTags.TryGetValue(queueName, out var consumerTag))
        {
            _channel.BasicCancel(consumerTag);
            _consumerTags.Remove(queueName);
        }
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
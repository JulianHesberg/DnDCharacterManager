using MessageBroker.Interfaces;
using RabbitMQ.Client;  
using RabbitMQ.Client.Events;  
using System.Text;  
using System.Text.Json;  

namespace MessageBroker.Implementations;

public class RabbitMqMessageBroker : IMessageBroker
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    
    public RabbitMqMessageBroker(string hostName, string userName, string password)
    {
        var factory = new ConnectionFactory() { HostName = hostName, UserName = userName, Password = password };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }
    
    public void Publish<T>(string queueName, T message)
    {
        _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);  
        var jsonMessage = JsonSerializer.Serialize(message);  
        var body = Encoding.UTF8.GetBytes(jsonMessage);  
  
        _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);  
    }

    public void Subscribe<T>(string queueName, Action<T> onMessageRecieved)
    {
        _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);  
        var consumer = new EventingBasicConsumer(_channel);  
              
        consumer.Received += (model, ea) =>  
        {  
            var body = ea.Body.ToArray();  
            var jsonMessage = Encoding.UTF8.GetString(body);  
            var message = JsonSerializer.Deserialize<T>(jsonMessage);  
                  
            onMessageRecieved(message);  
        };  
              
        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);  
    }
    
    public void Dispose()  
    {  
        _channel?.Dispose();  
        _connection?.Dispose();  
    }  
}
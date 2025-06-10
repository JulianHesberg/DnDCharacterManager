using EasyNetQ;
using MessageBroker.Configuration;
using MessageBroker.Implementations;

namespace MessageBroker.Factories;

public static class RabbitMQFactory
{
    public static RabbitMqMessageBroker Create(MessageBrokerOptions options)
    {
        var connectionString = $"host={options.HostName};username={options.UserName};password={options.Password};publisherConfirms=true;timeout=10";
        IBus bus = RabbitHutch.CreateBus(connectionString);
        return new RabbitMqMessageBroker(bus);
    }
    public static RabbitAMPQ CreateRabbitAMPQ(MessageBrokerOptions options)
    {
        return new RabbitAMPQ(options.HostName, options.UserName, options.Password);
    }
}
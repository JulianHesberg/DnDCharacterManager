using EasyNetQ;
using MessageBroker.Configuration;
using MessageBroker.Implementations;

namespace MessageBroker.Factories;

public static class RabbitMQFactory
{
    public static RabbitMqMessageBroker Create(MessageBrokerOptions options)
    {
        IBus bus = RabbitHutch.CreateBus(options.ConnectionString + ";publisherConfirms=true;timeout=10");
        return new RabbitMqMessageBroker(bus);
    }
}
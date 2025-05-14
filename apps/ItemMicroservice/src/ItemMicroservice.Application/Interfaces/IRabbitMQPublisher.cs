using ItemMicroservice.Domain.Entities;

namespace ItemMicroservice.Application.Interfaces;

public interface IRabbitMQPublisher : IAsyncDisposable
{
    Task PublishAsync<T>(T message, string routingKey = "");
    Task PublishItemCreated(ItemCreatedMessage message);
}

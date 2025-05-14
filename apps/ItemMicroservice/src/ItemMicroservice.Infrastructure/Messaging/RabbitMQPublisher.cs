using System.Text;
using System.Text.Json;
using ItemMicroservice.Application.Interfaces;
using ItemMicroservice.Domain.Entities;
using ItemMicroservice.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace ItemMicroService.Infrastructure.Configurations;

public class RabbitMQPublisher : IRabbitMQPublisher
{
    private readonly ConnectionFactory _factory;
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly string _exchangeName;

    public RabbitMQPublisher(RabbitMQSettings settings)
    {
        _exchangeName = settings.Exchange;
        _factory = new ConnectionFactory
        {
            HostName = settings.Host,
            UserName = settings.Username,
            Password = settings.Password
        };
    }

    public static async Task<RabbitMQPublisher> CreateAsync(IOptions<RabbitMQSettings> options)
    {
        ArgumentNullException.ThrowIfNull(options, nameof(options));

        var settings = options.Value;
        var pub = new RabbitMQPublisher(settings);
        await pub.InitializeChannelAsync();
        return pub;
    }

    private async Task InitializeChannelAsync()
    {
        try
        {
            _connection = await _factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(
                    exchange: _exchangeName,
                    type: ExchangeType.Fanout,
                    durable: true,
                    autoDelete: false
                );
        }
        catch (BrokerUnreachableException ex)
        {
            throw new InvalidOperationException($"Could not reach RabbitMQ broker at {_factory.HostName}", ex);

        }
    }

    /// <summary>
    /// Serializes the message and publishes via BasicPublishAsync.
    /// </summary>
    public async Task PublishAsync<T>(T message, string routingKey = "")
    {
        if (_channel == null || _connection == null || !_connection.IsOpen)
            await InitializeChannelAsync();

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await _channel!.BasicPublishAsync(
            _exchangeName,
            routingKey,
            false,
            body
        );
    }

    /// <summary>
    /// Convenience overload for ItemCreatedMessage DTO.
    /// </summary>
    public Task PublishItemCreated(ItemCreatedMessage message)
    => PublishAsync(message);

    /// <summary>
    /// Cleanly close channel and connection.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        if (_channel != null) { await _channel.CloseAsync(); _channel.Dispose(); }
        if (_connection != null) { await _connection.CloseAsync(); _connection.Dispose(); }
    }
}
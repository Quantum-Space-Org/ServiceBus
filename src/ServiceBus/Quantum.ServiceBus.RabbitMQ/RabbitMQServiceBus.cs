using System;
using System.Threading;
using System.Threading.Tasks;

namespace Quantum.ServiceBus.RabbitMQ;
using System.Text;
using global::RabbitMQ.Client;


public class RabbitMQServiceBus : IServiceBus
{

    public Task CreateTopicIfNotExists(TopicSpecification topicSpecification)
    {
        throw new NotImplementedException();
    }

    public async Task<DeliveryResult<TKey, TValue>> Publish<TKey, TValue>(string topic, Message<TKey, TValue> message, CancellationToken cancellationToken = default)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "hello",
                                                    durable: false,
                                                    exclusive: false,
                                                    autoDelete: false,
                                                    arguments: null);

        var body = Encoding.UTF8.GetBytes("Hello World!");

        channel.BasicPublish(exchange: string.Empty,
                                                        routingKey: "hello",
                                                        basicProperties: null,
                                                        body: body);

        return new DeliveryResult<TKey, TValue>();
    }

    public Task StartConsuming(string topicName, IEventBusMessageSubscriber subscriber)
    {
        throw new NotImplementedException();
    }
}
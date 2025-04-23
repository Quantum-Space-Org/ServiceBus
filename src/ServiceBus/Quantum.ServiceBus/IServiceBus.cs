using System.Threading;
using System.Threading.Tasks;

namespace Quantum.ServiceBus;

public interface IServiceBus
{
    Task CreateTopicIfNotExists(TopicSpecification topicSpecification);

    Task<DeliveryResult<TKey, TValue>> Publish<TKey, TValue>(string topic,
        Message<TKey, TValue> message,
        CancellationToken cancellationToken = default);

    Task StartConsuming(string topicName, IEventBusMessageSubscriber subscriber);
}
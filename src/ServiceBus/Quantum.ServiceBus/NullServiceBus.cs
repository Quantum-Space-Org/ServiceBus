using System.Threading;
using System.Threading.Tasks;

namespace Quantum.ServiceBus;

public class NullServiceBus : IServiceBus

{
    public  static IServiceBus Instance =>new NullServiceBus();
    public Task CreateTopicIfNotExists(TopicSpecification topicSpecification)
    {
        return Task.CompletedTask;
    }

    public async Task<DeliveryResult<TKey, TValue>> Publish<TKey, TValue>(string topic, Message<TKey, TValue> message, CancellationToken cancellationToken = default)
    {
        return default;
    }

    public Task StartConsuming(string topicName, IEventBusMessageSubscriber subscriber)
    {
        return Task.CompletedTask;
    }
}
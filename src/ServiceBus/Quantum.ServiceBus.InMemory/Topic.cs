
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Quantum.ServiceBus.InMemory;

public class Topic
{
    public string Name { get; }
    private readonly IList<TopicMessage> _messages;
    private long _currentOffset;
    private IEventBusMessageSubscriber _subscriber;

    public Topic(TopicSpecification topicSpecification)
    {
        Name = topicSpecification.Name;
        _messages = new List<TopicMessage>();
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (GetType() != obj.GetType())
            return false;

        var that = (Topic)obj;

        return Name == that.Name;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode() * 23;
    }

    internal async Task<DeliveryResult<TKey, TValue>> Append<TKey, TValue>(Message<TKey, TValue> message, CancellationToken cancellationToken)
    {
        _currentOffset++;

        TopicMessage? topicMessage = new(message.Key?.ToString(), ToJson(message), message.Type, 
            message.Headers, new Partition(1)
            , Offset.At(_currentOffset));

        _messages.Add(topicMessage);

        CallSubscriber(topicMessage);

        return new DeliveryResult<TKey, TValue>
        {
            Topic = Name,
            Offset = Offset.At(_currentOffset),
            Partition = new Partition(1),
            Status = PersistenceStatus.Persisted
        };
    }

    private void CallSubscriber(TopicMessage topicMessage)
    {
        Task.Run(() =>
                _subscriber?.Subscribe(new ConsumerMessageResult
                {
                    Topic = Name,
                    Key = topicMessage.Key,
                    Message = topicMessage.Message,
                    Headers = topicMessage.Headers,
                    MessageType = topicMessage.MessageType
                }))
            .ConfigureAwait(false);
    }

    private static string ToJson<TKey, TValue>(Message<TKey, TValue> message)
        => Newtonsoft.Json.JsonConvert.SerializeObject(message.Value);

    internal void Subscribe(IEventBusMessageSubscriber subscriber)
    {
        _subscriber = subscriber;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Quantum.Domain;
using Quantum.Domain.Messages.Command;

namespace Quantum.ServiceBus.InMemory
{
    public class InMemoryServiceBus : IServiceBus
    {
        private readonly List<Topic> _topics;

        public InMemoryServiceBus()
            => _topics = new List<Topic>();

        public async Task CreateTopicIfNotExists(TopicSpecification topicSpecification)
        {
            if (topicSpecification == null)
                throw new ArgumentNullException(nameof(topicSpecification));

            if (string.IsNullOrWhiteSpace(topicSpecification.Name))
                throw new TopicNullOrWhiteSpaceException();

            if (TopicExists(topicSpecification.Name))
                return;

            var topic = new Topic(topicSpecification);
            _topics.Add(topic);
        }

        public async Task<DeliveryResult<TKey, TValue>> Publish<TKey, TValue>(string topic,
            Message<TKey, TValue> message,
            CancellationToken cancellationToken = default)
        {
            AssertThatTopicIsExists(topic);
            var top = GetTopic(topic);
            var deliveryResult = await top.Append(message, cancellationToken);
            return deliveryResult;
        }

        public Task StartConsuming(string topicName, IEventBusMessageSubscriber subscriber)
        {
            AssertThatTopicIsExists(topicName);

            var topic = GetTopic(topicName);
            topic.Subscribe(subscriber);
            return Task.CompletedTask;
        }


        private Topic GetTopic(string topic)
        {
            return _topics.FirstOrDefault(t => t.Name == topic);
        }


        private void AssertThatTopicIsExists(string topicName)
        {
            if (!TopicExists(topicName))
                throw new NotExistsTopicException(topicName);
        }
        private bool TopicExists(string topicName)
        {
            return _topics.Any(t => t.Name == topicName);
        }

        [Serializable]
        public class TopicNullOrWhiteSpaceException : Exception
        {
            public TopicNullOrWhiteSpaceException()
            {
            }

            public TopicNullOrWhiteSpaceException(string message) : base(message)
            {
            }

            public TopicNullOrWhiteSpaceException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected TopicNullOrWhiteSpaceException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
        public class NotExistsTopicException : Exception
        {
            public string TopicName { get; }

            public NotExistsTopicException(string topicName)
            {
                TopicName = topicName;
            }
        }

    }

    public class TopicMessage
    {
        public TopicMessage(string key, string message, string messageType, Headers headers, Partition partition, Offset offset)
        {
            Key = key;
            Message = message;
            MessageType = messageType;
            Headers = headers;
            Partition = partition;
            Offset = offset;
        }

        public string Key { get; }
        public string Message { get; }
        public string MessageType { get; }
        public Headers Headers { get; }
        public Offset Offset { get; }
        public Partition Partition { get; }
    }
}
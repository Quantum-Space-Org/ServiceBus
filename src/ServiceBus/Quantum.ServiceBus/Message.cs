using Quantum.Domain;
using Quantum.Domain.Messages.Command;

namespace Quantum.ServiceBus
{
    /// <summary>
    ///     Represents a (deserialized) Kafka message.
    /// </summary>
    public class Message<TKey, TValue>
    {
        /// <summary>
        ///     Gets the message key value (possibly null).
        /// </summary>
        public TKey Key { get; set; }

        /// <summary>
        ///     Gets the message value (possibly null).
        /// </summary>
        public TValue Value { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
        public Headers Headers { get; }

        public Message(TKey key, string id, string messageType, TValue value)
        {
            Key = key;
            Id = id;
            Type = messageType;
            Value = value;
        }
        public Message(string id, string messageType, TValue value) : this(default!, id, messageType, value)
        {
        }

    }
}
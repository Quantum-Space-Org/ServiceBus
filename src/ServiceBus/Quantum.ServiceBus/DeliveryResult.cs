namespace Quantum.ServiceBus
{
    public class DeliveryResult<T, T1>
    {
        /// <summary>
        ///     The partition associated with the message.
        /// </summary>
        public Partition Partition { get; set; }

        /// <summary>
        ///     The partition Offset associated with the message.
        /// </summary>
        public Offset Offset { get; set; }

        /// <summary>
        ///     The topic associated with the message.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        ///     The persistence status of the message
        /// </summary>
        public PersistenceStatus Status { get; set; }
    }
}
using Quantum.Domain;
using Quantum.Domain.Messages.Command;

namespace Quantum.ServiceBus
{
    public class ConsumerMessageResult
    {
        public string Topic { get; set; }
        public string Key { get; set; }
        public string MessageType { get; set; }
        public string Message { get; set; }
        public Headers Headers { get; set; }
    }
}
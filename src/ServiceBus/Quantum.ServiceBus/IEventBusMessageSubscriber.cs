namespace Quantum.ServiceBus;

public interface IEventBusMessageSubscriber
{
    void Subscribe(ConsumerMessageResult message);
}
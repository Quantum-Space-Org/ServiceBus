using Quantum.ServiceBus;

namespace Quantum.UnitTests.EventBus
{
    public class MockEventBusMessageSubscriber : IEventBusMessageSubscriber
    {
        private bool _isCalled;

        public void Verify() => _isCalled.Should().BeTrue();

        public void Subscribe(ConsumerMessageResult messageValue)
            => _isCalled = true;
    }
}
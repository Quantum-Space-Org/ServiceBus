using Microsoft.Extensions.Logging.Abstractions;
using Quantum.Domain.Messages.Command;
using Quantum.ServiceBus;
using Quantum.ServiceBus.InMemory;

namespace Quantum.UnitTests.EventBus;

public class InMemoryEventBusShould
{
    private const string TopicName = "topic-name";
    [Fact]
    public async Task raise_exception_when_create_topic_with_null_topic_specification()
    {
        var func = async () => await CreateInMemoryEventBus().CreateTopicIfNotExists(null);
        await func.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task raise_exception_when_create_topic_with_empty_or_whitespace_topic_title()
    {
        var func = async () => await CreateInMemoryEventBus().CreateTopicIfNotExists(new TopicSpecification());
        await func.Should().ThrowAsync<InMemoryServiceBus.TopicNullOrWhiteSpaceException>();
    }

    [Fact]
    public async Task raise_exception_when_publish_message_to_not_exist_topic()
    {
        var inMemoryEventBus = CreateInMemoryEventBus();

        Func<Task> func = async () => await inMemoryEventBus.Publish(TopicName, CreateMessage());
        await func.Should().ThrowAsync<InMemoryServiceBus.NotExistsTopicException>();
    }

    private static Message<string, CreateCustomerCommand> CreateMessage()
    {
        return new Message<string, CreateCustomerCommand>(id: CreateCustomerCommand().GetId(), typeof(CreateCustomerCommand).AssemblyQualifiedName , CreateCustomerCommand());
    }

    [Fact]
    public async Task create_topic_successfully()
    {
        var inMemoryEventBus = CreateInMemoryEventBus();

        await inMemoryEventBus.CreateTopicIfNotExists(new TopicSpecification
        {
            Name = TopicName
        });

        await inMemoryEventBus.Publish(TopicName, CreateMessage());
    }

    [Fact]
    public async Task successfully_publish_message_to_a_topic()
    {
        var inMemoryEventBus = CreateInMemoryEventBus();

        await inMemoryEventBus.CreateTopicIfNotExists(new TopicSpecification
        {
            Name = TopicName
        });

        var deliveryResult = await inMemoryEventBus.Publish(TopicName, CreateMessage());

        deliveryResult.Topic.Should().Be(TopicName);
        deliveryResult.Status.Should().Be(PersistenceStatus.Persisted);
        deliveryResult.Offset.Should().Be(Offset.At(1));
        deliveryResult.Partition.Should().Be(new Partition(1));
    }

    [Fact]
    public async Task increment_offset_of_message_sequentially()
    {
        var inMemoryEventBus = CreateInMemoryEventBus();

        await inMemoryEventBus.CreateTopicIfNotExists(new TopicSpecification
        {
            Name = TopicName
        });

        //publish the first message
        var deliveryResult = await inMemoryEventBus.Publish(TopicName, CreateMessage());

        deliveryResult.Offset.Should().Be(Offset.At(1));

        //publish the second message
        deliveryResult = await inMemoryEventBus.Publish(TopicName, CreateMessage());

        deliveryResult.Offset.Should().Be(Offset.At(2));

        //publish the third message
        deliveryResult = await inMemoryEventBus.Publish(TopicName, CreateMessage());

        deliveryResult.Offset.Should().Be(Offset.At(3));
    }

    [Fact]
    public async Task raise_exception_when_consuming_from_a_not_exist_topic()
    {
        var inMemoryEventBus = CreateInMemoryEventBus();
        var func = async () => await inMemoryEventBus.StartConsuming(TopicName, null);
        await func.Should().ThrowAsync<InMemoryServiceBus.NotExistsTopicException>();
    }

    [Fact]
    public async Task consume_messages_from_a_topic()
    {
        var inMemoryEventBus = CreateInMemoryEventBus();

        await inMemoryEventBus.CreateTopicIfNotExists(new TopicSpecification
        {
            Name = TopicName
        });

        var subscriber = new MockEventBusMessageSubscriber();
        await inMemoryEventBus.StartConsuming(TopicName, subscriber);

        await inMemoryEventBus.Publish(TopicName, CreateMessage());

        await Waiter.Wait(() => false, TimeSpan.FromSeconds(1));
        subscriber.Verify();
    }

    private static CreateCustomerCommand CreateCustomerCommand()
        => new("1213","Martin","Fowler");

    private static IServiceBus CreateInMemoryEventBus()
    {
        IServiceBus serviceBus = new InMemoryServiceBus();
        return serviceBus;
    }
}

internal class CreateCustomerCommand(string id, string firstName, string lastName)
:IsACommand
{
    public string Id { get; private set; } = id;
    public string FirstName { get; private set; } = firstName;
    public string LastName { get; private set; } = lastName;
}
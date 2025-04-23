using Microsoft.Extensions.DependencyInjection;
using Quantum.Configurator;
using Quantum.ServiceBus;
using Quantum.ServiceBus.InMemory;

namespace Quantum.Dispatcher.Configurator
{

    public static class ConfigQuantumServiceBusExtensions
    {
        public static ConfigQuantumServiceBusBuilder ConfigQuantumServiceBus(this QuantumServiceCollection collection)
        {
            return new ConfigQuantumServiceBusBuilder(collection);
        }
    }

    public class ConfigQuantumServiceBusBuilder(QuantumServiceCollection collection)
    {
        public QuantumServiceCollection and()
        {
            return collection;
        }

        public ConfigQuantumServiceBusBuilder RegisterInMemoryServiceBusAsSingleton()
        {
            collection.Collection.AddSingleton<IServiceBus, InMemoryServiceBus>();
            return this;
        }

        public ConfigQuantumServiceBusBuilder RegisterInMemoryServiceBus<T>(ServiceLifetime serviceLifeTime)
            where T : class, IServiceBus
        {
            collection.Collection.Add(new ServiceDescriptor(typeof(IServiceBus), typeof(T), serviceLifeTime));
            return this;
        }
    }
}

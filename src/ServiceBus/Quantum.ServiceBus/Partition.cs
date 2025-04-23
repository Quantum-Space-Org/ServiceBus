namespace Quantum.ServiceBus
{

    public class Partition
    {
        public Partition(long value)
        {
            Value = value;
        }

        public long Value { get; }
        public override bool Equals(object obj)
        {
            return Value == ((Partition)obj).Value;
        }
    }
}
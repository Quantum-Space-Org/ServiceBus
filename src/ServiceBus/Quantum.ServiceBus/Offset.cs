namespace Quantum.ServiceBus
{
    public class Offset
    {
        public static Offset At(long value)
        {
            return new Offset(value);
        }

        private Offset(long value)
        {
            Value = value;
        }

        public long Value { get; }

        public override bool Equals(object obj)
        {
            return Value == ((Offset)obj).Value;
        }
    }
}
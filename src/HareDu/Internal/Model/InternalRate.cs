namespace HareDu.Internal.Model
{
    using Core.Extensions;
    using HareDu.Model;

    class InternalRate :
        Rate
    {
        public InternalRate(RateImpl obj)
        {
            Value = obj.IsNotNull() ? obj.Value : default;
        }

        public decimal Value { get; }
    }
}
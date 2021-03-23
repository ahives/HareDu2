namespace HareDu.Internal.Model
{
    using Core.Extensions;
    using HareDu.Model;

    class InternalRateImpl :
        Rate
    {
        public InternalRateImpl(RateImpl obj)
        {
            Value = obj.IsNotNull() ? obj.Value : default;
        }

        public decimal Value { get; }
    }
}
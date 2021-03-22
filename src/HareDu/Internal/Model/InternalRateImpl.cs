namespace HareDu.Internal.Model
{
    using Core.Extensions;
    using HareDu.Model;

    class InternalRateImpl :
        Rate
    {
        public InternalRateImpl(RateImpl rate)
        {
            Value = rate.IsNotNull() ? rate.Value : default;
        }

        public decimal Value { get; }
    }
}
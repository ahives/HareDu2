namespace HareDu.Internal.Model
{
    using HareDu.Model;

    class InternalExchangeType :
        ExchangeType
    {
        public InternalExchangeType(ExchangeTypeImpl obj)
        {
            Name = obj.Name;
            Description = obj.Description;
            IsEnabled = obj.IsEnabled;
        }

        public string Name { get; }
        public string Description { get; }
        public bool IsEnabled { get; }
    }
}
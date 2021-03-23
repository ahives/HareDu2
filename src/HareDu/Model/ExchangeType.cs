namespace HareDu.Model
{
    public interface ExchangeType
    {
        string Name { get; }

        string Description { get; }

        bool IsEnabled { get; }
    }
}
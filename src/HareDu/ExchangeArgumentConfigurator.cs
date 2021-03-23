namespace HareDu
{
    public interface ExchangeArgumentConfigurator
    {
        void Add<T>(string arg, T value);
    }
}
namespace HareDu
{
    public interface ExchangeRoutingType
    {
        void Fanout();

        void Direct();

        void Topic();

        void Headers();

        void Federated();

        void Match();
    }
}
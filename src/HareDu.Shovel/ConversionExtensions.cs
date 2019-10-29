namespace HareDu.Shovel
{
    public static class ConversionExtensions
    {
        public static string ToProtocolString(this ShovelProtocol protocol)
        {
            switch (protocol)
            {
                case ShovelProtocol.AMQP_091:
                    return "amqp091";
                case ShovelProtocol.AMQP_10:
                    return "amqp10";
                default:
                    return "amqp091";
            }
        }
    }
}
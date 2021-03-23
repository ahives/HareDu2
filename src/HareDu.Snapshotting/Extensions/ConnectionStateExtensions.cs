namespace HareDu.Snapshotting.Extensions
{
    using HareDu.Model;

    public static class ConnectionStateExtensions
    {
        public static BrokerConnectionState Convert(this string value)
        {
            switch (value)
            {
                case "starting":
                    return BrokerConnectionState.Starting;
                
                case "tuning":
                    return BrokerConnectionState.Tuning;
                
                case "opening":
                    return BrokerConnectionState.Opening;
                
                case "flow":
                    return BrokerConnectionState.Flow;
                
                case "blocking":
                    return BrokerConnectionState.Blocking;
                
                case "blocked":
                    return BrokerConnectionState.Blocked;
                
                case "closing":
                    return BrokerConnectionState.Closing;
                
                case "closed":
                    return BrokerConnectionState.Closed;
                
                case "running":
                    return BrokerConnectionState.Running;

                default:
                    return BrokerConnectionState.Inconclusive;
            }
        }
    }
}
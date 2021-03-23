namespace HareDu.Model
{
    public enum BrokerConnectionState
    {
        Starting,
        Tuning,
        Opening,
        Running,
        Flow,
        Blocking,
        Blocked,
        Closing,
        Closed,
        Inconclusive
    }
}
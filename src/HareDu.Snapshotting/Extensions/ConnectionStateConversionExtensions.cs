namespace HareDu.Snapshotting.Extensions
{
    using Model;

    public static class ConnectionStateConversionExtensions
    {
        public static ConnectionState Convert(this string value)
        {
            switch (value)
            {
                case "starting":
                    return ConnectionState.Starting;
                
                case "tuning":
                    return ConnectionState.Tuning;
                
                case "opening":
                    return ConnectionState.Opening;
                
                case "flow":
                    return ConnectionState.Flow;
                
                case "blocking":
                    return ConnectionState.Blocking;
                
                case "blocked":
                    return ConnectionState.Blocked;
                
                case "closing":
                    return ConnectionState.Closing;
                
                case "closed":
                    return ConnectionState.Closed;
                
                case "running":
                    return ConnectionState.Running;

                default:
                    return ConnectionState.Inconclusive;
            }
        }
    }
}
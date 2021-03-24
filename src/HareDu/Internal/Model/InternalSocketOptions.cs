namespace HareDu.Internal.Model
{
    using HareDu.Model;

    class InternalSocketOptions :
        SocketOptions
    {
        public InternalSocketOptions(SocketOptionsImpl obj)
        {
            Backlog = obj.Backlog;
            NoDelay = obj.NoDelay;
            ExitOnClose = obj.ExitOnClose;
        }

        public long Backlog { get; }
        public bool NoDelay { get; }
        public bool ExitOnClose { get; }
    }
}
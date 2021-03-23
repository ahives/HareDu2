namespace HareDu.Internal.Model
{
    using HareDu.Model;

    class InternalServerHealthInfo :
        ServerHealthInfo
    {
        public InternalServerHealthInfo(ServerHealthInfoImpl obj)
        {
            Status = obj.Status;
            Reason = obj.Reason;
        }

        public string Status { get; }
        public string Reason { get; }
    }
}
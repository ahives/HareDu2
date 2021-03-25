namespace HareDu
{
    public interface VirtualHostLimitsConfiguration
    {
        void SetMaxConnectionLimit(ulong value);

        void SetMaxQueueLimit(ulong value);
    }
}
namespace HareDu.Internal.Model
{
    using HareDu.Model;

    class InternalTotalMemory :
        TotalMemoryInfo
    {
        public InternalTotalMemory(TotalMemoryInfoImpl obj)
        {
            Erlang = obj.Erlang;
            Strategy = obj.Strategy;
            Allocated = obj.Allocated;
        }

        public long Erlang { get; }
        public long Strategy { get; }
        public long Allocated { get; }
    }
}
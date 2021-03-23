namespace HareDu.Internal.Model
{
    using Core.Extensions;
    using HareDu.Model;

    class InternalNodeMemoryUsageInfo :
        NodeMemoryUsageInfo
    {
        public InternalNodeMemoryUsageInfo(NodeMemoryUsageInfoImpl obj)
        {
            Memory = obj.Memory.IsNotNull() ? new InternalMemory(obj.Memory) : default;
        }

        public MemoryInfo Memory { get; }
    }
}
namespace HareDu.Diagnostics.Tests.Fakes
{
    using Snapshotting.Model;

    public class FakeMemorySnapshot1 :
        MemorySnapshot
    {
        public FakeMemorySnapshot1(ulong used, ulong limit, bool alarmInEffect)
        {
            Used = used;
            Limit = limit;
            AlarmInEffect = alarmInEffect;
        }

        public string NodeIdentifier { get; }
        public ulong Used { get; }
        public decimal UsageRate { get; }
        public ulong Limit { get; }
        public bool AlarmInEffect { get; }
    }
}
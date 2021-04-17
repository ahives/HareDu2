namespace HareDu.Diagnostics.Tests.Fakes
{
    using Snapshotting.Model;

    public class FakeDiskSnapshot1 :
        DiskSnapshot
    {
        public FakeDiskSnapshot1(ulong available, bool alarmInEffect, decimal rate)
        {
            AlarmInEffect = alarmInEffect;
            Capacity = new DiskCapacityDetailsImpl(available, rate);
        }

        public string NodeIdentifier { get; }
        public DiskCapacityDetails Capacity { get; }
        public ulong Limit { get; }
        public bool AlarmInEffect { get; }
        public IO IO { get; }


        class DiskCapacityDetailsImpl :
            DiskCapacityDetails
        {
            public DiskCapacityDetailsImpl(ulong available, decimal rate)
            {
                Available = available;
                Rate = rate;
            }

            public ulong Available { get; }
            public decimal Rate { get; }
        }
    }
}
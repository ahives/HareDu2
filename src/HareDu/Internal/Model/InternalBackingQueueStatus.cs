namespace HareDu.Internal.Model
{
    using System.Collections.Generic;
    using HareDu.Model;

    class InternalBackingQueueStatus :
        BackingQueueStatus
    {
        public InternalBackingQueueStatus(BackingQueueStatusImpl obj)
        {
            Mode = obj.Mode;
            Q1 = obj.Q1;
            Q2 = obj.Q2;
            Delta = obj.Delta;
            Q3 = obj.Q3;
            Q4 = obj.Q4;
            Length = obj.Length;
            TargetTotalMessagesInRAM = obj.TargetTotalMessagesInRAM;
            NextSequenceId = obj.NextSequenceId;
            AvgIngressRate = obj.AvgIngressRate;
            AvgEgressRate = obj.AvgEgressRate;
            AvgAcknowledgementIngressRate = obj.AvgAcknowledgementIngressRate;
            AvgAcknowledgementEgressRate = obj.AvgAcknowledgementEgressRate;
        }

        public BackingQueueMode Mode { get; }
        public long Q1 { get; }
        public long Q2 { get; }
        public IList<object> Delta { get; }
        public long Q3 { get; }
        public long Q4 { get; }
        public long Length { get; }
        public string TargetTotalMessagesInRAM { get; }
        public long NextSequenceId { get; }
        public decimal AvgIngressRate { get; }
        public decimal AvgEgressRate { get; }
        public decimal AvgAcknowledgementIngressRate { get; }
        public decimal AvgAcknowledgementEgressRate { get; }
    }
}
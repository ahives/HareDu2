namespace HareDu.Model
{
    using System.Collections.Generic;

    public interface BackingQueueStatus
    {
        BackingQueueMode Mode { get; }
        
        long Q1 { get; }
        
        long Q2 { get; }
        
        IList<object> Delta { get; }
        
        long Q3 { get; }
        
        long Q4 { get; }
        
        long Length { get; }
        
        string TargetTotalMessagesInRAM { get; }
        
        long NextSequenceId { get; }
        
        decimal AvgIngressRate { get; }
        
        decimal AvgEgressRate { get; }
        
        decimal AvgAcknowledgementIngressRate { get; }
        
        decimal AvgAcknowledgementEgressRate { get; }
    }
}
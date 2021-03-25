namespace HareDu.Snapshotting.Model
{
    public interface QueueInternals
    {
        Reductions Reductions { get; }
        
        long TargetCountOfMessagesAllowedInRAM { get; }
        
        decimal ConsumerUtilization { get; }

        long Q1 { get; }
        
        long Q2 { get; }
        
        long Q3 { get; }
        
        long Q4 { get; }
        
        decimal AvgIngressRate { get; }
        
        decimal AvgEgressRate { get; }
        
        decimal AvgAcknowledgementIngressRate { get; }
        
        decimal AvgAcknowledgementEgressRate { get; }
    }
}
namespace HareDu
{
    using System.Runtime.Serialization;

    public enum QueueSyncAction
    {
        [EnumMember(Value = "sync")]
        Sync,
        
        [EnumMember(Value = "cancel_sync")]
        CancelSync
    }
}
namespace HareDu
{
    using System.Runtime.Serialization;

    public enum AckMode
    {
        [EnumMember(Value = "on-confirm")]
        OnConfirm,
        
        [EnumMember(Value = "on-publish")]
        OnPublish,
        
        [EnumMember(Value = "no-ack")]
        NoAck
    }
}
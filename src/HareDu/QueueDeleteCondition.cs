namespace HareDu
{
    public interface QueueDeleteCondition
    {
        void IfUnused();
        
        void IfEmpty();
    }
}
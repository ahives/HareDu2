namespace HareDu.Diagnostics.KnowledgeBase
{
    public interface KnowledgeBaseArticle
    {
        string Id { get; }
        
        ProbeResultStatus Status { get; }
        
        string Reason { get; }
        
        string Remediation { get; }
    }
}
namespace HareDu.Diagnostics.KnowledgeBase
{
    public class MissingKnowledgeBaseArticle :
        KnowledgeBaseArticle
    {
        public MissingKnowledgeBaseArticle(string identifier, ProbeResultStatus status)
        {
            Id = identifier;
            Status = status;
        }

        public string Id { get; }
        public ProbeResultStatus Status { get; }
        public string Reason => "No KB article Available";
        public string Remediation => "NA";
    }
}
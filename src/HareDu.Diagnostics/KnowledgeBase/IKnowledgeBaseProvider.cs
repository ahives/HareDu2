namespace HareDu.Diagnostics.KnowledgeBase
{
    using System.Collections.Generic;
    using Probes;

    public interface IKnowledgeBaseProvider
    {
        bool TryGet(string identifier, ProbeResultStatus status, out KnowledgeBaseArticle article);
        
        bool TryGet(string identifier, out IReadOnlyList<KnowledgeBaseArticle> articles);

        void Add<T>(ProbeResultStatus status, string reason, string remediation)
            where T : DiagnosticProbe;
    }
}
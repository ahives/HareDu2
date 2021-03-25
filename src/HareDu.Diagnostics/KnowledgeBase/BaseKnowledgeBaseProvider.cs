namespace HareDu.Diagnostics.KnowledgeBase
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Extensions;
    using Probes;

    public abstract class BaseKnowledgeBaseProvider :
        IKnowledgeBaseProvider
    {
        protected readonly List<KnowledgeBaseArticle> _articles;

        protected BaseKnowledgeBaseProvider()
        {
            _articles = new List<KnowledgeBaseArticle>();

            Load();
        }

        protected abstract void Load();

        public bool TryGet(string identifier, ProbeResultStatus status, out KnowledgeBaseArticle article)
        {
            if (_articles.Exists(x => x.Id == identifier))
            {
                try
                {
                    article = _articles.Single(x => x.Id == identifier && x.Status == status);
                    return true;
                }
                catch
                {
                    article = new MissingKnowledgeBaseArticle(identifier, status);
                    return false;
                }
            }

            article = new MissingKnowledgeBaseArticle(identifier, status);
            return false;
        }

        public bool TryGet(string identifier, out IReadOnlyList<KnowledgeBaseArticle> articles)
        {
            if (_articles.Exists(x => x.Id == identifier))
            {
                try
                {
                    articles = _articles.Where(x => x.Id == identifier).ToList();
                    return true;
                }
                catch
                {
                    articles = new KnowledgeBaseArticle[] {new MissingKnowledgeBaseArticle(identifier, ProbeResultStatus.NA)};
                    return false;
                }
            }

            articles = new KnowledgeBaseArticle[] {new MissingKnowledgeBaseArticle(identifier, ProbeResultStatus.NA)};
            return false;
        }

        public void Add<T>(ProbeResultStatus status, string reason, string remediation)
            where T : DiagnosticProbe
        {
            _articles.Add(new KnowledgeBaseArticleImpl<T>(status, reason, remediation));
        }


        protected class KnowledgeBaseArticleImpl<T> :
            KnowledgeBaseArticle
            where T : DiagnosticProbe
        {
            public KnowledgeBaseArticleImpl(ProbeResultStatus status, string reason, string remediation)
            {
                Status = status;
                Reason = reason;
                Remediation = remediation;
                Id = typeof(T).GetIdentifier();
            }

            public KnowledgeBaseArticleImpl(ProbeResultStatus status, string reason)
            {
                Status = status;
                Reason = reason;
                Id = typeof(T).GetIdentifier();
            }

            public string Id { get; }
            public ProbeResultStatus Status { get; }
            public string Reason { get; }
            public string Remediation { get; }
        }
    }
}
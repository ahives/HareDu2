namespace HareDu.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using KnowledgeBase;

    public interface ProbeResult
    {
        string ParentComponentId { get; }
        
        string ComponentId { get; }
        
        ComponentType ComponentType { get; }
        
        /// <summary>
        /// Probe identifier
        /// </summary>
        string Id { get; }
        
        /// <summary>
        /// Probe human readable name
        /// </summary>
        string Name { get; }
        
        ProbeResultStatus Status { get; }
        
        KnowledgeBaseArticle KB { get; }
        
        IReadOnlyList<ProbeData> Data { get; }
        
        DateTimeOffset Timestamp { get; }
    }
}
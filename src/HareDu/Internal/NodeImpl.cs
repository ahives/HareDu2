namespace HareDu.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using HareDu.Model;
    using Model;

    class NodeImpl :
        BaseBrokerObject,
        Node
    {
        public NodeImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<ResultList<NodeInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/nodes";

            ResultList<NodeInfoImpl> result = await GetAll<NodeInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            ResultList<NodeInfo> MapResult(ResultList<NodeInfoImpl> result) => new ResultListNodeCopy(result);

            return MapResult(result);
        }

        public async Task<Result<NodeHealthInfo>> GetHealth(string node = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = string.IsNullOrWhiteSpace(node) ? "api/healthchecks/node" : $"/api/healthchecks/node/{node}";

            Result<NodeHealthInfoImpl> result = await Get<NodeHealthInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            Result<NodeHealthInfo> MapResult(Result<NodeHealthInfoImpl> result) => new ResultListNodeHealthCopy(result);

            return MapResult(result);
        }

        public async Task<Result<NodeMemoryUsageInfo>> GetMemoryUsage(string node, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var errors = new List<Error>();
            
            if (string.IsNullOrWhiteSpace(node))
                errors.Add(new ErrorImpl("Name of the node for which to return memory usage data is missing."));
            
            string url = $"api/nodes/{node}/memory";
            
            if (errors.Any())
                return new FaultedResult<NodeMemoryUsageInfo>(errors, new DebugInfoImpl(url));
            
            Result<NodeMemoryUsageInfoImpl> result = await Get<NodeMemoryUsageInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            Result<NodeMemoryUsageInfo> MapResult(Result<NodeMemoryUsageInfoImpl> result) => new ResultListNodeMemoryUsageCopy(result);

            return MapResult(result);
        }

        
        class ResultListNodeMemoryUsageCopy :
            Result<NodeMemoryUsageInfo>
        {
            public ResultListNodeMemoryUsageCopy(Result<NodeMemoryUsageInfoImpl> result)
            {
                Timestamp = result.Timestamp;
                DebugInfo = result.DebugInfo;
                Errors = result.Errors;
                HasFaulted = result.HasFaulted;
                HasData = result.HasData;
                Data = result.Data.IsNotNull() ? new InternalNodeMemoryUsageInfo(result.Data) : default;
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public NodeMemoryUsageInfo Data { get; }
            public bool HasData { get; }
        }


        class ResultListNodeHealthCopy :
            Result<NodeHealthInfo>
        {
            public ResultListNodeHealthCopy(Result<NodeHealthInfoImpl> result)
            {
                Timestamp = result.Timestamp;
                DebugInfo = result.DebugInfo;
                Errors = result.Errors;
                HasFaulted = result.HasFaulted;
                HasData = result.HasData;
                Data = new InternalNodeHealthInfo(result.Data);
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public NodeHealthInfo Data { get; }
            public bool HasData { get; }
        }

        
        class ResultListNodeCopy :
            ResultList<NodeInfo>
        {
            public ResultListNodeCopy(ResultList<NodeInfoImpl> result)
            {
                Timestamp = result.Timestamp;
                DebugInfo = result.DebugInfo;
                Errors = result.Errors;
                HasFaulted = result.HasFaulted;
                HasData = result.HasData;

                var data = new List<NodeInfo>();
                foreach (NodeInfoImpl item in result.Data)
                    data.Add(new InternalNodeInfo(item));

                Data = data;
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public IReadOnlyList<NodeInfo> Data { get; }
            public bool HasData { get; }
        }
    }
}
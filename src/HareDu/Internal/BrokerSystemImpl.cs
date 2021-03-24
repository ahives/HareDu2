namespace HareDu.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using HareDu.Model;
    using Model;

    public class BrokerSystemImpl :
        BaseBrokerObject,
        BrokerSystem
    {
        public BrokerSystemImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<Result<SystemOverviewInfo>> GetSystemOverview(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/overview";

            Result<SystemOverviewInfoImpl> result = await GetRequest<SystemOverviewInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            Result<SystemOverviewInfo> MapResult(Result<SystemOverviewInfoImpl> result) => new ResultCopy(result);

            return MapResult(result);
        }

        public async Task<Result> RebalanceAllQueues(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/rebalance/queues";
            
            return await PostEmptyRequest(url, cancellationToken).ConfigureAwait(false);
        }

        
        class ResultCopy :
            Result<SystemOverviewInfo>
        {
            public ResultCopy(Result<SystemOverviewInfoImpl> result)
            {
                Timestamp = result.Timestamp;
                DebugInfo = result.DebugInfo;
                Errors = result.Errors;
                HasFaulted = result.HasFaulted;
                HasData = result.HasData;
                Data = new InternalSystemOverviewInfo(result.Data);
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public SystemOverviewInfo Data { get; }
            public bool HasData { get; }
        }
    }
}
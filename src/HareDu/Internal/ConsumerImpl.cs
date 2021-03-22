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

    class ConsumerImpl :
        BaseBrokerObject,
        Consumer
    {
        public ConsumerImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<ResultList<ConsumerInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/consumers";

            var result = await GetAll<ConsumerInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            ResultList<ConsumerInfo> MapResult(ResultList<ConsumerInfoImpl> result) => new ResultListCopy(result);

            return MapResult(result);
        }

        
        class ResultListCopy :
            ResultList<ConsumerInfo>
        {
            public ResultListCopy(ResultList<ConsumerInfoImpl> result)
            {
                Timestamp = result.Timestamp;
                DebugInfo = result.DebugInfo;
                Errors = result.Errors;
                HasFaulted = result.HasFaulted;
                HasData = result.HasData;

                var data = new List<ConsumerInfo>();
                foreach (ConsumerInfoImpl item in result.Data)
                    data.Add(new InternalConsumerInfo(item));

                Data = data;
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public IReadOnlyList<ConsumerInfo> Data { get; }
            public bool HasData { get; }
        }
    }
}
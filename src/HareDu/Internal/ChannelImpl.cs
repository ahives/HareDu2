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

    class ChannelImpl :
        BaseBrokerObject,
        Channel
    {
        public ChannelImpl(HttpClient client) :
            base(client)
        {
        }

        public async Task<ResultList<ChannelInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/channels";

            ResultList<ChannelInfoImpl> result = await GetAll<ChannelInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            ResultList<ChannelInfo> MapResult(ResultList<ChannelInfoImpl> result) => new ResultListCopy(result);

            return MapResult(result);
        }

        
        class ResultListCopy :
            ResultList<ChannelInfo>
        {
            public ResultListCopy(ResultList<ChannelInfoImpl> result)
            {
                Timestamp = result.Timestamp;
                DebugInfo = result.DebugInfo;
                Errors = result.Errors;
                HasFaulted = result.HasFaulted;
                HasData = result.HasData;

                var data = new List<ChannelInfo>();
                foreach (ChannelInfoImpl item in result.Data)
                    data.Add(new InternalChannelInfo(item));

                Data = data;
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public IReadOnlyList<ChannelInfo> Data { get; }
            public bool HasData { get; }
        }
    }
}
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

    class ConnectionImpl :
        BaseBrokerObject,
        Connection
    {
        public ConnectionImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<ResultList<ConnectionInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/connections";

            ResultList<ConnectionInfoImpl> result = await GetAll<ConnectionInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            ResultList<ConnectionInfo> MapResult(ResultList<ConnectionInfoImpl> result) => new ResultListCopy(result);

            return MapResult(result);
        }

        public async Task<Result> Delete(string connection, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var errors = new List<Error>();
            
            if (string.IsNullOrWhiteSpace(connection))
                errors.Add(new ErrorImpl("The name of the connection is missing."));

            string url = $"api/connections/{connection}";
            
            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, errors));

            return await DeleteRequest(url, cancellationToken);
        }

        
        class ResultListCopy :
            ResultList<ConnectionInfo>
        {
            public ResultListCopy(ResultList<ConnectionInfoImpl> result)
            {
                Timestamp = result.Timestamp;
                DebugInfo = result.DebugInfo;
                Errors = result.Errors;
                HasFaulted = result.HasFaulted;
                HasData = result.HasData;

                var data = new List<ConnectionInfo>();
                foreach (ConnectionInfoImpl item in result.Data)
                    data.Add(new InternalConnectionInfo(item));

                Data = data;
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public IReadOnlyList<ConnectionInfo> Data { get; }
            public bool HasData { get; }
        }
    }
}
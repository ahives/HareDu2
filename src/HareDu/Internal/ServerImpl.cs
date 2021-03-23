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

    class ServerImpl :
        BaseBrokerObject,
        Server
    {
        public ServerImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<Result<ServerInfo>> Get(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/definitions";

            Result<ServerInfoImpl> result = await Get<ServerInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            Result<ServerInfo> MapResult(Result<ServerInfoImpl> result) => new ResultServerCopy(result);

            return MapResult(result);
        }

        public async Task<Result<ServerHealthInfo>> GetHealth(Action<HealthCheckAction> action,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new HealthCheckActionImpl();
            action(impl);

            string url;
            
            switch (impl.CheckUpType)
            {
                case HealthCheckType.VirtualHost:
                    url = $"api/aliveness-test/{impl.RmqObjectName.ToSanitizedName()}";
                    break;
                    
                case HealthCheckType.Node:
                    url = $"api/healthchecks/node/{impl.RmqObjectName}";
                    break;
                    
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Result<ServerHealthInfoImpl> result = await Get<ServerHealthInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            Result<ServerHealthInfo> MapResult(Result<ServerHealthInfoImpl> result) => new ResultHealthCopy(result);

            return MapResult(result);
        }

        
        class ResultServerCopy :
            Result<ServerInfo>
        {
            public ResultServerCopy(Result<ServerInfoImpl> result)
            {
                Timestamp = result.Timestamp;
                DebugInfo = result.DebugInfo;
                Errors = result.Errors;
                HasFaulted = result.HasFaulted;
                HasData = result.HasData;
                Data = new InternalServerInfo(result.Data);
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public ServerInfo Data { get; }
            public bool HasData { get; }
        }

        
        class ResultHealthCopy :
            Result<ServerHealthInfo>
        {
            public ResultHealthCopy(Result<ServerHealthInfoImpl> result)
            {
                Timestamp = result.Timestamp;
                DebugInfo = result.DebugInfo;
                Errors = result.Errors;
                HasFaulted = result.HasFaulted;
                HasData = result.HasData;
                Data = new InternalServerHealthInfo(result.Data);
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public ServerHealthInfo Data { get; }
            public bool HasData { get; }
        }


        class HealthCheckActionImpl :
            HealthCheckAction
        {
            public string RmqObjectName { get; private set; }
            public HealthCheckType CheckUpType { get; private set; }
            
            public void VirtualHost(string name)
            {
                CheckUpType = HealthCheckType.VirtualHost;
                RmqObjectName = name;
            }

            public void Node(string name)
            {
                CheckUpType = HealthCheckType.Node;
                RmqObjectName = name;
            }
        }
    }
}
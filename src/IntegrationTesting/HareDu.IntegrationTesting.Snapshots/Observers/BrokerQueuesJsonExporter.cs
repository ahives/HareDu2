namespace HareDu.IntegrationTesting.Snapshots.Observers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Core.Extensions;
    using Extensions;
    using Serialization;
    using Snapshotting;
    using Snapshotting.Model;

    public class BrokerQueuesJsonExporter :
        IObserver<SnapshotContext<BrokerQueuesSnapshot>>
    {
        public void OnCompleted() => throw new NotImplementedException();

        public void OnError(Exception error) => throw new NotImplementedException();

        public void OnNext(SnapshotContext<BrokerQueuesSnapshot> value)
        {
            string path = $"{Directory.GetCurrentDirectory()}/snapshots";
            
            if (!File.Exists(path))
            {
                var directory = Directory.CreateDirectory(path);
                
                if (directory.Exists)
                    File.WriteAllText($"{path}/snapshot_{value.Identifier}.json", value.ToJsonString(Deserializer.Options));
            }
        }
    }
}
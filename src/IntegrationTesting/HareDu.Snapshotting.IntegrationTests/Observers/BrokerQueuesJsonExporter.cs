namespace HareDu.Snapshotting.IntegrationTests.Observers
{
    using System;
    using System.IO;
    using HareDu.Extensions;
    using Serialization;
    using Snapshotting;
    using Model;

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
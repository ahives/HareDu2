namespace HareDu.Snapshotting.Persistence
{
    using System.IO;
    using Core.Extensions;
    using HareDu.Extensions;
    using Serialization;

    public class SnapshotWriter :
        ISnapshotWriter
    {
        public bool TrySave<T>(SnapshotResult<T> result, string file, string path)
            where T : Snapshot
        {
            if (DirectoryExists(path))
                return Write(result, file, path);

            if (CreateDirectory(path))
                return Write(result, file, path);

            return false;
        }

        protected virtual bool CreateDirectory(string path)
        {
            var directory = Directory.CreateDirectory(path);

            return !directory.IsNull() && directory.Exists;
        }

        protected virtual bool DirectoryExists(string path) => Directory.Exists(path);

        protected virtual bool Write<T>(SnapshotResult<T> result, string file, string path)
            where T : Snapshot
        {
            string fullPath = $"{path}/{file}";
            
            if (File.Exists(fullPath))
                return false;

            File.WriteAllText(fullPath, result.ToJsonString(Deserializer.Options));
            return true;
        }
    }
}
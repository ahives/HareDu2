namespace HareDu.Diagnostics
{
    using System.IO;
    using Core.Extensions;
    using Snapshotting;

    public static class PersistenceExtensions
    {
        public static bool PersistJson(this DiagnosticReport report, string path)
        {
            string file = $"{path}/report_{report.Identifier.ToString()}.json";
            if (File.Exists(file))
                return false;
            
            File.WriteAllText(file, report.ToJsonString());
            return true;
        }

        public static bool PersistJson<T>(this SnapshotContext<T> context, string path)
            where T : Snapshot
        {
            string file = $"{path}/snapshot_{context.Identifier}.json";
            if (File.Exists(file))
                return false;

            File.WriteAllText(file, context.ToJsonString());
            return true;
        }
    }
}
namespace HareDu.Diagnostics.Persistence
{
    using System.IO;
    using HareDu.Extensions;
    using Serialization;

    public class DiagnosticWriter :
        IDiagnosticWriter
    {
        public bool TrySave(ScannerResult result, string file, string path)
        {
            if (Directory.Exists(path))
                return Write(result, file, path);
            
            var dir = Directory.CreateDirectory(path);

            return dir.Exists ? Write(result, file, path) : Write(result, file, path);
        }

        bool Write(ScannerResult result, string file, string path)
        {
            string fullPath = $"{path}/{file}";
            
            if (File.Exists(fullPath))
                return false;

            File.WriteAllText(fullPath, result.ToJsonString(Deserializer.Options));
            return true;
        }
    }
}
namespace HareDu.Diagnostics
{
    using System.Security.Cryptography;
    using System.Text;

    public static class DiagnosticSensorExtensions
    {
        static readonly SHA256 _sha256;

        static DiagnosticSensorExtensions()
        {
            _sha256 = SHA256.Create();
        }

        public static string ComputeHash(this string value)
        {
            byte[] data = _sha256.ComputeHash(Encoding.UTF8.GetBytes(value));
            
            StringBuilder buffer = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                buffer.Append(data[i].ToString("x2"));
            }

            return buffer.ToString();
        }
    }
}
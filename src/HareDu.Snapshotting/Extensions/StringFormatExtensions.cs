namespace HareDu.Snapshotting.Extensions
{
    using System.Globalization;

    public static class StringFormatExtensions
    {
        public static string ToByteString(this ulong bytes)
        {
            if (bytes < 1000)
                return $"{bytes}";

            if (bytes / 1000f < 1000)
                return string.Format(CultureInfo.CurrentCulture, "{0:0.000} KB", bytes / 1000f);

            if (bytes / 1000000f < 1000)
                return string.Format(CultureInfo.CurrentCulture, "{0:0.000} MB", bytes / 1000000f);
            
            if (bytes / 1000000000f < 1000)
                return string.Format(CultureInfo.CurrentCulture, "{0:0.000} GB", bytes / 1000000000f);
            
            return $"{bytes}";
        }
    }
}
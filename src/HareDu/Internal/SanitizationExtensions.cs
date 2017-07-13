namespace HareDu.Internal
{
    internal static class SanitizationExtensions
    {
        internal static string SanitizeVirtualHostName(this string value) => value == @"/" ? value.Replace("/", "%2f") : value;

        internal static string SanitizePropertiesKey(this string value) => value.Replace("%5F", "%255F");
    }
}
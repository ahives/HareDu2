namespace HareDu.Core.Extensions
{
    public static class SanitizationExtensions
    {
        public static string SanitizePropertiesKey(this string value) =>
            string.IsNullOrWhiteSpace(value) ? string.Empty : value.Replace("%5F", "%255F");

        public static string ToSanitizedName(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return value == @"/" ? value.Replace("/", "%2f") : value;
        }
    }
}
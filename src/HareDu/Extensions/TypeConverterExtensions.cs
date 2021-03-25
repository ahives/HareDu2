namespace HareDu.Extensions
{
    public static class TypeConverterExtensions
    {
        public static ulong ToLong(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return ulong.MaxValue;
            
            if (value.Equals("infinity"))
                return ulong.MaxValue;

            return ulong.TryParse(value, out ulong result) ? result : ulong.MaxValue;
        }
    }
}
namespace HareDu.Extensions
{
    public static class ValueCastingExtensions
    {
        public static T Cast<T>(this object value)
        {
            if (value is T obj)
                return obj;

            return default;
        }
    }
}
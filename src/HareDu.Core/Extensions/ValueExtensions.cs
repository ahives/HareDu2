namespace HareDu.Core.Extensions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class ValueExtensions
    {
        /// <summary>
        /// Returns true if the value is null, otherwise, returns true.
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsNull<T>(this T value) => value == null;
        
        /// <summary>
        /// Returns true if the value is not null, otherwise, returns false.
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsNotNull<T>(this T value) => !IsNull(value);

        /// <summary>
        /// Returns true if all the values in the specified list is not equal to null, empty, or whitespace, otherwise, false.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool IsEmpty(this IList<string> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(values[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns true if at least one value in the specified list is not equal to null, empty, or whitespace, otherwise, false.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool IsNotEmpty(this IList<string> values) => !IsEmpty(values);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryGetValue<T>(this ResultList<T> source, int index, out T value)
        {
            if (source.IsNull() || !source.HasData || index < 0 || index >= source.Data.Count)
            {
                value = default;
                return false;
            }

            value = source.Data[index];
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryGetValue<T>(this Task<ResultList<T>> source, int index, out T value)
        {
            if (source.IsNull() || index < 0)
            {
                value = default;
                return false;
            }
            
            ResultList<T> result = source.GetResult();

            if (result.IsNull() || result.Data.IsNull() || !result.HasData || result.HasFaulted || index >= result.Data.Count)
            {
                value = default;
                return false;
            }

            value = result.Data[index];
            return true;
        }
    }
}
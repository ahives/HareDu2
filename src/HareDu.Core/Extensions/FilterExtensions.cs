namespace HareDu.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class FilterExtensions
    {
        /// <summary>
        /// Returns a filtered list of results meeting the specified predicate.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IReadOnlyList<T> Where<T>(this ResultList<T> source, Func<T, bool> predicate)
        {
            if (source.IsNull())
                return new List<T>();
            
            return !source.HasData ? new List<T>() : Filter(source.Data, predicate);
        }

        /// <summary>
        /// Returns a filtered list of results meeting the specified predicate.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IReadOnlyList<T> Where<T>(this Task<ResultList<T>> source, Func<T, bool> predicate)
        {
            if (source.IsNull())
                return new List<T>();
            
            ResultList<T> result = source.Result;

            return !result.HasData ? new List<T>() : Filter(result.Data, predicate);
        }

        static IReadOnlyList<T> Filter<T>(IReadOnlyList<T> list, Func<T, bool> predicate)
        {
            var internalList = new List<T>();

            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                    internalList.Add(list[i]);
            }
            
            return internalList;
        }
    }
}
namespace HareDu.Core.Extensions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public static class LinqExtensions
    {
        /// <summary>
        /// Returns a single element within <see cref="ResultList{T}"/>, otherwise, return default <see cref="T"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T SingleOrDefault<T>(this Task<ResultList<T>> source)
        {
            if (source.IsNull())
                return default;
            
            var data = source.Select(x => x.Data);
            if (data.IsNull() || !data.Any())
                return default;
            
            return data.SingleOrDefault();
        }

        /// <summary>
        /// Returns a single element within <see cref="ResultList{T}"/> that matches the predicate, otherwise, return default <see cref="T"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T SingleOrDefault<T>(this Task<ResultList<T>> source, Func<T, bool> predicate)
        {
            if (source.IsNull())
                return default;
            
            var data = source.Select(x => x.Data);
            if (data.IsNull() || !data.Any())
                return default;

            try
            {
                return data.SingleOrDefault(predicate);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Returns a single element within <see cref="ResultList{T}"/>, otherwise, return default <see cref="T"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T SingleOrDefault<T>(this ResultList<T> source)
        {
            if (source.IsNull())
                return default;
            
            var data = source.Select(x => x.Data);
            if (data.IsNull() || !data.Any())
                return default;

            try
            {
                return data.SingleOrDefault();
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Returns a single element within <see cref="ResultList{T}"/> that matches the predicate, otherwise, return default <see cref="T"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T SingleOrDefault<T>(this ResultList<T> source, Func<T, bool> predicate)
        {
            if (source.IsNull())
                return default;
            
            var data = source.Select(x => x.Data);
            if (data.IsNull() || !data.Any())
                return default;
            
            try
            {
                return data.SingleOrDefault(predicate);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Returns the first element within <see cref="ResultList{T}"/>, otherwise, return default <see cref="T"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FirstOrDefault<T>(this Task<ResultList<T>> source)
        {
            if (source.IsNull())
                return default;
            
            var data = source.Select(x => x.Data);
            if (data.IsNull() || !data.Any())
                return default;
            
            return data.FirstOrDefault();
        }

        /// <summary>
        /// Returns the first element within <see cref="ResultList{T}"/> that matches the predicate, otherwise, return default <see cref="T"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FirstOrDefault<T>(this Task<ResultList<T>> source, Func<T, bool> predicate)
        {
            if (source.IsNull())
                return default;
            
            var data = source.Select(x => x.Data);
            if (data.IsNull() || !data.Any())
                return default;
            
            return data.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Returns the first element within <see cref="ResultList{T}"/>, otherwise, return default <see cref="T"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FirstOrDefault<T>(this ResultList<T> source)
        {
            if (source.IsNull())
                return default;
            
            var data = source.Select(x => x.Data);
            if (data.IsNull() || !data.Any())
                return default;
            
            return data.FirstOrDefault();
        }

        /// <summary>
        /// Returns the first element within <see cref="ResultList{T}"/> that matches the predicate, otherwise, return default <see cref="T"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FirstOrDefault<T>(this ResultList<T> source, Func<T, bool> predicate)
        {
            if (source.IsNull())
                return default;
            
            var data = source.Select(x => x.Data);
            if (data.IsNull() || !data.Any())
                return default;
            
            return data.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Returns true if there are any elements within <see cref="ResultList{T}"/>, otherwise, return false.
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool Any<T>(this Task<ResultList<T>> source)
        {
            if (source.IsNull())
                return default;
            
            var data = source.Select(x => x.Data);
            if (data.IsNull() || !data.Any())
                return default;
            
            return data.Any();
        }

        /// <summary>
        /// Returns true if any elements within <see cref="ResultList{T}"/> matches the predicate, otherwise, return false.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool Any<T>(this Task<ResultList<T>> source, Func<T, bool> predicate)
        {
            if (source.IsNull())
                return default;
            
            var data = source.Select(x => x.Data);
            if (data.IsNull() || !data.Any())
                return default;
            
            return data.Any(predicate);
        }

        /// <summary>
        /// Returns true if there are any elements within <see cref="ResultList{T}"/>, otherwise, return false.
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool Any<T>(this ResultList<T> source)
        {
            if (source.IsNull())
                return default;
            
            var data = source.Select(x => x.Data);
            
            return data.IsNull() ? default : data.Any();
        }

        /// <summary>
        /// Returns true if any elements within <see cref="ResultList{T}"/> matches the predicate, otherwise, return false.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool Any<T>(this ResultList<T> source, Func<T, bool> predicate)
        {
            if (source.IsNull())
                return default;
            
            var data = source.Select(x => x.Data);
            if (data.IsNull() || !data.Any())
                return default;
            
            return data.Any(predicate);
        }
    }
}
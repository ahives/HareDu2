namespace HareDu
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Model;

    public static class ValueExtensions
    {
        public static bool HasValue<T>(this Result<T> source) => source != null && source.Data != null;

        public static bool HasValue<T>(this Result<IEnumerable<T>> source) => source?.Data != null && source.Data.Any();

        public static IEnumerable<T> Where<T>(this Result<IEnumerable<T>> source, Func<T, bool> predicate)
            => source?.Data == null || !source.Data.Any() ? Enumerable.Empty<T>() : source.Data.Where(predicate);

        public static T Single<T>(this Task<Result<IEnumerable<T>>> source)
        {
            var data = source?.Result;
            if (data?.Data == null || !data.Data.Any())
                return default(T);

            return data.Data.Single();
        }

        public static T SingleOrDefault<T>(this Task<Result<IEnumerable<T>>> source)
        {
            var data = source?.Result;
            if (data?.Data == null || !data.Data.Any())
                return default(T);

            return data.Data.SingleOrDefault();
        }

        public static T SingleOrDefault<T>(this Task<Result<IEnumerable<T>>> source, Func<T, bool> predicate)
        {
            var data = source?.Result;
            if (data?.Data == null || !data.Data.Any())
                return default(T);

            return data.Data.SingleOrDefault(predicate);
        }

        public static T FirstOrDefault<T>(this Task<Result<IEnumerable<T>>> source)
        {
            var data = source?.Result;
            if (data?.Data == null || !data.Data.Any())
                return default(T);

            return data.Data.FirstOrDefault();
        }

        public static T FirstOrDefault<T>(this Task<Result<IEnumerable<T>>> source, Func<T, bool> predicate)
        {
            var data = source?.Result;
            if (data?.Data == null || !data.Data.Any())
                return default(T);

            return data.Data.FirstOrDefault(predicate);
        }
        
        public static bool Any<T>(this Task<Result<IEnumerable<T>>> source)
        {
            var data = source?.Result;
            if (data?.Data == null || !data.Data.Any())
                return false;

            return data.Data.Any();
        }

        public static bool Any<T>(this Task<Result<IEnumerable<T>>> source, Func<T, bool> predicate)
        {
            var data = source?.Result;
            if (data?.Data == null || !data.Data.Any())
                return false;

            return data.Data.Any(predicate);
        }
        
        public static IEnumerable<T> Where<T>(this Task<Result<IEnumerable<T>>> source, Func<T, bool> predicate)
        {
            var data = source?.Result;
            if (data?.Data == null || !data.Data.Any())
                return Enumerable.Empty<T>();
            
            return source.Result.Data.Where(predicate);
        }
    }
}
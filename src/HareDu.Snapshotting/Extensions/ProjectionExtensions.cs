namespace HareDu.Snapshotting.Extensions
{
    using System;
    using Core.Extensions;

    public static class ProjectionExtensions
    {
        /// <summary>
        /// Safely attempts to unwrap the specified object and returns the resultant value. If the object is NULL, then the default object value will be returned.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="projection"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <returns></returns>
        public static T Select<T, U>(this U obj, Func<U, T> projection)
            => obj.IsNull()
                ? default
                : projection(obj);
    }
}
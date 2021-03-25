namespace HareDu.Core.Extensions
{
    using System.Threading;
    using System.Threading.Tasks;

    public static class TaskExtensions
    {
        /// <summary>
        /// Unwrap <see cref="Task{T}"/> and return T.
        /// </summary>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetResult<T>(this Task<T> result)
            => !result.IsNull() && !result.IsCanceled && !result.IsFaulted
                ? result.GetAwaiter().GetResult()
                : default;
        
        public static void RequestCanceled(this CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
                return;

            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
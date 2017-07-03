namespace HareDu.Internal
{
    using System;
    using System.Threading;

    internal static class AsyncTaskExtensions
    {
        internal static void RequestCanceled(this CancellationToken cancellationToken, Action<string> logging)
        {
            if (!cancellationToken.IsCancellationRequested)
                return;
            
            logging?.Invoke(
                "Cancellation of this task was requested by the caller, therefore, request for resources has been canceled.");

            cancellationToken.ThrowIfCancellationRequested();
        }

    }
}
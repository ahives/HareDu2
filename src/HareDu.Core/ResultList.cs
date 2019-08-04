namespace HareDu.Core
{
    using System.Collections.Generic;

    public interface ResultList<out TResult> :
        Result
    {
        IReadOnlyList<TResult> Data { get; }
        bool HasData { get; }
    }
}
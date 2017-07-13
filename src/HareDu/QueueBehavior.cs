namespace HareDu
{
    using System.Collections.Generic;

    public interface QueueBehavior
    {
        void IsDurable();

        void OnNode(string node);

        void WithArguments(IDictionary<string, object> args);

        void AutoDeleteWhenNotInUse();
    }
}
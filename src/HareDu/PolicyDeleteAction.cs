namespace HareDu
{
    using System;

    public interface PolicyDeleteAction
    {
        /// <summary>
        /// Specify the name of the policy.
        /// </summary>
        /// <param name="name"></param>
        void Policy(string name);

        /// <summary>
        /// Specify what virtual host will the policy be deleted.
        /// </summary>
        /// <param name="target">Define where the policy will be delete from</param>
        void Targeting(Action<PolicyTarget> target);
    }
}
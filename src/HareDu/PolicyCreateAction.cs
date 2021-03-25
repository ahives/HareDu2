namespace HareDu
{
    using System;

    public interface PolicyCreateAction
    {
        /// <summary>
        /// Specify the name of the policy.
        /// </summary>
        /// <param name="name"></param>
        void Policy(string name);
        
        /// <summary>
        /// Specify how the policy should be configured.
        /// </summary>
        /// <param name="configuration">User-defined configuration</param>
        void Configure(Action<PolicyConfiguration> configuration);

        /// <summary>
        /// Specify what virtual host will the policy be scoped to.
        /// </summary>
        /// <param name="target">Define where the policy will live</param>
        void Targeting(Action<PolicyTarget> target);
    }
}
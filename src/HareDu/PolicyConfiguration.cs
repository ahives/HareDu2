namespace HareDu
{
    using System;

    public interface PolicyConfiguration
    {
        /// <summary>
        /// Specify the pattern which the policy will be applied.
        /// </summary>
        /// <param name="pattern"></param>
        void UsingPattern(string pattern);
        
        /// <summary>
        /// Specify arguments for the policy.
        /// </summary>
        /// <param name="arguments">Pre-defined arguments applied to the definition of the policy.</param>
        void HasArguments(Action<PolicyDefinitionArguments> arguments);
        
        /// <summary>
        /// Specify the priority for which the policy will be executed.
        /// </summary>
        /// <param name="priority"></param>
        void HasPriority(int priority);
        
        /// <summary>
        /// Specify how the policy is applied.
        /// </summary>
        /// <param name="applyTo"></param>
        void ApplyTo(string applyTo);
    }
}
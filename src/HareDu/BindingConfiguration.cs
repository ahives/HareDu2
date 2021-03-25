namespace HareDu
{
    using System;

    public interface BindingConfiguration
    {
        /// <summary>
        /// Specify how the binding will be set up rout messages.
        /// </summary>
        /// <param name="routingKey"></param>
        void HasRoutingKey(string routingKey);
        
        /// <summary>
        /// Specify user-defined binding arguments.
        /// </summary>
        /// <param name="arguments"></param>
        void HasArguments(Action<BindingArguments> arguments);
    }
}
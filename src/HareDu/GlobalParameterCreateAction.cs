namespace HareDu
{
    using System;

    public interface GlobalParameterCreateAction
    {
        /// <summary>
        /// Specify the name of the global parameter.
        /// </summary>
        /// <param name="name"></param>
        void Parameter(string name);
        
        /// <summary>
        /// Specify global parameter arguments.
        /// </summary>
        /// <param name="arguments"></param>
        void Value(Action<GlobalParameterArguments> arguments);
        
        /// <summary>
        /// Specify global parameter argument.
        /// </summary>
        /// <param name="arguments"></param>
        void Value<T>(T argument);
    }
}
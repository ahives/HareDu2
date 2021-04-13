namespace HareDu
{
    public interface GlobalParameterArgumentConfigurator
    {
        /// <summary>
        /// Create a new argument.
        /// </summary>
        /// <param name="arg">Name of the argument.</param>
        /// <param name="value">Value of the argument.</param>
        /// <typeparam name="T"></typeparam>
        void Add<T>(string arg, T value);
    }
}
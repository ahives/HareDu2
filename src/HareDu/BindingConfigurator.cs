namespace HareDu
{
    public interface BindingConfigurator
    {
        /// <summary>
        /// Set a user-defined argument.
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        void Add<T>(string arg, T value);
    }
}
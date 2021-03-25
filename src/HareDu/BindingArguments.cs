namespace HareDu
{
    public interface BindingArguments
    {
        /// <summary>
        /// Set a user-defined argument.
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        void Set<T>(string arg, T value);
    }
}
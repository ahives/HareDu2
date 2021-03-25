namespace HareDu
{
    public interface ScopedParameterTarget
    {
        /// <summary>
        /// Specify the name of the component.
        /// </summary>
        /// <param name="component"></param>
        void Component(string component);

        /// <summary>
        /// Specify the name of the virtual host.
        /// </summary>
        /// <param name="name">Name of the virtual host being targeted.</param>
        void VirtualHost(string name);
    }
}
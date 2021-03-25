namespace HareDu
{
    public interface BindingTarget
    {
        /// <summary>
        /// Specify where the binding will bind source (e.g. exchange, queue) and destination (e.g. exchange, queue).
        /// </summary>
        /// <param name="name">Name of the virtual host being targeted.</param>
        void VirtualHost(string name);
    }
}